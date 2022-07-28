using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumCSharp
{
    class GlobalVariables
    {
        public struct Config
        {
            public string mac_adress { get; set; }
            public string api_url { get; set; }
            public string endpt_rfid2jan { get; set; }
            public string api_key { get; set; }
            public string haravan_url { get; set; }
            public string haravan_user_name { get; set; }
            public int wait_api { get; set; }
            public int sleep_task { get; set; }
            public int read_interval { get; set; }
            public string device_name { get; set; }
            public int count_miss { get; set; }
            public string usr_name { get; set; }
            public string usr_pwd { get; set; }
        }

        public struct barcode_rfid
        {
            public string barcode;
            public string rfid;
        }

        public static Config config = new Config();

        public static uint WM_KEYDOWN = 0x0100;

        public static AxOPOSRFIDLib.AxOPOSRFID OPOSRFID1 = new AxOPOSRFIDLib.AxOPOSRFID();
        public static OPOS opos;
        public static bool addItem = false;
        public static IntPtr main_wd = IntPtr.Zero;
        public static bool clearList = false;
        public enum Keys : Int32
        {
            //0 key
            D0 = 0x30,
            //1 key
            D1 = 0x31,
            //2 key
            D2 = 0x32,
            //3 key
            D3 = 0x33,
            //4 key
            D4 = 0x34,
            //5 key
            D5 = 0x35,
            //6 key
            D6 = 0x36,
            //7 key
            D7 = 0x37,
            //8 key
            D8 = 0x38,
            //9 key
            D9 = 0x39,
            // - key
            KEY_MINUS = 0xBD,
            // + key
            KEY_PLUS = 0xBB,
            //A key
            A = 0x41,
            //B key
            B = 0x42,
            //C key
            C = 0x43,
            //D key
            D = 0x44,
            //E key
            E = 0x45,
            //F key
            F = 0x46,
            //G key
            G = 0x47,
            //H key
            H = 0x48,
            //I key
            I = 0x49,
            //J key
            J = 0x4A,
            //L key
            L = 0x4C,
            //K key
            K = 0x4B,
            //M key
            M = 0x4D,
            //N key
            N = 0x4E,
            //O key
            O = 0x4F,
            //P key
            P = 0x50,
            //Q key
            Q = 0x51,
            //R key
            R = 0x52,
            //S key
            S = 0x53,
            //T key
            T = 0x54,
            //U key
            U = 0x55,
            //V key
            V = 0x56,
            //W key
            W = 0x57,
            //X key
            X = 0x58,
            //Y key
            Y = 0x59,
            //Z key
            Z = 0x5A,
            //Left mouse button
            KEY_LBUTTON = 0x01,
            //Right mouse button
            KEY_RBUTTON = 0x02,
            //Control-break processing
            KEY_CANCEL = 0x03,
            //Middle mouse button (three-button mouse)
            KEY_MBUTTON = 0x04,
            //BACKSPACE key  
            Back = 0x08,
            //TAB key
            Tab = 0x09,
            //CLEAR key
            KEY_CLEAR = 0x0C,
            //ENTER key
            Return = 0x0D,
            //SHIFT key
            KEY_SHIFT = 0x10,
            //CTRL key
            LeftCtrl = 0x11,
            RightCtrl = 0x11,
            //ALT key
            System = 0x12,
            LWin = 0x12,
            //PAUSE key
            KEY_PAUSE = 0x13,
            //CAPS LOCK key
            KEY_CAPITAL = 0x14,
            //ESC key
            Escape = 0x1B,
            //SPACEBAR
            Space = 0x20,
            //PAGE UP key
            KEY_PRIOR = 0x21,
            //PAGE DOWN key
            KEY_NEXT = 0x22,
            //END key
            KEY_END = 0x23,
            //HOME key
            KEY_HOME = 0x24,
            //LEFT ARROW key
            KEY_LEFT = 0x25,
            //UP ARROW key
            Up = 0x26,
            //RIGHT ARROW key
            Right = 0x27,
            //DOWN ARROW key
            Down = 0x28,
            //SELECT key
            KEY_SELECT = 0x29,
            //PRINT key
            KEY_PRINT = 0x2A,
            //EXECUTE key
            KEY_EXECUTE = 0x2B,
            //PRINT SCREEN key
            KEY_SNAPSHOT = 0x2C,
            //INS key
            KEY_INSERT = 0x2D,
            //DEL key
            KEY_DELETE = 0x2E,
            //HELP key
            KEY_HELP = 0x2F,
            //Numeric keypad 0 key
            NumPad0 = 0x60,
            //Numeric keypad 1 key
            NumPad1 = 0x61,
            //Numeric keypad 2 key
            NumPad2 = 0x62,
            //Numeric keypad 3 key  
            NumPad3 = 0x63,
            //Numeric keypad 4 key  
            NumPad4 = 0x64,
            //Numeric keypad 5 key  
            NumPad5 = 0x65,
            //Numeric keypad 6 key  
            NumPad6 = 0x66,
            //Numeric keypad 7 key
            NumPad7 = 0x67,
            //Numeric keypad 8 key  
            NumPad8 = 0x68,
            //Numeric keypad 9 key  
            NumPad9 = 0x69,
            //Separator key
            KEY_SEPARATOR = 0x6C,
            //Subtract key
            KEY_SUBTRACT = 0x6D,
            //Decimal key
            KEY_DECIMAL = 0x6E,
            //Divide key
            KEY_DIVIDE = 0x6F,
            //F1 key
            KEY_F1 = 0x70,
            //F2 key
            KEY_F2 = 0x71,
            //F3 key
            KEY_F3 = 0x72,
            //F4 key
            KEY_F4 = 0x73,
            //F5 key
            KEY_F5 = 0x74,
            //F6 key
            KEY_F6 = 0x75,
            //F7 key
            KEY_F7 = 0x76,
            //F8 key
            KEY_F8 = 0x77,
            //F9 key
            KEY_F9 = 0x78,
            //F10 key
            KEY_F10 = 0x79,
            //F11 key
            KEY_F11 = 0x7A,
            //F12 key
            KEY_F12 = 0x7B,
            //SCROLL LOCK key
            KEY_SCROLL = 0x91,
            //Left SHIFT key
            KEY_LSHIFT = 0xA0,
            //Right SHIFT key
            KEY_RSHIFT = 0xA1,
            //Left CONTROL key
            KEY_LCONTROL = 0xA2,
            //Right CONTROL key
            KEY_RCONTROL = 0xA3,
            //Left MENU key
            KEY_LMENU = 0xA4,
            //Right MENU key
            KEY_RMENU = 0xA5,
            //, key
            KEY_COMMA = 0xBC,
            //. key
            KEY_PERIOD = 0xBE,
            //Play key
            KEY_PLAY = 0xFA,
            //Zoom key
            KEY_ZOOM = 0xFB,
            NumLock = 0x0,
            Null = 0x0,
        }

        public static List<string> bar_code = new List<string>();
        public static List<string> rfid_code = new List<string>();
        public static List<string> checkInterval = new List<string>();
        public static List<barcode_rfid> barcode_check = new List<barcode_rfid>();
        public static List<WebModule.DislayedProduct> onscreen_items = new List<WebModule.DislayedProduct>();
        public static Task checkchange;
        public static int num_tab_open;
        public static bool isReading = false;

        public static ResetButton resetBtn = null;
        public static WebDriver main_driver;

    }
}
