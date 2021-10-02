namespace ATPrintUtility
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.rcRibbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.bbiPrintLT = new DevExpress.XtraBars.BarButtonItem();
            this.bbiPrintHT = new DevExpress.XtraBars.BarButtonItem();
            this.HomePage = new DevExpress.XtraBars.BarButtonItem();
            this.bbiHindi = new DevExpress.XtraBars.BarButtonItem();
            this.bbiEnglish = new DevExpress.XtraBars.BarButtonItem();
            this.bbiBroadcastMessage = new DevExpress.XtraBars.BarButtonItem();
            this.bbiEBillLocation = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.bbiPrintLTMD = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.rpPrint = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpConfigPanel = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rpgConfiguration = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpgBroadCastMessage = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpgOutputLocation = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ATDocumentManager = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.rcRibbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ATDocumentManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
            this.SuspendLayout();
            // 
            // rcRibbonControl
            // 
            this.rcRibbonControl.ExpandCollapseItem.Id = 0;
            this.rcRibbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcRibbonControl.ExpandCollapseItem,
            this.rcRibbonControl.SearchEditItem,
            this.bbiPrintLT,
            this.bbiPrintHT,
            this.HomePage,
            this.bbiHindi,
            this.bbiEnglish,
            this.bbiBroadcastMessage,
            this.bbiEBillLocation,
            this.barStaticItem1,
            this.barButtonItem1,
            this.barSubItem1,
            this.bbiPrintLTMD,
            this.barButtonItem2,
            this.barButtonItem3});
            this.rcRibbonControl.Location = new System.Drawing.Point(0, 0);
            this.rcRibbonControl.MaxItemId = 17;
            this.rcRibbonControl.Name = "rcRibbonControl";
            this.rcRibbonControl.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpPrint,
            this.rpConfigPanel});
            this.rcRibbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2019;
            this.rcRibbonControl.Size = new System.Drawing.Size(1235, 148);
            // 
            // bbiPrintLT
            // 
            this.bbiPrintLT.Caption = "Print LT";
            this.bbiPrintLT.Id = 1;
            this.bbiPrintLT.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("bbiPrintLT.ImageOptions.SvgImage")));
            this.bbiPrintLT.ItemAppearance.Normal.Options.UseTextOptions = true;
            this.bbiPrintLT.ItemAppearance.Normal.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.bbiPrintLT.Name = "bbiPrintLT";
            this.bbiPrintLT.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.bbiPrintLT.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // bbiPrintHT
            // 
            this.bbiPrintHT.Caption = "Print HT";
            this.bbiPrintHT.Id = 2;
            this.bbiPrintHT.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("bbiPrintHT.ImageOptions.SvgImage")));
            this.bbiPrintHT.ItemAppearance.Normal.Options.UseTextOptions = true;
            this.bbiPrintHT.ItemAppearance.Normal.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.bbiPrintHT.Name = "bbiPrintHT";
            this.bbiPrintHT.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.bbiPrintHT.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiPrintHT_ItemClick);
            // 
            // HomePage
            // 
            this.HomePage.Caption = "barButtonItem1";
            this.HomePage.Id = 4;
            this.HomePage.Name = "HomePage";
            // 
            // bbiHindi
            // 
            this.bbiHindi.Caption = "Hindi";
            this.bbiHindi.Id = 5;
            this.bbiHindi.ImageOptions.Image = global::AT.Print.Properties.Resources.Google_Hindi_Input_icon;
            this.bbiHindi.Name = "bbiHindi";
            this.bbiHindi.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.bbiHindi.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiHindi_ItemClick);
            // 
            // bbiEnglish
            // 
            this.bbiEnglish.Caption = "English";
            this.bbiEnglish.Id = 6;
            this.bbiEnglish.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("bbiEnglish.ImageOptions.SvgImage")));
            this.bbiEnglish.Name = "bbiEnglish";
            this.bbiEnglish.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiEnglish_ItemClick);
            // 
            // bbiBroadcastMessage
            // 
            this.bbiBroadcastMessage.Caption = "Broadcast Message";
            this.bbiBroadcastMessage.Id = 7;
            this.bbiBroadcastMessage.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("bbiBroadcastMessage.ImageOptions.SvgImage")));
            this.bbiBroadcastMessage.Name = "bbiBroadcastMessage";
            this.bbiBroadcastMessage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiBroadcastMessage_ItemClick);
            // 
            // bbiEBillLocation
            // 
            this.bbiEBillLocation.Caption = "Output Location";
            this.bbiEBillLocation.Id = 8;
            this.bbiEBillLocation.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("bbiEBillLocation.ImageOptions.SvgImage")));
            this.bbiEBillLocation.Name = "bbiEBillLocation";
            this.bbiEBillLocation.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiEBillLocation_ItemClick);
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "barStaticItem1";
            this.barStaticItem1.Enabled = false;
            this.barStaticItem1.Id = 9;
            this.barStaticItem1.Name = "barStaticItem1";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 10;
            this.barButtonItem1.ImageOptions.Image = global::AT.Print.Properties.Resources.logo;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "barSubItem1";
            this.barSubItem1.Id = 11;
            this.barSubItem1.Name = "barSubItem1";
            // 
            // bbiPrintLTMD
            // 
            this.bbiPrintLTMD.Caption = "Print LTMD";
            this.bbiPrintLTMD.Id = 12;
            this.bbiPrintLTMD.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("bbiPrintLTMD.ImageOptions.SvgImage")));
            this.bbiPrintLTMD.Name = "bbiPrintLTMD";
            this.bbiPrintLTMD.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiPrintLTMD_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "Print LTMD Solar";
            this.barButtonItem2.Id = 15;
            this.barButtonItem2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem2.ImageOptions.Image")));
            this.barButtonItem2.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem2.ImageOptions.LargeImage")));
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiPrintLTMD_Solar_ItemClick);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "Print HT Solar";
            this.barButtonItem3.Id = 16;
            this.barButtonItem3.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem3.ImageOptions.Image")));
            this.barButtonItem3.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem3.ImageOptions.LargeImage")));
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiPrint_HT_Solar_ItemClick);
            // 
            // rpPrint
            // 
            this.rpPrint.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2});
            this.rpPrint.Name = "rpPrint";
            this.rpPrint.Text = "Print ";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.bbiPrintLT);
            this.ribbonPageGroup1.ItemLinks.Add(this.bbiPrintLTMD);
            this.ribbonPageGroup1.ItemLinks.Add(this.bbiPrintHT);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Print Bill";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem2);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem3);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Print Solar Bills";
            // 
            // rpConfigPanel
            // 
            this.rpConfigPanel.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rpgConfiguration,
            this.rpgBroadCastMessage,
            this.rpgOutputLocation});
            this.rpConfigPanel.Name = "rpConfigPanel";
            this.rpConfigPanel.Text = "Configuration";
            // 
            // rpgConfiguration
            // 
            this.rpgConfiguration.AllowTextClipping = false;
            this.rpgConfiguration.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("rpgConfiguration.ImageOptions.SvgImage")));
            this.rpgConfiguration.ItemLinks.Add(this.bbiHindi);
            this.rpgConfiguration.ItemLinks.Add(this.bbiEnglish);
            this.rpgConfiguration.Name = "rpgConfiguration";
            this.rpgConfiguration.Text = "Manage Messages";
            // 
            // rpgBroadCastMessage
            // 
            this.rpgBroadCastMessage.ItemLinks.Add(this.bbiBroadcastMessage);
            this.rpgBroadCastMessage.Name = "rpgBroadCastMessage";
            this.rpgBroadCastMessage.Text = "Broadcast Message";
            // 
            // rpgOutputLocation
            // 
            this.rpgOutputLocation.ItemLinks.Add(this.bbiEBillLocation);
            this.rpgOutputLocation.Name = "rpgOutputLocation";
            this.rpgOutputLocation.Text = "E-Bill Output Location";
            // 
            // ATDocumentManager
            // 
            this.ATDocumentManager.ContainerControl = this;
            this.ATDocumentManager.MenuManager = this.rcRibbonControl;
            this.ATDocumentManager.View = this.tabbedView1;
            this.ATDocumentManager.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView1});
            // 
            // dockManager
            // 
            this.dockManager.Form = this;
            this.dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl",
            "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl"});
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1235, 714);
            this.Controls.Add(this.rcRibbonControl);
            this.IconOptions.Image = global::AT.Print.Properties.Resources.smalllogo;
            this.Name = "MainForm";
            this.Ribbon = this.rcRibbonControl;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Torrent Power (Version 2.1.5)";
            ((System.ComponentModel.ISupportInitialize)(this.rcRibbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ATDocumentManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl rcRibbonControl;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpPrint;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Docking2010.DocumentManager ATDocumentManager;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private DevExpress.XtraBars.BarButtonItem bbiPrintLT;
        private DevExpress.XtraBars.Docking.DockManager dockManager;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager;
        private DevExpress.XtraBars.BarButtonItem bbiPrintHT;
        private DevExpress.XtraBars.BarButtonItem HomePage;
        private DevExpress.XtraBars.BarButtonItem bbiHindi;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpConfigPanel;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgConfiguration;
        private DevExpress.XtraBars.BarButtonItem bbiEnglish;
        private DevExpress.XtraBars.BarButtonItem bbiBroadcastMessage;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgBroadCastMessage;
        private DevExpress.XtraBars.BarButtonItem bbiEBillLocation;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgOutputLocation;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem bbiPrintLTMD;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
    }
}

