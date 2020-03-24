using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dynamic_menu.Controls
{
    public class Panel : IControl
    {
        private const char HorizontalLineSymbol = '━';
        private const char VerticalLineSymbol = '┃';
        private const char LeftUpCornerSymbol = '┏';
        private const char RightUpCornerSymbol = '┓';
        private const char LeftDownCornerSymbol = '┗';
        private const char RightDownCornerSymbol = '┛';
        private const int ControlLoopSleep = 100;

        private List<IControl> Controls { get; set; }
        public int ControlIndex { get; set; }
        public IControl Parent { get; set; }
        public Position CtrlPosition { get; set; }
        public Size CtrlSize { get; set; }
        public bool Selected { get; set; }
        public bool Active { get; set; }
        public int TabIndex { get; set; }

        public Panel()
        {
            Controls = new List<IControl>();
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
            Controls.ForEach(x => x.Deselect());
            Controls[ControlIndex].Select();
        }

        private void MoveMarkerDown()
        {
            // TODO: implement with TabIndex
            ControlIndex = (ControlIndex + 1 < Controls.Count) ? ControlIndex += 1 : 0;
            Controls.ForEach(x => x.Deselect());
            Controls[ControlIndex].Select();
        }

        public void Draw()
        {
            // draw panel square
            Console.SetCursorPosition(CtrlPosition.LeftSpacing, CtrlPosition.TopSpacing);
            Console.WriteLine(LeftUpCornerSymbol + new string(HorizontalLineSymbol, CtrlSize.Width) + RightUpCornerSymbol);

            for (int line = 0; line < CtrlSize.Height - 2; line++)
            {
                Console.SetCursorPosition(CtrlPosition.LeftSpacing, line + CtrlPosition.TopSpacing + 1);
                Console.Write(VerticalLineSymbol);
                Console.SetCursorPosition(CtrlPosition.LeftSpacing + CtrlSize.Width + 1, line + CtrlPosition.TopSpacing + 1);
                Console.Write(VerticalLineSymbol);
            }

            Console.SetCursorPosition(CtrlPosition.LeftSpacing, CtrlPosition.TopSpacing + CtrlSize.Height - 1);
            Console.WriteLine(LeftDownCornerSymbol + new string(HorizontalLineSymbol, CtrlSize.Width) + RightDownCornerSymbol);

            // draw children controls
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
