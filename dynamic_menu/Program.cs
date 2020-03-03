using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dynamic_menu
{
    class Program
    {
        static void Main(string[] args)
        {
            // main menu setup
            Menu mainMenu = new VerticalMenu
            {
                HeaderText = new List<string>()
                {
                    "============",
                    " Main Menu  ",
                    "============",
                    "Options:"
                },
                HeaderPos = new Menu.Position
                {
                    LeftCentered = true,
                    TopCentered = true
                },
                EntriesPos = new Menu.Position
                {
                    TopSpacing = 2,
                    TopPadding = 1,
                    LeftCentered = true
                }
            };
            Entry blueEntry = new Entry
            {
                Selected = true,
                EntryTitle = "Blue",
                Event = () => { Console.BackgroundColor = ConsoleColor.DarkBlue; }
            };
            Entry redEntry = new Entry
            {
                EntryTitle = "Red",
                Event = () => { Console.BackgroundColor = ConsoleColor.DarkRed; }
            };


            // submenu setup
            Menu subMenu = new HorizontalMenu
            {
                HeaderText = new List<string>()
                {
                    "==================",
                    "     Sub Menu  ",
                    "=================="
                },
                EntryTitle = "Sub Menu",
                HeaderPos = new Menu.Position
                {
                    LeftCentered = true,
                    TopCentered = true
                },
                EntriesPos = new Menu.Position
                {
                    TopSpacing = 2,
                    LeftPadding = 8,
                    LeftCentered = true
                }
            };
            Entry greenEntry = new Entry
            {
                Selected = true,
                EntryTitle = "Green",
                Event = () => { Console.BackgroundColor = ConsoleColor.DarkGreen; }
            };
            Entry yellowEntry = new Entry
            {
                EntryTitle = "Yellow",
                Event = () => { Console.BackgroundColor = ConsoleColor.DarkYellow; }
            };
            Menu subMenuBack = new HorizontalMenu
            {
                EntryTitle = "Back",
                Selected = false
            };

            subMenu.AddChild(greenEntry);
            subMenu.AddChild(yellowEntry);
            subMenu.AddChild(subMenuBack);

            // add entries to the main menu
            mainMenu.AddChild(blueEntry);
            mainMenu.AddChild(redEntry);
            mainMenu.AddChild(subMenu);
            mainMenu.ControlLoop();
        }
    }
}