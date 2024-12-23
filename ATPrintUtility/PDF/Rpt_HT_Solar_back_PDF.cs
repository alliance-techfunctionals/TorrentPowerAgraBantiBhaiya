using AT.Print.Utils;
using DevExpress.XtraCharts;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AT.Print.PDF
{
    public partial class Rpt_HT_Solar_back_PDF : DevExpress.XtraReports.UI.XtraReport
    {
        public Rpt_HT_Solar_back_PDF()
        {
            InitializeComponent();
        }
            private void Rpt_HT_solar_Back_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // var data = sender as Rpt_LTMD_Solar_PDF;
            //var op = data.DataSource as List<SolarBill>;

            var Data = this.DataSource as List<Solar_Bill_HT>;
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



            if (!string.IsNullOrEmpty(Data[0].L11_MTRSNO_METER_2_IF_AVAILABLE))
            {
                mtr1.Text = Data[0].L11_MTRSNO_METER1;
                mtr2.Text = Data[0].L11_MTRSNO_METER_2_IF_AVAILABLE;
                
                if (string.IsNullOrEmpty(Data[0].L6_Kvah_indicator) || Data[0].L6_Kvah_indicator == "0.00")
                {
                    MTR1_KW1.Text = Data[0].L17_TOD1_KVA_Units;
                    MTR1_KW2.Text = Data[0].L17_TOD2_KVA_Units;
                    MTR1_KW3.Text = Data[0].L17_TOD3_KVA_Units;
                    MTR1_KW4.Text = Data[0].L17_TOD4_KVA_Units;

                    MTR1_KWH1.Text = Data[0].L16_TOD1_KVAH_Units;
                    MTR1_KWH2.Text = Data[0].L16_TOD2_KVAH_Units;
                    MTR1_KWH3.Text = Data[0].L16_TOD3_KVAH_Units;
                    MTR1_KWH4.Text = Data[0].L16_TOD4_KVAH_Units;

                    MTR1_KW1_ex.Text = Data[0].L54_Exp_TOD1_KW_Units;
                    MTR1_KW2_ex.Text = Data[0].L54_Exp_TOD2_KW_Units;
                    MTR1_KW3_ex.Text = Data[0].L54_Exp_TOD3_KW_Units;
                    MTR1_KW4_ex.Text = Data[0].L54_Exp_TOD4_KW_Units;

                    MTR1_KWH1_ex.Text = Data[0].L34_Exp_TOD1_KWH_Units;
                    MTR1_KWH2_ex.Text = Data[0].L34_Exp_TOD2_KWH_Units;
                    MTR1_KWH3_ex.Text = Data[0].L34_Exp_TOD3_KWH_Units;
                    MTR1_KWH4_ex.Text = Data[0].L34_Exp_TOD4_KWH_Units;

                    MTR1_NET1.Text = Data[0].L43_Exp_CURRENT_NET_EXPORT_TOD1_KWH_UNITS;
                    MTR1_NET2.Text = Data[0].L43_Exp_CURRENT_NET_EXPORT_TOD2_KWH_UNITS;
                    MTR1_NET3.Text = Data[0].L43_Exp_CURRENT_NET_EXPORT_TOD3_KWH_UNITS;
                    MTR1_NET4.Text = Data[0].L43_Exp_CURRENT_NET_EXPORT_TOD4_KWH_UNITS;

                    MTR1_Pre1.Text = Data[0].L43_Previous_CREDIT_Units_TOD1_KWH;
                    MTR1_Pre2.Text = Data[0].L43_Previous_CREDIT_Units_TOD2_KWH;
                    MTR1_Pre3.Text = Data[0].L43_Previous_CREDIT_Units_TOD3_KWH;
                    MTR1_Pre4.Text = Data[0].L43_Previous_CREDIT_Units_TOD4_KWH;

                    MTR1_NBU1.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD1_KWH;
                    MTR1_NBU2.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD2_KWH;
                    MTR1_NBU3.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD3_KWH;
                    MTR1_NBU4.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD4_KWH;

                    MTR1_CF1.Text = Data[0].L45_Carry_Forward_Units_TOD1_KWH;
                    MTR1_CF2.Text = Data[0].L45_Carry_Forward_Units_TOD2_KWH;
                    MTR1_CF3.Text = Data[0].L45_Carry_Forward_Units_TOD3_KWH;
                    MTR1_CF4.Text = Data[0].L45_Carry_Forward_Units_TOD4_KWH;



                    // second meter 


                    MTR2_KW1.Text = Data[0].L23_TOD1_KVA_Units;
                    MTR2_KW2.Text = Data[0].L23_TOD2_KVA_Units;
                    MTR2_KW3.Text = Data[0].L23_TOD3_KVA_Units;
                    MTR2_KW4.Text = Data[0].L23_TOD4_KVA_Units;
                    //
                    MTR2_KWH1.Text = Data[0].L22_TOD1_KVAH_Units;
                    MTR2_KWH2.Text = Data[0].L22_TOD2_KVAH_Units;
                    MTR2_KWH3.Text = Data[0].L22_TOD3_KVAH_Units;
                    MTR2_KWH4.Text = Data[0].L22_TOD4_KVAH_Units;
                    //

                    //MTR2_KW1_ex.Text = Data[0].L53_Exp_TOD1_KW_Units;
                    //MTR2_KW2_ex.Text = Data[0].L53_Exp_TOD2_KW_Units;
                    //MTR2_KW3_ex.Text = Data[0].L53_Exp_TOD3_KW_Units;
                    //MTR2_KW4_ex.Text = Data[0].L53_Exp_TOD4_KW_Units;

                    //4 Sep 2021
                    MTR2_KW1_ex.Text = Data[0].L54_Exp_TOD1_KW_Units;
                    MTR2_KW2_ex.Text = Data[0].L54_Exp_TOD2_KW_Units;
                    MTR2_KW3_ex.Text = Data[0].L54_Exp_TOD3_KW_Units;
                    MTR2_KW4_ex.Text = Data[0].L54_Exp_TOD4_KW_Units;
                    //

                    //MTR2_KWH1_ex.Text = Data[0].L51_Exp_TOD1_KWH_Units;
                    //MTR2_KWH2_ex.Text = Data[0].L51_Exp_TOD2_KWH_Units;
                    //MTR2_KWH3_ex.Text = Data[0].L51_Exp_TOD3_KWH_Units;
                    //MTR2_KWH4_ex.Text = Data[0].L51_Exp_TOD4_KWH_Units;

                    //4 Sep 2021
                    MTR2_KWH1_ex.Text = Data[0].L50_Exp_TOD1_KVAH_Units;
                    MTR2_KWH2_ex.Text = Data[0].L50_Exp_TOD2_KVAH_Units;
                    MTR2_KWH3_ex.Text = Data[0].L50_Exp_TOD3_KVAH_Units;
                    MTR2_KWH4_ex.Text = Data[0].L50_Exp_TOD4_KVAH_Units;
                    //
                }
                // IMP
                if (!string.IsNullOrEmpty(Data[0].L6_Kvah_indicator) || Data[0].L6_Kvah_indicator == "1")
                {

                    MTR1_KW1.Text = Data[0].L17_TOD1_KVA_Units;
                    MTR1_KW2.Text = Data[0].L17_TOD2_KVA_Units;
                    MTR1_KW3.Text = Data[0].L17_TOD3_KVA_Units;
                    MTR1_KW4.Text = Data[0].L17_TOD4_KVA_Units;

                    MTR1_KWH1.Text = Data[0].L16_TOD1_KVAH_Units;
                    MTR1_KWH2.Text = Data[0].L16_TOD2_KVAH_Units;
                    MTR1_KWH3.Text = Data[0].L16_TOD3_KVAH_Units;
                    MTR1_KWH4.Text = Data[0].L16_TOD4_KVAH_Units;

                    MTR1_KW1_ex.Text = Data[0].L36_Exp_TOD1_KVA_Units;
                    MTR1_KW2_ex.Text = Data[0].L36_Exp_TOD2_KVA_Units;
                    MTR1_KW3_ex.Text = Data[0].L36_Exp_TOD3_KVA_Units;
                    MTR1_KW4_ex.Text = Data[0].L36_Exp_TOD4_KVA_Units;

                    MTR1_KWH1_ex.Text = Data[0].L35_Exp_TOD1_KVAH_Units;
                    MTR1_KWH2_ex.Text = Data[0].L35_Exp_TOD2_KVAH_Units;
                    MTR1_KWH3_ex.Text = Data[0].L35_Exp_TOD3_KVAH_Units;
                    MTR1_KWH4_ex.Text = Data[0].L35_Exp_TOD4_KVAH_Units;

                    MTR1_NET1.Text = Data[0].L42_Exp_CURRENT_NET_EXPORT_TOD1_KVAH_UNITS;
                    MTR1_NET2.Text = Data[0].L42_Exp_CURRENT_NET_EXPORT_TOD2_KVAH_UNITS;
                    MTR1_NET3.Text = Data[0].L42_Exp_CURRENT_NET_EXPORT_TOD3_KVAH_UNITS;
                    MTR1_NET4.Text = Data[0].L42_Exp_CURRENT_NET_EXPORT_TOD4_KVAH_UNITS;

                    MTR1_Pre1.Text = Data[0].L42_Previous_CREDIT_Units_TOD1_KVAH;
                    MTR1_Pre2.Text = Data[0].L42_Previous_CREDIT_Units_TOD2_KVAH;
                    MTR1_Pre3.Text = Data[0].L42_Previous_CREDIT_Units_TOD3_KVAH;
                    MTR1_Pre4.Text = Data[0].L42_Previous_CREDIT_Units_TOD4_KVAH;

                    MTR1_NBU1.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD1_KVAH;
                    MTR1_NBU2.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD2_KVAH;
                    MTR1_NBU3.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD3_KVAH;
                    MTR1_NBU4.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD4_KVAH;

                    MTR1_CF1.Text = Data[0].L44_Carry_Forward_Units_TOD1_KVAH;
                    MTR1_CF2.Text = Data[0].L44_Carry_Forward_Units_TOD2_KVAH;
                    MTR1_CF3.Text = Data[0].L44_Carry_Forward_Units_TOD3_KVAH;
                    MTR1_CF4.Text = Data[0].L44_Carry_Forward_Units_TOD4_KVAH;
                    //MTR2
                    MTR2_KW1.Text     = Data[0].L23_TOD1_KVA_Units;
                    MTR2_KW2.Text     = Data[0].L23_TOD2_KVA_Units;
                    MTR2_KW3.Text     = Data[0].L23_TOD3_KVA_Units;
                    MTR2_KW4.Text     = Data[0].L23_TOD4_KVA_Units;
                                      
                    MTR2_KWH1.Text    = Data[0].L22_TOD1_KVAH_Units;
                    MTR2_KWH2.Text    = Data[0].L22_TOD2_KVAH_Units;
                    MTR2_KWH3.Text    = Data[0].L22_TOD3_KVAH_Units;
                    MTR2_KWH4.Text    = Data[0].L22_TOD4_KVAH_Units;
                                      
                    MTR2_KW1_ex.Text  = Data[0].L52_Exp_TOD1_KVA_Units;
                    MTR2_KW2_ex.Text  = Data[0].L52_Exp_TOD2_KVA_Units;
                    MTR2_KW3_ex.Text  = Data[0].L52_Exp_TOD3_KVA_Units;
                    MTR2_KW4_ex.Text  = Data[0].L52_Exp_TOD4_KVA_Units;


                    //MTR2_KWH1_ex.Text = Data[0].L50_Exp_TOD1_KVAH_Units;
                    //MTR2_KWH2_ex.Text = Data[0].L50_Exp_TOD2_KVAH_Units;
                    //MTR2_KWH3_ex.Text = Data[0].L50_Exp_TOD3_KVAH_Units;
                    //MTR2_KWH4_ex.Text = Data[0].L50_Exp_TOD4_KVAH_Units;

                    //4 Sep 2021
                    MTR2_KWH1_ex.Text = Data[0].L51_Exp_TOD1_KWH_Units;
                    MTR2_KWH2_ex.Text = Data[0].L51_Exp_TOD2_KWH_Units;
                    MTR2_KWH3_ex.Text = Data[0].L51_Exp_TOD3_KWH_Units;
                    MTR2_KWH4_ex.Text = Data[0].L51_Exp_TOD4_KWH_Units;
                    //
                }

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
                    mtr1.Text = Data[0].L11_MTRSNO_METER1;
                   
                    MTR1_KW1.Text = Data[0].L17_TOD1_KVA_Units;
                    MTR1_KW2.Text = Data[0].L17_TOD2_KVA_Units;
                    MTR1_KW3.Text = Data[0].L17_TOD3_KVA_Units;
                    MTR1_KW4.Text = Data[0].L17_TOD4_KVA_Units;

                    MTR1_KWH1.Text = Data[0].L16_TOD1_KVAH_Units;
                    MTR1_KWH2.Text = Data[0].L16_TOD2_KVAH_Units;
                    MTR1_KWH3.Text = Data[0].L16_TOD3_KVAH_Units;
                    MTR1_KWH4.Text = Data[0].L16_TOD4_KVAH_Units;


                    //MTR1_KW1_ex.Text = Data[0].L36_Exp_TOD1_KVA_Units;
                    //MTR1_KW2_ex.Text = Data[0].L36_Exp_TOD2_KVA_Units;
                    //MTR1_KW3_ex.Text = Data[0].L36_Exp_TOD3_KVA_Units;
                    //MTR1_KW4_ex.Text = Data[0].L36_Exp_TOD4_KVA_Units;

                    //4 Sep 2021
                    MTR1_KW1_ex.Text = Data[0].L53_Exp_TOD1_KW_Units;
                    MTR1_KW2_ex.Text = Data[0].L53_Exp_TOD2_KW_Units;
                    MTR1_KW3_ex.Text = Data[0].L53_Exp_TOD3_KW_Units;
                    MTR1_KW4_ex.Text = Data[0].L53_Exp_TOD4_KW_Units;
                    //

                    //MTR1_KWH1_ex.Text = Data[0].L35_Exp_TOD1_KVAH_Units;
                    //MTR1_KWH2_ex.Text = Data[0].L35_Exp_TOD2_KVAH_Units;
                    //MTR1_KWH3_ex.Text = Data[0].L35_Exp_TOD3_KVAH_Units;
                    //MTR1_KWH4_ex.Text = Data[0].L35_Exp_TOD4_KVAH_Units;

                    //4 Sep 2021
                    MTR1_KWH1_ex.Text = Data[0].L34_Exp_TOD1_KWH_Units;
                    MTR1_KWH2_ex.Text = Data[0].L34_Exp_TOD2_KWH_Units;
                    MTR1_KWH3_ex.Text = Data[0].L34_Exp_TOD3_KWH_Units;
                    MTR1_KWH4_ex.Text = Data[0].L34_Exp_TOD4_KWH_Units;
                    //

                    MTR1_NET1.Text = Data[0].L43_Exp_CURRENT_NET_EXPORT_TOD1_KWH_UNITS;
                    MTR1_NET2.Text = Data[0].L43_Exp_CURRENT_NET_EXPORT_TOD2_KWH_UNITS;
                    MTR1_NET3.Text = Data[0].L43_Exp_CURRENT_NET_EXPORT_TOD3_KWH_UNITS;
                    MTR1_NET4.Text = Data[0].L43_Exp_CURRENT_NET_EXPORT_TOD4_KWH_UNITS;

                    MTR1_Pre1.Text = Data[0].L43_Previous_CREDIT_Units_TOD1_KWH;
                    MTR1_Pre2.Text = Data[0].L43_Previous_CREDIT_Units_TOD2_KWH;
                    MTR1_Pre3.Text = Data[0].L43_Previous_CREDIT_Units_TOD3_KWH;
                    MTR1_Pre4.Text = Data[0].L43_Previous_CREDIT_Units_TOD4_KWH;

                    MTR1_NBU1.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD1_KWH;
                    MTR1_NBU2.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD2_KWH;
                    MTR1_NBU3.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD3_KWH;
                    MTR1_NBU4.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD4_KWH;

                    MTR1_CF1.Text = Data[0].L45_Carry_Forward_Units_TOD1_KWH;
                    MTR1_CF2.Text = Data[0].L45_Carry_Forward_Units_TOD2_KWH;
                    MTR1_CF3.Text = Data[0].L45_Carry_Forward_Units_TOD3_KWH;
                    MTR1_CF4.Text = Data[0].L45_Carry_Forward_Units_TOD4_KWH;

                }
                if (!string.IsNullOrEmpty(Data[0].L6_Kvah_indicator) || Data[0].L6_Kvah_indicator == "1")
                {
                    
                    mtr1.Text = Data[0].L11_MTRSNO_METER1;
                   

                    MTR1_KW1.Text = Data[0].L17_TOD1_KVA_Units;
                    MTR1_KW2.Text = Data[0].L17_TOD2_KVA_Units;
                    MTR1_KW3.Text = Data[0].L17_TOD3_KVA_Units;
                    MTR1_KW4.Text = Data[0].L17_TOD4_KVA_Units;

                    MTR1_KWH1.Text = Data[0].L16_TOD1_KVAH_Units;
                    MTR1_KWH2.Text = Data[0].L16_TOD2_KVAH_Units;
                    MTR1_KWH3.Text = Data[0].L16_TOD3_KVAH_Units;
                    MTR1_KWH4.Text = Data[0].L16_TOD4_KVAH_Units;

                    //MTR1_KW1_ex.Text = Data[0].L53_Exp_TOD1_KW_Units;
                    //MTR1_KW2_ex.Text = Data[0].L53_Exp_TOD2_KW_Units;
                    //MTR1_KW3_ex.Text = Data[0].L53_Exp_TOD3_KW_Units;
                    //MTR1_KW4_ex.Text = Data[0].L53_Exp_TOD4_KW_Units;

                    // 4 Sep 2021
                    MTR1_KW1_ex.Text = Data[0].L36_Exp_TOD1_KVA_Units;
                    MTR1_KW2_ex.Text = Data[0].L36_Exp_TOD2_KVA_Units;
                    MTR1_KW3_ex.Text = Data[0].L36_Exp_TOD3_KVA_Units;
                    MTR1_KW4_ex.Text = Data[0].L36_Exp_TOD4_KVA_Units;
                    //

                    //MTR1_KWH1_ex.Text = Data[0].L50_Exp_TOD1_KVAH_Units;
                    //MTR1_KWH2_ex.Text = Data[0].L50_Exp_TOD2_KVAH_Units;
                    //MTR1_KWH3_ex.Text = Data[0].L50_Exp_TOD3_KVAH_Units;
                    //MTR1_KWH4_ex.Text = Data[0].L50_Exp_TOD4_KVAH_Units;

                    //4 Sep 2021
                    MTR1_KWH1_ex.Text = Data[0].L35_Exp_TOD1_KVAH_Units;
                    MTR1_KWH2_ex.Text = Data[0].L35_Exp_TOD2_KVAH_Units;
                    MTR1_KWH3_ex.Text = Data[0].L35_Exp_TOD3_KVAH_Units;
                    MTR1_KWH4_ex.Text = Data[0].L35_Exp_TOD4_KVAH_Units;
                    //

                    MTR1_NET1.Text = Data[0].L42_Exp_CURRENT_NET_EXPORT_TOD1_KVAH_UNITS;
                    MTR1_NET2.Text = Data[0].L42_Exp_CURRENT_NET_EXPORT_TOD2_KVAH_UNITS;
                    MTR1_NET3.Text = Data[0].L42_Exp_CURRENT_NET_EXPORT_TOD3_KVAH_UNITS;
                    MTR1_NET4.Text = Data[0].L42_Exp_CURRENT_NET_EXPORT_TOD4_KVAH_UNITS;

                    MTR1_Pre1.Text = Data[0].L42_Previous_CREDIT_Units_TOD1_KVAH;
                    MTR1_Pre2.Text = Data[0].L42_Previous_CREDIT_Units_TOD2_KVAH;
                    MTR1_Pre3.Text = Data[0].L42_Previous_CREDIT_Units_TOD3_KVAH;
                    MTR1_Pre4.Text = Data[0].L42_Previous_CREDIT_Units_TOD4_KVAH;

                    MTR1_NBU1.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD1_KVAH;
                    MTR1_NBU2.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD2_KVAH;
                    MTR1_NBU3.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD3_KVAH;
                    MTR1_NBU4.Text = Data[0].L46_Net_Billed_Units_MAIN_TOD4_KVAH;

                    MTR1_CF1.Text = Data[0].L44_Carry_Forward_Units_TOD1_KVAH;
                    MTR1_CF2.Text = Data[0].L44_Carry_Forward_Units_TOD2_KVAH;
                    MTR1_CF3.Text = Data[0].L44_Carry_Forward_Units_TOD3_KVAH;
                    MTR1_CF4.Text = Data[0].L44_Carry_Forward_Units_TOD4_KVAH;

                }


            }

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
                xrLabel95.Text = Data[0].L24_MonYear_1;
                xrLabel96.Text = Data[0].L24_MonYear_2;
                xrLabel97.Text = Data[0].L24_MonYear_3;
                xrLabel98.Text = Data[0].L24_MonYear_4;
                xrLabel99.Text = Data[0].L24_MonYear_5;
                xrLabel100.Text = Data[0].L24_MonYear_6;
                //Billed KVA/KW
                BillKW1.Text = Data[0].L24_KVA_UNITS_1;
                BillKW2.Text = Data[0].L24_KVA_UNITS_2;
                BillKW3.Text = Data[0].L24_KVA_UNITS_3;
                BillKW4.Text = Data[0].L24_KVA_UNITS_4;
                BillKW5.Text = Data[0].L24_KVA_UNITS_5;
                BillKW6.Text = Data[0].L24_KVA_UNITS_6;
                //Billed KVAH/KWH
                BillKWH1.Text = Data[0].L25_KVAH_UNITS_1;
                BillKWH2.Text = Data[0].L25_KVAH_UNITS_2;
                BillKWH3.Text = Data[0].L25_KVAH_UNITS_3;
                BillKWH4.Text = Data[0].L25_KVAH_UNITS_4;
                BillKWH5.Text = Data[0].L25_KVAH_UNITS_5;
                BillKWH6.Text = Data[0].L25_KVAH_UNITS_6;
                //Export KVAH/KWH
                Exp_Con1.Text = Data[0].L47_Exp_KVAH_UNITS1;
                Exp_Con2.Text = Data[0].L47_Exp_KVAH_UNITS2;
                Exp_Con3.Text = Data[0].L47_Exp_KVAH_UNITS3;
                Exp_Con4.Text = Data[0].L47_Exp_KVAH_UNITS4;
                Exp_Con5.Text = Data[0].L47_Exp_KVAH_UNITS5;
                Exp_Con6.Text = Data[0].L47_Exp_KVAH_UNITS6;
                //Gen. KVAH/KWH
                gen_con1.Text = Data[0].L48_Gen_KVAH_UNITS1;
                gen_con2.Text = Data[0].L48_Gen_KVAH_UNITS2;
                gen_con3.Text = Data[0].L48_Gen_KVAH_UNITS3;
                gen_con4.Text = Data[0].L48_Gen_KVAH_UNITS4;
                gen_con5.Text = Data[0].L48_Gen_KVAH_UNITS5;
                gen_con6.Text = Data[0].L48_Gen_KVAH_UNITS6;
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
                    xrLabel87.Text = Data[0].L40_Gen_MF1;
                    xrLabel88.Text = Data[0].L41_Gen_KVAH_NET_UNITS;

                    #region Consumption Information
                    //Months
                     xrLabel95.Text  = Data[0].L24_MonYear_1;
                     xrLabel96.Text  = Data[0].L24_MonYear_2;
                     xrLabel97.Text  = Data[0].L24_MonYear_3;
                     xrLabel98.Text  = Data[0].L24_MonYear_4;
                     xrLabel99.Text  = Data[0].L24_MonYear_5;
                     xrLabel100.Text = Data[0].L24_MonYear_6;
                     //Billed KVA/K
                     BillKW1.Text = Data[0].L24_KVA_UNITS_1;
                     BillKW2.Text = Data[0].L24_KVA_UNITS_2;
                     BillKW3.Text = Data[0].L24_KVA_UNITS_3;
                     BillKW4.Text = Data[0].L24_KVA_UNITS_4;
                     BillKW5.Text = Data[0].L24_KVA_UNITS_5;
                     BillKW6.Text = Data[0].L24_KVA_UNITS_6;
                     //Billed KVAH/WH
                     BillKWH1.Text  = Data[0].L25_KVAH_UNITS_1;
                     BillKWH2.Text  = Data[0].L25_KVAH_UNITS_2;
                     BillKWH3.Text  = Data[0].L25_KVAH_UNITS_3;
                     BillKWH4.Text  = Data[0].L25_KVAH_UNITS_4;
                     BillKWH5.Text  = Data[0].L25_KVAH_UNITS_5;
                     BillKWH6.Text  = Data[0].L25_KVAH_UNITS_6;
                     //Export KVAH/WH
                     Exp_Con1.Text  = Data[0].L47_Exp_KVAH_UNITS1;
                     Exp_Con2.Text  = Data[0].L47_Exp_KVAH_UNITS2;
                     Exp_Con3.Text  = Data[0].L47_Exp_KVAH_UNITS3;
                     Exp_Con4.Text  = Data[0].L47_Exp_KVAH_UNITS4;
                     Exp_Con5.Text  = Data[0].L47_Exp_KVAH_UNITS5;
                     Exp_Con6.Text  = Data[0].L47_Exp_KVAH_UNITS6;
                     //Gen. KVAH/KW
                     gen_con1.Text  = Data[0].L48_Gen_KVAH_UNITS1;
                     gen_con2.Text  = Data[0].L48_Gen_KVAH_UNITS2;
                     gen_con3.Text  = Data[0].L48_Gen_KVAH_UNITS3;
                     gen_con4.Text  = Data[0].L48_Gen_KVAH_UNITS4;
                     gen_con5.Text  = Data[0].L48_Gen_KVAH_UNITS5;
                     gen_con6.Text  = Data[0].L48_Gen_KVAH_UNITS6;
                    #endregion

                }
            }   
        }
        public void visible()
        {
            xrLabel7.Visible = false;
            xrLabel8.Visible = false;
            xrLabel9.Visible = false;
            xrLabel10.Visible = false;
            MTR1_NET1.Visible = false;
            MTR1_NET2.Visible = false;
            MTR1_NET3.Visible = false;
            MTR1_NET4.Visible = false;
            MTR1_Pre1.Visible = false;
            MTR1_Pre2.Visible = false;
            MTR1_Pre3.Visible = false;
            MTR1_Pre4.Visible = false;
            MTR1_NBU1.Visible = false;
            MTR1_NBU2.Visible = false;
            MTR1_NBU3.Visible = false;
            MTR1_NBU4.Visible = false;
            MTR1_CF1.Visible = false;
            MTR1_CF2.Visible = false;
            MTR1_CF3.Visible = false;
            MTR1_CF4.Visible = false;
        }
        public void visibleon()
        {
            xrLabel7.Visible = true;
            xrLabel8.Visible = true;
            xrLabel9.Visible = true;
            xrLabel10.Visible = true;
            MTR1_NET1.Visible = true;
            MTR1_NET2.Visible = true;
            MTR1_NET3.Visible = true;
            MTR1_NET4.Visible = true;
            MTR1_Pre1.Visible = true;
            MTR1_Pre2.Visible = true;
            MTR1_Pre3.Visible = true;
            MTR1_Pre4.Visible = true;
            MTR1_NBU1.Visible = true;
            MTR1_NBU2.Visible = true;
            MTR1_NBU3.Visible = true;
            MTR1_NBU4.Visible = true;
            MTR1_CF1.Visible = true;
            MTR1_CF2.Visible = true;
            MTR1_CF3.Visible = true;
            MTR1_CF4.Visible = true;
        }
    }
}
