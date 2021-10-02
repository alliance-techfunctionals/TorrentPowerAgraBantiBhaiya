using System;
using System.Data;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AT.Print
{
    public partial class ManageHindiMessages : UserControl
    {
        DataTable _HindiMessage = new DataTable();
        public ManageHindiMessages()
        {
            InitializeComponent();

            _HindiMessage.Columns.Add("Code");
            _HindiMessage.Columns.Add("Description");
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
                        case "Hindi":
                            foreach (var message in XMessage.Elements())
                            {
                                _HindiMessage.Rows.Add(new object[] { message.Attribute("Code").Value, message.Attribute("Description").Value });
                            }
                            break;

                    }
                }
                gridView1.PopulateColumns(_HindiMessage);
                gridControl1.DataSource = _HindiMessage;


            }
            catch (Exception ex)
            {
                AT.Print.Utils.AppFunctions.LogError(ex.Message);
            }


        }

        private void gridControl1_DataSourceChanged(object sender, EventArgs e)
        {
            gridView1.Columns["Description"].AppearanceCell.Font = new System.Drawing.Font("Sans Serif", 15);
            gridView1.Columns["Code"].Width = 5;
            gridView1.Columns["Description"].AppearanceCell.Font = new System.Drawing.Font("Kruti Dev 010", 15);



        }
    }
}
