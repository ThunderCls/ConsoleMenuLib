using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dynamic_menu.Controls
{
    public class Dialog : IControl
    {
        public IControl Parent { get; set; }
        public Position CtrlPosition { get; set; }
        public Size CtrlSize { get; set; }
        public bool Selected { get; set; }
        public bool Active { get; set; }
        public int TabIndex { get; set; }

        public void Activate()
        {
            throw new NotImplementedException();
        }
        public async Task ActivateAsync()
        {
            await Task.Run(() =>
            {
                Activate();
            });
        }

        public void Deselect()
        {
            throw new NotImplementedException();
        }

        public void Draw()
        {
            throw new NotImplementedException();
        }

        public void ProcessKeyPress()
        {
            throw new NotImplementedException();
        }

        public void Select()
        {
            throw new NotImplementedException();
        }
    }
}
