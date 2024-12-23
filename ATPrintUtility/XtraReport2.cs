namespace AT.Print
{
    public partial class XtraReport2 : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport2()
        {
            InitializeComponent();
        }

        private void XtraReport2_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            xrLabel160.TopF = xrLabel157.BottomF;
        }
    }
}
