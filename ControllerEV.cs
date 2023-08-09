using SharpDX.DirectInput;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControlWin
{
    public class Controller
    {
        protected Mouse _m = new();
        private static int DegreeMax2 { get { return 16; } }
        private static int DegreeMax2Value { get { return (int)Math.Pow(2, DegreeMax2) - 1; } }
        private static int CenterDegree { get { return DegreeMax2 - 1; } }
        private static int Center { get { return (int)Math.Pow(2, CenterDegree); } }
        private static int Mask { get { return 5000; } }
        public class ValBtns
        {
            public JoystickOffset LeftButtonOffset = JoystickOffset.Buttons4;
            public JoystickOffset RighttButtonOffset = JoystickOffset.Buttons5;
            public JoystickOffset MoveX = JoystickOffset.X;
            public JoystickOffset MoveY = JoystickOffset.Y;
        }
        public ValBtns ButtonsOffsets = new();
        private readonly MainWindow _WinToShow;

        private void Func(JoystickOffset offset)
        {
            ValueOfOffset[(int)offset] += 1 - Center - Convert.ToInt32(ValueOfOffset[(int)offset] + 1 <= Center);
            ValueOfOffset[(int)offset] /= Mask;
        }
        
        private void Moving(JoystickOffset offset)
        {
            Mouse.MoveTo(ValueOfOffset[(int)ButtonsOffsets.MoveX], ValueOfOffset[(int)ButtonsOffsets.MoveY]);
        }

        private void ChangeBackround(JoystickOffset offset)
        {
            Brush? br;
            if (ValueOfOffset[(int)offset] == 0)
                br = Application.Current.Resources["MainColor"] as Brush;
            else
                br = Application.Current.Resources["PrimaryColor"] as Brush;
            _WinToShow.Dispatcher.Invoke(() => UIEls[(int)offset].Background = br ?? throw new("Brush isn't exists"));
        }

        private void ChangeLeftTriggerBackround(JoystickOffset offset)
        {
            Application.Current.Resources["LeftTriggerValue"] = ValueOfOffset[(int)offset] / DegreeMax2Value;
        }
        private void ChangeRightTriggerBackround(JoystickOffset offset)
        {
            Application.Current.Resources["RightTriggerValue"] = ValueOfOffset[(int)offset] / DegreeMax2Value;
        }

        public readonly int[] ValueOfOffset = new int[(int)(Enum.GetValues(typeof(JoystickOffset)).Cast<JoystickOffset>().Last())];
        private readonly Border[] UIEls = new Border[(int)(Enum.GetValues(typeof(JoystickOffset)).Cast<JoystickOffset>().Last())];
        private delegate void Delegates(JoystickOffset offset);
        private readonly Delegates[] Functions = new Delegates[(int)(Enum.GetValues(typeof(JoystickOffset)).Cast<JoystickOffset>().Last())];

        public void SetValue(JoystickOffset offset, int value)
        {
            ValueOfOffset[(int)offset] = value;
            Functions[(int)offset]?.Invoke(offset);
        }
        public Controller(MainWindow win)
        {
            _WinToShow = win;
            
            // stick values

            Functions[(int)JoystickOffset.X] = Func;
            Functions[(int)JoystickOffset.X] += Moving;

            Functions[(int)JoystickOffset.Y] = Func;
            Functions[(int)JoystickOffset.X] += Moving;

            Functions[(int)JoystickOffset.Z] = Func;
            Functions[(int)JoystickOffset.X] += Moving;

            Functions[(int)JoystickOffset.RotationZ] = Func;
            Functions[(int)JoystickOffset.X] += Moving;

            //buttons

            Functions[(int)JoystickOffset.Buttons0] = ChangeBackround;
            UIEls[(int)JoystickOffset.Buttons0] = _WinToShow.B0;

            Functions[(int)JoystickOffset.Buttons1] = ChangeBackround;
            UIEls[(int)JoystickOffset.Buttons1] = _WinToShow.B1;

            Functions[(int)JoystickOffset.Buttons2] = ChangeBackround;
            UIEls[(int)JoystickOffset.Buttons2] = _WinToShow.B2;

            Functions[(int)JoystickOffset.Buttons3] = ChangeBackround;
            UIEls[(int)JoystickOffset.Buttons3] = _WinToShow.B3;

            Functions[(int)JoystickOffset.Buttons4] = ChangeBackround;
            UIEls[(int)JoystickOffset.Buttons4] = _WinToShow.L1;

            Functions[(int)JoystickOffset.Buttons5] = ChangeBackround;
            UIEls[(int)JoystickOffset.Buttons5] = _WinToShow.R1;
            
            Functions[(int)JoystickOffset.Buttons6] = ChangeLeftTriggerBackround;

            Functions[(int)JoystickOffset.Buttons7] = ChangeRightTriggerBackround;

            Functions[(int)JoystickOffset.Buttons10] = ChangeBackround;
            UIEls[(int)JoystickOffset.Buttons10] = _WinToShow.StickLL;

            Functions[(int)JoystickOffset.Buttons11] = ChangeBackround;
            UIEls[(int)JoystickOffset.Buttons11] = _WinToShow.StickLR;
        }
    }
}
