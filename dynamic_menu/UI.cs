using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dynamic_menu
{
    class UI
    {
        public static Controls.Slider sliderContrast;
        public static Controls.Slider sliderBrightness;
        public static Controls.Button btnClose;
        public static Controls.TextInput textInput;
        public static Controls.Button btnReset;
        public static Controls.Panel mainPanel;
        public static Controls.Panel panel;

        static private void CreateUI()
        {
            panel = new Controls.Panel()
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
            mainPanel = new Controls.Panel
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

            sliderContrast = new Controls.Slider()
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
            sliderBrightness = new Controls.Slider()
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
            sliderBrightness.OnValueChanged += Program.ValueChanged;

            btnClose = new Controls.Button
            {
                Caption = "Close",
                CtrlPosition = new Controls.Position
                {
                    TopSpacing = 10,
                    LeftSpacing = 30,
                    LeftPadding = 3
                },
                CtrlSize = new Controls.Size
                {
                    Height = 5,
                    Width = 13
                }
            };
            btnClose.OnExecute += Program.CloseExecute;

            btnReset = new Controls.Button
            {
                Caption = "Reset",
                CtrlPosition = new Controls.Position
                {
                    TopSpacing = 10,
                    LeftSpacing = 10,
                    LeftPadding = 3
                },
                CtrlSize = new Controls.Size
                {
                    Height = 3,
                    Width = 13
                }
            };
            btnReset.OnExecute += Program.ResetValues;

            textInput = new Controls.TextInput
            {
                Caption = "Name:",
                MaxLength = 10,
                CtrlPosition = new Controls.Position
                {
                    TopSpacing = 8,
                    LeftSpacing = 10
                }
            };

            mainPanel.Draw();

            panel.AddControl(sliderContrast);
            panel.AddControl(sliderBrightness);
            panel.AddControl(textInput);
            panel.AddControl(btnReset);
            panel.AddControl(btnClose);
        }

        public static void RunUI()
        {
            CreateUI();
            panel.ActivateAsync();
        }
    }
}
