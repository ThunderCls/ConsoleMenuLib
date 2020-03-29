using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleUI.Controls
{
    public class TextInput : IControl
    {
        private const char HorizontalLineSymbol = '─';
        private const char VerticalLineSymbol = '│';
        private const char LeftUpCornerSymbol = '┌';
        private const char RightUpCornerSymbol = '┐';
        private const char LeftDownCornerSymbol = '└';
        private const char RightDownCornerSymbol = '┘';

        private const char InputCaret = '▌';

        public IControl Parent { get; set; }
        public Position CtrlPosition { get; set; }
        public Size CtrlSize { get; set; }
        public bool Selected { get; set; }
        public bool Active { get; set; }
        public bool MaskChars { get; set; }
        public char Mask { get; set; }
        public bool Border { get; set; }
        public int TabIndex { get; set; }

        public string Caption { get; set; }
        public string Text { get; set; }
        public int MaxLength { get; set; } // TODO: filter negative values
        //public int MaxLength {
        //    get
        //    {
        //        return MaxLength;
        //    }
        //    set
        //    {
        //        if (value < 0)
        //            MaxLength = 0;
        //        else
        //            MaxLength = value;
        //    }
        //}

        public TextInput()
        {
            CtrlPosition = new Position();
            CtrlSize = new Size();
            Caption = "TextInput";
            Text = string.Empty;
            MaxLength = 0;
            Mask = '*';
            Border = true;
        }

        public void Draw()
        {
            Console.ForegroundColor = Active ? ConsoleColor.White : ConsoleColor.Gray;

            int parentLeft = Parent?.CtrlPosition.LeftSpacing ?? 0;
            int parentTop = Parent?.CtrlPosition.TopSpacing ?? 0;

            if (Border)
            {
                DrawWithBorders(parentLeft, parentTop);
            }
            else
            {
                DrawNoBorders(parentLeft, parentTop);
            }
        }

        private void DrawNoBorders(int parentLeft, int parentTop)
        {
            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing);
            Console.Write($"{GetMarker()} {Caption} ");
            string ctrlText = MaskChars ? new string(Mask, Text.Length) : Text;
            Console.Write($"{ctrlText}");
            Console.Write(Active ? InputCaret.ToString() + " " : " ");

            UpdateControlArea();
        }

        private void DrawWithBorders(int parentLeft, int parentTop)
        {
            // draw initial upper line of box
            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing + GetMarker().Length + Caption.Length + 2,
                                      parentTop + CtrlPosition.TopSpacing - 1);
            Console.Write(LeftUpCornerSymbol +
                              new string(HorizontalLineSymbol, CtrlSize.Width == 0 ? 0 : CtrlSize.Width - 2) +
                              RightUpCornerSymbol);
            // draw box label
            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing);
            Console.Write($"{GetMarker()} {Caption} ");

            // start drawing the textinput box
            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing + GetMarker().Length + Caption.Length + 2,
                                      parentTop + CtrlPosition.TopSpacing);
            Console.Write(VerticalLineSymbol);
            // draw box content
            string ctrlText = MaskChars ? new string(Mask, Text.Length) : Text;
            Console.Write($"{ctrlText}");
            Console.Write(Active ? InputCaret.ToString() : "");
            Console.Write(new string(' ', CtrlSize.Width - ctrlText.Length - (Active ? 3 : 2)) + VerticalLineSymbol);

            // draw lower line of box
            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing + GetMarker().Length + Caption.Length + 2,
                                      parentTop + CtrlPosition.TopSpacing + 1);
            Console.Write(LeftDownCornerSymbol +
                              new string(HorizontalLineSymbol, CtrlSize.Width == 0 ? 0 : CtrlSize.Width - 2) +
                              RightDownCornerSymbol);
        }

        private void UpdateControlArea()
        {
            int padding = MaxLength - Text.Length;
            Console.Write(new string(' ', padding));
        }

        private void CalculateMaxLength()
        {
            // if not defined set automatic max length respect to parent
            int ctrlText = GetMarker().Length + Caption.Length + 3;
            int maxInput = (Parent?.CtrlSize.Width ?? Console.WindowWidth) -
                           CtrlPosition.LeftSpacing - CtrlPosition.LeftPadding - ctrlText;
            MaxLength = MaxLength > 0 ? MaxLength : maxInput;
        }

        public void Activate()
        {
            CalculateMaxLength();
            Active = true;

            do
            {
                Draw();
                ProcessKeyPress();
            } while (Active);
        }

        public async Task ActivateAsync()
        {
            await Task.Run(Activate);
        }

        private void Deactivate()
        {
            Active = false;
            Console.ResetColor();            
        }

        private string GetMarker()
        {
            return Selected ? Common.SelectionSymbol : new string(' ', Common.SelectionSymbol.Length);
        }

        public void ProcessKeyPress()
        {
            if (!Active)
                return;

            int lastChar = 0;
            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    Deactivate();
                    break;

                case ConsoleKey.Backspace:
                    lastChar = Text.Length - 1;
                    if (lastChar >= 0)
                    {
                        Text = Text.Substring(0, lastChar);
                    }                    
                    break;

                default:
                    if(char.IsLetterOrDigit(key.KeyChar) || 
                       char.IsWhiteSpace(key.KeyChar) || 
                       char.IsPunctuation(key.KeyChar))
                    {
                        if(MaxLength == 0 || Text.Length + 1 <= MaxLength)
                        {
                            Text += key.KeyChar;
                        }                        
                    }
                    else
                    {
                        key = default(ConsoleKeyInfo);
                    }
                    break;
            }
        }

        public void Select()
        {
            Selected = true;
        }

        public void Deselect()
        {
            Selected = false;
        }
    }
}
