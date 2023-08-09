using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WindowsInput;

namespace ControlWin
{
    public class Mouse
    {
        [Flags]
        public enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }
        public enum ScanCodeShort : short
        {
            VK_LEFT = 0x25,
            VK_UP,
            VK_RIGHT,
            VK_DOWN,
            KEY_0 = 0x30,
            KEY_1,
            KEY_2,
            KEY_3,
            KEY_4,
            KEY_5,
            KEY_6,
            KEY_7,
            KEY_8,
            KEY_9,
            KEY_A = 0x41,
            KEY_B,
            KEY_C,
            KEY_D,
            KEY_E,
            KEY_F,
            KEY_G,
            KEY_H,
            KEY_I,
            KEY_J,
            KEY_K,
            KEY_L,
            KEY_M,
            KEY_N,
            KEY_O,
            KEY_P,
            KEY_Q,
            KEY_R,
            KEY_S,
            KEY_T,
            KEY_U,
            KEY_V,
            KEY_W,
            KEY_X,
            KEY_Y,
            KEY_Z,
            VK_LWIN,
            VK_RWIN,
            VK_LSHIFT = 0xA0,
            VK_RSHIFT,
            VK_LCONTROL,
            VK_RCONTROL,
            VK_LMENU,
            VK_RMENU,
        }
        public Buttons LeftButton = new(MouseEventFlags.LeftDown, MouseEventFlags.LeftUp);
        public Buttons RightButton = new(MouseEventFlags.RightDown, MouseEventFlags.RightUp);
        public Buttons MiddleButton = new(MouseEventFlags.MiddleDown, MouseEventFlags.MiddleUp);
        private bool _shift = false;
        public bool Shift
        {
            get => _shift;
            set
            {
                _shift = value;
            }
        }
        private bool _capslock = false;
        public bool CapsLock
        {
            get => _capslock;
            set
            {
                _capslock = value;
            }
        }
        public class Buttons
        {
            private readonly MouseEventFlags _dw;
            private readonly MouseEventFlags _up;
            public int ButtonState
            {
                set
                {
                    if (value == 0)
                    {
                        mouse_event((int)_up, CursorPos.X, CursorPos.Y, 0, 0);
                        return;
                    }
                    mouse_event((int)_dw, CursorPos.X, CursorPos.Y, 0, 0);
                }
            }
            public Buttons(MouseEventFlags down, MouseEventFlags up) { _dw = down; _up = up; }
        }
        public static void MoveTo(int x, int y)
        {
            SetCursorPos(CursorPos.X + x, CursorPos.Y + y);
        }
        private static CURSORINFO Cursorinfo
        {
            get
            {
                CURSORINFO pci;
                pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
                GetCursorInfo(out pci);
                return pci;
            }
        }
        private static Point CursorPos
        {
            get
            {
                return Cursorinfo.ptScreenPos;
            }
        }
        public static IntPtr CursorState // text is 65541
        {
            get => Cursorinfo.hCursor;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct CURSORINFO
        {
            public Int32 cbSize;        // Specifies the size, in bytes, of the structure. 
                                        // The caller must set this to Marshal.SizeOf(typeof(CURSORINFO)).
            public Int32 flags;         // Specifies the cursor state. This parameter can be one of the following values:
                                        //    0             The cursor is hidden.
                                        //    CURSOR_SHOWING    The cursor is showing.
            public IntPtr hCursor;          // Handle to the cursor. 
            public Point ptScreenPos;       // A POINT structure that receives the screen coordinates of the cursor. 
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll")]
        private static extern long SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo); 
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        async public static void UpperStringEmulate(string key)
        {
            foreach(char c in key)
            {
                await Simulate.Events().Click(c).Invoke();
            }
        }
        async public static void LowerStringEmulate(string input)
        {
            foreach(char c in input)
            {
                await Task.Delay(10);
                keybd_event((byte)Enum.Parse(typeof(ScanCodeShort), $"KEY_{c.ToString().ToUpper()}"), 0x45, 0, 0);
            }
        }
    }
}
