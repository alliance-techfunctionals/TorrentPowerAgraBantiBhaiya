using AT.Print.Utils;
using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AT.Print
{
    public partial class EbillOutputLocation : UserControl
    {
        public EbillOutputLocation()
        {
            InitializeComponent();

            LoadLocation();

        }

        private void LoadLocation()
        {



            XDocument Messagesa = XDocument.Load(Application.StartupPath + "//DBEntity//xMessage.xml");
            var XMessages = Messagesa.Root;
            foreach (var mes in XMessages.Elements())
            {

                switch (mes.Name.ToString())
                {
                    case "EBill":
                        foreach (var mElement in mes.Elements())
                        {
                            beOutputLocation.Text = mElement.Attribute("DestinationPath").Value;

                        }


                        break;
                }
            }
        }

        private void cbSeparatorTraySource_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void beOutputLocation_Click(object sender, EventArgs e)
        {

        }

        private void beOutputLocation_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            using (FolderBrowserDialog ofdMv = new FolderBrowserDialog())
            {
                if (ofdMv.ShowDialog() == DialogResult.OK)
                {
                    XDocument Messagesa = XDocument.Load(Application.StartupPath + "//DBEntity//xMessage.xml");
                    var XMessages = Messagesa.Root;
                    foreach (var mes in XMessages.Elements())
                    {

                        switch (mes.Name.ToString())
                        {
                            case "EBill":
                                foreach (var mElement in mes.Elements())
                                {
                                    mElement.Remove();

                                }
                                XElement newBroadCastMsg = new XElement("OutputLocation", new XAttribute("DestinationPath", ofdMv.SelectedPath));
                                mes.Add(newBroadCastMsg);
                                Messagesa.Save(Application.StartupPath + "//DBEntity//xMessage.xml");
                                LoadLocation();
                                LoadStaticData.LoadData();
                                break;
                        }
                    }
                }
            }
        }
    }
}
