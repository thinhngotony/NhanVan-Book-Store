using com.gigatms;
using com.gigatms.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelfRegi_V2
{
    public partial class frmMain : Form
    {
        //public static Host _host = new Host();
        //public static TS800 _ts800 = new TS800(3, 3);
        //public static List<string> _tagList = new List<string>();
        //public bool m_bIsInventoryProcessing = false;

        public frmMain()
        {
            InitializeComponent();
            //ConnectDevice(Session._ts800, Session._host);
            //StartInventory(Session._ts800);
            //deviceBindingEvent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //ConnectDevice(Session._ts800, Session._host);
            //StartInventory(Session._ts800);
            //deviceBindingEvent();
        }

        private void deviceBindingEvent()
        {
            Session._ts800.OnTagPresented += _ts800_OnTagPresented;
        }

        private void _ts800_OnTagPresented(object sender, com.gigatms.Parameters.TagInformationFormat tagInformation)
        {
            //data_event

           
            string szPCEPC = tagInformation.PcEpcHex;
            string szTID = tagInformation.TidHex;

            string rfid = szPCEPC;
            if (true)
            {
                rfid = rfid.Substring(4, rfid.Length - 4);

            }
            Console.WriteLine(rfid);

            
        }

        private void frmMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Session.m_bIsInventoryProcessing)
                e.Handled = true;
        }

        public bool ConnectDevice(TS800 _ts800, Host _host)
        {
            string szBaudrate;
            string szPort;
            string szFirmware;

            //for (int i = 0; i <= (_host.GetPortCount() - 1); i++)
            //{
            //    Console.WriteLine(_host.GetPortName(i));
            //}

            _host.NetDeviceSearcherEnabled = false;
            if (_ts800.IsConnected)
            {
                _ts800.Disconnect();
            }

            _ts800.DeviceAddress = 0;
            szPort = "USB1 (TS800)"; //_host.GetPortName(i);
            szBaudrate = "AUTO";
            bool result = _ts800.Connect(szPort, szBaudrate);
            if (result)
            {
                _ts800.StopInventory();
                szFirmware = _ts800.GetFirmwareVersion();
                Console.WriteLine("CONNECT DEVICE: ===> Device connceted!");
                return true;
            }
            else
            {
                Console.WriteLine("CONNECT DEVICE: ===> Failed to connect device!");
                return false;
            }

        }

        public void ConnectDeviceOLD(TS800 _ts800, Host _host)
        {
            string szBaudrate;
            string szPort;
            string szFirmware;

            //for (int i = 0; i <= (_host.GetPortCount() - 1); i++)
            //{
            //    Console.WriteLine(_host.GetPortName(i));
            //}

            _host.NetDeviceSearcherEnabled = false;
            if (_ts800.IsConnected)
            {
                _ts800.Disconnect();
            }

            _ts800.DeviceAddress = 0;
            szPort = "USB1 (TS800)"; //_host.GetPortName(i);
            szBaudrate = "AUTO";
            bool result = _ts800.Connect(szPort, szBaudrate);
            if (result)
            {
                _ts800.StopInventory();
                szFirmware = _ts800.GetFirmwareVersion();
                Console.WriteLine("CONNECT DEVICE: ===> Device connceted!");
            }
            else
            {
                Console.WriteLine("CONNECT DEVICE: ===> Failed to connect device!");
            }

        }

        public void DisConnectDevice(TS800 _ts800)
        {
            if (_ts800.IsConnected)
            {
                if (_ts800.Disconnect())
                {
                    Console.WriteLine("DISCONNECT DEVICE: ===> Device disconnected!");
                }
                else
                {
                    Console.WriteLine("DISCONNECT DEVICE: ===> Failed to disconnect device!");
                }
            }
        }

        public bool StartInventory(TS800 _ts800)
        {
            bool result;
            TagPresentedType tagPresetnedType;
            tagPresetnedType = TagPresentedType.PC_EPC_TID;
            result = _ts800.StartInventory(tagPresetnedType);

            if (result)
            {
                Session.m_bIsInventoryProcessing = true;
                Console.WriteLine("INVENTORY: ===> Start inventory!");
                return true;
            }
            else
            {
                Console.WriteLine("INVENTORY: ===> Failed to start inventory!");
                return false;
            }
        }
        public void StartInventoryOLD(TS800 _ts800)
        {
            bool result;
            TagPresentedType tagPresetnedType;
            tagPresetnedType = TagPresentedType.PC_EPC_TID;
            result = _ts800.StartInventory(tagPresetnedType);

            if (result)
            {
                Session.m_bIsInventoryProcessing = true;
                Console.WriteLine("INVENTORY: ===> Start inventory!");
            }
            else
            {
                Console.WriteLine("INVENTORY: ===> Failed to start inventory!");
            }
        }

        public static void ReadData(TS800 _ts800)
        {
            byte[] dataByteArray = _ts800.ReadTag("00000000", com.gigatms.Parameters.MemoryBank.TID, 0, 0);
            if (dataByteArray != null)
            {
                string szReadData = ByteToCode(dataByteArray);
                Console.WriteLine("====READ: " + szReadData);
            }
            //Console.ReadKey();
        }

        public static string ByteToCode(byte[] dataByteArray)
        {
            string szReadData = "";
            for (var i = 0; i <= (dataByteArray.Length - 1); i++)
                szReadData = szReadData + dataByteArray[i].ToString("X02") + "";
            return szReadData;
        }
    }
}
