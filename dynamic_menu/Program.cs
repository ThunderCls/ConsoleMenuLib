using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

namespace dynamic_menu
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

            Controls.Panel panel = new Controls.Panel()
            {
                CtrlPosition = new Controls.Position
                {
                    LeftSpacing = 8,
                    TopSpacing = 5
                },
                CtrlSize = new Controls.Size
                {
                    Height = 20,
                    Width = 50
                }
            };
            Controls.Panel mainPanel = new Controls.Panel
            {
                CtrlPosition = new Controls.Position
                {
                    LeftSpacing = 3,
                    TopSpacing = 3
                },
                CtrlSize = new Controls.Size
                {
                    Height = 24,
                    Width = 60
                }
            };

            Controls.Slider sliderContrast = new Controls.Slider()
            {
                Caption = "Contrast:  ",
                Minimum = 0,
                Maximum = 10,
                Value = 3,
                Selected = true,
                CtrlPosition = new Controls.Position
                {
                    //LeftSpacing = 8,
                    TopSpacing = 4,
                    LeftSpacing = 6,
                    LeftPadding = 2
                }
            };
            Controls.Slider sliderBrightness = new Controls.Slider()
            {
                Caption = "Brightness:",
                Minimum = 0,
                Maximum = 10,
                Value = 5,
                CtrlPosition = new Controls.Position
                {
                    //LeftSpacing = 8,
                    TopSpacing = 6,
                    LeftSpacing = 6, 
                    LeftPadding = 2
                }
            };
            sliderBrightness.OnValueChanged += ValueChanged;

            Controls.Button btnBack = new Controls.Button
            {
                Caption = "Close",
                CtrlPosition = new Controls.Position
                {
                    TopSpacing = 10,
                    LeftSpacing = 20, 
                    LeftPadding = 3
                }
            };
            btnBack.OnExecute += CloseExecute;

            mainPanel.Draw();

            panel.AddControl(sliderContrast);
            panel.AddControl(sliderBrightness);
            panel.AddControl(btnBack);            
            panel.Activate();
            
        }

        static void CloseExecute(Object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        static void ValueChanged(object sender, EventArgs e)
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