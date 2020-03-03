using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dynamic_menu
{
    /// <summary>
    /// HorizontalMenu.cs
    /// Horizontal menu class
    /// </summary>
    public class HorizontalMenu : Menu
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
                  ((EntriesPos.TopSpacing > 0) ? EntriesPos.TopSpacing + 1 : 1)) / 2)
                : Console.CursorTop + HeaderPos.TopSpacing;

            Console.SetCursorPosition(leftPosition, topPosition);
            foreach (string line in HeaderText)
            {
                Console.WriteLine(line);
                Console.SetCursorPosition(leftPosition, Console.CursorTop + HeaderPos.TopPadding);
            }

            // calculating left positioning for menu entries
            int totalEntriesLeftSpace = Children.Select(x => x.GetEntryString().Length + EntriesPos.LeftPadding)
                                                .ToList().Sum(x => x) - EntriesPos.LeftPadding;
            topPosition = Console.CursorTop + EntriesPos.TopSpacing;
            leftPosition = (EntriesPos.LeftCentered)
                ? (Console.WindowWidth / 2) - (totalEntriesLeftSpace / 2)
                : EntriesPos.LeftSpacing;
            foreach (var entry in Children)
            {
                Console.SetCursorPosition(leftPosition, topPosition);
                Console.WriteLine(entry.GetEntryString());
                leftPosition += MarkerSymbol.Length + entry.EntryTitle.Length + EntriesPos.LeftPadding;
            }
        }

        protected internal override void ProcessKeyPress()
        {
            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    MoveMarkerLeft();
                    break;

                case ConsoleKey.RightArrow:
                    MoveMarkerRight();
                    break;

                case ConsoleKey.Enter:
                    ExecuteEntry();
                    break;

                case ConsoleKey.Escape:
                    TryExitConsole();
                    break;
            }
        }

        private void MoveMarkerLeft()
        {
            ChildIndex = (ChildIndex - 1 >= 0) ? ChildIndex -= 1 : Children.Count - 1;
            Children.ForEach(x => x.Deselect());
            Children[ChildIndex].Select();
        }

        private void MoveMarkerRight()
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
