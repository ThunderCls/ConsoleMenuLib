using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.Controls
{
    public class CheckBox : IControl
    {
        private const string CheckBoxChecked = "[√]";
        private const string CheckBoxUnChecked = "[ ]";

        public IControl Parent { get; set; }
        public Position CtrlPosition { get; set; }
        public Size CtrlSize { get; set; }
        public bool Selected { get; set; }
        public bool Active { get; set; }
        public int TabIndex { get; set; }
        public string Caption { get; set; }

        private bool _checked;
        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                if (Parent != null)
                {
                    //Draw();
                    StateChange();
                }
            }
        }

        public event EventHandler OnStateChanged;

        protected virtual void StateChange()
        {
            OnStateChanged?.Invoke(this, null);
        }

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

        private string GetMarker()
        {
            return Selected ? Common.SelectionSymbol : new string(' ', Common.SelectionSymbol.Length);
        }

        public void ProcessKeyPress() { }
    }
}
