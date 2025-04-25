using AT.Print.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Drawing.Printing.PrinterSettings;

namespace AT.Print
{
    public partial class Print_Solar_HT : UserControl
    {
        public Print_Solar_HT()
        {
            InitializeComponent();
            BindPrinters();
        }
        string textFileName;
        string mVImagePath;
        string ServiceNo = "";
        string LineNo = "";
        DataTable TemplateConditionalWithSTHindi = ParseAsDataTable.TemplateConditionalWithSTHindi();
        DataTable TemplateConditionalWithSTEnglish = ParseAsDataTable.TemplateConditionalWithSTEnglish();
        DataTable TemplateConditionalWithServiceNoHindi = ParseAsDataTable.TemplateConditionalWithServiceNoHindi();
        DataTable TemplateConditionalWithServiceNoEnglish = ParseAsDataTable.TemplateConditionalWithServiceNoEnglish();

        public void BindPrinters()
        {
            PaperSource pkSource;

            PrintDocument printDoc = new PrintDocument();

            foreach (var printers in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                cbDefaultPrinter.Properties.Items.Add(printers);
            }
            for (int i = 0; i < printDoc.PrinterSettings.PaperSources.Count; i++)
            {
                pkSource = printDoc.PrinterSettings.PaperSources[i];
                cbNonTODTraySource.Properties.Items.Add(pkSource.SourceName);
            }
        }


        string[] Solar_Bill_HT;

        private void SbPrintBill_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {

                ofd.Title = "Select bill text(*.txt) file ";
                ofd.Multiselect = false;
                ofd.Filter = "txt Files|*.txt";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textFileName = ofd.SafeFileName.ToUpper().Replace(".TXT", "");

                    string contents = File.ReadAllText(ofd.FileName);
                    if (contents.StartsWith("HT")) 
                    {
                        Solar_Bill_HT = contents.Split(new String[] { "HT " }, StringSplitOptions.RemoveEmptyEntries);
                        if (!select_mVImg())
                        {
                            AppFunctions.CloseWaitForm();
                            return;
                        }
                        XtraMessageBox.Show("Total Bills in this file: " + Solar_Bill_HT.Length.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        var sb = sender as SimpleButton;
                        if (ValidatetxtFile(Solar_Bill_HT))
                        {
                            StartPrinting_HT_Solar_Bills(Solar_Bill_HT, sb.Name);
                        }
                        else
                        {
                            AppFunctions.CloseWaitForm();
                            return;
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("It seeems that you have chosen a wrong file,\n try again and pick correct file!!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
        }

        private bool select_mVImg()
        {
            using (OpenFileDialog ofdMv = new OpenFileDialog())
            {
                ofdMv.Title = "Select Mobile Van Image ";
                ofdMv.Multiselect = false;
                ofdMv.Filter = "All Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
                if (ofdMv.ShowDialog() == DialogResult.OK)
                {
                    mVImagePath = ofdMv.FileName;
                    return true;
                }
                else
                {
                    XtraMessageBox.Show("Canceled the selection.");
                    AppFunctions.CloseWaitForm();
                    return false;
                }
            }
        }

        private void StartPrinting_HT_Solar_Bills(string[] Bills, string Name)
        {
            int BillNo = 1,  ParsedBills = 0;
            DataTable dtSolarBill=new DataTable();
            string FileName = AppFunctions.ProcessedBillData();
            XtraReport collectorReport = new XtraReport
            {
                DisplayName = "HT Solar Print",
            };
            foreach (var Bill in Bills)
            {
                try
                {
                    AppFunctions.ShowWaitForm("Loading...");
                    List<Solar_Bill_HT> lstformattedbills = new List<Solar_Bill_HT>();

                   dtSolarBill = ParseAsDataTable.LTMD_Solar_FileTxtToDataTable(Bill);
                   
                    Solar_Bill_HT sht = parseSolarBill(dtSolarBill);
                     sht.MVPicture = mVImagePath;
                     lstformattedbills.Add(sht);

                    #region HT_solar-PDF 

                    if (Name == "sbSavePDF")
                    {
                        AT.Print.PDF.Rpt_HT_Solar_back_PDF rpts = new AT.Print.PDF.Rpt_HT_Solar_back_PDF
                        {
                            DataSource = lstformattedbills,
                        };

                        AT.Print.PDF.Rpt_HT_Solar_PDF rptsd = new AT.Print.PDF.Rpt_HT_Solar_PDF(rpts)
                        {
                            DataSource = lstformattedbills,
                        };

                        #region WaterMark Picture Front Page PDF Solar
                        DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkFrontSolar = new DevExpress.XtraPrinting.Drawing.Watermark();
                        pictureWatermarkFrontSolar.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Solar_Front_Page.jpg");
                        pictureWatermarkFrontSolar.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        pictureWatermarkFrontSolar.ImageTiling = false;
                        pictureWatermarkFrontSolar.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Zoom;
                        pictureWatermarkFrontSolar.ImageTransparency = 0;
                        pictureWatermarkFrontSolar.ShowBehind = true;
                        rptsd.Watermark.CopyFrom(pictureWatermarkFrontSolar);
                        #endregion

                        rptsd.CreateDocument(false);
                        
                        #region WaterMark Picture Back Page PDF Solar
                        DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkBackSolar = new DevExpress.XtraPrinting.Drawing.Watermark();
                        pictureWatermarkBackSolar.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Solar_Back_Page.jpg");
                        pictureWatermarkBackSolar.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        pictureWatermarkBackSolar.ImageTiling = false;
                        pictureWatermarkBackSolar.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Zoom;
                        pictureWatermarkBackSolar.ImageTransparency = 0;
                        pictureWatermarkBackSolar.ShowBehind = true;
                        rpts.Watermark.CopyFrom(pictureWatermarkBackSolar);
                        #endregion

                        rpts.CreateDocument(false);
                        rptsd.ModifyDocument(x => { x.AddPages(rpts.Pages); });
                        DevExpress.XtraPrinting.Page myPage2 = rptsd.Pages[1];
                        myPage2.AssignWatermark(pictureWatermarkBackSolar);
                        string billdate = lstformattedbills.FirstOrDefault().L1_MONTH_YEAR;
                        string ServiceNo = lstformattedbills.FirstOrDefault().L6_SERVDET_SERVNO;
                        var outputfolder = "C://Bills//HT_Solar_Files//" + billdate + "//" + textFileName;
                        OutputFolderPath OFP = new OutputFolderPath();
                        outputfolder = OFP.LoadLocation() + "//HT_Solar_Files//" + billdate + "//" + textFileName; ;
                        if (!Directory.Exists(outputfolder))
                            Directory.CreateDirectory(outputfolder);

                        if (Directory.Exists(outputfolder))
                        {
                            rptsd.ExportToPdf(outputfolder + "//" + ServiceNo + ".pdf");
                        }
                        ParsedBills++;
                        AppFunctions.CloseWaitForm();
                    }
                    #endregion



                    #region Print HT_solar 

                        else 
                        {
                         PrinterSettings ps = new PrinterSettings() { PrinterName = cbDefaultPrinter.Text };
                         using (Graphics g = ps.CreateMeasurementGraphics(ps.DefaultPageSettings))
                         {
                             Margins MinMargins = DevExpress.XtraPrinting.Native.DeviceCaps.GetMinMargins(g);
                             Console.WriteLine("Minimum Margins for " + ps.PrinterName + ": " + MinMargins.ToString());
                         }
                        AT.Print.Rpt_HT_Solar_Print_Back rptb = new Rpt_HT_Solar_Print_Back
                        {
                            DataSource = lstformattedbills,
                        };

                        AT.Print.Rpt_HT_Solar_Print rpta = new Rpt_HT_Solar_Print(rptb)
                         {
                             DataSource = lstformattedbills,
                             DisplayName = sht.L6_SERVDET_SERVNO,
                         };
                         rpta.Watermark.ImageTransparency = 255;
                         rpta.PrinterName = cbDefaultPrinter.SelectedItem.ToString();    
                         rpta.PrintingSystem.Document.Name = sht.L6_SERVDET_SERVNO;
                         rpta.CreateDocument();
                         rptb.CreateDocument();
                         rpta.ModifyDocument(x => { x.AddPages(rptb.Pages); });
                         rpta.PrintingSystem.StartPrint += NonTOD_StartPrint;
                         rpta.Print(cbDefaultPrinter.Text);
                        
                         AppFunctions.CloseWaitForm();
                        
                         ParsedBills++;
                    }
                    #endregion
                }
                catch (System.OutOfMemoryException)
                {
                    AppFunctions.LogError("Error Parsing Service No. " + ServiceNo + " of the given file due to out of memory.");
                    AppFunctions.LogProcessedBill(Convert.ToString(dtSolarBill.Rows[0][1]), Convert.ToString(dtSolarBill.Rows[0][4]), Convert.ToString(dtSolarBill.Rows[0][2]), Convert.ToString(dtSolarBill.Rows[0][5]), ServiceNo, FileName, "No");
                    System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                    GC.Collect();
                    GC.RemoveMemoryPressure(1024 * 1024);
                    break;
                }
                catch (Exception ex)
                {
                    AppFunctions.LogError(ex);
                    AppFunctions.LogProcessedBill(Convert.ToString(dtSolarBill.Rows[0][1]), Convert.ToString(dtSolarBill.Rows[0][4]), Convert.ToString(dtSolarBill.Rows[0][2]), Convert.ToString(dtSolarBill.Rows[0][5]), ServiceNo, FileName, "No");
                    AppFunctions.CloseWaitForm();
                    XtraMessageBox.Show("Error Parsing Bill " + BillNo + " of the given file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.Message);
                }
                AppFunctions.LogProcessedBill(Convert.ToString(dtSolarBill.Rows[0][1]), Convert.ToString(dtSolarBill.Rows[0][4]), Convert.ToString(dtSolarBill.Rows[0][2]), Convert.ToString(dtSolarBill.Rows[0][5]), ServiceNo, FileName, "Yes");
                BillNo++;
            }
            //AppFunctions.CloseWaitForm();
            XtraMessageBox.Show(ParsedBills + " Bills Parsed Successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        Solar_Bill_HT parseSolarBill(DataTable dtSolarBill)
        {

            Solar_Bill_HT sht = new Solar_Bill_HT();
            #region --Lines
            ServiceNo = dtSolarBill.Rows[5][0].ToString();
            #region Line-1
            LineNo = "1";
            sht.L1_Bill_Type = "HT";
            sht.L1_MONTH_YEAR = dtSolarBill.Rows[0][0].ToString();
            sht.L1_ZONE = dtSolarBill.Rows[0][1].ToString();
            sht.L1_BU = dtSolarBill.Rows[0][2].ToString();
            sht.L1_PC = dtSolarBill.Rows[0][3].ToString();
            sht.L1_route = dtSolarBill.Rows[0][4].ToString();
            sht.L1_Bill_seq_no = dtSolarBill.Rows[0][5].ToString();
            if (dtSolarBill.Rows[0][9].ToString() == "")
            {
                sht.L1_Customer_PAN = dtSolarBill.Rows[0][9].ToString();
            }
            else
            {
                sht.L1_Customer_PAN = "PAN: " + dtSolarBill.Rows[0][9].ToString();
            }
            #endregion

            #region Line-2
            LineNo = "2";
            sht.L2_NAME = dtSolarBill.Rows[1][0].ToString().Trim('�');
            #endregion

            #region Line-3
            LineNo = "3";
            sht.L3_ADDR1 = dtSolarBill.Rows[2][0].ToString().Trim('�');
            #endregion

            #region Line-4
            LineNo = "4";
            sht.L4_ADDR2 = dtSolarBill.Rows[3][0].ToString().Trim('�');
            #endregion

            #region Line-5
            LineNo = "5";
            sht.L5_ADDR3 = dtSolarBill.Rows[4][0].ToString().Trim('�');
            #endregion

            #region Line-6
            LineNo = "6";
            sht.L6_MEASURE_OF_CONTRACT_Demand = dtSolarBill.Rows[5][10].ToString();
            sht.L6_SERVDET_SERVNO = dtSolarBill.Rows[5][0].ToString();
            sht.L6_SERVDET_SANC_LOAD = dtSolarBill.Rows[5][1].ToString();
            sht.L6_Bill_Demand = dtSolarBill.Rows[5][2].ToString();
            sht.L6_ACTUAL_DEMAND = dtSolarBill.Rows[5][3].ToString();
            sht.L6_TARIFF_DESCR = dtSolarBill.Rows[5][4].ToString(); 
            sht.L6_EXCESS_DEMAND = dtSolarBill.Rows[5][5].ToString();
            sht.L6_SUPPLY_VOLTAGE = dtSolarBill.Rows[5][6].ToString();
            sht.L6_Avg_Power_Factor = dtSolarBill.Rows[5][7].ToString();
            sht.L6_MTRDET_LF_PERC = dtSolarBill.Rows[5][8].ToString(); 
            sht.L6_Bill_Type_Assess_OR_normal = dtSolarBill.Rows[5][9].ToString();
            sht.L6_Kvah_indicator = dtSolarBill.Rows[5][11].ToString();
            sht.L6_LT_Metering_Flag = dtSolarBill.Rows[5][12].ToString();
            #endregion

            #region Line-7
            LineNo = "7";
            sht.L7_due_date = dtSolarBill.Rows[6][0].ToString();
            sht.L7_Billdt = dtSolarBill.Rows[6][1].ToString();
            int YYYY, MM, DD;
            YYYY = int.Parse(dtSolarBill.Rows[6][2].ToString().Split('-')[2]);
            MM = int.Parse(dtSolarBill.Rows[6][2].ToString().Split('-')[1]);
            DD = int.Parse(dtSolarBill.Rows[6][2].ToString().Split('-')[0]);
            DateTime PreviousDate = new DateTime(YYYY, MM, DD);
            sht.L7_PrevReadDt = (PreviousDate.AddDays(-1)).ToString("dd-MM-yy");

            sht.L7_readt = dtSolarBill.Rows[6][3].ToString();
            sht.L7_LastpymtDate = dtSolarBill.Rows[6][4].ToString();
            sht.L7_Last_Payement_amount = dtSolarBill.Rows[6][5].ToString();

            #endregion

            #region Line-8
            LineNo = "8";
            sht.L8_FixedCharge = dtSolarBill.Rows[7][0].ToString();
            sht.L8_EnergyCharge = dtSolarBill.Rows[7][1].ToString();
            sht.L8_TOD_Charges = dtSolarBill.Rows[7][2].ToString();
            sht.L8_TOD_Charges = sht.L8_TOD_Charges.Contains("-") ? ("-" + sht.L8_TOD_Charges.Replace("-", "")) : sht.L8_TOD_Charges;
            sht.L8_AC_Charges = dtSolarBill.Rows[7][3].ToString();
            sht.L8_GovTax = dtSolarBill.Rows[7][4].ToString();
            sht.L8_min_charge = dtSolarBill.Rows[7][5].ToString();
            sht.L8_SERVDET_TOTDB_BDT_OTHER = dtSolarBill.Rows[7][6].ToString();
            sht.L8_Regulatory_Charge1 = dtSolarBill.Rows[7][7].ToString();
            sht.L8_Regulatory_Charge2 = dtSolarBill.Rows[7][8].ToString();
            sht.L8_Rebate_incurred_of_current_month = dtSolarBill.Rows[7][9].ToString();
            sht.L8_amount_payable_before_due_date = dtSolarBill.Rows[7][10].ToString();
            
            sht.L8_amount_payable_before_due_date = string.IsNullOrEmpty(sht.L8_amount_payable_before_due_date) ? "0" : sht.L8_amount_payable_before_due_date;
            sht.L8_amount_payable_before_due_date = sht.L8_amount_payable_before_due_date.Contains("CR") ? ("-" + sht.L8_amount_payable_before_due_date.Replace("CR", "")) : (sht.L8_amount_payable_before_due_date.Contains("-") ? ("-" + sht.L8_amount_payable_before_due_date.Replace("-", "")) : sht.L8_amount_payable_before_due_date);

            
            sht.L8_T_No = dtSolarBill.Rows[7][11].ToString().Trim('�');
            sht.L8_Subsidy_Charges = dtSolarBill.Rows[7][13].ToString();
            sht.L8_Solar_Export_Energy = dtSolarBill.Rows[7][14].ToString();
            sht.L8_GreenTariff_Charges = dtSolarBill.Rows[7][15].ToString();
          
            #endregion

            #region Line-9
            LineNo = "9";
            sht.L9_TOT_DB_ARR = dtSolarBill.Rows[8][0].ToString();
            sht.L9_CurrBillamt = dtSolarBill.Rows[8][1].ToString();
            sht.L9_CurrBillamt = sht.L9_CurrBillamt.Contains("-") ? ("-" + sht.L9_CurrBillamt.Replace("-", "")) : sht.L9_CurrBillamt;
            sht.L9_INT_TPL = dtSolarBill.Rows[8][2].ToString();
            sht.L9_Arrs_TPL = dtSolarBill.Rows[8][3].ToString();
            sht.L9_nCurrBillamt_INT_TPL_ARRS_TPL = dtSolarBill.Rows[8][4].ToString();
            sht.L9_Total_Bill_payable_rounded = dtSolarBill.Rows[8][5].ToString();

            if (Convert.ToDouble(dtSolarBill.Rows[8][5].ToString()) < 0)
            {
                sht.L9_Total_Bill_payable_rounded = "NOT TO PAY";
            }

            #endregion

            #region Line-10
            LineNo = "10";
            sht.L10_LFincentive = dtSolarBill.Rows[9][0].ToString();
            sht.L10_DISCONN_DATE_date = dtSolarBill.Rows[9][1].ToString();
            sht.L10_TOTARR_UPPCL_INT_UPPCL_INTARR_UPPCL = dtSolarBill.Rows[9][2].ToString();
            sht.L10_TotArrUPPCLIntUPPCLIntArrUPPCL_Rounded = string.IsNullOrEmpty(sht.L10_TOTARR_UPPCL_INT_UPPCL_INTARR_UPPCL) ? "0" : Math.Round(Convert.ToDecimal(sht.L10_TOTARR_UPPCL_INT_UPPCL_INTARR_UPPCL) + (decimal).49, 0, MidpointRounding.AwayFromZero).ToString();
            sht.L10_SECDEPT_BDT = dtSolarBill.Rows[9][3].ToString();
            sht.L10_DMDCHG_PENALTY = dtSolarBill.Rows[9][4].ToString();
            sht.L10_UPPCL_Arrear_Amount = dtSolarBill.Rows[9][5].ToString();
            sht.L10_UPPCL_Int_on_Arrear_Amount = dtSolarBill.Rows[9][6].ToString();
            sht.L10_Mode = dtSolarBill.Rows[9][7].ToString();


            #endregion

            #region Line-11
            LineNo = "11";
            sht.L11_MTRSNO_METER1 = dtSolarBill.Rows[10][0].ToString();
            sht.L11_MTRSNO_METER_2_IF_AVAILABLE = dtSolarBill.Rows[10][1].ToString();
            #endregion

            #region Line-12
            LineNo = "12";
            sht.L12_KWH_PRESREAD  = dtSolarBill.Rows[11][0].ToString();
            sht.L12_KVAH_PRESREAD = dtSolarBill.Rows[11][1].ToString();
            sht.L12_KVA_PRESREAD  = dtSolarBill.Rows[11][2].ToString();
            #endregion

            #region Line-13
            LineNo = "13";
            sht.L13_KWH_PASTREAD   = dtSolarBill.Rows[12][0].ToString();
            sht.L13_KVAH_PASTREAD  = dtSolarBill.Rows[12][1].ToString();
            sht.L13_KVA_PASTREAD   = dtSolarBill.Rows[12][2].ToString();
            sht.L13_Purpose        = dtSolarBill.Rows[12][3].ToString();
            #endregion

            #region Line-14
            LineNo = "14";
            sht.L14_Multiplying_factor_KWH   = dtSolarBill.Rows[13][0].ToString();
            sht.L14_Multiplying_factor_KVAH  = dtSolarBill.Rows[13][1].ToString();
            sht.L14_Multiplying_factor_KVA   = dtSolarBill.Rows[13][2].ToString();
            #endregion

            #region Line-15
            LineNo = "15";
            sht.L15_KWH_UNITS  = dtSolarBill.Rows[14][0].ToString();
            sht.L15_KVAH_UNITS = dtSolarBill.Rows[14][1].ToString(); 
            sht.L15_KVA_UNITS  = dtSolarBill.Rows[14][2].ToString();
            #endregion

            #region Line-16
            LineNo = "16";
            sht.L16_TOD1_KVAH_Units  = dtSolarBill.Rows[15][0].ToString();
            sht.L16_TOD2_KVAH_Units  = dtSolarBill.Rows[15][1].ToString();
            sht.L16_TOD3_KVAH_Units  = dtSolarBill.Rows[15][2].ToString();
            sht.L16_TOD4_KVAH_Units  = dtSolarBill.Rows[15][3].ToString();
            #endregion

            #region Line-17
            LineNo = "17";
            sht.L17_TOD1_KVA_Units   = dtSolarBill.Rows[16][0].ToString();
            sht.L17_TOD2_KVA_Units   = dtSolarBill.Rows[16][1].ToString();
            sht.L17_TOD3_KVA_Units   = dtSolarBill.Rows[16][2].ToString();
            sht.L17_TOD4_KVA_Units   = dtSolarBill.Rows[16][3].ToString();
            #endregion

            #region Line-18
            LineNo = "18";
            sht.L18_KWH_PRESREAD    = dtSolarBill.Rows[17][0].ToString();
            sht.L18_KVAH_PRESREAD   = dtSolarBill.Rows[17][1].ToString();
            sht.L18_KVA_PRESREAD    = dtSolarBill.Rows[17][2].ToString();
            #endregion

            #region Line-19
            LineNo = "19";
            sht.L19_KWH_PASTREAD    = dtSolarBill.Rows[18][0].ToString();
            sht.L19_KVAH_PASTREAD   = dtSolarBill.Rows[18][1].ToString();
            sht.L19_KVA_PASTREAD    = dtSolarBill.Rows[18][2].ToString();
            #endregion

            #region Line-20
            LineNo = "20";
            sht.L20_Multiplying_Factor_KWH   = dtSolarBill.Rows[19][0].ToString();
            sht.L20_Multiplying_Factor_KVAH  = dtSolarBill.Rows[19][1].ToString();
            sht.L20_Multiplying_Factor_KVA   = dtSolarBill.Rows[19][2].ToString();
            #endregion

            #region Line-21
            LineNo = "21";
            sht.L21_KWH_UNITS   = dtSolarBill.Rows[20][0].ToString();
            sht.L21_KVAH_UNITS  = dtSolarBill.Rows[20][1].ToString();
            sht.L21_KVA_UNITS   = dtSolarBill.Rows[20][2].ToString();
            #endregion

            #region Line-22
            LineNo = "22";
            sht.L22_TOD1_KVAH_Units   = dtSolarBill.Rows[21][0].ToString();
             sht.L22_TOD2_KVAH_Units   = dtSolarBill.Rows[21][1].ToString();
             sht.L22_TOD3_KVAH_Units   = dtSolarBill.Rows[21][2].ToString();
             sht.L22_TOD4_KVAH_Units   = dtSolarBill.Rows[21][3].ToString();
            #endregion

            #region Line-23
            LineNo = "23";
            sht.L23_TOD1_KVA_Units  = dtSolarBill.Rows[22][0].ToString();
           sht.L23_TOD2_KVA_Units  = dtSolarBill.Rows[22][1].ToString();
           sht.L23_TOD3_KVA_Units  = dtSolarBill.Rows[22][2].ToString();
           sht.L23_TOD4_KVA_Units  = dtSolarBill.Rows[22][3].ToString();
            #endregion

            #region Line-24
            LineNo = "24";
            sht.L24_MonYear_1    = dtSolarBill.Rows[23][0].ToString();
            sht.L24_KVA_UNITS_1  = dtSolarBill.Rows[23][1].ToString();
            sht.L24_MonYear_2    = dtSolarBill.Rows[23][2].ToString();
            sht.L24_KVA_UNITS_2  = dtSolarBill.Rows[23][3].ToString();
            sht.L24_MonYear_3    = dtSolarBill.Rows[23][4].ToString();
            sht.L24_KVA_UNITS_3  = dtSolarBill.Rows[23][5].ToString();
            sht.L24_MonYear_4    = dtSolarBill.Rows[23][6].ToString();
            sht.L24_KVA_UNITS_4  = dtSolarBill.Rows[23][7].ToString();
            sht.L24_MonYear_5    = dtSolarBill.Rows[23][8].ToString();
            sht.L24_KVA_UNITS_5  = dtSolarBill.Rows[23][9].ToString();
            sht.L24_MonYear_6    = dtSolarBill.Rows[23][10].ToString();
            sht.L24_KVA_UNITS_6  = dtSolarBill.Rows[23][11].ToString();
            sht.L24_MonYear_7    = dtSolarBill.Rows[23][12].ToString();
            sht.L24_KVA_UNITS_7  = dtSolarBill.Rows[23][13].ToString();
            #endregion


            #region Line-25
            LineNo = "25";
            sht.L25_MonYear_1    = dtSolarBill.Rows[24][0].ToString();
            sht.L25_KVAH_UNITS_1 = dtSolarBill.Rows[24][1].ToString();
            sht.L25_MonYear_2    = dtSolarBill.Rows[24][2].ToString();
            sht.L25_KVAH_UNITS_2 = dtSolarBill.Rows[24][3].ToString();
            sht.L25_MonYear_3    = dtSolarBill.Rows[24][4].ToString();
            sht.L25_KVAH_UNITS_3 = dtSolarBill.Rows[24][5].ToString();
            sht.L25_MonYear_4    = dtSolarBill.Rows[24][6].ToString();
            sht.L25_KVAH_UNITS_4 = dtSolarBill.Rows[24][7].ToString();
            sht.L25_MonYear_5    = dtSolarBill.Rows[24][8].ToString();
            sht.L25_KVAH_UNITS_5 = dtSolarBill.Rows[24][9].ToString();
            sht.L25_MonYear_6    = dtSolarBill.Rows[24][10].ToString();
            sht.L25_KVAH_UNITS_6 = dtSolarBill.Rows[24][11].ToString();
            sht.L25_MonYear_7    = dtSolarBill.Rows[24][12].ToString();
            sht.L25_KVAH_UNITS_7 = dtSolarBill.Rows[24][13].ToString();
            #endregion


            #region Lines-26-31
            LineNo = "26";
            sht.L26_MESSAGE1 = dtSolarBill.Rows[25][0].ToString();
            LineNo = "27";
            sht.L27_MESSAGE2 = dtSolarBill.Rows[26][0].ToString();
            LineNo = "28";
            sht.L28_MESSAGE3 = dtSolarBill.Rows[27][0].ToString();
            LineNo = "29";
            sht.L29_MESSAGE4 = dtSolarBill.Rows[28][0].ToString();
            LineNo = "30";
            sht.L30_MESSAGE5 = dtSolarBill.Rows[29][0].ToString();
            LineNo = "31";
            sht.L31_MESSAGE6 = dtSolarBill.Rows[30][0].ToString();
            #endregion

            #region TemplateConditionalWithSTHindi
            DataView DVTemplateConditionalWithSTHindi = new DataView();
            DVTemplateConditionalWithSTHindi = TemplateConditionalWithSTHindi.DefaultView;
            DVTemplateConditionalWithSTHindi.RowFilter = "[1] = '" + sht.L6_TARIFF_DESCR + "'";
            LineNo = "6";
            DataTable TemplateConditionalWithSTHindiCopy = DVTemplateConditionalWithSTHindi.ToTable();
            for (int i = 0; i < TemplateConditionalWithSTHindiCopy.Rows.Count; i++)
            {
                if (sht.L6_MEASURE_OF_CONTRACT_Demand.ToUpper() == "KW" && Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD) >= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
                {
                    sht.L33_MESSAGE7 += TemplateConditionalWithSTHindiCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (sht.L6_MEASURE_OF_CONTRACT_Demand.ToUpper() == "KVA" && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD))*(0.9)) >= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD))*(0.9)) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
                {
                    sht.L33_MESSAGE7 += TemplateConditionalWithSTHindiCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (sht.L6_MEASURE_OF_CONTRACT_Demand.ToUpper() == "HP" && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD))*(0.746)) >= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD))*(0.746)) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
                {
                    sht.L33_MESSAGE7 += TemplateConditionalWithSTHindiCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
            }
            if (!string.IsNullOrEmpty(sht.L33_MESSAGE7))
            {
                sht.L33_MESSAGE7 = sht.L33_MESSAGE7.Replace('�', ' ');
                sht.L33_MESSAGE7 = sht.L33_MESSAGE7.TrimEnd(' ');
                sht.L33_MESSAGE7 = sht.L33_MESSAGE7.TrimEnd('\r');
                sht.L33_MESSAGE7 = sht.L33_MESSAGE7.TrimEnd('\n');
            }
            #endregion

            #region TemplateConditionalWithSTEnglish
            DataView DVTemplateConditionalWithSTEnglish = new DataView();
            DVTemplateConditionalWithSTEnglish = TemplateConditionalWithSTEnglish.DefaultView;
            DVTemplateConditionalWithSTEnglish.RowFilter = "[1] = '" + sht.L6_TARIFF_DESCR + "'";

            DataTable TemplateConditionalWithSTEnglishCopy = DVTemplateConditionalWithSTEnglish.ToTable();
            for (int i = 0; i < TemplateConditionalWithSTEnglishCopy.Rows.Count; i++)
            {
                if (sht.L6_MEASURE_OF_CONTRACT_Demand.ToUpper() == "KW" && Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
                {
                    sht.L34_MESSAGE8 += TemplateConditionalWithSTEnglishCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (sht.L6_MEASURE_OF_CONTRACT_Demand.ToUpper() == "KVA" && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD))*(0.9)) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD))*(0.9)) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
                {
                    sht.L34_MESSAGE8 += TemplateConditionalWithSTEnglishCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (sht.L6_MEASURE_OF_CONTRACT_Demand.ToUpper() == "HP" && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD))*(0.9)) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD))*(0.746)) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
                {
                    sht.L34_MESSAGE8 += TemplateConditionalWithSTEnglishCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
            }
            if (!string.IsNullOrEmpty(sht.L34_MESSAGE8))
            {
                sht.L34_MESSAGE8 = sht.L34_MESSAGE8.TrimEnd(' ');
                sht.L34_MESSAGE8 = sht.L34_MESSAGE8.TrimEnd('\r');
                sht.L34_MESSAGE8 = sht.L34_MESSAGE8.TrimEnd('\n');
            }
            #endregion

            #region TemplateConditionalWithServiceNoHindi
            DataView DVTemplateConditionalWithServiceNoHindi = new DataView();
            DVTemplateConditionalWithServiceNoHindi = TemplateConditionalWithServiceNoHindi.DefaultView;
            DVTemplateConditionalWithServiceNoHindi.RowFilter = "[1] = '" + sht.L6_SERVDET_SERVNO + "'";

            DataTable TemplateConditionalWithServiceNoHindiCopy = DVTemplateConditionalWithServiceNoHindi.ToTable();
            for (int i = 0; i < TemplateConditionalWithServiceNoHindiCopy.Rows.Count; i++)
            {
                sht.L35_MESSAGE9 += TemplateConditionalWithServiceNoHindiCopy.Rows[i]["2"].ToString().Trim('�') + " \r\n";
            }
            if (!string.IsNullOrEmpty(sht.L35_MESSAGE9))
            {
                sht.L35_MESSAGE9 = sht.L35_MESSAGE9.Replace('�', ' ');
                sht.L35_MESSAGE9 = sht.L35_MESSAGE9.TrimEnd(' ');
                sht.L35_MESSAGE9 = sht.L35_MESSAGE9.TrimEnd('\r');
                sht.L35_MESSAGE9 = sht.L35_MESSAGE9.TrimEnd('\n');
            }
            #endregion

            #region TemplateConditionalWithServiceNoEnglish
            DataView DVTemplateConditionalWithServiceNoEnglish = new DataView();
            DVTemplateConditionalWithServiceNoEnglish = TemplateConditionalWithServiceNoEnglish.DefaultView;
            DVTemplateConditionalWithServiceNoEnglish.RowFilter = "[1] = '" + sht.L6_SERVDET_SERVNO + "'";

            DataTable TemplateConditionalWithServiceNoEnglishCopy = DVTemplateConditionalWithServiceNoEnglish.ToTable();
            for (int i = 0; i < TemplateConditionalWithServiceNoEnglishCopy.Rows.Count; i++)
            {
                sht.L36_MESSAGE10 += TemplateConditionalWithServiceNoEnglishCopy.Rows[i]["2"].ToString().Trim('�') + " \r\n";
            }
            if (!string.IsNullOrEmpty(sht.L36_MESSAGE10))
            {
                sht.L36_MESSAGE10 = sht.L36_MESSAGE10.TrimEnd('\n');
                sht.L36_MESSAGE10 = sht.L36_MESSAGE10.TrimEnd('\r');
                sht.L36_MESSAGE10 = sht.L36_MESSAGE10.TrimEnd(' ');
            }
            #endregion


            #region Line-32
            LineNo = "32";
            sht.L32_BarCode = dtSolarBill.Rows[31][0].ToString();
            #endregion

            #region Line-33
            LineNo = "33";
            sht.L33_Exp_KWH_UNITS = dtSolarBill.Rows[32][0].ToString();
            sht.L33_Exp_Past_KWH_UNITS = dtSolarBill.Rows[32][1].ToString();
            sht.L33_Exp_Present_KWH_UNITS = dtSolarBill.Rows[32][2].ToString();
            sht.L33_Exp_KVAH_UNITS = dtSolarBill.Rows[32][3].ToString();
            sht.L33_Exp_Past_KVAH_UNITS = dtSolarBill.Rows[32][4].ToString();
            sht.L33_Exp_Present_KVAH_UNITS = dtSolarBill.Rows[32][5].ToString();
            sht.L33_Exp_KVA_UNITS = dtSolarBill.Rows[32][6].ToString();
            sht.L33_Exp_Past_KVA_UNITS = dtSolarBill.Rows[32][7].ToString();
            sht.L33_Exp_Present_KVA_UNITS = dtSolarBill.Rows[32][8].ToString();
            sht.L33_Exp_CURRENT_NET_EXPORT_KVA_UNITS = dtSolarBill.Rows[32][9].ToString();
            sht.L33_Exp_CURRENT_NET_EXPORT_KVAH_UNITS = dtSolarBill.Rows[32][10].ToString();
            sht.L33_Exp_CURRENT_NET_EXPORT_KWH_UNITS = dtSolarBill.Rows[32][11].ToString();
            sht.L33_Exp_KW_UNITS = dtSolarBill.Rows[32][12].ToString();
            sht.L33_Exp_Past_KW_UNITS = dtSolarBill.Rows[32][13].ToString();
            sht.L33_Exp_Present_KW_UNITS = dtSolarBill.Rows[32][14].ToString();
            #endregion

            #region Line-34
            LineNo = "34";
            sht.L34_Exp_TOD1_KWH_Units = dtSolarBill.Rows[33][0].ToString();
            sht.L34_Exp_TOD2_KWH_Units = dtSolarBill.Rows[33][1].ToString();
            sht.L34_Exp_TOD3_KWH_Units = dtSolarBill.Rows[33][2].ToString();
            sht.L34_Exp_TOD4_KWH_Units = dtSolarBill.Rows[33][3].ToString();
            #endregion

            #region Line-35
            LineNo = "35";
            sht.L35_Exp_TOD1_KVAH_Units = dtSolarBill.Rows[34][0].ToString();
            sht.L35_Exp_TOD2_KVAH_Units = dtSolarBill.Rows[34][1].ToString();
            sht.L35_Exp_TOD3_KVAH_Units = dtSolarBill.Rows[34][2].ToString();
            sht.L35_Exp_TOD4_KVAH_Units = dtSolarBill.Rows[34][3].ToString();

            #endregion

            #region Line-36
            LineNo = "36";
            sht.L36_Exp_TOD1_KVA_Units = dtSolarBill.Rows[35][0].ToString();
            sht.L36_Exp_TOD2_KVA_Units = dtSolarBill.Rows[35][1].ToString();
            sht.L36_Exp_TOD3_KVA_Units = dtSolarBill.Rows[35][2].ToString();
            sht.L36_Exp_TOD4_KVA_Units = dtSolarBill.Rows[35][3].ToString();

            #endregion

            #region Line-37
            LineNo = "37";
            sht.L37_Gen_Meter_Serial_Number = dtSolarBill.Rows[36][0].ToString();
            #endregion

            #region Line-38
            LineNo = "38";
            sht.L38_Gen_KWH_PRESREAD  = dtSolarBill.Rows[37][0].ToString();
            sht.L38_Gen_KVAH_PRESREAD = dtSolarBill.Rows[37][1].ToString();
            sht.L38_Gen_KVA_PRESREAD  = dtSolarBill.Rows[37][2].ToString();
            sht.L38_Gen_KW_PRESREAD   = dtSolarBill.Rows[37][3].ToString();

            #endregion

            #region Line-39
            LineNo = "39";
            sht.L39_Gen_KWH_PASTREAD  = dtSolarBill.Rows[38][0].ToString();
            sht.L39_Gen_KVAH_PASTREAD = dtSolarBill.Rows[38][1].ToString();
            sht.L39_Gen_KVA_PASTREAD  = dtSolarBill.Rows[38][2].ToString();
            sht.L39_Gen_KW_PASTREAD   = dtSolarBill.Rows[38][3].ToString();

            #endregion

            #region Line-40
            LineNo = "40";
            sht.L40_Gen_MF1 = dtSolarBill.Rows[39][0].ToString();
            sht.L40_Gen_MF2 = dtSolarBill.Rows[39][1].ToString();
            sht.L40_Gen_MF3 = dtSolarBill.Rows[39][2].ToString();
            sht.L40_Gen_MF4 = dtSolarBill.Rows[39][3].ToString();
            #endregion

            #region Line-41
            LineNo = "41";
            sht.L41_Gen_KWH_NET_UNITS  = dtSolarBill.Rows[40][0].ToString();
            sht.L41_Gen_KVAH_NET_UNITS = dtSolarBill.Rows[40][1].ToString();
            sht.L41_Gen_KVA_NET_UNITS  = dtSolarBill.Rows[40][2].ToString();
            sht.L41_Gen_KW_NET_UNITS   = dtSolarBill.Rows[40][3].ToString();

            #endregion

            #region Line-42
            LineNo = "42";
            sht.L42_Previous_CREDIT_Units_TOD1_KVAH = dtSolarBill.Rows[41][0].ToString();
            sht.L42_Previous_CREDIT_Units_TOD2_KVAH = dtSolarBill.Rows[41][1].ToString();
            sht.L42_Previous_CREDIT_Units_TOD3_KVAH = dtSolarBill.Rows[41][2].ToString();
            sht.L42_Previous_CREDIT_Units_TOD4_KVAH = dtSolarBill.Rows[41][3].ToString();

            sht.L42_Exp_CURRENT_NET_EXPORT_TOD1_KVAH_UNITS = dtSolarBill.Rows[41][4].ToString();
            sht.L42_Exp_CURRENT_NET_EXPORT_TOD2_KVAH_UNITS = dtSolarBill.Rows[41][5].ToString();
            sht.L42_Exp_CURRENT_NET_EXPORT_TOD3_KVAH_UNITS = dtSolarBill.Rows[41][6].ToString();
            sht.L42_Exp_CURRENT_NET_EXPORT_TOD4_KVAH_UNITS = dtSolarBill.Rows[41][7].ToString();
            #endregion

            #region Line-43
            LineNo = "43";
            sht.L43_Previous_CREDIT_Units_TOD1_KWH = dtSolarBill.Rows[42][0].ToString();
            sht.L43_Previous_CREDIT_Units_TOD2_KWH = dtSolarBill.Rows[42][1].ToString();
            sht.L43_Previous_CREDIT_Units_TOD3_KWH = dtSolarBill.Rows[42][2].ToString();
            sht.L43_Previous_CREDIT_Units_TOD4_KWH = dtSolarBill.Rows[42][3].ToString();

            sht.L43_Exp_CURRENT_NET_EXPORT_TOD1_KWH_UNITS = dtSolarBill.Rows[42][4].ToString();
            sht.L43_Exp_CURRENT_NET_EXPORT_TOD2_KWH_UNITS = dtSolarBill.Rows[42][5].ToString();
            sht.L43_Exp_CURRENT_NET_EXPORT_TOD3_KWH_UNITS = dtSolarBill.Rows[42][6].ToString();
            sht.L43_Exp_CURRENT_NET_EXPORT_TOD4_KWH_UNITS = dtSolarBill.Rows[42][7].ToString();

            #endregion

            #region Line-44
            LineNo = "44";
            sht.L44_Carry_Forward_Units_TOD1_KVAH = dtSolarBill.Rows[43][0].ToString();
            sht.L44_Carry_Forward_Units_TOD2_KVAH = dtSolarBill.Rows[43][1].ToString();
            sht.L44_Carry_Forward_Units_TOD3_KVAH = dtSolarBill.Rows[43][2].ToString();
            sht.L44_Carry_Forward_Units_TOD4_KVAH = dtSolarBill.Rows[43][3].ToString();
            #endregion

            #region Line-45
            LineNo = "45";
            sht.L45_Carry_Forward_Units_TOD1_KWH = dtSolarBill.Rows[44][0].ToString();
            sht.L45_Carry_Forward_Units_TOD2_KWH = dtSolarBill.Rows[44][1].ToString();
            sht.L45_Carry_Forward_Units_TOD3_KWH = dtSolarBill.Rows[44][2].ToString();
            sht.L45_Carry_Forward_Units_TOD4_KWH = dtSolarBill.Rows[44][3].ToString();

            #endregion

            #region Line-46
            LineNo = "46";
            sht.L46_Previous_CREDIT_Units_MAIN_KVAH = dtSolarBill.Rows[45][0].ToString();
            sht.L46_Net_Billed_Units_MAIN = dtSolarBill.Rows[45][1].ToString();

            sht.L46_Net_Billed_Units_MAIN_TOD1_KVAH = dtSolarBill.Rows[45][2].ToString();
            sht.L46_Net_Billed_Units_MAIN_TOD2_KVAH = dtSolarBill.Rows[45][3].ToString();
            sht.L46_Net_Billed_Units_MAIN_TOD3_KVAH = dtSolarBill.Rows[45][4].ToString();
            sht.L46_Net_Billed_Units_MAIN_TOD4_KVAH = dtSolarBill.Rows[45][5].ToString();

            sht.L46_Carry_Forward_Units_MAIN_KVAH = dtSolarBill.Rows[45][6].ToString();
            sht.L46_Previous_CREDIT_Units_MAIN_KWH = dtSolarBill.Rows[45][7].ToString();
            sht.L46_Net_Billed_Units_MAIN_KWH = dtSolarBill.Rows[45][8].ToString();

            sht.L46_Net_Billed_Units_MAIN_TOD1_KWH = dtSolarBill.Rows[45][9].ToString();
            sht.L46_Net_Billed_Units_MAIN_TOD2_KWH = dtSolarBill.Rows[45][10].ToString();
            sht.L46_Net_Billed_Units_MAIN_TOD3_KWH = dtSolarBill.Rows[45][11].ToString();
            sht.L46_Net_Billed_Units_MAIN_TOD4_KWH = dtSolarBill.Rows[45][12].ToString();

            sht.L46_Carry_Forward_Units_MAIN_KWH = dtSolarBill.Rows[45][13].ToString();
            //sht.L46_SolarLoad = dtSolarBill.Rows[45][14].ToString();
            //   Line 46 end
            #endregion

            #region Line-47
            LineNo = "47";
            sht.L47_MonYear1 = dtSolarBill.Rows[46][0].ToString();
            sht.L47_Exp_KVAH_UNITS1 = dtSolarBill.Rows[46][1].ToString();
            sht.L47_MonYear2 = dtSolarBill.Rows[46][2].ToString();
            sht.L47_Exp_KVAH_UNITS2 = dtSolarBill.Rows[46][3].ToString();
            sht.L47_MonYear3 = dtSolarBill.Rows[46][4].ToString();
            sht.L47_Exp_KVAH_UNITS3 = dtSolarBill.Rows[46][5].ToString();
            sht.L47_MonYear4 = dtSolarBill.Rows[46][6].ToString();
            sht.L47_Exp_KVAH_UNITS4 = dtSolarBill.Rows[46][7].ToString();
            sht.L47_MonYear5 = dtSolarBill.Rows[46][8].ToString();
            sht.L47_Exp_KVAH_UNITS5 = dtSolarBill.Rows[46][9].ToString();
            sht.L47_MonYear6 = dtSolarBill.Rows[46][10].ToString();
            sht.L47_Exp_KVAH_UNITS6 = dtSolarBill.Rows[46][11].ToString();
            sht.L47_MonYear7 = dtSolarBill.Rows[46][12].ToString();
            sht.L47_Exp_KVAH_UNITS7 = dtSolarBill.Rows[46][13].ToString();
            #endregion

            #region Line-48
            LineNo = "48";
            sht.L48_MonYear1 = dtSolarBill.Rows[47][0].ToString();
            sht.L48_Gen_KVAH_UNITS1 = dtSolarBill.Rows[47][1].ToString();
            sht.L48_MonYear2 = dtSolarBill.Rows[47][2].ToString();
            sht.L48_Gen_KVAH_UNITS2 = dtSolarBill.Rows[47][3].ToString();
            sht.L48_MonYear3 = dtSolarBill.Rows[47][4].ToString();
            sht.L48_Gen_KVAH_UNITS3 = dtSolarBill.Rows[47][5].ToString();
            sht.L48_MonYear4 = dtSolarBill.Rows[47][6].ToString();
            sht.L48_Gen_KVAH_UNITS4 = dtSolarBill.Rows[47][7].ToString();
            sht.L48_MonYear5 = dtSolarBill.Rows[47][8].ToString();
            sht.L48_Gen_KVAH_UNITS5 = dtSolarBill.Rows[47][9].ToString();
            sht.L48_MonYear6 = dtSolarBill.Rows[47][10].ToString();
            sht.L48_Gen_KVAH_UNITS6 = dtSolarBill.Rows[47][11].ToString();
            sht.L48_MonYear7 = dtSolarBill.Rows[47][12].ToString();
            sht.L48_Gen_KVAH_UNITS7 = dtSolarBill.Rows[47][13].ToString();
            #endregion

            #region Line-49
            LineNo = "49";
            sht.L49_Exp_KWH_UNITS = dtSolarBill.Rows[48][0].ToString();
            sht.L49_Exp_Past_KWH_UNITS = dtSolarBill.Rows[48][1].ToString();
            sht.L49_Exp_Present_KWH_UNITS = dtSolarBill.Rows[48][2].ToString();
            sht.L49_Exp_KVAH_UNITS = dtSolarBill.Rows[48][3].ToString();
            sht.L49_Exp_Past_KVAH_UNITS = dtSolarBill.Rows[48][4].ToString();
            sht.L49_Exp_Present_KVAH_UNITS = dtSolarBill.Rows[48][5].ToString();
            sht.L49_Exp_KVA_UNITS = dtSolarBill.Rows[48][6].ToString();
            sht.L49_Exp_Past_KVA_UNITS = dtSolarBill.Rows[48][7].ToString();
            sht.L49_Exp_Present_KVA_UNITS = dtSolarBill.Rows[48][8].ToString();
            sht.L49_Exp_CURRENT_NET_EXPORT_KVA_UNITS = dtSolarBill.Rows[48][9].ToString();
            sht.L49_Exp_CURRENT_NET_EXPORT_KVAH_UNITS = dtSolarBill.Rows[48][10].ToString();
            sht.L49_Exp_CURRENT_NET_EXPORT_KWH_UNITS = dtSolarBill.Rows[48][11].ToString();
            #endregion

            #region Line-50
            LineNo = "50";
            sht.L50_Exp_TOD1_KVAH_Units = dtSolarBill.Rows[49][0].ToString();
            sht.L50_Exp_TOD2_KVAH_Units = dtSolarBill.Rows[49][1].ToString();
            sht.L50_Exp_TOD3_KVAH_Units = dtSolarBill.Rows[49][2].ToString();
            sht.L50_Exp_TOD4_KVAH_Units = dtSolarBill.Rows[49][3].ToString();
            #endregion

            #region Line-51
            LineNo = "51";
            sht.L51_Exp_TOD1_KWH_Units = dtSolarBill.Rows[50][0].ToString();
            sht.L51_Exp_TOD2_KWH_Units = dtSolarBill.Rows[50][1].ToString();
            sht.L51_Exp_TOD3_KWH_Units = dtSolarBill.Rows[50][2].ToString();
            sht.L51_Exp_TOD4_KWH_Units = dtSolarBill.Rows[50][3].ToString();
            #endregion

            #region Line-52
            LineNo = "52";
            sht.L52_Exp_TOD1_KVA_Units = dtSolarBill.Rows[51][0].ToString();
            sht.L52_Exp_TOD2_KVA_Units = dtSolarBill.Rows[51][1].ToString();
            sht.L52_Exp_TOD3_KVA_Units = dtSolarBill.Rows[51][2].ToString();
            sht.L52_Exp_TOD4_KVA_Units = dtSolarBill.Rows[51][3].ToString();
            #endregion

            #region Line-53
            LineNo = "53";
            sht.L53_Exp_TOD1_KW_Units = dtSolarBill.Rows[52][0].ToString();
            sht.L53_Exp_TOD2_KW_Units = dtSolarBill.Rows[52][1].ToString();
            sht.L53_Exp_TOD3_KW_Units = dtSolarBill.Rows[52][2].ToString();
            sht.L53_Exp_TOD4_KW_Units = dtSolarBill.Rows[52][3].ToString();
            #endregion

            #region Line-54
            LineNo = "54";
            sht.L54_Exp_TOD1_KW_Units = dtSolarBill.Rows[53][0].ToString();
            sht.L54_Exp_TOD2_KW_Units = dtSolarBill.Rows[53][1].ToString();
            sht.L54_Exp_TOD3_KW_Units = dtSolarBill.Rows[53][2].ToString();
            sht.L54_Exp_TOD4_KW_Units = dtSolarBill.Rows[53][3].ToString();
            #endregion

            #region Line-55
            LineNo = "55";
            sht.L55_MonYear1 = dtSolarBill.Rows[54][0].ToString();
            sht.L55_Exp_KWH_UNITS1 = dtSolarBill.Rows[54][1].ToString();
            sht.L55_MonYear2 = dtSolarBill.Rows[54][2].ToString();
            sht.L55_Exp_KWH_UNITS2 = dtSolarBill.Rows[54][3].ToString();
            sht.L55_MonYear3 = dtSolarBill.Rows[54][4].ToString();
            sht.L55_Exp_KWH_UNITS3 = dtSolarBill.Rows[54][5].ToString();
            sht.L55_MonYear4 = dtSolarBill.Rows[54][6].ToString();
            sht.L55_Exp_KWH_UNITS4 = dtSolarBill.Rows[54][7].ToString();
            sht.L55_MonYear5 = dtSolarBill.Rows[54][8].ToString();
            sht.L55_Exp_KWH_UNITS5 = dtSolarBill.Rows[54][9].ToString();
            sht.L55_MonYear6 = dtSolarBill.Rows[54][10].ToString();
            sht.L55_Exp_KWH_UNITS6 = dtSolarBill.Rows[54][11].ToString();
            sht.L55_MonYear7 = dtSolarBill.Rows[54][12].ToString();
            sht.L55_Exp_KWH_UNITS7 = dtSolarBill.Rows[54][13].ToString();

            #endregion

            #region Line-56
            LineNo = "56";
            sht.L56_MonYear1 = dtSolarBill.Rows[55][0].ToString();
            sht.L56_Gen_KWH_UNITS1 = dtSolarBill.Rows[55][1].ToString();
            sht.L56_MonYear2 = dtSolarBill.Rows[55][2].ToString();
            sht.L56_Gen_KWH_UNITS2 = dtSolarBill.Rows[55][3].ToString();
            sht.L56_MonYear3 = dtSolarBill.Rows[55][4].ToString();
            sht.L56_Gen_KWH_UNITS3 = dtSolarBill.Rows[55][5].ToString();
            sht.L56_MonYear4 = dtSolarBill.Rows[55][6].ToString();
            sht.L56_Gen_KWH_UNITS4 = dtSolarBill.Rows[55][7].ToString();
            sht.L56_MonYear5 = dtSolarBill.Rows[55][8].ToString();
            sht.L56_Gen_KWH_UNITS5 = dtSolarBill.Rows[55][9].ToString();
            sht.L56_MonYear6 = dtSolarBill.Rows[55][10].ToString();
            sht.L56_Gen_KWH_UNITS6 = dtSolarBill.Rows[55][11].ToString();
            sht.L56_MonYear7 = dtSolarBill.Rows[55][12].ToString();
            sht.L56_Gen_KWH_UNITS7 = dtSolarBill.Rows[55][13].ToString();

            #endregion

            #endregion
            #region Custom Fields

            var meter = sht.L11_MTRSNO_METER_2_IF_AVAILABLE.Trim() != "" ? sht.L11_MTRSNO_METER_2_IF_AVAILABLE : sht.L11_MTRSNO_METER1;
            sht.TopPanel_Row_1 = sht.L1_MONTH_YEAR + " / " + sht.L1_ZONE + " / " + sht.L1_BU + " / " + sht.L1_route + " / " + sht.L1_Bill_seq_no;
            sht.TopPanel_Row_3 = "T No.  " + sht.L8_T_No;
            sht.TopPanel_Row_4 = "Bill Date  " + sht.L7_Billdt;
          
            dtSolarBill.Rows[9][2].ToString();
            Console.WriteLine("Custom Fields calculated");
            #endregion

            return sht;
        }

        PaperSourceCollection printerSources;


        void NonTOD_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            e.PrintDocument.DefaultPageSettings.PaperSource = e.PrintDocument.PrinterSettings.PaperSources[cbNonTODTraySource.SelectedIndex];
            e.PrintDocument.PrintPage += PrintDocument_PrintPage;
            if (e.PrintDocument.PrinterSettings.CanDuplex)
                e.PrintDocument.PrinterSettings.Duplex = Duplex.Default;
            printerSources = e.PrintDocument.PrinterSettings.PaperSources;
        }

        void sap_print(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            e.PrintDocument.PrintPage += PrintDocument_PrintPage;
            if (e.PrintDocument.PrinterSettings.CanDuplex)
                e.PrintDocument.PrinterSettings.Duplex = Duplex.Simplex;
        }
        
        void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.PageSettings.PrinterSettings.Duplex = Duplex.Vertical;
        }

        private void cbDefaultPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            AppFunctions.ShowWaitForm("Please wait we are searching for printer trays.!!");
            PrintDocument printDoc = new PrintDocument();
            cbNonTODTraySource.Properties.Items.Clear();
            printDoc.PrinterSettings.PrinterName = cbDefaultPrinter.SelectedText;
            PaperSourceCollection ps = printDoc.PrinterSettings.PaperSources;
            for (int i = 0; i < ps.Count; i++)
            {
                PaperSource pkSource = printDoc.PrinterSettings.PaperSources[i];
                cbNonTODTraySource.Properties.Items.Add(ps[i].SourceName);
            }
            
           
            cbNonTODTraySource.SelectedIndex = 0;
        
            AppFunctions.CloseWaitForm();
        }

        private void sbLoadTariffBasedMessages_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog ofdMsg = new OpenFileDialog())
            {
                ofdMsg.Title = "Select load based tariff message templetes.";
                ofdMsg.Multiselect = false;
                ofdMsg.Filter = "txt Files|*.txt";
                if (ofdMsg.ShowDialog() == DialogResult.OK)
                {
                    mVImagePath = ofdMsg.FileName;
                    var sb = sender as SimpleButton;
                }
            }
        }

        private void sbLoadServiceNoBasedMessages_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog ofdMv = new OpenFileDialog())
            {
                ofdMv.Title = "Select Service number based message templates (*.txt) messages.";
                ofdMv.Multiselect = false;
                ofdMv.Filter = "txt Files|*.txt";
                if (ofdMv.ShowDialog() == DialogResult.OK)
                {
                    mVImagePath = ofdMv.FileName;
                    var sb = sender as SimpleButton;
                    XtraMessageBox.Show("Total Bill in this file " + Solar_Bill_HT.Length.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AppFunctions.ShowWaitForm("Generating Solar HT Bills Now..!!");

                    StartPrinting_HT_Solar_Bills(Solar_Bill_HT, sb.Name);
                }
            }
        }

        private void Printers_Refresh_Button_Click(object sender, EventArgs e)
        {
            AppFunctions.ShowWaitForm("Please wait..");
            cbDefaultPrinter.SelectedIndex = -1;
            cbDefaultPrinter.Properties.Items.Clear();
            cbNonTODTraySource.Properties.Items.Clear();
          
            foreach (var printers in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                cbDefaultPrinter.Properties.Items.Add(printers);
            }
            AppFunctions.CloseWaitForm();

        }

        private bool ValidatetxtFile(string[] Bills)
        {
            try
            {
                int BillNo = 0;
                foreach (var Bill in Bills)
                {
                    BillNo++;
                    DataTable dtSolarBill = ParseAsDataTable.LTMD_Solar_FileTxtToDataTable(Bill, BillNo, "HT Solar");
                    if (dtSolarBill.Rows.Count == 56)
                    {
                        Solar_Bill_HT sht = parseSolarBill(dtSolarBill);
                    }
                    else
                    {
                        return false;
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                AppFunctions.LogError(ex);
                XtraMessageBox.Show(ex.Message.Replace('.', ' ') + "in txt file for Service no:" + ServiceNo + " and Line No." + LineNo);
                return false;
            }

        }
    }
}
