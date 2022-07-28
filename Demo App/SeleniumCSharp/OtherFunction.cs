using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SeleniumCSharp.WebModule;
using OpenQA.Selenium.Support.Events;
using System.Drawing;

namespace SeleniumCSharp
{
    class OtherFunction
    {
       

        public static void ReadConfig(string filepath)
        {
            Dictionary<string, string> dataInFile = getDictionaryConfig("CONFIG.ini");
            List<string> listKey = new List<string>(new string[] { "api_key", "api_url", "endpt_rfid2jan", "mac_adress", "haravan_url", "haravan_user_name" });
            foreach (string key in listKey)
            {
                if (!dataInFile.ContainsKey(key))
                {
                    Environment.Exit(0);
                }
            }

            GlobalVariables.config.api_key = dataInFile["api_key"];
            GlobalVariables.config.api_url = dataInFile["api_url"];
            GlobalVariables.config.endpt_rfid2jan = dataInFile["endpt_rfid2jan"];
            GlobalVariables.config.mac_adress = dataInFile["mac_adress"];
            GlobalVariables.config.haravan_url = dataInFile["haravan_url"];
            GlobalVariables.config.haravan_user_name = dataInFile["haravan_user_name"];
            GlobalVariables.config.wait_api = Int32.Parse(dataInFile["wait_api"]);
            GlobalVariables.config.sleep_task = Int32.Parse(dataInFile["sleep_task"]);
            GlobalVariables.config.read_interval = Int32.Parse(dataInFile["read_interval"]);
            GlobalVariables.config.count_miss = Int32.Parse(dataInFile["count_miss"]);
            GlobalVariables.config.device_name = dataInFile["device_name"];
            GlobalVariables.config.usr_name = dataInFile["usr_name"];
            GlobalVariables.config.usr_pwd = dataInFile["usr_pwd"];
        }


        public static Dictionary<string, string> getDictionaryConfig(string path)
        {
            Dictionary<string, string> Config = new Dictionary<string, string>();
            List<string> result = read_Fileini(path);
            foreach (string line in result)
            {
                if (!line.Contains("="))
                {
                    Environment.Exit(0);
                }
                string[] temp = line.Split('=');
                Config[temp[0]] = temp[1];
            }
            return Config;
        }

        public static List<string> read_Fileini(string path)
        {
            List<string> result = new List<string>();
            if (!File.Exists(path))
            {
                Environment.Exit(0);
            }
            else
                try
                {
                    StreamReader sr = new StreamReader(path);
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        result.Add(line);
                        line = sr.ReadLine();
                    }
                    sr.Close();
                }
                catch (Exception e)
                {
                    Environment.Exit(0);
                }
            //replace all space
            int size = result.Count;
            for (int i = 0; i < size; i++)
                result[i] = result[i].Replace(" ", "");
            return result;

        }
        public static async Task StartReadTag()
        {
            GlobalVariables.opos = new OPOS();
            GlobalVariables.OPOSRFID1.CreateControl();
            GlobalVariables.opos.OPOS_EnableDevice(GlobalVariables.OPOSRFID1);
            GlobalVariables.opos.OPOS_StartReading(GlobalVariables.OPOSRFID1);
        }

        public static Task CheckNoneItem(IWebDriver driver)
        {

            //GlobalVariables.opos.OPOS_StartReading(GlobalVariables.OPOSRFID1);
            while (true)
            {
                Thread.Sleep(GlobalVariables.config.sleep_task);

                try
                {
                    IWebElement tab_container = driver.FindElement(By.Id("tab-container"));
                    int tab_opening = tab_container.FindElements(By.TagName("li")).Count();
                    if (tab_opening > GlobalVariables.num_tab_open)
                    {
                        GlobalVariables.clearList = true;
                    }
                    GlobalVariables.num_tab_open = tab_opening;
                }
                catch { }
               
            }
        }

       
        public static Task StartScan()
        {
            GlobalVariables.opos.OPOS_StartReading(GlobalVariables.OPOSRFID1);
            while (true)
            {
                Thread.Sleep(1000);
                Thread.Sleep(GlobalVariables.config.sleep_task);
                //Console.WriteLine("LOG====> :"+driver.Manage().Logs.GetLog(LogType.Browser)[-1].ToString());
                Process[] chrome_process = Process.GetProcessesByName("chrome");

                if (chrome_process.Count() == 0)
                {
                    Process main_process = Process.GetProcessesByName("RFIDBookStore").First();
                    //GlobalVariables.opos.OPOS_StopReading(GlobalVariables.OPOSRFID1);
                    main_process.Kill();
                    main_process.WaitForExit();
                    main_process.Dispose();
                }
            }
        }

        public static Task CheckPaymentPopup(IWebDriver driver)
        {
            while (true)
            {
                Thread.Sleep(GlobalVariables.config.sleep_task);
                try
                {
                    IWebElement payment_popup = driver.FindElement(By.Id("order.create.success_popup.new_button"));                    
                    GlobalVariables.opos.OPOS_StopReading(GlobalVariables.OPOSRFID1);
                    GlobalVariables.clearList = true;
                    GlobalVariables.isReading = false;
                    //update log API
                }
                catch
                {
                    if(GlobalVariables.isReading == false)
                    {
                        //GlobalVariables.opos.OPOS_StartReading(GlobalVariables.OPOSRFID1);
                    }
                }
            }
        }

        public static Task isBrowserClose(IWebDriver driver)
        {
            while (true)
            {
                Thread.Sleep(GlobalVariables.config.sleep_task);
                //Console.WriteLine("LOG====> :"+driver.Manage().Logs.GetLog(LogType.Browser)[-1].ToString());
                Process[] chrome_process = Process.GetProcessesByName("chrome");

                if (chrome_process.Count() == 0)
                {
                    Process main_process = Process.GetProcessesByName("RFIDBookStore").First();
                   // GlobalVariables.opos.OPOS_StopReading(GlobalVariables.OPOSRFID1);
                    main_process.Kill();
                    main_process.WaitForExit();
                    main_process.Dispose();
                }
            }
        } 

        public static Task checkChangeProduct(IWebDriver driver)
        {
            double timestamp = Stopwatch.GetTimestamp();
            double start_time = timestamp / Stopwatch.Frequency;
            int count_miss = 0;
            int count_removeall = 0;

            while (true)
            {
                Thread.Sleep(GlobalVariables.config.sleep_task);

                timestamp = Stopwatch.GetTimestamp();
                double process_time = timestamp / Stopwatch.Frequency;
                //Console.WriteLine("Check change ===> Time: "+ (int)(process_time - start_time));

                if ((int)(process_time - start_time) % 1 == 0)
                {
                    GlobalVariables.rfid_code.Sort();
                    GlobalVariables.checkInterval.Sort();

                    if (!GlobalVariables.rfid_code.SequenceEqual(GlobalVariables.checkInterval))
                    {
                        Console.WriteLine("Product change: " + GlobalVariables.rfid_code.Count.ToString() + " --- " + GlobalVariables.checkInterval.Count.ToString());
                        if (GlobalVariables.checkInterval.Count > 0)
                        {
                            count_miss += 1;
                        }
                        else if(GlobalVariables.checkInterval.Count == 0)
                        {
                            Console.WriteLine("***** => No RFID code is read!");
                            count_removeall += 1;
                        }

                        if (count_miss == GlobalVariables.config.count_miss || count_removeall == GlobalVariables.config.count_miss - 1)
                        {
                            Console.WriteLine("*********** UPDATE PRODUCT ***********");
                            updateRFID(driver);

                            count_miss = 0;
                            count_removeall = 0;
                        }
                    }
                    else
                    {
                        count_miss = 0;
                        count_removeall = 0;
                    }

                    GlobalVariables.checkInterval.Clear();
                }
            }
        }

        public static void updateRFID(IWebDriver driver)
        {
            try
            {
                List<GlobalVariables.barcode_rfid> check_list = GlobalVariables.barcode_check.ToList();

                foreach (var bf_el in check_list)
                {
                    string rfid = bf_el.rfid;
                    if (!GlobalVariables.checkInterval.Contains(rfid))
                    {
                        //find barcode book out
                        string miss_barcode = GlobalVariables.barcode_check.Find(el => el.rfid == rfid).barcode;
                        DeleteFromSreen(miss_barcode, driver);
                        GlobalVariables.rfid_code.Remove(rfid);
                        Thread.Sleep(GlobalVariables.config.wait_api);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("=====> UPDATE PRODUCT: "+ e);
            }
            Console.WriteLine("Edit complete!");
        }


       

        protected virtual void OnElementClicked(WebElementEventArgs e)
        {

        }
    }
}
