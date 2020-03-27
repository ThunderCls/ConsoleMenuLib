using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleUI
{
    class Program
    {
        static List<ConsoleColor> colors = new List<ConsoleColor> { ConsoleColor.Black, ConsoleColor.DarkGray, ConsoleColor.Gray };
        static int colorIndex;
        
        static void Main(string[] args)
        {
            // main menu setup
            //Menu mainMenu = new VerticalMenu
            //{
            //    HeaderText = new List<string>()
            //    {
            //        "============",
            //        " Main Menu  ",
            //        "============",
            //        "Options:"
            //    },
            //    HeaderPos = new Menu.Position
            //    {
            //        LeftCentered = true,
            //        TopCentered = true
            //    },
            //    EntriesPos = new Menu.Position
            //    {
            //        TopSpacing = 2,
            //        TopPadding = 1,
            //        LeftCentered = true
            //    }
            //};
            //Entry blueEntry = new Entry
            //{
            //    Selected = true,
            //    EntryTitle = "Blue",
            //    Event = () => { Console.BackgroundColor = ConsoleColor.DarkBlue; }
            //};
            //Entry redEntry = new Entry
            //{
            //    EntryTitle = "Red",
            //    Event = () => { Console.BackgroundColor = ConsoleColor.DarkRed; }
            //};


            //// submenu setup
            //Menu subMenu = new HorizontalMenu
            //{
            //    HeaderText = new List<string>()
            //    {
            //        "==================",
            //        "     Sub Menu  ",
            //        "=================="
            //    },
            //    EntryTitle = "Sub Menu",
            //    HeaderPos = new Menu.Position
            //    {
            //        LeftCentered = true,
            //        TopCentered = true
            //    },
            //    EntriesPos = new Menu.Position
            //    {
            //        TopSpacing = 2,
            //        LeftPadding = 8,
            //        LeftCentered = true
            //    }
            //};
            //Entry greenEntry = new Entry
            //{
            //    Selected = true,
            //    EntryTitle = "Green",
            //    Event = () => { Console.BackgroundColor = ConsoleColor.DarkGreen; }
            //};
            //Entry yellowEntry = new Entry
            //{
            //    EntryTitle = "Yellow",
            //    Event = () => { Console.BackgroundColor = ConsoleColor.DarkYellow; }
            //};
            //Menu subMenuBack = new HorizontalMenu
            //{
            //    EntryTitle = "Back",
            //};

            //subMenu.AddChild(greenEntry);
            //subMenu.AddChild(yellowEntry);
            //subMenu.AddChild(subMenuBack);

            //// add entries to the main menu
            //mainMenu.AddChild(blueEntry);
            //mainMenu.AddChild(redEntry);
            //mainMenu.AddChild(subMenu);
            //mainMenu.ControlLoop();

            UI.RunUI();
            while (true)
            {
                Thread.Sleep(1000);
                string name = UI.txtName.Text;
                int value = UI.sliderContrast.Value;
            }
        }

        public static void CloseExecute(Object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        public static void ResetValues(Object sender, EventArgs e)
        {
            UI.txtName.Text = "";
            UI.txtPassword.Text = "";
            UI.sliderContrast.Value = 0;
            UI.sliderBrightness.Value = 0;
            UI.btnClose.Caption = "Exit";
        }

        public static void ValueChanged(object sender, EventArgs e)
        {
            colorIndex = ((Controls.Slider)sender).Value % 3;
            //Color c1 = Color.Red;
            //Color c2 = Color.FromArgb(c1.A,
            //    (int)(c1.R * 0.8), (int)(c1.G * 0.8), (int)(c1.B * 0.8));
            Console.BackgroundColor = colors[colorIndex];
            //Console.Clear();
        }
    }
}