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



        private void Rpt_solar_PDF_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var data = sender as Rpt_LTMD_Solar_PDF;
            var op = data.DataSource as List<SolarBill>;


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

            //xrLabelTotalAmt.Text = "₹" + ToDecimal(op[0].L8_amount_payable_before_due_date).ToString("G");
            xrLabelTotal.Text = "Thank you for your previous payment of ₹" + op[0].L7_Last_Payement_amount + " on " + op[0].L7_LastpymtDate;
            xrLabel13.BringToFront();
            xrLabelday.BringToFront();
            xrLabel19.BringToFront();
            xrLine1.SendToBack();


            if (!string.IsNullOrEmpty(op[0].L1_Customer_PAN))
            {
                xrLabelPanNo.Visible = true;
            }
            else
            {
                xrLabelPanNo.Visible = false;
            }

            // To keep Address and PAN together             
            if (op[0].L2_NAME.ToString() == "")
            {
                xrLabelName.Visible = false;
                xrLabeladd1.TopF = xrLabelName.TopF;
            }
            if (op[0].L3_ADDR1.ToString() == "")
            {
                xrLabeladd1.Visible = false;
                xrLabeladd2.TopF = xrLabeladd1.TopF;
            }
            if (op[0].L4_ADDR2.ToString() == "")
            {
                xrLabeladd2.Visible = false;
                xrLabelAdd3.TopF = xrLabeladd2.TopF;
            }
            if (op[0].L5_ADDR3.ToString() == "")
            {
                xrLabelAdd3.Visible = false;
                xrLabelRegMobile.TopF = xrLabelAdd3.TopF;
                xrLabelRedEmail.TopF = xrLabelRegMobile.BottomF;
                xrLabelPanNo.TopF = xrLabelRedEmail.BottomF;
            }
            else
            {
                xrLabelRegMobile.TopF = xrLabelAdd3.BottomF;
                xrLabelRedEmail.TopF = xrLabelRegMobile.BottomF;
                xrLabelPanNo.TopF = xrLabelRedEmail.BottomF;
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

                    
                    KW_HEAD1.Text = op[0].unit1;
                    KWH_HEAD1.Text = op[0].unit1 + "H(I)";
                    KW_HEAD2.Text = op[0].unit1;
                    KWH_HEAD2.Text = op[0].unit1 + "H(I)";
                    KWHE_HEAD1.Text = op[0].unit1 + "H(E)";
                    KWHE_HEAD2.Text = op[0].unit1 + "H(E)";
                }
                else
                {
                    op[0].unit1 = "KW";
                    KW_HEAD1.Text = op[0].unit1;
                    KWH_HEAD1.Text = op[0].unit1 + "H(I)";
                    KW_HEAD2.Text = op[0].unit1;
                    KWH_HEAD2.Text = op[0].unit1 + "H(I)";
                    KWHE_HEAD1.Text = op[0].unit1 + "H(E)";
                    KWHE_HEAD2.Text = op[0].unit1 + "H(E)";
                }
            }
            else if (op[0].L6_MEASURE_OF_CONTRACT_Demand == "KW")
            {
                if (!string.IsNullOrEmpty(op[0].L6_Kvah_indicator) && op[0].L6_Kvah_indicator == "1")
                {
                    op[0].unit1 = "KVA";
                    KW_HEAD1.Text = op[0].unit1;
                    KWH_HEAD1.Text = op[0].unit1 + "H(I)";
                    KW_HEAD2.Text = op[0].unit1;
                    KWH_HEAD2.Text = op[0].unit1 + "H(I)";
                    KWHE_HEAD1.Text = op[0].unit1 + "H(E)";
                    KWHE_HEAD2.Text = op[0].unit1 + "H(E)";
                }
                else
                {
                    op[0].unit1 = "KW";
                    KW_HEAD1.Text = op[0].unit1;
                    KWH_HEAD1.Text = op[0].unit1 + "H(I)";
                    KW_HEAD2.Text = op[0].unit1;
                    KWH_HEAD2.Text = op[0].unit1 + "H(I)";
                    KWHE_HEAD1.Text = op[0].unit1 + "H(E)";
                    KWHE_HEAD2.Text = op[0].unit1 + "H(E)";
                }
            }
            else if (op[0].L6_MEASURE_OF_CONTRACT_Demand == "KVA")
            {
                if (op[0].L6_Kvah_indicator == "1")
                {
                    op[0].unit1 = "KVA";

                    KW_HEAD1.Text = op[0].unit1;
                    KWH_HEAD1.Text = op[0].unit1 + "H(I)";
                    KW_HEAD2.Text = op[0].unit1;
                    KWH_HEAD2.Text = op[0].unit1 + "H(I)";
                    KWHE_HEAD1.Text = op[0].unit1 + "H(E)";
                    KWHE_HEAD2.Text = op[0].unit1 + "H(E)";
                }
                else
                {
                    op[0].unit1 = "KW";
                    KW_HEAD1.Text = op[0].unit1;
                    KWH_HEAD1.Text = op[0].unit1 + "H(I)";
                    KW_HEAD2.Text = op[0].unit1;
                    KWH_HEAD2.Text = op[0].unit1 + "H(I)";
                    KWHE_HEAD1.Text = op[0].unit1 + "H(E)";
                    KWHE_HEAD2.Text = op[0].unit1 + "H(E)";

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
            pieSeries.Points.Add(new SeriesPoint("Energy Charges", energyCharge));
            pieSeries.Points.Add(new SeriesPoint("Excess Demand Charge", excessDemandCharge));
            pieSeries.Points.Add(new SeriesPoint("Fixed Charges", fixedCharge));


            DoughnutSeriesLabel label = (DoughnutSeriesLabel)pieSeries.Label;
            label.Position = PieSeriesLabelPosition.TwoColumns;
            label.ResolveOverlappingMode = ResolveOverlappingMode.Default;
            label.ResolveOverlappingMinIndent = 15;

            label.TextPattern = "{A}\n₹{V:G}";
            label.TextColor = Color.Black;
            label.Font = new Font("Manrope", 5);
            label.BackColor = Color.Transparent;
            label.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;

            DoughnutSeriesView view = (DoughnutSeriesView)pieSeries.View;
            view.HoleRadiusPercent = 75;
            view.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;

            pieSeries.Points[0].Color = Color.FromArgb(208, 208, 207);  // Duty
            pieSeries.Points[1].Color = Color.FromArgb(151, 151, 151);   // Energy
            pieSeries.Points[2].Color = Color.FromArgb(179, 180, 180); // Excess
            pieSeries.Points[3].Color = Color.FromArgb(125, 125, 124);  // Fixed
            xrChartPie.Series.Add(pieSeries);

            decimal tValue = energyCharge + fixedCharge + electricityDuty + excessDemandCharge;
            xrLabelTotalAmt.Text = "₹" + tValue.ToString();
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
            xrLabel2.BringToFront();
            xrLabel18.BringToFront();
            #region front page details
            #region Meter Change
            //Meter Change Print
            if (op[0].L12_MTRSNO_METER_2_IF_AVAILABLE != "")
            {
                xrLabel2.Text = op[0].L12_MTRSNO_METER_2_IF_AVAILABLE;//older
                xrLabel18.Text = op[0].L12_MTRSNO_METER1;

                if (string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "0.00")
                {    //mtr1
                    imp11.Text = op[0].L14_KVA_PASTREAD;
                    imp21.Text = op[0].L14_KWH_PASTREAD;
                    exp21.Text = op[0].L33_Exp_Past_KWH_UNITS;

                    imp12.Text = op[0].L13_KVA_PRESREAD;
                    imp22.Text = op[0].L13_KWH_PRESREAD;
                    exp22.Text = op[0].L33_Exp_Present_KWH_UNITS;

                    imp13.Text = op[0].L15_Multiplying_factor_KVA;
                    imp23.Text = op[0].L15_Multiplying_factor_KWH;
                    exp23.Text = op[0].L15_Multiplying_factor_KWH;

                    imp14.Text = op[0].L16_KVA_UNITS;
                    imp24.Text = op[0].L16_KWH_UNITS;
                    exp24.Text = op[0].L33_Exp_KWH_UNITS;

                    kva11.Text = op[0].L33_Exp_CURRENT_NET_EXPORT_KWH_UNITS;
                    kva12.Text = op[0].L46_Previous_CREDIT_Units_MAIN_KWH;
                    kva13.Text = op[0].L46_Net_Billed_Units_MAIN_KWH;
                    kva14.Text = op[0].L46_Carry_Forward_Units_MAIN_KWH;

                    //mtr2
                    MTR2_PR1.Text = op[0].L18_KVA_PASTREAD;
                    MTR2_PR3.Text = op[0].L18_KWH_PASTREAD;
                    MTR2_PR4.Text = op[0].L49_Exp_Past_KWH_UNITS;

                    MTR2_CR1.Text = op[0].L17_KVA_PRESREAD;
                    MTR2_CR3.Text = op[0].L17_KWH_PRESREAD;
                    MTR2_CR4.Text = op[0].L49_Exp_Present_KWH_UNITS;

                    MTR2_MF1.Text = op[0].L19_Multiplying_factor_KW;
                    MTR2_MF3.Text = op[0].L19_Multiplying_factor_KWH;
                    MTR2_MF4.Text = op[0].L19_Multiplying_factor_KWH;

                    MTR2_CU1.Text = op[0].L20_KVA_UNITS;
                    MTR2_CU3.Text = op[0].L20_KWH_UNITS;
                    MTR2_CU4.Text = op[0].L49_Exp_KWH_UNITS;


                }

                if (!string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "1")
                {
                    //MTR1
                    imp11.Text = op[0].L14_KVA_PASTREAD;
                    imp21.Text = op[0].L14_KWH_PASTREAD;
                    exp21.Text = op[0].L33_Exp_Past_KVAH_UNITS;

                    imp12.Text = op[0].L13_KVA_PRESREAD;
                    imp22.Text = op[0].L13_KWH_PRESREAD;
                    exp22.Text = op[0].L33_Exp_Present_KVAH_UNITS;

                    imp13.Text = op[0].L15_Multiplying_factor_KVA;
                    imp23.Text = op[0].L15_Multiplying_factor_KWH;
                    exp23.Text = op[0].L15_Multiplying_factor_KWH;

                    imp14.Text = op[0].L16_KVA_UNITS;
                    imp24.Text = op[0].L16_KWH_UNITS;
                    exp24.Text = op[0].L33_Exp_KVAH_UNITS;

                    kva11.Text = op[0].L33_Exp_CURRENT_NET_EXPORT_KVAH_UNITS;
                    kva12.Text = op[0].L46_Previous_CREDIT_Units_MAIN_KVAH;
                    kva13.Text = op[0].L46_Net_Billed_Units_MAIN;
                    kva14.Text = op[0].L46_Carry_Forward_Units_MAIN_KVAH;

                    //MTR2
                    MTR2_PR1.Text = op[0].L18_KVA_PASTREAD;
                    MTR2_PR3.Text = op[0].L18_KWH_PASTREAD;
                    MTR2_PR4.Text = op[0].L49_Exp_Past_KVAH_UNITS;

                    MTR2_CR1.Text = op[0].L17_KVA_PRESREAD;
                    MTR2_CR3.Text = op[0].L17_KWH_PRESREAD;
                    MTR2_CR4.Text = op[0].L49_Exp_Present_KVAH_UNITS;

                    MTR2_MF1.Text = op[0].L19_Multiplying_factor_KW;
                    MTR2_MF3.Text = op[0].L19_Multiplying_factor_KWH;
                    MTR2_MF4.Text = op[0].L19_Multiplying_factor_KWH;

                    MTR2_CU1.Text = op[0].L20_KVA_UNITS;
                    MTR2_CU3.Text = op[0].L20_KWH_UNITS;
                    MTR2_CU4.Text = op[0].L49_Exp_KVAH_UNITS;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "0.00")
                {
                    xrLabel18.Text = op[0].L12_MTRSNO_METER1;
                    KW_HEAD2.Visible = false;
                    KWH_HEAD2.Visible = false;
                    KWHE_HEAD2.Visible = false;
                    //Newer                                                   
                    imp11.Text = op[0].L14_KVA_PASTREAD;
                    imp21.Text = op[0].L14_KWH_PASTREAD;
                    exp21.Text = op[0].L33_Exp_Past_KWH_UNITS;

                    imp12.Text = op[0].L13_KVA_PRESREAD;
                    imp22.Text = op[0].L13_KWH_PRESREAD;
                    exp22.Text = op[0].L33_Exp_Present_KWH_UNITS;

                    imp13.Text = op[0].L15_Multiplying_factor_KVA;
                    imp23.Text = op[0].L15_Multiplying_factor_KWH;
                    exp23.Text = op[0].L15_Multiplying_factor_KWH;

                    imp14.Text = op[0].L16_KVA_UNITS;
                    imp24.Text = op[0].L16_KWH_UNITS;
                    exp24.Text = op[0].L33_Exp_KWH_UNITS;

                    kva11.Text = op[0].L33_Exp_CURRENT_NET_EXPORT_KWH_UNITS;
                    kva12.Text = op[0].L46_Previous_CREDIT_Units_MAIN_KWH;
                    kva13.Text = op[0].L46_Net_Billed_Units_MAIN_KWH;
                    kva14.Text = op[0].L46_Carry_Forward_Units_MAIN_KWH;
                }
                if (!string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "1")
                {
                    xrLabel18.Text = op[0].L12_MTRSNO_METER1;
                    KW_HEAD2.Visible = false;
                    KWH_HEAD2.Visible = false;
                    KWHE_HEAD2.Visible = false;
                    //Newer
                    imp11.Text = op[0].L14_KVA_PASTREAD;
                    imp21.Text = op[0].L14_KWH_PASTREAD;
                    exp21.Text = op[0].L33_Exp_Past_KVAH_UNITS;

                    imp12.Text = op[0].L13_KVA_PRESREAD;
                    imp22.Text = op[0].L13_KWH_PRESREAD;
                    exp22.Text = op[0].L33_Exp_Present_KVAH_UNITS;

                    imp13.Text = op[0].L15_Multiplying_factor_KVA;
                    imp23.Text = op[0].L15_Multiplying_factor_KWH;
                    exp23.Text = op[0].L15_Multiplying_factor_KWH;

                    imp14.Text = op[0].L16_KVA_UNITS;
                    imp24.Text = op[0].L16_KWH_UNITS;
                    exp24.Text = op[0].L33_Exp_KVAH_UNITS;

                    kva11.Text = op[0].L33_Exp_CURRENT_NET_EXPORT_KVAH_UNITS;
                    kva12.Text = op[0].L46_Previous_CREDIT_Units_MAIN_KVAH;
                    kva13.Text = op[0].L46_Net_Billed_Units_MAIN;
                    kva14.Text = op[0].L46_Carry_Forward_Units_MAIN_KVAH;
                }

            }
            #endregion

            #endregion
            #region Total 
            //tod details total meter 1
            decimal v1 = ToDecimal(op[0].L22_TOD_1_KWH);
            decimal v2 = ToDecimal(op[0].L22_TOD_2_KWH);
            decimal v3 = ToDecimal(op[0].L22_TOD_3_KWH);
            decimal v4 = ToDecimal(op[0].L22_TOD_4_KWH);
            decimal total = v1 + v2 + v3 + v4;


            decimal v11 = ToDecimal(op[0].L34_Exp_TOD1_KWH_Units);
            decimal v21 = ToDecimal(op[0].L34_Exp_TOD2_KWH_Units);
            decimal v31 = ToDecimal(op[0].L34_Exp_TOD3_KWH_Units);
            decimal v41 = ToDecimal(op[0].L34_Exp_TOD4_KWH_Units);
            decimal total2 = v11 + v21 + v31 + v41;
            decimal v51 = ToDecimal(op[0].L35_Exp_TOD1_KVAH_Units);
            decimal v61 = ToDecimal(op[0].L35_Exp_TOD2_KVAH_Units);
            decimal v71 = ToDecimal(op[0].L35_Exp_TOD3_KVAH_Units);
            decimal v81 = ToDecimal(op[0].L35_Exp_TOD4_KVAH_Units);
            decimal total3 = v51 + v61 + v71 + v81;

            //tod details total meter 2
            decimal a11 = ToDecimal(op[0].L24_TOD_1_KWH);
            decimal a21 = ToDecimal(op[0].L24_TOD_2_KWH);
            decimal a31 = ToDecimal(op[0].L24_TOD_3_KWH);
            decimal a41 = ToDecimal(op[0].L24_TOD_4_KWH);
            decimal totalA1 = a11 + a21 + a31 + a41;

            decimal a12 = ToDecimal(op[0].L51_Exp_TOD1_KWH_Units);
            decimal a22 = ToDecimal(op[0].L51_Exp_TOD2_KWH_Units);
            decimal a32 = ToDecimal(op[0].L51_Exp_TOD3_KWH_Units);
            decimal a42 = ToDecimal(op[0].L51_Exp_TOD4_KWH_Units);
            decimal totalA2 = a12 + a22 + a32 + a42;

            decimal a13 = ToDecimal(op[0].L50_Exp_TOD1_KVAH_Units);
            decimal a23 = ToDecimal(op[0].L50_Exp_TOD2_KVAH_Units);
            decimal a33 = ToDecimal(op[0].L50_Exp_TOD3_KVAH_Units);
            decimal a43 = ToDecimal(op[0].L50_Exp_TOD4_KVAH_Units);
            decimal totalA3 = a13 + a23 + a33 + a43;

            //Current Net Unit total
            decimal i11 = ToDecimal(op[0].L43_Exp_CURRENT_NET_EXPORT_TOD1_KWH_UNITS);
            decimal i21 = ToDecimal(op[0].L43_Exp_CURRENT_NET_EXPORT_TOD2_KWH_UNITS);
            decimal i31 = ToDecimal(op[0].L43_Exp_CURRENT_NET_EXPORT_TOD3_KWH_UNITS);
            decimal i41 = ToDecimal(op[0].L43_Exp_CURRENT_NET_EXPORT_TOD4_KWH_UNITS);
            decimal totalI1 = i11 + i21 + i31 + i41;

            decimal i12 = ToDecimal(op[0].L42_Exp_CURRENT_NET_EXPORT_TOD1_KVAH_UNITS);
            decimal i22 = ToDecimal(op[0].L42_Exp_CURRENT_NET_EXPORT_TOD2_KVAH_UNITS);
            decimal i32 = ToDecimal(op[0].L42_Exp_CURRENT_NET_EXPORT_TOD3_KVAH_UNITS);
            decimal i42 = ToDecimal(op[0].L42_Exp_CURRENT_NET_EXPORT_TOD4_KVAH_UNITS);
            decimal totalI2 = i12 + i22 + i32 + i42;


            //Previous net Unit
            decimal j11 = ToDecimal(op[0].L43_Previous_CREDIT_Units_TOD1_KWH);
            decimal j21 = ToDecimal(op[0].L43_Previous_CREDIT_Units_TOD2_KWH);
            decimal j31 = ToDecimal(op[0].L43_Previous_CREDIT_Units_TOD3_KWH);
            decimal j41 = ToDecimal(op[0].L43_Previous_CREDIT_Units_TOD4_KWH);
            decimal totalJ1 = j11 + j21 + j31 + j41;


            decimal j12 = ToDecimal(op[0].L42_Previous_CREDIT_Units_TOD1_KVAH);
            decimal j22 = ToDecimal(op[0].L42_Previous_CREDIT_Units_TOD2_KVAH);
            decimal j32 = ToDecimal(op[0].L42_Previous_CREDIT_Units_TOD3_KVAH);
            decimal j42 = ToDecimal(op[0].L42_Previous_CREDIT_Units_TOD4_KVAH);
            decimal totalJ2 = j12 + j22 + j32 + j42;

            //
            decimal k11 = ToDecimal(op[0].L46_Net_Billed_Units_MAIN_TOD1_KWH);
            decimal k21 = ToDecimal(op[0].L46_Net_Billed_Units_MAIN_TOD2_KWH);
            decimal k31 = ToDecimal(op[0].L46_Net_Billed_Units_MAIN_TOD3_KWH);
            decimal k41 = ToDecimal(op[0].L46_Net_Billed_Units_MAIN_TOD4_KWH);
            decimal totalK1 = k11 + k21 + k31 + k41;


            decimal k12 = ToDecimal(op[0].L46_Net_Billed_Units_MAIN_TOD1_KVAH);
            decimal k22 = ToDecimal(op[0].L46_Net_Billed_Units_MAIN_TOD2_KVAH);
            decimal k32 = ToDecimal(op[0].L46_Net_Billed_Units_MAIN_TOD3_KVAH);
            decimal k42 = ToDecimal(op[0].L46_Net_Billed_Units_MAIN_TOD4_KVAH);
            decimal totalK2 = k12 + k22 + k32 + k42;

            //

            decimal l11 = ToDecimal(op[0].L45_Carry_Forward_Units_TOD1_KWH);
            decimal l21 = ToDecimal(op[0].L45_Carry_Forward_Units_TOD2_KWH);
            decimal l31 = ToDecimal(op[0].L45_Carry_Forward_Units_TOD3_KWH);
            decimal l41 = ToDecimal(op[0].L45_Carry_Forward_Units_TOD4_KWH);
            decimal totalL1 = l11 + l21 + l31 + l41;


            decimal l12 = ToDecimal(op[0].L44_Carry_Forward_Units_TOD1_KVAH);
            decimal l22 = ToDecimal(op[0].L44_Carry_Forward_Units_TOD2_KVAH);
            decimal l32 = ToDecimal(op[0].L44_Carry_Forward_Units_TOD3_KVAH);
            decimal l42 = ToDecimal(op[0].L44_Carry_Forward_Units_TOD4_KVAH);
            decimal totall2 = l12 + l22 + l32 + l42;


            #endregion

            #region back page details
            if (op[0].L12_MTRSNO_METER_2_IF_AVAILABLE != "")
            {
                MTR_TOD1.Text = op[0].L12_MTRSNO_METER1;
                MTR_TOD2.Text = op[0].L12_MTRSNO_METER_2_IF_AVAILABLE;
                #region Meter(KW)
                if (string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "0.00")
                {
                    xrLabel17.Text = op[0].L22_TOD_1_KWH;
                    xrLabel24.Text = op[0].L22_TOD_2_KWH;
                    xrLabel45.Text = op[0].L22_TOD_3_KWH;
                    xrLabel31.Text = op[0].L22_TOD_4_KWH;

                    xrLabelab.Text = total.ToString("0.00");
                   


                    xrLabel38.Text = op[0].L34_Exp_TOD1_KWH_Units;
                    xrLabel52.Text = op[0].L34_Exp_TOD2_KWH_Units;
                    xrLabel59.Text = op[0].L34_Exp_TOD3_KWH_Units;
                    xrLabel66.Text = op[0].L34_Exp_TOD4_KWH_Units;
                    xrLabelcd.Text  = total2.ToString("0.00");


                    //Current Net Unit
                    xrLabel39.Text = op[0].L43_Exp_CURRENT_NET_EXPORT_TOD1_KWH_UNITS;
                    xrLabel53.Text = op[0].L43_Exp_CURRENT_NET_EXPORT_TOD2_KWH_UNITS;
                    xrLabel60.Text = op[0].L43_Exp_CURRENT_NET_EXPORT_TOD3_KWH_UNITS;
                    xrLabel67.Text = op[0].L43_Exp_CURRENT_NET_EXPORT_TOD4_KWH_UNITS;
                    xrLabel16.Text = totalI1.ToString("0.00");
                    //Previous net Unit
                    xrLabel40.Text = op[0].L43_Previous_CREDIT_Units_TOD1_KWH;
                    xrLabel54.Text = op[0].L43_Previous_CREDIT_Units_TOD2_KWH;
                    xrLabel61.Text = op[0].L43_Previous_CREDIT_Units_TOD3_KWH;
                    xrLabel68.Text = op[0].L43_Previous_CREDIT_Units_TOD4_KWH;
                    xrLabel25.Text = totalJ1.ToString("0.00");
                    //net bill unit
                    xrLabel41.Text = op[0].L46_Net_Billed_Units_MAIN_TOD1_KWH;
                    xrLabel55.Text = op[0].L46_Net_Billed_Units_MAIN_TOD2_KWH;
                    xrLabel62.Text = op[0].L46_Net_Billed_Units_MAIN_TOD3_KWH;
                    xrLabel69.Text = op[0].L46_Net_Billed_Units_MAIN_TOD4_KWH;
                    xrLabel23.Text = totalK1.ToString("0.00");
                    //carry forword unit
                    xrLabel42.Text = op[0].L45_Carry_Forward_Units_TOD1_KWH;
                    xrLabel56.Text = op[0].L45_Carry_Forward_Units_TOD2_KWH;
                    xrLabel63.Text = op[0].L45_Carry_Forward_Units_TOD3_KWH;
                    xrLabel70.Text = op[0].L45_Carry_Forward_Units_TOD4_KWH;
                    xrLabel20.Text = totalL1.ToString("0.00");



                    MTR2_KWH1.Text = op[0].L24_TOD_1_KWH;
                    MTR2_KWH2.Text = op[0].L24_TOD_2_KWH;
                    MTR2_KWH3.Text = op[0].L24_TOD_3_KWH;
                    MTR2_KWH4.Text = op[0].L24_TOD_4_KWH;
                    xrLabel11.Text = totalA1.ToString("0.00");

                    MTR2_KWH1_ex.Text = op[0].L51_Exp_TOD1_KWH_Units;
                    MTR2_KWH2_ex.Text = op[0].L51_Exp_TOD2_KWH_Units;
                    MTR2_KWH3_ex.Text = op[0].L51_Exp_TOD3_KWH_Units;
                    MTR2_KWH4_ex.Text = op[0].L51_Exp_TOD4_KWH_Units;
                    xrLabel12.Text= totalA2.ToString("0.00");



                }
                #endregion
                #region Meter(KVA)
                if (!string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "1")
                {


                    xrLabel17.Text = op[0].L22_TOD_1_KWH;
                    xrLabel24.Text = op[0].L22_TOD_2_KWH;
                    xrLabel45.Text = op[0].L22_TOD_3_KWH;
                    xrLabel31.Text = op[0].L22_TOD_4_KWH;
                    xrLabelab.Text = total.ToString("0.00");

                    xrLabel38.Text = op[0].L35_Exp_TOD1_KVAH_Units;
                    xrLabel52.Text = op[0].L35_Exp_TOD2_KVAH_Units;
                    xrLabel59.Text = op[0].L35_Exp_TOD3_KVAH_Units;
                    xrLabel66.Text = op[0].L35_Exp_TOD4_KVAH_Units;
                    xrLabelcd.Text = total3.ToString("0.00");


                    xrLabel39.Text = op[0].L42_Exp_CURRENT_NET_EXPORT_TOD1_KVAH_UNITS;
                    xrLabel53.Text = op[0].L42_Exp_CURRENT_NET_EXPORT_TOD2_KVAH_UNITS;
                    xrLabel60.Text = op[0].L42_Exp_CURRENT_NET_EXPORT_TOD3_KVAH_UNITS;
                    xrLabel67.Text = op[0].L42_Exp_CURRENT_NET_EXPORT_TOD4_KVAH_UNITS;
                    xrLabel16.Text = totalI2.ToString("0.00");


                    xrLabel40.Text = op[0].L42_Previous_CREDIT_Units_TOD1_KVAH;
                    xrLabel54.Text = op[0].L42_Previous_CREDIT_Units_TOD2_KVAH;
                    xrLabel61.Text = op[0].L42_Previous_CREDIT_Units_TOD3_KVAH;
                    xrLabel68.Text = op[0].L42_Previous_CREDIT_Units_TOD4_KVAH;
                    xrLabel25.Text = totalJ2.ToString("0.00");

                    xrLabel41.Text = op[0].L46_Net_Billed_Units_MAIN_TOD1_KVAH;
                    xrLabel55.Text = op[0].L46_Net_Billed_Units_MAIN_TOD2_KVAH;
                    xrLabel62.Text = op[0].L46_Net_Billed_Units_MAIN_TOD3_KVAH;
                    xrLabel69.Text = op[0].L46_Net_Billed_Units_MAIN_TOD4_KVAH;
                    xrLabel23.Text = totalK2.ToString("0.00");

                    xrLabel42.Text = op[0].L44_Carry_Forward_Units_TOD1_KVAH;
                    xrLabel56.Text = op[0].L44_Carry_Forward_Units_TOD2_KVAH;
                    xrLabel63.Text = op[0].L44_Carry_Forward_Units_TOD3_KVAH;
                    xrLabel70.Text = op[0].L44_Carry_Forward_Units_TOD4_KVAH;
                    xrLabel20.Text = totall2.ToString("0.00");

                    MTR2_KWH1.Text = op[0].L24_TOD_1_KWH;
                    MTR2_KWH2.Text = op[0].L24_TOD_2_KWH;
                    MTR2_KWH3.Text = op[0].L24_TOD_3_KWH;
                    MTR2_KWH4.Text = op[0].L24_TOD_4_KWH;
                    xrLabel11.Text = totalA1.ToString("0.00");
                    MTR2_KWH1_ex.Text = op[0].L50_Exp_TOD1_KVAH_Units;
                    MTR2_KWH2_ex.Text = op[0].L50_Exp_TOD2_KVAH_Units;
                    MTR2_KWH3_ex.Text = op[0].L50_Exp_TOD3_KVAH_Units;
                    MTR2_KWH4_ex.Text = op[0].L50_Exp_TOD4_KVAH_Units;
                    xrLabel12.Text = totalA3.ToString("0.00");


                }
                #endregion
            }
            else
            {
                MTR2_TOD1.Visible = false;
                MTR2_TOD2.Visible = false;
                MTR2_TOD3.Visible = false;
                MTR2_TOD4.Visible = false;
                xrLabelef.Visible = false;
                xrLabel11.Visible = false;
                xrLabel12.Visible = false;

                if (string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "0.00")
                {
                    MTR_TOD1.Text = op[0].L12_MTRSNO_METER1;
                    //MTR1
                    xrLabel17.Text = op[0].L22_TOD_1_KWH;
                    xrLabel24.Text = op[0].L22_TOD_2_KWH;
                    xrLabel45.Text = op[0].L22_TOD_3_KWH;
                    xrLabel31.Text = op[0].L22_TOD_4_KWH;
                    xrLabelab.Text = total.ToString("0.00");

                    xrLabel38.Text = op[0].L34_Exp_TOD1_KWH_Units;
                    xrLabel52.Text = op[0].L34_Exp_TOD2_KWH_Units;
                    xrLabel59.Text = op[0].L34_Exp_TOD3_KWH_Units;
                    xrLabel66.Text = op[0].L34_Exp_TOD4_KWH_Units;
                    xrLabelcd.Text = total2.ToString("0.00");

                    xrLabel39.Text = op[0].L43_Exp_CURRENT_NET_EXPORT_TOD1_KWH_UNITS;
                    xrLabel40.Text = op[0].L43_Previous_CREDIT_Units_TOD1_KWH;
                    xrLabel41.Text = op[0].L46_Net_Billed_Units_MAIN_TOD1_KWH;
                    xrLabel42.Text = op[0].L45_Carry_Forward_Units_TOD1_KWH;
                    xrLabel16.Text = totalI1.ToString("0.00");
                    xrLabel25.Text = totalJ1.ToString("0.00");
                    xrLabel23.Text = totalK1.ToString("0.00");
                    xrLabel20.Text = totalL1.ToString("0.00");
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

                    xrLabel17.Text = op[0].L22_TOD_1_KWH;
                    xrLabel24.Text = op[0].L22_TOD_2_KWH;
                    xrLabel45.Text = op[0].L22_TOD_3_KWH;
                    xrLabel31.Text = op[0].L22_TOD_4_KWH;
                    xrLabelab.Text = total.ToString("0.00");

                    xrLabel38.Text = op[0].L35_Exp_TOD1_KVAH_Units;
                    xrLabel52.Text = op[0].L35_Exp_TOD2_KVAH_Units;
                    xrLabel59.Text = op[0].L35_Exp_TOD3_KVAH_Units;
                    xrLabel66.Text = op[0].L35_Exp_TOD4_KVAH_Units;
                    xrLabelcd.Text = total3.ToString("0.00");

                    xrLabel39.Text = op[0].L42_Exp_CURRENT_NET_EXPORT_TOD1_KVAH_UNITS;
                    xrLabel53.Text = op[0].L42_Exp_CURRENT_NET_EXPORT_TOD2_KVAH_UNITS;
                    xrLabel60.Text = op[0].L42_Exp_CURRENT_NET_EXPORT_TOD3_KVAH_UNITS;
                    xrLabel67.Text = op[0].L42_Exp_CURRENT_NET_EXPORT_TOD4_KVAH_UNITS;
                    xrLabel16.Text = totalI2.ToString("0.00");

                    xrLabel40.Text = op[0].L42_Previous_CREDIT_Units_TOD1_KVAH;
                    xrLabel54.Text = op[0].L42_Previous_CREDIT_Units_TOD2_KVAH;
                    xrLabel61.Text = op[0].L42_Previous_CREDIT_Units_TOD3_KVAH;
                    xrLabel68.Text = op[0].L42_Previous_CREDIT_Units_TOD4_KVAH;
                    xrLabel25.Text = totalJ2.ToString("0.00");
                    xrLabel41.Text = op[0].L46_Net_Billed_Units_MAIN_TOD1_KVAH;
                    xrLabel55.Text = op[0].L46_Net_Billed_Units_MAIN_TOD2_KVAH;
                    xrLabel62.Text = op[0].L46_Net_Billed_Units_MAIN_TOD3_KVAH;
                    xrLabel69.Text = op[0].L46_Net_Billed_Units_MAIN_TOD4_KVAH;
                    xrLabel23.Text = totalK2.ToString("0.00");


                    xrLabel42.Text = op[0].L44_Carry_Forward_Units_TOD1_KVAH;
                    xrLabel56.Text = op[0].L44_Carry_Forward_Units_TOD2_KVAH;
                    xrLabel63.Text = op[0].L44_Carry_Forward_Units_TOD3_KVAH;
                    xrLabel70.Text = op[0].L44_Carry_Forward_Units_TOD4_KVAH;
                    xrLabel20.Text = totall2.ToString("0.00");
                }

            }

            #region Meter Print2
            if (op[0].L37_Gen_Meter_Serial_Number != "")
            {
                if (string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "0.00")
                {
                    xrLabel78.Text = op[0].L37_Gen_Meter_Serial_Number;
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
            #endregion


            #region Solar Export Energy Adjustment
            //Solar Export Energy Adjustment

            if (!(op[0].L8_Solar_Export_Energy == "0.00" || op[0].L8_Solar_Export_Energy == ""))
            {
                xrLabel51.Text = "(Net Billing)"; // New Solar Req
                xrLabelTotalAmt.LocationF = new DevExpress.Utils.PointFloat(104.83F, 562F);


                xrLabel33.Visible = false;
                xrLabel34.Visible = false;
                xrLabel35.Visible = false;
                xrLabel36.Visible = false;
               // kvah21.Visible = false;
                kva11.Visible = false;
                kva12.Visible = false;
                kva13.Visible = false;
                kva14.Visible = false;
                
               visible();


            }
            else
            {
                visibleon();
            }
            #endregion

            #region Disconnection Message
            //if (op[0].L1_DisconnectionMSGPrintingIMMEDIATE == "1")
            //{
            //    xrDueDate.Text = "IMMEDIATE";
            //    bd_Bottom_BillDueDate.Text = "IMMEDIATE";
            //    xrImmediatedissconnectiondate.Text = "IMMEDIATE";
            //    xrImmediatedissconnectiondate.Visible = true;
            //}
            //else
            //{
                xrDueDate.Text = op[0].L7_due_date;
                bd_Bottom_BillDueDate.Text = op[0].L7_due_date;
                xrImmediatedissconnectiondate.Text = op[0].L10_DISCONN_DATE_date;
            // }
            #endregion


            if (!String.IsNullOrEmpty(op[0].L6_LT_Metering_Flag))
            {
                xrlBillDemand.Text = "*" + xrlBillDemand.Text;

            }
        }
        private decimal ToDecimal(string value)
        {
            decimal.TryParse(value, out decimal result);
            return result;
        }

        public void visible()
        {
            xrLabel39.Visible = false;
            xrLabel7.Visible = false;
            xrLabel8.Visible = false;
            xrLabel9.Visible = false;
            xrLabel10.Visible = false;
            xrLabel40.Visible = false;
            xrLabel41.Visible = false;
            xrLabel42.Visible = false;
            xrLabel53.Visible = false;
            xrLabel54.Visible = false;
            xrLabel55.Visible = false;
            xrLabel56.Visible = false;
            xrLabel60.Visible = false;
            xrLabel61.Visible = false;
            xrLabel62.Visible = false;
            xrLabel63.Visible = false;
            xrLabel67.Visible = false;
            xrLabel68.Visible = false;
            xrLabel69.Visible = false;
            xrLabel70.Visible = false;
            xrLine1.Visible = false;
            xrLabel14.Visible = false;
            xrLabel16.Visible = false;
            xrLabel25.Visible = false;
            xrLabel23.Visible = false;
            xrLabel20.Visible = false;

        }
        public void visibleon()
        {
            xrLabel39.Visible = true;
            xrLabel7.Visible = true;
            xrLabel8.Visible = true;
            xrLabel9.Visible = true;
            xrLabel10.Visible = true;
            xrLabel40.Visible = true;
            xrLabel41.Visible = true;
            xrLabel42.Visible = true;
            xrLabel53.Visible = true;
            xrLabel54.Visible = true;
            xrLabel55.Visible = true;
            xrLabel56.Visible = true;
            xrLabel60.Visible = true;
            xrLabel61.Visible = true;
            xrLabel62.Visible = true;
            xrLabel63.Visible = true;
            xrLabel67.Visible = true;
            xrLabel68.Visible = true;
            xrLabel69.Visible = true;
            xrLabel70.Visible = true;
            xrLine1.Visible = true;
            xrLabel14.Visible = true;
            xrLabel16.Visible = true;
            xrLabel25.Visible = true;
            xrLabel23.Visible = true;
            xrLabel20.Visible = true;

        }


    }

}


