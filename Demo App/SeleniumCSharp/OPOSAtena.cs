using AxOPOSRFIDLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumCSharp
{
    public class OPOS : System.Windows.Forms.UserControl
    {
        public OPOS()
        {
            GlobalVariables.OPOSRFID1.DataEvent += OPOSRFID1_DataEvent;
        }

        public void OPOS_EnableDevice(AxOPOSRFID OPOSRFID1)
        {
            int Result;
            int phase;
            string strData;

            // Open Device
            string device_name = GlobalVariables.config.device_name;
            Result = OPOSRFID1.Open(device_name);
            if (Result != OposStatus.OposSuccess)
            {
                Console.WriteLine("err");
            }

            Result = OPOSRFID1.ClaimDevice(3000);
            if (Result != OposStatus.OposSuccess)
            {
                Console.WriteLine("Err Claim");
                OPOSRFID1.Close();
            }

            OPOSRFID1.DeviceEnabled = true;
            Result = OPOSRFID1.ResultCode;
            if (Result != OposStatus.OposSuccess)
            {
                Console.WriteLine("Err Enable");
                OPOSRFID1.Close();
            }

            //    'DirectIOを用いて現在の位相状態を取得する
            phase = 0;
            strData = "";
            Result = OPOSRFID1.DirectIO(115, ref phase, ref strData);
            OPOSRFID1.BinaryConversion = OposStatus.OposBcNibble;
            //OPOSRFID1.BinaryConversion = OposStatus.OposBcNone;
            Result = OPOSRFID1.ResultCode;
            if (Result != OposStatus.OposSuccess)
            {
                OPOSRFID1.Close();
            }

            OPOSRFID1.ProtocolMask = OposStatus.RfidPrEpc1g2;
            Result = OPOSRFID1.ResultCode;
            if (Result != OposStatus.OposSuccess)
            {
                OPOSRFID1.Close();
            }
        }
        //disable device
        private void OPOS_DisableDevice(AxOPOSRFID OPOSRFID1)
        {

        }

        //Scanning
        public void OPOS_StartReading(AxOPOSRFID OPOSRFID1)
        {
            int Result;
            OPOSRFID1.DataEventEnabled = true;
            OPOSRFID1.ReadTimerInterval = GlobalVariables.config.read_interval;


            if (GlobalVariables.OPOSRFID1.TagCount > 0)
            {
                GlobalVariables.OPOSRFID1.ClearInputProperties();
            }

            
            PhaseChange(OPOSRFID1);
            Result = OPOSRFID1.StartReadTags(OposStatus.RfidRtId, "000000000000000000000000", "000000000000000000000000", 0, 0, 1000, "00000000");
            if (Result != OposStatus.OposSuccess)
            {
                Console.WriteLine("read err");
            }
            //OPOSRFID1.DataEventEnabled = true;
            GlobalVariables.isReading = true;
        }

        //Set phase lenght of code
        private void PhaseChange(AxOPOSRFID OPOSRFID1)
        {
            int Result;
            int intData;
            string strData;
            //'DirectIOを使用して位相の有効／無効を制御する
            //'位相を有効にするDirectIOを実行する
            intData = 0;
            strData = "";
            Result = OPOSRFID1.DirectIO(116, ref intData, ref strData);
            if (Result == OposStatus.OposEBusy)
            {
                Console.WriteLine("読み取り中です。StopReadTagsを実行してください");
            }
            else if (Result == OposStatus.OposEIllegal)
            {
                Console.WriteLine("共存できない機能を使用している可能性があります");
            }
            else if (Result != OposStatus.OposSuccess)
            {
                Console.WriteLine("位相設定失敗しました");
            }

        }

        //Stop read
        public void OPOS_StopReading(AxOPOSRFID OPOSRFID1)
        {
            int Result;
            Result = OPOSRFID1.StopReadTags("00000000");
            if (Result != OposStatus.OposSuccess)
            {
                Console.WriteLine("Err Stop");
            }
        }

        private string ConvertTagIDCode(string code_value)
        {
            Dictionary<char, char> nibble_code = new Dictionary<char, char> { { ':', 'A' }, { ';', 'B' }, { '<', 'C' }, { '=', 'D' }, { '>', 'E' }, { '?', 'F' } };
            var stringBuilder = new StringBuilder();
            foreach (var character in code_value)
            {
                if (nibble_code.TryGetValue(character, out var value))
                {
                    stringBuilder.Append(value);
                }
                else
                {
                    stringBuilder.Append(character);
                }
            }
            return stringBuilder.ToString();
        }

        private void OPOSRFID1_DataEvent(object sender, _DOPOSRFIDEvents_DataEventEvent e)
        {
            
            // WindowAPI.PostMessage(GlobalVariables.mWnd, (uint)GlobalVariables.WM_KEYDOWN, (int)(System.Enum.Parse(typeof(GlobalVariables.Keys), "Return")), (int)(0));

            string CurrentTagID;
            int TagCount;
            int LoopCnt;
            string UserData;
            HttpClient api_client = new HttpClient();
            api_client.BaseAddress = new Uri(GlobalVariables.config.api_url);
            api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // TagCount
            TagCount = GlobalVariables.OPOSRFID1.TagCount;

            if (GlobalVariables.clearList && GlobalVariables.rfid_code.Count()>0)
            {
                GlobalVariables.rfid_code.Clear();
                GlobalVariables.checkInterval.Clear();
                GlobalVariables.barcode_check.Clear();
                GlobalVariables.clearList = false;
                
                Console.WriteLine("Clear all list item!");
                Thread.Sleep(GlobalVariables.config.sleep_task);
                //Thread.Sleep(1000);
            }

            for (int i = 0; i < TagCount; i++)
            {
                //Thread.Sleep(GlobalVariables.config.wait_api);
                //var code_value = OPOSRFID1.CurrentTagID;
                UserData = " Userdata=" + GlobalVariables.OPOSRFID1.CurrentTagUserData;

                if (UserData == " Userdata=")
                {
                    UserData = "";
                }
                var code_value = GlobalVariables.OPOSRFID1.CurrentTagID + UserData;
                string new_code = ConvertTagIDCode(code_value);

                //take book out
                if (!GlobalVariables.checkInterval.Contains(new_code))
                {
                    GlobalVariables.checkInterval.Add(new_code);
                }

                //Console.WriteLine("RFID: " + code_value);
                if (!GlobalVariables.rfid_code.Contains(new_code))
                {
                    Console.WriteLine("RFID: "+ new_code);
                    GlobalVariables.rfid_code.Add(new_code);
                    Task.Run(() => RFIDtoBarcode(api_client, new_code, GlobalVariables.main_wd)).Wait();
                }
                //GlobalVariables.resetBtn.ShowDialog();
                //WindowAPI.SetForegroundWindow(GlobalVariables.resetBtn);
                GlobalVariables.OPOSRFID1.NextTag();
            }

            //Thread.Sleep(100);
            // DataEventEnabled=True for next DataEvent
            GlobalVariables.OPOSRFID1.DataEventEnabled = true;
        }

        public static void sendKeys(string barcode, IntPtr sub_wm)
        {
            foreach (var c in barcode)
            {
                WindowAPI.PostMessage(sub_wm, (uint)GlobalVariables.WM_KEYDOWN, (int)(System.Enum.Parse(typeof(GlobalVariables.Keys), "D" + c)), (int)(0));
            }
            WindowAPI.PostMessage(sub_wm, (uint)GlobalVariables.WM_KEYDOWN, (int)(System.Enum.Parse(typeof(GlobalVariables.Keys), "Return")), (int)(0));
            Thread.Sleep(GlobalVariables.config.wait_api);
        }

        //public static List<WebModule.DislayedProduct> getListBarcodeRead(List<string> barcode_list)
        //{
        //    List<WebModule.DislayedProduct> read_code = new List<WebModule.DislayedProduct>();

        //    var frequency = barcode_list.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        //    foreach (var el in frequency)
        //    {
        //        WebModule.DislayedProduct pro = new WebModule.DislayedProduct();
        //        pro.barcode = el.Key.ToString();
        //        pro.count = el.Value.ToString();
        //        read_code.Add(pro);
        //    }
        //    return read_code;
        //}

        public async Task RFIDtoBarcode(HttpClient api_client, string RFID, IntPtr check_hWnd)
        {
            string json = JsonSerializer.Serialize(new { rfid = RFID, api_key = GlobalVariables.config.api_key });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await api_client.PostAsync(GlobalVariables.config.endpt_rfid2jan, content);
            string resultContent = await result.Content.ReadAsStringAsync();
            ConnectDevice.MyJson objectJson = JsonSerializer.Deserialize<ConnectDevice.MyJson>(resultContent);
            if (objectJson.code == "00")
            {
                WindowAPI.SetForegroundWindow(check_hWnd);

                var barcode = objectJson.data["jancode_1"];
                sendKeys(barcode, check_hWnd);
                GlobalVariables.bar_code.Add(barcode);
                GlobalVariables.barcode_rfid bf = new GlobalVariables.barcode_rfid();
                bf.barcode = barcode;
                bf.rfid = RFID;
                GlobalVariables.barcode_check.Add(bf);

                
                //Thread.Sleep(GlobalVariables.config.wait_api);
            }
        }

    }
}
