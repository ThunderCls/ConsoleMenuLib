using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleUI.Controls
{
    public class Slider : IControl
    {
        private const char SliderLine = '─';
        private const char SliderMark = '▌';

        // TODO: implement
        public enum SliderPosition
        {
            Horizontal,
            Vertical
        }

        public string Caption { get; set; }

        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                if (Parent == null)
                {
                    _value = value;
                }
                else
                {
                    // draw current text
                    _value = value;
                    OnValueChange();
                }
            }
        }

        public int Maximum { get; set; }
        public int Minimum { get; set; }
        public int Step { get; set; }
        
        public SliderPosition Orientation { get; set; }
        public IControl Parent { get; set; }
        public Position CtrlPosition { get; set; }
        public Size CtrlSize { get; set; }
        public bool Selected { get; set; }
        public bool Active { get; set; }
        public int TabIndex { get; set; }

        public event EventHandler OnValueChanged;
        protected virtual void OnValueChange()
        {
            OnValueChanged?.Invoke(this, null);
        }

        public Slider()
        {
            CtrlPosition = new Position();
            CtrlSize = new Size();
            Caption = "Slide";
            Maximum = 10;
            Step = 1;
        }

        public void ProcessKeyPress()
        {
            if (!Active)
                return;

            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                case ConsoleKey.Subtract:
                    MoveMarkerLeft();
                    OnValueChange();
                    break;

                case ConsoleKey.RightArrow:
                case ConsoleKey.Add:
                    MoveMarkerRight();
                    OnValueChange();
                    break;

                case ConsoleKey.Enter:
                    Deactivate();
                    break;
            }
        }

        private void MoveMarkerLeft()
        {
            // TODO: implement steps instead
            _value = _value == Minimum ? Minimum : _value -= 1;            
        }

        private void MoveMarkerRight()
        {
            // TODO: implement steps instead
            _value = _value == Maximum ? Maximum : _value += 1;
        }

        public void Activate()
        {
            Active = true;

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

        private void Deactivate()
        {
            Active = false;
            Console.ResetColor();
            //Console.Clear();
        }

        public void Draw()
        {
            // TODO: implement scaling maximum/value/step with control width

            //Console.BackgroundColor = Active ? ConsoleColor.DarkGreen : ConsoleColor.Black;
            Console.ForegroundColor = Active ? ConsoleColor.White : ConsoleColor.Gray;

            int parentLeft = Parent?.CtrlPosition.LeftSpacing ?? 0;
            int parentTop = Parent?.CtrlPosition.TopSpacing ?? 0;

            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing);
            Console.Write($"{GetMarker()} {Caption}" + new string(' ', CtrlPosition.LeftPadding));

            for (int index = Minimum; index <= Maximum; index++)
            {
                Console.Write(index == _value ? SliderMark : SliderLine);
            }
            Console.Write(new string(' ', CtrlPosition.LeftPadding) +
                          _value + 
                          new string(' ', Convert.ToString(Maximum).Length - Convert.ToString(_value).Length));
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
    }
}
