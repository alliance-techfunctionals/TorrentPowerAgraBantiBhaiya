using AT.Print.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;

namespace AT.Print.PDF
{
    public partial class Rpt_LTPDF : DevExpress.XtraReports.UI.XtraReport
    {
        public Rpt_LTPDF()
        {
            InitializeComponent();
        }
        private void Rpt_LT_Back_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
        private void Rpt_LTPDF_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = sender as Rpt_LTPDF;
            var op = data.DataSource as List<SingleLTBill>;

            #region RISC1 Change
            if (op[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 5A") || op[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 5B") || op[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 1B") || op[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 1C"))
            {
                bd_RlSC1Rate.Text = "@ 1.14%";

            }
            #endregion

            #region QRCODE

            if (ConfigurationManager.AppSettings["generateQRCodeinLTBills"].ToString() == "True")
            {
                string qrServiceno = "AGR@" + (op[0].L6_SERVDET_SERVNO);
                byte[] bytesToEncode = Encoding.UTF8.GetBytes(qrServiceno);
                string base64Encoded = Convert.ToBase64String(bytesToEncode);
                string textToEncode = (ConfigurationManager.AppSettings["generateQRCodeURL"].ToString())+ base64Encoded;
                BarcodeWriter barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                var encodingOptions = new ZXing.Common.EncodingOptions
                {
                    Margin = 0,
                   
                };
                barcodeWriter.Options = encodingOptions;
                var qrCodeBitmap = barcodeWriter.Write(textToEncode);
                xrQRCODE.Image = qrCodeBitmap;
                xrQRCODE.SizeF = new System.Drawing.SizeF(60, 60);
            }

            #endregion



            //xrLabel86.Text = string.Format(" # {0}. ", op[0].L9_AmountPayable); 



            #region Disconnection Message
            //Disconnection Messages Print
            if (op[0].L1_DisconnectionMSGPrintingIMMEDIATE == "1")
            {
                xrDueDate.Text = "IMMEDIATE /";
                bd_Bottom_BillDueDate.Text = "IMMEDIATE";
                xrImmediatedissconnectiondate.Text = "IMMEDIATE /";
                xrImmediatelbl.Visible = true;
                xrLabel20.Visible = true;
                xrImmediatedissconnectiondate.Visible = true;
            }
            else
            {
                xrDueDate.Text = op[0].L7_Due_Date;
                bd_Bottom_BillDueDate.Text = op[0].L7_Due_Date;
                xrImmediatedissconnectiondate.Text = op[0].L10_DisconnDate;
                xrDueDate.TextAlignment = TextAlignment.MiddleLeft;
                xrImmediatedissconnectiondate.TextAlignment = TextAlignment.MiddleRight;
            }
            #endregion

            //if (Convert.ToInt32(op[0].L8_AmountPayableBeforeDueDate.Contains('-')?("-" + op[0].L8_AmountPayableBeforeDueDate.Replace('-',' ')): op[0].L8_AmountPayableBeforeDueDate) >= 200000)

            if (!string.IsNullOrEmpty(op[0].L1_Customer_PAN))
            {
                xrLabel31.Visible = true;
                xrLabel23.Visible = true;
            }
            else
            {
                xrLabel31.Visible = false;
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
                xrLabel23.TopF = xrLabel142.TopF;
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

            //op[0].L6_SERVDET_SANC_LOAD = op[0].L6_SERVDET_SANC_LOAD + "(" + op[0].L6_MeasureContractDemand + ")";
            op[0].L6_ACTUAL_DEMAND = op[0].L6_ACTUAL_DEMAND + "(" + unit + ")";
            //op[0].L6_TARIFF_DESCR = op[0].L6_TARIFF_DESCR + "(" + unit + ")";
            op[0].L6_EXCESS_DEMAND = op[0].L6_EXCESS_DEMAND + "(" + unit + ")";
            op[0].L6_bill_demand = op[0].L6_bill_demand + "(" + unit + ")";


            #region Excess Demand Print
            //Excess Demand Print
            if (op[0].L6_EXCESS_DEMAND != "0.00(KW)" || op[0].L6_EXCESS_DEMAND != "(KW)")
            {
                xrlL6ExcessDemand.Text = op[0].L6_EXCESS_DEMAND;
            }
            #endregion

            #region Meter Change
            //Meter Change Print
            if (op[0].L12_MTRSNO_METER_2_IF_AVAILABLE != "")
            {
                met1_headingMDKW.Visible = true;
                met1_headingMDKW_1.Visible = true;
                //Old Meter Setting
                xrLabel5.Text = op[0].L12_MTRSNO_METER_2_IF_AVAILABLE;//older
                xrLabel19.Text = op[0].L12_MTRSNO_METER1;//Newer
                //Meter Old
                met1_11.Text = "____"; //op[0].L14_M1_KVA_PASTREAD == "0.00" ? "____" : op[0].L14_M1_KVA_PASTREAD == "" ? "____" : op[0].L14_M1_KVA_PASTREAD;
                met1_12.Text = op[0].L14_M1_KWH_PASTREAD;
                met1_21.Text = "____";//op[0].L13_M1_KVA_PRESREAD == "0.00" ? "____" : op[0].L13_M1_KVA_PRESREAD == "" ? "____" : op[0].L13_M1_KVA_PRESREAD;
                met1_22.Text = op[0].L13_M1_KWH_PRESREAD;
                met1_31.Text = op[0].L15_M1_MultiplyingFactor_2;
                met1_32.Text = op[0].L15_M1_MultiplyingFactor_1;
                met1_41.Text = op[0].L16_M1_KVA_UNITS;
                met1_42.Text = op[0].L16_M1_KWH_UNITS;

                //Meter New

                met2_11.Text = "____";//op[0].L18_M2_KVA_PASTREAD == "0.00" ? "____" : op[0].L18_M2_KVA_PASTREAD == "" ? "____" : op[0].L18_M2_KVA_PASTREAD;
                met2_12.Text = op[0].L18_M2_KWH_PASTREAD;
                met2_21.Text = "____";//op[0].L17_M2_KVA_PRESREAD == "0.00" ? "____" : op[0].L17_M2_KVA_PRESREAD == "" ? "____" : op[0].L17_M2_KVA_PRESREAD;
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
                met2_11.Text = "____";//op[0].L14_M1_KVA_PASTREAD == "0.00" ? "____" : op[0].L14_M1_KVA_PASTREAD == "" ? "____" : op[0].L14_M1_KVA_PASTREAD;
                met2_12.Text = op[0].L14_M1_KWH_PASTREAD;
                met2_21.Text = "____";//op[0].L13_M1_KVA_PRESREAD == "0.00" ? "____" : op[0].L13_M1_KVA_PRESREAD == "" ? "____" : op[0].L13_M1_KVA_PRESREAD;
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


            #region Bill Details

            //Excess Demand Surcharge Print
            if (op[0].L10_DmdChgPenalty == "0.00" || string.IsNullOrEmpty(op[0].L10_DmdChgPenalty))
            {
                bd_ExcessDemandCharges.Visible = false;
                bd_ExcessDemandChargesHindi.Visible = false;
                bd_ExcessDemandChargesValue.Visible = false;

                bd_ExcessDemandCharges.TopF = bd_FixedCharge.TopF;
                bd_ExcessDemandChargesHindi.TopF = bd_FixedCharge.TopF;
                bd_ExcessDemandChargesValue.TopF = bd_FixedCharge.TopF;
            }


            bd_EnergyCharge.TopF = bd_ExcessDemandChargesValue.BottomF;
            bd_EnergyChargeHindi.TopF = bd_ExcessDemandChargesValue.BottomF;
            bd_EnergyChargeValues.TopF = bd_ExcessDemandChargesValue.BottomF;

            bd_AcCharge.TopF = bd_EnergyCharge.BottomF;
            bd_AcChargeHindi.TopF = bd_EnergyChargeHindi.BottomF;
            bd_AcChargeValue.TopF = bd_EnergyChargeValues.BottomF;
            //AC Charge Print

            if (op[0].L8_ACCharge == "0.00" || string.IsNullOrEmpty(op[0].L8_ACCharge))
            {
                bd_AcCharge.Visible = false;
                bd_AcChargeHindi.Visible = false;
                bd_AcChargeValue.Visible = false;

                bd_AcCharge.TopF = bd_EnergyCharge.TopF;
                bd_AcChargeHindi.TopF = bd_EnergyCharge.TopF;
                bd_AcChargeValue.TopF = bd_EnergyCharge.TopF;


            }
            bdPowerFactorCharges.TopF = bd_AcCharge.BottomF;
            bdPowerFactorHindi.TopF = bd_AcCharge.BottomF;
            bd_powerFactorValue.TopF = bd_AcCharge.BottomF;


            if (op[0].L8_PowerFactorAdj == "0.00" || string.IsNullOrEmpty(op[0].L8_PowerFactorAdj))
            {
                bdPowerFactorCharges.Visible = false;
                bdPowerFactorHindi.Visible = false;
                bd_powerFactorValue.Visible = false;

                bdPowerFactorCharges.TopF = bd_AcCharge.TopF;
                bdPowerFactorHindi.TopF = bd_AcCharge.TopF;
                bd_powerFactorValue.TopF = bd_AcCharge.TopF;


            }
            bd_AdjustmentMinimumCharges.TopF = bdPowerFactorCharges.BottomF;
            bd_AdjustmentMinimumChargesHindi.TopF = bdPowerFactorCharges.BottomF;
            bd_AdjustmentMinimumChargesValue.TopF = bdPowerFactorCharges.BottomF;



            //AdjustmentMinimumCharges Print
            if (op[0].L8_MinCharge == "0.00" || string.IsNullOrEmpty(op[0].L8_MinCharge))
            {
                bd_AdjustmentMinimumCharges.Visible = false;
                bd_AdjustmentMinimumChargesHindi.Visible = false;
                bd_AdjustmentMinimumChargesValue.Visible = false;

                bd_AdjustmentMinimumCharges.TopF = bdPowerFactorCharges.TopF;
                bd_AdjustmentMinimumChargesHindi.TopF = bdPowerFactorCharges.TopF;
                bd_AdjustmentMinimumChargesValue.TopF = bdPowerFactorCharges.TopF;
            }

            bd_ElectricityDuty.TopF = bd_AdjustmentMinimumCharges.BottomF;
            bd_ElectricityDutyHindi.TopF = bd_AdjustmentMinimumCharges.BottomF;
            bd_ElectricityDutyValues.TopF = bd_AdjustmentMinimumCharges.BottomF;


            bd_RlSC1.TopF = bd_ElectricityDuty.BottomF;
            bd_RlSC1Hindi.TopF = bd_ElectricityDuty.BottomF;
            bd_RlSC1Value.TopF = bd_ElectricityDuty.BottomF;
            bd_RlSC1Rate.TopF = bd_ElectricityDuty.BottomF;

            bd_RlSC2.TopF = bd_RlSC1.BottomF;
            bd_RlSC2Hindi.TopF = bd_RlSC1.BottomF;
            bd_RlSC2Value.TopF = bd_RlSC1.BottomF;
            bd_RlSC2Rate.TopF = bd_RlSC1.BottomF;

            bd_OtherCharges.TopF = bd_RlSC2.BottomF;
            bd_OtherChargesHindi.TopF = bd_RlSC2.BottomF;
            bd_OtherChargesValue.TopF = bd_RlSC2.BottomF;


            //Other Charges Print
            if (op[0].L8_ServdetTotbBdtOthr == "0.00" || string.IsNullOrEmpty(op[0].L8_ServdetTotbBdtOthr))
            {
                bd_OtherCharges.Visible = false;
                bd_OtherChargesHindi.Visible = false;
                bd_OtherChargesValue.Visible = false;

                Subsidy.TopF = bd_OtherCharges.TopF;
                SubsidyHindi.TopF = bd_OtherChargesHindi.TopF;
                SubsidyValue.TopF = bd_OtherChargesValue.TopF;


            }
            else
            {
                Subsidy.TopF = bd_OtherCharges.BottomF;
                SubsidyHindi.TopF = bd_OtherChargesHindi.BottomF;
                SubsidyValue.TopF = bd_OtherChargesValue.BottomF;

            }
            bd_OtherCharges.TopF = bd_RlSC2.BottomF;
            bd_OtherChargesHindi.TopF = bd_RlSC2.BottomF;
            bd_OtherChargesValue.TopF = bd_RlSC2.BottomF;

            if (!op[0].L6_TARIFF_DESCR.Contains("LMV") || (op[0].L8_Subsidy_Charges == "" || op[0].L8_Subsidy_Charges == "0.00"))
            {


                Subsidy.Visible = false;
                SubsidyHindi.Visible = false;
                SubsidyValue.Visible = false;

            }
            else
            {

                Subsidy.Visible = true;
                SubsidyHindi.Visible = true;
                SubsidyValue.Visible = true;
            }










            //Late Payment Surcharge
            if (op[0].L9_Int_Tpl == "0.00" || string.IsNullOrEmpty(op[0].L9_Int_Tpl))
            {
                LPSC.Visible = false;
                LPSCHindi.Visible = false;
                LPSCValue.Visible = false;
            }

            #endregion

            var messageFromFile = 0;

            #region File Messages

            if (!string.IsNullOrEmpty(op[0].L22_MESSAGE1))
            {
                messageFromFile++;
                XRLabel xrMessage1 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L22_MESSAGE1,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = (float)0.01,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    //BorderDashStyle = BorderDashStyle.Dot,
                    //Borders = DevExpress.XtraPrinting.BorderSide.All,
                    //BorderWidth = 1,
                };
                xrPanel1.Controls.Add(xrMessage1);
                adjustMessages(xrMessage1);

            }
            if (!string.IsNullOrEmpty(op[0].L23_MESSAGE2))
            {
                messageFromFile++;
                XRLabel xrMessage2 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L23_MESSAGE2,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = (float)0.01,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    //BorderDashStyle = BorderDashStyle.Dot,
                    //Borders = DevExpress.XtraPrinting.BorderSide.All,
                    //BorderWidth = 1,
                };
                xrPanel1.Controls.Add(xrMessage2);
                adjustMessages(xrMessage2);

            }
            if (!string.IsNullOrEmpty(op[0].L24_MESSAGE3))
            {
                messageFromFile++;
                XRLabel xrMessage3 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L24_MESSAGE3,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = (float)0.01,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    //BorderDashStyle = BorderDashStyle.Dot,
                    //Borders = DevExpress.XtraPrinting.BorderSide.All,
                    //BorderWidth = 1,
                };
                xrPanel1.Controls.Add(xrMessage3);
                adjustMessages(xrMessage3);

            }
            if (!string.IsNullOrEmpty(op[0].L25_MESSAGE4))
            {
                messageFromFile++;
                XRLabel xrMessage4 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L25_MESSAGE4,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = (float)0.01,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    //BorderDashStyle = BorderDashStyle.Dot,
                    //Borders = DevExpress.XtraPrinting.BorderSide.All,
                    //BorderWidth = 1,
                };
                xrPanel1.Controls.Add(xrMessage4);
                adjustMessages(xrMessage4);

            }
            if (!string.IsNullOrEmpty(op[0].L26_MESSAGE5))
            {
                messageFromFile++;
                XRLabel xrMessage5 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L26_MESSAGE5,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = (float)0.01,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    //BorderDashStyle = BorderDashStyle.Dot,
                    //Borders = DevExpress.XtraPrinting.BorderSide.All,
                    //BorderWidth = 1,
                };
                xrPanel1.Controls.Add(xrMessage5);
                adjustMessages(xrMessage5);

            }
            if (!string.IsNullOrEmpty(op[0].L27_MESSAGE6))
            {
                messageFromFile++;
                XRLabel xrMessage6 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L27_MESSAGE6,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = (float)0.01,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    //BorderDashStyle = BorderDashStyle.Dot,
                    //Borders = DevExpress.XtraPrinting.BorderSide.All,
                    //BorderWidth = 1,
                };
                xrPanel1.Controls.Add(xrMessage6);
                adjustMessages(xrMessage6);

            }
            if (!string.IsNullOrEmpty(op[0].L28_MESSAGE7))
            {
                messageFromFile++;
                XRLabel xrMessage7 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L28_MESSAGE7,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = (float)0.01,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    //BorderDashStyle = BorderDashStyle.Dot,
                    //Borders = DevExpress.XtraPrinting.BorderSide.All,
                    //BorderWidth = 1,
                };
                xrPanel1.Controls.Add(xrMessage7);
                adjustMessages(xrMessage7);

            }
            if (!string.IsNullOrEmpty(op[0].L29_MESSAGE8))
            {
                messageFromFile++;
                XRLabel xrMessage8 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L29_MESSAGE8,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = (float)0.01,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    //BorderDashStyle = BorderDashStyle.Dot,
                    //Borders = DevExpress.XtraPrinting.BorderSide.All,
                    //BorderWidth = 1,
                };
                xrPanel1.Controls.Add(xrMessage8);
                adjustMessages(xrMessage8);

            }
            if (!string.IsNullOrEmpty(op[0].L30_MESSAGE9))
            {
                if (!IsMessageLimitExceeds(messageFromFile))
                {
                    messageFromFile++;
                    XRLabel xrMessage9 = new XRLabel
                    {
                        Font = new System.Drawing.Font("DIN Pro Regular", 8),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = op[0].L30_MESSAGE9,
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = (float)0.01,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                        //BorderDashStyle = BorderDashStyle.Dot,
                        //Borders = DevExpress.XtraPrinting.BorderSide.All,
                        //BorderWidth = 1,
                    };
                    xrPanel1.Controls.Add(xrMessage9);
                    adjustMessages(xrMessage9);

                }
            }
            if (!string.IsNullOrEmpty(op[0].L31_MESSAGE10))
            {
                if (!IsMessageLimitExceeds(messageFromFile))
                {
                    messageFromFile++;
                    XRLabel xrMessage10 = new XRLabel
                    {
                        Font = new System.Drawing.Font("DIN Pro Regular", 8),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = op[0].L31_MESSAGE10,
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = (float)0.01,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                        //BorderDashStyle = BorderDashStyle.Dot,
                        //Borders = DevExpress.XtraPrinting.BorderSide.All,
                        //BorderWidth = 1,
                    };
                    xrPanel1.Controls.Add(xrMessage10);
                    adjustMessages(xrMessage10);

                }
            }
            #endregion

            #region Custom Messages
            var totalMessages = messageFromFile;


            if ((!string.IsNullOrEmpty(op[0].L6_EXCESS_DEMAND) && op[0].L6_EXCESS_DEMAND != "0.00(KW)") || ((!string.IsNullOrEmpty(op[0].L9_MessageIndication) && (op[0].L9_MessageIndication == "2"))))
            {
                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageExcessDemand = new XRLabel
                    {
                        Font = new System.Drawing.Font("Kruti Dev 010", 9),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = getMessage(LoadStaticData._HindiMessage, "EDC"),
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = (float)0.01,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                        //BorderDashStyle = BorderDashStyle.Dot,
                        //Borders = DevExpress.XtraPrinting.BorderSide.All,
                        //BorderWidth = 1,
                    };
                    xrPanel1.Controls.Add(xrMessageExcessDemand);
                    adjustMessages(xrMessageExcessDemand);
                }
            }


            if (!string.IsNullOrEmpty(op[0].L8_PowerFactorAdj) && op[0].L8_PowerFactorAdj != "0.00")
            {
                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageExcessDemand = new XRLabel
                    {
                        Font = new System.Drawing.Font("Kruti Dev 010", 9),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        //Text = string.Format(getMessage(LoadStaticData._HindiMessage, "PFM"), op[0].L8_PowerFactorAdj),
                        Text = string.Format(getMessage(LoadStaticData._HindiMessage, "PFM"), "0.90".ToString().Replace('.', '-')),
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = (float)0.01,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                        //BorderDashStyle = BorderDashStyle.Dot,
                        //Borders = DevExpress.XtraPrinting.BorderSide.All,
                        //BorderWidth = 1,
                    };
                    xrPanel1.Controls.Add(xrMessageExcessDemand);
                    adjustMessages(xrMessageExcessDemand);
                }
            }

            if (!string.IsNullOrEmpty(op[0].L1_DisconnectionMSGPrintingIMMEDIATE) && op[0].L1_DisconnectionMSGPrintingIMMEDIATE != "0")
            {
                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageDisconnection = new XRLabel
                    {
                        Font = new System.Drawing.Font("Kruti Dev 010", 9),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = getMessage(LoadStaticData._HindiMessage, "IDC"),
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = (float)0.01,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                        //BorderDashStyle = BorderDashStyle.Dot,
                        //Borders = DevExpress.XtraPrinting.BorderSide.All,
                        //BorderWidth = 1,
                    };
                    xrPanel1.Controls.Add(xrMessageDisconnection);
                    adjustMessages(xrMessageDisconnection);
                }
            }

            if (!string.IsNullOrEmpty(op[0].L10_TheftAmount) && op[0].L10_TheftAmount != "0.00")
            {
                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageTheftAmount = new XRLabel
                    {
                        Font = new System.Drawing.Font("Kruti Dev 010", 9),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = string.Format(getMessage(LoadStaticData._HindiMessage, "TFA"), op[0].L10_TheftAmount.Replace('.', '-')),
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = (float)0.01,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                        //BorderDashStyle = BorderDashStyle.Dot,
                        //Borders = DevExpress.XtraPrinting.BorderSide.All,
                        //BorderWidth = 1,
                    };


                    xrPanel1.Controls.Add(xrMessageTheftAmount);
                    adjustMessages(xrMessageTheftAmount);
                }
            }

            if (!string.IsNullOrEmpty(op[0].L10_TotArrUPPCLIntUPPCLIntArrUPPCL) && !Convert.ToDecimal(op[0].L10_TotArrUPPCLIntUPPCLIntArrUPPCL).Equals(decimal.Zero))
            {

                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageTheftAmount = new XRLabel
                    {
                        Font = new System.Drawing.Font("Kruti Dev 010", 9),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = string.Format(getMessage(LoadStaticData._HindiMessage, "DAD"), op[0].L10_TheftAmount),
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = (float)0.01,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                        //BorderDashStyle = BorderDashStyle.Dot,
                        //Borders = DevExpress.XtraPrinting.BorderSide.All,
                        //BorderWidth = 1,
                    };


                    xrPanel1.Controls.Add(xrMessageTheftAmount);
                    adjustMessages(xrMessageTheftAmount);
                }
            }

            if (!string.IsNullOrEmpty(op[0].L8_ParkingAmount) && op[0].L8_ParkingAmount != "0.00")
            {
                xrLabel21.Visible = true;
            }

            if (!string.IsNullOrEmpty(op[0].L1_BillingCode))
            {
                if (op[0].L1_BillingCode == "3000")
                {
                    if (!IsMessageLimitExceeds(totalMessages))
                    {
                        totalMessages++;
                        decimal totalUnits = decimal.Zero;
                        if (!string.IsNullOrEmpty(op[0].L16_M1_KWH_UNITS))
                        {
                            totalUnits += Convert.ToDecimal(op[0].L16_M1_KWH_UNITS);
                        }

                        if (!string.IsNullOrEmpty(op[0].L20_M2_KWH_UNITS))
                        {
                            totalUnits += Convert.ToDecimal(op[0].L20_M2_KWH_UNITS);
                        }
                        var PrevReadDt = ChangeMonthToHindi(op[0].L7_PrevReadDt);
                        var ReadDt = ChangeMonthToHindi(op[0].L7_ReaDt);

                        XRLabel xrMessageTheftAmount = new XRLabel
                        {
                            Font = new System.Drawing.Font("Kruti Dev 010", 9),
                            TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                            Text = string.Format(getMessage(LoadStaticData._HindiMessage, "AB1"), totalUnits.ToString().Replace(".", "-"), PrevReadDt.Replace("-", "&"), ReadDt.Replace("-", "&"), op[0].L10_Mode + " fnu"),
                            WordWrap = false,
                            AutoWidth = true,
                            KeepTogether = true,
                            HeightF = (float)0.01,
                            Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                            //BorderDashStyle = BorderDashStyle.Dot,
                            //Borders = DevExpress.XtraPrinting.BorderSide.All,
                            //BorderWidth = 1,
                        };


                        xrPanel1.Controls.Add(xrMessageTheftAmount);
                        adjustMessages(xrMessageTheftAmount);

                        XRLabel xrAB2Msg = new XRLabel
                        {
                            Font = new System.Drawing.Font("Kruti Dev 010", 9),
                            TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                            Text = string.Format(getMessage(LoadStaticData._HindiMessage, "AB2")),
                            WordWrap = false,
                            AutoWidth = true,
                            KeepTogether = true,
                            HeightF = (float)0.01,
                            Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                            //BorderDashStyle = BorderDashStyle.Dot,
                            //Borders = DevExpress.XtraPrinting.BorderSide.All,
                            //BorderWidth = 1,
                        };


                        xrPanel1.Controls.Add(xrAB2Msg);
                        adjustMessages(xrAB2Msg);


                    }
                }
            }


            if (!string.IsNullOrEmpty(op[0].L9_MessageFlag))
            {
                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageExcessDemand = new XRLabel
                    {
                        Font = new System.Drawing.Font("Kruti Dev 010", 9),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = getMessage(LoadStaticData._HindiMessage, "TPC"),
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = (float)0.01,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                        //BorderDashStyle = BorderDashStyle.Dot,
                        //Borders = DevExpress.XtraPrinting.BorderSide.All,
                        //BorderWidth = 1,
                    };
                    xrPanel1.Controls.Add(xrMessageExcessDemand);
                    adjustMessages(xrMessageExcessDemand);
                }
            }

            if (!string.IsNullOrEmpty(op[0].L8_Intrest_Amount) && Convert.ToDecimal(op[0].L8_Intrest_Amount) > 0)
            {
                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageExcessDemand = new XRLabel
                    {
                        Font = new System.Drawing.Font("Kruti Dev 010", 9),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = string.Format(getMessage(LoadStaticData._HindiMessage, "IAE"), op[0].L8_Intrest_Amount.ToString().Replace('.', '-')),
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = (float)0.01,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                        //BorderDashStyle = BorderDashStyle.Dot,
                        //Borders = DevExpress.XtraPrinting.BorderSide.All,
                        //BorderWidth = 1,
                    };
                    xrPanel1.Controls.Add(xrMessageExcessDemand);
                    adjustMessages(xrMessageExcessDemand);
                }
            }

            #endregion

            #region Template Messages
            if (!string.IsNullOrEmpty(op[0].L33_MESSAGE7))
            {
                messageFromFile++;
                XRLabel xrMessage7 = new XRLabel
                {
                    Font = new System.Drawing.Font("Kruti Dev 010", 9),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopJustify,
                    Text = op[0].L33_MESSAGE7,
                    WordWrap = false,
                    AutoWidth = true,
                    Multiline = true,
                    KeepTogether = true,
                    HeightF = 0.1f,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    //BorderDashStyle = BorderDashStyle.Dot,
                    //Borders = DevExpress.XtraPrinting.BorderSide.All,
                    //BorderWidth = 1,
                };
                xrPanel1.Controls.Add(xrMessage7);
                adjustMessages(xrMessage7);

            }
            if (!string.IsNullOrEmpty(op[0].L34_MESSAGE8))
            {
                messageFromFile++;
                XRLabel xrMessage8 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopJustify,
                    Text = op[0].L34_MESSAGE8,
                    WordWrap = false,
                    CanShrink = true,
                    Multiline = true,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 0.1f,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    //BorderDashStyle = BorderDashStyle.Dot,
                    //Borders = DevExpress.XtraPrinting.BorderSide.All,
                    //BorderWidth = 1,
                };
                xrPanel1.Controls.Add(xrMessage8);
                adjustMessages(xrMessage8);
            }
            if (!string.IsNullOrEmpty(op[0].L35_MESSAGE9))
            {
                messageFromFile++;
                XRLabel xrMessage9 = new XRLabel
                {
                    Font = new System.Drawing.Font("Kruti Dev 010", 9),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopJustify,
                    Text = op[0].L35_MESSAGE9,
                    WordWrap = false,
                    CanShrink = true,
                    Multiline = true,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 0.1f,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    //BorderDashStyle = BorderDashStyle.Dot,
                    //Borders = DevExpress.XtraPrinting.BorderSide.All,
                    //BorderWidth = 1,
                };
                xrPanel1.Controls.Add(xrMessage9);
                adjustMessages(xrMessage9);
            }
            if (!string.IsNullOrEmpty(op[0].L36_MESSAGE10))
            {
                messageFromFile++;
                XRLabel xrMessage10 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopJustify,
                    Text = op[0].L36_MESSAGE10,
                    WordWrap = false,
                    CanShrink = true,
                    Multiline = true,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 0.1f,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    //BorderDashStyle = BorderDashStyle.Dot,
                    //Borders = DevExpress.XtraPrinting.BorderSide.All,
                    //BorderWidth = 1,
                };
                xrPanel1.Controls.Add(xrMessage10);
                adjustMessages(xrMessage10);
            }
            #endregion

            #region BroadCast Messages             
            if (!IsMessageLimitExceeds(totalMessages))
            {
                if (LoadStaticData._BroadcastMessage.FindAll(x => x.ServiceNo.ToUpper().Equals(op[0].L6_SERVDET_SERVNO)).FirstOrDefault() != null)
                {
                    BroadcastMessage brdcstMsg = LoadStaticData._BroadcastMessage.FindAll(x => x.ServiceNo.ToUpper().Equals(op[0].L6_SERVDET_SERVNO)).FirstOrDefault();
                    totalMessages++;
                    XRLabel xrMessageTheftAmount = new XRLabel
                    {


                        Font = brdcstMsg.MessageType.ToUpper() == "ENG" ? Font = new System.Drawing.Font("DIN Pro Regular", 8) : new System.Drawing.Font("Kruti Dev 010", 9),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = brdcstMsg.MessageType.ToUpper() == "ENG" ? brdcstMsg.EnglishMessageString : brdcstMsg.HindiMessageString,
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = (float)0.01,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                        //BorderDashStyle = BorderDashStyle.Dot,
                        //Borders = DevExpress.XtraPrinting.BorderSide.All,
                        //BorderWidth = 1,
                    };

                    xrPanel1.Controls.Add(xrMessageTheftAmount);
                    adjustMessages(xrMessageTheftAmount);
                }
            }
            #endregion

            #region Security Deposit Message   
            if ((string.IsNullOrEmpty(op[0].L10_SecDeptBdt) || Convert.ToDouble(op[0].L10_SecDeptBdt) == 0) && Convert.ToDouble(op[0].L6_SERVDET_SERVNO) < 674199999)
            {
                messageFromFile++;
                XRLabel xrMessage11 = new XRLabel
                {
                    Font = new System.Drawing.Font("Kruti Dev 010", 9),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = "vfHkys[kksa ds vuqlkj vkids la;kstu ij tekur /kujkf'k 'kwU; vafdr gSaA ;fn vkids }kjk la;kstu jkf'k tek dh xbZ gS rks mDr tekur jkf'k dh ewy jlhn ds lkFk \r\ngekjs xzkgd lsok dsUnz  ij lEidZ djsaA ",
                    WordWrap = false,
                    AutoWidth = true,
                    Multiline = true,
                    KeepTogether = true,
                    HeightF = 0.1f,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    //BorderDashStyle = BorderDashStyle.Dot,
                    //Borders = DevExpress.XtraPrinting.BorderSide.All,
                    //BorderWidth = 1,
                };
                xrPanel1.Controls.Add(xrMessage11);
                adjustMessages(xrMessage11);

            }

            #endregion

        }

        #region Helper Functions
        bool IsMessageLimitExceeds(int messagesCount)
        {
            if (messagesCount >= 8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void adjustMessages(XRLabel lbl)
        {
            if (xrPanel1.Controls.Count != 0)
            {
                foreach (XRLabel plbl in xrPanel1.Controls)
                {
                    lbl.TopF = (plbl.BottomF - 0.85f);
                }
            }
            else
            {
                lbl.TopF = xrPanel1.TopF;
            }
        }

        public string getMessage(Hashtable _tbl, string Code)
        {
            string message = string.Empty;
            foreach (DictionaryEntry element in _tbl)
            {
                message = element.Key.ToString() == Code ? element.Value.ToString() : "";
                if (!string.IsNullOrEmpty(message))
                {
                    return message;
                }
            }

            return message;

        }
        public string ChangeMonthToHindi(string Date)
        {
            if (!string.IsNullOrEmpty(Date))
            {
                var month = Date.Split('-')[1];
                switch (month)
                {

                    case "01":
                        return Date.Replace("01", "tuojh");
                    case "02":
                        return Date.Replace("02", "Qjojh");
                    case "03":
                        return Date.Replace("03", "ekpZ");
                    case "04":
                        return Date.Replace("04", "vizSy");
                    case "05":
                        return Date.Replace("05", "ebZ");
                    case "06":
                        return Date.Replace("06", "twu");
                    case "07":
                        return Date.Replace("07", "tqykbZ");
                    case "08":
                        return Date.Replace("08", "vxLr");
                    case "09":
                        return Date.Replace("09", "flrEcj");
                    case "10":
                        return Date.Replace("10", "vDVwcj");
                    case "11":
                        return Date.Replace("11", "ucEcj");
                    case "12":
                        return Date.Replace("12", "fnlacj");

                }
            }
            return "";

        }


        #endregion

        private void xrPanel1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {


        }
    }

}

