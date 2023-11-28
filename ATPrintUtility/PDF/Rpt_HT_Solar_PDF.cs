using AT.Print.Utils;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ZXing;

namespace AT.Print.PDF
{
    public partial class Rpt_HT_Solar_PDF : DevExpress.XtraReports.UI.XtraReport
    {
        public Rpt_HT_Solar_back_PDF Rpt_HT_Solar_PDF_visible;
        public Rpt_HT_Solar_PDF(Rpt_HT_Solar_back_PDF d)
        {
            InitializeComponent();
            Rpt_HT_Solar_PDF_visible = d;
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
        private void Rpt_solar_PDF_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = sender as Rpt_HT_Solar_PDF;
            var op = data.DataSource as List<Solar_Bill_HT>;

            #region RISC1 Change
            if (op[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 5A") || op[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 5B") || op[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 1B") || op[0].L6_TARIFF_DESCR.ToUpper().Equals("LMV 1C"))
            {
                bd_RlSC1Rate.Text = "@ 1.14%";

            }
            #endregion
            #region QRCODE
            if (ConfigurationManager.AppSettings["generateQRCodeinSolarHTBills"].ToString() == "True")
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
                xrQRCODE.SizeF = new System.Drawing.SizeF(52, 52);
            }
            #endregion

            if (!string.IsNullOrEmpty(op[0].L1_Customer_PAN))
            {
                xrLabel5.Visible = true;
            }
            else
            {
                xrLabel5.Visible = false;
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
                xrLabel5.TopF = xrLabel142.TopF;
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
            xrlContractDemand.Text = "Contract Demand(" + unit + ")";

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
            else if(op[0].L6_MEASURE_OF_CONTRACT_Demand == "KW")
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
                    KW_HEAD1.Text = op[0].unit1;
                    KWH_HEAD1.Text = op[0].unit1 + "H";
                    KW_HEAD2.Text = op[0].unit1;
                    KWH_HEAD2.Text = op[0].unit1 + "H";
                }
            }
            else if(op[0].L6_MEASURE_OF_CONTRACT_Demand == "KVA")
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



            #region  LF Fector
            if (op[0].L6_MTRDET_LF_PERC == "0.00" || op[0].L6_MTRDET_LF_PERC == " " || op[0].L6_MTRDET_LF_PERC == "0")
             {
                Load_Factor.Visible = false;
                xrLabel7.Visible = false;
               // SolarLoad.TopF = xrLabel7.TopF;
               // SolarLoadValue. TopF = Load_Factor.TopF;
           
            }
            //if (op[0].L46_SolarLoad == "0.00" || string.IsNullOrEmpty(op[0].L46_SolarLoad))
            //{
            //    //SolarLoad.Visible = false;
            //    //SolarLoadValue.Visible = false;
            //}

            #endregion

            #region Meter Change
            //Meter Change Print
            if (op[0].L11_MTRSNO_METER_2_IF_AVAILABLE != "")
            {
                MTR1.Text = op[0].L11_MTRSNO_METER_2_IF_AVAILABLE;//older
                MTR2.Text = op[0].L11_MTRSNO_METER1;//Newer


                if (string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "0.00")
                {
                    //For Mtr 1
                    imp11.Text = op[0].L13_KVA_PASTREAD;
                    imp12.Text = op[0].L12_KVA_PRESREAD;
                    imp13.Text = op[0].L14_Multiplying_factor_KVA;
                    imp14.Text = op[0].L15_KVA_UNITS;

                    exp11.Text = op[0].L33_Exp_Past_KW_UNITS;
                    exp12.Text = op[0].L33_Exp_Present_KW_UNITS;
                    exp13.Text = op[0].L14_Multiplying_factor_KVA;
                    exp14.Text = op[0].L33_Exp_KW_UNITS;

                    //FOR_KWH             
                    imp21.Text = op[0].L13_KWH_PASTREAD;
                    imp22.Text = op[0].L12_KWH_PRESREAD;
                    imp23.Text = op[0].L14_Multiplying_factor_KVAH;
                    imp24.Text = op[0].L15_KWH_UNITS;

                    exp21.Text = op[0].L33_Exp_Past_KWH_UNITS;
                    exp22.Text = op[0].L33_Exp_Present_KWH_UNITS;
                    exp23.Text = op[0].L14_Multiplying_factor_KWH;
                    exp24.Text = op[0].L33_Exp_KWH_UNITS;

                    kva11.Text = op[0].L33_Exp_CURRENT_NET_EXPORT_KWH_UNITS;
                    kva12.Text = op[0].L46_Previous_CREDIT_Units_MAIN_KWH;
                    kva13.Text = op[0].L46_Net_Billed_Units_MAIN_KWH;
                    kva14.Text = op[0].L46_Carry_Forward_Units_MAIN_KWH;

                    //////////////////////////////////////////////////
                    //For MTR2                                        
                    MTR_IMP1.Text  = op[0].L19_KVA_PASTREAD;           
                    MTR_IMP2.Text  = op[0].L18_KVA_PRESREAD;
                    MTR_IMP3.Text  = op[0].L20_Multiplying_Factor_KVA;
                    MTR_IMP4.Text  = op[0].L21_KVA_UNITS;

                    MTR_EXP1.Text  = op[0].L49_Exp_Past_KVAH_UNITS;
                    MTR_EXP2.Text  = op[0].L49_Exp_Present_KVAH_UNITS;
                    MTR_EXP3.Text = op[0].L20_Multiplying_Factor_KVAH;
                    MTR_EXP4.Text  = op[0].L49_Exp_KVAH_UNITS;

                    MTR2_IMP1.Text = op[0].L19_KWH_PASTREAD;
                    MTR2_IMP2.Text = op[0].L18_KWH_PRESREAD;
                    MTR2_IMP3.Text = op[0].L20_Multiplying_Factor_KWH;
                    MTR2_IMP4.Text = op[0].L21_KWH_UNITS;

                    MTR2_EXP1.Text = op[0].L49_Exp_Past_KWH_UNITS;
                    MTR2_EXP2.Text = op[0].L49_Exp_Present_KWH_UNITS;
                    MTR2_EXP3.Text = op[0].L20_Multiplying_Factor_KWH;
                    MTR2_EXP4.Text = op[0].L49_Exp_KWH_UNITS;

                    kvah21.Text    = op[0].L49_Exp_CURRENT_NET_EXPORT_KWH_UNITS;
                   

                }
                
                if (!string.IsNullOrEmpty(op[0].L6_Kvah_indicator) && op[0].L6_Kvah_indicator == "1")
                        {
                            //first meter
                    imp11.Text = op[0].L13_KVA_PASTREAD;
                    imp12.Text = op[0].L12_KVA_PRESREAD;
                    imp13.Text = op[0].L14_Multiplying_factor_KVA;
                    imp14.Text = op[0].L15_KVA_UNITS;

                    exp11.Text = op[0].L33_Exp_Past_KVA_UNITS;
                    exp12.Text = op[0].L33_Exp_Present_KVA_UNITS;
                    exp13.Text = op[0].L14_Multiplying_factor_KVA;
                    exp14.Text = op[0].L33_Exp_KVA_UNITS;  
                    
                    imp21.Text = op[0].L13_KVAH_PASTREAD;
                    imp22.Text = op[0].L12_KVAH_PRESREAD;
                    imp23.Text = op[0].L14_Multiplying_factor_KWH;
                    imp24.Text = op[0].L15_KVAH_UNITS;

                    exp21.Text = op[0].L33_Exp_Past_KVAH_UNITS;
                    exp22.Text = op[0].L33_Exp_Present_KVAH_UNITS;
                    exp23.Text = op[0].L14_Multiplying_factor_KWH;
                    exp24.Text = op[0].L33_Exp_KVAH_UNITS;

                    kva11.Text = op[0].L33_Exp_CURRENT_NET_EXPORT_KVAH_UNITS;
                    kva12.Text = op[0].L46_Previous_CREDIT_Units_MAIN_KVAH;
                    kva13.Text = op[0].L46_Net_Billed_Units_MAIN;
                    kva14.Text = op[0].L46_Carry_Forward_Units_MAIN_KVAH;

                    //mtr2
                    MTR_IMP1.Text = op[0].L19_KVA_PASTREAD;
                    MTR_IMP2.Text = op[0].L18_KVA_PRESREAD;
                    MTR_IMP3.Text = op[0].L20_Multiplying_Factor_KVA;
                    MTR_IMP4.Text = op[0].L21_KVA_UNITS;

                    MTR_EXP1.Text = op[0].L49_Exp_Past_KVA_UNITS;
                    MTR_EXP2.Text = op[0].L49_Exp_Present_KVA_UNITS;
                    MTR_EXP3.Text = op[0].L20_Multiplying_Factor_KVAH;
                    MTR_EXP4.Text = op[0].L49_Exp_KVA_UNITS;

                    MTR2_IMP1.Text = op[0].L12_KVAH_PRESREAD;
                    MTR2_IMP2.Text = op[0].L18_KVAH_PRESREAD;
                    MTR2_IMP3.Text = op[0].L20_Multiplying_Factor_KVA;
                    MTR2_IMP4.Text = op[0].L21_KVAH_UNITS;
                                          
                    MTR2_EXP1.Text = op[0].L49_Exp_Past_KVAH_UNITS;
                    MTR2_EXP2.Text = op[0].L49_Exp_Present_KVAH_UNITS;
                    MTR2_EXP3.Text = op[0].L20_Multiplying_Factor_KVAH;
                    MTR2_EXP4.Text = op[0].L49_Exp_KVAH_UNITS;

                    kvah21.Text = op[0].L49_Exp_CURRENT_NET_EXPORT_KVAH_UNITS;
                   



                }



            }
            else
            {
                if (string.IsNullOrEmpty(op[0].L6_Kvah_indicator) || op[0].L6_Kvah_indicator == "0.00")
                {
                    MTR1.Text = op[0].L11_MTRSNO_METER1;
                    KW_HEAD2.Visible = false;
                    KWH_HEAD2.Visible = false;
                    xrLabel1.Visible = false;
                    xrLabel2.Visible = false;
                    xrLabel3.Visible = false;
                    xrLabel4.Visible = false;
                   

                    //Newer
                    //                                         //Meter Old
                    imp11.Text = op[0].L13_KVA_PASTREAD;
                    imp12.Text = op[0].L12_KVA_PRESREAD;
                    imp13.Text = op[0].L14_Multiplying_factor_KVA;
                    imp14.Text = op[0].L15_KVA_UNITS;

                    exp11.Text = op[0].L33_Exp_Past_KW_UNITS;
                    exp12.Text = op[0].L33_Exp_Present_KW_UNITS;
                    exp13.Text = op[0].L14_Multiplying_factor_KVA;
                    exp14.Text = op[0].L33_Exp_KW_UNITS;

                    //FOR_KWH             
                    imp21.Text = op[0].L13_KWH_PASTREAD;
                    imp22.Text = op[0].L12_KWH_PRESREAD;
                    imp23.Text = op[0].L14_Multiplying_factor_KVAH;
                    imp24.Text = op[0].L15_KWH_UNITS;

                    exp21.Text = op[0].L33_Exp_Past_KWH_UNITS;
                    exp22.Text = op[0].L33_Exp_Present_KWH_UNITS;
                    exp23.Text = op[0].L14_Multiplying_factor_KWH;
                    exp24.Text = op[0].L33_Exp_KWH_UNITS;

                    kva11.Text = op[0].L33_Exp_CURRENT_NET_EXPORT_KWH_UNITS;
                    kva12.Text = op[0].L46_Previous_CREDIT_Units_MAIN_KWH;
                    kva13.Text = op[0].L46_Net_Billed_Units_MAIN_KWH;
                    kva14.Text = op[0].L46_Carry_Forward_Units_MAIN_KWH;
                }

                if (!string.IsNullOrEmpty(op[0].L6_Kvah_indicator) && op[0].L6_Kvah_indicator == "1")
                {
                    MTR1.Text = op[0].L11_MTRSNO_METER1;
                    KW_HEAD2.Visible = false;
                    KWH_HEAD2.Visible = false;
                    xrLabel1.Visible = false;
                    xrLabel2.Visible = false;
                    xrLabel3.Visible = false;
                    xrLabel4.Visible = false;
                    //Newer
                    //                                         //Meter Old
                    imp11.Text = op[0].L13_KVA_PASTREAD;
                    imp12.Text = op[0].L12_KVA_PRESREAD;
                    imp13.Text = op[0].L14_Multiplying_factor_KVA;
                    imp14.Text = op[0].L15_KVA_UNITS;
                    exp11.Text = op[0].L33_Exp_Past_KVA_UNITS;
                    exp12.Text = op[0].L33_Exp_Present_KVA_UNITS;
                    exp13.Text = op[0].L14_Multiplying_factor_KVA;
                    exp14.Text = op[0].L33_Exp_KVA_UNITS;

                    //FOR_KWH             
                    imp21.Text = op[0].L13_KVAH_PASTREAD;
                    imp22.Text = op[0].L12_KVAH_PRESREAD;
                    imp23.Text = op[0].L14_Multiplying_factor_KWH;
                    imp24.Text = op[0].L15_KVAH_UNITS;
                    exp21.Text = op[0].L33_Exp_Past_KVAH_UNITS;
                    exp22.Text = op[0].L33_Exp_Present_KVAH_UNITS;
                    exp23.Text = op[0].L14_Multiplying_factor_KWH;
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

            //op[0].L6_SERVDET_SANC_LOAD = op[0].L6_SERVDET_SANC_LOAD + "(" + op[0].L6_MEASURE_OF_CONTRACT_Demand + ")";


            #region Bill Details
            //Excess Demand Surcharge Print
            if (op[0].L10_DMDCHG_PENALTY == "0.00" || string.IsNullOrEmpty(op[0].L10_DMDCHG_PENALTY))
            {
                bd_ExcessDemandCharges.Visible = false;
                bd_ExcessDemandChargesValue.Visible = false;

                bd_ExcessDemandCharges.TopF = bd_Demand_charges.TopF;
                bd_ExcessDemandChargesValue.TopF = bd_Demand_chargesValue.TopF;

            }
            bd_EnergyCharge.TopF = bd_ExcessDemandCharges.BottomF;
            bd_EnergyChargeValues.TopF = bd_ExcessDemandCharges.BottomF;

            bd_TODCharges.TopF = bd_EnergyCharge.BottomF;
            bd_TODChargesValues.TopF = bd_EnergyCharge.BottomF;

            bd_ElectricityDuty.TopF = bd_TODCharges.BottomF;
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

            if (op[0].L8_AC_Charges == "0.00" || string.IsNullOrEmpty(op[0].L8_AC_Charges))
            {
                bd_AcCharges.Visible = false;
                bd_AcChargesValues.Visible = false;

                bd_AcCharges.TopF = bd_RlSC2.TopF;
                bd_AcChargesValues.TopF = bd_RlSC2.TopF;

            }            

            bd_AdjustmentCharges.TopF = bd_AcCharges.BottomF;
            bd_AdjustmentChargesValues.TopF = bd_AcChargesValues.BottomF;

            if (op[0].L8_min_charge == "0.00" || string.IsNullOrEmpty(op[0].L8_min_charge))
            {
                bd_AdjustmentCharges.Visible = false;
                bd_AdjustmentChargesValues.Visible = false;

                bd_AdjustmentCharges.TopF = bd_AcCharges.TopF;
                bd_AdjustmentChargesValues.TopF = bd_AcChargesValues.TopF;

            }
            bd_Other.TopF = bd_AdjustmentCharges.BottomF;
            bd_OtherValues.TopF = bd_AdjustmentChargesValues.BottomF;
            if (op[0].L8_SERVDET_TOTDB_BDT_OTHER == "0.00" || string.IsNullOrEmpty(op[0].L8_SERVDET_TOTDB_BDT_OTHER))
            {
                bd_Other.Visible = false;
                bd_OtherValues.Visible = false;

                bd_Other.TopF = bd_AdjustmentCharges.TopF;
                bd_OtherValues.TopF = bd_AdjustmentChargesValues.TopF;
            }

            Subsidy.TopF = bd_Other.BottomF;
            SubsidyValue.TopF = bd_Other.BottomF;

            if ((op[0].L8_Subsidy_Charges == "" || op[0].L8_Subsidy_Charges == "0.00"))
            {
                Subsidy.Visible = false;
                SubsidyValue.Visible = false;

                Subsidy.TopF = bd_Other.TopF;
                SubsidyValue.TopF = bd_Other.TopF;

            }

            GreenTariff.TopF = Subsidy.BottomF;
            GreenTariffValue.TopF = Subsidy.BottomF;

            if (op[0].L8_GreenTariff_Charges == "0.00"  || string.IsNullOrEmpty(op[0].L8_GreenTariff_Charges))
            {
                GreenTariff.Visible = false;
                GreenTariffValue.Visible = false;

                GreenTariff.TopF = Subsidy.TopF;
                GreenTariffValue.TopF= Subsidy.TopF;
            }

            bd_TotalCurrentDues.TopF = GreenTariff.BottomF;
            bd_TotalCurrentDuesValues.TopF = GreenTariff.BottomF;

            bd_Arrears.TopF = bd_TotalCurrentDues.BottomF;
            bd_Arrears_values.TopF = bd_TotalCurrentDues.BottomF;

            bd_LatePaymentSurcharges.TopF = bd_Arrears.BottomF;
            bd_LatePaymentSurchargesVALUE.TopF = bd_Arrears.BottomF;


            if (op[0].L9_INT_TPL == "0.00" || string.IsNullOrEmpty(op[0].L9_INT_TPL))
            {
                bd_LatePaymentSurcharges.Visible = false;
                bd_LatePaymentSurchargesVALUE.Visible = false;

                bd_LatePaymentSurcharges.TopF = bd_Arrears.TopF;
                bd_LatePaymentSurchargesVALUE.TopF = bd_Arrears_values.TopF;
            }

            bd_TotalDuesVALUE.TopF = bd_LatePaymentSurcharges.BottomF;
            bd_TotalDues.TopF = bd_TotalDuesVALUE.TopF;

          //  if (bd_TotalDues.LocationF.Y >= 208.43)
                if (bd_TotalDues.LocationF.Y >= 208)
                {
                xrPanel1.TopF = bd_TotalDues.BottomF + 370;
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
                    Font = new System.Drawing.Font("DIN Pro Regular", 9),
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
                    Font = new System.Drawing.Font("DIN Pro Regular", 9),
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
            if (!string.IsNullOrEmpty(op[0].L6_LT_Metering_Flag))
            {
                messageFromFile++;
                XRLabel xrMessage6 = new XRLabel
                {
                    Font = new System.Drawing.Font("DIN Pro Regular", 8),
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
            if (!String.IsNullOrEmpty(op[0].L6_LT_Metering_Flag))
            {
                xrlL6Servdet_Sanc_load.Text = "*" + xrlL6Servdet_Sanc_load.Text;


            }
            #endregion

            #region Custom Messages
            var totalMessages = messageFromFile;

            if (!string.IsNullOrEmpty(op[0].L6_EXCESS_DEMAND) && op[0].L6_EXCESS_DEMAND != "0.00")
            {
                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageExcessDemand = new XRLabel
                    {
                        Font = new System.Drawing.Font("Kruti Dev 010", 9),
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

            // if (!string.IsNullOrEmpty(op[0].L1_DisconnectionMSGPrintingIMMEDIATE) && op[0].L1_DisconnectionMSGPrintingIMMEDIATE != "0")
            // {
            //     if (!IsMessageLimitExceeds(totalMessages))
            //     {
            //         totalMessages++;
            //         XRLabel xrMessageDisconnection = new XRLabel
            //         {
            //             Font = new System.Drawing.Font("Kruti Dev 010", 9),
            //             TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
            //             Text = getMessage(LoadStaticData._HindiMessage, "IDC"),
            //             WordWrap = false,
            //             AutoWidth = true,
            //             KeepTogether = true,
            //             HeightF = 2,
            //         };
            //         xrPanel1.Controls.Add(xrMessageDisconnection);
            //         adjustMessages(xrMessageDisconnection);
            //     }
            // }
            //
            if (!string.IsNullOrEmpty(op[0].L10_Theft_Amount) && op[0].L10_Theft_Amount != "0.00")
            {
                if (!IsMessageLimitExceeds(totalMessages))
                {
                    totalMessages++;
                    XRLabel xrMessageTheftAmount = new XRLabel
                    {
                        Font = new System.Drawing.Font("Kruti Dev 010", 10),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                        Text = string.Format(getMessage(LoadStaticData._HindiMessage, "TFA"), op[0].L10_Theft_Amount.Replace('.', '-')),
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

            // if (!string.IsNullOrEmpty(op[0].L10_TOTARR_UPPCL_INT_UPPCL_INTARR_UPPCL) &&  ! Convert.ToDecimal(op[0].L10_TOTARR_UPPCL_INT_UPPCL_INTARR_UPPCL).Equals(decimal.Zero))
            // {
            // 
            //    // if (!IsMessageLimitExceeds(totalMessages))
            //    // {
            //    //     totalMessages++;
            //    //     XRLabel xrMessageTheftAmount = new XRLabel
            //    //     {
            //    //         Font = new System.Drawing.Font("Kruti Dev 010", 9),
            //    //         TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
            //    //         Text = string.Format(getMessage(LoadStaticData._HindiMessage, "DAD"), op[0].L10_Theft_Amount),
            //    //         WordWrap = true,
            //    //         WidthF = xrPanel1.WidthF,
            //    //         KeepTogether = true,
            //    //         HeightF = 1,
            //    //     };
            //    //
            //    //
            //    //     xrPanel1.Controls.Add(xrMessageTheftAmount);
            //    //     adjustMessages(xrMessageTheftAmount);
            //    // }
            // }

            // if (!string.IsNullOrEmpty(op[0].L8_ParkingAmount) && op[0].L8_ParkingAmount != "0.00")
            // {
            //     xrLabel21.Visible = true;
            // }

            // if (!string.IsNullOrEmpty(op[0].L1_BillingCode))
            // {
            //     if (op[0].L1_BillingCode == "3000")
            //     {
            //         if (!IsMessageLimitExceeds(totalMessages))
            //         {
            //             totalMessages++;
            //             decimal totalUnits = decimal.Zero;
            //             //if (!string.IsNullOrEmpty(op[0].L16_M1_KWH_UNITS))
            //             //{
            //             //    totalUnits += Convert.ToDecimal(op[0].L16_M1_KWH_UNITS);
            //             //}
            //
            //             //if (!string.IsNullOrEmpty(op[0].L20_M2_KWH_UNITS))
            //             //{
            //             //    totalUnits += Convert.ToDecimal(op[0].L20_M2_KWH_UNITS);
            //             //}
            //             var PrevReadDt = ChangeMonthToHindi(op[0].L7_PrevReadDt);
            //             var ReadDt = ChangeMonthToHindi(op[0].L7_ReaDt);
            //
            //             XRLabel xrMessageTheftAmount = new XRLabel
            //             {
            //                 Font = new System.Drawing.Font("Kruti Dev 010", 9),
            //                 TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
            //                 Text = string.Format(getMessage(LoadStaticData._HindiMessage, "AB1"), totalUnits.ToString().Replace(".", "-"), PrevReadDt.Replace("-", "&"), ReadDt.Replace("-", "&"), op[0].L10_Mode + " fnu"),
            //                 WordWrap = false,
            //                 AutoWidth = true,
            //                 KeepTogether = true,
            //                 HeightF = 2,
            //             };
            //
            //
            //             xrPanel1.Controls.Add(xrMessageTheftAmount);
            //             adjustMessages(xrMessageTheftAmount);
            //
            //             XRLabel xrAB2Msg = new XRLabel
            //             {
            //                 Font = new System.Drawing.Font("Kruti Dev 010", 9),
            //                 TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
            //                 Text = string.Format(getMessage(LoadStaticData._HindiMessage, "AB2")),
            //                 WordWrap = false,
            //                 AutoWidth = true,
            //                 KeepTogether = true,
            //                 HeightF = 2,
            //             };
            //
            //
            //             xrPanel1.Controls.Add(xrAB2Msg);
            //             adjustMessages(xrAB2Msg);
            //
            //
            //         }
            //     }
            // }
            //
            #endregion

            #region Template Messages
            if (!string.IsNullOrEmpty(op[0].L33_MESSAGE7))
            {
                messageFromFile++;
                XRLabel xrMessage7 = new XRLabel
                {
                    Font = new System.Drawing.Font("Kruti Dev 010", 9f),
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
                    Font = new System.Drawing.Font("Kruti Dev 010", 9),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Text = "vfHkys[kksa ds vuqlkj vkids la;kstu ij tekur /kujkf'k 'kwU; vafdr gSaA \r\n;fn vkids }kjk la;kstu jkf'k tek dh xbZ gS rks mDr tekur jkf'k dh ewy jlhn ds lkFk \r\ngekjs xzkgd lsok dsUnz  ij lEidZ djsaA ",
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

            //if (!string.IsNullOrEmpty(op[0].L8_Intrest_Amount) && Convert.ToDecimal(op[0].L8_Intrest_Amount) > 0)
            //{
            //    if (!IsMessageLimitExceeds(totalMessages))
            //    {
            //        totalMessages++;
            //        XRLabel xrMessageExcessDemand = new XRLabel
            //        {
            //            Font = new System.Drawing.Font("Kruti Dev 010", 9),
            //            TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
            //            Text = "fiz; miHkksäk] ,d eq'r ;kstuk ds varxZr 100 izfr'kr C;kt ekQ+h dk ykHk mBkus ds fy,] \nd`Ik;k mijksDr orZeku fcy ds lkFk ekfld fdLr jkf'k : " + op[0].L8_Intrest_Amount.ToString().Replace('.', '-') + " dk Hkqxrku djsaA",
            //            WordWrap = false,
            //            AutoWidth = true,
            //            Multiline = true,
            //            KeepTogether = true,
            //            HeightF = (float)0.01,
            //            Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
            //            //BorderDashStyle = BorderDashStyle.Dot,
            //            //Borders = DevExpress.XtraPrinting.BorderSide.All,
            //            //BorderWidth = 1,
            //        };
            //        xrPanel1.Controls.Add(xrMessageExcessDemand);
            //        adjustMessages(xrMessageExcessDemand);
            //    }
            //}


            #endregion

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

                Rpt_HT_Solar_PDF_visible.visible();

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
                Rpt_HT_Solar_PDF_visible.visibleon();
                bd_SolarExportEnergy.Visible = false;
                bd_Solar_Export_Value.Visible = false;
                bd_SolarExportEnergy.TopF = GreenTariff.TopF;
                bd_Solar_Export_Value.TopF = GreenTariffValue.TopF;
            }
#endregion
        }

#region  Helper Functions
// bool IsMessageLimitExceeds(int messagesCount)
// {
//     if (messagesCount >= 8)
//     {
//         return true;
//     }
//     else
//     {
//         return false;
//     }
// }

//  public void adjustMessages(XRLabel lbl)
//  {
//      if (xrPanel1.Controls.Count != 0)
//      {
//          foreach (XRLabel plbl in xrPanel1.Controls)
//          {
//              lbl.TopF = plbl.BottomF;
//          }
//      }
//      else
//      {
//          lbl.TopF = xrPanel1.TopF;
//      }
//  }

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

