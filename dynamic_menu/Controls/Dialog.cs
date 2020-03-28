using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleUI.Controls
{
    public class Dialog : IControl
    {
        private const char HorizontalLineSymbol = '━';
        private const char VerticalLineSymbol = '┃';
        private const char LeftUpCornerSymbol = '┏';
        private const char RightUpCornerSymbol = '┓';
        private const char LeftDownCornerSymbol = '┗';
        private const char RightDownCornerSymbol = '┛';        

        public List<IControl> Controls { get; set; }
        private int ControlIndex { get; set; }
        public IControl Parent { get; set; }
        public Position CtrlPosition { get; set; }
        public Size CtrlSize { get; set; }
        public bool Selected { get; set; }
        public bool Active { get; set; }
        public string Caption { get; set; }
        public bool CenteredCaption { get; set; }
        public bool Maximized { get; set; } // TODO: implement with Console.WindowWidth in Draw
        public int TabIndex { get; set; }

        public Dialog()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.CursorVisible = false;

            Controls = new List<IControl>();
            CtrlPosition = new Position();
            CtrlSize = new Size();
            Caption = " Dialog ";
        }

        public void AddControl(IControl ctrl)
        {
            ctrl.Parent = this;
            Controls.Add(ctrl);
        }

        public void ProcessKeyPress()
        {
            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    MoveMarkerUp();
                    break;

                case ConsoleKey.DownArrow:
                    MoveMarkerDown();
                    break;

                case ConsoleKey.Enter:
                    ActivateEntry();                    
                    break;
            }
        }

        private void ActivateEntry()
        {
            if (ControlIndex >= 0 && ControlIndex < Controls.Count)
            {
                IControl ctrl = Controls[ControlIndex];
                if (!ctrl.Active)
                {
                    ctrl.Activate();
                }
                else
                {
                    ctrl.Active = false;
                }
            }
        }

        private void MoveMarkerUp()
        {
            // TODO: implement with TabIndex
            ControlIndex = (ControlIndex - 1 >= 0) ? ControlIndex -= 1 : Controls.Count - 1;
            IControl ctrl = Controls[ControlIndex];

            int ctrlIndex = 0;
            // filter control selection
            while (ctrl.GetType() == typeof(TextLabel) ||
                   ctrl.GetType() == typeof(ProgressBar) &&
                   ctrlIndex < Controls.Count)
            {
                ControlIndex = (ControlIndex - 1 >= 0) ? ControlIndex -= 1 : Controls.Count - 1;
                ctrl = Controls[ControlIndex];
                ctrlIndex++;
            }

            Controls.ForEach(x => x.Deselect());
            Controls[ControlIndex].Select();
        }

        private void MoveMarkerDown()
        {
            // TODO: implement with TabIndex
            ControlIndex = (ControlIndex + 1 < Controls.Count) ? ControlIndex += 1 : 0;
            IControl ctrl = Controls[ControlIndex];

            int ctrlIndex = 0;
            // filter control selection
            while (ctrl.GetType() == typeof(TextLabel) ||
                   ctrl.GetType() == typeof(ProgressBar) &&
                   ctrlIndex < Controls.Count)
            {
                ControlIndex = (ControlIndex + 1 < Controls.Count) ? ControlIndex += 1 : 0;
                ctrl = Controls[ControlIndex];
                ctrlIndex++;
            }

            Controls.ForEach(x => x.Deselect());
            Controls[ControlIndex].Select();
        }

        public void Draw()
        {
            // draw the upper line with dialog caption
            Console.SetCursorPosition(CtrlPosition.LeftSpacing, CtrlPosition.TopSpacing);

            int dlgLeftTitleLine = 1;
            if (CenteredCaption)
            {
                dlgLeftTitleLine = (CtrlSize.Width / 2) - (Caption.Length / 2);
            }
            Console.Write(LeftUpCornerSymbol + new string(HorizontalLineSymbol, dlgLeftTitleLine) + Caption);
            Console.WriteLine(new string(HorizontalLineSymbol, CtrlSize.Width - (Caption.Length + dlgLeftTitleLine + 1)) + RightUpCornerSymbol);

            // draw the dialog body lines
            for (int line = 0; line < CtrlSize.Height - 2; line++)
            {
                Console.SetCursorPosition(CtrlPosition.LeftSpacing, line + CtrlPosition.TopSpacing + 1);
                Console.Write(VerticalLineSymbol);
                Console.SetCursorPosition(CtrlPosition.LeftSpacing + CtrlSize.Width, line + CtrlPosition.TopSpacing + 1);
                Console.Write(VerticalLineSymbol);
            }

            // draw the lower line
            Console.SetCursorPosition(CtrlPosition.LeftSpacing, CtrlPosition.TopSpacing + CtrlSize.Height - 1);
            Console.WriteLine(LeftDownCornerSymbol + new string(HorizontalLineSymbol, CtrlSize.Width - 1) + RightDownCornerSymbol);

            // draw every children control
            foreach (var ctrl in Controls)
            {
                ctrl.Draw();
            }
        }

        public void Activate()
        {
            Active = true;

            do
            {
                Draw();
                ProcessKeyPress();
                Thread.Sleep(Common.ControlLoopSleep);
            } while (Active);
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
    }
}
