using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.Controls
{
    public class CheckBox : IControl
    {
        private const string CheckBoxChecked = "[x]";
        private const string CheckBoxUnChecked = "[ ]";

        public IControl Parent { get; set; }
        public Position CtrlPosition { get; set; }
        public Size CtrlSize { get; set; }
        public bool Selected { get; set; }
        public bool Active { get; set; }
        public int TabIndex { get; set; }
        public string Caption { get; set; }
        public bool Checked { get; set; }

        public CheckBox()
        {
            CtrlPosition = new Position();
            CtrlSize = new Size();
            Caption = "CheckBox";
        }

        public void Activate()
        {
            Checked = !Checked;
        }

        public void Draw()
        {
            int parentLeft = Parent?.CtrlPosition.LeftSpacing ?? 0;
            int parentTop = Parent?.CtrlPosition.TopSpacing ?? 0;

            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing);
            string checkBoxSymbol = Checked ? CheckBoxChecked : CheckBoxUnChecked;
            Console.WriteLine($"{GetMarker()} {checkBoxSymbol} {Caption}");
        }

        public async Task ActivateAsync()
        {
            await Task.Run(() =>
            {
                Activate();
            });
        }

        public void Select()
        {
            Selected = true;
        }

        public void Deselect()
        {
            Selected = false;
        }

        private string GetMarker()
        {
            return Selected ? Common.SelectionSymbol : new string(' ', Common.SelectionSymbol.Length);
        }

        public void ProcessKeyPress() { }
    }
}
