using System.Collections.Generic;

namespace AT.Print.Utils
{
    public class Solar_Bill_HT
    {
        public string unit { get; set; }
        public string unit1 { get; set; }
        public string L1_Bill_Type { get; set; }
        public string L1_MONTH_YEAR { get; set; }
        public string L1_ZONE { get; set; }
        public string L1_BU { get; set; }
        public string L1_PC { get; set; }
        public string L1_route { get; set; }
        public string L1_Bill_seq_no { get; set; }
        public string L1_Customer_PAN { get; set; }
        public string L2_NAME { get; set; }
        public string L3_ADDR1 { get; set; }
        public string L4_ADDR2 { get; set; }
        public string L5_ADDR3 { get; set; }
        public string L6_SERVDET_SERVNO { get; set; }
        public string L6_SERVDET_SANC_LOAD { get; set; }
        public string L6_Bill_Demand { get; set; }
        public string L6_ACTUAL_DEMAND { get; set; }
        public string L6_TARIFF_DESCR { get; set; }
        public string L6_EXCESS_DEMAND { get; set; }
        public string L6_SUPPLY_VOLTAGE { get; set; }
        public string L6_Avg_Power_Factor { get; set; }
        public string L6_MTRDET_LF_PERC { get; set; }
        public string L6_Bill_Type_Assess_OR_normal { get; set; }
        public string L6_MEASURE_OF_CONTRACT_Demand { get; set; }
        public string L6_Kvah_indicator { get; set; }
        public string L6_LT_Metering_Flag { get; set; }


        public string L7_due_date { get; set; }
        public string L7_Billdt { get; set; }
        public string L7_PrevReadDt { get; set; }
        public string L7_readt { get; set; }
        public string L7_LastpymtDate { get; set; }
        public string L7_Last_Payement_amount { get; set; }


        public string L8_FixedCharge { get; set; }
        public string L8_EnergyCharge { get; set; }
        public string L8_AC_Charges { get; set; }
        public string L8_GovTax { get; set; }
        public string L8_min_charge { get; set; }
        public string L8_SERVDET_TOTDB_BDT_OTHER { get; set; }
        public string L8_power_factor_adj { get; set; }
        public string L8_TOD_Charges { get; set; }
        public string L8_Regulatory_Charge1 { get; set; }
        public string L8_Regulatory_Charge2 { get; set; }
        public string L8_Rebate_incurred_of_current_month { get; set; }
        public string L8_amount_payable_before_due_date { get; set; }
        public string L8_T_No { get; set; }
        public string L8_Subsidy_Charges { get; set; }

        public string L8_Solar_Export_Energy { get; set; }
        public string L8_GreenTariff_Charges { get; set; }
        public string L8_Intrest_Amount { get; set; }
      
        public string L9_TOT_DB_ARR { get; set; }
        public string L9_CurrBillamt{ get; set; }
        public string L9_INT_TPL    { get; set; }
        public string L9_Arrs_TPL { get; set; }
        public string L9_nCurrBillamt_INT_TPL_ARRS_TPL{ get; set; }
        public string L9_Total_Bill_payable_rounded { get; set; }
        public string L10_LFincentive { get; set; }
        public string L10_DISCONN_DATE_date { get; set; }
        public string L10_TOTARR_UPPCL_INT_UPPCL_INTARR_UPPCL { get; set; }
        public string L10_TotArrUPPCLIntUPPCLIntArrUPPCL_Rounded { get; set; }
        public string L10_SECDEPT_BDT { get; set; }
        public string L10_DMDCHG_PENALTY { get; set; }
        public string L10_UPPCL_Arrear_Amount { get; set; }
        public string L10_UPPCL_Int_on_Arrear_Amount { get; set; }
        public string L10_Theft_Amount { get; set; }
        public string L10_Mode { get; set; }

        public string L11_MTRSNO_METER1 { get; set; }
        public string L11_MTRSNO_METER_2_IF_AVAILABLE { get; set; }


        public string L12_KWH_PRESREAD { get; set; }
        public string L12_KVAH_PRESREAD { get; set; }
        public string L12_KVA_PRESREAD { get; set; }

        public string L13_KWH_PASTREAD { get; set; }
        public string L13_KVAH_PASTREAD { get; set; }
        public string L13_KVA_PASTREAD { get; set; }
        public string L13_Purpose { get; set; }

        public string L14_Multiplying_factor_KWH { get; set; }
        public string L14_Multiplying_factor_KVAH { get; set; }
        public string L14_Multiplying_factor_KVA { get; set; }

        public string L15_KWH_UNITS { get; set; }
        public string L15_KVAH_UNITS { get; set; }
        public string L15_KVA_UNITS { get; set; }

        public string L16_TOD1_KVAH_Units { get; set; }
        public string L16_TOD2_KVAH_Units { get; set; }
        public string L16_TOD3_KVAH_Units { get; set; }
        public string L16_TOD4_KVAH_Units { get; set; }


        public string L17_TOD1_KVA_Units { get; set; }
        public string L17_TOD2_KVA_Units { get; set; }
        public string L17_TOD3_KVA_Units { get; set; }
        public string L17_TOD4_KVA_Units { get; set; }

        public string L18_KWH_PRESREAD { get; set; }
        public string L18_KVAH_PRESREAD { get; set; }
        public string L18_KVA_PRESREAD { get; set; }

        public string L19_KWH_PASTREAD { get; set; }
        public string L19_KVAH_PASTREAD { get; set; }
        public string L19_KVA_PASTREAD { get; set; }


        public string L20_Multiplying_Factor_KWH { get; set; }
        public string L20_Multiplying_Factor_KVAH { get; set; }
        public string L20_Multiplying_Factor_KVA { get; set; }


        public string L21_KWH_UNITS { get; set; }
        public string L21_KVAH_UNITS { get; set; }
        public string L21_KVA_UNITS { get; set; }


        public string L22_TOD1_KVAH_Units { get; set; }
        public string L22_TOD2_KVAH_Units { get; set; }
        public string L22_TOD3_KVAH_Units { get; set; }
        public string L22_TOD4_KVAH_Units { get; set; }

        public string L23_TOD1_KVA_Units { get; set; }
        public string L23_TOD2_KVA_Units { get; set; }
        public string L23_TOD3_KVA_Units { get; set; }
        public string L23_TOD4_KVA_Units { get; set; }


        public string L24_MonYear_1 { get; set; }
        public string L24_KVA_UNITS_1 { get; set; }
        public string L24_MonYear_2 { get; set; }
        public string L24_KVA_UNITS_2 { get; set; }
        public string L24_MonYear_3 { get; set; }
        public string L24_KVA_UNITS_3 { get; set; }
        public string L24_MonYear_4 { get; set; }
        public string L24_KVA_UNITS_4 { get; set; }
        public string L24_MonYear_5 { get; set; }
        public string L24_KVA_UNITS_5 { get; set; }
        public string L24_MonYear_6 { get; set; }
        public string L24_KVA_UNITS_6 { get; set; }
        public string L24_MonYear_7 { get; set; }
        public string L24_KVA_UNITS_7 { get; set; }

        public string L25_MonYear_1 { get; set; }
        public string L25_KVAH_UNITS_1 { get; set; }
        public string L25_MonYear_2 { get; set; }
        public string L25_KVAH_UNITS_2 { get; set; }
        public string L25_MonYear_3 { get; set; }
        public string L25_KVAH_UNITS_3 { get; set; }
        public string L25_MonYear_4 { get; set; }
        public string L25_KVAH_UNITS_4 { get; set; }
        public string L25_MonYear_5 { get; set; }
        public string L25_KVAH_UNITS_5 { get; set; }
        public string L25_MonYear_6 { get; set; }
        public string L25_KVAH_UNITS_6 { get; set; }
        public string L25_MonYear_7 { get; set; }
        public string L25_KVAH_UNITS_7 { get; set; }



        public string L26_MESSAGE1 { get; set; }
        public string L27_MESSAGE2 { get; set; }
        public string L28_MESSAGE3 { get; set; }
        public string L29_MESSAGE4 { get; set; }
        public string L30_MESSAGE5 { get; set; }
        public string L31_MESSAGE6 { get; set; }
        public string L33_MESSAGE7 { get; set; }
        public string L34_MESSAGE8 { get; set; }
        public string L35_MESSAGE9 { get; set; }
        public string L36_MESSAGE10 { get; set; }
        public string L32_BarCode { get; set; }



        public string L33_Exp_KWH_UNITS          { get; set; }
        public string L33_Exp_Past_KWH_UNITS     { get; set; }
        public string L33_Exp_Present_KWH_UNITS  { get; set; }
        public string L33_Exp_KVAH_UNITS         { get; set; }
        public string L33_Exp_Past_KVAH_UNITS    { get; set; }
        public string L33_Exp_Present_KVAH_UNITS { get; set; }
        public string L33_Exp_KVA_UNITS          { get; set; }
        public string L33_Exp_Past_KVA_UNITS     { get; set; }
        public string L33_Exp_Present_KVA_UNITS { get; set; }
        public string L33_Exp_CURRENT_NET_EXPORT_KVA_UNITS  { get; set; }
        public string L33_Exp_CURRENT_NET_EXPORT_KVAH_UNITS { get; set; }
        public string L33_Exp_CURRENT_NET_EXPORT_KWH_UNITS { get; set; }
        public string L33_Exp_KW_UNITS { get; set; }
        public string L33_Exp_Past_KW_UNITS { get; set; }
        public string L33_Exp_Present_KW_UNITS { get; set; }
        public string L34_Exp_TOD1_KWH_Units  { get; set; }
        public string L34_Exp_TOD2_KWH_Units  { get; set; }
        public string L34_Exp_TOD3_KWH_Units  { get; set; }
        public string L34_Exp_TOD4_KWH_Units  { get; set; }
        public string L35_Exp_TOD1_KVAH_Units { get; set; }
        public string L35_Exp_TOD2_KVAH_Units { get; set; }
        public string L35_Exp_TOD3_KVAH_Units { get; set; }
        public string L35_Exp_TOD4_KVAH_Units { get; set; }
        public string L36_Exp_TOD1_KVA_Units  { get; set; }
        public string L36_Exp_TOD2_KVA_Units  { get; set; }
        public string L36_Exp_TOD3_KVA_Units  { get; set; }
        public string L36_Exp_TOD4_KVA_Units { get; set; }
        public string L37_Gen_Meter_Serial_Number { get; set; }
        public string L38_Gen_KWH_PRESREAD  { get; set; }
        public string L38_Gen_KVAH_PRESREAD { get; set; }
        public string L38_Gen_KVA_PRESREAD  { get; set; }
        public string L38_Gen_KW_PRESREAD   { get; set; }
        public string L39_Gen_KWH_PASTREAD  { get; set; }
        public string L39_Gen_KVAH_PASTREAD { get; set; }
        public string L39_Gen_KVA_PASTREAD  { get; set; }
        public string L39_Gen_KW_PASTREAD { get; set; }
        public string L40_Gen_MF1 { get; set; }
        public string L40_Gen_MF2 { get; set; }
        public string L40_Gen_MF3 { get; set; }
        public string L40_Gen_MF4 { get; set; }
        public string L41_Gen_KWH_NET_UNITS  { get; set; }
        public string L41_Gen_KVAH_NET_UNITS { get; set; }
        public string L41_Gen_KVA_NET_UNITS  { get; set; }
        public string L41_Gen_KW_NET_UNITS { get; set; }
        public string L42_Previous_CREDIT_Units_TOD1_KVAH { get; set; }
        public string L42_Previous_CREDIT_Units_TOD2_KVAH { get; set; }
        public string L42_Previous_CREDIT_Units_TOD3_KVAH { get; set; }
        public string L42_Previous_CREDIT_Units_TOD4_KVAH { get; set; }
        public string L42_Exp_CURRENT_NET_EXPORT_TOD1_KVAH_UNITS { get; set; }
        public string L42_Exp_CURRENT_NET_EXPORT_TOD2_KVAH_UNITS { get; set; }
        public string L42_Exp_CURRENT_NET_EXPORT_TOD3_KVAH_UNITS { get; set; }
        public string L42_Exp_CURRENT_NET_EXPORT_TOD4_KVAH_UNITS { get; set; }
        public string L43_Previous_CREDIT_Units_TOD1_KWH { get; set; }
        public string L43_Previous_CREDIT_Units_TOD2_KWH { get; set; }
        public string L43_Previous_CREDIT_Units_TOD3_KWH { get; set; }
        public string L43_Previous_CREDIT_Units_TOD4_KWH { get; set; }
        public string L43_Exp_CURRENT_NET_EXPORT_TOD1_KWH_UNITS { get; set; }
        public string L43_Exp_CURRENT_NET_EXPORT_TOD2_KWH_UNITS { get; set; }
        public string L43_Exp_CURRENT_NET_EXPORT_TOD3_KWH_UNITS { get; set; }
        public string L43_Exp_CURRENT_NET_EXPORT_TOD4_KWH_UNITS { get; set; }
        public string L44_Carry_Forward_Units_TOD1_KVAH { get; set; }
        public string L44_Carry_Forward_Units_TOD2_KVAH { get; set; }
        public string L44_Carry_Forward_Units_TOD3_KVAH { get; set; }
        public string L44_Carry_Forward_Units_TOD4_KVAH { get; set; }
        public string L45_Carry_Forward_Units_TOD1_KWH  { get; set; }
        public string L45_Carry_Forward_Units_TOD2_KWH  { get; set; }
        public string L45_Carry_Forward_Units_TOD3_KWH  { get; set; }
        public string L45_Carry_Forward_Units_TOD4_KWH { get; set; }
        public string L46_Previous_CREDIT_Units_MAIN_KVAH { get; set; }
        public string L46_Net_Billed_Units_MAIN { get; set; }
        public string L46_Net_Billed_Units_MAIN_TOD1_KVAH { get; set; }
        public string L46_Net_Billed_Units_MAIN_TOD2_KVAH { get; set; }
        public string L46_Net_Billed_Units_MAIN_TOD3_KVAH { get; set; }
        public string L46_Net_Billed_Units_MAIN_TOD4_KVAH { get; set; }
        public string L46_Carry_Forward_Units_MAIN_KVAH   { get; set; }
        public string L46_Previous_CREDIT_Units_MAIN_KWH  { get; set; }
        public string L46_Net_Billed_Units_MAIN_KWH       { get; set; }
        public string L46_Net_Billed_Units_MAIN_TOD1_KWH  { get; set; }
        public string L46_Net_Billed_Units_MAIN_TOD2_KWH  { get; set; }
        public string L46_Net_Billed_Units_MAIN_TOD3_KWH  { get; set; }
        public string L46_Net_Billed_Units_MAIN_TOD4_KWH  { get; set; }
        public string L46_Carry_Forward_Units_MAIN_KWH { get; set; }
        public string L46_SolarLoad { get; set; }   
        public string L47_MonYear1        { get; set; }
        public string L47_Exp_KVAH_UNITS1 { get; set; }
        public string L47_MonYear2        { get; set; }
        public string L47_Exp_KVAH_UNITS2 { get; set; }
        public string L47_MonYear3        { get; set; }
        public string L47_Exp_KVAH_UNITS3 { get; set; }
        public string L47_MonYear4        { get; set; }
        public string L47_Exp_KVAH_UNITS4 { get; set; }
        public string L47_MonYear5        { get; set; }
        public string L47_Exp_KVAH_UNITS5 { get; set; }
        public string L47_MonYear6        { get; set; }
        public string L47_Exp_KVAH_UNITS6 { get; set; }
        public string L47_MonYear7        { get; set; }
        public string L47_Exp_KVAH_UNITS7 { get; set; }
        public string L48_MonYear1        { get; set; }
        public string L48_Gen_KVAH_UNITS1 { get; set; }
        public string L48_MonYear2        { get; set; }
        public string L48_Gen_KVAH_UNITS2 { get; set; }
        public string L48_MonYear3        { get; set; }
        public string L48_Gen_KVAH_UNITS3 { get; set; }
        public string L48_MonYear4        { get; set; }
        public string L48_Gen_KVAH_UNITS4 { get; set; }
        public string L48_MonYear5        { get; set; }
        public string L48_Gen_KVAH_UNITS5 { get; set; }
        public string L48_MonYear6        { get; set; }
        public string L48_Gen_KVAH_UNITS6 { get; set; }
        public string L48_MonYear7        { get; set; }
        public string L48_Gen_KVAH_UNITS7 { get; set; }
        public string L49_Exp_KWH_UNITS { get; set; }
        public string L49_Exp_Past_KWH_UNITS     { get; set; }
        public string L49_Exp_Present_KWH_UNITS  { get; set; }
        public string L49_Exp_KVAH_UNITS         { get; set; }
        public string L49_Exp_Past_KVAH_UNITS    { get; set; }
        public string L49_Exp_Present_KVAH_UNITS { get; set; }
        public string L49_Exp_KVA_UNITS          { get; set; }
        public string L49_Exp_Past_KVA_UNITS     { get; set; }
        public string L49_Exp_Present_KVA_UNITS { get; set; }
        public string L49_Exp_CURRENT_NET_EXPORT_KVA_UNITS  { get; set; }
        public string L49_Exp_CURRENT_NET_EXPORT_KVAH_UNITS { get; set; }
        public string L49_Exp_CURRENT_NET_EXPORT_KWH_UNITS { get; set; }
        public string L50_Exp_TOD1_KVAH_Units { get; set; }
        public string L50_Exp_TOD2_KVAH_Units { get; set; }
        public string L50_Exp_TOD3_KVAH_Units { get; set; }
        public string L50_Exp_TOD4_KVAH_Units { get; set; }
        public string L51_Exp_TOD1_KWH_Units  { get; set; }
        public string L51_Exp_TOD2_KWH_Units  { get; set; }
        public string L51_Exp_TOD3_KWH_Units  { get; set; }
        public string L51_Exp_TOD4_KWH_Units  { get; set; }
        public string L52_Exp_TOD1_KVA_Units  { get; set; }
        public string L52_Exp_TOD2_KVA_Units  { get; set; }
        public string L52_Exp_TOD3_KVA_Units  { get; set; }
        public string L52_Exp_TOD4_KVA_Units  { get; set; }
        public string L53_Exp_TOD1_KW_Units   { get; set; }
        public string L53_Exp_TOD2_KW_Units   { get; set; }
        public string L53_Exp_TOD3_KW_Units   { get; set; }
        public string L53_Exp_TOD4_KW_Units   { get; set; }
        public string L54_Exp_TOD1_KW_Units   { get; set; }
        public string L54_Exp_TOD2_KW_Units   { get; set; }
        public string L54_Exp_TOD3_KW_Units   { get; set; }
        public string L54_Exp_TOD4_KW_Units   { get; set; }
        public string L55_MonYear1            { get; set; }
        public string L55_Exp_KWH_UNITS1      { get; set; }
        public string L55_MonYear2            { get; set; }
        public string L55_Exp_KWH_UNITS2      { get; set; }
        public string L55_MonYear3            { get; set; }
        public string L55_Exp_KWH_UNITS3      { get; set; }
        public string L55_MonYear4            { get; set; }
        public string L55_Exp_KWH_UNITS4      { get; set; }
        public string L55_MonYear5            { get; set; }
        public string L55_Exp_KWH_UNITS5      { get; set; }
        public string L55_MonYear6            { get; set; }
        public string L55_Exp_KWH_UNITS6      { get; set; }
        public string L55_MonYear7            { get; set; }
        public string L55_Exp_KWH_UNITS7      { get; set; }
        public string L56_MonYear1            { get; set; }
        public string L56_Gen_KWH_UNITS1      { get; set; }
        public string L56_MonYear2            { get; set; }
        public string L56_Gen_KWH_UNITS2      { get; set; }
        public string L56_MonYear3            { get; set; }
        public string L56_Gen_KWH_UNITS3      { get; set; }
        public string L56_MonYear4            { get; set; }
        public string L56_Gen_KWH_UNITS4      { get; set; }
        public string L56_MonYear5            { get; set; }
        public string L56_Gen_KWH_UNITS5      { get; set; }
        public string L56_MonYear6            { get; set; }
        public string L56_Gen_KWH_UNITS6      { get; set; }
        public string L56_MonYear7            { get; set; }
        public string L56_Gen_KWH_UNITS7 { get; set; }
        public string MVPicture { get; set; }
        public string TopPanel_Row_1 { get; set; }
        public string TopPanel_Row_2 { get; set; }
        public string TopPanel_Row_3 { get; set; }
        public string TopPanel_Row_4 { get; set; }
        public string TopPanel_Row_5 { get; set; }
        public string TopPanel_Row_6 { get; set; }
        public string TopPanel { get; set; }
        public string Sap_Zone { get; set; }
        public string Sap_LotNo { get; set; }
        public string Sap_GrpNo { get; set; }
        public string lblSapratorNote { get; set; }

















    }

}