using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleUI.Controls
{
    public class Dialog : IControl, IContainer
    {
        // TODO: implement changing console window to fit dialog size

        private const char HorizontalLineSymbol = '━';
        private const char VerticalLineSymbol = '┃';
        private const char LeftUpCornerSymbol = '┏';
        private const char RightUpCornerSymbol = '┓';
        private const char LeftDownCornerSymbol = '┗';
        private const char RightDownCornerSymbol = '┛';

        // P/Invoke declarations
        private struct RECT { public int left, top, right, bottom; }
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT rc);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool repaint);

        public List<IControl> Controls { get; set; }
        public int ControlIndex { get; set; }
        public IControl Parent { get; set; }
        public Position CtrlPosition { get; set; }
        public Size CtrlSize { get; set; }
        public bool Selected { get; set; }
        public bool Active { get; set; }
        public string Caption { get; set; }
        public bool Borders { get; set; }
        public bool CenteredCaption { get; set; }
        public bool ConsoleWindowAutoSize { get; set; }
        public bool ConsoleWindowCentered { get; set; }
        public SelectionFlow selectionFlow { get; set; }

        public Dialog()
        {
            // hide cursor and activate unicode
            Console.OutputEncoding = Encoding.Unicode;
            Console.CursorVisible = false;

            Controls = new List<IControl>();
            CtrlPosition = new Position();
            CtrlSize = new Size();
            Caption = " Dialog ";
            Borders = true;
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

        public void MoveMarkerUp()
        {
            selectionFlow = SelectionFlow.Upwards;

            // TODO: implement with TabIndex
            if (ControlIndex - 1 >= 0)
            {
                ControlIndex--;
            }
            else
            {
                ControlIndex = Controls.Count - 1;
                // reset containers ctrl index to last item
                foreach (var container in Controls.FindAll(x => x.GetType().GetInterfaces()
                                                         .Any(y => y == typeof(IContainer))))
                {
                    ((IContainer) container).ControlIndex = ((IContainer) container).Controls.Count - 1;
                }
            }

            var ctrl = Controls[ControlIndex];

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

                ControlIndex = (ControlIndex - 1 >= 0) ? ControlIndex -= 1 : Controls.Count - 1;
                ctrl = Controls[ControlIndex];
                ctrlIndex++;
            }

            Controls.ForEach(x => x.Deselect());
            Controls[ControlIndex].Select();
        }

        public void MoveMarkerDown()
        {
            selectionFlow = SelectionFlow.Downwards;

            // TODO: implement with TabIndex
            if (ControlIndex + 1 < Controls.Count)
            {
                ControlIndex++;
            }
            else
            {
                ControlIndex = 0;
                foreach (var container in Controls.FindAll(x => x.GetType().GetInterfaces()
                                                         .Any(y => y == typeof(IContainer))))
                {
                    // reset container ctrl index to last item
                    ((IContainer)container).ControlIndex = 0;
                }
            }

            var ctrl = Controls[ControlIndex];

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

                ControlIndex = (ControlIndex + 1 < Controls.Count) ? ControlIndex += 1 : 0;
                ctrl = Controls[ControlIndex];
                ctrlIndex++;
            }

            Controls.ForEach(x => x.Deselect());
            Controls[ControlIndex].Select();
        }

        public void Draw()
        {
            if (Borders)
            {
                DrawBorders();
            }
            else
            {
                DrawNoBorders();
            }
        }

        public void DrawNoBorders()
        {
            Console.SetCursorPosition(CtrlPosition.LeftSpacing, CtrlPosition.TopSpacing);

            int dlgLeftTitleLine = 1;
            if (CenteredCaption)
            {
                dlgLeftTitleLine = (CtrlSize.Width / 2) - (Caption.Length / 2);
            }

            Console.Write(new string(' ', dlgLeftTitleLine + 1));
            Console.Write(Caption);

            // draw every children control
            foreach (var ctrl in Controls)
            {
                ctrl.Draw();
            }
        }

        public void DrawBorders()
        {
            // draw the upper line with dialog caption
            Console.SetCursorPosition(CtrlPosition.LeftSpacing, CtrlPosition.TopSpacing);

            int dlgLeftTitleLine = 1;
            if (CenteredCaption)
            {
                dlgLeftTitleLine = (CtrlSize.Width / 2) - (Caption.Length / 2);
            }
            Console.Write(LeftUpCornerSymbol + new string(HorizontalLineSymbol, dlgLeftTitleLine));
            Console.Write(Caption);
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

            if (ConsoleWindowAutoSize)
            {
                SetConsoleWindowSize();
            }

            if (ConsoleWindowCentered)
            {
                CenterConsoleWindow();
            }

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

        private void SetConsoleWindowSize()
        {
            Console.SetWindowSize(CtrlSize.Width + CtrlPosition.LeftSpacing * 2,
                CtrlSize.Height + CtrlPosition.TopSpacing * 2);
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
        }

        private void CenterConsoleWindow()
        {
            IntPtr hWin = GetConsoleWindow();
            RECT rc;
            GetWindowRect(hWin, out rc);
            Screen scr = Screen.FromPoint(new Point(rc.left, rc.top));
            int x = scr.WorkingArea.Left + (scr.WorkingArea.Width - (rc.right - rc.left)) / 2;
            int y = scr.WorkingArea.Top + (scr.WorkingArea.Height - (rc.bottom - rc.top)) / 2;
            MoveWindow(hWin, x, y, rc.right - rc.left, rc.bottom - rc.top, false);
        }

        public void Select() { }

        public void Deselect() { }
    }
}
