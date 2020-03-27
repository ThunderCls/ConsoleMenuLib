using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    class UI
    {
        public static Controls.Slider sliderContrast;
        public static Controls.Slider sliderBrightness;
        public static Controls.Button btnClose;
        public static Controls.TextInput txtName;
        public static Controls.TextInput txtPassword;
        public static Controls.Button btnReset;
        public static Controls.Dialog mainDialog;
        public static Controls.CheckBox checkSavePass;
        static private void CreateUI()
        {
            mainDialog = new Controls.Dialog()
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
                },
                Caption = " Form 1 "
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
                    TopSpacing = 6,
                    LeftSpacing = 6,
                    LeftPadding = 2
                }
            };
            sliderBrightness.OnValueChanged += Program.ValueChanged;

            txtName = new Controls.TextInput
            {
                Caption = "    Name:",
                MaxLength = 20,
                //Border = false,
                CtrlPosition = new Controls.Position
                {
                    TopSpacing = 9,
                    LeftSpacing = 6
                },
                CtrlSize = new Controls.Size
                {
                    Width = 24,
                    Height = 3
                }
            };

            txtPassword = new Controls.TextInput
            {
                Caption = "Password:",
                MaxLength = 20,
                MaskChars = true,
                //Border = false,
                CtrlPosition = new Controls.Position
                {
                    TopSpacing = 12,
                    LeftSpacing = 6
                },
                CtrlSize = new Controls.Size
                {
                    Width = 24,
                    Height = 3
                }
            };

            checkSavePass = new Controls.CheckBox
            {
                Caption = "Remember Password",
                CtrlPosition = new Controls.Position
                {
                    TopSpacing = 14,
                    LeftSpacing = 16
                }
            };

            btnClose = new Controls.Button
            {
                Caption = "Close",
                CtrlPosition = new Controls.Position
                {
                    TopSpacing = 17,
                    LeftSpacing = 35,
                    LeftPadding = 3
                },
                CtrlSize = new Controls.Size
                {
                    Height = 3,
                    Width = 13
                }
            };
            btnClose.OnExecute += Program.CloseExecute;

            btnReset = new Controls.Button
            {
                Caption = "Reset",
                CtrlPosition = new Controls.Position
                {
                    TopSpacing = 17,
                    LeftSpacing = 15,
                    LeftPadding = 3
                },
                CtrlSize = new Controls.Size
                {
                    Height = 3,
                    Width = 13
                }
            };
            btnReset.OnExecute += Program.ResetValues;

            mainDialog.AddControl(sliderContrast);
            mainDialog.AddControl(sliderBrightness);
            mainDialog.AddControl(txtName);
            mainDialog.AddControl(txtPassword);
            mainDialog.AddControl(checkSavePass);
            mainDialog.AddControl(btnReset);
            mainDialog.AddControl(btnClose);
        }

        public static void RunUI()
        {
            CreateUI();
            mainDialog.ActivateAsync();
        }
    }
}
