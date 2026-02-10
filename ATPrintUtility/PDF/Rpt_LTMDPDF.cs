using AT.Print.Utils;
using DevExpress.Drawing;
using DevExpress.XtraCharts;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using ZXing;
using ZXing.QrCode.Internal;

namespace AT.Print
{
    public partial class Rpt_LTMDPDF : DevExpress.XtraReports.UI.XtraReport
    {
        public Rpt_LTMDPDF()
        {
            InitializeComponent();


        }


        private void Rpt_LTMDPDF_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {



            var data = sender as Rpt_LTMDPDF;
            var op = data.DataSource as List<SingleLTMDBill>;

           
            #region QRCODE

            if (ConfigurationManager.AppSettings["generateQRCodeinLTMDBills"].ToString() == "True")
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
                //xrQRCODE.SizeF = new System.Drawing.SizeF(55, 55);
            }

            #endregion

            #region Disconnection Message
            if (op[0].L1_DisconnectionMSGPrintingIMMEDIATE == "1")
            {
                xrDueDate.Text = "IMMEDIATE";
                bd_Bottom_BillDueDate.Text = "IMMEDIATE";
                xrImmediatedissconnectiondate.Text = "IMMEDIATE";
                // xrImmediatelbl.Visible = true;
                //xrLabel20.Visible = true;
                xrImmediatedissconnectiondate.Visible = true;
                // xrLabel40.Text = "IMMEDIATE";
            }
            else
            {
                xrDueDate.Text = op[0].L7_Due_Date;
                bd_Bottom_BillDueDate.Text = op[0].L7_Due_Date;
                xrImmediatedissconnectiondate.Text = op[0].L10_DisconnDate;
                //xrDueDate.TextAlignment = TextAlignment.MiddleLeft;
                //xrImmediatedissconnectiondate.TextAlignment = TextAlignment.MiddleRight;
                //xrLabel40.Text = op[0].L7_Due_Date;
            }
            #endregion

            if (!string.IsNullOrEmpty(op[0].L1_Customer_PAN))
            {
                // xrLabel31.Visible = true;
                xrLabel23.Visible = true;
            }
            else
            {
                // xrLabel31.Visible = false;
                xrLabel23.Visible = false;
            }


            xrLabel23.Text = op[0].L1_Customer_PAN;
            // To keep Address and PAN together            
            if (op[0].L2_Name.ToString() == "")
            {
                xrLabel139.Visible = false;
                xrLabel140.TopF = xrLabel139.TopF;
            }
            if (op[0].L3_Addr1.ToString() == "")
            {
                xrLabel140.Visible = false;
                xrLabel141.TopF = xrLabel140.TopF;
            }
            if (op[0].L4_Addr2.ToString() == "")
            {
                xrLabel141.Visible = false;
                xrLabel142.TopF = xrLabel141.TopF;
            }
            if (op[0].L5_Addr3.ToString() == "")
            {
                xrLabel142.Visible = false;
                xrLabel31.TopF = xrLabel142.TopF;
                xrLabel40.TopF = xrLabel31.BottomF;
                xrLabel23.TopF = xrLabel40.TopF;
            }


            string unit = "KW";
            if (!string.IsNullOrEmpty(op[0].L6_Kvah_Indicator) && op[0].L6_Kvah_Indicator == "1")
            {
                op[0].unit = op[0].L6_MeasureContractDemand;
                unit = op[0].L6_MeasureContractDemand;
            }
            else
            {
                op[0].unit = "KW";

                if (Decimal.TryParse(op[0].L6_SERVDET_SANC_LOAD, out decimal contractdemand))
                {

                    if (op[0].L6_MeasureContractDemand.ToUpper() == "HP")
                    {
                        if ((contractdemand * (decimal)0.746) >= (decimal)10)
                        {
                            if (Convert.ToDecimal(op[0].L6_Avg_Power_Factor) >= (decimal)95)
                            {
                                op[0].L6_Avg_Power_Factor = op[0].L6_Avg_Power_Factor + "(Lead)";
                            }
                        }
                    }
                    if (op[0].L6_MeasureContractDemand.ToUpper() == "KW")
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




            if (op[0].L6_MeasureContractDemand == "HP")
            {

                if (op[0].L6_Kvah_Indicator == "1")
                {
                    string unit1 = "KVA";

                    op[0].L6_ACTUAL_DEMAND = op[0].L6_ACTUAL_DEMAND + "(" + unit1 + ")";
                    op[0].L6_EXCESS_DEMAND = op[0].L6_EXCESS_DEMAND + "(" + unit1 + ")";
                    op[0].L6_bill_demand = op[0].L6_bill_demand + "(" + unit1 + ")";

                    met1_headingMDKW.Text = "MD" + unit1;
                    met1_headingMDKW_1.Text = unit1 + "H";
                    met2_headingMDKW.Text = "MD" + unit1;
                    met2_headingMDKW_2.Text = unit1 + "H";
                }
                else
                {
                    string unit1 = "KW";

                    op[0].L6_ACTUAL_DEMAND = op[0].L6_ACTUAL_DEMAND + "(" + unit1 + ")";
                    op[0].L6_EXCESS_DEMAND = op[0].L6_EXCESS_DEMAND + "(" + unit1 + ")";
                    op[0].L6_bill_demand = op[0].L6_bill_demand + "(" + unit1 + ")";

                    met1_headingMDKW.Text = "MD" + unit1;
                    met1_headingMDKW_1.Text = unit1 + "H";
                    met2_headingMDKW.Text = "MD" + unit1;
                    met2_headingMDKW_2.Text = unit1 + "H";
                }
            }
            else if (op[0].L6_MeasureContractDemand == "KW")
            {
                if (!string.IsNullOrEmpty(op[0].L6_Kvah_Indicator) && op[0].L6_Kvah_Indicator == "1")
                {
                    string unit1 = "KVA";

                    op[0].L6_ACTUAL_DEMAND = op[0].L6_ACTUAL_DEMAND + "(" + unit1 + ")";
                    op[0].L6_EXCESS_DEMAND = op[0].L6_EXCESS_DEMAND + "(" + unit1 + ")";
                    op[0].L6_bill_demand = op[0].L6_bill_demand + "(" + unit1 + ")";


                    met1_headingMDKW.Text = "MD" + unit1;
                    met1_headingMDKW_1.Text = unit1 + "H";
                    met2_headingMDKW.Text = "MD" + unit1;
                    met2_headingMDKW_2.Text = unit1 + "H";
                }
                else
                {
                    string unit1 = "KW";

                    op[0].L6_ACTUAL_DEMAND = op[0].L6_ACTUAL_DEMAND + "(" + unit1 + ")";
                    op[0].L6_EXCESS_DEMAND = op[0].L6_EXCESS_DEMAND + "(" + unit1 + ")";
                    op[0].L6_bill_demand = op[0].L6_bill_demand + "(" + unit1 + ")";


                    met1_headingMDKW.Text = "MD" + unit1;
                    met1_headingMDKW_1.Text = unit1 + "H";
                    met2_headingMDKW.Text = "MD" + unit1;
                    met2_headingMDKW_2.Text = unit1 + "H";
                }
            }
            else if (op[0].L6_MeasureContractDemand == "KVA")
            {
                if (op[0].L6_Kvah_Indicator == "1")
                {
                    string unit1 = "KVA";

                    op[0].L6_ACTUAL_DEMAND = op[0].L6_ACTUAL_DEMAND + "(" + unit1 + ")";
                    op[0].L6_EXCESS_DEMAND = op[0].L6_EXCESS_DEMAND + "(" + unit1 + ")";
                    op[0].L6_bill_demand = op[0].L6_bill_demand + "(" + unit1 + ")";


                    met1_headingMDKW.Text = "MD" + unit1;
                    met1_headingMDKW_1.Text = unit1 + "H";
                    met2_headingMDKW.Text = "MD" + unit1;
                    met2_headingMDKW_2.Text = unit1 + "H";
                }
                else
                {
                    string unit1 = "KW";

                    op[0].L6_ACTUAL_DEMAND = op[0].L6_ACTUAL_DEMAND + "(" + unit1 + ")";
                    op[0].L6_EXCESS_DEMAND = op[0].L6_EXCESS_DEMAND + "(" + unit1 + ")";
                    op[0].L6_bill_demand = op[0].L6_bill_demand + "(" + unit1 + ")";


                    met1_headingMDKW.Text = "MD" + unit1;
                    met1_headingMDKW_1.Text = unit1 + "H";
                    met2_headingMDKW.Text = "MD" + unit1;
                    met2_headingMDKW_2.Text = unit1 + "H";
                }
            }
            #region Excess Demand Print
            //Excess Demand Print
            if (op[0].L6_EXCESS_DEMAND.Substring(0, 4) != "0.00")
            {
                xrlL6ExcessDemand.Text = op[0].L6_EXCESS_DEMAND;
            }
            #endregion

            #region Meter Change
            if (op[0].L12_MTRSNO_METER_2_IF_AVAILABLE != "")
            {
                met1_headingMDKW.Visible = true;
                met1_headingMDKW_1.Visible = true;
                //Old Meter Setting
                xrLabel5.Text = op[0].L12_MTRSNO_METER_2_IF_AVAILABLE;//older
                xrLabel19.Text = op[0].L12_MTRSNO_METER1;//Newer
                                                         //Meter Old
                met1_11.Text = "____";
                met1_12.Text = op[0].L14_M1_KWH_PASTREAD;
                met1_21.Text = "____";
                met1_22.Text = op[0].L13_M1_KWH_PRESREAD;
                met1_31.Text = op[0].L15_M1_MultiplyingFactor_2;
                met1_32.Text = op[0].L15_M1_MultiplyingFactor_1;
                met1_41.Text = op[0].L16_M1_KVA_UNITS;
                met1_42.Text = op[0].L16_M1_KWH_UNITS;

                //Meter New

                met2_11.Text = "____";
                met2_12.Text = op[0].L18_M2_KWH_PASTREAD;
                met2_21.Text = "____";
                met2_22.Text = op[0].L17_M2_KWH_PRESREAD;
                met2_31.Text = op[0].L19_M2_Multiplying_Factor_2;
                met2_32.Text = op[0].L19_M2_Multiplying_Factor_1;
                met2_41.Text = op[0].L20_M2_KVA_UNITS;
                met2_42.Text = op[0].L20_M2_KWH_UNITS;



            }
            else
            {
                met1_headingMDKW.Visible = false;
                met1_headingMDKW_1.Visible = false;
                xrLabel5.Text = op[0].L12_MTRSNO_METER1;//Newer
                                                        //Meter Old
                met2_11.Text = "____";
                met2_12.Text = op[0].L14_M1_KWH_PASTREAD;
                met2_21.Text = "____";
                met2_22.Text = op[0].L13_M1_KWH_PRESREAD;
                met2_31.Text = op[0].L15_M1_MultiplyingFactor_2;
                met2_32.Text = op[0].L15_M1_MultiplyingFactor_1;
                met2_41.Text = op[0].L16_M1_KVA_UNITS;
                met2_42.Text = op[0].L16_M1_KWH_UNITS;
            }
            #endregion

            if (!string.IsNullOrEmpty(op[0].L6_Kvah_Indicator) && op[0].L6_Kvah_Indicator == "1")
            {

            }
            else
            {
                if (Decimal.TryParse(op[0].L6_SERVDET_SANC_LOAD, out decimal contractdemand))
                {
                    if (op[0].L6_MeasureContractDemand.ToUpper() == "KW" && Math.Ceiling(contractdemand) >= (decimal)10.0)
                    {
                        op[0].L6_MeasureContractDemand = op[0].L6_MeasureContractDemand + "/KVA";
                    }
                    else if (op[0].L6_MeasureContractDemand.ToUpper() == "HP" && Math.Ceiling(contractdemand) >= (decimal)13.4)
                    {
                        op[0].L6_MeasureContractDemand = op[0].L6_MeasureContractDemand + "/KVA";
                    }
                }
            }

            op[0].L6_SERVDET_SANC_LOAD = op[0].L6_SERVDET_SANC_LOAD + "(" + op[0].L6_MeasureContractDemand + ")";
            xrLblAmount.Text = "₹" + ToDecimal(op[0].L8_AmountPayableBeforeDueDate).ToString("G");
            xrLabelTotalAmt.BringToFront();

            xrLabelTotalAmt.Text = "₹" + ToDecimal(op[0].L8_AmountPayableBeforeDueDate).ToString("G");
            xrLabel13.BringToFront();
            xrLabel5.BringToFront();
            xrLabel19.BringToFront();

            #region pieChart
            //PieChart//
            decimal energyCharge = ToDecimal(op[0].L8_EnergyCharge);
            decimal fixedCharge = ToDecimal(op[0].L8_FixedCharge);
            decimal electricityDuty = ToDecimal(op[0].L8_GovTax);
            decimal excessDemandCharge = ToDecimal(op[0].L10_DmdChgPenalty);

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

            xrChart1.Series[0].DataSource = op[0].KWHgrph;
            xrChart1.Series[0].ArgumentScaleType = ScaleType.Qualitative;
            xrChart1.Series[0].ArgumentDataMember = "MonthYear";
            xrChart1.Series[0].Label.TextOrientation = TextOrientation.BottomToTop;
            xrChart1.Series[0].ValueScaleType = ScaleType.Numerical;
            xrChart1.Series[0].ValueDataMembers.AddRange(new string[] { "Value" });
            xrChart2.Series[0].DataSource = op[0].KVAgrph;
            xrChart2.Series[0].ArgumentScaleType = ScaleType.Qualitative;
            xrChart2.Series[0].ArgumentDataMember = "MonthYear";
            xrChart2.Series[0].Label.TextOrientation = TextOrientation.BottomToTop;
            xrChart2.Series[0].ValueScaleType = ScaleType.Numerical;
            xrChart2.Series[0].ValueDataMembers.AddRange(new string[] { "Value" });
            xrChart2.WidthF = xrChart1.WidthF;

            xrLabel14.Text = "Thank you for your previous payment of ₹" + op[0].L7_LastPayementAmount + " on " + op[0].L7_LastPymtDate;

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

    }
}





