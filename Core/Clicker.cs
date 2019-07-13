using Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class Clicker
    {
        public bool IsRunning { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Message { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int Ms { get; set; }
        public DateTime? LastRun { get; private set; } = null;
        public ButtonType Button { get; set; }
        public ClickType Click { get; set; }
        public void Reset()
        {
            X = 0;
            Y = 0;
            Ms = 0;
            Seconds = 0;
            Minutes = 0;
            Hours = 0;
            Message = null;
            IsRunning = false;
            LastRun = null;
        }
        public void Stop()
        {
            IsRunning = false;
        }
        public async Task Start(int startDelay)
        {
            IsRunning = true;

            await Task.Delay(startDelay);

            while (IsRunning)
            {
                DateTime? nextRun = LastRun.HasValue
                    ? (DateTime?)LastRun.Value
                                        .AddMilliseconds(Ms)
                                        .AddSeconds(Seconds)
                                        .AddMinutes(Minutes)
                                        .AddHours(Hours)
                    : null;

                bool ready = LastRun == null || DateTime.Now >= nextRun;

                if (ready)
                {
                    MouseMove(X, Y);

                    switch (Button)
                    {
                        case ButtonType.Left:
                            await LeftClick(Click);
                            break;
                        case ButtonType.Right:
                            await RightClick(Click);
                            break;
                    }

                    if (!string.IsNullOrWhiteSpace(Message))
                    {
                        uint result = SendMessage(Message);
                        
                        if (result > 0)
                        {
                            await Task.Delay(10);
                            Send(VirtualKeyCode.RETURN);
                            await Task.Delay(10);
                            Send(VirtualKeyCode.RETURN);
                        }
                    }

                    LastRun = DateTime.Now;
                }

                int time = Hours * 3600000 + Minutes * 60000 + Seconds * 1000 + Ms;

                await Task.Delay(time);
            }
        }
        public static void Send(VirtualKeyCode code)
        {
            Input input = new Input();
            input.Type = InputType.Keyboard;
            input.Data.Keyboard.dwFlags = KeyEventFlags.Unicode;
            input.Data.Keyboard.wVk = code;
            input.Data.Keyboard.time = 0;
            SendInput(1, new Input[] { input }, Input.Size);
        }
        public static uint Send(ScanCode code)
        {
            Input input = new Input();
            input.Type = InputType.Keyboard;
            input.Data.Keyboard.dwFlags = KeyEventFlags.ScanCode;
            input.Data.Keyboard.wScan = code;
            input.Data.Keyboard.time = 0;
            return SendInput(1, new Input[] { input }, Input.Size);
        }

        public static async Task LeftClick(ClickType click)
        {
            Input mouseDownInput = new Input();
            mouseDownInput.Data.Mouse.dwFlags = MouseEventFlags.LEFTDOWN;
            Input mouseUpInput = new Input();
            mouseUpInput.Data.Mouse.dwFlags = MouseEventFlags.LEFTUP;
            Input[] inputs = new Input[] { mouseDownInput, mouseUpInput };

            SendInput(inputs.Length, inputs, Input.Size);

            if (click == ClickType.Double)
            {
                await Task.Delay(5);
                SendInput(inputs.Length, inputs, Input.Size);
            }
        }

        public static async Task RightClick(ClickType click)
        {
            Input mouseDownInput = new Input();
            mouseDownInput.Data.Mouse.dwFlags = MouseEventFlags.RIGHTDOWN;
            Input mouseUpInput = new Input();
            mouseUpInput.Data.Mouse.dwFlags = MouseEventFlags.RIGHTUP;
            Input[] inputs = new Input[] { mouseDownInput, mouseUpInput };

            SendInput(inputs.Length, inputs, Input.Size);

            if (click == ClickType.Double)
            {
                await Task.Delay(5);
                SendInput(inputs.Length, inputs, Input.Size);
            }
        }

        public static void MouseMove(int x, int y)
        {
            Input mouseMove = new Input();
            mouseMove.Type = InputType.Mouse;
            mouseMove.Data.Mouse.dx = x;
            mouseMove.Data.Mouse.dy = y;
            mouseMove.Data.Mouse.dwFlags = MouseEventFlags.MOVE;
            SendInput(1, new Input[] { mouseMove }, Input.Size);
        }

        public static uint SendMessage(string message)
        {
            List<Input> list = new List<Input>();

            using (var keyboard = new KeyboardPointer(CultureInfo.CurrentCulture))
            {
                foreach (var c in message)
                {
                    var vkc = (uint)keyboard.GetVirtualKeyValue(c);
                    var sc = MapVirtualKey(vkc, 0x00);
                    Input msg = new Input();
                    msg.Type = InputType.Keyboard;
                    msg.Data.Keyboard.dwFlags = KeyEventFlags.ScanCode;
                    msg.Data.Keyboard.wScan = (ScanCode)sc;
                    list.Add(msg);
                }
            }
      
            //if (list.Count > 0)
            //{
            //    Input enter = new Input();
            //    enter.Type = InputType.Keyboard;
            //    enter.Data.Keyboard.dwFlags = KeyEventFlags.Unicode;
            //    enter.Data.Keyboard.wVk = VirtualKeyCode.RETURN;
            //    enter.Data.Keyboard.time = 0;
            //    list.Add(enter);       
            //}

            return SendInput(list.Count, list.ToArray(), Input.Size);
        }

        /// <summary>
        /// Synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(int numberOfInputs, Input[] Inputs, int sizeOfInputStructure);

        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint virtualKeyCode, ScanCode scanCode,
            byte[] keyboardState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)]
StringBuilder receivingBuffer,
            int bufferSize, uint flags);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
