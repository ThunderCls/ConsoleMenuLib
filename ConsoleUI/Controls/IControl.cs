using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleUI.Controls
{
    public static class Common
    {
        public static string SelectionSymbol = "►";
    }    

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

        void Draw();
        void Activate();
        Task ActivateAsync();
        void ProcessKeyPress();
        void Select();
        void Deselect();
        bool CoordinateInsideClientArea(int x, int y);
        //TODO: implement a ReDraw() function to repaint the control space only and it's children
    }
}
