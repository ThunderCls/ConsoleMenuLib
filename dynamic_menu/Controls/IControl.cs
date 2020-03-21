using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dynamic_menu.Controls
{
    public class Position
    {
        public int TopSpacing { get; set; }
        public int TopPadding { get; set; }
        public int LeftPadding { get; set; }
        public int LeftSpacing { get; set; }
        public bool LeftCentered { get; set; }
        public bool TopCentered { get; set; }
    }
    public class Size
    {
        public int Height { get; set; }
        public int Width { get; set; }
    }

    public interface IControl
    {
        IControl Parent { get; set; }
        Position CtrlPosition { get; set; }
        Size CtrlSize { get; set; }
        bool Selected { get; set; }
        bool Active { get; set; }
        int TabIndex { get; set; }

        //void Show();
        void Draw();
        void Activate();
        void ProcessKeyPress();
        void Select();
        void Deselect();        

        //public async Task ActivateAsync()
        //{
        //    await Task.Run(() =>
        //    {
        //        do
        //        {
        //            Draw();
        //            ProcessKeyPress();
        //        } while (Active);
        //    });         
        //}
    }
}
