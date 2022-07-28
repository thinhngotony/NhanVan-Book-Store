using com.gigatms;
using com.gigatms.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeleniumCSharp
{
    public partial class frmMain : Form
    {
        public static Host _host = new Host();
        public static TS800 _ts800 = new TS800(3, 3);
        public static List<string> _tagList = new List<string>();
        public bool m_bIsInventoryProcessing = false;

        public frmMain()
        {
            InitializeComponent();
            ConnectDevice(_ts800, _host);
            StartInventory(_ts800);
            deviceBindingEvent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            ConnectDevice(_ts800, _host);
            StartInventory(_ts800);
            deviceBindingEvent();
        }

        private void deviceBindingEvent()
        {
            _ts800.OnTagPresented += _ts800_OnTagPresented;
        }

        private void _ts800_OnTagPresented(object sender, com.gigatms.Parameters.TagInformationFormat tagInformation)
        {
            //data_event

            HttpClient api_client = new HttpClient();
            api_client.BaseAddress = new Uri(GlobalVariables.config.api_url);
            api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string szPCEPC = tagInformation.PcEpcHex;
            string szTID = tagInformation.TidHex;

            string rfid = szPCEPC;
            if (true)
            {
                rfid = rfid.Substring(4, rfid.Length - 4);

            }

            if (!GlobalVariables.checkInterval.Contains(rfid))
            {
                GlobalVariables.checkInterval.Add(rfid);
            }

            if (!GlobalVariables.rfid_code.Contains(rfid))
            {
                _tagList.Add(rfid);
                GlobalVariables.rfid_code.Add(rfid);
                Task.Run(() => Common.RFIDtoBarcode(api_client, rfid, GlobalVariables.main_wd)).Wait();

                Console.WriteLine(rfid);
            }
        }

        private void frmMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (m_bIsInventoryProcessing)
                e.Handled = true;
        }

        public void ConnectDevice(TS800 _ts800, Host _host)
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

        public void StartInventory(TS800 _ts800)
        {
            bool result;
            TagPresentedType tagPresetnedType;
            tagPresetnedType = TagPresentedType.PC_EPC_TID;
            result = _ts800.StartInventory(tagPresetnedType);

            if (result)
            {
                m_bIsInventoryProcessing = true;
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
