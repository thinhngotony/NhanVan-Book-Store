using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SeleniumCSharp;
using OpenQA.Selenium.Chrome;

namespace SeleniumCSharp
{
    class WebModule
    {
        public struct DislayedProduct
        {
            public string barcode { get; set; }
            public string count { get; set; }
        }

        public static List <DislayedProduct> getlistProduct(IWebDriver driver) 
        {
            List<DislayedProduct> list_product = new List<DislayedProduct>(); 
            ICollection <IWebElement> element_barcode = driver.FindElements(By.ClassName("product-code"));
            ICollection<IWebElement> element_count = driver.FindElements(By.ClassName("input-decimal"));
            
           for(int i = 0; i < element_count.Count(); i++)
            {
                DislayedProduct pro = new DislayedProduct();
                pro.barcode = element_barcode.ToList()[i * 2].Text;
                pro.count = element_count.ToList()[i].GetAttribute("value");
                list_product.Add(pro);
            }
            return list_product;
        }

        public static List<DislayedProduct> getTable(IWebDriver driver)
        {
            List<DislayedProduct> list_product = new List<DislayedProduct>();

            IWebElement tableElement = driver.FindElement(By.ClassName("table-cart-item"));
            IList<IWebElement> tableRow = tableElement.FindElements(By.TagName("variant"));
            IList<IWebElement> rowTD;
            foreach (IWebElement row in tableRow)
            {
                rowTD = row.FindElements(By.TagName("td"));
                foreach (IWebElement rTD in rowTD)
                {
                    try
                    {
                        IWebElement rtd_barcode = rowTD[1];
                        IWebElement infor_code = rtd_barcode.FindElement(By.TagName("div"));
                        IWebElement barcode_element = infor_code.FindElement(By.ClassName("product-code"));

                        try
                        {
                            IWebElement rtd_x = rowTD[3];
                            IWebElement tail_btn = rtd_x.FindElement(By.TagName("div"));
                            IWebElement count_element = tail_btn.FindElement(By.TagName("input"));

                            DislayedProduct pro = new DislayedProduct();
                            pro.barcode = barcode_element.Text;
                            pro.count = count_element.GetAttribute("value").ToString();
                            list_product.Add(pro);
                        }
                        catch { }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("=====> GET TABLE: " + e);
                    }
                }               
            }
            //Console.WriteLine(list_product.Count());
            return list_product;
        }

        public static string getTitleWindow(IWebDriver driver) {
            return driver.Title;
        }

        public static void DeleteAllFromSreen(IWebDriver driver)
        {
            IWebElement tableElement = driver.FindElement(By.ClassName("table-cart-item"));
            IList<IWebElement> tableRow = tableElement.FindElements(By.ClassName("variant"));
            IList<IWebElement> rowTD;
            foreach (IWebElement row in tableRow)
            {
                rowTD = row.FindElements(By.TagName("td"));
                try
                {
                    IWebElement rtd_barcode = rowTD[1];
                    //IWebElement infor_code = rtd_barcode.FindElement(By.TagName("div"));
                    //IWebElement barcode_element = infor_code.FindElement(By.ClassName("product-code"));
                    IWebElement barcode_element = rtd_barcode.FindElement(By.ClassName("product-code"));
                    try
                        {
                         IWebElement rtd_x = rowTD.Last();
                        IWebElement delete_btn = rtd_x.FindElement(By.TagName("span"));
                        //IWebElement tail_btn = rtd_x.FindElement(By.TagName("div"));
                        //IWebElement delete_btn = tail_btn.FindElement(By.TagName("span"));
                        delete_btn.Click();
                         System.Threading.Thread.Sleep(GlobalVariables.config.wait_api);
                       }
                    catch(Exception e) { Console.WriteLine("=====> DELETE ALL SUB: "+e);
                    }                    
                }
                catch(Exception e) { Console.WriteLine("=====> DELETE ALL: "+e); }
            }
        }

        public static void DeleteFromSreen(string barcode, IWebDriver driver)
        {
            try
            {
                IWebElement tableElement = driver.FindElement(By.ClassName("table-cart-item"));
                IList<IWebElement> tableRow = tableElement.FindElements(By.ClassName("variant"));
                IList<IWebElement> rowTD;

                //IWebElement rowTD;
                foreach (IWebElement row in tableRow)
                {
                    rowTD = row.FindElements(By.TagName("td"));

                    try
                    {
                        IWebElement rtd_barcode = rowTD[1];
                        IWebElement barcode_element = rtd_barcode.FindElement(By.ClassName("product-code"));
                        //IWebElement infor_code = rtd_barcode.FindElement(By.TagName("div"));
                        //IWebElement barcode_element = infor_code.FindElement(By.ClassName("product-code"));

                        if (barcode == barcode_element.Text)
                        {
                            try
                            {
                                IWebElement rtd_x = rowTD.Last();
                                IWebElement delete_btn = rtd_x.FindElement(By.TagName("span"));
                                //IWebElement tail_btn = rtd_x.FindElement(By.TagName("div"));
                                //IWebElement delete_btn = tail_btn.FindElement(By.TagName("span"));
                                delete_btn.Click();
                                break;
                            }
                            catch (Exception e) { Console.WriteLine(e); }
                        }
                    }
                    catch (Exception e) { Console.WriteLine("=====> DELETE BARCODE: " + e); }
                }
            }
            catch
            {
                Console.WriteLine("DELETE: CAN'T FIND TABLE!");
            }
        }

        public static void EditCountFromScreen(string barcode, string new_count, IWebDriver driver)
        {
            IWebElement tableElement = driver.FindElement(By.ClassName("table-cart-item"));
            IList<IWebElement> tableRow = tableElement.FindElements(By.ClassName("variant"));
            IList<IWebElement> rowTD;
            foreach (IWebElement row in tableRow)
            {
                rowTD = row.FindElements(By.TagName("td"));

                try
                {
                    IWebElement rtd_barcode = rowTD[1];
                    IWebElement infor_code = rtd_barcode.FindElement(By.TagName("div"));
                    IWebElement barcode_element = infor_code.FindElement(By.ClassName("product-code"));

                    if (barcode == barcode_element.Text)
                    {
                        try
                        {
                            IWebElement rtd_x = rowTD[3];
                            IWebElement tail_btn = rtd_x.FindElement(By.TagName("div"));
                            IWebElement count = tail_btn.FindElement(By.TagName("input"));
                            count.SendKeys(new_count);
                        }
                        catch (Exception e) { Console.WriteLine(e); }
                    }
                }
                catch (Exception e) { Console.WriteLine("=====> EDIT COUNT: " + e); }
            }
        }

        public static void CompareList(List <DislayedProduct> onSreendata, List<DislayedProduct> readData, IWebDriver driver)
        {
            onSreendata.OrderBy(o => o.barcode);
            readData.OrderBy(o => o.barcode);
            foreach (DislayedProduct item in onSreendata)
            {
                if (!readData.Contains(item))
                {
                    DeleteFromSreen(item.barcode, driver);
                }
            }
        }

        public static bool checkNoneProduct(IWebDriver driver)
        {
            try
            {
                IWebElement tableElement = driver.FindElement(By.ClassName("table-cart-item"));
                IWebElement tableHead = tableElement.FindElement(By.TagName("thead"));
                int num_pro = Int32.Parse(tableHead.FindElement(By.TagName("span")).Text);

                if (num_pro == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                Console.WriteLine("BROWSER OFF!");
                return false;
            }
                   
        }

        public static void StateSale()
        {

        }

    }
}
