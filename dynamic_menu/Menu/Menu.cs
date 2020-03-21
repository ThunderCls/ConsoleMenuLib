using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dynamic_menu
{
    /// <summary>
    /// Menu.cs
    /// Abstract menu class
    /// </summary>
    public abstract class Menu
    {
        /// <summary>
        /// Menu header UI details
        /// </summary>
        public class Position
        {
            public int TopSpacing { get; set; }
            public int TopPadding { get; set; }
            public int LeftPadding { get; set; }
            public int LeftSpacing { get; set; }
            public bool LeftCentered { get; set; }
            public bool TopCentered { get; set; }
        }

        public const string MarkerSymbol = "=>";
        public List<string> HeaderText { get; set; }
        public string EntryTitle { get; set; }
        public bool Selected { get; set; }
        public Position HeaderPos { get; set; }
        public Position EntriesPos { get; set; }

        protected internal Menu Parent;
        protected internal List<Menu> Children;
        protected internal int ChildIndex;

        protected internal Menu()
        {
            Children = new List<Menu>();
            HeaderPos = new Position();
            EntriesPos = new Position();
        }

        protected internal void Select()
        {
            Selected = true;
        }

        protected internal void Deselect()
        {
            Selected = false;
        }

        protected internal string GetMarker()
        {
            return Selected ? MarkerSymbol : new string(' ', MarkerSymbol.Length);
        }

        public void AddChild(Menu child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        protected internal string GetEntryString()
        {
            return $"{this.GetMarker()} {this.EntryTitle}";
        }

        public void ControlLoop()
        {
            Console.CursorVisible = false;
            do
            {
                ShowMenu();
                ProcessKeyPress();
            } while (true);
        }

        protected internal abstract void Execute();
        protected internal abstract void ShowMenu();
        protected internal abstract void ProcessKeyPress();
    }
}
