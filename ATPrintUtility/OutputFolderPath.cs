using AT.Print.Utils;
using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AT.Print
{
    public class OutputFolderPath
    {
        public string LoadLocation()
        {
            string OutPath = "";

            XDocument Messagesa = XDocument.Load(Application.StartupPath + "//DBEntity//xMessage.xml");
            var XMessages = Messagesa.Root;
            foreach (var mes in XMessages.Elements())
            {
                switch (mes.Name.ToString())
                {
                    case "EBill":
                        foreach (var mElement in mes.Elements())
                        {
                            OutPath= mElement.Attribute("DestinationPath").Value;

                        }
                        break;
                }
            }
            return OutPath;
        }
    }
}
