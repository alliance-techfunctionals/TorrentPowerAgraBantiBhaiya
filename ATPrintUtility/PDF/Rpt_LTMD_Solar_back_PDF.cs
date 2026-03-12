using AT.Print.Utils;
using System.Collections.Generic;
using System.Windows.Forms;
using System;
using DevExpress.Drawing;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Collections;

namespace AT.Print.PDF
{
    public partial class Rpt_LTMD_Solar_back_PDF : DevExpress.XtraReports.UI.XtraReport
    {
        public Rpt_LTMD_Solar_back_PDF()
        {
            InitializeComponent();
        }



        #region Meter Print
        private void Rpt_LTMD_solar_Back_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {

            var op = this.DataSource as List<SolarBill>;
            xrPictureBox2.ImageUrl = Application.StartupPath + "\\Contents\\CategorySlabImages\\" + op[0].L6_TARIFF_DESCR + ".png";
            xrPictureBox1.ImageUrl = Application.StartupPath + "\\Contents\\CategorySlabImages\\PromotionImage.png";
            xrLabel2.SendToBack();

            #region RISC1 Change
            if (op[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 5A") || op[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 5B") || op[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 1B") || op[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 1C"))
            {
                bd_RlSC1Rate.Text = "@ 1.14%";

            }
            #endregion

            #region Bill Details
            //Excess Demand Surcharge Print
            if (op[0].L10_DMDCHG_PENALTY == "0.00" || string.IsNullOrEmpty(op[0].L10_DMDCHG_PENALTY))
            {
                bd_ExcessDemandCharges.Visible = false;
                bd_ExcessDemandChargesHindi.Visible = false;
                bd_ExcessDemandChargesValue.Visible = false;

                bd_ExcessDemandCharges.TopF = bd_Demand_charges.TopF;
                bd_ExcessDemandChargesHindi.TopF = bd_Demand_charges.TopF;
                bd_ExcessDemandChargesValue.TopF = bd_Demand_chargesValue.TopF;

            }
            bd_EnergyCharge.TopF = bd_ExcessDemandCharges.BottomF;
            bd_EnergyChargeHindi.TopF = bd_ExcessDemandChargesValue.BottomF;
            bd_EnergyChargeValues.TopF = bd_ExcessDemandCharges.BottomF;

            bd_TODCharges.TopF = bd_EnergyCharge.BottomF;
            bd_TODChargesHindi.TopF = bd_EnergyCharge.BottomF;
            bd_TODChargesValues.TopF = bd_EnergyCharge.BottomF;

            bd_ElectricityDuty.TopF = bd_TODCharges.BottomF;
            bd_ElectricityDutyHindi.TopF = bd_TODCharges.BottomF;
            bd_ElectricityDutyValues.TopF = bd_TODCharges.BottomF;

            bd_RlSC1.TopF = bd_ElectricityDuty.BottomF;
            bd_RlSC1Hindi.TopF = bd_ElectricityDuty.BottomF;
            bd_RlSC1Value.TopF = bd_ElectricityDuty.BottomF;
            bd_RlSC1Rate.TopF = bd_ElectricityDuty.BottomF;

            bd_RlSC2.TopF = bd_RlSC1.BottomF;
            bd_RlSC2Hindi.TopF = bd_RlSC1.BottomF;
            bd_RlSC2Value.TopF = bd_RlSC1.BottomF;
            bd_RlSC2Rate.TopF = bd_RlSC1.BottomF;

            bd_AcCharges.TopF = bd_RlSC2.BottomF;
            bd_AcChargesValues.TopF = bd_RlSC2.BottomF;
            bd_AcChargeHindi.TopF = bd_RlSC2.BottomF;

            if (op[0].L8_AC_Charges == "0.00" || string.IsNullOrEmpty(op[0].L8_AC_Charges))
            {
                bd_AcCharges.Visible = false;
                bd_AcChargesValues.Visible = false;
                bd_AcChargeHindi.Visible = false;

                bd_AcCharges.TopF = bd_RlSC2.TopF;
                bd_AcChargeHindi.TopF = bd_RlSC2.TopF;
                bd_AcChargesValues.TopF = bd_RlSC2.TopF;

            }

            bd_Power_Fector_Charges.TopF = bd_AcCharges.BottomF;
            bd_powerFactorHindi.TopF = bd_AcCharges.BottomF;
            bd_Power_Fector_ChargesValues.TopF = bd_AcChargesValues.BottomF;
            if (op[0].L8_power_factor_adj == "0.00" || string.IsNullOrEmpty(op[0].L8_power_factor_adj))
            {
                bd_Power_Fector_Charges.Visible = false;
                bd_Power_Fector_ChargesValues.Visible = false;
                bd_powerFactorHindi.Visible = false;

                bd_Power_Fector_Charges.TopF = bd_AcCharges.TopF;
                bd_Power_Fector_ChargesValues.TopF = bd_AcChargesValues.TopF;
                bd_powerFactorHindi.TopF = bd_AcCharges.TopF;

            }

            bd_AdjustmentCharges.TopF = bd_Power_Fector_Charges.BottomF;
            bd_AdjustmentChargesValues.TopF = bd_Power_Fector_ChargesValues.BottomF;
            bd_AdjustmentMinimumChargesHindi.TopF = bd_Power_Fector_Charges.BottomF;

            if (op[0].L8_min_charge == "0.00" || string.IsNullOrEmpty(op[0].L8_min_charge))
            {
                bd_AdjustmentCharges.Visible = false;
                bd_AdjustmentChargesValues.Visible = false;
                bd_AdjustmentMinimumChargesHindi.Visible = false;

                bd_AdjustmentCharges.TopF = bd_Power_Fector_Charges.TopF;
                bd_AdjustmentChargesValues.TopF = bd_Power_Fector_ChargesValues.TopF;
                bd_AdjustmentMinimumChargesHindi.TopF = bd_Power_Fector_Charges.TopF;

            }
            bd_Other.TopF = bd_AdjustmentCharges.BottomF;
            bd_OtherValues.TopF = bd_AdjustmentChargesValues.BottomF;
            bd_OtherChargesHindi.TopF = bd_AdjustmentCharges.BottomF;
            if (op[0].L8_SERVDET_TOTDB_BDT_OTHER == "0.00" || string.IsNullOrEmpty(op[0].L8_SERVDET_TOTDB_BDT_OTHER))
            {
                bd_Other.Visible = false;
                bd_OtherValues.Visible = false;
                bd_OtherChargesHindi.Visible = false;

                bd_Other.TopF = bd_AdjustmentCharges.TopF;
                bd_OtherValues.TopF = bd_AdjustmentChargesValues.TopF;
                bd_OtherChargesHindi.TopF = bd_AdjustmentCharges.TopF;
            }



            Subsidy.TopF = bd_Other.BottomF;
            SubsidyValue.TopF = bd_OtherValues.BottomF;
            SubsidyHindi.TopF = bd_Other.BottomF;
            if (op[0].L8_Subsidy_Charges == "" || op[0].L8_Subsidy_Charges == "0.00")
            {
                Subsidy.Visible = false;
                SubsidyValue.Visible = false;
                SubsidyHindi.Visible = false;

                Subsidy.TopF = bd_Other.TopF;
                SubsidyValue.TopF = bd_OtherValues.TopF;
                SubsidyHindi.TopF = bd_Other.TopF;

            }

            GreenTariff.TopF = Subsidy.BottomF;
            GreenTariffValue.TopF = SubsidyValue.BottomF;
            GreenTariffHindi.TopF = Subsidy.BottomF;

            if (op[0].L8_GreenTariff_Charges == "0.00" || string.IsNullOrEmpty(op[0].L8_GreenTariff_Charges))
            {
                GreenTariff.Visible = false;
                GreenTariffValue.Visible = false;
                GreenTariffHindi.Visible = false;

                GreenTariff.TopF = Subsidy.TopF;
                GreenTariffValue.TopF = Subsidy.TopF;
                GreenTariffHindi.TopF = Subsidy.TopF;
            }
            lblFPPA.TopF = GreenTariff.BottomF;
            FPPASurchargeValue.TopF = GreenTariff.BottomF;
            lblFPPAHindi.TopF = GreenTariff.BottomF;    

            if (op[0].L10_FPPASurcharge == "0.00" || string.IsNullOrEmpty(op[0].L10_FPPASurcharge))
            {
                lblFPPA.Visible = false;
                FPPASurchargeValue.Visible = false;
                lblFPPAHindi.Visible = false;

                lblFPPA.TopF = GreenTariff.TopF;
                FPPASurchargeValue.TopF = GreenTariff.TopF;
                lblFPPAHindi.TopF = GreenTariff.TopF;
            }

            if (op[0].L9_INT_TPL == "0.00" || string.IsNullOrEmpty(op[0].L9_INT_TPL))
            {
                bd_LatePaymentSurcharges.Visible = false;
                bd_LatePaymentSurchargesVALUE.Visible = false;
                LPSCHindi.Visible = false;
            }

            #endregion

            var messageFromFile = 0;

            #region File Messages

            if (!string.IsNullOrEmpty(op[0].L26_Message_1))
            {
                messageFromFile++;
                XRLabel xrMessage1 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L26_Message_1,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 2,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage1);
                adjustMessages(xrMessage1);

            }
            if (!string.IsNullOrEmpty(op[0].L27_Message_2))
            {
                messageFromFile++;
                XRLabel xrMessage2 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L27_Message_2,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 2,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage2);
                adjustMessages(xrMessage2);

            }
            if (!string.IsNullOrEmpty(op[0].L28_Message_3))
            {
                messageFromFile++;
                XRLabel xrMessage3 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L28_Message_3,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 2,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage3);
                adjustMessages(xrMessage3);

            }
            if (!string.IsNullOrEmpty(op[0].L29_Message_4))
            {
                messageFromFile++;
                XRLabel xrMessage4 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L29_Message_4,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 2,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage4);
                adjustMessages(xrMessage4);

            }
            if (!string.IsNullOrEmpty(op[0].L30_Message_5))
            {
                messageFromFile++;
                XRLabel xrMessage5 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L30_Message_5,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 2,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage5);
                adjustMessages(xrMessage5);

            }
            if (!string.IsNullOrEmpty(op[0].L31_Message_6))
            {
                messageFromFile++;
                XRLabel xrMessage6 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = op[0].L31_Message_6,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = 2,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage6);
                adjustMessages(xrMessage6);

            }
            if (!string.IsNullOrEmpty(op[0].L6_LT_Metering_Flag))
            {
                messageFromFile++;
                XRLabel xrMessage6 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = "*" + getMessage(LoadStaticData._EnglishMessage, "BRDCST3"),
                    WordWrap = true,
                    WidthF = xrPanel1.WidthF,
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

            if ((!string.IsNullOrEmpty(op[0].L6_EXCESS_DEMAND) && op[0].L6_EXCESS_DEMAND != "0.00(" + op[0].unit + ")") || ((!string.IsNullOrEmpty(op[0].L9_MessageIndication) && (op[0].L9_MessageIndication == "2"))))
            {
                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageExcessDemand = new XRLabel
                    {
                        Font = new DXFont("Noto Sans Devanagari", 8),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = getMessage(LoadStaticData._HindiMessage, "EDC"),
                        WordWrap = true,
                        WidthF = xrPanel1.WidthF,
                        KeepTogether = true,
                        HeightF = 1,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    };
                    xrPanel1.Controls.Add(xrMessageExcessDemand);
                    adjustMessages(xrMessageExcessDemand);
                }
            }

            if (!string.IsNullOrEmpty(op[0].L8_power_factor_adj) && op[0].L8_power_factor_adj != "0.00")
            {
                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageExcessDemand = new XRLabel
                    {
                        Font = new DXFont("Noto Sans Devanagari", 8),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = string.Format(getMessage(LoadStaticData._HindiMessage, "PFM"), "0.90".ToString()),
                        WordWrap = true,
                        WidthF = xrPanel1.WidthF,
                        KeepTogether = true,
                        HeightF = 1,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    };
                    xrPanel1.Controls.Add(xrMessageExcessDemand);
                    adjustMessages(xrMessageExcessDemand);
                }
            }
            if (!string.IsNullOrEmpty(op[0].L10_Theft_Amount) && op[0].L10_Theft_Amount != "0.00")
            {
                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageTheftAmount = new XRLabel
                    {
                        Font = new DXFont("Noto Sans Devanagari", 8),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = string.Format(getMessage(LoadStaticData._HindiMessage, "TFA"), op[0].L10_Theft_Amount),
                        WordWrap = true,
                        WidthF = xrPanel1.WidthF,
                        KeepTogether = true,
                        HeightF = 1,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    };


                    xrPanel1.Controls.Add(xrMessageTheftAmount);
                    adjustMessages(xrMessageTheftAmount);
                }
            }

            if (!string.IsNullOrEmpty(op[0].L10_TOTARR_UPPCL_INT_UPPCL_INTARR_UPPCL) && !Convert.ToDecimal(op[0].L10_TOTARR_UPPCL_INT_UPPCL_INTARR_UPPCL).Equals(decimal.Zero))
            {

                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageTheftAmount = new XRLabel
                    {
                        Font = new DXFont("Noto Sans Devanagari", 8),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = string.Format(getMessage(LoadStaticData._HindiMessage, "DAD"), op[0].L10_Theft_Amount),
                        WordWrap = true,
                        WidthF = xrPanel1.WidthF,
                        KeepTogether = true,
                        HeightF = 1,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    };


                    xrPanel1.Controls.Add(xrMessageTheftAmount);
                    adjustMessages(xrMessageTheftAmount);
                }
            }


            if (!string.IsNullOrEmpty(op[0].L9_MessageFlag))
            {
                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageExcessDemand = new XRLabel
                    {
                        Font = new DXFont("Noto Sans Devanagari", 8),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = getMessage(LoadStaticData._HindiMessage, "TPC"),
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = (float)0.01,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
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
                    Font = new DXFont("Noto Sans Devanagari", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopJustify,
                    Text = op[0].L33_MESSAGE7,
                    WordWrap = true,
                    AutoWidth = true,
                    WidthF = 410f,
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
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopJustify,
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
                    Font = new DXFont("Noto Sans Devanagari", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopJustify,
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
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopJustify,
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
                        Font = brdcstMsg.MessageType.ToUpper() == "ENG" ? new DXFont("Manrope", 8) : new DXFont("Noto Sans Devanagari", 8),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = brdcstMsg.MessageType.ToUpper() == "ENG" ? brdcstMsg.EnglishMessageString : brdcstMsg.HindiMessageString,
                        WordWrap = true,
                        WidthF = xrPanel1.WidthF,
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
            if ((string.IsNullOrEmpty(op[0].L10_SECDEPT_BDT) || Convert.ToDouble(op[0].L10_SECDEPT_BDT) == 0) && Convert.ToDouble(op[0].L6_SERVDET_SERVNO) < 674199999)
            {
                messageFromFile++;
                XRLabel xrMessage11 = new XRLabel
                {
                    Font = new DXFont("Noto Sans Devanagari", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = "अभिलेखों के अनुसार आपके संयोजन पर जमानत धनराशि शून्य अंकित है। यदि आपके द्वारा संयोजन \n" +
                    "राशि जमा की गई है तो उक्त जमानत राशि की मूल रसीद के साथ हमारे ग्राहक सेवा केंद्र पर संपर्क करें।",
                    WordWrap = false,
                    AutoWidth = true,
                    Multiline = true,
                    KeepTogether = true,
                    HeightF = 0.1f,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage11);
                adjustMessages(xrMessage11);

            }
            #endregion

            if (op[0].L9_INT_TPL == "0.00" || op[0].L9_INT_TPL == " ")
            {
                bd_LatePaymentSurcharges.Visible = false;
                bd_LatePaymentSurchargesVALUE.Visible = false;
                LPSCHindi.Visible = false;
            }

            #region Solar Export Energy Adjustment
            //Solar Export Energy Adjustment

            if (!(op[0].L8_Solar_Export_Energy == "0.00" || op[0].L8_Solar_Export_Energy == ""))
            {
              
                bd_SolarExportEnergy.TopF = lblFPPA.BottomF;
                bd_Solar_Export_Value.TopF = lblFPPA.BottomF;
                lblSolarExportHindi.TopF = lblFPPA.BottomF;
            }
            else
            {
                bd_SolarExportEnergy.Visible = false;
                bd_Solar_Export_Value.Visible = false;
                lblSolarExportHindi.Visible = false;
                bd_SolarExportEnergy.TopF = lblFPPA.TopF;
                bd_Solar_Export_Value.TopF = FPPASurchargeValue.TopF; 
                lblSolarExportHindi.TopF = lblFPPA.TopF;
            }
            #endregion
        }


        #endregion



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
        #endregion
    }

}
