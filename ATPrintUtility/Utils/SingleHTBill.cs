using System.Collections.Generic;

namespace AT.Print.Utils
{
    public class SingleHTBill
    {

       
        public string L1_PowerFactorMSGIndicator { get; set; }
        public string L10_TheftAmount { get; set; }
       
        public string L1_BillType { get; set; }
        public string L1_MonthYear { get; set; }
        public string L1_Zone { get; set; }
        public string L1_BU { get; set; }
        public string L1_PC { get; set; }
        public string L1_Route { get; set; }
        public string L1_Bill_seq_no { get; set; }
        public string L1_FeederName { get; set; }
        public string L1_TODOrNon_TODFlag { get; set; }
        public string L1_AKY_indicator { get; set; }
        public string L1_DisconnectionMSGPrintingIMMEDIATE { get; set; }
        public string L1_BillingCode { get; set; }
        public string L1_Customer_PAN { get; set; }
        public string L2_Name { get; set; }
        public string L3_Addr1 { get; set; }
        public string L4_Addr2 { get; set; }
        public string L5_Addr3 { get; set; }
        public string L6_SERVDET_SERVNO { get; set; }
        public string L6_SERVDET_SANC_LOAD { get; set; }
       
        public string L6_bill_demand { get; set; }
        public string L6_ACTUAL_DEMAND { get; set; }
        public string L6_TARIFF_DESCR { get; set; }
        public string L6_EXCESS_DEMAND { get; set; }
        public string L6_SUPPLY_VOLTAGE { get; set; }
        public string L6_BILL_PF { get; set; }
        public string L6_MTRDET_LF_PERC { get; set; }
        public string L6_BILL_TYPE { get; set; }
        public string L6_MeasureContractDemand { get; set; }
        public string L6_Kvah_Indicator { get; set; }
        public string L6_LT_Metering_Flag { get; set; }
        
        public string L7_Due_Date { get; set; }
        public string L7_BillDt { get; set; }
        public string L7_PrevReadDt { get; set; }
        public string L7_ReaDt { get; set; }
        public string L7_LastPymtDate { get; set; }
        public string L7_LastPayementAmount { get; set; }
        public string L7_LastPayementMode { get; set; }
       
        public string L8_FixedCharge { get; set; }
        public string L8_EnergyCharge { get; set; }
        public string L8_TODCharges { get; set; }
        public string L8_ACCharge { get; set; }
        public string L8_GovTax { get; set; }
        public string L8_MinCharge { get; set; }
        public string L8_ServdetTotbBdtOthr { get; set; }
        public string L8_RegulatoryCharge_1 { get; set; }
        public string L8_RegulatoryCharge_2 { get; set; }
        public string L8_RebateIncurredCurrentMonth { get; set; }
        public string L8_AmountPayableBeforeDueDate { get; set; }
        public string L8_TNo { get; set; }
      
        public string L8_ParkingAmount { get; set; }
        public string L8_Subsidy_Charges { get; set; }
       
        public string L8_Solar_Export_Energy { get; set; }
        public string L8_GreenTariff_Charges { get; set; }

        public string L8_Intrest_Amount { get; set; }

        public string L9_TotDbArr { get; set; }
        public string L9_CurrBillAmt { get; set; }
        public string L9_Int_Tpl { get; set; }
        public string L9_ArrsTpl { get; set; }
        public string L9_CurrBillAmtIntTplArrsTpl { get; set; }
        public string L9_Amount_Payable { get; set; }
    
        public string L10_LFincentive { get; set; }
        public string L10_DisconnDate { get; set; }
        public string L10_TotArrUPPCLIntUPPCLIntArrUPPCL { get; set; }
        public string L10_TotArrUPPCLIntUPPCLIntArrUPPCL_Rounded { get; set; }
        public string L10_SecDeptBdt { get; set; }
        public string L10_DmdChgPenalty { get; set; }
        public string L10_UPPCL_ArrearAmount { get; set; }
        public string L10_UPPCLIntOnArrearAmount { get; set; }
        public string L10_Mode { get; set; }

        public string L11_MTRSNO_1 { get; set; }
        public string L11_MTRSNO_2_IF_AVAILABLE{ get; set; }

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
        public string L33_ForGST { get; set; }
        public string L34_ForGST { get; set; }
        public string L35_ForGST { get; set; }
        public string L36_ForGST { get; set; }
        public string L37_Last_13_months_Power_factor_for_graph { get; set; }
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
        public string MVPicture { get; set; }





        public System.Data.DataTable KVAgrph { get; set; }
        public System.Data.DataTable KVAHgrph { get; set; }
        public System.Data.DataTable PFgrph { get; set; }

        public string unit { get; set; }

       




        public List<BillDetails> BillDetails { get; set; }
        public System.Data.DataTable dtMessage { get; set; }

        //public string BillType { get; set; }
        //public string BillType { get; set; }
        //public string BillType { get; set; }
        //public string BillType { get; set; }
        //public string BillType { get; set; }
        //public string BillType { get; set; }
        //public string BillType { get; set; }
        //public string BillType { get; set; }
        //public string BillType { get; set; }
        //public string BillType { get; set; }
        //public string BillType { get; set; }

        //public string OwnerAddress_1 { get; set; }
        //public string OwnerAddress_2 { get; set; }
    }
}
