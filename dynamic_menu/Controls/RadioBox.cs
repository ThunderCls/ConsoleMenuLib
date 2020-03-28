using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.Controls
{
    public class RadioBox : IControl
    {
        private const string RadioBoxChecked = "●";
        private const string RadioBoxUnChecked = "⃝";

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

        public RadioBox()
        {
            CtrlPosition = new Position();
            CtrlSize = new Size();
            Caption = "RadioBox";
        }

        public void Draw()
        {
            int parentLeft = Parent?.CtrlPosition.LeftSpacing ?? 0;
            int parentTop = Parent?.CtrlPosition.TopSpacing ?? 0;

            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing);
            string checkBoxSymbol = Checked ? RadioBoxChecked : RadioBoxUnChecked;
            Console.WriteLine($"{GetMarker()} {checkBoxSymbol} {Caption}");
        }

        public void Activate()
        {
            // uncheck every other radio
            ((Dialog)Parent).Controls.Where(x => x.GetType() == typeof(RadioBox))
                                     .Select(x =>
                                     {
                                         ((RadioBox)x)._checked = false;
                                         return x;
                                     }).ToList();

            // check the current radio
            Checked = true;
        }

        public async Task ActivateAsync()
        {
            await Task.Run(Activate);
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
