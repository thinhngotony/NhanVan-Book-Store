using com.gigatms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SelfRegi_V2.Front;

namespace SelfRegi_V2
{
    class Session 
    {
        //TS_800////TS_800////TS_800////TS_800////TS_800////TS_800////TS_800////TS_800////TS_800////TS_800////TS_800//
        public static Host _host = new Host();
        public static TS800 _ts800 = new TS800(3, 3);
        public static List<string> _tagList = new List<string>();
        public static bool m_bIsInventoryProcessing = false;
        

        //RFID//////RFID//////RFID//////RFID//////RFID//////RFID//////RFID//////RFID//////RFID//////RFID//////RFID//
        public static AxOPOSRFIDLib.AxOPOSRFID OPOSRFID1 = new AxOPOSRFIDLib.AxOPOSRFID();
        public static Front front;
        public static string device_name = "";
        public static string rfidcode = "";
        //E2806810000000391B7FA508
        //public static string rfidcode = "E2806810000000391B7FA508";
        public static string barcode = "";
        //9784003102787
        public static int rT = 500;
        public static bool force_update = true;

        //API//////API//////API//////API//////API//////API//////API//////API//////API//////API//////API//////API////
        public static string rfmaster_api = "";
        public static string rfmaster_sub = "";
        public static string rfmaster_sub_delete = "";
        public static string rfmaster_key = "";

        public static string haravan_api = "";
        public static string haravan_sub = "";
        public static string haravan_key_name = "";
        public static string haravan_key_pass = "";


        public static string SHOPCD = "";
        public static string reload = "";

        public static string sync_api = "";
        public static string sync_sub = "";
        //readCSV//////readCSV//////readCSV//////readCSV//////readCSV//////readCSV//////readCSV//////readCSV////
        public static string path = "";
        public static ProductData product = new ProductData();

        //BarcodeFormat//////BarcodeFormat//////BarcodeFormat//////BarcodeFormat//////BarcodeFormat//////BarcodeFormat////
        public static int JanLen = 13;
    }
}
