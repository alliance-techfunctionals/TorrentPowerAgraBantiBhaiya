using AT.Print.Utils;
using DevExpress.Drawing;
using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AT.Print.PDF
{
    public partial class rpt_LT_Back : DevExpress.XtraReports.UI.XtraReport
    {
        public rpt_LT_Back()
        {
            InitializeComponent();
        }

        private void Rpt_LT_Back_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var Data = this.DataSource as List<SingleLTBill>;

            #region RISC1 Change
            if (Data[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 5A") || Data[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 5B") || Data[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 1B") || Data[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 1C"))
            {
                bd_RlSC1Rate.Text = "@ 1.14%";

            }
            #endregion

            xrLabel40.BringToFront();
            xrLabel21.BringToFront();
            xrLabel78.BringToFront();
            xrLabel41.BringToFront();
            xrLabel35.BringToFront();
            xrLabel134.BringToFront();
            xrLabel75.BringToFront();



            #region Bill Details

            //Excess Demand Surcharge Print
            if (Data[0].L10_DmdChgPenalty == "0.00" || string.IsNullOrEmpty(Data[0].L10_DmdChgPenalty))
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

            if (Data[0].L8_ACCharge == "0.00" || string.IsNullOrEmpty(Data[0].L8_ACCharge))
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


            if (Data[0].L8_PowerFactorAdj == "0.00" || string.IsNullOrEmpty(Data[0].L8_PowerFactorAdj))
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
            if (Data[0].L8_MinCharge == "0.00" || string.IsNullOrEmpty(Data[0].L8_MinCharge))
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
            if (Data[0].L8_ServdetTotbBdtOthr == "0.00" || string.IsNullOrEmpty(Data[0].L8_ServdetTotbBdtOthr))
            {
                bd_OtherCharges.Visible = false;
                bd_OtherChargesHindi.Visible = false;
                bd_OtherChargesValue.Visible = false;

                bd_OtherCharges.TopF = bd_RlSC2.TopF;
                bd_OtherChargesHindi.TopF = bd_RlSC2.TopF;
                bd_OtherChargesValue.TopF = bd_RlSC2.TopF;




            }
            Subsidy.TopF = bd_OtherCharges.BottomF;
            SubsidyHindi.TopF = bd_OtherChargesHindi.BottomF;
            SubsidyValue.TopF = bd_OtherChargesValue.BottomF;

            if (Data[0].L8_Subsidy_Charges == "0.00" || string.IsNullOrEmpty(Data[0].L8_Subsidy_Charges))
            {
                Subsidy.Visible = false;
                SubsidyHindi.Visible = false;
                SubsidyValue.Visible = false;

                Subsidy.TopF = bd_OtherCharges.TopF;
                SubsidyHindi.TopF = bd_OtherCharges.TopF;
                SubsidyValue.TopF = bd_OtherCharges.TopF;

            }
            GreenTariff.TopF = Subsidy.BottomF;
            GreenTariffHindi.TopF = Subsidy.BottomF;
            GreenTariffValue.TopF = Subsidy.BottomF;

            if (Data[0].L8_GreenTariff_Charges == "0.00" || string.IsNullOrEmpty(Data[0].L8_GreenTariff_Charges))
            {
                GreenTariff.Visible = false;
                GreenTariffHindi.Visible = false;
                GreenTariffValue.Visible = false;

                GreenTariff.TopF = Subsidy.TopF;
                GreenTariffHindi.TopF = Subsidy.TopF;
                GreenTariffValue.TopF = Subsidy.TopF;

            }
            lblFPPA.TopF = GreenTariff.BottomF;
            lblFPPAHindi.TopF = GreenTariff.BottomF;
            FPPASurchargeValue.TopF = GreenTariff.BottomF;

            if (Data[0].L10_FPPASurcharge == "0.00" || string.IsNullOrEmpty(Data[0].L10_FPPASurcharge))
            {
                lblFPPA.Visible = false;
                lblFPPAHindi.Visible = false;
                FPPASurchargeValue.Visible = false;


            }
            float lastBottomF = 0;
            int visibleCount = 0;
            if (bd_ExcessDemandCharges.Visible)
            {
                lastBottomF = bd_ExcessDemandCharges.BottomF;
                visibleCount++;
            }
            if (bd_AcCharge.Visible)
            {
                lastBottomF = bd_AcCharge.BottomF;
                visibleCount++;
            }
            if (bdPowerFactorCharges.Visible)
            {
                lastBottomF = bdPowerFactorCharges.BottomF;
                visibleCount++;
            }

            if (bd_AdjustmentMinimumCharges.Visible)
            {
                lastBottomF = bd_AdjustmentMinimumCharges.BottomF;
                visibleCount++;
            }

            if (bd_OtherCharges.Visible)
            {
                lastBottomF = bd_OtherCharges.BottomF;
                visibleCount++;
            }
            if (Subsidy.Visible)
            {
                lastBottomF = Subsidy.BottomF;
                visibleCount++;
            }
            if (GreenTariff.Visible)
            {
                lastBottomF = GreenTariff.BottomF;
                visibleCount++;
            }
            if (lblFPPA.Visible)
            {
                lastBottomF = lblFPPA.BottomF;
                visibleCount++;
            }
            if (visibleCount >= 6)
            {
                xrLabel35.TopF = lastBottomF;
                xrLabel34.TopF = lastBottomF;
                xrLabel75.TopF = lastBottomF;
                xrLabel2.TopF = lastBottomF;

            }



            //Late Payment Surcharge
            if (Data[0].L9_Int_Tpl == "0.00" || string.IsNullOrEmpty(Data[0].L9_Int_Tpl))
            {
                LPSC.Visible = false;
                LPSCHindi.Visible = false;
                LPSCValue.Visible = false;
            }
            #endregion

            var messageFromFile = 0;

            #region File Messages

            if (!string.IsNullOrEmpty(Data[0].L22_MESSAGE1))
            {
                messageFromFile++;
                XRLabel xrMessage1 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = Data[0].L22_MESSAGE1,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = (float)0.25,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage1);
                adjustMessages(xrMessage1);

            }
            if (!string.IsNullOrEmpty(Data[0].L23_MESSAGE2))
            {
                messageFromFile++;
                XRLabel xrMessage2 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = Data[0].L23_MESSAGE2,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = (float)0.25,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage2);
                adjustMessages(xrMessage2);

            }
            if (!string.IsNullOrEmpty(Data[0].L24_MESSAGE3))
            {
                messageFromFile++;
                XRLabel xrMessage3 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = Data[0].L24_MESSAGE3,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = (float)0.25,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage3);
                adjustMessages(xrMessage3);

            }
            if (!string.IsNullOrEmpty(Data[0].L25_MESSAGE4))
            {
                messageFromFile++;
                XRLabel xrMessage4 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = Data[0].L25_MESSAGE4,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = (float)0.25,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage4);
                adjustMessages(xrMessage4);

            }
            if (!string.IsNullOrEmpty(Data[0].L26_MESSAGE5))
            {
                messageFromFile++;
                XRLabel xrMessage5 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = Data[0].L26_MESSAGE5,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = (float)0.25,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage5);
                adjustMessages(xrMessage5);

            }
            if (!string.IsNullOrEmpty(Data[0].L27_MESSAGE6))
            {
                messageFromFile++;
                XRLabel xrMessage6 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = Data[0].L27_MESSAGE6,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = (float)0.25,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage6);
                adjustMessages(xrMessage6);

            }
            if (!string.IsNullOrEmpty(Data[0].L28_MESSAGE7))
            {
                messageFromFile++;
                XRLabel xrMessage7 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = Data[0].L28_MESSAGE7,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = (float)0.25,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage7);
                adjustMessages(xrMessage7);

            }
            if (!string.IsNullOrEmpty(Data[0].L29_MESSAGE8))
            {
                messageFromFile++;
                XRLabel xrMessage8 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = Data[0].L29_MESSAGE8,
                    WordWrap = false,
                    AutoWidth = true,
                    KeepTogether = true,
                    HeightF = (float)0.25,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                };
                xrPanel1.Controls.Add(xrMessage8);
                adjustMessages(xrMessage8);

            }
            if (!string.IsNullOrEmpty(Data[0].L30_MESSAGE9))
            {
                if (!IsMessageLimitExceeds(messageFromFile))
                {
                    messageFromFile++;
                    XRLabel xrMessage9 = new XRLabel
                    {
                        Font = new DXFont("Manrope", 8),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = Data[0].L30_MESSAGE9,
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = (float)0.25,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    };
                    xrPanel1.Controls.Add(xrMessage9);
                    adjustMessages(xrMessage9);

                }
            }
            if (!string.IsNullOrEmpty(Data[0].L31_MESSAGE10))
            {
                if (!IsMessageLimitExceeds(messageFromFile))
                {
                    messageFromFile++;
                    XRLabel xrMessage10 = new XRLabel
                    {
                        Font = new DXFont("Manrope", 8),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = Data[0].L31_MESSAGE10,
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = (float)0.25,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    };
                    xrPanel1.Controls.Add(xrMessage10);
                    adjustMessages(xrMessage10);

                }
            }
            #endregion

            #region Custom Messages
            var totalMessages = messageFromFile;

            if ((!string.IsNullOrEmpty(Data[0].L6_EXCESS_DEMAND) && Data[0].L6_EXCESS_DEMAND != "0.00(KW)") || ((!string.IsNullOrEmpty(Data[0].L9_MessageIndication) && (Data[0].L9_MessageIndication == "2"))))
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
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = 0.1f,
                        WidthF = xrPanel1.WidthF,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    };
                    xrPanel1.Controls.Add(xrMessageExcessDemand);
                    adjustMessages(xrMessageExcessDemand);
                }
            }


            if (!string.IsNullOrEmpty(Data[0].L8_PowerFactorAdj) && Data[0].L8_PowerFactorAdj != "0.00")
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
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = 0.1f,
                        WidthF = xrPanel1.WidthF,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),

                    };
                    xrPanel1.Controls.Add(xrMessageExcessDemand);
                    adjustMessages(xrMessageExcessDemand);
                }
            }

            if (!string.IsNullOrEmpty(Data[0].L1_DisconnectionMSGPrintingIMMEDIATE) && Data[0].L1_DisconnectionMSGPrintingIMMEDIATE != "0")
            {
                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageDisconnection = new XRLabel
                    {
                        Font = new DXFont("Noto Sans Devanagari", 8),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = getMessage(LoadStaticData._HindiMessage, "IDC"),
                        WordWrap = true,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = 0.1f,
                        WidthF = xrPanel1.WidthF,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),

                    };
                    xrPanel1.Controls.Add(xrMessageDisconnection);
                    adjustMessages(xrMessageDisconnection);
                }
            }

            if (!string.IsNullOrEmpty(Data[0].L10_TheftAmount) && Data[0].L10_TheftAmount != "0.00")
            {
                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageTheftAmount = new XRLabel
                    {
                        Font = new DXFont("Noto Sans Devanagari", 8),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = string.Format(getMessage(LoadStaticData._HindiMessage, "TFA"), Data[0].L10_TheftAmount),
                        WordWrap = true,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = 0.1f,
                        WidthF = xrPanel1.WidthF,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),

                    };


                    xrPanel1.Controls.Add(xrMessageTheftAmount);
                    adjustMessages(xrMessageTheftAmount);
                }
            }

            if (!string.IsNullOrEmpty(Data[0].L10_TotArrUPPCLIntUPPCLIntArrUPPCL) && !Convert.ToDecimal(Data[0].L10_TotArrUPPCLIntUPPCLIntArrUPPCL).Equals(decimal.Zero))
            {

                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageTheftAmount = new XRLabel
                    {
                        Font = new DXFont("Noto Sans Devanagari", 8),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = string.Format(getMessage(LoadStaticData._HindiMessage, "DAD"), Data[0].L10_TheftAmount),
                        WordWrap = true,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = 0.1f,
                        WidthF = xrPanel1.WidthF,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),

                    };


                    xrPanel1.Controls.Add(xrMessageTheftAmount);
                    adjustMessages(xrMessageTheftAmount);
                }
            }

            if (!string.IsNullOrEmpty(Data[0].L1_BillingCode))
            {
                if (Data[0].L1_BillingCode == "3000")
                {
                    if (!IsMessageLimitExceeds(totalMessages))
                    {
                        totalMessages++;
                        decimal totalUnits = decimal.Zero;
                        if (!string.IsNullOrEmpty(Data[0].L16_M1_KWH_UNITS))
                        {
                            totalUnits += Convert.ToDecimal(Data[0].L16_M1_KWH_UNITS);
                        }

                        if (!string.IsNullOrEmpty(Data[0].L20_M2_KWH_UNITS))
                        {
                            totalUnits += Convert.ToDecimal(Data[0].L20_M2_KWH_UNITS);
                        }
                        var PrevReadDt = ParseAsDataTable.ChangeMonthToHindi(Data[0].L7_PrevReadDt);
                        var ReadDt = ParseAsDataTable.ChangeMonthToHindi(Data[0].L7_ReaDt);

                        XRLabel xrMessageTheftAmount = new XRLabel
                        {
                            Font = new DXFont("Noto Sans Devanagari", 8),
                            TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                            Text = string.Format(getMessage(LoadStaticData._HindiMessage, "AB1"), totalUnits.ToString(), PrevReadDt, ReadDt, Data[0].L10_Mode + " दिन"),
                            WordWrap = true,
                            AutoWidth = true,
                            KeepTogether = true,
                            HeightF = 0.1f,
                            WidthF = xrPanel1.WidthF,
                            Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                        };


                        xrPanel1.Controls.Add(xrMessageTheftAmount);
                        adjustMessages(xrMessageTheftAmount);

                        XRLabel xrAB2Msg = new XRLabel
                        {
                            Font = new DXFont("Noto Sans Devanagari", 8),
                            TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                            Text = string.Format(getMessage(LoadStaticData._HindiMessage, "AB2")),
                            WordWrap = true,
                            AutoWidth = true,
                            KeepTogether = true,
                            HeightF = 0.1f,
                            WidthF = xrPanel1.WidthF,
                            Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                        };


                        xrPanel1.Controls.Add(xrAB2Msg);
                        adjustMessages(xrAB2Msg);


                    }
                }
            }

            if (!string.IsNullOrEmpty(Data[0].L9_MessageFlag))
            {
                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageExcessDemand = new XRLabel
                    {
                        Font = new DXFont("Noto Sans Devanagari", 8),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = getMessage(LoadStaticData._HindiMessage, "TPC"),
                        WordWrap = true,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = 0.1f,
                        WidthF = xrPanel1.WidthF,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),

                    };
                    xrPanel1.Controls.Add(xrMessageExcessDemand);
                    adjustMessages(xrMessageExcessDemand);
                }
            }
            #endregion

            #region Template Messages
            if (!string.IsNullOrEmpty(Data[0].L33_MESSAGE7))
            {
                messageFromFile++;
                XRLabel xrMessage7 = new XRLabel
                {
                    Font = new DXFont("Noto Sans Devanagari", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopJustify,
                    Text = Data[0].L33_MESSAGE7,
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
            if (!string.IsNullOrEmpty(Data[0].L34_MESSAGE8))
            {
                messageFromFile++;
                XRLabel xrMessage8 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopJustify,
                    Text = Data[0].L34_MESSAGE8,
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
            if (!string.IsNullOrEmpty(Data[0].L35_MESSAGE9))
            {
                messageFromFile++;
                XRLabel xrMessage9 = new XRLabel
                {
                    Font = new DXFont("Noto Sans Devanagari", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopJustify,
                    Text = Data[0].L35_MESSAGE9,
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
            if (!string.IsNullOrEmpty(Data[0].L36_MESSAGE10))
            {
                messageFromFile++;
                XRLabel xrMessage10 = new XRLabel
                {
                    Font = new DXFont("Manrope", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopJustify,
                    Text = Data[0].L36_MESSAGE10,
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
                if (LoadStaticData._BroadcastMessage.FindAll(x => x.ServiceNo.ToUpper().Equals(Data[0].L6_SERVDET_SERVNO)).FirstOrDefault() != null)
                {
                    BroadcastMessage brdcstMsg = LoadStaticData._BroadcastMessage.FindAll(x => x.ServiceNo.ToUpper().Equals(Data[0].L6_SERVDET_SERVNO)).FirstOrDefault();
                    totalMessages++;


                    XRLabel xrMessageTheftAmount = new XRLabel
                    {


                        Font = brdcstMsg.MessageType.ToUpper() == "ENG" ? new DXFont("Manrope", 8) : new DXFont("Noto Sans Devanagari", 8),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = brdcstMsg.MessageType.ToUpper() == "ENG" ? brdcstMsg.EnglishMessageString : brdcstMsg.HindiMessageString,
                        WordWrap = false,
                        AutoWidth = true,
                        KeepTogether = true,
                        HeightF = (float)0.25,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    };

                    xrPanel1.Controls.Add(xrMessageTheftAmount);
                    adjustMessages(xrMessageTheftAmount);
                }
            }
            #endregion

            #region Security Deposit Message   
            if ((string.IsNullOrEmpty(Data[0].L10_SecDeptBdt) || Convert.ToDouble(Data[0].L10_SecDeptBdt) == 0) && Convert.ToDouble(Data[0].L6_SERVDET_SERVNO) < 674199999)
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
            xrPictureBox2.ImageUrl = Application.StartupPath + "\\Contents\\CategorySlabImages\\" + Data[0].L6_TARIFF_DESCR + ".png";
            xrPictureBox1.ImageUrl = Application.StartupPath + "\\Contents\\CategorySlabImages\\PromotionImage.png";

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
                    lbl.TopF = (plbl.BottomF - 0.96f);
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

