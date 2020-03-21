using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dynamic_menu.Controls
{
    public class Button : IControl
    {
        private const char HorizontalLineSymbol = '━';
        private const char VerticalLineSymbol = '┃';
        private const char LeftUpCornerSymbol = '┏';
        private const char RightUpCornerSymbol = '┓';
        private const char LeftDownCornerSymbol = '┗';
        private const char RightDownCornerSymbol = '┛';
        public const string SelectionSymbol = "►";

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

        public void Activate()
        {
            Execute();
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
            
            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing);
            Console.WriteLine(LeftUpCornerSymbol + 
                              new string(HorizontalLineSymbol, Caption.Length + CtrlPosition.LeftPadding * 2) + 
                              RightUpCornerSymbol);
            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing + 1);
            Console.WriteLine(VerticalLineSymbol + 
                              new string(' ', CtrlPosition.LeftPadding - GetMarker().Length - 1) + 
                              $"{GetMarker()} {Caption}" + 
                              new string(' ', CtrlPosition.LeftPadding) + VerticalLineSymbol);
            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing + 2);
            Console.WriteLine(LeftDownCornerSymbol + 
                              new string(HorizontalLineSymbol, Caption.Length + CtrlPosition.LeftPadding * 2) + 
                              RightDownCornerSymbol);
        }

        private string GetMarker()
        {
            return Selected ? SelectionSymbol : new string(' ', SelectionSymbol.Length);
        }

        public void ProcessKeyPress() { }
    }
}
