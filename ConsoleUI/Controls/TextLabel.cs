using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.Controls
{
    public class TextLabel : IControl
    {
        public IControl Parent { get; set; }
        public Position CtrlPosition { get; set; }
        public Size CtrlSize { get; set; }
        public bool Selected { get; set; }
        public bool Active { get; set; }
        public int TabIndex { get; set; }

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                if (Parent == null)
                {
                    _text = value;
                }
                else
                {
                    // clean previous text
                    _text = new string(' ', _text.Length);
                    Draw();
                    // draw current text
                    _text = value;
                    Draw();
                }
            }
        }

        public TextLabel()
        {
            CtrlPosition = new Position();
            CtrlSize = new Size();
            Text = "TextLabel";
        }

        public void Draw()
        {
            int parentLeft = Parent?.CtrlPosition.LeftSpacing ?? 0;
            int parentTop = Parent?.CtrlPosition.TopSpacing ?? 0;

            Console.SetCursorPosition(parentLeft + CtrlPosition.LeftSpacing, parentTop + CtrlPosition.TopSpacing);
            Console.WriteLine(Text);
        }

        public void Activate() { }

        public async Task ActivateAsync() { }

        public void ProcessKeyPress() { }

        public void Select() { }

        public void Deselect() { }
    }
}
