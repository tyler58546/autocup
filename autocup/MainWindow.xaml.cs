using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace autocup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        uint status = 0;
        // STATUS CODES:
        // 0 - Disabled
        // 1 - Countdown
        // 2 - Running

        uint countdown = 5;
        System.Timers.Timer myTimer = new System.Timers.Timer();
        ulong cups = 0;
        int interval = 6500;

        public MainWindow()
        {
            InitializeComponent();
            myTimer.Elapsed += new ElapsedEventHandler(TimerFunc);
            myTimer.Interval = 1000;
        }

        public void ShowErrorMessage(string text)
        {
            text = "you fool. you absolute buffoon. you think you can challenge me in my own realm? you think you can rebel against my authority? you dare come into my house and upturn my dining chairs and spill coffee grounds in my Keurig? you thought you were safe in your chain mail armor behind that screen of yours. I will take these laminate wood floor boards and destroy you. I didn’t want war. but i didn’t start it.\n\n" + text;
            this.Dispatcher.Invoke(() =>
            {
                System.Windows.MessageBox.Show(text);
            });
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            //Toggle button clicked
            if (status == 0)
            {
                try
                {
                    interval = Convert.ToInt32(Convert.ToDouble(delayTextField.Text) * 1000);
                }
                catch (FormatException)
                {
                    ShowErrorMessage("delay must be a number between 1 and 60.");
                    return;
                }
                catch (OverflowException)
                {
                    ShowErrorMessage("delay must be a number between 1 and 60.");
                    return;
                }
                if (interval < 1000 || interval > 60000)
                {
                    ShowErrorMessage("delay must be a number between 1 and 60.");
                    return;
                }

                //Start the countdown
                myTimer.Interval = 1000;
                myTimer.Start();
                status = 1;
                countdown = 5;
                UpdateCountdown();
                countdown--;
                toggleButton.Content = "Stop";
                delayTextField.IsEnabled = false;
                return;
            }
            if (status == 1 || status == 2)
            {
                //Stop
                myTimer.Stop();
                status = 0;
                toggleButton.Content = "Start";
                statusLabel.Content = "Ready";
                delayTextField.IsEnabled = true;
            } 
        }

        private void TimerFunc(object sender, ElapsedEventArgs e)
        {
            switch (status)
            {
                case 0:
                    myTimer.Stop();
                    break;
                case 1:
                    if (countdown > 0)
                    {
                        UpdateCountdown();
                        countdown--;
                    } else
                    {
                        status = 2;
                        UpdateCountdown();
                        myTimer.Interval = interval;
                        this.Dispatcher.Invoke(() =>
                        {
                            cup();
                        });
                        cups++;
                        UpdateCount();
                    }
                    break;
                case 2:
                    this.Dispatcher.Invoke(() =>
                    {
                        cup();
                    });
                    cups++;
                    UpdateCount();
                    break;
                default:
                    throw new Exception();
            }
        }

        private void UpdateCountdown()
        {
            this.Dispatcher.Invoke(() =>
            {
                statusLabel.Content = "Starting in " + countdown + "...";
                if (status == 2)
                {
                    statusLabel.Content = "Running";
                }
            });
        }
        private void UpdateCount()
        {
            this.Dispatcher.Invoke(() =>
            {
                statusLabel.Content = "Running";
                cupsLabel.Content = cups.ToString();
            });
        }

        public static void cup()
        {
            Console.WriteLine("cup");
            SendKeyBoradKey((short)Keys.Back);
            SendKeyBoradKey((short)Keys.Back);
            SendKeyBoradKey((short)Keys.Back);
            SendUnicode("cup");
            SendKeyBoradKey((short)Keys.Enter);
        }

        [DllImport("user32.dll")]
        private static extern UInt32 SendInput(UInt32 nInputs, ref INPUT pInputs, int cbSize);
        [DllImport("user32.dll", EntryPoint = "GetMessageExtraInfo", SetLastError = true)]
        private static extern IntPtr GetMessageExtraInfo();
        private enum InputType
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2,
        }
        [Flags()]
        private enum MOUSEEVENTF
        {
            MOVE = 0x0001,  //mouse move     
            LEFTDOWN = 0x0002,  //left button down     
            LEFTUP = 0x0004,  //left button up     
            RIGHTDOWN = 0x0008,  //right button down     
            RIGHTUP = 0x0010,  //right button up     
            MIDDLEDOWN = 0x0020, //middle button down     
            MIDDLEUP = 0x0040,  //middle button up     
            XDOWN = 0x0080,  //x button down     
            XUP = 0x0100,  //x button down     
            WHEEL = 0x0800,  //wheel button rolled     
            VIRTUALDESK = 0x4000,  //map to entire virtual desktop     
            ABSOLUTE = 0x8000,  //absolute move     
        }
        [Flags()]
        private enum KEYEVENTF
        {
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            UNICODE = 0x0004,
            SCANCODE = 0x0008,
        }
        [StructLayout(LayoutKind.Explicit)]
        private struct INPUT
        {
            [FieldOffset(0)]
            public Int32 type;//0-MOUSEINPUT;1-KEYBDINPUT;2-HARDWAREINPUT     
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public Int32 dx;
            public Int32 dy;
            public Int32 mouseData;
            public Int32 dwFlags;
            public Int32 time;
            public IntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public Int16 wVk;
            public Int16 wScan;
            public Int32 dwFlags;
            public Int32 time;
            public IntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public Int32 uMsg;
            public Int16 wParamL;
            public Int16 wParamH;
        }
        //Simulate mouse   
        public static void Click()
        {
            INPUT input_down = new INPUT();
            input_down.mi.dx = 0;
            input_down.mi.dy = 0;
            input_down.mi.mouseData = 0;
            input_down.mi.dwFlags = (int)MOUSEEVENTF.LEFTDOWN;
            SendInput(1, ref input_down, Marshal.SizeOf(input_down));
            INPUT input_up = input_down;
            input_up.mi.dwFlags = (int)MOUSEEVENTF.LEFTUP;
            SendInput(1, ref input_up, Marshal.SizeOf(input_up));
        }
        //Simulate keystrokes  Send unicode characters to send any character
        public static void SendUnicode(string message)
        {
            for (int i = 0; i < message.Length; i++)
            {
                INPUT input_down = new INPUT();
                input_down.type = (int)InputType.INPUT_KEYBOARD;
                input_down.ki.dwFlags = (int)KEYEVENTF.UNICODE;
                input_down.ki.wScan = (short)message[i];
                input_down.ki.wVk = 0;
                SendInput(1, ref input_down, Marshal.SizeOf(input_down));//keydown     
                INPUT input_up = new INPUT();
                input_up.type = (int)InputType.INPUT_KEYBOARD;
                input_up.ki.wScan = (short)message[i];
                input_up.ki.wVk = 0;
                input_up.ki.dwFlags = (int)(KEYEVENTF.KEYUP | KEYEVENTF.UNICODE);
                SendInput(1, ref input_up, Marshal.SizeOf(input_up));//keyup      
            }
        }
        //Simulate keystrokes 
        public static void SendKeyBoradKey(short key)
        {
            INPUT input_down = new INPUT();
            input_down.type = (int)InputType.INPUT_KEYBOARD;
            input_down.ki.dwFlags = 0;
            input_down.ki.wVk = key;
            SendInput(1, ref input_down, Marshal.SizeOf(input_down));//keydown     

            INPUT input_up = new INPUT();
            input_up.type = (int)InputType.INPUT_KEYBOARD;
            input_up.ki.wVk = key;
            input_up.ki.dwFlags = (int)KEYEVENTF.KEYUP;
            SendInput(1, ref input_up, Marshal.SizeOf(input_up));//keyup      

        }
        //Send non-unicode characters, only send lowercase letters and numbers (发送非unicode字符，只能发送小写字母和数字)     
        public static void SendNoUnicode(string message)
        {
            string str = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < message.Length; i++)
            {
                short sendChar = 0;
                if (str.IndexOf(message[i].ToString().ToLower()) > -1)
                    sendChar = (short)GetKeysByChar(message[i]);
                else
                    sendChar = (short)message[i];
                INPUT input_down = new INPUT();
                input_down.type = (int)InputType.INPUT_KEYBOARD;
                input_down.ki.dwFlags = 0;
                input_down.ki.wVk = sendChar;
                SendInput(1, ref input_down, Marshal.SizeOf(input_down));//keydown     
                INPUT input_up = new INPUT();
                input_up.type = (int)InputType.INPUT_KEYBOARD;
                input_up.ki.wVk = sendChar;
                input_up.ki.dwFlags = (int)KEYEVENTF.KEYUP;
                SendInput(1, ref input_up, Marshal.SizeOf(input_up));//keyup      
            }
        }
        private static Keys GetKeysByChar(char c)
        {
            string str = "abcdefghijklmnopqrstuvwxyz";
            int index = str.IndexOf(c.ToString().ToLower());
            switch (index)
            {
                case 0:
                    return Keys.A;
                case 1:
                    return Keys.B;
                case 2:
                    return Keys.C;
                case 3:
                    return Keys.D;
                case 4:
                    return Keys.E;
                case 5:
                    return Keys.F;
                case 6:
                    return Keys.G;
                case 7:
                    return Keys.H;
                case 8:
                    return Keys.I;
                case 9:
                    return Keys.J;
                case 10:
                    return Keys.K;
                case 11:
                    return Keys.L;
                case 12:
                    return Keys.M;
                case 13:
                    return Keys.N;
                case 14:
                    return Keys.O;
                case 15:
                    return Keys.P;
                case 16:
                    return Keys.Q;
                case 17:
                    return Keys.R;
                case 18:
                    return Keys.S;
                case 19:
                    return Keys.T;
                case 20:
                    return Keys.U;
                case 21:
                    return Keys.V;
                case 22:
                    return Keys.W;
                case 23:
                    return Keys.X;
                case 24:
                    return Keys.Y;
                default:
                    return Keys.Z;
            }
        }
    }
    
}
