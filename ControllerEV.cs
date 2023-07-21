using SharpDX.DirectInput;
using SharpDX.XInput;
using System;
using System.Windows;
using System.Windows.Media;

namespace TEST1
{
    public class Controller
    {
        protected Mouse _m = new();
        public class ValBtns
        {
            public JoystickOffset LeftButtonOffset = JoystickOffset.Buttons4;
            public JoystickOffset RighttButtonOffset = JoystickOffset.Buttons5;
            public JoystickOffset MoveX = JoystickOffset.X;
            public JoystickOffset MoveY = JoystickOffset.Y;
        }
        public ValBtns ButtonsOffsets = new();
        private int _tgX = 0;
        public int TrigerVarX
        {
            get { return _tgX; }
            set 
            { 
                _tgX = Func(value);
            }
        }
        private int _tgY = 0;
        public int TrigerVarY
        {
            get { return _tgY; }
            set 
            { 
                _tgY = Func(value);
            }
        }
        private int _a = 0;
        public int A
        {
            get => _a;
            set
            {
                _a = value;
            }
        }
        private int _b = 0;
        public int B
        {
            get => _b;
            set
            {
                _b = value;
            }
        }
        private int _y = 0;
        public int Y
        {
            get => _y;
            set
            {
                _y = value;
            }
        }
        private int _x = 0;
        public int X
        {
            get => _x;
            set
            {
                _x = value;
            }
        }
        private int _r1 = 0;
        public int R1
        {
            get => _r1;
            set
            {
                _r1 = value;
                _m.RightButton.ButtonState = _r1;
            }
        }
        private int _r2 = 0;
        public int R2
        {
            get => _r2;
            set
            {
                _r2 = value;
            }
        }
        private int _l1 = 0;
        public int L1
        {
            get => _l1;
            set
            {
                _l1 = value;
                _m.LeftButton.ButtonState = _l1;
            }
        }
        private int _l2 = 0;
        public int L2
        {
            get => _l2;
            set
            {
                _l2 = value;
            }
        }
        private static int Mask { get { return 5000; } }
        private static int Center;
        private static int Func(int num)
        {
            num++;
            return (num - Center - Convert.ToInt32(num <= Center)) / Mask;
        }
        public Controller(int center)
        {
            Center = center;
        }
    }
}