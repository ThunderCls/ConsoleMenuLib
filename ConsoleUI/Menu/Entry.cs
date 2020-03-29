using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    /// <summary>
    /// Entry.cs
    /// Menu entry class
    /// </summary>
    public class Entry : Menu
    {
        public Action Event { get; set; }

        protected internal override void Execute()
        {
            Event?.Invoke();
        }

        protected internal override void ShowMenu() { }

        protected internal override void ProcessKeyPress() { }
    }
}
