using AT.Print.Utils;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;
using ZXing;
using DevExpress.Drawing;
using DevExpress.XtraCharts;
using System.Drawing;
namespace AT.Print.PDF
{
    public partial class Rpt_LTMD_Solar_PDF : DevExpress.XtraReports.UI.XtraReport
    {
        public Rpt_LTMD_Solar_back_PDF Rpt_LTMD_Solar_back_visible;
        public Rpt_LTMD_Solar_PDF(Rpt_LTMD_Solar_back_PDF d = null)
        {
            InitializeComponent();
            Rpt_LTMD_Solar_back_visible =d;
        }

      #region Helper Functions

        //bool IsMessageLimitExceeds(int messagesCount)
        //{
        //    if (messagesCount >= 8)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //public void adjustMessages(XRLabel lbl)
        //{
        //    if (xrPanel1.Controls.Count != 0)
        //    {
        //        foreach (XRLabel plbl in xrPanel1.Controls)
        //        {
        //            lbl.TopF = plbl.BottomF;
        //        }
        //    }
        //    else
        //    {
        //        lbl.TopF = xrPanel1.TopF;
        //    }
        //}
        #endregion

        private void Rpt_solar_PDF_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var data = sender as Rpt_LTMD_Solar_PDF;
            var op = data.DataSource as List<SolarBill>;

          

            if (!String.IsNullOrEmpty(op[0].L6_LT_Metering_Flag))
            {
                xrlL6Servdet_Sanc_load.Text = "*" + xrlL6Servdet_Sanc_load.Text;

            }

            #region QRCODE

            if (ConfigurationManager.AppSettings["generateQRCodeinSolarLTMDBills"].ToString() == "True")
            {
                string qrServiceno = "AGR@" + (op[0].L6_SERVDET_SERVNO);
                byte[] bytesToEncode = Encoding.UTF8.GetBytes(qrServiceno);
                string base64Encoded = Convert.ToBase64String(bytesToEncode);
                string textToEncode = (ConfigurationManager.AppSettings["generateQRCodeURL"].ToString()) + base64Encoded;
                BarcodeWriter barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                var encodingOptions = new ZXing.Common.EncodingOptions
                {
                    Margin = 0,
                };
                barcodeWriter.Options = encodingOptions;
                var qrCodeBitmap = barcodeWriter.Write(textToEncode);
                xrQRCODE.Image = qrCodeBitmap;
                xrQRCODE.SizeF = new System.Drawing.SizeF(51, 51);
            }

            #endregion
            xrLblAmount.Text = "₹" + ToDecimal(op[0].L8_amount_payable_before_due_date).ToString("G");
            xrLabelTotalAmt.BringToFront();

            xrLabelTotalAmt.Text = "₹" + ToDecimal(op[0].L8_amount_payable_before_due_date).ToString("G");
            xrLabel14.Text = "Thank you for your previous payment of ₹" + op[0].L7_Last_Payement_amount + " on " + op[0].L7_LastpymtDate;
            xrLabel13.BringToFront();
            xrLabel5.BringToFront();
            xrLabel19.BringToFront();


            if (!string.IsNullOrEmpty(op[0].L1_Customer_PAN))
            {
                xrLabel3.Visible = true;
            }
            else
            {
                xrLabel3.Visible = false;
            }

            // To keep Address and PAN together             
            if (op[0].L2_NAME.ToString() == "")
            {
                xrLabel139.Visible = false;
                xrLabel140.TopF = xrLabel139.TopF;
            }
            if (op[0].L3_ADDR1.ToString() == "")
            {
                xrLabel140.Visible = false;
                xrLabel141.TopF = xrLabel140.TopF;
            }
            if (op[0].L4_ADDR2.ToString() == "")
            {
                xrLabel141.Visible = false;
                xrLabel142.TopF = xrLabel141.TopF;
            }
            if (op[0].L5_ADDR3.ToString() == "")
            {
                xrLabel142.Visible = false;
                xrLabel3.TopF = xrLabel142.TopF;
                xrLabel40.TopF = xrLabel3.BottomF;
                xrLabel8.TopF = xrLabel40.TopF;
            }


            string unit = "KW";
            if (!string.IsNullOrEmpty(op[0].L6_Kvah_indicator) && op[0].L6_Kvah_indicator == "1")
            {
                op[0].unit = op[0].L6_MEASURE_OF_CONTRACT_Demand;
                unit = op[0].L6_MEASURE_OF_CONTRACT_Demand;
            }
            else
            {
                op[0].unit = "KW";

                if (Decimal.TryParse(op[0].L6_SERVDET_SANC_LOAD, out decimal contractdemand))
                {

                    if (op[0].L6_MEASURE_OF_CONTRACT_Demand.ToUpper() == "HP")
                    {
                        if ((contractdemand * (decimal)0.746) >= (decimal)10)
                        {
                            if (Convert.ToDecimal(op[0].L6_Avg_Power_Factor) >= (decimal)95)
                            {
                                op[0].L6_Avg_Power_Factor = op[0].L6_Avg_Power_Factor + "(Lead)";
                            }
                        }
                    }
                    if (op[0].L6_MEASURE_OF_CONTRACT_Demand.ToUpper() == "KW")
                    {
                        if (Math.Ceiling(contractdemand) >= (decimal)10.0)
                        {
                            if (Convert.ToDecimal(op[0].L6_Avg_Power_Factor) >= (decimal)95)
                            {
                                op[0].L6_Avg_Power_Factor = op[0].L6_Avg_Power_Factor + "(Lead)";
                            }
                        }
                    }
                }
            }

            
            if (op[0].L6_MEASURE_OF_CONTRACT_Demand == "HP")
            {

                if (op[0].L6_Kvah_indicator == "1")
                {
                    
                    op[0].unit1 = "KVA";

                    //xrlRecordedDemand.Text = "Recorded Demand(" + op[0].unit1 + ")";
                    //xrlL6Servdet_Sanc_load.Text = "Bill Demand(" + op[0].unit1 + ")";
                    //xrlL6ExcessDemand.Text = "Excess Demand(" + op[0].unit1 + ")";
                    KW_HEAD1.Text = op[0].unit1;
                    KWH_HEAD1.Text = op[0].unit1 + "H";
                    KW_HEAD2.Text = op[0].unit1;
                    KWH_HEAD2.Text = op[0].unit1 + "H";
                }
                else
                {
                    op[0].unit1 = "KW";

                    //xrlRecordedDemand.Text = "Recorded Demand(" + op[0].unit1 + ")";
                    //xrlL6ExcessDemand.Text = "Excess Demand(" + op[0].unit1 + ")";
                    //xrlL6Servdet_Sanc_load.Text = "Bill Demand(" + op[0].unit1 + ")";
                    KW_HEAD1.Text = op[0].unit1;
                    KWH_HEAD1.Text = op[0].unit1 + "H";
                    KW_HEAD2.Text = op[0].unit1;
                    KWH_HEAD2.Text = op[0].unit1 + "H";
                }
            }
            else if (op[0].L6_MEASURE_OF_CONTRACT_Demand == "KW")
            {
                if (!string.IsNullOrEmpty(op[0].L6_Kvah_indicator) && op[0].L6_Kvah_indicator == "1")
                {
                    op[0].unit1 = "KVA";

                    //xrlRecordedDemand.Text = "Recorded Demand(" + op[0].unit1 + ")";
                    //xrlL6ExcessDemand.Text = "Excess Demand(" + op[0].unit1 + ")";
                    //xrlL6Servdet_Sanc_load.Text = "Bill Demand(" + op[0].unit1 + ")";
                    KW_HEAD1.Text = op[0].unit1;
                    KWH_HEAD1.Text = op[0].unit1 + "H";
                    KW_HEAD2.Text = op[0].unit1;
                    KWH_HEAD2.Text = op[0].unit1 + "H";
                }
                else
                {
                    op[0].unit1 = "KW";

                    //xrlRecordedDemand.Text = "Recorded Demand(" + op[0].unit1 + ")";
                    //xrlL6ExcessDemand.Text = "Excess Demand(" + op[0].unit1 + ")";
                    //xrlL6Servdet_Sanc_load.Text = "Bill Demand(" + op[0].unit1 + ")";
                    KW_HEAD1.Text  = op[0].unit1;
                    KWH_HEAD1.Text = op[0].unit1 + "H";
                    KW_HEAD2.Text  = op[0].unit1;
                    KWH_HEAD2.Text = op[0].unit1 + "H";
                }
            }
            else if (op[0].L6_MEASURE_OF_CONTRACT_Demand == "KVA")
            {
                if (op[0].L6_Kvah_indicator == "1")
                {
                    op[0].unit1 = "KVA";

                    //xrlRecordedDemand.Text = "Recorded Demand(" + op[0].unit1 + ")";
                    //xrlL6ExcessDemand.Text = "Excess Demand(" + op[0].unit1 + ")";
                    //xrlL6Servdet_Sanc_load.Text = "Bill Demand(" + op[0].unit1 + ")";
                    KW_HEAD1.Text = op[0].unit1;
                    KWH_HEAD1.Text = op[0].unit1 + "H";
                    KW_HEAD2.Text = op[0].unit1;
                    KWH_HEAD2.Text = op[0].unit1 + "H";
                }
                else
                {
                    op[0].unit1 = "KW";

                    //xrlRecordedDemand.Text = "Recorded Demand(" + op[0].unit1 + ")";
                    //xrlL6ExcessDemand.Text = "Excess Demand(" + op[0].unit1 + ")";
                    //xrlL6Servdet_Sanc_load.Text = "Bill Demand(" + op[0].unit1 + ")";
                    KW_HEAD1.Text = op[0].unit1;
                    KWH_HEAD1.Text = op[0].unit1 + "H";
                    KW_HEAD2.Text = op[0].unit1;
                    KWH_HEAD2.Text = op[0].unit1 + "H";

                }
            }

            #region Excess Demand Print
            //Excess Demand Print
            if (op[0].L6_EXCESS_DEMAND == "0.00" || op[0].L6_EXCESS_DEMAND == " " || op[0].L6_EXCESS_DEMAND == "0")
            {
                VlL6ExcessDemand.Visible = false;
            }
            #endregion


            #region pieChart
            //PieChart//
            decimal energyCharge = ToDecimal(op[0].L8_EnergyCharge);
            decimal fixedCharge = ToDecimal(op[0].L8_FixedCharge);
            decimal electricityDuty = ToDecimal(op[0].L8_GovTax);
            decimal excessDemandCharge = ToDecimal(op[0].L10_DMDCHG_PENALTY);

            xrChartPie.Series.Clear();
            xrChartPie.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            Series pieSeries = new Series("Major Bill Components", ViewType.Doughnut);

            pieSeries.Points.Add(new SeriesPoint("Electricity Duty", electricityDuty));
            pieSeries.Points.Add(new SeriesPoint("Excess Demand Charge", excessDemandCharge));
            pieSeries.Points.Add(new SeriesPoint("Energy Charges", energyCharge));
            pieSeries.Points.Add(new SeriesPoint("Fixed Charges", fixedCharge));


            DoughnutSeriesLabel label = (DoughnutSeriesLabel)pieSeries.Label;
            label.Position = PieSeriesLabelPosition.TwoColumns;

            label.TextPattern = "{A}\n₹{V:G}";
            label.TextColor = Color.Black;
            label.Font = new Font("Manrope", 5);
            label.BackColor = Color.Transparent;
            label.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;

            DoughnutSeriesView view = (DoughnutSeriesView)pieSeries.View;
            view.HoleRadiusPercent = 75;
            view.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;



            // ----- MANUAL COLORS (NO RANDOMNESS) -----
            pieSeries.Points[0].Color = Color.FromArgb(208, 208, 207);  // Duty
            pieSeries.Points[1].Color = Color.FromArgb(179, 180, 180); // Excess
            pieSeries.Points[2].Color = Color.FromArgb(151, 151, 151);   // Energy
            pieSeries.Points[3].Color = Color.FromArgb(125, 125, 124);  // Fixed
            xrChartPie.Series.Add(pieSeries);

            #endregion



            if (!string.IsNullOrEmpty(op[0].L6_Kvah_indicator) && op[0].L6_Kvah_indicator == "1")
            {

            }
            else
            {
                if (Decimal.TryParse(op[0].L6_SERVDET_SANC_LOAD, out decimal contractdemand))
                {
                    if (op[0].L6_MEASURE_OF_CONTRACT_Demand.ToUpper() == "KW" && Math.Ceiling(contractdemand) >= (decimal)10.0)
                    {
                        op[0].L6_MEASURE_OF_CONTRACT_Demand = op[0].L6_MEASURE_OF_CONTRACT_Demand + "/KVA";
                    }
                    else if (op[0].L6_MEASURE_OF_CONTRACT_Demand.ToUpper() == "HP" && Math.Ceiling(contractdemand) >= (decimal)13.4)
                    {
                        op[0].L6_MEASURE_OF_CONTRACT_Demand = op[0].L6_MEASURE_OF_CONTRACT_Demand + "/KVA";
                    }
                }
            }

           
           


            if (op[0].L12_MTRSNO_METER_2_IF_AVAILABLE != "")
            {
                MTR_TOD1.Text = op[0].L12_MTRSNO_METER1;
                MTR_TOD2.Text = op[0].L12_MTRSNO_METER_2_IF_AVAILABLE;
                #region Meter(KW)
                if (string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "0.00")
                {

                    //imp
                    xrLabel16.Text = op[0].L23_TOD_1_KW;
                    xrLabel23.Text = op[0].L23_TOD_2_KW;
                    xrLabelcload.Text = op[0].L23_TOD_3_KW;
                    xrLabel30.Text = op[0].L23_TOD_4_KW;

                    xrLabel17.Text = op[0].L22_TOD_1_KWH;
                    xrLabel24.Text = op[0].L22_TOD_2_KWH;
                    xrLabel45.Text = op[0].L22_TOD_3_KWH;
                    xrLabel31.Text = op[0].L22_TOD_4_KWH;
                    //EXP

                    xrLabel37.Text = op[0].L53_Exp_TOD1_KW_Units;
                    xrLabel51.Text = op[0].L53_Exp_TOD2_KW_Units;
                    xrLabel58.Text = op[0].L53_Exp_TOD3_KW_Units;
                    xrLabel65.Text = op[0].L53_Exp_TOD4_KW_Units;

                    xrLabel38.Text = op[0].L34_Exp_TOD1_KWH_Units;
                    xrLabel52.Text = op[0].L34_Exp_TOD2_KWH_Units;
                    xrLabel59.Text = op[0].L34_Exp_TOD3_KWH_Units;
                    xrLabel66.Text = op[0].L34_Exp_TOD4_KWH_Units;



                    //Current Net Unit
                    xrLabel39.Text = op[0].L43_Exp_CURRENT_NET_EXPORT_TOD1_KWH_UNITS;
                    xrLabel53.Text = op[0].L43_Exp_CURRENT_NET_EXPORT_TOD2_KWH_UNITS;
                    xrLabel60.Text = op[0].L43_Exp_CURRENT_NET_EXPORT_TOD3_KWH_UNITS;
                    xrLabel67.Text = op[0].L43_Exp_CURRENT_NET_EXPORT_TOD4_KWH_UNITS;
                    //Previous net Unit
                    xrLabel40.Text = op[0].L43_Previous_CREDIT_Units_TOD1_KWH;
                    xrLabel54.Text = op[0].L43_Previous_CREDIT_Units_TOD2_KWH;
                    xrLabel61.Text = op[0].L43_Previous_CREDIT_Units_TOD3_KWH;
                    xrLabel68.Text = op[0].L43_Previous_CREDIT_Units_TOD4_KWH;
                    //net bill unit
                    xrLabel41.Text = op[0].L46_Net_Billed_Units_MAIN_TOD1_KWH;
                    xrLabel55.Text = op[0].L46_Net_Billed_Units_MAIN_TOD2_KWH;
                    xrLabel62.Text = op[0].L46_Net_Billed_Units_MAIN_TOD3_KWH;
                    xrLabel69.Text = op[0].L46_Net_Billed_Units_MAIN_TOD4_KWH;
                    //carry forword unit
                    xrLabel42.Text = op[0].L45_Carry_Forward_Units_TOD1_KWH;
                    xrLabel56.Text = op[0].L45_Carry_Forward_Units_TOD2_KWH;
                    xrLabel63.Text = op[0].L45_Carry_Forward_Units_TOD3_KWH;
                    xrLabel70.Text = op[0].L45_Carry_Forward_Units_TOD4_KWH;

                    //mtr2
                    //MTR2_KW1.Text = op[0].L25_TOD_1_KWH;
                    //MTR2_KW2.Text = op[0].L25_TOD_2_KWH;
                    //MTR2_KW3.Text = op[0].L25_TOD_3_KWH;
                    //MTR2_KW4.Text = op[0].L25_TOD_4_KWH;
                    //MTR2_KW1_ex.Text = op[0].L54_Exp_TOD1_KW_Units;
                    //MTR2_KW2_ex.Text = op[0].L54_Exp_TOD2_KW_Units;
                    //MTR2_KW3_ex.Text = op[0].L54_Exp_TOD3_KW_Units;
                    //MTR2_KW4_ex.Text = op[0].L54_Exp_TOD4_KW_Units;

                    MTR2_KWH1.Text = op[0].L24_TOD_1_KWH;
                    MTR2_KWH2.Text = op[0].L24_TOD_2_KWH;
                    MTR2_KWH3.Text = op[0].L24_TOD_3_KWH;
                    MTR2_KWH4.Text = op[0].L24_TOD_4_KWH;
                    MTR2_KWH1_ex.Text = op[0].L51_Exp_TOD1_KWH_Units;
                    MTR2_KWH2_ex.Text = op[0].L51_Exp_TOD2_KWH_Units;
                    MTR2_KWH3_ex.Text = op[0].L51_Exp_TOD3_KWH_Units;
                    MTR2_KWH4_ex.Text = op[0].L51_Exp_TOD4_KWH_Units;



                }
                #endregion
                #region Meter(KVA)
                if (!string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "1")
                {

                    xrLabel16.Text = op[0].L23_TOD_1_KW;
                    xrLabel23.Text = op[0].L23_TOD_2_KW;
                    xrLabelcload.Text = op[0].L23_TOD_3_KW;
                    xrLabel30.Text = op[0].L23_TOD_4_KW;

                    xrLabel17.Text = op[0].L22_TOD_1_KWH;
                    xrLabel24.Text = op[0].L22_TOD_2_KWH;
                    xrLabel45.Text = op[0].L22_TOD_3_KWH;
                    xrLabel31.Text = op[0].L22_TOD_4_KWH;
                    //EXP
                    xrLabel37.Text = op[0].L36_Exp_TOD1_KVA_Units;
                    xrLabel51.Text = op[0].L36_Exp_TOD2_KVA_Units;
                    xrLabel58.Text = op[0].L36_Exp_TOD3_KVA_Units;
                    xrLabel65.Text = op[0].L36_Exp_TOD4_KVA_Units;


                    xrLabel38.Text = op[0].L35_Exp_TOD1_KVAH_Units;
                    xrLabel52.Text = op[0].L35_Exp_TOD2_KVAH_Units;
                    xrLabel59.Text = op[0].L35_Exp_TOD3_KVAH_Units;
                    xrLabel66.Text = op[0].L35_Exp_TOD4_KVAH_Units;

                    xrLabel39.Text = op[0].L42_Exp_CURRENT_NET_EXPORT_TOD1_KVAH_UNITS;
                    xrLabel53.Text = op[0].L42_Exp_CURRENT_NET_EXPORT_TOD2_KVAH_UNITS;
                    xrLabel60.Text = op[0].L42_Exp_CURRENT_NET_EXPORT_TOD3_KVAH_UNITS;
                    xrLabel67.Text = op[0].L42_Exp_CURRENT_NET_EXPORT_TOD4_KVAH_UNITS;


                    xrLabel40.Text = op[0].L42_Previous_CREDIT_Units_TOD1_KVAH;
                    xrLabel54.Text = op[0].L42_Previous_CREDIT_Units_TOD2_KVAH;
                    xrLabel61.Text = op[0].L42_Previous_CREDIT_Units_TOD3_KVAH;
                    xrLabel68.Text = op[0].L42_Previous_CREDIT_Units_TOD4_KVAH;

                    xrLabel41.Text = op[0].L46_Net_Billed_Units_MAIN_TOD1_KVAH;
                    xrLabel55.Text = op[0].L46_Net_Billed_Units_MAIN_TOD2_KVAH;
                    xrLabel62.Text = op[0].L46_Net_Billed_Units_MAIN_TOD3_KVAH;
                    xrLabel69.Text = op[0].L46_Net_Billed_Units_MAIN_TOD4_KVAH;


                    xrLabel42.Text = op[0].L44_Carry_Forward_Units_TOD1_KVAH;
                    xrLabel56.Text = op[0].L44_Carry_Forward_Units_TOD2_KVAH;
                    xrLabel63.Text = op[0].L44_Carry_Forward_Units_TOD3_KVAH;
                    xrLabel70.Text = op[0].L44_Carry_Forward_Units_TOD4_KVAH;

                    //mtr2
                    //MTR2_KW1.Text = op[0].L25_TOD_1_KWH;
                    //MTR2_KW2.Text = op[0].L25_TOD_2_KWH;
                    //MTR2_KW3.Text = op[0].L25_TOD_3_KWH;
                    //MTR2_KW4.Text = op[0].L25_TOD_4_KWH;
                    //MTR2_KW1_ex.Text = op[0].L52_Exp_TOD1_KVA_Units;
                    //MTR2_KW2_ex.Text = op[0].L52_Exp_TOD2_KVA_Units;
                    //MTR2_KW3_ex.Text = op[0].L52_Exp_TOD3_KVA_Units;
                    //MTR2_KW4_ex.Text = op[0].L52_Exp_TOD4_KVA_Units;

                    MTR2_KWH1.Text = op[0].L24_TOD_1_KWH;
                    MTR2_KWH2.Text = op[0].L24_TOD_2_KWH;
                    MTR2_KWH3.Text = op[0].L24_TOD_3_KWH;
                    MTR2_KWH4.Text = op[0].L24_TOD_4_KWH;
                    MTR2_KWH1_ex.Text = op[0].L50_Exp_TOD1_KVAH_Units;
                    MTR2_KWH2_ex.Text = op[0].L50_Exp_TOD2_KVAH_Units;
                    MTR2_KWH3_ex.Text = op[0].L50_Exp_TOD3_KVAH_Units;
                    MTR2_KWH4_ex.Text = op[0].L50_Exp_TOD4_KVAH_Units;


                }
                #endregion
            }
            else
            {
                //mtr2_IMP.Visible = false;
                //mtr2_exp.Visible = false;
                //MTR2_TOD1.Visible = false;
                //MTR2_TOD2.Visible = false;
                //MTR2_TOD3.Visible = false;
                //MTR2_TOD4.Visible = false;
                //MTR2_EXP1.Visible = false;
                //MTR2_EXP2.Visible = false;
                //MTR2_EXP3.Visible = false;
                //MTR2_EXP4.Visible = false;

                if (string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "0.00")
                {
                    MTR_TOD1.Text = op[0].L12_MTRSNO_METER1;
                    //MTR1
                    xrLabel16.Text = op[0].L23_TOD_1_KW;
                    xrLabel17.Text = op[0].L22_TOD_1_KWH;
                    xrLabel23.Text = op[0].L23_TOD_2_KW;
                    xrLabel24.Text = op[0].L22_TOD_2_KWH;
                    xrLabelcload.Text = op[0].L23_TOD_3_KW;
                    xrLabel45.Text = op[0].L22_TOD_3_KWH;
                    xrLabel30.Text = op[0].L23_TOD_4_KW;
                    xrLabel31.Text = op[0].L22_TOD_4_KWH;
                    //EXP
                    xrLabel37.Text = op[0].L53_Exp_TOD1_KW_Units;
                    xrLabel51.Text = op[0].L53_Exp_TOD2_KW_Units;
                    xrLabel58.Text = op[0].L53_Exp_TOD3_KW_Units;
                    xrLabel65.Text = op[0].L53_Exp_TOD4_KW_Units;

                    xrLabel38.Text = op[0].L34_Exp_TOD1_KWH_Units;
                    xrLabel52.Text = op[0].L34_Exp_TOD2_KWH_Units;
                    xrLabel59.Text = op[0].L34_Exp_TOD3_KWH_Units;
                    xrLabel66.Text = op[0].L34_Exp_TOD4_KWH_Units;




                    xrLabel39.Text = op[0].L43_Exp_CURRENT_NET_EXPORT_TOD1_KWH_UNITS;
                    xrLabel40.Text = op[0].L43_Previous_CREDIT_Units_TOD1_KWH;
                    xrLabel41.Text = op[0].L46_Net_Billed_Units_MAIN_TOD1_KWH;
                    xrLabel42.Text = op[0].L45_Carry_Forward_Units_TOD1_KWH;
                    //other
                    xrLabel53.Text = op[0].L43_Exp_CURRENT_NET_EXPORT_TOD2_KWH_UNITS;
                    xrLabel54.Text = op[0].L43_Previous_CREDIT_Units_TOD2_KWH;
                    xrLabel55.Text = op[0].L46_Net_Billed_Units_MAIN_TOD2_KWH;
                    xrLabel56.Text = op[0].L45_Carry_Forward_Units_TOD2_KWH;
                    xrLabel60.Text = op[0].L43_Exp_CURRENT_NET_EXPORT_TOD3_KWH_UNITS;
                    xrLabel61.Text = op[0].L43_Previous_CREDIT_Units_TOD3_KWH;
                    xrLabel62.Text = op[0].L46_Net_Billed_Units_MAIN_TOD3_KWH;
                    xrLabel63.Text = op[0].L45_Carry_Forward_Units_TOD3_KWH;
                    xrLabel67.Text = op[0].L43_Exp_CURRENT_NET_EXPORT_TOD4_KWH_UNITS;
                    xrLabel68.Text = op[0].L43_Previous_CREDIT_Units_TOD4_KWH;
                    xrLabel69.Text = op[0].L46_Net_Billed_Units_MAIN_TOD4_KWH;
                    xrLabel70.Text = op[0].L45_Carry_Forward_Units_TOD4_KWH;

                }
                if (!string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "1")
                {
                    MTR_TOD1.Text = op[0].L12_MTRSNO_METER1;

                    xrLabel16.Text = op[0].L23_TOD_1_KW;
                    xrLabel23.Text = op[0].L23_TOD_2_KW;
                    xrLabelcload.Text = op[0].L23_TOD_3_KW;
                    xrLabel30.Text = op[0].L23_TOD_4_KW;

                    xrLabel17.Text = op[0].L22_TOD_1_KWH;
                    xrLabel24.Text = op[0].L22_TOD_2_KWH;
                    xrLabel45.Text = op[0].L22_TOD_3_KWH;
                    xrLabel31.Text = op[0].L22_TOD_4_KWH;
                    //EXP
                    xrLabel37.Text = op[0].L36_Exp_TOD1_KVA_Units;
                    xrLabel51.Text = op[0].L36_Exp_TOD2_KVA_Units;
                    xrLabel58.Text = op[0].L36_Exp_TOD3_KVA_Units;
                    xrLabel65.Text = op[0].L36_Exp_TOD4_KVA_Units;


                    xrLabel38.Text = op[0].L35_Exp_TOD1_KVAH_Units;
                    xrLabel52.Text = op[0].L35_Exp_TOD2_KVAH_Units;
                    xrLabel59.Text = op[0].L35_Exp_TOD3_KVAH_Units;
                    xrLabel66.Text = op[0].L35_Exp_TOD4_KVAH_Units;

                    xrLabel39.Text = op[0].L42_Exp_CURRENT_NET_EXPORT_TOD1_KVAH_UNITS;
                    xrLabel53.Text = op[0].L42_Exp_CURRENT_NET_EXPORT_TOD2_KVAH_UNITS;
                    xrLabel60.Text = op[0].L42_Exp_CURRENT_NET_EXPORT_TOD3_KVAH_UNITS;
                    xrLabel67.Text = op[0].L42_Exp_CURRENT_NET_EXPORT_TOD4_KVAH_UNITS;


                    xrLabel40.Text = op[0].L42_Previous_CREDIT_Units_TOD1_KVAH;
                    xrLabel54.Text = op[0].L42_Previous_CREDIT_Units_TOD2_KVAH;
                    xrLabel61.Text = op[0].L42_Previous_CREDIT_Units_TOD3_KVAH;
                    xrLabel68.Text = op[0].L42_Previous_CREDIT_Units_TOD4_KVAH;

                    xrLabel41.Text = op[0].L46_Net_Billed_Units_MAIN_TOD1_KVAH;
                    xrLabel55.Text = op[0].L46_Net_Billed_Units_MAIN_TOD2_KVAH;
                    xrLabel62.Text = op[0].L46_Net_Billed_Units_MAIN_TOD3_KVAH;
                    xrLabel69.Text = op[0].L46_Net_Billed_Units_MAIN_TOD4_KVAH;


                    xrLabel42.Text = op[0].L44_Carry_Forward_Units_TOD1_KVAH;
                    xrLabel56.Text = op[0].L44_Carry_Forward_Units_TOD2_KVAH;
                    xrLabel63.Text = op[0].L44_Carry_Forward_Units_TOD3_KVAH;
                    xrLabel70.Text = op[0].L44_Carry_Forward_Units_TOD4_KVAH;

                }

            }

            #region Meter Print2
            if (op[0].L37_Gen_Meter_Serial_Number != "")
            {
                if (string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "0.00")
                {
                    xrLabel78.Text = op[0].L37_Gen_Meter_Serial_Number;

                    xrLabel81.Text = op[0].L39_Gen_KVA_PASTREAD;
                    xrLabel82.Text = op[0].L38_Gen_KVA_PRESREAD;
                    xrLabel83.Text = op[0].L40_Gen_MF3;
                    xrLabel84.Text = op[0].L41_Gen_KVA_NET_UNITS;
                    xrLabel85.Text = op[0].L39_Gen_KWH_PASTREAD;
                    xrLabel86.Text = op[0].L38_Gen_KWH_PRESREAD;
                    xrLabel87.Text = op[0].L40_Gen_MF1;
                    xrLabel88.Text = op[0].L41_Gen_KWH_NET_UNITS;


                    #region Consumption Information
                    //Months
                    xrLabel95.Text = op[0].L21_MonYear1;
                    xrLabel96.Text = op[0].L21_MonYear2;
                    xrLabel97.Text = op[0].L21_MonYear3;
                    xrLabel98.Text = op[0].L21_MonYear4;
                    xrLabel99.Text = op[0].L21_MonYear5;
                    xrLabel100.Text = op[0].L21_MonYear6;
                    //Billed KVA/KW
                    xrLabel101.Text = op[0].L21_KVA_UNITS1;
                    xrLabel102.Text = op[0].L21_KVA_UNITS2;
                    xrLabel103.Text = op[0].L21_KVA_UNITS3;
                    xrLabel104.Text = op[0].L21_KVA_UNITS4;
                    xrLabel105.Text = op[0].L21_KVA_UNITS5;
                    xrLabel106.Text = op[0].L21_KVA_UNITS6;
                    //Billed KVAH/KWH
                    xrLabel107.Text = op[0].L11_KWH_UNITS1;
                    xrLabel108.Text = op[0].L11_KWH_UNITS2;
                    xrLabel109.Text = op[0].L11_KWH_UNITS3;
                    xrLabel110.Text = op[0].L11_KWH_UNITS4;
                    xrLabel111.Text = op[0].L11_KWH_UNITS5;
                    xrLabel112.Text = op[0].L11_KWH_UNITS6;
                    //Export KVAH/KWH
                    xrLabel113.Text = op[0].L47_Exp_KVAH_UNITS1;
                    xrLabel114.Text = op[0].L47_Exp_KVAH_UNITS2;
                    xrLabel115.Text = op[0].L47_Exp_KVAH_UNITS3;
                    xrLabel116.Text = op[0].L47_Exp_KVAH_UNITS4;
                    xrLabel117.Text = op[0].L47_Exp_KVAH_UNITS5;
                    xrLabel118.Text = op[0].L47_Exp_KVAH_UNITS6;
                    //Gen. KVAH/KWH
                    xrLabel119.Text = op[0].L48_Gen_KVAH_UNITS1;
                    xrLabel120.Text = op[0].L48_Gen_KVAH_UNITS2;
                    xrLabel121.Text = op[0].L48_Gen_KVAH_UNITS3;
                    xrLabel122.Text = op[0].L48_Gen_KVAH_UNITS4;
                    xrLabel123.Text = op[0].L48_Gen_KVAH_UNITS5;
                    xrLabel124.Text = op[0].L48_Gen_KVAH_UNITS6;
                    #endregion

                }
                if (!string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "1")
                {
                    xrLabel78.Text = op[0].L37_Gen_Meter_Serial_Number;

                    xrLabel81.Text = op[0].L39_Gen_KVA_PASTREAD;
                    xrLabel82.Text = op[0].L38_Gen_KVA_PRESREAD;
                    xrLabel83.Text = op[0].L40_Gen_MF3;
                    xrLabel84.Text = op[0].L41_Gen_KVA_NET_UNITS;
                    xrLabel85.Text = op[0].L39_Gen_KVAH_PASTREAD;
                    xrLabel86.Text = op[0].L38_Gen_KVAH_PRESREAD;
                    xrLabel87.Text = op[0].L40_Gen_MF2;
                    xrLabel88.Text = op[0].L41_Gen_KVAH_NET_UNITS;

                    #region Consumption Information
                    //Months
                    xrLabel95.Text = op[0].L21_MonYear1;
                    xrLabel96.Text = op[0].L21_MonYear2;
                    xrLabel97.Text = op[0].L21_MonYear3;
                    xrLabel98.Text = op[0].L21_MonYear4;
                    xrLabel99.Text = op[0].L21_MonYear5;
                    xrLabel100.Text = op[0].L21_MonYear6;
                    //Billed KVA/KW
                    xrLabel101.Text = op[0].L21_KVA_UNITS1;
                    xrLabel102.Text = op[0].L21_KVA_UNITS2;
                    xrLabel103.Text = op[0].L21_KVA_UNITS3;
                    xrLabel104.Text = op[0].L21_KVA_UNITS4;
                    xrLabel105.Text = op[0].L21_KVA_UNITS5;
                    xrLabel106.Text = op[0].L21_KVA_UNITS6;
                    //Billed KVAH/KWH
                    xrLabel107.Text = op[0].L11_KWH_UNITS1;
                    xrLabel108.Text = op[0].L11_KWH_UNITS2;
                    xrLabel109.Text = op[0].L11_KWH_UNITS3;
                    xrLabel110.Text = op[0].L11_KWH_UNITS4;
                    xrLabel111.Text = op[0].L11_KWH_UNITS5;
                    xrLabel112.Text = op[0].L11_KWH_UNITS6;
                    //Export KVAH/KWH
                    xrLabel113.Text = op[0].L47_Exp_KVAH_UNITS1;
                    xrLabel114.Text = op[0].L47_Exp_KVAH_UNITS2;
                    xrLabel115.Text = op[0].L47_Exp_KVAH_UNITS3;
                    xrLabel116.Text = op[0].L47_Exp_KVAH_UNITS4;
                    xrLabel117.Text = op[0].L47_Exp_KVAH_UNITS5;
                    xrLabel118.Text = op[0].L47_Exp_KVAH_UNITS6;
                    //Gen. KVAH/KWH
                    xrLabel119.Text = op[0].L48_Gen_KVAH_UNITS1;
                    xrLabel120.Text = op[0].L48_Gen_KVAH_UNITS2;
                    xrLabel121.Text = op[0].L48_Gen_KVAH_UNITS3;
                    xrLabel122.Text = op[0].L48_Gen_KVAH_UNITS4;
                    xrLabel123.Text = op[0].L48_Gen_KVAH_UNITS5;
                    xrLabel124.Text = op[0].L48_Gen_KVAH_UNITS6;
                    #endregion

                }
            }
            #endregion
        }
        private decimal ToDecimal(string value)
        {
            decimal.TryParse(value, out decimal result);
            return result;
        }

    }

}

