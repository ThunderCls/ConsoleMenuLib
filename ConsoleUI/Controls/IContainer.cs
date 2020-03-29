using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleUI.Controls;

namespace ConsoleUI.Controls
{
    public enum SelectionFlow
    {
        Upwards,
        Downwards
    }

    public interface IContainer
    {
        List<IControl> Controls { get; set; }
        int ControlIndex { get; set; }
        SelectionFlow selectionFlow { get; set; }
        void MoveMarkerUp();
        void MoveMarkerDown();
        void AddControl(IControl ctrl);
    }
}
