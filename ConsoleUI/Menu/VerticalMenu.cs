using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    /// <summary>
    /// VerticalMenu.cs
    /// Vertical menu class
    /// </summary>
    public class VerticalMenu : Menu
    {
        protected internal override void Execute()
        {
            ControlLoop();
        }

        protected internal override void ShowMenu()
        {
            Console.Clear();

            // calculating left positioning for menu header
            int leftPosition = (HeaderPos.LeftCentered)
                    ? (Console.WindowWidth / 2) - (HeaderText.Max(x => x.Length) / 2)
                    : HeaderPos.LeftSpacing;

            int topPosition = (HeaderPos.TopCentered)
                ? (Console.WindowHeight / 2) - 
                  (((HeaderPos.TopPadding > 0) ? HeaderText.Count * 2 * HeaderPos.TopPadding : HeaderText.Count + 
                  ((EntriesPos.TopPadding > 0) ? Children.Count * 2 * EntriesPos.TopPadding : Children.Count)) / 2)
                : Console.CursorTop + HeaderPos.TopSpacing;

            Console.SetCursorPosition(leftPosition, topPosition);
            foreach (string line in HeaderText)
            {
                Console.WriteLine(line);
                Console.SetCursorPosition(leftPosition, Console.CursorTop + HeaderPos.TopPadding);
            }

            // calculating left positioning for menu entries
            int totalLeft = Children.Select(x => x.GetEntryString().Length).ToList().Max(x => x);
            leftPosition = (EntriesPos.LeftCentered)
                ? (Console.WindowWidth / 2) -
                  (totalLeft / 2)
                : EntriesPos.LeftSpacing;

            Console.SetCursorPosition(leftPosition, Console.CursorTop + EntriesPos.TopSpacing);
            foreach (var child in Children)
            {
                Console.WriteLine(child.GetEntryString());
                Console.SetCursorPosition(leftPosition, Console.CursorTop + EntriesPos.TopPadding);
            }
        }

        protected internal override void ProcessKeyPress()
        {
            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    MoveMarkerUp();
                    break;

                case ConsoleKey.DownArrow:
                    MoveMarkerDown();
                    break;

                case ConsoleKey.Enter:
                    ExecuteEntry();
                    break;

                case ConsoleKey.Escape:
                    TryExitConsole();
                    break;
            }
        }

        private void MoveMarkerUp()
        {
            ChildIndex = (ChildIndex - 1 >= 0) ? ChildIndex -= 1 : Children.Count - 1;
            Children.ForEach(x => x.Deselect());
            Children[ChildIndex].Select();
        }

        private void MoveMarkerDown()
        {
            ChildIndex = (ChildIndex + 1 < Children.Count) ? ChildIndex += 1 : 0;
            Children.ForEach(x => x.Deselect());
            Children[ChildIndex].Select();
        }

        private void ExecuteEntry()
        {
            if (ChildIndex >= 0 && ChildIndex < Children.Count)
            {
                Menu menu = Children[ChildIndex];
                if (menu?.GetType() != typeof(Entry) && menu?.Children.Count == 0)
                {
                    menu = this.Parent;
                }

                menu?.Execute();
            }
        }

        private void TryExitConsole()
        {
            if (this.Parent == null)
                Environment.Exit(0);
        }
    }
}
