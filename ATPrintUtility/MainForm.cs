using AT.Print;
using AT.Print.Messages;
using AT.Print.Utils;
using DevExpress.XtraBars.Docking;
using System.Windows.Forms;

namespace ATPrintUtility
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {

        public MainForm()
        {
            InitializeComponent();
            LoadStaticData.LoadData();
        }



        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AppFunctions.ShowWaitForm("Opening Print LT Page");
            var createEditControl = new PrintLT { Name = "PrintLT" };
            AppFunctions.OpenNewPanel(createEditControl, DockingStyle.Float, "Print LT", "Print LT", true, true);
            AppFunctions.CloseWaitForm();
        }

        private void bbiPrintLTMD_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AppFunctions.ShowWaitForm("Opening Print LTMD Page");
            var createEditControl = new PrintLTMD { Name = "PrintLTMD" };
            AppFunctions.OpenNewPanel(createEditControl, DockingStyle.Float, "Print LTMD", "Print LTMD", true, true);
            AppFunctions.CloseWaitForm();
        }
        private void bbiPrintHT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AppFunctions.ShowWaitForm("Opening Print HT Page");
            var createEditControl = new Print_HT1 { Name = "Print_HT1" };
            AppFunctions.OpenNewPanel(createEditControl, DockingStyle.Float, "Print HT", "Print HT", true, true);
            AppFunctions.CloseWaitForm();
        }

        private void bbiPrintLTMD_Solar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AppFunctions.ShowWaitForm("Opening Print LTMD Solar Page");
            var createEditControl = new Print_Solar_LTMD { Name = "Print_Solar_LTMD" };
            AppFunctions.OpenNewPanel(createEditControl, DockingStyle.Float, "Print LTMD Solar", "Print LTMD Solar", true, true);
            AppFunctions.CloseWaitForm();

        }

        private void bbiPrint_HT_Solar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AppFunctions.ShowWaitForm("Opening Print HT Solar Page");
            var createEditControl = new Print_Solar_HT { Name = "Print_Solar_HT" };
            AppFunctions.OpenNewPanel(createEditControl, DockingStyle.Float, "Print HT Solar", "Print HT Solar", true, true);
            AppFunctions.CloseWaitForm();

        }


        public DockManager GetDockManager()
        {
            return dockManager;
        }

        public void ShowStatus(string status)
        {
            Application.DoEvents();
        }

        private void bbiEnglish_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AppFunctions.ShowWaitForm("Opening English Messages Page");
            var createEditControl = new ManageEnglishMessages { Name = "English" };
            AppFunctions.OpenNewPanel(createEditControl, DockingStyle.Float, "Manage English Messages", "Manage English Messages", true, true);
            AppFunctions.CloseWaitForm();

        }

        private void bbiHindi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AppFunctions.ShowWaitForm("Opening Hindi Messages Page");
            var createEditControl = new ManageHindiMessages { Name = "Hindi" };
            AppFunctions.OpenNewPanel(createEditControl, DockingStyle.Float, "Manage Hindi Messages", "Manage Hindi Messages", true, true);
            AppFunctions.CloseWaitForm();
        }

        private void bbiBroadcastMessage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AppFunctions.ShowWaitForm("Opening Broadcast Messages Page");
            var createEditControl = new BroadcastMessages { Name = "Broadcast Messages" };
            AppFunctions.OpenNewPanel(createEditControl, DockingStyle.Float, "Manage Broadcast Messages", "Manage Broadcast Messages", true, true);
            AppFunctions.CloseWaitForm();
        }

        private void bbiEBillLocation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AppFunctions.ShowWaitForm("E-Bill Output Location..!!");
            var createEditControl = new EbillOutputLocation { Name = "E-Bill Output Location..!!" };
            AppFunctions.OpenNewPanel(createEditControl, DockingStyle.Float, "E-Bill Output Location..!!", "E-Bill Output Location..!!", true, true);
            AppFunctions.CloseWaitForm();
        }

        
    }
}
