using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleUI.Controls
{
    // See: https://github.com/Mpdreamz/shellprogressbar
    // for ideas
    public class ProgressBar : IControl
    {
        private const char LeftSymbol = '[';//'▓';//
        private const char FillSymbol = '■';//'█';
        private const char RightSymbol = ']';//'▓';

        public IControl Parent { get; set; }
        public Position CtrlPosition { get; set; }
        public Size CtrlSize { get; set; }
        public bool Selected { get; set; }
        public bool Active { get; set; }
        public int TabIndex { get; set; }

        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                _value = value <= Maximum ? value : _value;
                if (Parent != null)
                {
                    Draw(); // draw in real time since parent could be waiting on some child
                    ProcessStep();
                }
            }
        }

        public int Maximum { get; set; }
        public int Minimum { get; set; }
        public int Step { get; set; }
        public bool Loop { get; set; } // TODO: implement

        public event EventHandler OnProgress;

        protected virtual void ProcessStep()
        {
            OnProgress?.Invoke(this, null);
        }

        public ProgressBar()
        {
            CtrlPosition = new Position();
            CtrlSize = new Size();
            Value = 0;
            Maximum = 10;
            Minimum = 0;
            Step = 1;
        }

        public void Draw()
        {
            // TODO: implement scaling maximum/value/step with control width

            int parentLeft = Parent?.CtrlPosition.LeftSpacing ?? 0;
            int parentTop = Parent?.CtrlPosition.TopSpacing ?? 0;

            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing);
            string fillState = new string(FillSymbol, Value <= Maximum ? Value : 0);
            Console.WriteLine(LeftSymbol +
                              fillState +
                              new string(' ', CtrlSize.Width == 0 ? 0 : (CtrlSize.Width - fillState.Length - 1)) +
                              RightSymbol);
        }

        public void Activate() { }

        public async Task ActivateAsync() { }

        public void ProcessKeyPress() { }

        public void Select() { }

        public void Deselect() { }
    }
}
