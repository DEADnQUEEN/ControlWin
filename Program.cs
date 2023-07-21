using SharpDX.DirectInput;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace TEST1
{
    internal class Sharper
    {
        private readonly DirectInput d = new();
        private readonly List<Joystick> joysticks = new();
        private readonly List<Guid> joyGuids = new();
        private IList<DeviceInstance> Gamepads { get { return d.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AllDevices); } }
        private static int DegreeMax2 { get { return 16; } }
        private static int CenterDegree { get { return DegreeMax2 - 1; } }
        private static int Center { get { return (int)Math.Pow(2, CenterDegree); } }
        private readonly Controller _c = new(Center);
        private Thread? _mThread;
        private Thread MainThread
        {
            get { return _mThread ?? throw new ArgumentNullException(); }
            set { _mThread = value; }
        }
        private Thread? _pThread;
        private Thread PrimaryThread
        {
            get => _pThread ?? throw new ArgumentNullException();
            set => _pThread = value;
        }
        private bool _Polling = false;
        public bool Polling
        {
            get
            {
                return _Polling;
            }
            set
            {
                _Polling = value;
                if (value)
                {
                    CorrectPolling();
                    return;
                }
            }
        }
        private void GuidFounder()
        {
            while (Gamepads.Count <= 0)
            {
                Console.WriteLine("Waiting for gamepad(-s)");
                Thread.Sleep(1000);
                Console.Clear();
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
        private void CorrectPolling()
        {
            GuidFounder();

            MainThread.Start();
            PrimaryThread.Start();
            MainThread.Join();
            PrimaryThread.Join();
        }
        public Sharper()
        {
            _mThread = new Thread(() =>
            {
                while (Polling)
                {
                    foreach (var g in Gamepads)
                    {
                        int id = joyGuids.IndexOf(g.InstanceGuid);
                        if (id == -1) { GuidFounder(); }
                        joysticks[id].Acquire();
                        joysticks[id].Poll();
                        foreach (var data in joysticks[id].GetBufferedData())
                        {
                            switch (data.Offset)
                            {
                                case var value when value == _c.ButtonsOffsets.MoveX: // x
                                    _c.TrigerVarX = data.Value;
                                    break;
                                case var value when value == _c.ButtonsOffsets.MoveY: // y
                                    _c.TrigerVarY = data.Value;
                                    break;
                                case var value when value == _c.ButtonsOffsets.LeftButtonOffset: // L
                                    _c.L1 = data.Value;
                                    break;
                                case var value when value == _c.ButtonsOffsets.RighttButtonOffset: // R
                                    _c.R1 = data.Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            });
            _pThread = new Thread(() =>
            {
                while (Polling)
                {
                    if (_c.TrigerVarX != 0 || _c.TrigerVarY != 0)
                    {
                        Mouse.MoveTo(_c.TrigerVarX, _c.TrigerVarY);
                    }
                    Thread.Sleep(1000 / 144);
                }
            })
            {
                IsBackground = true
            };
            Polling = true;
        }
        ~Sharper()
        {
            Polling = false;
        }
    }
}
