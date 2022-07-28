using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumCSharp
{
    class Common
    {
        public static async Task RFIDtoBarcode(HttpClient api_client, string RFID, IntPtr check_hWnd)
        {
            string json = JsonSerializer.Serialize(new { rfid = RFID, api_key = GlobalVariables.config.api_key });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await api_client.PostAsync(GlobalVariables.config.endpt_rfid2jan, content);
            string resultContent = await result.Content.ReadAsStringAsync();
            ConnectDevice.MyJson objectJson = JsonSerializer.Deserialize<ConnectDevice.MyJson>(resultContent);
            Console.WriteLine("===> MsG API: "+ objectJson.message);
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

        public static void sendKeys(string barcode, IntPtr sub_wm)
        {
            foreach (var c in barcode)
            {
                WindowAPI.PostMessage(sub_wm, (uint)GlobalVariables.WM_KEYDOWN, (int)(System.Enum.Parse(typeof(GlobalVariables.Keys), "D" + c)), (int)(0));
            }
            WindowAPI.PostMessage(sub_wm, (uint)GlobalVariables.WM_KEYDOWN, (int)(System.Enum.Parse(typeof(GlobalVariables.Keys), "Return")), (int)(0));
            Thread.Sleep(GlobalVariables.config.wait_api);
        }
    }
}
