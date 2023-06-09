using AT.Print.Utils;
using System.Collections.Generic;
using System.Windows.Forms;
using System;

namespace AT.Print
{
    public partial class Rpt_LTMD_Solar_Print_Back : DevExpress.XtraReports.UI.XtraReport
    {
        public Rpt_LTMD_Solar_Print_Back()
        {
            InitializeComponent();
        }



        #region Meter Print
        private void Rpt_LTMD_solar_Back_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            // var data = sender as Rpt_LTMD_Solar_PDF;
            //var op = data.DataSource as List<SolarBill>;

            var Data = this.DataSource as List<SolarBill>;
            xrPictureBox2.ImageUrl = Application.StartupPath + "\\Contents\\CategorySlabImages\\" + Data[0].L6_TARIFF_DESCR + ".png";
            xrPictureBox1.ImageUrl = Data[0].MVPicture;

           

            if (Data[0].L6_MEASURE_OF_CONTRACT_Demand == "HP")
            {

                if (Data[0].L6_Kvah_indicator == "1")
                {

                    Data[0].unit1 = "KVA";
                 }
                else
                {
                    Data[0].unit1 = "KW";

                }
            }
            else if (Data[0].L6_MEASURE_OF_CONTRACT_Demand == "KW")
            {
                if (!string.IsNullOrEmpty(Data[0].L6_Kvah_indicator) && Data[0].L6_Kvah_indicator == "1")
                {
                    Data[0].unit1 = "KVA";

                }
                else
                {
                    Data[0].unit1 = "KW";

                }
            }
            else if (Data[0].L6_MEASURE_OF_CONTRACT_Demand == "KVA")
            {
                if (Data[0].L6_Kvah_indicator == "1")
                {
                    Data[0].unit1 = "KVA";

                   
                }
                else
                {
                    Data[0].unit1 = "KW";
                }
            }


            //
            if (Data[0].L12_MTRSNO_METER_2_IF_AVAILABLE != "")
            {
                MTR_TOD1.Text = Data[0].L12_MTRSNO_METER1;
                MTR_TOD2.Text = Data[0].L12_MTRSNO_METER_2_IF_AVAILABLE;
                #region Meter(KW)
                if (string.IsNullOrEmpty(Data[0].L6_Kvah_indicator) || Data[0].L6_Kvah_indicator == "0.00")
                {

                    //imp
                    xrLabel16.Text = Data[0].L23_TOD_1_KW;
                    xrLabel23.Text = Data[0].L23_TOD_2_KW;
                    xrLabel44.Text = Data[0].L23_TOD_3_KW;
                    xrLabel30.Text = Data[0].L23_TOD_4_KW;

                    xrLabel17.Text = Data[0].L22_TOD_1_KWH;
                    xrLabel24.Text = Data[0].L22_TOD_2_KWH;
                    xrLabel45.Text = Data[0].L22_TOD_3_KWH;
                    xrLabel31.Text = Data[0].L22_TOD_4_KWH;
                    //EXP
                    
                    xrLabel37.Text = Data[0].L53_Exp_TOD1_KW_Units;
                    xrLabel51.Text = Data[0].L53_Exp_TOD2_KW_Units;
                    xrLabel58.Text = Data[0].L53_Exp_TOD3_KW_Units;
                    xrLabel65.Text = Data[0].L53_Exp_TOD4_KW_Units;

                    xrLabel38.Text = Data[0].L34_Exp_TOD1_KWH_Units;
                    xrLabel52.Text = Data[0].L34_Exp_TOD2_KWH_Units;
                    xrLabel59.Text = Data[0].L34_Exp_TOD3_KWH_Units;
                    xrLabel66.Text = Data[0].L34_Exp_TOD4_KWH_Units;



                    //Current Net Unit
                    xrLabel39.Text = Data[0].L43_Exp_CURRENT_NET_EXPORT_TOD1_KWH_UNITS;
                    xrLabel53.Text = Data[0].L43_Exp_CURRENT_NET_EXPORT_TOD2_KWH_UNITS;
                    xrLabel60.Text = Data[0].L43_Exp_CURRENT_NET_EXPORT_TOD3_KWH_UNITS;
                    xrLabel67.Text = Data[0].L43_Exp_CURRENT_NET_EXPORT_TOD4_KWH_UNITS;
                    //Previous net Unit
                    xrLabel40.Text = Data[0].L43_Previous_CREDIT_Units_TOD1_KWH;
                    xrLabel54.Text = Data[0].L43_Previous_CREDIT_Units_TOD2_KWH;
                    xrLabel61.Text = Data[0].L43_Previous_CREDIT_Units_TOD3_KWH;
                    xrLabel68.Text = Data[0].L43_Previous_CREDIT_Units_TOD4_KWH;
                    //net bill unit
                    xrLabel41.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD1_KWH;
                    xrLabel55.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD2_KWH;
                    xrLabel62.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD3_KWH;
                    xrLabel69.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD4_KWH;
                    //carry forword unit
                    xrLabel42.Text = Data[0].L45_Carry_Forward_Units_TOD1_KWH;
                    xrLabel56.Text = Data[0].L45_Carry_Forward_Units_TOD2_KWH;
                    xrLabel63.Text = Data[0].L45_Carry_Forward_Units_TOD3_KWH;
                    xrLabel70.Text = Data[0].L45_Carry_Forward_Units_TOD4_KWH;

                    //mtr2
                    MTR2_KW1.Text = Data[0].L25_TOD_1_KWH;
                    MTR2_KW2.Text = Data[0].L25_TOD_2_KWH;
                    MTR2_KW3.Text = Data[0].L25_TOD_3_KWH;
                    MTR2_KW4.Text = Data[0].L25_TOD_4_KWH;
                    MTR2_KW1_ex.Text = Data[0].L54_Exp_TOD1_KW_Units;
                    MTR2_KW2_ex.Text = Data[0].L54_Exp_TOD2_KW_Units;
                    MTR2_KW3_ex.Text = Data[0].L54_Exp_TOD3_KW_Units;
                    MTR2_KW4_ex.Text = Data[0].L54_Exp_TOD4_KW_Units;

                    MTR2_KWH1.Text = Data[0].L24_TOD_1_KWH;
                    MTR2_KWH2.Text = Data[0].L24_TOD_2_KWH;
                    MTR2_KWH3.Text = Data[0].L24_TOD_3_KWH;
                    MTR2_KWH4.Text = Data[0].L24_TOD_4_KWH;
                    MTR2_KWH1_ex.Text = Data[0].L51_Exp_TOD1_KWH_Units;
                    MTR2_KWH2_ex.Text = Data[0].L51_Exp_TOD2_KWH_Units;
                    MTR2_KWH3_ex.Text = Data[0].L51_Exp_TOD3_KWH_Units;
                    MTR2_KWH4_ex.Text = Data[0].L51_Exp_TOD4_KWH_Units;



                }
                #endregion
                #region Meter(KVA)
                if (!string.IsNullOrEmpty(Data[0].L6_Kvah_indicator) || Data[0].L6_Kvah_indicator == "1")
                {

                    xrLabel16.Text = Data[0].L23_TOD_1_KW;
                    xrLabel23.Text = Data[0].L23_TOD_2_KW;
                    xrLabel44.Text = Data[0].L23_TOD_3_KW;
                    xrLabel30.Text = Data[0].L23_TOD_4_KW;

                    xrLabel17.Text = Data[0].L22_TOD_1_KWH;
                    xrLabel24.Text = Data[0].L22_TOD_2_KWH;
                    xrLabel45.Text = Data[0].L22_TOD_3_KWH;
                    xrLabel31.Text = Data[0].L22_TOD_4_KWH;
                    //EXP
                    xrLabel37.Text = Data[0].L36_Exp_TOD1_KVA_Units;
                    xrLabel51.Text = Data[0].L36_Exp_TOD2_KVA_Units;
                    xrLabel58.Text = Data[0].L36_Exp_TOD3_KVA_Units;
                    xrLabel65.Text = Data[0].L36_Exp_TOD4_KVA_Units;


                    xrLabel38.Text = Data[0].L35_Exp_TOD1_KVAH_Units;
                    xrLabel52.Text = Data[0].L35_Exp_TOD2_KVAH_Units;
                    xrLabel59.Text = Data[0].L35_Exp_TOD3_KVAH_Units;
                    xrLabel66.Text = Data[0].L35_Exp_TOD4_KVAH_Units;

                    xrLabel39.Text = Data[0].L42_Exp_CURRENT_NET_EXPORT_TOD1_KVAH_UNITS;
                    xrLabel53.Text = Data[0].L42_Exp_CURRENT_NET_EXPORT_TOD2_KVAH_UNITS;
                    xrLabel60.Text = Data[0].L42_Exp_CURRENT_NET_EXPORT_TOD3_KVAH_UNITS;
                    xrLabel67.Text = Data[0].L42_Exp_CURRENT_NET_EXPORT_TOD4_KVAH_UNITS;


                    xrLabel40.Text = Data[0].L42_Previous_CREDIT_Units_TOD1_KVAH;
                    xrLabel54.Text = Data[0].L42_Previous_CREDIT_Units_TOD2_KVAH;
                    xrLabel61.Text = Data[0].L42_Previous_CREDIT_Units_TOD3_KVAH;
                    xrLabel68.Text = Data[0].L42_Previous_CREDIT_Units_TOD4_KVAH;

                    xrLabel41.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD1_KVAH;
                    xrLabel55.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD2_KVAH;
                    xrLabel62.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD3_KVAH;
                    xrLabel69.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD4_KVAH;


                    xrLabel42.Text = Data[0].L44_Carry_Forward_Units_TOD1_KVAH;
                    xrLabel56.Text = Data[0].L44_Carry_Forward_Units_TOD2_KVAH;
                    xrLabel63.Text = Data[0].L44_Carry_Forward_Units_TOD3_KVAH;
                    xrLabel70.Text = Data[0].L44_Carry_Forward_Units_TOD4_KVAH;

                    //mtr2
                    MTR2_KW1.Text = Data[0].L25_TOD_1_KWH;
                    MTR2_KW2.Text = Data[0].L25_TOD_2_KWH;
                    MTR2_KW3.Text = Data[0].L25_TOD_3_KWH;
                    MTR2_KW4.Text = Data[0].L25_TOD_4_KWH;
                    MTR2_KW1_ex.Text = Data[0].L52_Exp_TOD1_KVA_Units;
                    MTR2_KW2_ex.Text = Data[0].L52_Exp_TOD2_KVA_Units;
                    MTR2_KW3_ex.Text = Data[0].L52_Exp_TOD3_KVA_Units;
                    MTR2_KW4_ex.Text = Data[0].L52_Exp_TOD4_KVA_Units;

                    MTR2_KWH1.Text = Data[0].L24_TOD_1_KWH;
                    MTR2_KWH2.Text = Data[0].L24_TOD_2_KWH;
                    MTR2_KWH3.Text = Data[0].L24_TOD_3_KWH;
                    MTR2_KWH4.Text = Data[0].L24_TOD_4_KWH;
                    MTR2_KWH1_ex.Text = Data[0].L50_Exp_TOD1_KVAH_Units;
                    MTR2_KWH2_ex.Text = Data[0].L50_Exp_TOD2_KVAH_Units;
                    MTR2_KWH3_ex.Text = Data[0].L50_Exp_TOD3_KVAH_Units;
                    MTR2_KWH4_ex.Text = Data[0].L50_Exp_TOD4_KVAH_Units;


                }
                #endregion
            }
            else
            {
                mtr2_IMP.Visible = false;
                mtr2_exp.Visible = false;
                MTR2_TOD1.Visible = false;
                MTR2_TOD2.Visible = false;
                MTR2_TOD3.Visible = false;
                MTR2_TOD4.Visible = false;
                MTR2_EXP1.Visible = false;
                MTR2_EXP2.Visible = false;
                MTR2_EXP3.Visible = false;
                MTR2_EXP4.Visible = false;

                if (string.IsNullOrEmpty(Data[0].L6_Kvah_indicator) || Data[0].L6_Kvah_indicator == "0.00")
                {
                    MTR_TOD1.Text = Data[0].L12_MTRSNO_METER1;
                    //MTR1
                    xrLabel16.Text = Data[0].L23_TOD_1_KW;
                    xrLabel17.Text = Data[0].L22_TOD_1_KWH;
                    xrLabel23.Text = Data[0].L23_TOD_2_KW;
                    xrLabel24.Text = Data[0].L22_TOD_2_KWH;
                    xrLabel44.Text = Data[0].L23_TOD_3_KW;
                    xrLabel45.Text = Data[0].L22_TOD_3_KWH;
                    xrLabel30.Text = Data[0].L23_TOD_4_KW;
                    xrLabel31.Text = Data[0].L22_TOD_4_KWH;
                    //EXP
                    xrLabel37.Text = Data[0].L53_Exp_TOD1_KW_Units;
                    xrLabel51.Text = Data[0].L53_Exp_TOD2_KW_Units;
                    xrLabel58.Text = Data[0].L53_Exp_TOD3_KW_Units;
                    xrLabel65.Text = Data[0].L53_Exp_TOD4_KW_Units;

                    xrLabel38.Text = Data[0].L34_Exp_TOD1_KWH_Units;
                    xrLabel52.Text = Data[0].L34_Exp_TOD2_KWH_Units;
                    xrLabel59.Text = Data[0].L34_Exp_TOD3_KWH_Units;
                    xrLabel66.Text = Data[0].L34_Exp_TOD4_KWH_Units;




                    xrLabel39.Text = Data[0].L43_Exp_CURRENT_NET_EXPORT_TOD1_KWH_UNITS;
                    xrLabel40.Text = Data[0].L43_Previous_CREDIT_Units_TOD1_KWH;
                    xrLabel41.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD1_KWH;
                    xrLabel42.Text = Data[0].L45_Carry_Forward_Units_TOD1_KWH;
                    //other
                    xrLabel53.Text = Data[0].L43_Exp_CURRENT_NET_EXPORT_TOD2_KWH_UNITS;
                    xrLabel54.Text = Data[0].L43_Previous_CREDIT_Units_TOD2_KWH;
                    xrLabel55.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD2_KWH;
                    xrLabel56.Text = Data[0].L45_Carry_Forward_Units_TOD2_KWH;
                    xrLabel60.Text = Data[0].L43_Exp_CURRENT_NET_EXPORT_TOD3_KWH_UNITS;
                    xrLabel61.Text = Data[0].L43_Previous_CREDIT_Units_TOD3_KWH;
                    xrLabel62.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD3_KWH;
                    xrLabel63.Text = Data[0].L45_Carry_Forward_Units_TOD3_KWH;
                    xrLabel67.Text = Data[0].L43_Exp_CURRENT_NET_EXPORT_TOD4_KWH_UNITS;
                    xrLabel68.Text = Data[0].L43_Previous_CREDIT_Units_TOD4_KWH;
                    xrLabel69.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD4_KWH;
                    xrLabel70.Text = Data[0].L45_Carry_Forward_Units_TOD4_KWH;

                }
                if (!string.IsNullOrEmpty(Data[0].L6_Kvah_indicator) || Data[0].L6_Kvah_indicator == "1")
                {
                    MTR_TOD1.Text = Data[0].L12_MTRSNO_METER1;
                    
                    xrLabel16.Text = Data[0].L23_TOD_1_KW;
                    xrLabel23.Text = Data[0].L23_TOD_2_KW;
                    xrLabel44.Text = Data[0].L23_TOD_3_KW;
                    xrLabel30.Text = Data[0].L23_TOD_4_KW;

                    xrLabel17.Text = Data[0].L22_TOD_1_KWH;
                    xrLabel24.Text = Data[0].L22_TOD_2_KWH;
                    xrLabel45.Text = Data[0].L22_TOD_3_KWH;
                    xrLabel31.Text = Data[0].L22_TOD_4_KWH;
                    //EXP
                    xrLabel37.Text = Data[0].L36_Exp_TOD1_KVA_Units;
                    xrLabel51.Text = Data[0].L36_Exp_TOD2_KVA_Units;
                    xrLabel58.Text = Data[0].L36_Exp_TOD3_KVA_Units;
                    xrLabel65.Text = Data[0].L36_Exp_TOD4_KVA_Units;


                    xrLabel38.Text = Data[0].L35_Exp_TOD1_KVAH_Units;
                    xrLabel52.Text = Data[0].L35_Exp_TOD2_KVAH_Units;
                    xrLabel59.Text = Data[0].L35_Exp_TOD3_KVAH_Units;
                    xrLabel66.Text = Data[0].L35_Exp_TOD4_KVAH_Units;

                    xrLabel39.Text = Data[0].L42_Exp_CURRENT_NET_EXPORT_TOD1_KVAH_UNITS;
                    xrLabel53.Text = Data[0].L42_Exp_CURRENT_NET_EXPORT_TOD2_KVAH_UNITS;
                    xrLabel60.Text = Data[0].L42_Exp_CURRENT_NET_EXPORT_TOD3_KVAH_UNITS;
                    xrLabel67.Text = Data[0].L42_Exp_CURRENT_NET_EXPORT_TOD4_KVAH_UNITS;


                    xrLabel40.Text = Data[0].L42_Previous_CREDIT_Units_TOD1_KVAH;
                    xrLabel54.Text = Data[0].L42_Previous_CREDIT_Units_TOD2_KVAH;
                    xrLabel61.Text = Data[0].L42_Previous_CREDIT_Units_TOD3_KVAH;
                    xrLabel68.Text = Data[0].L42_Previous_CREDIT_Units_TOD4_KVAH;

                    xrLabel41.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD1_KVAH;
                    xrLabel55.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD2_KVAH;
                    xrLabel62.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD3_KVAH;
                    xrLabel69.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD4_KVAH;


                    xrLabel42.Text = Data[0].L44_Carry_Forward_Units_TOD1_KVAH;
                    xrLabel56.Text = Data[0].L44_Carry_Forward_Units_TOD2_KVAH;
                    xrLabel63.Text = Data[0].L44_Carry_Forward_Units_TOD3_KVAH;
                    xrLabel70.Text = Data[0].L44_Carry_Forward_Units_TOD4_KVAH;

                }

            }

            #region Meter Print2
            if (Data[0].L37_Gen_Meter_Serial_Number != "")
            {
                   if (string.IsNullOrEmpty(Data[0].L6_Kvah_indicator) || Data[0].L6_Kvah_indicator == "0.00")
                   {
                    xrLabel78.Text = Data[0].L37_Gen_Meter_Serial_Number;

                    xrLabel81.Text = Data[0].L39_Gen_KVA_PASTREAD;
                    xrLabel82.Text = Data[0].L38_Gen_KVA_PRESREAD;
                    xrLabel83.Text = Data[0].L40_Gen_MF3;
                    xrLabel84.Text = Data[0].L41_Gen_KVA_NET_UNITS;
                    xrLabel85.Text = Data[0].L39_Gen_KWH_PASTREAD;
                    xrLabel86.Text = Data[0].L38_Gen_KWH_PRESREAD;
                    xrLabel87.Text = Data[0].L40_Gen_MF1;
                    xrLabel88.Text = Data[0].L41_Gen_KWH_NET_UNITS;


                    #region Consumption Information
                    //Months
                    xrLabel95.Text = Data[0].L21_MonYear1;
                    xrLabel96.Text = Data[0].L21_MonYear2;
                    xrLabel97.Text = Data[0].L21_MonYear3;
                    xrLabel98.Text = Data[0].L21_MonYear4;
                    xrLabel99.Text = Data[0].L21_MonYear5;
                    xrLabel100.Text = Data[0].L21_MonYear6;
                    //Billed KVA/KW
                    xrLabel101.Text = Data[0].L21_KVA_UNITS1;
                    xrLabel102.Text = Data[0].L21_KVA_UNITS2;
                    xrLabel103.Text = Data[0].L21_KVA_UNITS3;
                    xrLabel104.Text = Data[0].L21_KVA_UNITS4;
                    xrLabel105.Text = Data[0].L21_KVA_UNITS5;
                    xrLabel106.Text = Data[0].L21_KVA_UNITS6;
                    //Billed KVAH/KWH
                    xrLabel107.Text = Data[0].L11_KWH_UNITS1;
                    xrLabel108.Text = Data[0].L11_KWH_UNITS2;
                    xrLabel109.Text = Data[0].L11_KWH_UNITS3;
                    xrLabel110.Text = Data[0].L11_KWH_UNITS4;
                    xrLabel111.Text = Data[0].L11_KWH_UNITS5;
                    xrLabel112.Text = Data[0].L11_KWH_UNITS6;
                    //Export KVAH/KWH
                    xrLabel113.Text = Data[0].L47_Exp_KVAH_UNITS1;
                    xrLabel114.Text = Data[0].L47_Exp_KVAH_UNITS2;
                    xrLabel115.Text = Data[0].L47_Exp_KVAH_UNITS3;
                    xrLabel116.Text = Data[0].L47_Exp_KVAH_UNITS4;
                    xrLabel117.Text = Data[0].L47_Exp_KVAH_UNITS5;
                    xrLabel118.Text = Data[0].L47_Exp_KVAH_UNITS6;
                    //Gen. KVAH/KWH
                    xrLabel119.Text = Data[0].L48_Gen_KVAH_UNITS1;
                    xrLabel120.Text = Data[0].L48_Gen_KVAH_UNITS2;
                    xrLabel121.Text = Data[0].L48_Gen_KVAH_UNITS3;
                    xrLabel122.Text = Data[0].L48_Gen_KVAH_UNITS4;
                    xrLabel123.Text = Data[0].L48_Gen_KVAH_UNITS5;
                    xrLabel124.Text = Data[0].L48_Gen_KVAH_UNITS6;
                    #endregion

                   }
                   if (!string.IsNullOrEmpty(Data[0].L6_Kvah_indicator) || Data[0].L6_Kvah_indicator == "1")
                   {
                    xrLabel78.Text = Data[0].L37_Gen_Meter_Serial_Number;

                    xrLabel81.Text = Data[0].L39_Gen_KVA_PASTREAD; 
                    xrLabel82.Text = Data[0].L38_Gen_KVA_PRESREAD; 
                    xrLabel83.Text = Data[0].L40_Gen_MF3; 
                    xrLabel84.Text = Data[0].L41_Gen_KVA_NET_UNITS;
                    xrLabel85.Text = Data[0].L39_Gen_KVAH_PASTREAD; 
                    xrLabel86.Text = Data[0].L38_Gen_KVAH_PRESREAD; 
                    xrLabel87.Text = Data[0].L40_Gen_MF2;
                    xrLabel88.Text = Data[0].L41_Gen_KVAH_NET_UNITS;

                    #region Consumption Information
                    //Months
                    xrLabel95.Text = Data[0].L21_MonYear1;
                    xrLabel96.Text = Data[0].L21_MonYear2;
                    xrLabel97.Text = Data[0].L21_MonYear3;
                    xrLabel98.Text = Data[0].L21_MonYear4;
                    xrLabel99.Text = Data[0].L21_MonYear5;
                    xrLabel100.Text = Data[0].L21_MonYear6;
                    //Billed KVA/KW
                    xrLabel101.Text = Data[0].L21_KVA_UNITS1;
                    xrLabel102.Text = Data[0].L21_KVA_UNITS2;
                    xrLabel103.Text = Data[0].L21_KVA_UNITS3;
                    xrLabel104.Text = Data[0].L21_KVA_UNITS4;
                    xrLabel105.Text = Data[0].L21_KVA_UNITS5;
                    xrLabel106.Text = Data[0].L21_KVA_UNITS6;
                    //Billed KVAH/KWH
                    xrLabel107.Text = Data[0].L11_KWH_UNITS1;
                    xrLabel108.Text = Data[0].L11_KWH_UNITS2;
                    xrLabel109.Text = Data[0].L11_KWH_UNITS3;
                    xrLabel110.Text = Data[0].L11_KWH_UNITS4;
                    xrLabel111.Text = Data[0].L11_KWH_UNITS5;
                    xrLabel112.Text = Data[0].L11_KWH_UNITS6;
                    //Export KVAH/KWH
                    xrLabel113.Text = Data[0].L47_Exp_KVAH_UNITS1;
                    xrLabel114.Text = Data[0].L47_Exp_KVAH_UNITS2;
                    xrLabel115.Text = Data[0].L47_Exp_KVAH_UNITS3;
                    xrLabel116.Text = Data[0].L47_Exp_KVAH_UNITS4;
                    xrLabel117.Text = Data[0].L47_Exp_KVAH_UNITS5;
                    xrLabel118.Text = Data[0].L47_Exp_KVAH_UNITS6;
                    //Gen. KVAH/KWH
                    xrLabel119.Text = Data[0].L48_Gen_KVAH_UNITS1;
                    xrLabel120.Text = Data[0].L48_Gen_KVAH_UNITS2;
                    xrLabel121.Text = Data[0].L48_Gen_KVAH_UNITS3;
                    xrLabel122.Text = Data[0].L48_Gen_KVAH_UNITS4;
                    xrLabel123.Text = Data[0].L48_Gen_KVAH_UNITS5;
                    xrLabel124.Text = Data[0].L48_Gen_KVAH_UNITS6;
                    #endregion

                }
            }
            #endregion


        }
        #endregion

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
        }
    }
}
