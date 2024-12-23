using DevExpress.Drawing;
using System;
using System.Data;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AT.Print.Messages
{
    public partial class BroadcastMessages : UserControl
    {
        DataTable _BroadcastMessage = new DataTable();
        public BroadcastMessages()
        {
            InitializeComponent();
            _BroadcastMessage.Columns.Add("ServiceNo");
            _BroadcastMessage.Columns.Add("MessageType");
            _BroadcastMessage.Columns.Add("EnglishMessage");
            _BroadcastMessage.Columns.Add("HindiMessage");

            LoadData();

        }
        public void LoadData()
        {
            XDocument Messages = XDocument.Load(Application.StartupPath + "//DBEntity//xMessage.xml");
            var XMessages = Messages.Root.Elements();
            foreach (var XMessage in XMessages)
            {
                switch (XMessage.Name.ToString())
                {
                    case "BROADCAST":
                        foreach (var mElement in XMessage.Elements())
                        {
                            _BroadcastMessage.Rows.Add(new object[]
                            {
                                 mElement.Attribute("SRVNO").Value,
                                mElement.Attribute("MSGTYP").Value,
                                mElement.Attribute("ENGMESSAGE").Value,
                                 mElement.Attribute("HINMESSAGE").Value,

                            });

                        }
                        break;
                }
            }
            gridView1.PopulateColumns(_BroadcastMessage);
            gridControl1.DataSource = _BroadcastMessage;
        }

        private void gridControl1_DataSourceChanged(object sender, EventArgs e)
        {
            gridView1.BestFitColumns();
            gridView1.Appearance.Row.Font = new DXFont("DIN Pro Regular", 15);
            gridView1.Columns["HindiMessage"].AppearanceCell.Font = new DXFont("Kruti Dev 010", 15);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var Engmessage = cmbMessageType.Text == "ENG" ? txtMessage.Text : "";
            var Hinmessage = cmbMessageType.Text == "HINDI" ? txtMessage.Text : "";
            XDocument Messagesa = XDocument.Load(Application.StartupPath + "//DBEntity//xMessage.xml");
            var XMessages = Messagesa.Root;
            foreach (var mes in XMessages.Elements())
            {

                switch (mes.Name.ToString())
                {
                    case "BROADCAST":
                        XElement newBroadCastMsg = new XElement("BCAST", new XAttribute("SRVNO", txtServiceNo.Text.Trim()), new XAttribute("MSGTYP", cmbMessageType.Text), new XAttribute("ENGMESSAGE", Engmessage), new XAttribute("HINMESSAGE", Hinmessage));
                        mes.Add(newBroadCastMsg);
                        Messagesa.Save(Application.StartupPath + "//DBEntity//xMessage.xml");
                        break;
                }
            }
        }
    }
}
