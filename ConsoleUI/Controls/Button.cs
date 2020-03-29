using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.Controls
{
    public class Button : IControl
    {
        private const char HorizontalLineSymbol = '━';
        private const char VerticalLineSymbol = '┃';
        private const char LeftUpCornerSymbol = '┏';
        private const char RightUpCornerSymbol = '┓';
        private const char LeftDownCornerSymbol = '┗';
        private const char RightDownCornerSymbol = '┛';        

        public string Caption { get; set; }
        public IControl Parent { get; set; }
        public Position CtrlPosition { get; set; }
        public Size CtrlSize { get; set; }
        public bool Selected { get; set; }
        public bool Active { get; set; }
        public int TabIndex { get; set; }
        public event EventHandler OnExecute;

        protected virtual void Execute()
        {
            OnExecute?.Invoke(this, null);
        }

        public Button()
        {
            CtrlPosition = new Position();
            CtrlSize = new Size();
            Caption = "Button";
        }

        public void Activate()
        {
            Execute();
        }

        public async Task ActivateAsync()
        {
            await Task.Run(Activate);
        }

        public bool CoordinateInsideClientArea(int x, int y)
        {
            return (x >= CtrlPosition.LeftSpacing &&
                    x <= CtrlPosition.LeftSpacing + CtrlSize.Width - 1 &&
                    y >= CtrlPosition.TopSpacing &&
                    y <= CtrlPosition.TopSpacing + CtrlSize.Height - 1);
        }

        public void Select()
        {
            Selected = true;
        }

        public void Deselect()
        {
            Selected = false;
        }

        public void Draw()
        {
            int parentLeft = Parent?.CtrlPosition.LeftSpacing ?? 0;
            int parentTop = Parent?.CtrlPosition.TopSpacing ?? 0;
            
            //if(!AutoSize)
            //{
                DrawUserDefinedSize(parentLeft, parentTop);
            //}
            //else
            //{
            //    DrawAutoSize(parentLeft, parentTop);
            //}
        }

        //private void DrawAutoSize(int parentLeft, int parentTop)
        //{
        //    Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing);
        //    Console.WriteLine(LeftUpCornerSymbol +
        //                      new string(HorizontalLineSymbol, Caption.Length + CtrlPosition.LeftPadding * 2) +
        //                      RightUpCornerSymbol);
        //    Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing + 1);
        //    Console.WriteLine(VerticalLineSymbol +
        //                      new string(' ', CtrlPosition.LeftPadding - GetMarker().Length - 1) +
        //                      $"{GetMarker()} {Caption}" +
        //                      new string(' ', CtrlPosition.LeftPadding) + VerticalLineSymbol);
        //    Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing + 2);
        //    Console.WriteLine(LeftDownCornerSymbol +
        //                      new string(HorizontalLineSymbol, Caption.Length + CtrlPosition.LeftPadding * 2) +
        //                      RightDownCornerSymbol);
        //}

        private void DrawUserDefinedSize(int parentLeft, int parentTop)
        {
            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing);
            Console.WriteLine(LeftUpCornerSymbol +
                              new string(HorizontalLineSymbol, CtrlSize.Width == 0 ? 0 : CtrlSize.Width - 2) +
                              RightUpCornerSymbol);

            int textLine = (CtrlSize.Height / 2);
            int caret = 1;
            for (; caret < CtrlSize.Height - 1; caret++)
            {
                Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing + caret);
                Console.Write(VerticalLineSymbol);

                if (caret != textLine)
                {
                    Console.Write(new string(' ', CtrlSize.Width == 0 ? 0 : CtrlSize.Width - 2));
                }
                else
                {
                    int ctrlTextSpace = (CtrlPosition.LeftPadding - GetMarker().Length - 2) + GetMarker().Length + Caption.Length + 2;
                    int padding = (CtrlSize.Width == 0 ? 0 : CtrlSize.Width - 2) - ctrlTextSpace;
                    Console.Write(new string(' ', CtrlPosition.LeftPadding - GetMarker().Length - 1) +
                                  $"{GetMarker()} {Caption}" +
                                  new string(' ', padding < 0 ? 0 : padding));
                }

                Console.WriteLine(VerticalLineSymbol);
            }

            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing + caret);
            Console.WriteLine(LeftDownCornerSymbol +
                              new string(HorizontalLineSymbol, CtrlSize.Width == 0 ? 0 : CtrlSize.Width - 2) +
                              RightDownCornerSymbol);
        }

        private string GetMarker()
        {
            return Selected ? Common.SelectionSymbol : new string(' ', Common.SelectionSymbol.Length);
        }

        public void ProcessKeyPress() { }
    }
}
