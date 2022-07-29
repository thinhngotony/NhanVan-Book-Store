using AxOPOSRFIDLib;
using com.gigatms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelfRegi_V2
{
    public partial class Front : Form
    {
        OPOS opos;
        frmMain Device;
        public string barcode = "";
        public bool barcode_state = true;
        DataTable dt;
        string api_status = "";
        string api_message = "";
        bool rfid_reading = true;
        public Front()
        {
            //startDialog();
            //RestartWebPOS();
            InitializeComponent();
            //txtRfid.DataBindings.Add("Text",Session,"rfidCode",true,DataSourceUpdateMode.Never);
            this.StartPosition = FormStartPosition.Manual;
            this.CenterToScreen();
            init();

            Session.front = this;
            txtRfid.Text = Session.rfidcode;
            txtJan.Text = Session.barcode;
            txtRfid.Focus();
            TextBoxControl(1);
        }

        private void RestartWebPOS()
        {
            var process = Process.GetProcessesByName("AIR_START.exe")[0];
            var path = process.MainModule.FileName;
            process.Kill();
            Process.Start(path);
        }

        private void closeWebpos()
        {

        }

        private void init()
        {
            //Set for new device, comment opos
            //opos = new OPOS();
            Device = new frmMain();

            Dictionary<string, string> dataInFile = getDictionaryConfig("REGISMASTER_CONFIG.ini");

            Session.rfmaster_key = dataInFile["api_key"];
            Session.rfmaster_api = dataInFile["url_api"];
            Session.rfmaster_sub = dataInFile["sub_url"];
            Session.rfmaster_sub_delete = dataInFile["sub_url_delete"];

            Session.haravan_api = dataInFile["haravan_api"];
            Session.haravan_key_name = dataInFile["haravan_key_name"];
            Session.haravan_key_pass = dataInFile["haravan_key_pass"];
            Session.haravan_sub = dataInFile["haravan_sub"];

            Session.SHOPCD = dataInFile["SHOPCD"];
            Session.reload = dataInFile["reload"];
            Session.sync_api = dataInFile["sync_api"];
            Session.sync_sub = dataInFile["sync_sub"];
            Session.device_name = dataInFile["device_name"];
            Session.path = dataInFile["path"];
            Session.rT = (int)Int64.Parse(dataInFile["rT"]);// them bien rT vo GLobal
            Session.JanLen = (int)Int64.Parse(dataInFile["JanLen"]);

        }


        private string getJan2()
        {
            

            if (!String.IsNullOrEmpty(Session.product.ccode))
            {
                bool is978Book = true;

                if (Session.product.Jancode != null)
                {
                    if (jan_cd.Substring(0, 3) == "978" || jan_cd.Substring(0, 3) == "491")
                    {
                        is978Book = true;
                        
                    }
                    
                    else
                    {
                        is978Book = false;
                        Console.WriteLine("This Jan does not have Jan2");
                    }
                }
                

                //Console.WriteLine(Session.product);
                //if (Session.product.goods_type == "1" || Session.product.Jancode.Substring(0, 3) == "978")
                if (Session.product.goods_type == "1" || is978Book)
                {
                    string ccode = Session.product.ccode.PadLeft(4, '0');
                    string price_tax_off = "";
                    if (Session.product.price < 100000)
                    {
                        price_tax_off = Session.product.price.ToString().PadLeft(5, '0');
                    }
                    else
                    {
                        price_tax_off = "00000";
                    }
                    
                    //Console.WriteLine(ccode + "====" + price_tax_off);
                    int checkdigit = 0;
                    string first12 = "" + 192 + ccode + price_tax_off;
                    
                    int[] ch = { 1, 3 };
                    int n = ch.Length;
                    //Console.WriteLine(first12);
                    for (int i = 0; i < 12; i++)
                    {
                        checkdigit += (int)char.GetNumericValue(first12[i]) * ch[i % n];
                        //Console.WriteLine((int)char.GetNumericValue(first12[i]) + "====="+ch[i%n]);
                    }
                    checkdigit %= 10;
                    checkdigit = checkdigit>0?(10 - checkdigit % 10):0;
                    return "" + 192 + ccode + price_tax_off + checkdigit;
                }
                else
                {
                    Console.WriteLine("This Jan does not have Jan2");
                    return "";
                }

            }
            return "";

        }
        public void updateView()
        {

            txtJan.Text = Session.barcode;
            txtRfid.Text = Session.rfidcode;

            txtPName.Text = Session.product.goods_name;
            lProductType.Text = Session.product.rf_goods_type;

            //lRCode.Text = Session.rfidcode;
            //lJanCode.Text = Session.barcode;

            txtPrice.Text = Session.product.price.ToString();
            txtMakerName.Text = Session.product.maker_name;
            txtProductID.Text = Session.product.product_id;
            barcode_state = true;

        }


        public void resetLabel(int mode)
        {
            if (mode == 1)
            {
                Session.product = new ProductData();
                Session.barcode = "";
                Session.rfidcode = "";
                
                txtMakerName.Text = "";
                txtPName.Text = "";
                lProductType.Text = "";
                lJanCode.Text = "";
                lRCode.Text = "";
                txtProductID.Text = "";
                txtPrice.Text = "";
                txtRfid.Text = "";
                txtJan.Text = "";
            }
            if (mode == 2)
            {
                txtMakerName.Text = "";
                txtPName.Text = "";
                txtProductID.Text = "";
                txtPrice.Text = "";
            }
            if (mode == 3)
            {
                Session.product = new ProductData();

                //rfid_cd = "";
                Session.product.RFIDcode = "";
                Session.barcode = "";
                barcode = "";
                txtJan.Text = "";
                lRCode.Text = "";
                lJanCode.Text = "";
                txtMakerName.Text = "";
                txtPName.Text = "";
                lProductType.Text = "";
                lJanCode.Text = "";
                lRCode.Text = "";
                txtProductID.Text = "";
                txtPrice.Text = "";
                //txtRfid.Text = "";
                //txtJan.Text = "";
            }
        }
        public void TextBoxControl(int mode)
        {
            if (mode == 1)
            {
                txtPName.Enabled = false;
                txtMakerName.Enabled = false;
                txtPrice.Enabled = false;
                txtProductID.Enabled = false;
            } else if (mode == 2)
            {
                txtPName.Enabled = true;
                txtMakerName.Enabled = true;
                txtPrice.Enabled = true;
                txtProductID.Enabled = true;
            }
        }
        public void updateLabel()
        {
            txtPName.Text = Session.product.media_name;
            //Remove for NV
            lProductType.Text = Session.product.rf_goods_type;
            //txtProductID.Text = Session.product.tax_type;
            txtProductID.Text = "";

            lJanCode.Text = Session.barcode;
            lRCode.Text = Session.product.RFIDcode;
            
            txtPrice.Text = Session.product.price.ToString();
        }

        //get item data from api
        private async Task ApiGetDataFromHaravan()
        {
            try
            {
                barcode_state = false;
                Session.haravan_sub = "page=1&barcode=";
                Session.product = new ProductData();
                HttpClient api_client = new HttpClient();
                api_client.BaseAddress = new Uri(Session.haravan_api);
                api_client.DefaultRequestHeaders.Add("Authorization", Session.haravan_key_name + " " + Session.haravan_key_pass);

                Console.WriteLine(api_client.DefaultRequestHeaders);
                var builder = new UriBuilder(Session.haravan_api);
                //builder.Query = Session.haravan_sub + Session.barcode;
                builder.Query = Session.haravan_sub + Session.barcode;
                var url = builder.ToString();
                var res = await api_client.GetAsync(url);
                var content = await res.Content.ReadAsStringAsync();

                // Extract value from response data

                dynamic data = JObject.Parse(content);
                Console.WriteLine(data);
                if (content != "") { 
                Session.product.goods_name = data.products[0].title;
                Session.product.rf_goods_type = data.products[0].product_type;
                Session.product.price = data.products[0].variants[0].price;

                Session.product.maker_name = data.products[0].vendor;
                Session.product.product_id = data.products[0].id;
                  
                // Displayed in the user interface
                api_message = "Received data from server successfully";
                
                } else
                {
                    return;
                }
            }
            catch (Exception e)
            {
                api_message = "No product information found";
                Console.WriteLine(e);
            }
        }

        private async Task Api_Recv_bq()
        {

            try
            {
                Session.product = new ProductData();
                //Console.WriteLine(Session.barcode);
                HttpClient api_client = new HttpClient();
                api_client.BaseAddress = new Uri(Session.haravan_api);
                api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string json = System.Text.Json.JsonSerializer.Serialize(new
                {
                    api_key = Session.haravan_key_name,
                    jancode = Session.barcode,
                });


                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await api_client.PostAsync(Session.haravan_sub, content);
                if (result.IsSuccessStatusCode)
                {
                    string resultContent = await result.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(resultContent);

                    api_status = (string)data["code"];
                    api_message = (string)data["message"];
                    if (api_status.Equals("00"))
                    {
                        Session.product.ccode = (string)data["data"]["c_code"] == null ? "//" : (string)data["data"]["c_code"];
                        Session.product.artist_name = (string)data["data"]["artist_name"] == "" ? "//" : (string)data["data"]["artist_name"];
                        Session.product.artist_kana = (string)data["data"]["artist_kana"] == "" ? "//" : (string)data["data"]["artist_kana"];
                        Session.product.Jancode = Session.barcode;
                        Session.product.goods_name = (string)data["data"]["goods_name"];
                        Session.product.goods_name_kana = (string)data["data"]["goods_name_kana"];
                        Session.product.media_cd = (string)data["data"]["media_cd"];
                        Session.product.genreCD = (string)data["data"]["genre_cd"];
                        Session.product.price = (int)data["data"]["price"];
                        Session.product.tax_rate = 1.1;
                        Session.product.Jancode2 = getJan2();
                        Session.product.price_intax = Convert.ToInt32(Math.Floor(Session.product.price * Session.product.tax_rate));
                        Session.product.cost_rate = (float)data["data"]["cost_rate"];
                        

                        //add ,iss fields
                        Session.product.makerCD = (string)data["data"]["publisher_cd"];
                        Session.product.maker_name = (string)data["data"]["publisher_name"];
                        Session.product.maker_name_kana = (string)data["data"]["publisher_name_kana"];
                        Session.product.selling_date = (string)data["data"]["selling_date"];
                    }

                    Console.WriteLine();

                }
                else
                {
                    Console.WriteLine(result);
                    Console.WriteLine("Connect to API Server Failed.");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + "Get Info Successfully";
            //string jan2 = getJan2();
            //Console.WriteLine(jan2);



        }
        //post item data to rfid_master
        private async Task ApiUpdateMasterTable()
        {
            try
            {
                
                HttpClient api_client = new HttpClient();
                api_client.BaseAddress = new Uri(Session.rfmaster_api);
                api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string json = System.Text.Json.JsonSerializer.Serialize(new
                {
                    api_key = Session.rfmaster_key,
                    force_update = Session.force_update,


                    drgm_pos_shop_cd = Session.SHOPCD,

                    //use book code
                    rf_goods_type = Session.product.rf_goods_type,

                    //use rfid code
                    rf_goods_cd_type = "0",

                    drgm_rfid_cd = rfid_cd,
                    drgm_jan = jan_cd,
                    //drgm_jan2 = Session.product.Jancode2,
                    drgm_goods_name = Session.product.goods_name,
                    drgm_goods_name_kana = Session.product.goods_name_kana,
                    drgm_artist = Session.product.artist_name,
                    drgm_artist_kana = Session.product.artist_kana,
                    drgm_maker_cd = Session.product.makerCD,
                    drgm_maker_name = Session.product.maker_name,
                    drgm_genre_cd = Session.product.genreCD,
                    drgm_maker_name_kana = Session.product.maker_name_kana,
                    drgm_c_code = Session.product.ccode,
                    drgm_selling_date = Session.product.selling_date,
                    drgm_price_tax_off = Session.product.price,
                    drgm_cost_rate = Session.product.cost_rate,
                    drgm_cost_price = Session.product.cost_price,
                    drgm_media_cd = Session.product.media_cd,
                    drgm_product_id = Session.product.product_id

                });

                
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine("======>"+content);
                var result = await api_client.PostAsync(Session.rfmaster_sub, content);

                if (result.IsSuccessStatusCode)
                {

                    string resultContent = await result.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(resultContent);

                    //api_message = (string)data["message"];
                    //richTextBox1.Text += DateTime.Now.ToString("h:mm:ss tt") + ": " + message.message + "\n";
                    Console.WriteLine(resultContent);
                    //updateCSVShopDB();
                    api_message = (string)data["message"];
                    api_status = (string)data["code"];
                }
                else
                {
                    Console.WriteLine(result);

                    Console.WriteLine("Connect to API Server Failed.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + "Connect to API failed \n";
            }


            


        }

        // Delete rows from RFID master
        private async Task ApiDelete()
        {

            try
            {
                HttpClient api_client = new HttpClient();
                api_client.BaseAddress = new Uri(Session.rfmaster_api);
                api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var rfids = new string[] { Session.rfidcode };
                string json = System.Text.Json.JsonSerializer.Serialize(new
                {
                    api_key = Session.rfmaster_key,
                    rfid = rfids

                });



                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await api_client.PostAsync(Session.rfmaster_sub_delete, content);

                if (result.IsSuccessStatusCode)
                {

                    string resultContent = await result.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(resultContent);
                    Console.WriteLine(resultContent);
                    api_message = (string)data["message"];
                    api_status = (string)data["code"];
                }
                else
                {
                    Console.WriteLine(result);
                    Console.WriteLine("Connect to API Server Failed.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }

        private async Task<HttpStatusCode> sync_api()
        {
            HttpClient api_client = new HttpClient();
            //api_client.BaseAddress = ;
            //string uri = Session.sync_api + "/api/shop-goods-master-sync?jans=9999,9991";
            Console.WriteLine(Session.sync_api + Session.sync_sub);
            HttpResponseMessage response = await api_client.GetAsync(new Uri(Session.sync_api + Session.sync_sub));

            return response.StatusCode;
        }

        public void ToCSV()
        {
            StreamWriter sw = new StreamWriter(Session.path, false, Encoding.GetEncoding(932));
            //StreamWriter sw = new StreamWriter("test.csv", false, Encoding.GetEncoding(932));
            //headers    
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string name = dt.Columns[i].ColumnName.Contains("Column") ? "" : dt.Columns[i].ColumnName;
                sw.Write(name);
                if (i < dt.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dt.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }

        //public void updateCSVShopDB()
        //{

        //    //dt = ReadCsvFile(Session.path);
        //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //    //Console.WriteLine(dt.Columns[1].ColumnName+"Name");
        //    foreach (DataColumn dc in dt.Columns)
        //    {
        //        sb.Append(dc.ColumnName);
        //        sb.Append(",");
        //    }
        //    sb.Remove(sb.Length - 1, 1);
        //    //sb.AppendLine();
        //    foreach (DataRow r in dt.Rows)
        //    {
        //        if (r.ItemArray[0].ToString() != Session.barcode)
        //        {
        //            for (int i = 0; i < dt.Columns.Count; i++)
        //            {
        //                sb.Append(r.ItemArray[i].ToString());
        //                sb.Append(",");
        //            }
        //        }
        //        sb.Remove(sb.Length - 1, 1);
        //    }
        //    //string product_update = Session.barcode + ",," + Session.product.goods_name + "," + Session.product.tax_type + "," + Session.product.cost_price + ",0," + Session.product.media_cd + ",1";
        //    //Console.WriteLine(product_update);
        //    //sb.AppendLine(product_update);
        //    System.IO.File.WriteAllText("test.csv", sb.ToString(),Encoding.GetEncoding(932));
        //}

        public DataTable ReadCsvFile(string csvfilepath)
        {
            DataTable dtCsv = new DataTable();
            var text = File.ReadAllText(csvfilepath, Encoding.GetEncoding(932));
            text = text.Replace("\n", "");
            string[] rows = text.Split('\r'); //split full file text into rows  
            for (int i = 0; i < rows.Count() - 1; i++)
            {
                string[] rowValues = rows[i].Split(','); //split each row with comma to get individual values  
                {
                    if (i == 0)
                    {
                        for (int j = 0; j < rowValues.Count(); j++)
                        {
                            dtCsv.Columns.Add(rowValues[j]); //add headers  
                        }
                    }
                    else
                    {
                        DataRow dr = dtCsv.NewRow();
                        for (int k = 0; k < rowValues.Count(); k++)
                        {
                            dr[k] = rowValues[k].ToString();
                        }
                        dtCsv.Rows.Add(dr); //add other rows  
                        //Console.WriteLine("====> Row csv: " + dr.ItemArray[0]);
                    }
                    // }
                }
            }

            return dtCsv;
        }

        private Dictionary<string, string> getDictionaryConfig(string path)
        {
            Dictionary<string, string> Config = new Dictionary<string, string>();
            List<string> result = read_Fileini(path);
            foreach (string line in result)
            {
                if (!line.Contains("="))
                {
                    MessageBox.Show("Config file have Incorrect syntax!",
                                   "Format Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
                string[] temp = line.Split('=');
                Config[temp[0]] = temp[1];
            }
            return Config;
        }
        private List<string> read_Fileini(string path)
        {
            List<string> result = new List<string>();
            if (!File.Exists(path))
            {
                MessageBox.Show("Error. Can not found file!\nPlease add config file! ",
                                   "Can not found file!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Error. Can not read file!" + "\n" + e,
                                   "Can not read file!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
            //replace all space
            int size = result.Count;
            for (int i = 0; i < size; i++)
                result[i] = result[i].Replace(" ", "");
            return result;

        }

        private int isLocal()
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString().Equals(Session.barcode))
                {
                    return i;
                }
            }
            return -1;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            
            if (true)
            {
                TextBoxControl(1);
                btnDeleteAuto.Enabled = true;
                btnDeleteManual.Enabled = true;
                Session.product.goods_name = txtPName.Text;
                Session.product.price = Int32.Parse(txtPrice.Text);
                Session.product.artist_name = txtProductID.Text;

                string product_update = String.Format("\n{0},{1},{2},{3},{4},{5},{6},{7}\n",
                        jan_cd, "", txtPName.Text, "1", txtPrice.Text, "0", "", "1");
                Console.WriteLine(product_update);
                File.AppendAllText(Session.path, product_update, Encoding.Default);

                Wait wait = new Wait();
                wait.Visible = true;
                Task.Run(() => ApiUpdateMasterTable()).Wait();
                wait.Visible = false;
                lNew.Visible = false;
                panel2.Focus();
                //panel2.Focus();
                txtPName.ReadOnly = true;
                txtProductID.ReadOnly = true;
                txtPrice.ReadOnly = true;

                richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss: ") + api_message + "\n";
                api_message = "";


                btnCancel.Visible = false;

                //btnRegister.BackColor = SystemColors.ControlLight;
                //btnRegister.ForeColor = Color.Black;
                opos.OPOS_StartReading(Session.OPOSRFID1);
                btnConnect.Text = "StopReading";
                btnConnect.Visible = true;
                resetLabel(1);
                Session.product = new ProductData();
                btnRegister.Visible = false;
                barcode_state = true;
            }
            else
            {
                MessageBox.Show("Please fill all the fields", "Error");
            }
            panel2.Focus();
        }


        public string rfid_cd;
        public string jan_cd;
        private void Front_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return && barcode_state)
            {
                barcode = "";
                rfid_reading = false;

                if (!Session.barcode.Equals("") && !Session.rfidcode.Equals("") && btnInsert.Checked)
                {

                    rfid_cd = Session.rfidcode;
                    jan_cd = Session.barcode;



                    Wait wait = new Wait();
                    wait.Visible = true;
                    Task.Run(() => ApiGetDataFromHaravan()).Wait();
                    updateView();
                    Task.Run(() => ApiUpdateMasterTable()).Wait();
                    richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": " + api_message + "\n";
                    wait.Visible = false;
                    lRCode.Text = rfid_cd;
                    lJanCode.Text = jan_cd;

                   

                    //Task.Run(() => Api_Recv()).Wait();


                    //switch (api_status)
                    //{
                    //    case "00":


                    //        richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": Received Data from API.\n";
                    //        int res = isLocal();
                    //        // Remove for NV

                    //        //if (res != -1)
                    //        if (true)
                    //        {
                    //            Session.product.Jancode2 = getJan2();
                    //            //updateView();


                    //            dt.Rows[res]["gdm_media_name"] = Session.product.goods_name;
                    //            dt.Rows[res]["gds_tax_type"] = "1";
                    //            dt.Rows[res]["Column2"] = Session.product.price_intax;
                    //            dt.Rows[res]["Column3"] = "0";
                    //            dt.Rows[res]["gdm_media_cd"] = Session.product.media_cd;
                    //            dt.Rows[res][7] = "1";
                    //            //updateCSVShopDB();
                    //            Console.WriteLine("======= Is in Localdb");
                    //            ToCSV();
                    //            richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": Updated LocalDB.\n";

                    //            wait.Visible = true;
                    //            Task.Run(() => ApiUpdateMasterTable()).Wait();                            
                    //            wait.Visible = false;

                    //            //Task.Run(() => ApiUpdateMasterTable()).Wait();


                    //            richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss: ") + api_message + "\n";
                    //            api_message = "";
                    //            barcode_state = true;
                    //        }

                    //        // Remove for NV - comment all
                    //        else
                    //        {
                    //            Session.product.Jancode2 = getJan2();
                    //            if (rfid_cd != "")
                    //            // if (!Session.rfidcode.Equals(""))
                    //            {
                    //                //Console.WriteLine(dt.Rows[0][7]);

                    //                wait.Visible = true;
                    //                Task.Run(() => ApiUpdateMasterTable()).Wait();
                    //                wait.Visible = false;

                    //                //Task.Run(() => ApiUpdateMasterTable()).Wait();


                    //                //Thread.Sleep(1000);

                    //                richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": " + api_message + "\n";
                    //                api_message = "";
                    //                // dt.Rows.Add(Session.barcode, Session.product.goods_name_kana, Session.product.goods_name, "1", Session.product.price_intax, "0", Session.product.media_cd, "1");
                    //                dt.Rows.Add(jan_cd, Session.product.goods_name_kana, Session.product.goods_name, "1", Session.product.price_intax, "0", Session.product.media_cd, "1");
                    //                //updateCSVShopDB();
                    //                Console.WriteLine("======= Is not in Localdb");
                    //                ToCSV();
                    //                richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": Added new Item to LocalDB\n";
                    //                barcode_state = true;
                    //            }

                    //        }
                    //        updateView();




                    //        break;
                    //    case "02":

                    //        //stop rfid reading
                    //        opos.OPOS_StopReading(Session.OPOSRFID1);
                    //        if (MessageBox.Show("データが存在しません。入力しますか？", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    //        {

                    //            btnDeleteAuto.Enabled = false;
                    //            btnDeleteManual.Enabled = false;
                    //            //need to handle

                    //            txtPName.Focus();
                    //            this.ActiveControl = txtPName;


                    //            btnRegister.Visible = true;
                    //            TextBoxControl(2);
                    //            barcode_state = false;
                    //            resetLabel(2);

                    //            btnConnect.Visible = false;
                    //            //btnConnect.Text = "StartReading";




                    //            //stop receiving barcode signal
                    //            richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": " + api_message + "\n";
                    //            api_message = "";

                    //            //lNew.Text = " データが存在しません。";
                    //            txtMakerName.ReadOnly = false;
                    //            lNew.Visible = true;
                    //            txtPName.ReadOnly = false;
                    //            txtProductID.ReadOnly = false;
                    //            txtPrice.ReadOnly = false;
                    //            //btnRegister.BackColor = Color.Teal;
                    //            //btnRegister.ForeColor = Color.WhiteSmoke;
                    //            btnCancel.Visible = true;


                    //            panel2.Focus();
                    //        }
                    //        else
                    //        {
                    //            opos.OPOS_StartReading(Session.OPOSRFID1);
                    //            barcode_state = true;
                    //        }
                    //        break;
                    //    default:
                    //        break;
                    //}
                    //Thread.Sleep(1000);
                    //resetLabel(1);

                }
                rfid_reading = true;
            }


        }
        
        private void Front_KeyPress(object sender, KeyPressEventArgs e)
        {
                if (barcode_state)
                {
                if (Char.IsDigit(e.KeyChar))
                    {
                        barcode += e.KeyChar;
                    }
                    if (barcode.Length == Session.JanLen)
                    {
                        if (barcode.Substring(0, 3).Equals("192") || barcode.Substring(0, 3).Equals("191"))
                        {
                            barcode = "";
                            Session.barcode = "";
                        }
                        else
                        {
                            Session.barcode = barcode;
                            barcode = "";
                        }

                        updateView();
                    }
                }
            }

        

        private void btnConnect_Click(object sender, EventArgs e)
        {
            panel2.Focus();
            btnConnect.Enabled = false;
            switch (btnConnect.Text)
            {
                case "Connect":



                    // Set for new device
                    //Session.OPOSRFID1.CreateControl();
                    //int n = opos.OPOS_EnableDevice(Session.OPOSRFID1);

                    //new_device
                    if (Device.ConnectDevice(Session._ts800, Session._host))
                    {
                        //opos.OPOS_StartReading(Session.OPOSRFID1);
                        if (Device.StartInventory(Session._ts800)) { 
                        btnConnect.Text = "StopReading";
                        richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": Connected to Device\n";
                        }
                    }
                    else
                    {
                        richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": Failed to connect to device\n";
                        //switch (n)
                        //{
                        //    case 0:
                        //        richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": Connect to Device Failed \n";
                        //        break;
                        //    case 1:
                        //        richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": Claim Device Failed\n";
                        //        break;
                        //    case 2:
                        //        richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": Enable Device Failed\n";
                        //        break;
                        //    default:
                        //        break;
                        //};
                    }
                    break;
                //new_device
                case "StopReading":

                    //opos.OPOS_StopReading(Session.OPOSRFID1);

                    if (Device.StopReading(Session._ts800)) { 
                    btnConnect.Text = "StartReading";
                    richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": Device stopped Reading \n";
                    resetLabel(1);
                    } else
                    {
                        richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": Unknown error, cannot stop reading \n";
                    }
                    break;
                case "StartReading":

                    //opos.OPOS_StartReading(Session.OPOSRFID1);

                    Device.StartInventory(Session._ts800);
                    btnConnect.Text = "StopReading";
                    richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": Device started Reading \n";
                    resetLabel(1);
                    break;
                default:
                    break;
            }
            btnConnect.Enabled = true;
            panel2.Focus();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            panel2.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            closeWebpos();
            this.Close();
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            TextBoxControl(1);
            btnDeleteAuto.Enabled = true;
            btnDeleteManual.Enabled = true;
            txtPrice.ReadOnly = true;
            txtProductID.ReadOnly = true;
            txtPName.ReadOnly = true;
            txtMakerName.ReadOnly = true;
            opos.OPOS_StartReading(Session.OPOSRFID1);
            btnConnect.Text = "StopReading";
            btnConnect.Visible = true; ;
            resetLabel(1);
            barcode_state = true;
            btnCancel.Visible = false;
            btnRegister.Visible = false;
            panel2.Focus();
        }

        private async void btnWritecsv_Click(object sender, EventArgs e)
        {
            try
            {
                await sync_api();
                panel2.Focus();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void txtMakerName_TextChanged(object sender, EventArgs e)
        {

            if (!txtMakerName.Text.Equals(""))
            {
                //Session.product.ccode = txtMakerName.Text;
                //if (!txtPrice.Text.Equals(""))
                //{
                //    Session.product.Jancode2 = getJan2();
                //    lProductType.Text = Session.product.rf_goods_type;
                //}
                //else
                //{
                //    Session.product.Jancode2 = "";
                //    lProductType.Text = "";
                //}

            }
            else
            {
                //Session.product.Jancode2 = "";
                //lProductType.Text = "";
            }
        }

        private void txtMakerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {

            if (!txtPrice.Text.Equals(""))
            {
                //Session.product.price = Int32.Parse(txtPrice.Text);

                //if (!txtMakerName.Text.Equals(""))
                //{
                //    Session.product.Jancode2 = getJan2();
                //    lProductType.Text = Session.product.Jancode2;
                //}
                //else
                //{
                //    Session.product.Jancode2 = "";
                //    lProductType.Text = "";
                //}
                

            }
            else
            {
                //Session.product.Jancode2 = "";
                //lProductType.Text = "";
            }
        }

        private void lProductType_Click(object sender, EventArgs e)
        {

        }

        private void txtRfid_TextChanged(object sender, EventArgs e)
        {
            resetLabel(3);
            updateView();
            //if (!Session.barcode.Equals("") && !Session.rfidcode.Equals("") && btnInsert.Checked)
            //{
            //    txtRfid.Text = Session.rfidcode;
            //    Task.Run(() => Api_Recv()).Wait();

            //    switch (api_status)
            //    {
            //        case "00":

            //            richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": Received Data from API.\n";
            //            int res = isLocal();
            //            if (res != -1)
            //            {
            //                Session.product.Jancode2 = getJan2();
            //                updateView();
            //                dt.Rows[res]["gdm_media_name"] = Session.product.goods_name;

            //                dt.Rows[res]["gds_tax_type"] = "1";
            //                dt.Rows[res]["Column2"] = Session.product.price_intax;
            //                dt.Rows[res]["Column3"] = "0";
            //                dt.Rows[res]["gdm_media_cd"] = Session.product.media_cd;
            //                dt.Rows[res][7] = "1";
            //                updateCSVShopDB();
            //                Console.WriteLine("======= Is in Localdb");
            //                ToCSV();
            //                richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": Updated LocalDB.\n";
            //                Task.Run(() => ApiUpdateMasterTable()).Wait();
            //                richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss: ") + api_message + "\n";
            //                api_message = "";
            //            }
            //            else
            //            {
            //                Session.product.Jancode2 = getJan2();
            //                if (!Session.rfidcode.Equals(""))
            //                {
            //                    Console.WriteLine(dt.Rows[0][7]);

            //                    Task.Run(() => ApiUpdateMasterTable()).Wait();
            //                    richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": " + api_message + "\n";
            //                    api_message = "";
            //                    dt.Rows.Add(Session.barcode, Session.product.Jancode2, Session.product.goods_name, "1", Session.product.price_intax, "0", Session.product.media_cd, "1");
            //                    updateCSVShopDB();
            //                    Console.WriteLine("======= Is not in Localdb");
            //                    ToCSV();
            //                    richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": Added new Item to LocalDB\n";
            //                }
            //            }
            //            updateView();
            //            break;
            //        case "02":
            //            stop rfid reading
            //            opos.OPOS_StopReading(Session.OPOSRFID1);
            //            if (MessageBox.Show("データが存在しません。入力しますか？", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //            {
            //                need to handle
            //                btnDeleteAuto.Enabled = false;
            //                btnDeleteManual.Enabled = false;

            //                this.ActiveControl = txtPName;
            //                txtPName.Focus();
            //                btnRegister.Visible = true;
            //                barcode_state = false;
            //                resetLabel(2);

            //                btnConnect.Visible = false;
            //                btnConnect.Text = "StopReading";




            //                stop receiving barcode signal
            //                richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": " + api_message + "\n";
            //                api_message = "";

            //                lNew.Text = " データが存在しません。";
            //                txtMakerName.ReadOnly = false;
            //                lNew.Visible = true;
            //                txtPName.ReadOnly = false;
            //                txtProductID.ReadOnly = false;
            //                txtPrice.ReadOnly = false;
            //                btnRegister.BackColor = Color.Teal;
            //                btnRegister.ForeColor = Color.WhiteSmoke;
            //                btnCancel.Visible = true;


            //                panel2.Focus();
            //            }
            //            else
            //            {
            //                resetLabel(1);
            //                opos.OPOS_StartReading(Session.OPOSRFID1);
            //            }
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //else
            
            if (!Session.rfidcode.Equals("") && Session.barcode.Equals("") && !btnInsert.Checked && btnDeleteAuto.Checked)
            {
                txtRfid.Text = Session.rfidcode;
                Task.Run(() => ApiDelete()).Wait();
                richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": " + api_message + "\n";
                updateView();

            } 
            //else if (Session.rfidcode.Equals("")  )
            //{
            //    resetLabel(1);
            //}
            
        }

        private void txtJan_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPName_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void txtPName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                txtMakerName.Focus();
            }
        }

        private void txtMakerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
            {
                txtPrice.Focus();
            }
        }

        private void txtPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
            {
                txtProductID.Focus();
            }
        }

        private void txtProductID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
            {
                txtPName.Focus();
            }
        }

        private void txtPName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtMakerName.Focus();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_CheckedChanged(object sender, EventArgs e)
        {

            if (btnDeleteAuto.Checked == true)
            {
                DialogResult confirmResult = MessageBox.Show("Are you sure you want to switch to delete mode?", "Confirm Diaglog", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.No)
                {
                    btnInsert.Checked = true;
                    btnDeleteAuto.Checked = false;
                }
                else
                {
                    //opos.OPOS_StartReading(Session.OPOSRFID1);
                    btnInsert.Checked = false;
                    btnDeleteAuto.Checked = true;
                    btnDeleteSingle2.Visible = false;
                    resetLabel(1);

                }
            }

        }


        public class ProductData
        {
            public string RFIDcode { get; set; }
            public string Jancode { set; get; }
            public string Jancode2 { set; get; }
            public string media_name { get; set; }
            public string tax_type { get; set; }
            public int cost_price { get; set; }
            public string media_cd { set; get; }
            public string goods_name_kana { set; get; }
            public string artist_name { set; get; }
            public string makerCD { set; get; }
            public string maker_name { set; get; }
            public string genreCD { set; get; }
            public string maker_name_kana { set; get; }
            public string ccode { set; get; }
            public string selling_date { set; get; }
            public int price { set; get; }
            public double tax_rate { set; get; }
            public string goods_type { set; get; }
            public int goods_cd_type { set; get; }
            public string goods_name { set; get; }
            public string artist_kana { set; get; }
            public string rf_goods_type { set; get; }
            public double price_intax { set; get; }
            public float cost_rate { set; get; }
            public string product_id { set; get; }

            public ProductData()
            {
                RFIDcode = "";
                Jancode = "";
                Jancode2 = "";
                this.media_name = "";
                this.artist_name = "";
                this.genreCD = "";
                this.ccode = "";
                price = 0;
                this.goods_name = "";
                this.rf_goods_type = "";
                this.product_id = "";
            }
        }



        public class OPOS : System.Windows.Forms.UserControl
        {

            public OPOS()
            {
                Session.OPOSRFID1.DataEvent += OPOSRFID1_DataEvent;
                Session.OPOSRFID1.ErrorEvent += OPOSRFID1_ErrorEvent;
            }

            private void OPOSRFID1_ErrorEvent(Object sender, _DOPOSRFIDEvents_ErrorEventEvent e)
            {
                //if(checkmiss == 3)
                //{
                //    Session.rfidcode = "";
                //    Session.front.updateView();
                //    checkmiss = 0;
                //}
                //checkmiss++;
                
            }


            private void OPOSRFID1_DataEvent(object sender, _DOPOSRFIDEvents_DataEventEvent e)
            {

                int TagCount;
                string UserData;

                TagCount = Session.OPOSRFID1.TagCount;


                for (int i = 0; i < TagCount; i++)
                {
                    UserData = " Userdata=" + Session.OPOSRFID1.CurrentTagUserData;

                    if (UserData == " Userdata=")
                    {
                        UserData = "";
                    }
                    var code_value = Session.OPOSRFID1.CurrentTagID + UserData;
                    string new_code = ConvertTagIDCode(code_value);
                    //Console.WriteLine(new_code);

                    //new_device
                    if (!Session.rfidcode.Equals(new_code)&&Session.front.rfid_reading)
                    {
                        Session.rfidcode = new_code;
                        Session.front.updateView();
                    }

                    Session.OPOSRFID1.NextTag();

                }

                Session.OPOSRFID1.DataEventEnabled = true;
            }

            //Enable Device
            public int OPOS_EnableDevice(AxOPOSRFID OPOSRFID1)
            {
                int Result;
                int phase;
                string strData;

                // Open Device
                string device_name = Session.device_name;
                Result = OPOSRFID1.Open(device_name);
                if (Result != OposStatus.OposSuccess)
                {
                    return 0;
                }

                Result = OPOSRFID1.ClaimDevice(3000);
                if (Result != OposStatus.OposSuccess)
                {

                    OPOSRFID1.Close();
                    return 1;
                }

                OPOSRFID1.DeviceEnabled = true;
                Result = OPOSRFID1.ResultCode;
                if (Result != OposStatus.OposSuccess)
                {

                    OPOSRFID1.Close();
                    return 2;
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
                return -1;
            }
            //disable device
            private void OPOS_DisableDevice(AxOPOSRFID OPOSRFID1)
            {

            }

            //Scanning
            public void OPOS_StartReading(AxOPOSRFID OPOSRFID1)
            {
                int Result;
                //OPOSRFID1.ClearInputProperties();
                OPOSRFID1.ReadTimerInterval = Session.rT;
                OPOSRFID1.DataEventEnabled = true;

                if (Session.OPOSRFID1.TagCount > 0)
                {

                    //Console.WriteLine("==========> TAGCOUNT Before clear: " + Session.OPOSRFID1.TagCount);
                    Session.OPOSRFID1.ClearInputProperties();
                    //Console.WriteLine("==========> TAGCOUNT After clear: " + Session.OPOSRFID1.TagCount);
                }

                PhaseChange(OPOSRFID1);
                Result = OPOSRFID1.StartReadTags(OposStatus.RfidRtId, "000000000000000000000000", "000000000000000000000000", 0, 0, 1000, "00000000");
                if (Result != OposStatus.OposSuccess)
                {
                    Console.WriteLine("read err");
                }

                //OPOSRFID1.DataEventEnabled = true;
                //Session.isReading = true;
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


            //Call API



            //Converte List
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




            public void OPOS_Connector(AxOPOSRFID OPOSRFID1)
            {
                //string CurrentTagID;
                //int TagCount;
                //int LoopCnt;
                //string UserData;
                OPOS_EnableDevice(OPOSRFID1);

                //if (Session.StateCurr == 1)
                //{
                //   // Session.rfid_code.Clear();
                //    if (Session.isReading)
                //    {
                //        OPOS_StopReading(OPOSRFID1);
                //    }
                //    else
                //    {
                //        OPOS_EnableDevice(OPOSRFID1);
                //    }

                //}

                //OPOS_StartReading(OPOSRFID1);
                //OPOSRFID1.ReadTimerInterval = 400;

                //if (OPOSRFID1.TagCount > 0)
                //{

                //    Console.WriteLine("==========> TAGCOUNT Before clear: " + OPOSRFID1.TagCount);
                //    OPOSRFID1.ClearInputProperties();
                //    Console.WriteLine(OPOSRFID1.ClearInputProperties());
                //    Console.WriteLine("==========> TAGCOUNT After clear: " + OPOSRFID1.TagCount);
                //}

                //while (Session.isReading)
                //{
                //    //Thread.Sleep(500);
                //    OPOSRFID1.DataEventEnabled = true;
                //    TagCount = OPOSRFID1.TagCount;
                //    for (int i = 0; i < TagCount; i++)
                //    {
                //        //var code_value = OPOSRFID1.CurrentTagID;
                //        UserData = " Userdata=" + OPOSRFID1.CurrentTagUserData;

                //        if (UserData == " Userdata=")
                //        {
                //            UserData = "";
                //        }
                //        var code_value = OPOSRFID1.CurrentTagID + UserData;
                //        OPOSRFID1.NextTag();
                //        //Console.WriteLine(code_value +"\n");
                //        if (!Session.rfid_code.Contains(code_value))
                //        {
                //            Session.rfid_code.Add(code_value);
                //            string new_code = ConvertTagIDCode(code_value);
                //            RFIDtoJAN(api_client, new_code, sub_wm);

                //            Thread.Sleep(500);
                //            //WindowAPI.PostMessage(sub_wm, (uint)Session.WM_KEYDOWN, (int)(System.Enum.Parse(typeof(Session.Keys), "Return")), (int)(0));
                //        }
                //    }
                //    OPOSRFID1.DataEventEnabled = true;
                //}
                //OPOS_StopReading(OPOSRFID1);
            }


            //private DataEvent

            private void InitializeComponent()
            {
                this.SuspendLayout();
                // 
                // OPOS
                // 
                this.Name = "OPOS";
                this.Load += new System.EventHandler(this.OPOS_Load);
                this.ResumeLayout(false);
            }

            private void OPOS_Load(object sender, EventArgs e)
            {

            }



            public static void stopReading(AxOPOSRFID OPOSRFID1)
            {
                //Session.isReading = false;
                int Result;
                Result = OPOSRFID1.StopReadTags("00000000");
                if (Result != OposStatus.OposSuccess)
                {
                    Console.WriteLine("Err Stop");
                }
            }

            //private void axOPOSRFID1_DataEvent(object sender, AxOPOSRFIDLib._DOPOSRFIDEvents_DataEventEvent e)
            //{
            //    string CurrentTagID;
            //    int TagCount;
            //    int LoopCnt;
            //    string UserData;

            //    // TagCount
            //    TagCount = Session.OPOSRFID1.TagCount;

            //    var loopTo = TagCount;
            //    TagCount = Session.OPOSRFID1.TagCount;
            //    for (int i = 0; i < TagCount; i++)
            //    {
            //        //var code_value = OPOSRFID1.CurrentTagID;
            //        UserData = " Userdata=" + Session.OPOSRFID1.CurrentTagUserData;

            //        if (UserData == " Userdata=")
            //        {
            //            UserData = "";
            //        }
            //        var code_value = Session.OPOSRFID1.CurrentTagID + UserData;
            //        Session.OPOSRFID1.NextTag();
            //        //Console.WriteLine("DataEvent " + code_value + "\n");
            //        //if (!Session.rfid_code.Contains(code_value))
            //        //{
            //        //    Session.rfid_code.Add(code_value);
            //        //    string new_code = ConvertTagIDCode(code_value);
            //        //    RFIDtoJAN(api_client, new_code, sub_wm);

            //        //    Thread.Sleep(500);
            //        //WindowAPI.PostMessage(sub_wm, (uint)Session.WM_KEYDOWN, (int)(System.Enum.Parse(typeof(Session.Keys), "Return")), (int)(0));
            //        //}
            //    }

            //    // DataEventEnabled=True for next DataEvent
            //    Session.OPOSRFID1.DataEventEnabled = true;
            //}


        }
        public class OposStatus
        {
            public const int OposSuccess = 0;
            public const int OposEClosed = 101;
            public const int OposEClaimed = 102;
            public const int OposENotclaimed = 103;
            public const int OposENoservice = 104;
            public const int OposEDisabled = 105;
            public const int OposEIllegal = 106;
            public const int OposENohardware = 107;
            public const int OposEOffline = 108;
            public const int OposENoexist = 109;
            public const int OposEExists = 110;
            public const int OposEFailure = 111;
            public const int OposETimeout = 112;
            public const int OposEBusy = 113;
            public const int OposEExtended = 114;

            [System.Runtime.InteropServices.DllImport("AxInterop.OPOSRFIDLib.dll")]
            public static extern int Open(string deviceName);

            //[System.Runtime.InteropServices.DllImport("Interop.OPOSRFIDLib.dll")]
            //public static extern string Amethod(string s);

            // *///////////////////////////////////////////////////////////////////
            // * OPOS "OpenResult" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int Oposopenerr = 300;

            public const int OposOrAlreadyopen = 301;
            public const int OposOrRegbadname = 302;
            public const int OposOrRegprogid = 303;
            public const int OposOrCreate = 304;
            public const int OposOrBadif = 305;
            public const int OposOrFailedopen = 306;
            public const int OposOrBadversion = 307;

            public const int Oposopenerrso = 400;

            public const short OposOrsNoport = 401;
            public const short OposOrsNotsupported = 402;
            public const short OposOrsConfig = 403;
            public const short OposOrsSpecific = 450;


            // *///////////////////////////////////////////////////////////////////
            // * OPOS "BinaryConversion" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int OposBcNone = 0;
            public const int OposBcNibble = 1;
            public const int OposBcDecimal = 2;


            // *///////////////////////////////////////////////////////////////////
            // * "CheckHealth" Method: "Level" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int OposChInternal = 1;
            public const int OposChExternal = 2;
            public const int OposChInteractive = 3;


            // *///////////////////////////////////////////////////////////////////
            // * OPOS "CapPowerReporting", "PowerState", "PowerNotify" Property
            // *   Constants
            // *///////////////////////////////////////////////////////////////////

            public const int OposPrNone = 0;
            public const int OposPrStandard = 1;
            public const int OposPrAdvanced = 2;

            public const int OposPnDisabled = 0;
            public const int OposPnEnabled = 1;

            public const int OposPsUnknown = 2000;
            public const int OposPsOnline = 2001;
            public const int OposPsOff = 2002;
            public const int OposPsOffline = 2003;
            public const int OposPsOffOffline = 2004;


            // *///////////////////////////////////////////////////////////////////
            // * "ErrorEvent" Event: "ErrorLocus" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int OposElOutput = 1;
            public const int OposElInput = 2;
            public const int OposElInputData = 3;


            // *///////////////////////////////////////////////////////////////////
            // * "ErrorEvent" Event: "ErrorResponse" Constants
            // *///////////////////////////////////////////////////////////////////

            public const int OposErRetry = 11;
            public const int OposErClear = 12;
            public const int OposErContinueinput = 13;


            // *///////////////////////////////////////////////////////////////////
            // * "StatusUpdateEvent" Event: Common "Status" Constants
            // *///////////////////////////////////////////////////////////////////

            public const int OposSuePowerOnline = 2001;
            public const int OposSuePowerOff = 2002;
            public const int OposSuePowerOffline = 2003;
            public const int OposSuePowerOffOffline = 2004;


            // *///////////////////////////////////////////////////////////////////
            // * General Constants
            // *///////////////////////////////////////////////////////////////////

            public const int OposForever = -1;



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposBb.h
            // *
            // *   Bump Bar header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 98-03-06 OPOS Release 1.3                                     BB
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * "CurrentUnitID" and "UnitsOnline" Properties
            // *  and "Units" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int BbUid1 = 0x1;
            public const int BbUid2 = 0x2;
            public const int BbUid3 = 0x4;
            public const int BbUid4 = 0x8;
            public const int BbUid5 = 0x10;
            public const int BbUid6 = 0x20;
            public const int BbUid7 = 0x40;
            public const int BbUid8 = 0x80;
            public const int BbUid9 = 0x100;
            public const int BbUid10 = 0x200;
            public const int BbUid11 = 0x400;
            public const int BbUid12 = 0x800;
            public const int BbUid13 = 0x1000;
            public const int BbUid14 = 0x2000;
            public const int BbUid15 = 0x4000;
            public const int BbUid16 = 0x8000;
            public const int BbUid17 = 0x10000;
            public const int BbUid18 = 0x20000;
            public const int BbUid19 = 0x40000;
            public const int BbUid20 = 0x80000;
            public const int BbUid21 = 0x100000;
            public const int BbUid22 = 0x200000;
            public const int BbUid23 = 0x400000;
            public const int BbUid24 = 0x800000;
            public const int BbUid25 = 0x1000000;
            public const int BbUid26 = 0x2000000;
            public const int BbUid27 = 0x4000000;
            public const int BbUid28 = 0x8000000;
            public const int BbUid29 = 0x10000000;
            public const int BbUid30 = 0x20000000;
            public const int BbUid31 = 0x40000000;
            public const int BbUid32 = int.MinValue + 0x00000000;


            // *///////////////////////////////////////////////////////////////////
            // * "DataEvent" Event: "Status" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int BbDeKey = 0x1;



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposCash.h
            // *
            // *   Cash Drawer header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 95-12-08 OPOS Release 1.0                                     CRM
            // * 98-03-06 OPOS Release 1.3                                     CRM
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * "StatusUpdateEvent" Event Constants
            // *///////////////////////////////////////////////////////////////////

            public const int CashSueDrawerclosed = 0;
            public const int CashSueDraweropen = 1;



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposCAT.h
            // *
            // *   CAT header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 98-09-23 OPOS Release 1.4                                 OPOS-J
            // * 00-09-24 OPOS Release 1.5                                    BKS
            // *   Add CAT_PAYMENT_DEBIT, CAT_MEDIA_UNSPECIFIED,
            // *   CAT_MEDIA_NONDEFINE, CAT_MEDIA_CREDIT, CAT_MEDIA_DEBIT
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * Payment Condition Constants
            // *///////////////////////////////////////////////////////////////////

            public const int CatPaymentLump = 10;
            public const int CatPaymentBonus1 = 21;
            public const int CatPaymentBonus2 = 22;
            public const int CatPaymentBonus3 = 23;
            public const int CatPaymentBonus4 = 24;
            public const int CatPaymentBonus5 = 25;
            public const int CatPaymentInstallment1 = 61;
            public const int CatPaymentInstallment2 = 62;
            public const int CatPaymentInstallment3 = 63;
            public const int CatPaymentBonusCombination1 = 31;
            public const int CatPaymentBonusCombination2 = 32;
            public const int CatPaymentBonusCombination3 = 33;
            public const int CatPaymentBonusCombination4 = 34;
            public const int CatPaymentRevolving = 80;
            public const int CatPaymentDebit = 110;


            // *///////////////////////////////////////////////////////////////////
            // * Transaction Type Constants
            // *///////////////////////////////////////////////////////////////////

            public const int CatTransactionSales = 10;
            public const int CatTransactionVoid = 20;
            public const int CatTransactionRefund = 21;
            public const int CatTransactionVoidpresales = 29;
            public const int CatTransactionCompletion = 30;
            public const int CatTransactionPresales = 40;
            public const int CatTransactionCheckcard = 41;


            // *///////////////////////////////////////////////////////////////////
            // * "PaymentMedia" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int CatMediaUnspecified = 0;
            public const int CatMediaNondefine = 0;
            public const int CatMediaCredit = 1;
            public const int CatMediaDebit = 2;


            // *///////////////////////////////////////////////////////////////////
            // * ResultCodeExtended Constants
            // *///////////////////////////////////////////////////////////////////

            public const int OposEcatCentererror = 1;
            public const int OposEcatCommanderror = 90;
            public const int OposEcatReset = 91;
            public const int OposEcatCommunicationerror = 92;
            public const int OposEcatDailylogoverflow = 200;


            // *///////////////////////////////////////////////////////////////////
            // * "Daily Log" Property  & Argument Constants
            // *///////////////////////////////////////////////////////////////////

            public const int CatDlNone = 0; // None of them
            public const int CatDlReporting = 1; // Only Reporting
            public const int CatDlSettlement = 2; // Only Settlement
            public const int CatDlReportingSettlement = 3; // Both of them



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposChan.h
            // *
            // *   Cash Changer header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 97-06-04 OPOS Release 1.2                                     CRM
            // * 00-09-24 OPOS Release 1.5                                  OPOS-J
            // *   Add DepositStatus Constants.
            // *   Add EndDeposit Constants.
            // *   Add PauseDeposit Constants.
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * "DeviceStatus" and "FullStatus" Property Constants
            // * "StatusUpdateEvent" Event Constants
            // *///////////////////////////////////////////////////////////////////

            public const int ChanStatusOk = 0; // DeviceStatus, FullStatus

            public const int ChanStatusEmpty = 11; // DeviceStatus, StatusUpdateEvent
            public const int ChanStatusNearempty = 12; // DeviceStatus, StatusUpdateEvent
            public const int ChanStatusEmptyok = 13; // StatusUpdateEvent

            public const int ChanStatusFull = 21; // FullStatus, StatusUpdateEvent
            public const int ChanStatusNearfull = 22; // FullStatus, StatusUpdateEvent
            public const int ChanStatusFullok = 23; // StatusUpdateEvent

            public const int ChanStatusJam = 31; // DeviceStatus, StatusUpdateEvent
            public const int ChanStatusJamok = 32; // StatusUpdateEvent

            public const int ChanStatusAsync = 91; // StatusUpdateEvent


            // *///////////////////////////////////////////////////////////////////
            // * "DepositStatus" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int ChanStatusDepositStart = 1;
            public const int ChanStatusDepositEnd = 2;
            public const int ChanStatusDepositNone = 3;
            public const int ChanStatusDepositCount = 4;
            public const int ChanStatusDepositJam = 5;


            // *///////////////////////////////////////////////////////////////////
            // * "EndDeposit" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int ChanDepositChange = 1;
            public const int ChanDepositNochange = 2;
            public const int ChanDepositrepay = 3;


            // *///////////////////////////////////////////////////////////////////
            // * "PauseDeposit" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int ChanDepositPause = 1;
            public const int ChanDepositRestart = 2;


            // *///////////////////////////////////////////////////////////////////
            // * "ResultCodeExtended" Property Constants for Cash Changer
            // *///////////////////////////////////////////////////////////////////

            public const int OposEchanOverdispense = 201;



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposCoin.h
            // *
            // *   Coin Dispenser header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 95-12-08 OPOS Release 1.0                                     CRM
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * "DispenserStatus" Property Constants
            // * "StatusUpdateEvent" Event: "Data" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int CoinStatusOk = 1;
            public const int CoinStatusEmpty = 2;
            public const int CoinStatusNearempty = 3;
            public const int CoinStatusJam = 4;



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposDisp.h
            // *
            // *   Line Display header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 95-12-08 OPOS Release 1.0                                     CRM
            // * 96-03-18 OPOS Release 1.01                                    CRM
            // *   Add DISP_MT_INIT constant and MarqueeFormat constants.
            // * 96-04-22 OPOS Release 1.1                                     CRM
            // *   Add CapCharacterSet values for Kana and Kanji.
            // * 00-09-24 OPOS Release 1.5                                     BKS
            // *   Add CapCharacterSet and CharacterSet constants for UNICODE
            // * 01-07-15 OPOS Release 1.6                                     BKS
            // *   Add CapCursorType, CapCustomGlyph, CapReadBack, CapReverse,
            // *     CursorType property constants.
            // *   Add DefineGlyph, DisplayText and DisplayTextAt parameter
            // *     constants.
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * "CapBlink" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int DispCbNoblink = 0;
            public const int DispCbBlinkall = 1;
            public const int DispCbBlinkeach = 2;


            // *///////////////////////////////////////////////////////////////////
            // * "CapCharacterSet" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int DispCcsNumeric = 0;
            public const int DispCcsAlpha = 1;
            public const int DispCcsAscii = 998;
            public const int DispCcsKana = 10;
            public const int DispCcsKanji = 11;
            public const int DispCcsUnicode = 997;


            // *///////////////////////////////////////////////////////////////////
            // * "CapCursorType" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int DispCctNone = 0x0;
            public const int DispCctFixed = 0x1;
            public const int DispCctBlock = 0x2;
            public const int DispCctHalfblock = 0x4;
            public const int DispCctUnderline = 0x8;
            public const int DispCctReverse = 0x10;
            public const int DispCctOther = 0x20;


            // *///////////////////////////////////////////////////////////////////
            // * "CapReadBack" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int DispCrbNone = 0x0;
            public const int DispCrbSingle = 0x1;


            // *///////////////////////////////////////////////////////////////////
            // * "CapReverse" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int DispCrNone = 0x0;
            public const int DispCrReverseall = 0x1;
            public const int DispCrReverseeach = 0x2;


            // *///////////////////////////////////////////////////////////////////
            // * "CharacterSet" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int DispCsUnicode = 997;
            public const int DispCsAscii = 998;
            public const int DispCsWindows = 999;


            // *///////////////////////////////////////////////////////////////////
            // * "CursorType" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int DispCtNone = 0;
            public const int DispCtFixed = 1;
            public const int DispCtBlock = 2;
            public const int DispCtHalfblock = 3;
            public const int DispCtUnderline = 4;
            public const int DispCtReverse = 5;
            public const int DispCtOther = 6;


            // *///////////////////////////////////////////////////////////////////
            // * "MarqueeType" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int DispMtNone = 0;
            public const int DispMtUp = 1;
            public const int DispMtDown = 2;
            public const int DispMtLeft = 3;
            public const int DispMtRight = 4;
            public const int DispMtInit = 5;


            // *///////////////////////////////////////////////////////////////////
            // * "MarqueeFormat" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int DispMfWalk = 0;
            public const int DispMfPlace = 1;


            // *///////////////////////////////////////////////////////////////////
            // * "DefineGlyph" Method: "GlyphType" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int DispGtSingle = 1;


            // *///////////////////////////////////////////////////////////////////
            // * "DisplayText" Method: "Attribute" Property Constants
            // * "DisplayTextAt" Method: "Attribute" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int DispDtNormal = 0;
            public const int DispDtBlink = 1;
            public const int DispDtReverse = 2;
            public const int DispDtBlinkReverse = 3;


            // *///////////////////////////////////////////////////////////////////
            // * "ScrollText" Method: "Direction" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int DispStUp = 1;
            public const int DispStDown = 2;
            public const int DispStLeft = 3;
            public const int DispStRight = 4;


            // *///////////////////////////////////////////////////////////////////
            // * "SetDescriptor" Method: "Attribute" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int DispSdOff = 0;
            public const int DispSdOn = 1;
            public const int DispSdBlink = 2;



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposFptr.h
            // *
            // *   Fiscal Printer header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 98-03-06 OPOS Release 1.3                                     PDU
            // * 00-09-24 OPOS Release 1.5                                     BKS
            // *   Change CountryCode constants and add code for Russia
            // * 01-07-15 OPOS Release 1.6                                     THH
            // *   Add values for all 1.6 added properties and method
            // *   parameters
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * "ActualCurrency" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int FptrAcBrc = 1;
            public const int FptrAcBgl = 2;
            public const int FptrAcEur = 3;
            public const int FptrAcGrd = 4;
            public const int FptrAcHuf = 5;
            public const int FptrAcItl = 6;
            public const int FptrAcPlz = 7;
            public const int FptrAcRol = 8;
            public const int FptrAcRur = 9;
            public const int FptrAcTrl = 10;


            // *///////////////////////////////////////////////////////////////////
            // * "ContractorId" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int FptrCidFirst = 1;
            public const int FptrCidSecond = 2;
            public const int FptrCidSingle = 3;


            // *///////////////////////////////////////////////////////////////////
            // * Fiscal Printer Station Constants
            // *///////////////////////////////////////////////////////////////////

            public const int FptrSJournal = 1;
            public const int FptrSReceipt = 2;
            public const int FptrSSlip = 4;

            public const int FptrSJournalReceipt = FptrSJournal | FptrSReceipt;


            // *///////////////////////////////////////////////////////////////////
            // * "CountryCode" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int FptrCcBrazil = 1;
            public const int FptrCcGreece = 2;
            public const int FptrCcHungary = 4;
            public const int FptrCcItaly = 8;
            public const int FptrCcPoland = 16;
            public const int FptrCcTurkey = 32;
            public const int FptrCcRussia = 64;
            public const int FptrCcBulgaria = 128;
            public const int FptrCcRomania = 256;


            // *///////////////////////////////////////////////////////////////////
            // * "DateType" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int FptrDtConf = 1;
            public const int FptrDtEod = 2;
            public const int FptrDtReset = 3;
            public const int FptrDtRtc = 4;
            public const int FptrDtVat = 5;


            // *///////////////////////////////////////////////////////////////////
            // * "ErrorLevel" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int FptrElNone = 1;
            public const int FptrElRecoverable = 2;
            public const int FptrElFatal = 3;
            public const int FptrElBlocked = 4;


            // *///////////////////////////////////////////////////////////////////
            // * "ErrorState", "PrinterState" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int FptrPsMonitor = 1;
            public const int FptrPsFiscalReceipt = 2;
            public const int FptrPsFiscalReceiptTotal = 3;
            public const int FptrPsFiscalReceiptEnding = 4;
            public const int FptrPsFiscalDocument = 5;
            public const int FptrPsFixedOutput = 6;
            public const int FptrPsItemList = 7;
            public const int FptrPsLocked = 8;
            public const int FptrPsNonfiscal = 9;
            public const int FptrPsReport = 10;


            // *///////////////////////////////////////////////////////////////////
            // * "FiscalReceiptStation" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int FptrRsReceipt = 1;
            public const int FptrRsSlip = 2;


            // *///////////////////////////////////////////////////////////////////
            // * "FiscalReceiptType" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int FptrRtCashIn = 1;
            public const int FptrRtCashOut = 2;
            public const int FptrRtGeneric = 3;
            public const int FptrRtSales = 4;
            public const int FptrRtService = 5;
            public const int FptrRtSimpleInvoice = 6;


            // *///////////////////////////////////////////////////////////////////
            // * "MessageType" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int FptrMtAdvance = 1;
            public const int FptrMtAdvancePaid = 2;
            public const int FptrMtAmountToBePaid = 3;
            public const int FptrMtAmountToBePaidBack = 4;
            public const int FptrMtCard = 5;
            public const int FptrMtCardNumber = 6;
            public const int FptrMtCardType = 7;
            public const int FptrMtCash = 8;
            public const int FptrMtCashier = 9;
            public const int FptrMtCashRegisterNumber = 10;
            public const int FptrMtChange = 11;
            public const int FptrMtCheque = 12;
            public const int FptrMtClientNumber = 13;
            public const int FptrMtClientSignature = 14;
            public const int FptrMtCounterState = 15;
            public const int FptrMtCreditCard = 16;
            public const int FptrMtCurrency = 17;
            public const int FptrMtCurrencyValue = 18;
            public const int FptrMtDeposit = 19;
            public const int FptrMtDepositReturned = 20;
            public const int FptrMtDotLine = 21;
            public const int FptrMtDriverNumb = 22;
            public const int FptrMtEmptyLine = 23;
            public const int FptrMtFreeText = 24;
            public const int FptrMtFreeTextWithDayLimit = 25;
            public const int FptrMtGivenDiscount = 26;
            public const int FptrMtLocalCredit = 27;
            public const int FptrMtMileageKm = 28;
            public const int FptrMtNote = 29;
            public const int FptrMtPaid = 30;
            public const int FptrMtPayIn = 31;
            public const int FptrMtPointGranted = 32;
            public const int FptrMtPointsBonus = 33;
            public const int FptrMtPointsReceipt = 34;
            public const int FptrMtPointsTotal = 35;
            public const int FptrMtProfited = 36;
            public const int FptrMtRate = 37;
            public const int FptrMtRegisterNumb = 38;
            public const int FptrMtShiftNumber = 39;
            public const int FptrMtStateOfAnAccount = 40;
            public const int FptrMtSubscription = 41;
            public const int FptrMtTable = 42;
            public const int FptrMtThankYouForLoyalty = 43;
            public const int FptrMtTransactionNumb = 44;
            public const int FptrMtValidTo = 45;
            public const int FptrMtVoucher = 46;
            public const int FptrMtVoucherPaid = 47;
            public const int FptrMtVoucherValue = 48;
            public const int FptrMtWithDiscount = 49;
            public const int FptrMtWithoutUplift = 50;


            // *///////////////////////////////////////////////////////////////////
            // * "SlipSelection" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int FptrSsFullLength = 1;
            public const int FptrSsValidation = 2;


            // *///////////////////////////////////////////////////////////////////
            // * "TotalizerType" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int FptrTtDocument = 1;
            public const int FptrTtDay = 2;
            public const int FptrTtReceipt = 3;
            public const int FptrTtGrand = 4;


            // *///////////////////////////////////////////////////////////////////
            // * "GetData" Method Constants
            // *///////////////////////////////////////////////////////////////////

            public const int FptrGdCurrentTotal = 1;
            public const int FptrGdDailyTotal = 2;
            public const int FptrGdReceiptNumber = 3;
            public const int FptrGdRefund = 4;
            public const int FptrGdNotPaid = 5;
            public const int FptrGdMidVoid = 6;
            public const int FptrGdZReport = 7;
            public const int FptrGdGrandTotal = 8;
            public const int FptrGdPrinterId = 9;
            public const int FptrGdFirmware = 10;
            public const int FptrGdRestart = 11;
            public const int FptrGdRefundVoid = 12;
            public const int FptrGdNumbConfigBlock = 13;
            public const int FptrGdNumbCurrencyBlock = 14;
            public const int FptrGdNumbHdrBlock = 15;
            public const int FptrGdNumbResetBlock = 16;
            public const int FptrGdNumbVatBlock = 17;
            public const int FptrGdFiscalDoc = 18;
            public const int FptrGdFiscalDocVoid = 19;
            public const int FptrGdFiscalRec = 20;
            public const int FptrGdFiscalRecVoid = 21;
            public const int FptrGdNonfiscalDoc = 22;
            public const int FptrGdNonfiscalDocVoid = 23;
            public const int FptrGdNonfiscalRec = 24;
            public const int FptrGdSimpInvoice = 25;
            public const int FptrGdTender = 26;
            public const int FptrGdLinecount = 27;
            public const int FptrGdDescriptionLength = 28;

            public const int FptrPdlCash = 1;
            public const int FptrPdlCheque = 2;
            public const int FptrPdlChitty = 3;
            public const int FptrPdlCoupon = 4;
            public const int FptrPdlCurrency = 5;
            public const int FptrPdlDrivenOff = 6;
            public const int FptrPdlEftImprinter = 7;
            public const int FptrPdlEftTerminal = 8;
            public const int FptrPdlTerminalImprinter = 9;
            public const int FptrPdlFreeGift = 10;
            public const int FptrPdlGiro = 11;
            public const int FptrPdlHome = 12;
            public const int FptrPdlImprinterWithIssuer = 13;
            public const int FptrPdlLocalAccount = 14;
            public const int FptrPdlLocalAccountCard = 15;
            public const int FptrPdlPayCard = 16;
            public const int FptrPdlPayCardManual = 17;
            public const int FptrPdlPrepay = 18;
            public const int FptrPdlPumpTest = 19;
            public const int FptrPdlShortCredit = 20;
            public const int FptrPdlStaff = 21;
            public const int FptrPdlVoucher = 22;

            public const int FptrLcItem = 1;
            public const int FptrLcItemVoid = 2;
            public const int FptrLcDiscount = 3;
            public const int FptrLcDiscountVoid = 4;
            public const int FptrLcSurcharge = 5;
            public const int FptrLcSurchargeVoid = 6;
            public const int FptrLcRefund = 7;
            public const int FptrLcRefundVoid = 8;
            public const int FptrLcSubtotalDiscount = 9;
            public const int FptrLcSubtotalDiscountVoid = 10;
            public const int FptrLcSubtotalSurcharge = 11;
            public const int FptrLcSubtotalSurchargeVoid = 12;
            public const int FptrLcComment = 13;
            public const int FptrLcSubtotal = 14;
            public const int FptrLcTotal = 15;

            public const int FptrDlItem = 1;
            public const int FptrDlItemAdjustment = 2;
            public const int FptrDlItemFuel = 3;
            public const int FptrDlItemFuelVoid = 4;
            public const int FptrDlNotPaid = 5;
            public const int FptrDlPackageAdjustment = 6;
            public const int FptrDlRefund = 7;
            public const int FptrDlRefundVoid = 8;
            public const int FptrDlSubtotalAdjustment = 9;
            public const int FptrDlTotal = 10;
            public const int FptrDlVoid = 11;
            public const int FptrDlVoidItem = 12;


            // *///////////////////////////////////////////////////////////////////
            // * "GetTotalizer" Method Constants
            // *///////////////////////////////////////////////////////////////////

            public const int FptrGtGross = 1;
            public const int FptrGtNet = 2;
            public const int FptrGtDiscount = 3;
            public const int FptrGtDiscountVoid = 4;
            public const int FptrGtItem = 5;
            public const int FptrGtItemVoid = 6;
            public const int FptrGtNotPaid = 7;
            public const int FptrGtRefund = 8;
            public const int FptrGtRefundVoid = 9;
            public const int FptrGtSubtotalDiscount = 10;
            public const int FptrGtSubtotalDiscountVoid = 11;
            public const int FptrGtSubtotalSurcharges = 12;
            public const int FptrGtSubtotalSurchargesVoid = 13;
            public const int FptrGtSurcharges = 14;
            public const int FptrGtSSurchargesVoid = 15;
            public const int FptrGtVat = 16;
            public const int FptrGtVatCategory = 17;


            // *///////////////////////////////////////////////////////////////////
            // * "AdjustmentType" arguments in diverse methods
            // *///////////////////////////////////////////////////////////////////

            public const int FptrAtAmountDiscount = 1;
            public const int FptrAtAmountSurcharge = 2;
            public const int FptrAtPercentageDiscount = 3;
            public const int FptrAtPercentageSurcharge = 4;


            // *///////////////////////////////////////////////////////////////////
            // * "ReportType" argument in "PrintReport" method
            // *///////////////////////////////////////////////////////////////////

            public const int FptrRtOrdinal = 1;
            public const int FptrRtDate = 2;


            // *///////////////////////////////////////////////////////////////////
            // * "NewCurrency" argument in "SetCurrency" method
            // *///////////////////////////////////////////////////////////////////

            public const int FptrScEuro = 1;


            // *///////////////////////////////////////////////////////////////////
            // * "StatusUpdateEvent" Event: "Data" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int FptrSueCoverOpen = 11;
            public const int FptrSueCoverOk = 12;

            public const int FptrSueJrnEmpty = 21;
            public const int FptrSueJrnNearempty = 22;
            public const int FptrSueJrnPaperok = 23;

            public const int FptrSueRecEmpty = 24;
            public const int FptrSueRecNearempty = 25;
            public const int FptrSueRecPaperok = 26;

            public const int FptrSueSlpEmpty = 27;
            public const int FptrSueSlpNearempty = 28;
            public const int FptrSueSlpPaperok = 29;

            public const int FptrSueIdle = 1001;


            // *///////////////////////////////////////////////////////////////////
            // * "ResultCodeExtended" Property Constants for Fiscal Printer
            // *///////////////////////////////////////////////////////////////////

            public const int OposEfptrCoverOpen = 201; // (Several)
            public const int OposEfptrJrnEmpty = 202; // (Several)
            public const int OposEfptrRecEmpty = 203; // (Several)
            public const int OposEfptrSlpEmpty = 204; // (Several)
            public const int OposEfptrSlpForm = 205; // EndRemoval
            public const int OposEfptrMissingDevices = 206; // (Several)
            public const int OposEfptrWrongState = 207; // (Several)
            public const int OposEfptrTechnicalAssistance = 208; // (Several)
            public const int OposEfptrClockError = 209; // (Several)
            public const int OposEfptrFiscalMemoryFull = 210; // (Several)
            public const int OposEfptrFiscalMemoryDisconnected = 211; // (Several)
            public const int OposEfptrFiscalTotalsError = 212; // (Several)
            public const int OposEfptrBadItemQuantity = 213; // (Several)
            public const int OposEfptrBadItemAmount = 214; // (Several)
            public const int OposEfptrBadItemDescription = 215; // (Several)
            public const int OposEfptrReceiptTotalOverflow = 216; // (Several)
            public const int OposEfptrBadVat = 217; // (Several)
            public const int OposEfptrBadPrice = 218; // (Several)
            public const int OposEfptrBadDate = 219; // (Several)
            public const int OposEfptrNegativeTotal = 220; // (Several)
            public const int OposEfptrWordNotAllowed = 221; // (Several)
            public const int OposEfptrBadLength = 222; // (Several)
            public const int OposEfptrMissingSetCurrency = 223; // (Several)




            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposKbd.h
            // *
            // *   POS Keyboard header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 96-04-22 OPOS Release 1.1                                     CRM
            // * 97-06-04 OPOS Release 1.2                                     CRM
            // *   Add "EventTypes" and "POSKeyEventType" values.
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * "EventTypes" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int KbdEtDown = 1;
            public const int KbdEtDownUp = 2;


            // *///////////////////////////////////////////////////////////////////
            // * "POSKeyEventType" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int KbdKetKeydown = 1;
            public const int KbdKetKeyup = 2;



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposLock.h
            // *
            // *   Keylock header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 95-12-08 OPOS Release 1.0                                     CRM
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * "KeyPosition" Property Constants
            // * "WaitForKeylockChange" Method: "KeyPosition" Parameter
            // * "StatusUpdateEvent" Event: "Data" Parameter
            // *///////////////////////////////////////////////////////////////////

            public const int LockKpAny = 0; // WaitForKeylockChange Only
            public const int LockKpLock = 1;
            public const int LockKpNorm = 2;
            public const int LockKpSupr = 3;



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposMicr.h
            // *
            // *   MICR header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 95-12-08 OPOS Release 1.0                                     CRM
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * "CheckType" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int MicrCtPersonal = 1;
            public const int MicrCtBusiness = 2;
            public const int MicrCtUnknown = 99;


            // *///////////////////////////////////////////////////////////////////
            // * "CountryCode" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int MicrCcUsa = 1;
            public const int MicrCcCanada = 2;
            public const int MicrCcMexico = 3;
            public const int MicrCcUnknown = 99;


            // *///////////////////////////////////////////////////////////////////
            // * "ResultCodeExtended" Property Constants for MICR
            // *///////////////////////////////////////////////////////////////////

            public const int OposEmicrNocheck = 201; // EndInsertion
            public const int OposEmicrCheck = 202; // EndRemoval



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposMsr.h
            // *
            // *   Magnetic Stripe Reader header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 95-12-08 OPOS Release 1.0                                     CRM
            // * 97-06-04 OPOS Release 1.2                                     CRM
            // *   Add ErrorReportingType values.
            // * 00-09-24 OPOS Release 1.5                                     BKS
            // *   Add constants relating to Track 4 Data
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * "TracksToRead" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int MsrTr1 = 1;
            public const int MsrTr2 = 2;
            public const int MsrTr3 = 4;
            public const int MsrTr4 = 8;

            public const int MsrTr12 = MsrTr1 | MsrTr2;
            public const int MsrTr13 = MsrTr1 | MsrTr3;
            public const int MsrTr14 = MsrTr1 | MsrTr4;
            public const int MsrTr23 = MsrTr2 | MsrTr3;
            public const int MsrTr24 = MsrTr2 | MsrTr4;
            public const int MsrTr34 = MsrTr3 | MsrTr4;

            public const int MsrTr123 = MsrTr1 | MsrTr2 | MsrTr3;
            public const int MsrTr124 = MsrTr1 | MsrTr2 | MsrTr4;
            public const int MsrTr134 = MsrTr1 | MsrTr3 | MsrTr4;
            public const int MsrTr234 = MsrTr2 | MsrTr3 | MsrTr4;

            public const int MsrTr1234 = MsrTr1 | MsrTr2 | MsrTr3 | MsrTr4;


            // *///////////////////////////////////////////////////////////////////
            // * "ErrorReportingType" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int MsrErtCard = 0;
            public const int MsrErtTrack = 1;


            // *///////////////////////////////////////////////////////////////////
            // * "ErrorEvent" Event: "ResultCodeExtended" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int OposEmsrStart = 201;
            public const int OposEmsrEnd = 202;
            public const int OposEmsrParity = 203;
            public const int OposEmsrLrc = 204;


            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposPcrw.H
            // *
            // *   Point Card Reader Writer header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 00-09-24 OPOS Release 1.5                                     BKS
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * "CapCharacterSet" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PcrwCcsAlpha = 1;
            public const int PcrwCcsAscii = 998;
            public const int PcrwCcsKana = 10;
            public const int PcrwCcsKanji = 11;
            public const int PcrwCcsUnicode = 997;


            // *///////////////////////////////////////////////////////////////////
            // * "CardState" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PcrwStateNocard = 1;
            public const int PcrwStateRemaining = 2;
            public const int PcrwStateInrw = 4;


            // *///////////////////////////////////////////////////////////////////
            // * CapTrackToRead and TrackToWrite Property constants
            // *///////////////////////////////////////////////////////////////////

            public const int PcrwTrack1 = 0x1;
            public const int PcrwTrack2 = 0x2;
            public const int PcrwTrack3 = 0x4;
            public const int PcrwTrack4 = 0x8;
            public const int PcrwTrack5 = 0x10;
            public const int PcrwTrack6 = 0x20;


            // *///////////////////////////////////////////////////////////////////
            // * "CharacterSet" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PcrwCsUnicode = 997;
            public const int PcrwCsAscii = 998;
            public const int PcrwCsWindows = 999;


            // *///////////////////////////////////////////////////////////////////
            // * "MappingMode" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PcrwMmDots = 1;
            public const int PcrwMmTwips = 2;
            public const int PcrwMmEnglish = 3;
            public const int PcrwMmMetric = 4;


            // *///////////////////////////////////////////////////////////////////
            // * "ResultCodeExtended" Property Constants for PoinrCardR/W
            // *///////////////////////////////////////////////////////////////////

            public const int OposEpcrwRead = 201;
            public const int OposEpcrwWrite = 202;
            public const int OposEpcrwJam = 203;
            public const int OposEpcrwMotor = 204;
            public const int OposEpcrwCover = 205;
            public const int OposEpcrwPrinter = 206;
            public const int OposEpcrwRelease = 207;
            public const int OposEpcrwDisplay = 208;
            public const int OposEpcrwNocard = 209;


            // *///////////////////////////////////////////////////////////////////
            // * Magnetic read/write status Property Constants for PoinrCardR/W
            // *///////////////////////////////////////////////////////////////////

            public const int OposEpcrwStart = 211;
            public const int OposEpcrwEnd = 212;
            public const int OposEpcrwParity = 213;
            public const int OposEpcrwEncode = 214;
            public const int OposEpcrwLrc = 215;
            public const int OposEpcrwVerify = 216;


            // *///////////////////////////////////////////////////////////////////
            // * "RotatedPrint" Method: "Rotation" Parameter Constants
            // * "RotateSpecial" Property Constants (PCRWRPNORMALASYNC not legal)
            // *///////////////////////////////////////////////////////////////////

            public const int PcrwRpNormal = 0x1;
            public const int PcrwRpNormalasync = 0x2;

            public const int PcrwRpRight90 = 0x101;
            public const int PcrwRpLeft90 = 0x102;
            public const int PcrwRpRotate180 = 0x103;


            // *///////////////////////////////////////////////////////////////////
            // * "StatusUpdateEvent" "Status" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PcrwSueNocard = 1;
            public const int PcrwSueRemaining = 2;
            public const int PcrwSueInrw = 4;


            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposPpad.h
            // *
            // *   PIN Pad header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 98-03-06 OPOS Release 1.3                                     JDB
            // * 00-09-24 OPOS Release 1.5                                     BKS
            // *   Add PpadDispNone for devices with no display
            // *   Add OposEppadBadKey extended result code
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * "CapDisplay" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PpadDispUnrestricted = 1;
            public const int PpadDispPinrestricted = 2;
            public const int PpadDispRestrictedList = 3;
            public const int PpadDispRestrictedOrder = 4;
            public const int PpadDispNone = 5;


            // *///////////////////////////////////////////////////////////////////
            // * "AvailablePromptsList" and "Prompt" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PpadMsgEnterpin = 1;
            public const int PpadMsgPleasewait = 2;
            public const int PpadMsgEntervalidpin = 3;
            public const int PpadMsgRetriesexceeded = 4;
            public const int PpadMsgApproved = 5;
            public const int PpadMsgDeclined = 6;
            public const int PpadMsgCanceled = 7;
            public const int PpadMsgAmountok = 8;
            public const int PpadMsgNotready = 9;
            public const int PpadMsgIdle = 10;
            public const int PpadMsgSlideCard = 11;
            public const int PpadMsgInsertcard = 12;
            public const int PpadMsgSelectcardtype = 13;


            // *///////////////////////////////////////////////////////////////////
            // * "CapLanguage" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PpadLangNone = 1;
            public const int PpadLangOne = 2;
            public const int PpadLangPinrestricted = 3;
            public const int PpadLangUnrestricted = 4;


            // *///////////////////////////////////////////////////////////////////
            // * "TransactionType" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PpadTransDebit = 1;
            public const int PpadTransCredit = 2;
            public const int PpadTransInq = 3;
            public const int PpadTransReconcile = 4;
            public const int PpadTransAdmin = 5;


            // *///////////////////////////////////////////////////////////////////
            // * "EndEFTTransaction" Method Completion Code Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PpadEftNormal = 1;
            public const int PpadEftAbnormal = 2;


            // *///////////////////////////////////////////////////////////////////
            // * "DataEvent" Event Status Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PpadSuccess = 1;
            public const int PpadCancel = 2;


            // *///////////////////////////////////////////////////////////////////
            // * "ResultCodeExtended" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const short OposEppadBadKey = 201;


            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposPtr.h
            // *
            // *   POS Printer header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 95-12-08 OPOS Release 1.0                                     CRM
            // * 96-04-22 OPOS Release 1.1                                     CRM
            // *   Add CapCharacterSet values.
            // *   Add ErrorLevel values.
            // *   Add TransactionPrint Control values.
            // * 97-06-04 OPOS Release 1.2                                     CRM
            // *   Remove PTR_RP_NORMAL_ASYNC.
            // *   Add more barcode symbologies.
            // * 98-03-06 OPOS Release 1.3                                     CRM
            // *   Add more PrintTwoNormal constants.
            // * 00-09-24 OPOS Release 1.5                                   EPSON
            // *   Add CapRecMarkFeed values and MarkFeed constants.
            // *   Add ChangePrintSide constants.
            // *   Add StatusUpdateEvent constants.
            // *   Add ResultCodeExtended values.
            // *   Add CapXxxCartridgeSensor and XxxCartridgeState values.
            // *   Add CartridgeNotify values.
            // * 00-09-24 OPOS Release 1.5                                     BKS
            // *   Add CapCharacterset and CharacterSet values for UNICODE.
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * Printer Station Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PtrSJournal = 1;
            public const int PtrSReceipt = 2;
            public const int PtrSSlip = 4;

            public const int PtrSJournalReceipt = PtrSJournal | PtrSReceipt;
            public const int PtrSJournalSlip = PtrSJournal | PtrSSlip;
            public const int PtrSReceiptSlip = PtrSReceipt | PtrSSlip;

            public const int PtrTwoReceiptJournal = 0x8000 + PtrSJournalReceipt;
            public const int PtrTwoSlipJournal = 0x8000 + PtrSJournalSlip;
            public const int PtrTwoSlipReceipt = 0x8000 + PtrSReceiptSlip;


            // *///////////////////////////////////////////////////////////////////
            // * "CapCharacterSet" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PtrCcsAlpha = 1;
            public const int PtrCcsAscii = 998;
            public const int PtrCcsKana = 10;
            public const int PtrCcsKanji = 11;
            public const int PtrCcsUnicode = 997;


            // *///////////////////////////////////////////////////////////////////
            // * "CharacterSet" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PtrCsUnicode = 997;
            public const int PtrCsAscii = 998;
            public const int PtrCsWindows = 999;


            // *///////////////////////////////////////////////////////////////////
            // * "ErrorLevel" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PtrElNone = 1;
            public const int PtrElRecoverable = 2;
            public const int PtrElFatal = 3;


            // *///////////////////////////////////////////////////////////////////
            // * "MapMode" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PtrMmDots = 1;
            public const int PtrMmTwips = 2;
            public const int PtrMmEnglish = 3;
            public const int PtrMmMetric = 4;


            // *///////////////////////////////////////////////////////////////////
            // * "CapXxxColor" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PtrColorPrimary = 0x1;
            public const int PtrColorCustom1 = 0x2;
            public const int PtrColorCustom2 = 0x4;
            public const int PtrColorCustom3 = 0x8;
            public const int PtrColorCustom4 = 0x10;
            public const int PtrColorCustom5 = 0x20;
            public const int PtrColorCustom6 = 0x40;
            public const int PtrColorCyan = 0x100;
            public const int PtrColorMagenta = 0x200;
            public const int PtrColorYellow = 0x400;
            public const int PtrColorFull = int.MinValue + 0x00000000;

            // *///////////////////////////////////////////////////////////////////
            // * "CapXxxCartridgeSensor" and  "XxxCartridgeState" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PtrCartUnknown = 0x10000000;
            public const int PtrCartOk = 0x0;
            public const int PtrCartRemoved = 0x1;
            public const int PtrCartEmpty = 0x2;
            public const int PtrCartNearend = 0x4;
            public const int PtrCartCleaning = 0x8;

            // *///////////////////////////////////////////////////////////////////
            // * "CartridgeNotify"  Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PtrCnDisabled = 0x0;
            public const int PtrCnEnabled = 0x1;


            // *///////////////////////////////////////////////////////////////////
            // * "CutPaper" Method Constant
            // *///////////////////////////////////////////////////////////////////

            public const int PtrCpFullcut = 100;


            // *///////////////////////////////////////////////////////////////////
            // * "PrintBarCode" Method Constants:
            // *///////////////////////////////////////////////////////////////////

            // *   "Alignment" Parameter
            // *     Either the distance from the left-most print column to the start
            // *     of the bar code, or one of the following:

            public const int PtrBcLeft = -1;
            public const int PtrBcCenter = -2;
            public const int PtrBcRight = -3;

            // *   "TextPosition" Parameter

            public const int PtrBcTextNone = -11;
            public const int PtrBcTextAbove = -12;
            public const int PtrBcTextBelow = -13;

            // *   "Symbology" Parameter:

            // *     One dimensional symbologies
            public const int PtrBcsUpca = 101; // Digits
            public const int PtrBcsUpce = 102; // Digits
            public const int PtrBcsJan8 = 103; // = EAN 8
            public const int PtrBcsEan8 = 103; // = JAN 8 (added in 1.2)
            public const int PtrBcsJan13 = 104; // = EAN 13
            public const int PtrBcsEan13 = 104; // = JAN 13 (added in 1.2)
            public const int PtrBcsTf = 105; // (Discrete 2 of 5) Digits
            public const int PtrBcsItf = 106; // (Interleaved 2 of 5) Digits
            public const int PtrBcsCodabar = 107; // Digits, -, $, :, /, ., +;
                                                  // 4 start/stop characters
                                                  // (a, b, c, d)
            public const int PtrBcsCode39 = 108; // Alpha, Digits, Space, -, .,
                                                 // $, /, +, %; start/stop (*)
                                                 // Also has Full ASCII feature
            public const int PtrBcsCode93 = 109; // Same characters as Code 39
            public const int PtrBcsCode128 = 110; // 128 data characters
                                                  // *        (The following were added in Release 1.2)
            public const int PtrBcsUpcaS = 111; // UPC-A with supplemental
                                                // barcode
            public const int PtrBcsUpceS = 112; // UPC-E with supplemental
                                                // barcode
            public const int PtrBcsUpcd1 = 113; // UPC-D1
            public const int PtrBcsUpcd2 = 114; // UPC-D2
            public const int PtrBcsUpcd3 = 115; // UPC-D3
            public const int PtrBcsUpcd4 = 116; // UPC-D4
            public const int PtrBcsUpcd5 = 117; // UPC-D5
            public const int PtrBcsEan8S = 118; // EAN 8 with supplemental
                                                // barcode
            public const int PtrBcsEan13S = 119; // EAN 13 with supplemental
                                                 // barcode
            public const int PtrBcsEan128 = 120; // EAN 128
            public const int PtrBcsOcra = 121; // OCR "A"
            public const int PtrBcsOcrb = 122; // OCR "B"


            // *     Two dimensional symbologies
            public const int PtrBcsPdf417 = 201;
            public const int PtrBcsMaxicode = 202;

            // *     Start of Printer-Specific bar code symbologies
            public const int PtrBcsOther = 501;


            // *///////////////////////////////////////////////////////////////////
            // * "PrintBitmap" Method Constants:
            // *///////////////////////////////////////////////////////////////////

            // *   "Width" Parameter
            // *     Either bitmap width or:

            public const int PtrBmAsis = -11; // One pixel per printer dot

            // *   "Alignment" Parameter
            // *     Either the distance from the left-most print column to the start
            // *     of the bitmap, or one of the following:

            public const int PtrBmLeft = -1;
            public const int PtrBmCenter = -2;
            public const int PtrBmRight = -3;


            // *///////////////////////////////////////////////////////////////////
            // * "RotatePrint" Method: "Rotation" Parameter Constants
            // * "RotateSpecial" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PtrRpNormal = 0x1;
            public const int PtrRpRight90 = 0x101;
            public const int PtrRpLeft90 = 0x102;
            public const int PtrRpRotate180 = 0x103;


            // *///////////////////////////////////////////////////////////////////
            // * "SetLogo" Method: "Location" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PtrLTop = 1;
            public const int PtrLBottom = 2;


            // *///////////////////////////////////////////////////////////////////
            // * "TransactionPrint" Method: "Control" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PtrTpTransaction = 11;
            public const int PtrTpNormal = 12;


            // *///////////////////////////////////////////////////////////////////
            // * "MarkFeed" Method: "Type" Parameter Constants
            // * "CapRecMarkFeed" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const short PtrMfToTakeup = 1;
            public const short PtrMfToCutter = 2;
            public const short PtrMfToCurrentTof = 4;
            public const short PtrMfToNextTof = 8;

            // *///////////////////////////////////////////////////////////////////
            // * "ChangePrintSide" Method: "Side" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const short PtrPsUnknown = 0;
            public const short PtrPsSide1 = 1;
            public const short PtrPsSide2 = 2;
            public const short PtrPsOpposite = 3;


            // *///////////////////////////////////////////////////////////////////
            // * "StatusUpdateEvent" Event: "Data" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int PtrSueCoverOpen = 11;
            public const int PtrSueCoverOk = 12;

            public const int PtrSueJrnEmpty = 21;
            public const int PtrSueJrnNearempty = 22;
            public const int PtrSueJrnPaperok = 23;

            public const int PtrSueRecEmpty = 24;
            public const int PtrSueRecNearempty = 25;
            public const int PtrSueRecPaperok = 26;

            public const int PtrSueSlpEmpty = 27;
            public const int PtrSueSlpNearempty = 28;
            public const int PtrSueSlpPaperok = 29;

            public const short PtrSueJrnCartridgeEmpty = 41;
            public const short PtrSueJrnCartridgeNearempty = 42;
            public const short PtrSueJrnHeadCleaning = 43;
            public const short PtrSueJrnCartridgeOk = 44;

            public const short PtrSueRecCartridgeEmpty = 45;
            public const short PtrSueRecCartridgeNearempty = 46;
            public const short PtrSueRecHeadCleaning = 47;
            public const short PtrSueRecCartridgeOk = 48;

            public const short PtrSueSlpCartridgeEmpty = 49;
            public const short PtrSueSlpCartridgeNearempty = 50;
            public const short PtrSueSlpHeadCleaning = 51;
            public const short PtrSueSlpCartridgeOk = 52;

            public const int PtrSueIdle = 1001;


            // *///////////////////////////////////////////////////////////////////
            // * "ResultCodeExtended" Property Constants for Printer
            // *///////////////////////////////////////////////////////////////////

            public const int OposEptrCoverOpen = 201; // (Several)
            public const int OposEptrJrnEmpty = 202; // (Several)
            public const int OposEptrRecEmpty = 203; // (Several)
            public const int OposEptrSlpEmpty = 204; // (Several)
            public const int OposEptrSlpForm = 205; // EndRemoval
            public const int OposEptrToobig = 206; // PrintBitmap
            public const int OposEptrBadformat = 207; // PrintBitmap
            public const short OposEptrJrnCartridgeRemoved = 208; // (Several)
            public const short OposEptrJrnCartridgeEmpty = 209; // (Several)
            public const short OposEptrJrnHeadCleaning = 210; // (Several)
            public const short OposEptrRecCartridgeRemoved = 211; // (Several)
            public const short OposEptrRecCartridgeEmpty = 212; // (Several)
            public const short OposEptrRecHeadCleaning = 213; // (Several)
            public const short OposEptrSlpCartridgeRemoved = 214; // (Several)
            public const short OposEptrSlpCartridgeEmpty = 215; // (Several)
            public const short OposEptrSlpHeadCleaning = 216; // (Several)



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposPwr.h
            // *
            // *   POSPower header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 99-02-22 OPOS Release 1.x                                     AL
            // * 99-09-13 OPOS Release 1.x                                     TH
            // *            ACCU -> UPS, FAN_ALARM and HEAT_ALARM added
            // * 99-12-06 OPOS Release 1.x                                     TH
            // *            FAN_ALARM and HEAT_ALARM changed to FAN_STOPPED,
            // *          FAN_RUNNING, TEMPERATURE_HIGH and TEMPERATURE_OK
            // * 00-09-24 OPOS Release 1.5                                     TH
            // *          SHUTDOWN added
            // *
            // *///////////////////////////////////////////////////////////////////

            // *///////////////////////////////////////////////////////////////////
            // * "UPSChargeState" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const short PwrUpsFull = 1;
            public const short PwrUpsWarning = 2;
            public const short PwrUpsLow = 4;
            public const short PwrUpsCritical = 8;


            // *///////////////////////////////////////////////////////////////////
            // * "StatusUpdateEvent" Event: "Status" Parameter
            // *///////////////////////////////////////////////////////////////////

            public const short PwrSueUpsFull = 11;
            public const short PwrSueUpsWarning = 12;
            public const short PwrSueUpsLow = 13;
            public const short PwrSueUpsCritical = 14;
            public const short PwrSueFanStopped = 15;
            public const short PwrSueFanRunning = 16;
            public const short PwrSueTemperatureHigh = 17;
            public const short PwrSueTemperatureOk = 18;
            public const short PwrSueShutdown = 19;



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposRod.h
            // *
            // *   Remote Order Display header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 98-03-06 OPOS Release 1.3                                     BB
            // * 00-09-24 OPOS Release 1.5                                    BKS
            // *   Added CharacterSet value for UNICODE.
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * "CurrentUnitID" and "UnitsOnline" Properties
            // *  and "Units" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int RodUid1 = 0x1;
            public const int RodUid2 = 0x2;
            public const int RodUid3 = 0x4;
            public const int RodUid4 = 0x8;
            public const int RodUid5 = 0x10;
            public const int RodUid6 = 0x20;
            public const int RodUid7 = 0x40;
            public const int RodUid8 = 0x80;
            public const int RodUid9 = 0x100;
            public const int RodUid10 = 0x200;
            public const int RodUid11 = 0x400;
            public const int RodUid12 = 0x800;
            public const int RodUid13 = 0x1000;
            public const int RodUid14 = 0x2000;
            public const int RodUid15 = 0x4000;
            public const int RodUid16 = 0x8000;
            public const int RodUid17 = 0x10000;
            public const int RodUid18 = 0x20000;
            public const int RodUid19 = 0x40000;
            public const int RodUid20 = 0x80000;
            public const int RodUid21 = 0x100000;
            public const int RodUid22 = 0x200000;
            public const int RodUid23 = 0x400000;
            public const int RodUid24 = 0x800000;
            public const int RodUid25 = 0x1000000;
            public const int RodUid26 = 0x2000000;
            public const int RodUid27 = 0x4000000;
            public const int RodUid28 = 0x8000000;
            public const int RodUid29 = 0x10000000;
            public const int RodUid30 = 0x20000000;
            public const int RodUid31 = 0x40000000;
            public const int RodUid32 = int.MinValue + 0x00000000;


            // *///////////////////////////////////////////////////////////////////
            // * Broadcast Methods: "Attribute" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int RodAttrBlink = 0x80;

            public const int RodAttrBgBlack = 0x0;
            public const int RodAttrBgBlue = 0x10;
            public const int RodAttrBgGreen = 0x20;
            public const int RodAttrBgCyan = 0x30;
            public const int RodAttrBgRed = 0x40;
            public const int RodAttrBgMagenta = 0x50;
            public const int RodAttrBgBrown = 0x60;
            public const int RodAttrBgGray = 0x70;

            public const int RodAttrIntensity = 0x8;

            public const int RodAttrFgBlack = 0x0;
            public const int RodAttrFgBlue = 0x1;
            public const int RodAttrFgGreen = 0x2;
            public const int RodAttrFgCyan = 0x3;
            public const int RodAttrFgRed = 0x4;
            public const int RodAttrFgMagenta = 0x5;
            public const int RodAttrFgBrown = 0x6;
            public const int RodAttrFgGray = 0x7;


            // *///////////////////////////////////////////////////////////////////
            // * "DrawBox" Method: "BorderType" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int RodBdrSingle = 1;
            public const int RodBdrDouble = 2;
            public const int RodBdrSolid = 3;


            // *///////////////////////////////////////////////////////////////////
            // * "ControlClock" Method: "Function" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int RodClkStart = 1;
            public const int RodClkPause = 2;
            public const int RodClkResume = 3;
            public const int RodClkMove = 4;
            public const int RodClkStop = 5;


            // *///////////////////////////////////////////////////////////////////
            // * "ControlCursor" Method: "Function" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int RodCrsLine = 1;
            public const int RodCrsLineBlink = 2;
            public const int RodCrsBlock = 3;
            public const int RodCrsBlockBlink = 4;
            public const int RodCrsOff = 5;


            // *///////////////////////////////////////////////////////////////////
            // * "SelectCharacterSet" Method: "CharacterSet" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int RodCsUnicode = 997;
            public const int RodCsAscii = 998;
            public const int RodCsWindows = 999;


            // *///////////////////////////////////////////////////////////////////
            // * "TransactionDisplay" Method: "Function" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int RodTdTransaction = 11;
            public const int RodTdNormal = 12;


            // *///////////////////////////////////////////////////////////////////
            // * "UpdateVideoRegionAttribute" Method: "Function" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int RodUaSet = 1;
            public const int RodUaIntensityOn = 2;
            public const int RodUaIntensityOff = 3;
            public const int RodUaReverseOn = 4;
            public const int RodUaReverseOff = 5;
            public const int RodUaBlinkOn = 6;
            public const int RodUaBlinkOff = 7;


            // *///////////////////////////////////////////////////////////////////
            // * "EventTypes" Property and "DataEvent" Event: "Status" Parameter Constants
            // *///////////////////////////////////////////////////////////////////

            public const int RodDeTouchUp = 0x1;
            public const int RodDeTouchDown = 0x2;
            public const int RodDeTouchMove = 0x4;


            // *///////////////////////////////////////////////////////////////////
            // * "ResultCodeExtended" Property Constants for Remote Order Display
            // *///////////////////////////////////////////////////////////////////

            public const int OposErodBadclk = 201; // ControlClock
            public const int OposErodNoclocks = 202; // ControlClock
            public const int OposErodNoregion = 203; // RestoreVideoRegion
            public const int OposErodNobuffers = 204; // SaveVideoRegion
            public const int OposErodNoroom = 205; // SaveVideoRegion



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposScal.h
            // *
            // *   Scale header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 95-12-08 OPOS Release 1.0                                     CRM
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * "WeightUnit" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int ScalWuGram = 1;
            public const int ScalWuKilogram = 2;
            public const int ScalWuOunce = 3;
            public const int ScalWuPound = 4;


            // *///////////////////////////////////////////////////////////////////
            // * "ResultCodeExtended" Property Constants for Scale
            // *///////////////////////////////////////////////////////////////////

            public const int OposEscalOverweight = 201; // ReadWeight



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposScan.h
            // *
            // *   Scanner header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 95-12-08 OPOS Release 1.0                                     CRM
            // * 97-06-04 OPOS Release 1.2                                     CRM
            // *   Add "ScanDataType" values.
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * "ScanDataType" Property Constants
            // *///////////////////////////////////////////////////////////////////

            // * One dimensional symbologies
            public const int ScanSdtUpca = 101; // Digits
            public const int ScanSdtUpce = 102; // Digits
            public const int ScanSdtJan8 = 103; // = EAN 8
            public const int ScanSdtEan8 = 103; // = JAN 8 (added in 1.2)
            public const int ScanSdtJan13 = 104; // = EAN 13
            public const int ScanSdtEan13 = 104; // = JAN 13 (added in 1.2)
            public const int ScanSdtTf = 105; // (Discrete 2 of 5) Digits
            public const int ScanSdtItf = 106; // (Interleaved 2 of 5) Digits
            public const int ScanSdtCodabar = 107; // Digits, -, $, :, /, ., +;
                                                   // 4 start/stop characters
                                                   // (a, b, c, d)
            public const int ScanSdtCode39 = 108; // Alpha, Digits, Space, -, .,
                                                  // $, /, +, %; start/stop (*)
                                                  // Also has Full ASCII feature
            public const int ScanSdtCode93 = 109; // Same characters as Code 39
            public const int ScanSdtCode128 = 110; // 128 data characters

            public const int ScanSdtUpcaS = 111; // UPC-A with supplemental
                                                 // barcode
            public const int ScanSdtUpceS = 112; // UPC-E with supplemental
                                                 // barcode
            public const int ScanSdtUpcd1 = 113; // UPC-D1
            public const int ScanSdtUpcd2 = 114; // UPC-D2
            public const int ScanSdtUpcd3 = 115; // UPC-D3
            public const int ScanSdtUpcd4 = 116; // UPC-D4
            public const int ScanSdtUpcd5 = 117; // UPC-D5
            public const int ScanSdtEan8S = 118; // EAN 8 with supplemental
                                                 // barcode
            public const int ScanSdtEan13S = 119; // EAN 13 with supplemental
                                                  // barcode
            public const int ScanSdtEan128 = 120; // EAN 128
            public const int ScanSdtOcra = 121; // OCR "A"
            public const int ScanSdtOcrb = 122; // OCR "B"

            // * Two dimensional symbologies
            public const int ScanSdtPdf417 = 201;
            public const int ScanSdtMaxicode = 202;

            // * Special cases
            public const int ScanSdtOther = 501; // Start of Scanner-Specific bar
                                                 // code symbologies
            public const int ScanSdtUnknown = 0; // Cannot determine the barcode
                                                 // symbology.



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposSig.h
            // *
            // *   Signature Capture header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 95-12-08 OPOS Release 1.0                                     CRM
            // *
            // *///////////////////////////////////////////////////////////////////


            // * No definitions required for this version.



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposTone.h
            // *
            // *   Tone Indicator header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 97-06-04 OPOS Release 1.2                                     CRM
            // *
            // *///////////////////////////////////////////////////////////////////


            // * No definitions required for this version.



            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposTot.h
            // *
            // *   Hard Totals header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 95-12-08 OPOS Release 1.0                                     CRM
            // *
            // *///////////////////////////////////////////////////////////////////


            // *///////////////////////////////////////////////////////////////////
            // * "ResultCodeExtended" Property Constants for Hard Totals
            // *///////////////////////////////////////////////////////////////////

            public const int OposEtotNoroom = 201; // Create, Write
            public const int OposEtotValidation = 202; // Read, Write

            // *///////////////////////////////////////////////////////////////////
            // *
            // * OposRfid.h
            // *
            // *   RFID Scanner header file for OPOS Applications.
            // *
            // * Modification history
            // * ------------------------------------------------------------------
            // * 07-08-17 OPOS Release 1.12                                     TEC
            // *
            // *///////////////////////////////////////////////////////////////////

            // *///////////////////////////////////////////////////////////////////
            // * "CapMultipleProtocols" & "ProtocolMask" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int RfidPrEpc0 = 0x1; // EPC Class 0 read-only passive tags
            public const int RfidPr0plus = 0x2; // Non-EPC Class “0+” write once passive tags
            public const int RfidPrEpc1 = 0x4; // EPC Class 1 write once passive tags
            public const int RfidPrEpc1g2 = 0x8; // EPC Class 1 Gen 2 write once passive tags
            public const int RfidPrEpc2 = 0x10; // EPC Class 2 rewritable tags
            public const int RfidPrIso14443A = 0x1000; // ISO 14443A HF tags
            public const int RfidPrIso14443B = 0x2000; // ISO 14443B HF tags
            public const int RfidPrIso15693 = 0x3000; // ISO 15693 HF tags
            public const int RfidPrIso180006B = 0x4000; // ISO 18000-6B UHF tags
            public const int RfidPrOther = 0x1000000; // A tag that does not fit into one of the defined protocols
            public const int RfidPrAll = 0x40000000; // (ProtocolMask only)

            // *///////////////////////////////////////////////////////////////////
            // * "CapWriteTag" Property Constants
            // *///////////////////////////////////////////////////////////////////

            public const int RfidCwtNone = 0; // No writable fields in the tag
            public const int RfidCwtId = 1; // The ID field in the tag is writable
            public const int RfidCwtUserData = 2; // The UserData field in the tag is writable
            public const int RfidCwtAll = 3; // All fields in the tag are writable

            // *///////////////////////////////////////////////////////////////////
            // * "ReadTags","StartReadTags" Methods Constants
            // *///////////////////////////////////////////////////////////////////

            public const int RfidRtId = 0x10; // Read only the ID data
            public const int RfidRtFullUserData = 0x1; // Read the full UserData
            public const int RfidRtPartialUserData = 0x2; // Read the defined partial UserData
            public const int RfidRtIdFullUserData = 0x11; // Read the ID and full UserData
            public const int RfidRtIdPartialUserData = 0x12; // Read the ID and 
        }

        private void btnInsert_CheckedChanged(object sender, EventArgs e)
        {
            btnDeleteSingle2.Visible = false;
            resetLabel(1);
            //opos.OPOS_StartReading(Session.OPOSRFID1);
            //DialogResult confirmResult = MessageBox.Show("Switching to Insert mode, are you sure?", "Confirm Diaglog", MessageBoxButtons.YesNo);
            //if (confirmResult == DialogResult.No)
            //{
            //    btnInsert.Checked = false;
            //    btnDelete.Checked = true;
            //} 

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnDeleteManual_CheckedChanged(object sender, EventArgs e)
        {
            if (btnDeleteManual.Checked == true)
            {
                DialogResult confirmResult = MessageBox.Show("Are you sure you want to switch to delete mode?", "Confirm Diaglog", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.No)
                {
                    btnInsert.Checked = true;
                    btnDeleteManual.Checked = false;
                }
                else
                {
                    btnInsert.Checked = false;
                    btnDeleteManual.Checked = true;
                    resetLabel(1);
                    btnDeleteSingle2.Visible = true;
                    //opos.OPOS_StartReading(Session.OPOSRFID1);

                }
            }
        }

        private void btnDeleteSingle_Click(object sender, EventArgs e)
        {
            if (!Session.rfidcode.Equals("") && !btnInsert.Checked && btnDeleteManual.Checked)
            {
                txtRfid.Text = Session.rfidcode;
                Task.Run(() => ApiDelete()).Wait();
                richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": " + api_message + "\n";
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            // scroll it automatically
            richTextBox1.ScrollToCaret();
        }

        private void btnDeleteSingle_Click_1(object sender, EventArgs e)
        {
            if (!Session.rfidcode.Equals("") && Session.barcode.Equals("") && !btnInsert.Checked && btnDeleteManual.Checked)
            {
                txtRfid.Text = Session.rfidcode;
                Task.Run(() => ApiDelete()).Wait();
                richTextBox1.Text += DateTime.Now.ToString("hh:mm:ss") + ": " + api_message + "\n";
            }
        }

        private void txtPName_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lJanCode_Click(object sender, EventArgs e)
        {

        }

        private void lRCode_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Front_Load(object sender, EventArgs e)
        {

        }

        private void txtProductID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
