using AT.Print.Utils;
using DevExpress.XtraCharts;
using System.Collections.Generic;
using System.Windows.Forms;



namespace AT.Print.PDF
{
    public partial class rpt_HT_BackPDF : DevExpress.XtraReports.UI.XtraReport
    {
        public rpt_HT_BackPDF()
        {
            InitializeComponent();
        }

        private void rpt_HT_Tod_Back_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //xrPictureBox3.ImageSource = Properties.Resources.New;
            var Data = this.DataSource as List<SingleHTBill>;
            xrChart1.Series[0].DataSource = Data[0].KVAHgrph;
            xrChart1.Series[0].ArgumentScaleType = ScaleType.Qualitative;
            xrChart1.Series[0].ArgumentDataMember = "MonthYear";
            xrChart1.Series[0].Label.TextOrientation = TextOrientation.BottomToTop;
            xrChart1.Series[0].ValueScaleType = ScaleType.Numerical;
            xrChart1.Series[0].ValueDataMembers.AddRange(new string[] { "Value" });
            xrPictureBox2.ImageUrl = Application.StartupPath + "\\Contents\\CategorySlabImages\\" + Data[0].L6_TARIFF_DESCR + ".png";
            xrPictureBox1.ImageUrl = Data[0].MVPicture;


            xrChart4.Series[0].DataSource = Data[0].KVAgrph;
            xrChart4.Series[0].ArgumentScaleType = ScaleType.Qualitative;
            xrChart4.Series[0].ArgumentDataMember = "MonthYear";
            xrChart4.Series[0].Label.TextOrientation = TextOrientation.BottomToTop;
            xrChart4.Series[0].ValueScaleType = ScaleType.Numerical;
            xrChart4.Series[0].ValueDataMembers.AddRange(new string[] { "Value" });
            xrChart4.WidthF = xrChart1.WidthF;

            xrChart3.Series[0].DataSource = Data[0].PFgrph;
            xrChart3.Series[0].ArgumentScaleType = ScaleType.Qualitative;
            xrChart3.Series[0].ArgumentDataMember = "MonthYear";
            xrChart3.Series[0].Label.TextOrientation = TextOrientation.BottomToTop;
            xrChart3.Series[0].ValueScaleType = ScaleType.Numerical;
            xrChart3.Series[0].ValueDataMembers.AddRange(new string[] { "Value" });
            xrChart3.WidthF = xrChart4.WidthF;

            //Disconnection Message
            if (Data[0].L1_DisconnectionMSGPrintingIMMEDIATE == "1")
            {
                xrDueDate.Text = "IMMEDIATE";
                
            }
            else
            {
                xrDueDate.Text = Data[0].L7_Due_Date;
            }

        }

       
    }
}
