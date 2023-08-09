using SharpDX.DirectInput;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Windows.Threading;

namespace ControlWin
{
    internal class Sharper
    {
        private readonly DirectInput d = new();
        private readonly List<Joystick> joysticks = new();
        private readonly List<Guid> joyGuids = new();
        private IList<DeviceInstance> Gamepads { get { return d.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AllDevices); } }
        private Controller? controll;
        private Controller C
        {
            get => controll ?? throw new ArgumentNullException(nameof(controll));
            set => controll = value;
        }
        private Thread? _mThread;
        private Thread MainThread
        {
            get => _mThread ?? throw new ArgumentNullException();
            set => _mThread = value;
        }
        private Thread? _pThread;
        private Thread PrimaryThread
        {
            get => _pThread ?? throw new ArgumentNullException();
            set => _pThread = value;
        }
        private void GuidFounder()
        {
            while (Gamepads.Count <= 0)
            {
                Thread.Sleep(1000);
            }
            foreach (var g in Gamepads)
            {
                if (!joyGuids.Contains(g.InstanceGuid))
                {
                    joysticks.Add(new Joystick(d, g.InstanceGuid));
                    joysticks[^1].Properties.BufferSize = 128;
                    joyGuids.Add(g.InstanceGuid);
                }
            }
        }
        public Sharper(MainWindow window)
        {
            C = new Controller(window);
            _mThread = new Thread(() =>
            {
                while (window.IsInitialized)
                {
                    foreach (var g in Gamepads)
                    {
                        int id = joyGuids.IndexOf(g.InstanceGuid);
                        if (id == -1) { GuidFounder(); break; }
                        joysticks[id].Acquire();
                        joysticks[id].Poll();
                        foreach (var data in joysticks[id].GetBufferedData())
                        {
                            C.SetValue(data.Offset, data.Value);
                        }
                    }
                }
            });
            _pThread = new Thread(() =>
            {
                while (_mThread.IsAlive)
                {
                    if (C.ValueOfOffset[(int)C.ButtonsOffsets.MoveX] != 0 || C.ValueOfOffset[(int)C.ButtonsOffsets.MoveY] != 0)
                    {
                        Mouse.MoveTo(C.ValueOfOffset[(int)C.ButtonsOffsets.MoveX], C.ValueOfOffset[(int)C.ButtonsOffsets.MoveY]);
                    }
                    Thread.Sleep(1000 / 144);
                }
            })
            {
                IsBackground = true
            };

            MainThread.Start();
            PrimaryThread.Start();

            MainThread.Join();
            PrimaryThread.Join();
        }
    }
}
