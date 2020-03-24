using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dynamic_menu.Controls
{
    public class TextInput : IControl
    {
        public const string SelectionSymbol = "►";
        public const char InputCaret = '▌';
        private const int ControlLoopSleep = 100;

        public IControl Parent { get; set; }
        public Position CtrlPosition { get; set; }
        public Size CtrlSize { get; set; }
        public bool Selected { get; set; }
        public bool Active { get; set; }
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
            Caption = string.Empty;
            Text = string.Empty;
            MaxLength = 0;
        }

        public void Draw()
        {
            Console.ForegroundColor = Active ? ConsoleColor.White : ConsoleColor.Gray;

            int parentLeft = Parent?.CtrlPosition.LeftSpacing ?? 0;
            int parentTop = Parent?.CtrlPosition.TopSpacing ?? 0;

            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing);
            Console.Write($"{GetMarker()} {Caption} {Text}");
            Console.Write(Active ? InputCaret.ToString() + " " : " ");

            UpdateControlArea();
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
                Thread.Sleep(ControlLoopSleep);
            } while (Active);
        }

        public async Task ActivateAsync()
        {
            await Task.Run(() =>
            {
                Activate();
            });
        }

        private void Deactivate()
        {
            Active = false;
            Console.ResetColor();            
        }

        private string GetMarker()
        {
            return Selected ? SelectionSymbol : new string(' ', SelectionSymbol.Length);
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
