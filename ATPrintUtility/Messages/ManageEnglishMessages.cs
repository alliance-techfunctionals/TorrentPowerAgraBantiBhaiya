using System;
using System.Data;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AT.Print
{
    public partial class ManageEnglishMessages : UserControl
    {
        DataTable _EnglishMessage = new DataTable();
        public ManageEnglishMessages()
        {
            InitializeComponent();

            _EnglishMessage.Columns.Add("Code");
            _EnglishMessage.Columns.Add("Description");
            BindingManagerData();

        }


        void BindingManagerData()
        {



            try
            {
                XDocument Messages = XDocument.Load(Application.StartupPath + "//DBEntity//xMessage.xml");
                var XMessages = Messages.Root.Elements();
                foreach (var XMessage in XMessages)
                {
                    switch (XMessage.Name.ToString())
                    {
                        case "English":
                            foreach (var message in XMessage.Elements())
                            {
                                _EnglishMessage.Rows.Add(new object[] { message.Attribute("Code").Value, message.Attribute("Description").Value });
                            }
                            break;

                    }
                }
                gridControl1.DataSource = _EnglishMessage;
            }
            catch (Exception ex)
            {
                AT.Print.Utils.AppFunctions.LogError(ex.Message);
            }


        }
    }
}
