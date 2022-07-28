using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SeleniumCSharp;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.IO;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using OpenQA.Selenium.Support.Events;
using System.Drawing;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;

namespace SeleniumCSharp
{
    class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                //Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new frmMain());
                //btn.Visible = true;


                OtherFunction.ReadConfig("CONFIG.ini");

                var options = new ChromeOptions();
                options.AddExcludedArgument("enable-automation");

                WebDriver driver = new ChromeDriver(options);
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(GlobalVariables.config.haravan_url);

                GlobalVariables.main_driver = driver;
                // EventFiringWebDriver event = new EventFiringWebDriver(driver);
                string title_window = WebModule.getTitleWindow(driver);

                while (true)
                {
                    title_window = WebModule.getTitleWindow(driver);
                    if (title_window == "Haravan - Accounts")
                    {
                        IWebElement username = driver.FindElement(By.Id("Username"));
                        IWebElement password = driver.FindElement(By.Id("Password"));

                        username.SendKeys(GlobalVariables.config.usr_name);
                        password.SendKeys(GlobalVariables.config.usr_pwd);
                        break;
                    }
                }

                while (true)
                {
                    title_window = WebModule.getTitleWindow(driver);
                    if (title_window == "HaraRetail - Siêu Thị Sách & Tiện Ích Nhân Văn")
                    {
                        break;
                    }
                }

                frmMain TS800 = new frmMain();
                foreach (Process window in Process.GetProcesses())
                {
                    window.Refresh();

                    if (window.MainWindowTitle.Contains("HaraRetail"))
                    {
                        GlobalVariables.main_wd = window.MainWindowHandle;
                        break;
                    }
                }
                IWebElement tab_container = driver.FindElement(By.Id("tab-container"));
                GlobalVariables.num_tab_open = tab_container.FindElements(By.TagName("li")).Count();

                

                //IntPtr reset_wnd = WindowAPI.FindWindowByCaption(IntPtr.Zero, "ResetButton");
                //WindowAPI.ShowWindow(reset_wnd, 9);

                //GlobalVariables.opos = new OPOS();
                //GlobalVariables.OPOSRFID1.CreateControl();
                //GlobalVariables.opos.OPOS_EnableDevice(GlobalVariables.OPOSRFID1);

                
                Task.Run(() => OtherFunction.isBrowserClose(driver));
                Task.Run(() => OtherFunction.CheckPaymentPopup(driver));

                //new Thread(() => GlobalVariables.resetBtn = new ResetButton(loc_btn)); // showResetButton(GlobalVariables.resetBtn)).Start();
                var checknone_task = Task.Run(() => OtherFunction.CheckNoneItem(driver));
                IWebElement loc_reset = driver.FindElement(By.ClassName("cart-header--icon"));
                Point loc_btn = loc_reset.Location;
                GlobalVariables.resetBtn = new ResetButton(loc_btn);
                checknone_task.Wait();
            }
            catch
            {
                Console.WriteLine("THE APP IS CLOSED INVALID!");
            }
            
        }
        public static void showResetButton(ResetButton btn)
        {
            //btn.StartPosition = FormStartPosition.Manual;
            //Console.WriteLine("lllllll"+ Screen.PrimaryScreen.Bounds.Width);
            //btn.Location = loc;

            //btn.Show();
            //btn.ShowDialog();                    
        }
    }
    
}
