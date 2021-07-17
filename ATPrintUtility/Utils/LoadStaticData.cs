using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AT.Print.Utils
{
    public static class LoadStaticData
    {
        public static Hashtable _categorySlabs = new Hashtable();
        public static Hashtable _HindiMessage = new Hashtable();
        public static Hashtable _EnglishMessage = new Hashtable();
        public static List<BroadcastMessage> _BroadcastMessage = new List<BroadcastMessage>();

        public static string EbillOutputLocation;
        public static void LoadData()
        {
            _categorySlabs = new Hashtable();
            _HindiMessage = new Hashtable();
            _EnglishMessage = new Hashtable();
            _BroadcastMessage = new List<BroadcastMessage>();
            EbillOutputLocation = string.Empty;
            try
            {
                XDocument priceSlabMapping = XDocument.Load(Application.StartupPath + "//DBEntity//PriceRateSlab.xml");
                var priceSlabElements = priceSlabMapping.Root.Elements();
                foreach (var priceSlabElement in priceSlabElements)
                {
                    switch (priceSlabElement.Name.ToString())
                    {
                        case "Category":
                            foreach (var priceSlab in priceSlabElement.Elements())
                            {
                                _categorySlabs.Add(priceSlab.Attribute("Slab").Value, priceSlab.Attribute("ImageName").Value);
                            }
                            break;
                    }
                }
                LoadMessages();
            }
            catch (Exception ex)
            {
                AppFunctions.LogError(ex);
            }
        }

        public static void LoadMessages()
        {

            try
            {
                XDocument Messages = XDocument.Load(Application.StartupPath + "//DBEntity//xMessage.xml");
                var XMessages = Messages.Root.Elements();
                foreach (var XMessage in XMessages)
                {
                    switch (XMessage.Name.ToString())
                    {
                        case "Hindi":
                            foreach (var mElement in XMessage.Elements())
                            {
                                _HindiMessage.Add(mElement.Attribute("Code").Value, mElement.Attribute("Description").Value);
                            }
                            break;
                        case "English":
                            foreach (var mElement in XMessage.Elements())
                            {
                                _EnglishMessage.Add(mElement.Attribute("Code").Value, mElement.Attribute("Description").Value);
                            }
                            break;
                        case "BROADCAST":
                            foreach (var mElement in XMessage.Elements())
                            {
                                _BroadcastMessage.Add(new BroadcastMessage()
                                {
                                    ServiceNo = mElement.Attribute("SRVNO").Value,
                                    EnglishMessageString = mElement.Attribute("ENGMESSAGE").Value,
                                    HindiMessageString = mElement.Attribute("HINMESSAGE").Value,
                                    MessageType = mElement.Attribute("MSGTYP").Value
                                });

                            }
                            break;
                        case "EBill":
                            foreach (var mElement in XMessage.Elements())
                            {
                                EbillOutputLocation = mElement.Attribute("DestinationPath").Value;

                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                AppFunctions.LogError(ex);
            }   

        }

        
    }
}
