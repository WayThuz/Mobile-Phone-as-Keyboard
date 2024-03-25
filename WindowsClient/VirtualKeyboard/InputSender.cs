using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace VirtualKeyboard {
    internal class InputSender {

        public void Send(string msg) {
            InputAnalyzer.AnalyzeInputs(msg, out var inputs);
            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public void SimulateInputs() {
            var inputs =  CreateSimulateInputs();
            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        private INPUT[] CreateSimulateInputs() {

            INPUT shiftDown = new INPUT {
                type = (int)InputType.Keyboard,
                u = new InputUnion {
                    ki = new KeyboardInput {
                        wVk = 0,
                        wScan = 0x2A, //shift
                        dwFlags = (uint)(KeyEventF.KeyDown | KeyEventF.Scancode),
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            };
            INPUT keyDown = new INPUT {
                type = (int)InputType.Keyboard,
                u = new InputUnion {
                    ki = new KeyboardInput {
                        wVk = 0,
                        wScan = 0x11, //Press W
                        dwFlags = (uint)(KeyEventF.KeyDown | KeyEventF.Scancode),
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            };

            INPUT keyUp = new INPUT {
                type = (int)InputType.Keyboard,
                u = new InputUnion {
                    ki = new KeyboardInput {
                        wVk = 0,
                        wScan = 0x11, //Press W
                        dwFlags = (uint)(KeyEventF.KeyUp | KeyEventF.Scancode),
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            };

            INPUT shiftUp = new INPUT {
                type = (int)InputType.Keyboard,
                u = new InputUnion {
                    ki = new KeyboardInput {
                        wVk = 0,
                        wScan = 0x2A, //shift
                        dwFlags = (uint)(KeyEventF.KeyUp | KeyEventF.Scancode),
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            };

            INPUT[] inputs = new INPUT[] { shiftDown, keyDown, keyUp, shiftUp }; //must be sequential
            return inputs;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        public static extern IntPtr GetMessageExtraInfo();


    }
}
