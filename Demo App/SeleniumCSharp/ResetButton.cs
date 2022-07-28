using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace SeleniumCSharp
{
    public partial class ResetButton : Form
    {
        public ResetButton(Point loc)
        {
            InitializeComponent();
            SetFormPosition(loc);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (WebModule.checkNoneProduct(GlobalVariables.main_driver))
            {
                GlobalVariables.rfid_code.Clear();
                GlobalVariables.checkInterval.Clear();
                GlobalVariables.barcode_check.Clear();
            }
            else
            {
                WindowAPI.SetForegroundWindow(GlobalVariables.main_wd);
                GlobalVariables.checkInterval.Clear();
                Thread.Sleep(GlobalVariables.config.sleep_task);

                OtherFunction.updateRFID(GlobalVariables.main_driver);
                //if (!GlobalVariables.rfid_code.SequenceEqual(GlobalVariables.checkInterval))
                //{
                    
                        
                //};
            }
            

            //WebModule.DeleteAllFromSreen(GlobalVariables.main_driver);
            //GlobalVariables.rfid_code.Clear();
            //GlobalVariables.checkInterval.Clear();
            //GlobalVariables.barcode_check.Clear();
            //GlobalVariables.clearList = false;
            //Console.WriteLine("======> Clear list!");
        }

        private void SetFormPosition(Point loc)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = loc.X;//Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            this.Top = loc.Y + 100;//Screen.PrimaryScreen.WorkingArea.Top + this.Height + 50;
            this.TopMost = true;
            this.ShowDialog();
        }
    }
}
