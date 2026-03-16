using AT.Print.Utils;
using DevExpress.Drawing;
using DevExpress.XtraCharts;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using ZXing;

namespace AT.Print.PDF
{
    public partial class Rpt_HT_TodPDF : DevExpress.XtraReports.UI.XtraReport
    {
        public Rpt_HT_TodPDF()
        {
            InitializeComponent();
        }

        private void Rpt_HT_TodPDF_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var data = sender as Rpt_HT_TodPDF;
            var op = data.DataSource as List<SingleHTBill>;


           
            #region QRCODE

            if (ConfigurationManager.AppSettings["generateQRCodeinSolarHTTODBills"].ToString() == "True")
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
            }
            #endregion


            #region Disconnection Message
            if (op[0].L1_DisconnectionMSGPrintingIMMEDIATE == "1")
            {
                xrDueDate.Text = "IMMEDIATE";
                bd_Bottom_BillDueDate.Text = "IMMEDIATE";
                xrImmediatedissconnectiondate.Text = "IMMEDIATE";
                xrImmediatedissconnectiondate.Visible = true;
            }
            else
            {
                xrDueDate.Text = op[0].L7_Due_Date;
                bd_Bottom_BillDueDate.Text = op[0].L7_Due_Date;
                xrImmediatedissconnectiondate.Text = op[0].L10_DisconnDate;
            }
            #endregion


            if (!string.IsNullOrEmpty(op[0].L1_Customer_PAN))
            {
                xrLabel23.Visible = true;
            }
            else
            {
                xrLabel23.Visible = false;
            }
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
                xrLabel23.TopF = xrLabel40.BottomF;
            }
            else
            {
                xrLabel31.TopF = xrLabel142.BottomF;
                xrLabel40.TopF = xrLabel31.BottomF;
                xrLabel23.TopF = xrLabel40.BottomF;
            }

            #region Excess Demand Print
            //Excess Demand Print
            if (op[0].L6_EXCESS_DEMAND == "0.00" || op[0].L6_EXCESS_DEMAND == " " || op[0].L6_EXCESS_DEMAND == "0")
            {
                xrlL6ExcessDemand.Visible = false;
            }
            #endregion











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


            #region Meter Change

            if (op[0].L1_TODOrNon_TODFlag == "0")
            {
                #region NON TOD METER CHANGE
                //Meter Change Print
                if (op[0].L11_MTRSNO_2_IF_AVAILABLE != "")
                {

                    met1_headingMDKW.Visible = true;
                    met1_headingMDKW_1.Visible = true;
                //  met2_headingMDKW.Visible = true;
                 // met2_headingMDKW_2.Visible = true;
                    //Old Meter Setting
                    xrLabel5.Text = op[0].L11_MTRSNO_2_IF_AVAILABLE;//older
                    xrLabel19.Text = op[0].L11_MTRSNO_1;//Newer

                    if (string.IsNullOrEmpty(op[0].L6_Kvah_Indicator))
                    {

                        ////Meter Old
                        met1_11.Text = "____";
                        met1_12.Text = op[0].L13_KWH_PASTREAD;
                        met1_21.Text = "____";
                        met1_22.Text = op[0].L12_KWH_PRESREAD;
                        met1_31.Text = op[0].L14_Multiplying_factor_KVA;
                        met1_32.Text = op[0].L14_Multiplying_factor_KVAH;
                        met1_41.Text = op[0].L15_KVA_UNITS;
                        met1_42.Text = op[0].L15_KWH_UNITS;

                        //Meter New

                        met2_11.Text = "____";
                        met2_12.Text = op[0].L19_KWH_PASTREAD;
                        met2_21.Text = "____";
                        met2_22.Text = op[0].L18_KWH_PRESREAD;
                        met2_31.Text = op[0].L20_Multiplying_Factor_KVA;
                        met2_32.Text = op[0].L20_Multiplying_Factor_KVAH;
                        met2_41.Text = op[0].L21_KVA_UNITS;
                        met2_42.Text = op[0].L21_KWH_UNITS;
                    }
                    else
                    {
                        ////Meter Old
                        met1_11.Text = "____";
                        met1_12.Text = op[0].L13_KVAH_PASTREAD;
                        met1_21.Text = "____";
                        met1_22.Text = op[0].L12_KVAH_PRESREAD;
                        met1_31.Text = op[0].L14_Multiplying_factor_KVA;
                        met1_32.Text = op[0].L14_Multiplying_factor_KVAH;
                        met1_41.Text = op[0].L15_KVA_UNITS;
                        met1_42.Text = op[0].L15_KVAH_UNITS;
                        //Meter New 
                        met2_11.Text = "____";
                        met2_12.Text = op[0].L19_KVAH_PASTREAD;
                        met2_21.Text = "____";
                        met2_22.Text = op[0].L18_KVAH_PRESREAD;
                        met2_31.Text = op[0].L20_Multiplying_Factor_KVA;
                        met2_32.Text = op[0].L20_Multiplying_Factor_KVAH;
                        met2_41.Text = op[0].L21_KVA_UNITS;
                        met2_42.Text = op[0].L21_KVAH_UNITS;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(op[0].L6_Kvah_Indicator))
                    {
                        met1_headingMDKW.Visible = false;
                        met1_headingMDKW_1.Visible = false;
                        xrLabel5.Text = op[0].L11_MTRSNO_1;
                        met2_11.Text = "____";
                        met2_12.Text = op[0].L13_KWH_PASTREAD;
                        met2_21.Text = "____";
                        met2_22.Text = op[0].L12_KWH_PRESREAD;
                        met2_31.Text = op[0].L14_Multiplying_factor_KVA;
                        met2_32.Text = op[0].L14_Multiplying_factor_KVAH;
                        met2_41.Text = op[0].L15_KVA_UNITS;
                        met2_42.Text = op[0].L15_KWH_UNITS;
                    }
                    else
                    {
                        met1_headingMDKW.Visible = false;
                        met1_headingMDKW_1.Visible = false;
                        xrLabel5.Text = op[0].L11_MTRSNO_1;
                        met2_11.Text = "____";
                        met2_12.Text = op[0].L13_KVAH_PASTREAD;
                        met2_21.Text = "____";
                        met2_22.Text = op[0].L12_KVAH_PRESREAD;
                        met2_31.Text = op[0].L14_Multiplying_factor_KVA;
                        met2_32.Text = op[0].L14_Multiplying_factor_KVAH;
                        met2_41.Text = op[0].L15_KVA_UNITS;
                        met2_42.Text = op[0].L15_KVAH_UNITS;
                    }
                }
                #endregion
            }
            else
            {
                #region TOD METER CHANGE
                xrLabel35.Visible = true;
                xrLabel36.Visible = true;
                xrLabel44.Visible = true;
                xrLabel45.Visible = true;
                xrLabel46.Visible = true;
                MeterSerial1.Visible = true;
                MeterSerial2.Visible = true;

                if (op[0].L11_MTRSNO_2_IF_AVAILABLE != "")
                {

                    met1_headingMDKW.Visible = true;
                    met1_headingMDKW_1.Visible = true;
                    xrLabel5.Text = op[0].L11_MTRSNO_2_IF_AVAILABLE;//older
                    xrLabel19.Text = op[0].L11_MTRSNO_1;//Newer


                    #region Meter 1 (below)

                    MeterSerial2.Text = op[0].L11_MTRSNO_1;
                    met1_11.Text = op[0].L13_KVA_PASTREAD == "0.00" ? "0.00" : op[0].L13_KVA_PASTREAD == "" ? "" : op[0].L13_KVA_PASTREAD;
                    met1_21.Text = op[0].L12_KVA_PRESREAD == "0.00" ? "0.00" : op[0].L12_KVA_PRESREAD == "" ? "" : op[0].L12_KVA_PRESREAD;
                    met1_31.Text = op[0].L14_Multiplying_factor_KVA == "0.00" ? "0.00" : op[0].L14_Multiplying_factor_KVA == "" ? "" : op[0].L14_Multiplying_factor_KVA;
                    met1_41.Text = op[0].L15_KVA_UNITS == "0.00" ? "0.00" : op[0].L15_KVA_UNITS == "" ? "" : op[0].L15_KVA_UNITS;

                    Tod1Label2.Text = op[0].L16_TOD1_KVAH_Units;
                    Tod2Label2.Text = op[0].L16_TOD2_KVAH_Units;
                    Tod3Label2.Text = op[0].L16_TOD3_KVAH_Units;
                    Tod4Label2.Text = op[0].L16_TOD4_KVAH_Units;

                    if (!string.IsNullOrEmpty(op[0].L6_Kvah_Indicator) && op[0].L6_Kvah_Indicator == "1")
                    {

                        met1_12.Text = op[0].L13_KVAH_PASTREAD == "0.00" ? "0.00" : op[0].L13_KVAH_PASTREAD == "" ? "" : op[0].L13_KVAH_PASTREAD;
                        met1_22.Text = op[0].L12_KVAH_PRESREAD == "0.00" ? "0.00" : op[0].L12_KVAH_PRESREAD == "" ? "" : op[0].L12_KVAH_PRESREAD;
                        met1_32.Text = op[0].L14_Multiplying_factor_KVAH == "0.00" ? "0.00" : op[0].L14_Multiplying_factor_KVAH == "" ? "" : op[0].L14_Multiplying_factor_KVAH;
                        met1_42.Text = op[0].L15_KVAH_UNITS == "0.00" ? "0.00" : op[0].L15_KVAH_UNITS == "" ? "" : op[0].L15_KVAH_UNITS;
                    }
                    else
                    {
                        met1_12.Text = op[0].L13_KWH_PASTREAD == "0.00" ? "0.00" : op[0].L13_KWH_PASTREAD == "" ? "" : op[0].L13_KWH_PASTREAD;
                        met1_22.Text = op[0].L12_KWH_PRESREAD == "0.00" ? "0.00" : op[0].L12_KWH_PRESREAD == "" ? "" : op[0].L12_KWH_PRESREAD;
                        met1_32.Text = op[0].L14_Multiplying_factor_KWH == "0.00" ? "0.00" : op[0].L14_Multiplying_factor_KWH == "" ? "" : op[0].L14_Multiplying_factor_KWH;
                        met1_42.Text = op[0].L15_KWH_UNITS == "0.00" ? "0.00" : op[0].L15_KWH_UNITS == "" ? "" : op[0].L15_KWH_UNITS;

                    }



                    #endregion

                    #region Meter 2 (above)

                    MeterSerial1.Text = op[0].L11_MTRSNO_2_IF_AVAILABLE;
                    met2_11.Text = op[0].L19_KVA_PASTREAD == "0.00" ? "0.00" : op[0].L19_KVA_PASTREAD == "" ? "" : op[0].L19_KVA_PASTREAD;
                    met2_21.Text = op[0].L18_KVA_PRESREAD == "0.00" ? "0.00" : op[0].L18_KVA_PRESREAD == "" ? "" : op[0].L18_KVA_PRESREAD;
                    met2_31.Text = op[0].L20_Multiplying_Factor_KVA == "0.00" ? "0.00" : op[0].L20_Multiplying_Factor_KVA == "" ? "" : op[0].L20_Multiplying_Factor_KVA;
                    met2_41.Text = op[0].L21_KVA_UNITS == "0.00" ? "0.00" : op[0].L21_KVA_UNITS == "" ? "" : op[0].L21_KVA_UNITS;

                    Tod1Label.Text = op[0].L22_TOD1_KVAH_Units;
                    Tod2Label.Text = op[0].L22_TOD2_KVAH_Units;
                    Tod3Label.Text = op[0].L22_TOD3_KVAH_Units;
                    Tod4Label.Text = op[0].L22_TOD4_KVAH_Units;



                    if (!string.IsNullOrEmpty(op[0].L6_Kvah_Indicator) && op[0].L6_Kvah_Indicator == "1")
                    {

                        met2_12.Text = op[0].L19_KVAH_PASTREAD == "0.00" ? "0.00" : op[0].L19_KVAH_PASTREAD == "" ? "" : op[0].L19_KVAH_PASTREAD;
                        met2_22.Text = op[0].L18_KVAH_PRESREAD == "0.00" ? "0.00" : op[0].L18_KVAH_PRESREAD == "" ? "" : op[0].L18_KVAH_PRESREAD;
                        met2_32.Text = op[0].L20_Multiplying_Factor_KVAH == "0.00" ? "0.00" : op[0].L20_Multiplying_Factor_KVAH == "" ? "" : op[0].L20_Multiplying_Factor_KVAH;
                        met2_42.Text = op[0].L21_KVAH_UNITS == "0.00" ? "0.00" : op[0].L21_KVAH_UNITS == "" ? "" : op[0].L21_KVAH_UNITS;
                    }
                    else
                    {
                        met2_12.Text = op[0].L19_KWH_PASTREAD == "0.00" ? "0.00" : op[0].L19_KWH_PASTREAD == "" ? "" : op[0].L19_KWH_PASTREAD;
                        met2_22.Text = op[0].L18_KWH_PRESREAD == "0.00" ? "0.00" : op[0].L18_KWH_PRESREAD == "" ? "" : op[0].L18_KWH_PRESREAD;
                        met2_32.Text = op[0].L20_Multiplying_Factor_KWH == "0.00" ? "0.00" : op[0].L20_Multiplying_Factor_KWH == "" ? "" : op[0].L20_Multiplying_Factor_KWH;
                        met2_42.Text = op[0].L21_KWH_UNITS == "0.00" ? "0.00" : op[0].L21_KWH_UNITS == "" ? "" : op[0].L21_KWH_UNITS;
                    }
                    #endregion

                }
                else
                {
                    //  MeterXRPanel2.Visible = false;
                    //DebitNoteLabel.TopF = MeterDetailTotLabel.BottomF;
                    // MessagesPanel.TopF = DebitNoteLabel.BottomF;

                    MeterSerial1.Text = op[0].L11_MTRSNO_1;
                    xrLabel5.Text = op[0].L11_MTRSNO_1;
                    met2_11.Text = op[0].L13_KVA_PASTREAD == "0.00" ? "0.00" : op[0].L13_KVA_PASTREAD == "" ? "" : op[0].L13_KVA_PASTREAD;
                    met2_21.Text = op[0].L12_KVA_PRESREAD == "0.00" ? "0.00" : op[0].L12_KVA_PRESREAD == "" ? "" : op[0].L12_KVA_PRESREAD;
                    met2_31.Text = op[0].L14_Multiplying_factor_KVA == "0.00" ? "0.00" : op[0].L14_Multiplying_factor_KVA == "" ? "" : op[0].L14_Multiplying_factor_KVA;
                    met2_41.Text = op[0].L15_KVA_UNITS == "0.00" ? "0.00" : op[0].L15_KVA_UNITS == "" ? "" : op[0].L15_KVA_UNITS;

                    Tod1Label.Text = op[0].L16_TOD1_KVAH_Units;
                    Tod2Label.Text = op[0].L16_TOD2_KVAH_Units;
                    Tod3Label.Text = op[0].L16_TOD3_KVAH_Units;
                    Tod4Label.Text = op[0].L16_TOD4_KVAH_Units;

                    if (!string.IsNullOrEmpty(op[0].L6_Kvah_Indicator) && op[0].L6_Kvah_Indicator == "1")
                    {
                        met2_12.Text = op[0].L13_KVAH_PASTREAD == "0.00" ? "0.00" : op[0].L13_KVAH_PASTREAD == "" ? "" : op[0].L13_KVAH_PASTREAD;
                        met2_22.Text = op[0].L12_KVAH_PRESREAD == "0.00" ? "0.00" : op[0].L12_KVAH_PRESREAD == "" ? "" : op[0].L12_KVAH_PRESREAD;
                        met2_32.Text = op[0].L14_Multiplying_factor_KVAH == "0.00" ? "0.00" : op[0].L14_Multiplying_factor_KVAH == "" ? "" : op[0].L14_Multiplying_factor_KVAH;
                        met2_42.Text = op[0].L15_KVAH_UNITS == "0.00" ? "0.00" : op[0].L15_KVAH_UNITS == "" ? "" : op[0].L15_KVAH_UNITS;
                    }
                    else
                    {
                        met2_12.Text = op[0].L13_KWH_PASTREAD == "0.00" ? "0.00" : op[0].L13_KWH_PASTREAD == "" ? "" : op[0].L13_KWH_PASTREAD;
                        met2_22.Text = op[0].L12_KWH_PRESREAD == "0.00" ? "0.00" : op[0].L12_KWH_PRESREAD == "" ? "" : op[0].L12_KWH_PRESREAD;
                        met2_32.Text = op[0].L14_Multiplying_factor_KWH == "0.00" ? "0.00" : op[0].L14_Multiplying_factor_KWH == "" ? "" : op[0].L14_Multiplying_factor_KWH;
                        met2_42.Text = op[0].L15_KWH_UNITS == "0.00" ? "0.00" : op[0].L15_KWH_UNITS == "" ? "" : op[0].L15_KWH_UNITS;

                    }


                }
                #endregion
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

            xrChart1.Series[0].DataSource = op[0].KVAHgrph;
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

