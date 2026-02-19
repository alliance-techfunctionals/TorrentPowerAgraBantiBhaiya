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

            xrlContractDemand.Text = "Contract Demand(" + op[0].L6_MEASURE_OF_CONTRACT_Demand + ")";
            
            if (op[0].L6_MEASURE_OF_CONTRACT_Demand == "HP")
            {

                if (op[0].L6_Kvah_indicator == "1")
                {
                    
                    op[0].unit1 = "KVA";

                    xrlRecordedDemand.Text = "Recorded Demand(" + op[0].unit1 + ")";
                    xrlL6Servdet_Sanc_load.Text = "Bill Demand(" + op[0].unit1 + ")";
                    xrlL6ExcessDemand.Text = "Excess Demand(" + op[0].unit1 + ")";
                    KW_HEAD1.Text = op[0].unit1;
                    KWH_HEAD1.Text = op[0].unit1 + "H";
                    KW_HEAD2.Text = op[0].unit1;
                    KWH_HEAD2.Text = op[0].unit1 + "H";
                }
                else
                {
                    op[0].unit1 = "KW";

                    xrlRecordedDemand.Text = "Recorded Demand(" + op[0].unit1 + ")";
                    xrlL6ExcessDemand.Text = "Excess Demand(" + op[0].unit1 + ")";
                    xrlL6Servdet_Sanc_load.Text = "Bill Demand(" + op[0].unit1 + ")";
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

                    xrlRecordedDemand.Text = "Recorded Demand(" + op[0].unit1 + ")";
                    xrlL6ExcessDemand.Text = "Excess Demand(" + op[0].unit1 + ")";
                    xrlL6Servdet_Sanc_load.Text = "Bill Demand(" + op[0].unit1 + ")";
                    KW_HEAD1.Text = op[0].unit1;
                    KWH_HEAD1.Text = op[0].unit1 + "H";
                    KW_HEAD2.Text = op[0].unit1;
                    KWH_HEAD2.Text = op[0].unit1 + "H";
                }
                else
                {
                    op[0].unit1 = "KW";

                    xrlRecordedDemand.Text = "Recorded Demand(" + op[0].unit1 + ")";
                    xrlL6ExcessDemand.Text = "Excess Demand(" + op[0].unit1 + ")";
                    xrlL6Servdet_Sanc_load.Text = "Bill Demand(" + op[0].unit1 + ")";
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

                    xrlRecordedDemand.Text = "Recorded Demand(" + op[0].unit1 + ")";
                    xrlL6ExcessDemand.Text = "Excess Demand(" + op[0].unit1 + ")";
                    xrlL6Servdet_Sanc_load.Text = "Bill Demand(" + op[0].unit1 + ")";
                    KW_HEAD1.Text = op[0].unit1;
                    KWH_HEAD1.Text = op[0].unit1 + "H";
                    KW_HEAD2.Text = op[0].unit1;
                    KWH_HEAD2.Text = op[0].unit1 + "H";
                }
                else
                {
                    op[0].unit1 = "KW";

                    xrlRecordedDemand.Text = "Recorded Demand(" + op[0].unit1 + ")";
                    xrlL6ExcessDemand.Text = "Excess Demand(" + op[0].unit1 + ")";
                    xrlL6Servdet_Sanc_load.Text = "Bill Demand(" + op[0].unit1 + ")";
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

            if (op[0].L9_INT_TPL == "0.00" || op[0].L9_INT_TPL == " ")
            {
                bd_LatePaymentSurcharges.Visible = false;
                bd_LatePaymentSurchargesVALUE.Visible = false;
                bd_LatePaymentSurcharges.TopF = bd_LatePaymentSurcharges.TopF;
                bd_LatePaymentSurchargesVALUE.TopF = bd_LatePaymentSurchargesVALUE.TopF;
                bd_TotalDues.TopF = bd_TotalDues.TopF;
                bd_TotalDuesVALUE.TopF = bd_TotalDuesVALUE.TopF;
            }
        
            #region  LF Fector
            if (op[0].L6_MTRDET_LF_PERC == "0.00" || op[0].L6_MTRDET_LF_PERC == " " || op[0].L6_MTRDET_LF_PERC == "0")
            {
                Load_Factor.Visible = false;
              xrLabel7.Visible = false;   

            }

            #endregion

           

            #region Meter Change
            //Meter Change Print
            if (op[0].L12_MTRSNO_METER_2_IF_AVAILABLE != "")
            {
                xrLabel2.Text = op[0].L12_MTRSNO_METER_2_IF_AVAILABLE;//older
                xrLabel18.Text = op[0].L12_MTRSNO_METER1;

                if (string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "0.00" )
                {    //mtr1
                    imp11.Text = op[0].L14_KVA_PASTREAD;
                    exp11.Text = op[0].L33_Exp_Past_KW_UNITS;
                    imp21.Text = op[0].L14_KWH_PASTREAD;
                    exp21.Text = op[0].L33_Exp_Past_KWH_UNITS;

                    imp12.Text = op[0].L13_KVA_PRESREAD;
                    exp12.Text = op[0].L33_Exp_Present_KW_UNITS;
                    imp22.Text = op[0].L13_KWH_PRESREAD;
                    exp22.Text = op[0].L33_Exp_Present_KWH_UNITS;

                    imp13.Text = op[0].L15_Multiplying_factor_KVA;
                    exp13.Text = op[0].L15_Multiplying_factor_KVA;
                    imp23.Text = op[0].L15_Multiplying_factor_KWH;
                    exp23.Text = op[0].L15_Multiplying_factor_KWH;

                    imp14.Text = op[0].L16_KVA_UNITS;
                    exp14.Text = op[0].L33_Exp_KW_UNITS;
                    imp24.Text = op[0].L16_KWH_UNITS;
                    exp24.Text = op[0].L33_Exp_KWH_UNITS;

                    kva11.Text = op[0].L33_Exp_CURRENT_NET_EXPORT_KWH_UNITS;
                    kva12.Text = op[0].L46_Previous_CREDIT_Units_MAIN_KWH;
                    kva13.Text = op[0].L46_Net_Billed_Units_MAIN_KWH;
                    kva14.Text = op[0].L46_Carry_Forward_Units_MAIN_KWH;

                    //mtr2
                    MTR2_PR1.Text = op[0].L18_KVA_PASTREAD;
                    MTR2_PR2.Text = op[0].L49_Exp_Past_KVA_UNITS;
                    MTR2_PR3.Text = op[0].L18_KWH_PASTREAD;
                    MTR2_PR4.Text = op[0].L49_Exp_Past_KWH_UNITS;

                    MTR2_CR1.Text = op[0].L17_KVA_PRESREAD;
                    MTR2_CR2.Text = op[0].L49_Exp_Present_KVA_UNITS;
                    MTR2_CR3.Text = op[0].L17_KWH_PRESREAD;
                    MTR2_CR4.Text = op[0].L49_Exp_Present_KWH_UNITS;

                    MTR2_MF1.Text = op[0].L19_Multiplying_factor_KW;
                    MTR2_MF2.Text = op[0].L19_Multiplying_factor_KW;
                    MTR2_MF3.Text = op[0].L19_Multiplying_factor_KWH;
                    MTR2_MF4.Text = op[0].L19_Multiplying_factor_KWH;

                    MTR2_CU1.Text = op[0].L20_KVA_UNITS;
                    MTR2_CU2.Text = op[0].L49_Exp_KVA_UNITS;
                    MTR2_CU3.Text = op[0].L20_KWH_UNITS;
                    MTR2_CU4.Text = op[0].L49_Exp_KWH_UNITS;

                    kvah21.Text = op[0].L49_Exp_CURRENT_NET_EXPORT_KWH_UNITS;
                   
                }

                if (!string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "1")
                {
                    //MTR1
                    imp11.Text = op[0].L14_KVA_PASTREAD;
                    exp11.Text = op[0].L33_Exp_Past_KVA_UNITS;
                    imp21.Text = op[0].L14_KWH_PASTREAD;
                    exp21.Text = op[0].L33_Exp_Past_KVAH_UNITS;

                    imp12.Text = op[0].L13_KVA_PRESREAD;
                    exp12.Text = op[0].L33_Exp_Present_KVA_UNITS;
                    imp22.Text = op[0].L13_KWH_PRESREAD;
                    exp22.Text = op[0].L33_Exp_Present_KVAH_UNITS;

                    imp13.Text = op[0].L15_Multiplying_factor_KVA;
                    exp13.Text = op[0].L15_Multiplying_factor_KVA;
                    imp23.Text = op[0].L15_Multiplying_factor_KWH;
                    exp23.Text = op[0].L15_Multiplying_factor_KWH;

                    imp14.Text = op[0].L16_KVA_UNITS;
                    exp14.Text = op[0].L33_Exp_KVA_UNITS;
                    imp24.Text = op[0].L16_KWH_UNITS;
                    exp24.Text = op[0].L33_Exp_KVAH_UNITS;

                    kva11.Text = op[0].L33_Exp_CURRENT_NET_EXPORT_KVAH_UNITS;
                    kva12.Text = op[0].L46_Previous_CREDIT_Units_MAIN_KVAH;
                    kva13.Text = op[0].L46_Net_Billed_Units_MAIN;
                    kva14.Text = op[0].L46_Carry_Forward_Units_MAIN_KVAH;

                    //MTR2
                    MTR2_PR1.Text = op[0].L18_KVA_PASTREAD;
                    MTR2_PR2.Text = op[0].L49_Exp_Past_KVA_UNITS;
                    MTR2_PR3.Text = op[0].L18_KWH_PASTREAD;
                    MTR2_PR4.Text = op[0].L49_Exp_Past_KVAH_UNITS;

                    MTR2_CR1.Text = op[0].L17_KVA_PRESREAD;
                    MTR2_CR2.Text = op[0].L49_Exp_Present_KVA_UNITS;
                    MTR2_CR3.Text = op[0].L17_KWH_PRESREAD;
                    MTR2_CR4.Text = op[0].L49_Exp_Present_KVAH_UNITS;

                    MTR2_MF1.Text = op[0].L19_Multiplying_factor_KW;
                    MTR2_MF2.Text = op[0].L19_Multiplying_factor_KW;
                    MTR2_MF3.Text = op[0].L19_Multiplying_factor_KWH;
                    MTR2_MF4.Text = op[0].L19_Multiplying_factor_KWH;

                    MTR2_CU1.Text = op[0].L20_KVA_UNITS;
                    MTR2_CU2.Text = op[0].L49_Exp_KVA_UNITS;
                    MTR2_CU3.Text = op[0].L20_KWH_UNITS;
                    MTR2_CU4.Text = op[0].L49_Exp_KVAH_UNITS;

                    kvah21.Text = op[0].L49_Exp_CURRENT_NET_EXPORT_KVAH_UNITS;
                }


            }
            else
            {
                if (string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "0.00")
                {
                    xrLabel18.Text = op[0].L12_MTRSNO_METER1;
                    KW_HEAD2.Visible = false;
                    KWH_HEAD2.Visible = false;
                    MTR2_IMP1.Visible = false;
                    MTR2_IMP2.Visible = false;
                    MTR2_EXP1.Visible = false;
                    MTR2_EXP2.Visible = false;
                    //Newer                                                   
                    imp11.Text = op[0].L14_KVA_PASTREAD;
                    exp11.Text = op[0].L33_Exp_Past_KW_UNITS;
                    imp21.Text = op[0].L14_KWH_PASTREAD;
                    exp21.Text = op[0].L33_Exp_Past_KWH_UNITS;

                    imp12.Text = op[0].L13_KVA_PRESREAD;
                    exp12.Text = op[0].L33_Exp_Present_KW_UNITS;
                    imp22.Text = op[0].L13_KWH_PRESREAD;
                    exp22.Text = op[0].L33_Exp_Present_KWH_UNITS;

                    imp13.Text = op[0].L15_Multiplying_factor_KVA;
                    exp13.Text = op[0].L15_Multiplying_factor_KVA;
                    imp23.Text = op[0].L15_Multiplying_factor_KWH;
                    exp23.Text = op[0].L15_Multiplying_factor_KWH;

                    imp14.Text = op[0].L16_KVA_UNITS;
                    exp14.Text = op[0].L33_Exp_KW_UNITS;
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
                    MTR2_IMP1.Visible = false;
                    MTR2_IMP2.Visible = false;
                    MTR2_EXP1.Visible = false;
                    MTR2_EXP2.Visible = false;
                    //Newer
                    imp11.Text = op[0].L14_KVA_PASTREAD;
                    exp11.Text = op[0].L33_Exp_Past_KVA_UNITS;
                    imp21.Text = op[0].L14_KWH_PASTREAD;
                    exp21.Text = op[0].L33_Exp_Past_KVAH_UNITS;

                    imp12.Text = op[0].L13_KVA_PRESREAD;
                    exp12.Text = op[0].L33_Exp_Present_KVA_UNITS;
                    imp22.Text = op[0].L13_KWH_PRESREAD;
                    exp22.Text = op[0].L33_Exp_Present_KVAH_UNITS;

                    imp13.Text = op[0].L15_Multiplying_factor_KVA;
                    exp13.Text = op[0].L15_Multiplying_factor_KVA;
                    imp23.Text = op[0].L15_Multiplying_factor_KWH;
                    exp23.Text = op[0].L15_Multiplying_factor_KWH;

                    imp14.Text = op[0].L16_KVA_UNITS;
                    exp14.Text = op[0].L33_Exp_KVA_UNITS;
                    imp24.Text = op[0].L16_KWH_UNITS;
                    exp24.Text = op[0].L33_Exp_KVAH_UNITS;

                    kva11.Text = op[0].L33_Exp_CURRENT_NET_EXPORT_KVAH_UNITS;
                    kva12.Text = op[0].L46_Previous_CREDIT_Units_MAIN_KVAH;
                    kva13.Text = op[0].L46_Net_Billed_Units_MAIN;
                    kva14.Text = op[0].L46_Carry_Forward_Units_MAIN_KVAH;
                }

            }
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
                        xrlContractDemand.Text = "Contract Demand(" + op[0].L6_MEASURE_OF_CONTRACT_Demand + ")";
                    }
                    else if (op[0].L6_MEASURE_OF_CONTRACT_Demand.ToUpper() == "HP" && Math.Ceiling(contractdemand) >= (decimal)13.4)
                    {
                        op[0].L6_MEASURE_OF_CONTRACT_Demand = op[0].L6_MEASURE_OF_CONTRACT_Demand + "/KVA";
                        xrlContractDemand.Text = "Contract Demand(" + op[0].L6_MEASURE_OF_CONTRACT_Demand + ")";
                    }
                }
            }

           
            #region Solar Export Energy Adjustment
            //Solar Export Energy Adjustment

            if (!(op[0].L8_Solar_Export_Energy == "0.00" || op[0].L8_Solar_Export_Energy == ""))
            {
                xrLabel34.Visible = false;
                xrLabel33.Visible = false;
                xrLabel35.Visible = false;
                xrLabel36.Visible = false;
                kvah21.Visible = false;
                kva11.Visible = false;
                kva12.Visible = false;
                kva13.Visible = false;
                kva14.Visible = false;
                Rpt_LTMD_Solar_back_visible?.visible();

                bd_SolarExportEnergy.TopF = bd_TotalCurrentDues.TopF;
                bd_Solar_Export_Value.TopF = bd_TotalCurrentDuesValues.TopF;
                bd_TotalCurrentDues.TopF = bd_Arrears.TopF;
                bd_TotalCurrentDuesValues.TopF = bd_Arrears_values.TopF;
                bd_Arrears.TopF = bd_TotalDues.TopF;
                bd_Arrears_values.TopF = bd_TotalDuesVALUE.TopF;
                bd_TotalDues.TopF = bd_TotalDues.BottomF;
                bd_TotalDuesVALUE.TopF = bd_TotalDuesVALUE.BottomF;

            }
            else
            {
                Rpt_LTMD_Solar_back_visible?.visibleon();
                bd_SolarExportEnergy.Visible = false;
                bd_Solar_Export_Value.Visible = false;
                bd_SolarExportEnergy.TopF = lblFPPA.TopF;
                bd_Solar_Export_Value.TopF = FPPASurchargeValue.TopF;
            }
            #endregion
        }

     
    }

}

