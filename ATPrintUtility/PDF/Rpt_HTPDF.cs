using AT.Print.Utils;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AT.Print.PDF
{
    public partial class Rpt_HTPDF : DevExpress.XtraReports.UI.XtraReport
    {
        public Rpt_HTPDF()
        {
            InitializeComponent();
        }

        private void Rpt_HTPDF_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = sender as Rpt_HTPDF;
            var op = data.DataSource as List<SingleHTBill>;

           // xrLabel6.Visible = true;

           #region RISC1 Change
            if (op[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 5A") || op[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 5B") || op[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 1B") || op[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 1C"))
            {
                bd_RlSC1Rate.Text = "@ 1.14%";

            }
            #endregion
            //if (Convert.ToInt32(op[0].L8_AmountPayableBeforeDueDate) >= 200000)
            if (Convert.ToDouble(op[0].L8_AmountPayableBeforeDueDate.Replace("CR","").Contains('-') ? ("-" + op[0].L8_AmountPayableBeforeDueDate.Replace("CR","").Replace('-', ' ').Trim()) : op[0].L8_AmountPayableBeforeDueDate.Replace("CR","")) >= 200000)
            {
                xrLabel31.Visible = true;
            }
            else
            {
                xrLabel31.Visible = false;
            }

            if (!string.IsNullOrEmpty(op[0].L1_Customer_PAN))
            {
                xrLabel30.Visible = true;
            }
            else
            {
                xrLabel30.Visible = false;
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
                xrLabel30.TopF = xrLabel142.TopF;
            }


            #region Disconnection Message
            //Disconnection Messages Print
            if (op[0].L1_DisconnectionMSGPrintingIMMEDIATE == "1")
            {
                 xrDueDate.Text = "IMMEDIATE/";           
                 xrDueDate2.Text = "IMMEDIATE";            
                 xrDisconnectionDate.Text = "IMMEDIATE/"; 
                 xrImmediatelbl.Visible = true;            
                 xrLabel20.Visible = true;                 
                // xrLabel6_2.Visible = true;                

            }
            else
            {
                 xrDueDate.Text = op[0].L7_Due_Date;
                 xrDueDate2.Text = op[0].L7_Due_Date;
                 xrDisconnectionDate.Text = op[0].L10_DisconnDate;
                 //xrDueDate.TextAlignment = TextAlignment.MiddleRight;
                 //xrDueDate2.TextAlignment = TextAlignment.MiddleRight;
                 xrDisconnectionDate.TextAlignment = TextAlignment.MiddleRight;
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
                            if (Convert.ToDecimal(op[0].L6_BILL_PF) >= (decimal)95)
                            {
                                op[0].L6_BILL_PF = op[0].L6_BILL_PF + "(Lead)";
                            }
                        }
                    }
                    if (op[0].L6_MeasureContractDemand.ToUpper() == "KW")
                    {
                        if (Math.Ceiling(contractdemand) >= (decimal)10.0)
                        {
                            if (Convert.ToDecimal(op[0].L6_BILL_PF) >= (decimal)95)
                            {
                                op[0].L6_BILL_PF = op[0].L6_BILL_PF + "(Lead)";
                            }
                        }
                    }
                }
            }

            //op[0].L6_SERVDET_SANC_LOAD = op[0].L6_SERVDET_SANC_LOAD + "(" + op[0].L6_MeasureContractDemand + ")";
            

         

            if (op[0].L6_MeasureContractDemand == "HP")
            {

                if (op[0].L6_Kvah_Indicator == "1")
                {
                    string unit1 = "KVA";

                    op[0].L6_ACTUAL_DEMAND = op[0].L6_ACTUAL_DEMAND + "(" + unit1 + ")";
                    op[0].L6_EXCESS_DEMAND = op[0].L6_EXCESS_DEMAND + "(" + unit1 + ")";
                    op[0].L6_bill_demand = op[0].L6_bill_demand + "(" + unit1 + ")";
                    
                    met1_headingMDKW.Text = "MD" + unit1;
                    met1_headingMDKW_1.Text  = unit1 + "H";
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





           // #region Excess Demand Print
           // //Excess Demand Print
           // if (op[0].L6_EXCESS_DEMAND != "0.00(" + unit + ")")
           // {
           //     xrlL6ExcessDemand.Text = op[0].L6_EXCESS_DEMAND;
           //     xrlL6ExcessDemand.Visible = true;
           // }
           // #endregion
            #region Excess Demand Print
             //Excess Demand Print
             if (op[0].L6_EXCESS_DEMAND.Substring(0, 4) != "0.00")
             {
                 xrlL6ExcessDemand.Text = op[0].L6_EXCESS_DEMAND;
                xrlL6ExcessDemand.Visible = true;
            }
            #endregion

            #region Meter Change
            //Meter Change Print
            if (op[0].L11_MTRSNO_2_IF_AVAILABLE != "")
            {

                met1_headingMDKW.Visible = true;
                met1_headingMDKW_1.Visible = true;
                met2_headingMDKW.Visible = true;
                met2_headingMDKW_2.Visible = true;
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
            bd_AdjustmentMinimumCharges.TopF = bd_AcCharge.BottomF;
            bd_AdjustmentMinimumChargesHindi.TopF = bd_AcCharge.BottomF;
            bd_AdjustmentMinimumChargesValue.TopF = bd_AcCharge.BottomF;

            //AdjustmentMinimumCharges Print
            if (op[0].L8_MinCharge == "0.00" || string.IsNullOrEmpty(op[0].L8_MinCharge))
            {
                bd_AdjustmentMinimumCharges.Visible = false;
                bd_AdjustmentMinimumChargesHindi.Visible = false;
                bd_AdjustmentMinimumChargesValue.Visible = false;

                bd_AdjustmentMinimumCharges.TopF = bd_AcCharge.TopF;
                bd_AdjustmentMinimumChargesHindi.TopF = bd_AcCharge.TopF;
                bd_AdjustmentMinimumChargesValue.TopF = bd_AcCharge.TopF;



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


            //Other Charges Print
            if (op[0].L8_ServdetTotbBdtOthr == "0.00" || string.IsNullOrEmpty(op[0].L8_ServdetTotbBdtOthr))
            {
                bd_OtherCharges.Visible = false;
                bd_OtherChargesHindi.Visible = false;
                bd_OtherChargesValue.Visible = false;
                bd_OtherCharges.TopF = bd_RlSC2.BottomF;
                bd_OtherChargesHindi.TopF = bd_RlSC2.BottomF;
                bd_OtherChargesValue.TopF = bd_RlSC2.BottomF;

            }
            bd_OtherCharges.TopF = bd_RlSC2.BottomF;
            bd_OtherChargesHindi.TopF = bd_RlSC2.BottomF;
            bd_OtherChargesValue.TopF = bd_RlSC2.BottomF;

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

            if (!string.IsNullOrEmpty(op[0].L26_MESSAGE1))
            {
                messageFromFile++;
                XRLabel xrMessage1 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L26_MESSAGE1,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 2,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage1);
                adjustMessages(xrMessage1);

            }
            if (!string.IsNullOrEmpty(op[0].L27_MESSAGE2))
            {
                messageFromFile++;
                XRLabel xrMessage2 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L27_MESSAGE2,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 2,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage2);
                adjustMessages(xrMessage2);

            }
            if (!string.IsNullOrEmpty(op[0].L28_MESSAGE3))
            {
                messageFromFile++;
                XRLabel xrMessage3 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L28_MESSAGE3,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 2,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage3);
                adjustMessages(xrMessage3);

            }
            if (!string.IsNullOrEmpty(op[0].L29_MESSAGE4))
            {
                messageFromFile++;
                XRLabel xrMessage4 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L29_MESSAGE4,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 2,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage4);
                adjustMessages(xrMessage4);

            }
            if (!string.IsNullOrEmpty(op[0].L30_MESSAGE5))
            {
                messageFromFile++;
                XRLabel xrMessage5 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L30_MESSAGE5,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 2,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage5);
                adjustMessages(xrMessage5);

            }
            if (!string.IsNullOrEmpty(op[0].L31_MESSAGE6))
            {
                messageFromFile++;
                XRLabel xrMessage6 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L31_MESSAGE6,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 2,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage6);
                adjustMessages(xrMessage6);

            }
            #endregion

            #region Custom Messages
            var totalMessages = messageFromFile;

            // if (!string.IsNullOrEmpty(op[0].L6_EXCESS_DEMAND) && op[0].L6_EXCESS_DEMAND != "0.00(" + op[0].unit + ")")
            // {
            //     if (!IsMessageLimitExceeds(totalMessages))
            //     {
            //         totalMessages++;
            //         XRLabel xrMessageExcessDemand = new XRLabel
            //         {
            //             Font = new System.Drawing.Font("Kruti Dev 010", 9),
            //             TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
            //             Text = getMessage(LoadStaticData._HindiMessage, "EDC"),
            //             WordWrap = false,
            //             AutoWidth = true,
            //             KeepTogether = true,
            //             HeightF = 2,
            //         };
            //         xrPanel1.Controls.Add(xrMessageExcessDemand);
            //         adjustMessages(xrMessageExcessDemand);
            //     }
            // }

            if (!String.IsNullOrEmpty(op[0].L6_LT_Metering_Flag))
            {

                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageTheftAmount = new XRLabel
                    {
                        Font = new System.Drawing.Font("DIN Pro Regular", 8),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = string.Format("*" + getMessage(LoadStaticData._EnglishMessage, "BRDCST3"), "*" + op[0].L10_TheftAmount.Replace('.', '-')),
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = 1,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    };


                    xrPanel1.Controls.Add(xrMessageTheftAmount);
                    adjustMessages(xrMessageTheftAmount);
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
                        HeightF = 1,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
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
                        HeightF = 1,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
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
                        Text = string.Format(getMessage(LoadStaticData._HindiMessage, "DAD"), op[0].L10_TotArrUPPCLIntUPPCLIntArrUPPCL),
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = 1,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    };


                    xrPanel1.Controls.Add(xrMessageTheftAmount);
                    adjustMessages(xrMessageTheftAmount);
                }
            }

            if (!string.IsNullOrEmpty(op[0].L8_ParkingAmount) && !Convert.ToDecimal(op[0].L8_ParkingAmount).Equals(Decimal.Zero))
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
                        if (!string.IsNullOrEmpty(op[0].L6_Kvah_Indicator) && op[0].L6_Kvah_Indicator == "1")
                        {
                            if (!string.IsNullOrEmpty(op[0].L15_KVAH_UNITS))
                            {
                                totalUnits += Convert.ToDecimal(op[0].L15_KVAH_UNITS);
                            }


                            if (!string.IsNullOrEmpty(op[0].L21_KVAH_UNITS))
                            {
                                totalUnits += Convert.ToDecimal(op[0].L21_KVAH_UNITS);
                            }

                        }
                        else
                        {

                            if (!string.IsNullOrEmpty(op[0].L15_KWH_UNITS))
                            {
                                totalUnits += Convert.ToDecimal(op[0].L15_KWH_UNITS);
                            }


                            if (!string.IsNullOrEmpty(op[0].L21_KWH_UNITS))
                            {
                                totalUnits += Convert.ToDecimal(op[0].L21_KWH_UNITS);
                            }


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
                            HeightF = 1,
                            Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
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
                            HeightF = 1,
                            Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                        };


                        xrPanel1.Controls.Add(xrAB2Msg);
                        adjustMessages(xrAB2Msg);


                    }
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
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L33_MESSAGE7,
                    WordWrap = false,
                    AutoWidth = true,
                    Multiline = true,
                    KeepTogether = true,
                    HeightF = 0.1f,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
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
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L34_MESSAGE8,
                    WordWrap = false,
                    CanShrink = true,
                    Multiline = true,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 0.1f,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
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
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L35_MESSAGE9,
                    WordWrap = false,
                    CanShrink = true,
                    Multiline = true,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 0.1f,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
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
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L36_MESSAGE10,
                    WordWrap = false,
                    CanShrink = true,
                    Multiline = true,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 0.1f,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
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
                        Font = brdcstMsg.MessageType.ToUpper() == "ENG" ? new System.Drawing.Font("DIN Pro Regular", 8) : new System.Drawing.Font("Kruti Dev 010", 9),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = brdcstMsg.MessageType.ToUpper() == "ENG" ? brdcstMsg.EnglishMessageString : brdcstMsg.HindiMessageString,
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = 1,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
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

            if (!String.IsNullOrEmpty(op[0].L6_LT_Metering_Flag))
            {
                xrlBillDemand.Text = "*" + xrlBillDemand.Text;
            }

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
                    lbl.TopF = plbl.BottomF;
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
    }

}

