using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dynamic_menu.Controls
{
    public class Slider : IControl
    {
        public const string SelectionSymbol = "►";
        private const char SliderLine = '─';
        private const char SliderMark = '▌';
        private const int ControlLoopSleep = 100;

        public enum SliderPosition
        {
            Horizontal,
            Vertical
        }

        public string Caption { get; set; }
        public int Value { get; set; }
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
            EventHandler handler = OnValueChanged;
            handler?.Invoke(this, null);
        }

        public Slider()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.CursorVisible = false;
            Active = false;
            Selected = false;
        }

        public void ProcessKeyPress()
        {
            if (!Active)
                return;

            var key = Console.ReadKey();
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
            Value = Value == Minimum ? Minimum : Value -= 1;            
        }

        private void MoveMarkerRight()
        {
            // TODO: implement steps instead
            Value = Value == Maximum ? Maximum : Value += 1;
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

        private void Deactivate()
        {
            Active = false;
            Console.ResetColor();
            //Console.Clear();
        }

        public void Draw()
        {
            //Console.BackgroundColor = Active ? ConsoleColor.DarkGreen : ConsoleColor.Black;
            Console.ForegroundColor = Active ? ConsoleColor.White : ConsoleColor.Gray;

            int parentLeft = Parent?.CtrlPosition.LeftSpacing ?? 0;
            int parentTop = Parent?.CtrlPosition.TopSpacing ?? 0;

            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing);
            Console.Write($"{GetMarker()} {Caption}" + new string(' ', CtrlPosition.LeftPadding));

            for (int index = Minimum; index <= Maximum; index++)
            {
                Console.Write(index == Value ? SliderMark : SliderLine);
            }
            Console.Write(new string(' ', CtrlPosition.LeftPadding) + 
                          Value + 
                          new string(' ', Convert.ToString(Maximum).Length - Convert.ToString(Value).Length));
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
            return Selected ? SelectionSymbol : new string(' ', SelectionSymbol.Length);
        }
    }
}
