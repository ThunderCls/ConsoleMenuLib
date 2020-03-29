using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleUI.Controls
{
    public class GroupBox : IControl, IContainer
    {
        private const char HorizontalLineSymbol = '─';
        private const char VerticalLineSymbol = '│';
        private const char LeftUpCornerSymbol = '┌';
        private const char RightUpCornerSymbol = '┐';
        private const char LeftDownCornerSymbol = '└';
        private const char RightDownCornerSymbol = '┘';

        public IControl Parent { get; set; }
        public List<IControl> Controls { get; set; }
        public int ControlIndex { get; set; }
        public SelectionFlow selectionFlow { get; set; }
        public Position CtrlPosition { get; set; }
        public Size CtrlSize { get; set; }
        public bool Selected { get; set; }
        public bool Active { get; set; }
        public int TabIndex { get; set; }
        public string Caption { get; set; }

        public GroupBox()
        {
            Controls = new List<IControl>();
            CtrlPosition = new Position();
            CtrlSize = new Size();
            Caption = " GroupBox ";
        }

        public void Draw()
        {
            int parentLeft = Parent?.CtrlPosition.LeftSpacing ?? 0;
            int parentTop = Parent?.CtrlPosition.TopSpacing ?? 0;

            // draw the upper line with dialog caption
            Console.SetCursorPosition(CtrlPosition.LeftSpacing + parentLeft, CtrlPosition.TopSpacing + parentTop);

            Console.Write(LeftUpCornerSymbol + new string(HorizontalLineSymbol, 1) + Caption);
            Console.WriteLine(new string(HorizontalLineSymbol, (CtrlSize.Width - Caption.Length) - 2) + RightUpCornerSymbol);

            // draw the dialog body lines
            for (int line = 0; line < CtrlSize.Height - 2; line++)
            {
                Console.SetCursorPosition(CtrlPosition.LeftSpacing + parentLeft, line + CtrlPosition.TopSpacing + 1 + parentTop);
                Console.Write(VerticalLineSymbol);
                Console.SetCursorPosition(CtrlPosition.LeftSpacing + CtrlSize.Width + parentLeft, line + CtrlPosition.TopSpacing + 1 + parentTop);
                Console.Write(VerticalLineSymbol);
            }

            // draw the lower line
            Console.SetCursorPosition(CtrlPosition.LeftSpacing + parentLeft, CtrlPosition.TopSpacing + CtrlSize.Height - 1 + parentTop);
            Console.WriteLine(LeftDownCornerSymbol + new string(HorizontalLineSymbol, CtrlSize.Width - 1) + RightDownCornerSymbol);

            // draw every children control
            foreach (var ctrl in Controls)
            {
                ctrl.Draw();
            }
        }

        private string GetMarker()
        {
            return Selected ? Common.SelectionSymbol : new string(' ', Common.SelectionSymbol.Length);
        }

        public void Activate()
        {
            Active = true;

            do
            {
                Select(); // show child selection
                Draw();
                ProcessKeyPress();
            } while (Active);
        }

        public async Task ActivateAsync()
        {
            await Task.Run(Activate);
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

        public bool CoordinateInsideClientArea(int x, int y)
        {
            return (x >= CtrlPosition.LeftSpacing &&
                    x <= CtrlPosition.LeftSpacing + CtrlSize.Width - 1 &&
                    y >= CtrlPosition.TopSpacing &&
                    y <= CtrlPosition.TopSpacing + CtrlSize.Height - 1);
        }

        public void Select()
        {
            var ctrl = Controls[ControlIndex];
            Controls[ControlIndex].Select();
        }

        public void Deselect()
        {
            Controls.ForEach(x => x.Deselect());
        }

        public void AddControl(IControl ctrl)
        {
            ctrl.Parent = this;
            Controls.Add(ctrl);
        }

        public void MoveMarkerUp()
        {
            selectionFlow = SelectionFlow.Upwards;

            if (ControlIndex - 1 >= 0)
            {
                if (Active)
                {
                    ControlIndex--;
                }
                
                var ctrl = Controls[ControlIndex];
                Active = true;

                int ctrlIndex = 0;
                // filter control selection
                while (ctrl.GetType() == typeof(TextLabel) ||
                       ctrl.GetType() == typeof(ProgressBar) ||
                       ctrl.GetType().GetInterfaces().Any(x => x == typeof(IContainer)) &&
                       ctrlIndex < Controls.Count)
                {
                    if (ctrl.GetType().GetInterfaces().Any(x => x == typeof(IContainer)) &&
                        ((IContainer)ctrl).Controls.Count > 0)
                    {
                        Controls.ForEach(x => x.Deselect());
                        Draw(); // redraw for deselection
                        ctrl.Activate();
                        if (selectionFlow != SelectionFlow.Upwards)
                        {
                            MoveMarkerDown();
                            return;
                        }
                    }

                    ControlIndex = (ControlIndex - 1 >= 0) ? ControlIndex -= 1 : 0;
                    ctrl = Controls[ControlIndex];
                    ctrlIndex++;
                }

                Deselect();
                Select();
            }
            else
            {
                Active = false;
                ((IContainer)Parent).selectionFlow = SelectionFlow.Upwards;
                Deselect();
            }
        }

        public void MoveMarkerDown()
        {
            selectionFlow = SelectionFlow.Downwards;
            if (ControlIndex + 1 < Controls.Count)
            {
                if (Active)
                {
                    ControlIndex++;
                }
                
                var ctrl = Controls[ControlIndex];
                Active = true;

                int ctrlIndex = 0;
                // filter control selection
                while (ctrl.GetType() == typeof(TextLabel) ||
                       ctrl.GetType() == typeof(ProgressBar) ||
                       ctrl.GetType().GetInterfaces().Any(x => x == typeof(IContainer)) &&
                       ctrlIndex < Controls.Count)
                {
                    if (ctrl.GetType().GetInterfaces().Any(x => x == typeof(IContainer)) &&
                        ((IContainer)ctrl).Controls.Count > 0)
                    {
                        Controls.ForEach(x => x.Deselect());
                        Draw(); // redraw for deselection
                        ctrl.Activate();
                        if (selectionFlow != SelectionFlow.Downwards)
                        {
                            MoveMarkerUp();
                            return;
                        }
                    }

                    ControlIndex = (ControlIndex + 1 < Controls.Count) ? ControlIndex += 1 : Controls.Count - 1;
                    ctrl = Controls[ControlIndex];
                    ctrlIndex++;
                }

                Deselect();
                Select();
            }
            else
            {
                Active = false;
                ((IContainer)Parent).selectionFlow = SelectionFlow.Downwards;
                Deselect();
            }
        }
    }
}
