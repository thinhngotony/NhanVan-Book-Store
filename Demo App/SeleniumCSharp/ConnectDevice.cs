using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;


namespace SeleniumCSharp
{
    class ConnectDevice
    {
        public class MyJson
        {
            public string code { get; set; }
            public Dictionary<string, string> data { get; set; }
            public bool isSuccess { get; set; }
            public string message { get; set; }
        }

        public long MactoLong(string macAddress)
        {
            string hex = macAddress.Replace(":", "");
            return Convert.ToInt64(hex, 16);
        }

        public async Task BluetoothConnector(IntPtr check_hWnd)
        {
            BluetoothClient client = new BluetoothClient();

            long mac_adr = MactoLong(GlobalVariables.config.mac_adress);
            BluetoothAddress adr = new BluetoothAddress(mac_adr);
            BluetoothDeviceInfo device = new BluetoothDeviceInfo(adr);

            HttpClient api_client = new HttpClient();
            api_client.BaseAddress = new Uri(GlobalVariables.config.api_url);
            api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            if (!device.Authenticated)
            {
                BluetoothSecurity.PairRequest(adr, "1234");
            }

            device.Refresh();
            System.Diagnostics.Debug.WriteLine(device.Authenticated);
            client.Connect(device.DeviceAddress, BluetoothService.SerialPort);

            var stream = client.GetStream();

            StreamReader sr = new StreamReader(stream);
            //string line;
            while (true)
            {
                string line = sr.ReadLine();
                //Console.WriteLine("Line: "+line);
                if (line.Contains("~eT") && line != null)
                {


                    string code_value = line.Replace("~eT", "");
                    ////code_value = code_value.Substring(4);
                    if (!GlobalVariables.rfid_code.Contains(code_value))
                    {
                        GlobalVariables.rfid_code.Add(code_value);
                        //Console.WriteLine("RFID: "+code_value);
                        try
                        {
                        //    string json = JsonSerializer.Serialize(new { rfid = code_value, api_key = GlobalVariables.config.api_key });
                        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
                        //    var result = await api_client.PostAsync(GlobalVariables.config.endpt_rfid2jan, content);
                        //    string resultContent = await result.Content.ReadAsStringAsync();
                        //    MyJson objectJson = JsonSerializer.Deserialize<MyJson>(resultContent);
                        //    if (objectJson.code == "00")
                        //    {
                        //        var barcode = objectJson.data["jancode_1"];
                        //        sendKeys(barcode, check_hWnd);
                        //        GlobalVariables.bar_code.Add(barcode);
                        //        Thread.Sleep(GlobalVariables.config.wait_api);
                        //    }
                            Task api_task = Task.Run(() => RFIDtoBarcode(api_client, code_value, check_hWnd));
                            api_task.Wait();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                    }
                  

                }
                else
                {
                    if (line.Contains("~Af") && sr.ReadLine().Contains("~As"))
                    {
                        string message = "Can't read RFID code from scanner. Please try again or use Barcode scanner!";
                        //MessageBox.Show(message, "WARNING");
                    }
                }

                if (device.Connected == true)
                {
                    break;
                }
            }
            client.Close();
        }

        public static void sendKeys(string barcode, IntPtr sub_wm)
        {
            foreach (var c in barcode)
            {
                WindowAPI.PostMessage(sub_wm, (uint)GlobalVariables.WM_KEYDOWN, (int)(System.Enum.Parse(typeof(GlobalVariables.Keys), "D" + c)), (int)(0));
            }
            WindowAPI.PostMessage(sub_wm, (uint)GlobalVariables.WM_KEYDOWN, (int)(System.Enum.Parse(typeof(GlobalVariables.Keys), "Return")), (int)(0));
        }

       public static List<WebModule.DislayedProduct> getListBarcodeRead(List <string> barcode_list)
        {
            List<WebModule.DislayedProduct> read_code = new List<WebModule.DislayedProduct>();

            var frequency = barcode_list.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            foreach(var el in frequency)
            {
                WebModule.DislayedProduct pro = new WebModule.DislayedProduct();
                pro.barcode = el.Key.ToString();
                pro.count = el.Value.ToString();
                read_code.Add(pro);
            }
            return read_code;
        }

        public async Task RFIDtoBarcode(HttpClient api_client, string RFID, IntPtr check_hWnd)
        {
            string json = JsonSerializer.Serialize(new { rfid = RFID, api_key = GlobalVariables.config.api_key });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await api_client.PostAsync(GlobalVariables.config.endpt_rfid2jan, content);
            string resultContent = await result.Content.ReadAsStringAsync();
            MyJson objectJson = JsonSerializer.Deserialize<MyJson>(resultContent);
            if (objectJson.code == "00")
            {
                var barcode = objectJson.data["jancode_1"];
                sendKeys(barcode, check_hWnd);
                GlobalVariables.bar_code.Add(barcode);
                Thread.Sleep(GlobalVariables.config.wait_api);
            }
        }
    }
}
