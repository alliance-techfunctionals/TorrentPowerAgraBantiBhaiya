﻿using AT.Print.Utils;
using DevExpress.XtraCharts;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AT.Print
{
    public partial class Rpt_LT_Print_Back : DevExpress.XtraReports.UI.XtraReport
    {
        public Rpt_LT_Print_Back()
        {
            InitializeComponent();
        }

        private void Rpt_LT_Print_Back_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var Data = this.DataSource as List<SingleLTBill>;
            xrChart1.Series[0].DataSource = Data[0].KWHgrph;
            xrChart1.Series[0].ArgumentScaleType = ScaleType.Qualitative;
            xrChart1.Series[0].ArgumentDataMember = "MonthYear";
            xrChart1.Series[0].Label.TextOrientation = TextOrientation.BottomToTop;
            xrChart1.Series[0].ValueScaleType = ScaleType.Numerical;
            
            xrChart1.Series[0].ValueDataMembers.AddRange(new string[] { "Value" });
            xrPictureBox2.ImageUrl = Application.StartupPath + "\\Contents\\CategorySlabImages\\" + Data[0].L6_TARIFF_DESCR + ".png";
            xrPictureBox1.ImageUrl = Data[0].MVPicture;


            xrChart2.Series[0].DataSource = Data[0].KVAgrph;
            xrChart2.Series[0].ArgumentScaleType = ScaleType.Qualitative;
            xrChart2.Series[0].ArgumentDataMember = "MonthYear";
            xrChart2.Series[0].Label.TextOrientation = TextOrientation.BottomToTop;
            xrChart2.Series[0].ValueScaleType = ScaleType.Numerical;
            xrChart2.Series[0].ValueDataMembers.AddRange(new string[] { "Value" });
            xrChart2.WidthF = xrChart1.WidthF;
        }
    }
}
