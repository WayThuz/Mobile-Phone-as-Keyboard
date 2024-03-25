using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VirtualKeyboard {
    public class InputAnalyzer {
        public static void AnalyzeInputs(string msg, out INPUT[] result) {
            result = new INPUT[msg.Length*2];
            for(int i = 0; i < msg.Length; i++) {
                result[2*i] = CreateKeyDownInput(msg[i]);
                result[2*i + 1] = CreateKeyUpInput(msg[i]);
            }
        }

        private static INPUT CreateKeyDownInput(char c) {
            INPUT keyDown = new INPUT {
                type = (int)InputType.Keyboard,
                u = new InputUnion {
                    ki = new KeyboardInput {
                        wVk = 0,
                        //wScan = GetKeyCode(c),
                        wScan = GetKeyCode(c),
                        dwFlags = (uint)(KeyEventF.KeyDown | KeyEventF.Scancode),
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            };
            return keyDown;
        }   
        
        private static INPUT CreateKeyUpInput(char c) {
            INPUT keyUp = new INPUT {
                type = (int)InputType.Keyboard,
                u = new InputUnion {
                    ki = new KeyboardInput {
                        wVk = 0,
                        wScan = GetKeyCode(c),
                        dwFlags = (uint)(KeyEventF.KeyUp | KeyEventF.Scancode),
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            };
            return keyUp;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetMessageExtraInfo();

        private static ushort GetKeyCode(char c) {
            c = char.ToLower(c);
            return c switch {
                '0' => 0x0b,
                '1' => 0x02,
                '2' => 0x03,
                '3' => 0x04,
                '4' => 0x05,
                '5' => 0x06,
                '6' => 0x07,
                '7' => 0x08,
                '8' => 0x09,
                '9' => 0x0a,
                'a' => 0x1e,
                'b' => 0x30,
                'c' => 0x2e,
                'd' => 0x20,
                'e' => 0x12,
                'f' => 0x21,
                'g' => 0x22,
                'h' => 0x23,
                'i' => 0x17,
                'j' => 0x24,
                'k' => 0x25,
                'l' => 0x26,
                'm' => 0x32,
                'n' => 0x31,
                'o' => 0x18,
                'p' => 0x19,
                'q' => 0x10,
                'r' => 0x13,
                's' => 0x1f,
                't' => 0x14,
                'u' => 0x16,
                'v' => 0x2f,
                'w' => 0x11,
                'x' => 0x2d,
                'y' => 0x15,
                'z' => 0x2c,
                _ => throw new NotImplementedException($"Not Implement Character: {c}.")
            };
        }

    }
}
