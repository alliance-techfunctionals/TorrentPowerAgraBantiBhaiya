using AT.Print.PDF;
using AT.Print.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Caching;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Drawing.Printing.PrinterSettings;

namespace AT.Print
{
    public partial class PrintLTMD : UserControl
    {
        public PrintLTMD()
        {
            InitializeComponent();
            BindPrinters();
        }
        string textFileName;
        string mVImagePath;
        string ServiceNo = "";
        String LineNo = "";
        string MonthYear = "";
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

            for (int i = 0; i < printDoc.PrinterSettings.PaperSources.Count; i++)
            {
                pkSource = printDoc.PrinterSettings.PaperSources[i];
                cbSeparatorTraySource.Properties.Items.Add(pkSource.SourceName);
            }
        }

        string[] singleLTMDBills;

        private void SbPrintBill_Click(object sender, EventArgs e)
        {
            var sb = sender as SimpleButton;
            try
            {
                if (sb.Name == "sbPrintBill")
                    if (cbDefaultPrinter.SelectedIndex == -1 || cbNonTODTraySource.SelectedIndex == -1 || cbTODTraySource.SelectedIndex == -1 || cbSeparatorTraySource.SelectedIndex == -1)
                    {
                        XtraMessageBox.Show("Please select correct printer and paper sources", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "Select bill text(*.txt) file ";
                    ofd.Multiselect = false;
                    ofd.Filter = "txt Files|*.txt";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        textFileName = ofd.SafeFileName.ToUpper().Replace(".TXT", "");

                        string contents = File.ReadAllText(ofd.FileName);
                        if (contents.StartsWith("LTMD|"))
                        {
                            singleLTMDBills = contents.Split(new String[] { "LTMD|" }, StringSplitOptions.RemoveEmptyEntries);
                            if (!select_mVImg())
                            {
                                AppFunctions.CloseWaitForm();
                                return;
                            }
                            XtraMessageBox.Show("Total Bill in this file " + singleLTMDBills.Length.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // StartPrinting_LTMDBills(singleLTMDBills, sb.Name);

                            if (ValidatetxtFile(singleLTMDBills))
                            {
                                StartPrinting_LTMDBills(singleLTMDBills, sb.Name);
                            }
                            else
                            {
                                //XtraMessageBox.Show("There is some error in txt file for Service no:" + ServiceNo);
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
            catch(Exception ex)
            {
                AppFunctions.LogError(ex);
                throw (ex);
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

        private void StartPrinting_LTMDBills(string[] Bills, string Name)
        {
            string LotNo = "InitialLot";
            string LotNoCopy = "InitialLot";
            string TOD_NonTODFlag = "";
            int BillNo = 1, Counter = 1, ParsedBills = 0;
            DataTable dtSingleLTBill=new DataTable();
            XtraReport NonTODReport = new XtraReport();
            XtraReport TODReport = new XtraReport();
            NonTODReport.PrinterName = cbDefaultPrinter.Text;
            TODReport.PrinterName = cbDefaultPrinter.Text;
            NonTODReport.CreateDocument();
            TODReport.CreateDocument();
            string FileName = AppFunctions.ProcessedBillData();
            XtraReport collectorReport = new XtraReport
            {
                DisplayName = "LTMD Print",
            };
            
            foreach (var Bill in Bills)
            {
                try
                {
                    AppFunctions.ShowWaitForm("Generating Bill..!!");
                    List<SingleLTMDBill> lstformattedbills = new List<SingleLTMDBill>();

                    dtSingleLTBill = ParseAsDataTable.LTMD_FileTxtToDataTable(Bill);

                    if ((LotNoCopy != dtSingleLTBill.Rows[0][4].ToString().Trim() || Counter == 51 || TOD_NonTODFlag!= dtSingleLTBill.Rows[0][10].ToString().Trim()) && LotNoCopy != "InitialLot" && TOD_NonTODFlag!="")
                    {
                        //ReportPrintTool printTool = new ReportPrintTool(collectorReport);
                        //printTool.ShowPreview();
                        MemoryStream ms = new MemoryStream();
                        var buffer = ms.GetBuffer();
                        Array.Clear(buffer, 0, buffer.Length);
                        ms.Position = 0;
                        ms.SetLength(0);
                        ms.Capacity = 0; 
                        collectorReport.Print(cbDefaultPrinter.Text);
                        collectorReport.Pages.Clear();
                        Counter = 1;
                        collectorReport.Dispose();
                    }


                    if (LotNo != dtSingleLTBill.Rows[0][4].ToString().Trim())
                    {
                        if (Name != "sbSavePDF")
                        {
                            LotNo = (String)dtSingleLTBill.Rows[0][4];
                            LotNoCopy = dtSingleLTBill.Rows[0][4].ToString().Trim();
                            SingleLTMDBill billSaprator = new SingleLTMDBill();
                            billSaprator.Sap_Zone = "Zone No. " + dtSingleLTBill.Rows[0][1];
                            billSaprator.Sap_LotNo = "LOT No. " + dtSingleLTBill.Rows[0][4];
                            billSaprator.Sap_GrpNo = "Group No. " + dtSingleLTBill.Rows[0][2];
                            lstformattedbills.Add(billSaprator);
                            Rpt_Saprator sap_rpt = new Rpt_Saprator
                            {
                                DataSource = lstformattedbills
                            };
                            //if (int.Parse(Bills.Count().ToString()) == BillNo)
                            {
                                sap_rpt.CreateDocument();
                                sap_rpt.ShowPrintMarginsWarning = false;
                                sap_rpt.PrinterName = cbDefaultPrinter.Text;
                                sap_rpt.PrintingSystem.StartPrint += Seperator_StartPrint;
                                sap_rpt.Print(cbDefaultPrinter.Text);
                                //collectorReport.Pages.AddRange(sap_rpt.Pages);
                            }
                            lstformattedbills.Clear();
                        }
                    }

                    SingleLTMDBill slt = parseSingleLTMDBill(dtSingleLTBill);

                    TOD_NonTODFlag = slt.L1_TODOrNon_TODFlag;

                    slt.MVPicture = mVImagePath;
                    lstformattedbills.Add(slt);

                    #region PDF LTMD

                    if (Name == "sbSavePDF" && String.Equals(slt.L1_TODOrNon_TODFlag, "0"))
                    {
                        AT.Print.Rpt_LTMDPDF rptsd = new Rpt_LTMDPDF
                        {
                            DataSource = lstformattedbills,
                        };

                        #region WaterMark Picture Front Page PDF Non-TOD
                        DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkFrontNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
                        pictureWatermarkFrontNonTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Non_TOD_Front_Page.png");
                        pictureWatermarkFrontNonTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        pictureWatermarkFrontNonTOD.ImageTiling = false;
                        pictureWatermarkFrontNonTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
                        pictureWatermarkFrontNonTOD.ImageTransparency = 0;
                        pictureWatermarkFrontNonTOD.ShowBehind = true;
                        //pictureWatermark.PageRange = "2,4";
                        rptsd.Watermark.CopyFrom(pictureWatermarkFrontNonTOD);
                        #endregion

                        rptsd.CreateDocument(false);
                        AT.Print.PDF.rpt_LTMD_Back rpts = new AT.Print.PDF.rpt_LTMD_Back
                        {
                            DataSource = lstformattedbills,
                        };

                        #region WaterMark Picture Back Page PDF Non-TOD
                        DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkBackNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
                        pictureWatermarkBackNonTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Non_TOD_Back_Page.png");
                        pictureWatermarkBackNonTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        pictureWatermarkBackNonTOD.ImageTiling = false;
                        pictureWatermarkBackNonTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
                        pictureWatermarkBackNonTOD.ImageTransparency = 0;
                        pictureWatermarkBackNonTOD.ShowBehind = true;
                        //pictureWatermark.PageRange = "2,4";
                        rptsd.Watermark.CopyFrom(pictureWatermarkBackNonTOD);
                        #endregion

                        rpts.CreateDocument(false);
                        rptsd.ModifyDocument(x => { x.AddPages(rpts.Pages); });
                        DevExpress.XtraPrinting.Page myPage2 = rptsd.Pages[1];
                        myPage2.AssignWatermark(pictureWatermarkBackNonTOD);
                        string billdate = lstformattedbills.FirstOrDefault().L1_MonthYear;
                        string ServiceNo = lstformattedbills.FirstOrDefault().L6_SERVDET_SERVNO;
                        //DateTime.TryParseExact(billdate, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime billDate);
                        var outputfolder = "C://Bills//LTMD Files//" + billdate + "//" + textFileName;
                        OutputFolderPath OFP = new OutputFolderPath();
                        outputfolder = OFP.LoadLocation() + "//LTMD Files//" + billdate + "//" + textFileName;;
                        if (!Directory.Exists(outputfolder))
                            Directory.CreateDirectory(outputfolder);
                        //var OutPutFolder = 
                        if (Directory.Exists(outputfolder))
                        {
                            //rptsd.ExportToPdf("C://Bills//LTMD Files//" + billdate + "//" + textFileName + "//" + ServiceNo + ".pdf");
                            rptsd.ExportToPdf(outputfolder + "//" + ServiceNo + ".pdf");
                        }
                        ParsedBills++;
                        AppFunctions.CloseWaitForm();
                    }
                    #endregion

                    #region Print Non_TOD LTMD

                    else if (String.Equals(slt.L1_TODOrNon_TODFlag, "0"))
                    {
                        PrinterSettings ps = new PrinterSettings() { PrinterName = cbDefaultPrinter.Text };
                        using (Graphics g = ps.CreateMeasurementGraphics(ps.DefaultPageSettings))
                        {
                            Margins MinMargins = DevExpress.XtraPrinting.Native.DeviceCaps.GetMinMargins(g);
                            Console.WriteLine("Minimum Margins for " + ps.PrinterName + ": " + MinMargins.ToString());
                        }
                        AT.Print.Rpt_LTMD_Print rpta = new Rpt_LTMD_Print
                        {
                            DataSource = lstformattedbills,
                            DisplayName = slt.L6_SERVDET_SERVNO,
                        };
                        rpta.Watermark.ImageTransparency = 255;
                        rpta.PrinterName = cbDefaultPrinter.SelectedItem.ToString();    //The PrinterName property should be specified before creating a document (which is performed using the XtraReport.CreateDocument method)
                        rpta.PrintingSystem.Document.Name = slt.L6_SERVDET_SERVNO;
                        rpta.CreateDocument();
                        // NonTODReport.ModifyDocument(x => { x.AddPages(rpta.Pages); });
                        Rpt_LTMD_Print_Back rptb = new Rpt_LTMD_Print_Back
                        {
                            DataSource = lstformattedbills,
                        };
                        rptb.CreateDocument();
                        //   NonTODReport.ModifyDocument(x => { x.AddPages(rptb.Pages); });
                        rpta.ModifyDocument(x => { x.AddPages(rptb.Pages); });
                        //rpta.PrintingSystem.StartPrint += NonTOD_StartPrint;
                        //rpta.Print(cbDefaultPrinter.Text);
                        collectorReport.PrintingSystem.StartPrint += NonTOD_StartPrint;
                        collectorReport.Pages.AddRange(rpta.Pages);
                        if (Bills.Count() == BillNo && LotNoCopy != "InitialLot")
                        {
                            collectorReport.Print(cbDefaultPrinter.Text);
                            collectorReport.Pages.Clear();
                        }
                        //This event fires only for Windows Forms and WPF applications.
                        //ReportPrintTool reportPrint = new ReportPrintTool(rpta);
                        //reportPrint.ShowPreview();      //Do not print from the preview window
                        AppFunctions.CloseWaitForm();
                        //rpta.Print();

                       // Exporting to PDF
                        //string billdate = lstformattedbills.FirstOrDefault().L7_BillDt;
                        //string ServiceNo = lstformattedbills.FirstOrDefault().L6_SERVDET_SERVNO;
                        //DateTime.TryParseExact(billdate, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime billDate);
                        //if (!Directory.Exists(LoadStaticData.EbillOutputLocation + "//Print " + billDate.ToString("dd-MMM-yyyy") + "//"))
                        //    Directory.CreateDirectory(LoadStaticData.EbillOutputLocation + "//Print " + billDate.ToString("dd-MMM-yyyy") + "//");
                        //rpta.ExportToPdf(LoadStaticData.EbillOutputLocation + "//Print " + billDate.ToString("dd-MMM-yyyy") + "//" + ServiceNo + ".pdf");

                        ParsedBills++;
                    }
                    #endregion

                    #region PDF LTMD with TOD

                    else if (Name == "sbSavePDF" && String.Equals(slt.L1_TODOrNon_TODFlag, "1"))
                    {
                        AT.Print.Rpt_LTMDwTodPDF rptsd = new Rpt_LTMDwTodPDF
                        {
                            DataSource = lstformattedbills,
                        };

                        #region WaterMark Picture Front Page PDF TOD
                        DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkFrontTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
                        pictureWatermarkFrontTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_TOD_Front_Page.png");
                        pictureWatermarkFrontTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        pictureWatermarkFrontTOD.ImageTiling = false;
                        pictureWatermarkFrontTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
                        pictureWatermarkFrontTOD.ImageTransparency = 0;
                        pictureWatermarkFrontTOD.ShowBehind = true;
                        //pictureWatermark.PageRange = "2,4";
                        rptsd.Watermark.CopyFrom(pictureWatermarkFrontTOD);
                        #endregion

                        rptsd.CreateDocument(false);
                        AT.Print.PDF.rpt_LTMDwT_BackPDF rpts = new AT.Print.PDF.rpt_LTMDwT_BackPDF
                        {
                            DataSource = lstformattedbills,
                        };

                        #region WaterMark Picture Back Page PDF TOD
                        DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkBackTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
                        pictureWatermarkBackTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_TOD_Back_Page.png");
                        pictureWatermarkBackTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        pictureWatermarkBackTOD.ImageTiling = false;
                        pictureWatermarkBackTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
                        pictureWatermarkBackTOD.ImageTransparency = 0;
                        pictureWatermarkBackTOD.ShowBehind = true;
                        //pictureWatermark.PageRange = "2,4";
                        rptsd.Watermark.CopyFrom(pictureWatermarkBackTOD);
                        #endregion

                        rpts.ShowPrintMarginsWarning = false;
                        rpts.CreateDocument(false);
                        rptsd.ModifyDocument(x => { x.AddPages(rpts.Pages); });
                        DevExpress.XtraPrinting.Page myPage = rptsd.Pages[1];
                        myPage.AssignWatermark(pictureWatermarkBackTOD);
                        string billdate = lstformattedbills.FirstOrDefault().L1_MonthYear;
                        string ServiceNo = lstformattedbills.FirstOrDefault().L6_SERVDET_SERVNO;
                        //DateTime.TryParseExact(billdate, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime billDate);
                        var outputfolder = "C://Bills//LTMD Files//" + billdate + "//" + textFileName;
                        OutputFolderPath OFP = new OutputFolderPath();
                        outputfolder = OFP.LoadLocation() + "//LTMD Files//" + billdate + "//" + textFileName; 
                        if (!Directory.Exists(outputfolder))
                            Directory.CreateDirectory(outputfolder);
                        //var OutPutFolder = 
                        if (Directory.Exists(outputfolder))
                        {
                            //rptsd.ExportToPdf("C://Bills//LTMD Files//" + billdate + "//" + textFileName + "//" + ServiceNo + ".pdf");
                            rptsd.ExportToPdf(outputfolder + "//" + ServiceNo + ".pdf");
                        }
                        AppFunctions.CloseWaitForm();
                        ParsedBills++;
                    }
                    #endregion

                    #region Print LTMD with TOD

                    else if (String.Equals(slt.L1_TODOrNon_TODFlag, "1"))
                    {
                        AT.Print.Rpt_LTMD_TOD_Print rpta = new Rpt_LTMD_TOD_Print
                        {
                            DataSource = lstformattedbills,
                            //DisplayName = slt.L6_SERVDET_SERVNO,
                        };
                        rpta.Watermark.ImageTransparency = 255;
                        rpta.PrinterName = cbDefaultPrinter.SelectedItem.ToString();    //The PrinterName property should be specified before creating a document (which is performed using the XtraReport.CreateDocument method)
                        rpta.PrintingSystem.Document.Name = slt.L6_SERVDET_SERVNO;
                        rpta.CreateDocument();
                        //TODReport.ModifyDocument(x => { x.AddPages(rpta.Pages); });
                        AT.Print.Rpt_LTMD_TOD_Print_Back rptb = new AT.Print.Rpt_LTMD_TOD_Print_Back
                        {
                            DataSource = lstformattedbills,
                        };
                        rptb.CreateDocument();
                        //  TODReport.ModifyDocument(x => { x.AddPages(rptb.Pages); });
                        rpta.ModifyDocument(x => { x.AddPages(rptb.Pages); });
                        //rpta.PrintingSystem.StartPrint += TOD_StartPrint;
                        //rpta.Print(cbDefaultPrinter.Text);//This event fires only for Windows Forms and WPF applications.
                        collectorReport.PrintingSystem.StartPrint += TOD_StartPrint;
                        collectorReport.Pages.AddRange(rpta.Pages);
                        if (Bills.Count() == BillNo && LotNoCopy != "InitialLot")
                        {
                            collectorReport.Print(cbDefaultPrinter.Text);
                            collectorReport.Pages.Clear();
                        }
                        //Exporting to PDF
                        //string billdate = lstformattedbills.FirstOrDefault().L7_BillDt;
                        //string ServiceNo = lstformattedbills.FirstOrDefault().L6_SERVDET_SERVNO;
                        //DateTime.TryParseExact(billdate, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime billDate);
                        //if (!Directory.Exists(LoadStaticData.EbillOutputLocation + "//Print " + billDate.ToString("dd-MMM-yyyy") + "//"))
                        //    Directory.CreateDirectory(LoadStaticData.EbillOutputLocation + "//Print " + billDate.ToString("dd-MMM-yyyy") + "//");
                        //rpta.ExportToPdf(LoadStaticData.EbillOutputLocation + "//Print " + billDate.ToString("dd-MMM-yyyy") + "//" + ServiceNo + ".pdf");

                        //ReportPrintTool reportPrint = new ReportPrintTool(rpta);
                        //reportPrint.ShowPreview();      //Do not print from the preview window
                        AppFunctions.CloseWaitForm();
                        //rpta.Print();
                        ParsedBills++;
                    }
                    #endregion
                    else
                    {
                        AppFunctions.CloseWaitForm();
                        XtraMessageBox.Show("Could not find TOD flag in Bill: " + slt.L6_SERVDET_SERVNO, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("Could not find TOD flag in Bill: " + slt.L6_SERVDET_SERVNO);
                    }
                }
                catch (System.OutOfMemoryException)
                {
                    //XtraMessageBox.Show("Error Parsing Bill " + BillNo + " of the given file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AppFunctions.LogError("Error Parsing Service No. " + ServiceNo + " of the given file due to out of memory.");
                    AppFunctions.LogProcessedBill(Convert.ToString(dtSingleLTBill.Rows[0][1]), Convert.ToString(dtSingleLTBill.Rows[0][4]), Convert.ToString(dtSingleLTBill.Rows[0][2]), Convert.ToString(dtSingleLTBill.Rows[0][5]), ServiceNo, FileName, "No");
                    //SaveFile(Convert.ToString(ServiceNo));
                    System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                    GC.Collect();
                    GC.RemoveMemoryPressure(1024 * 1024);
                    break;
                }
                catch (Exception ex)
                {
                    AppFunctions.LogError(ex);
                    AppFunctions.LogProcessedBill(Convert.ToString(dtSingleLTBill.Rows[0][1]), Convert.ToString(dtSingleLTBill.Rows[0][4]), Convert.ToString(dtSingleLTBill.Rows[0][2]), Convert.ToString(dtSingleLTBill.Rows[0][5]), ServiceNo, FileName, "No");
                    AppFunctions.CloseWaitForm();
                    //XtraMessageBox.Show("Error Parsing Bill " + BillNo + " of the given file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                AppFunctions.LogProcessedBill(Convert.ToString(dtSingleLTBill.Rows[0][1]), Convert.ToString(dtSingleLTBill.Rows[0][4]), Convert.ToString(dtSingleLTBill.Rows[0][2]), Convert.ToString(dtSingleLTBill.Rows[0][5]), ServiceNo, FileName, "Yes");
                BillNo++;
            }

            XtraMessageBox.Show(ParsedBills + " Bills Parsed Successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //ReportPrintTool TODPrintTool = new ReportPrintTool(TODReport);
            //ReportPrintTool NonTODPrintTool = new ReportPrintTool(NonTODReport);
            //TODPrintTool.PrinterSettings.PrinterName = cbDefaultPrinter.Text;
            //NonTODPrintTool.PrinterSettings.PrinterName = cbDefaultPrinter.Text;
            //Console.WriteLine("Is Printer Duplex? " + TODPrintTool.PrinterSettings.CanDuplex);
            //TODReport.PrintingSystem.StartPrint += TOD_StartPrint;
            //   NonTODReport.PrintingSystem.StartPrint += NonTOD_StartPrint;
            // TODReport.ExportToPdf("C://Bills//TOD.pdf");
            //  NonTODReport.ExportToPdf("C://Bills//NonTOD.pdf");

        }

        SingleLTMDBill parseSingleLTMDBill(DataTable dtSingleLTBill)
        {

            SingleLTMDBill slt = new SingleLTMDBill();
            #region --Lines
            #region Line-1
            ServiceNo = dtSingleLTBill.Rows[5][0].ToString();
            //Line 1 Starts
            LineNo = "1";
            slt.L1_BillType = "LTMD";
            slt.L1_MonthYear = dtSingleLTBill.Rows[0][0].ToString();
            slt.L1_Zone = dtSingleLTBill.Rows[0][1].ToString();
            slt.L1_BU = dtSingleLTBill.Rows[0][2].ToString();
            slt.L1_PC = dtSingleLTBill.Rows[0][3].ToString();
            slt.L1_Route = dtSingleLTBill.Rows[0][4].ToString();
            slt.L1_SubRoute = dtSingleLTBill.Rows[0][5].ToString();
            slt.L1_BillSequenceNo = dtSingleLTBill.Rows[0][6].ToString();
            slt.L1_PowerFactorMSGIndicator = dtSingleLTBill.Rows[0][7].ToString();
            slt.L1_FeederName = dtSingleLTBill.Rows[0][9].ToString();
            slt.L1_TODOrNon_TODFlag = dtSingleLTBill.Rows[0][10].ToString();
            slt.L1_AKY_indicator = dtSingleLTBill.Rows[0][11].ToString();
            slt.L1_DisconnectionMSGPrintingIMMEDIATE = dtSingleLTBill.Rows[0][12].ToString();
            slt.L1_BillingCode = dtSingleLTBill.Rows[0][13].ToString();
            if (dtSingleLTBill.Rows[0][14].ToString() == "" || dtSingleLTBill.Rows[0][14].ToString().Contains("AVAILABLE"))
            {
                slt.L1_Customer_PAN = "PAN: " + dtSingleLTBill.Rows[0][14].ToString();
            }
            else
            {
                slt.L1_Customer_PAN = "PAN: " + dtSingleLTBill.Rows[0][14].ToString();
            }
            //Line 1 End
            #endregion

            #region Line-2
            //Line 2 Starts
            LineNo = "2";
            slt.L2_Name = dtSingleLTBill.Rows[1][0].ToString().Trim('�');
            //Line 2 End
            #endregion

            #region Line-3
            //Line 3 Starts
            LineNo = "3";
            slt.L3_Addr1 = dtSingleLTBill.Rows[2][0].ToString().Trim('�');
            //Line 3 End
            #endregion

            #region Line-4
            //Line 4 Starts
            LineNo = "4";
            slt.L4_Addr2 = dtSingleLTBill.Rows[3][0].ToString().Trim('�');
            //Line 4 End
            #endregion

            #region Line-5
            //Line 5 Starts
            LineNo = "5";
            slt.L5_Addr3 = dtSingleLTBill.Rows[4][0].ToString().Trim('�');
            //Line 5 End
            #endregion

            #region Line-6
            //Line 6 Starts
            LineNo = "6";
            slt.L6_MeasureContractDemand = dtSingleLTBill.Rows[5][10].ToString();
            slt.L6_SERVDET_SERVNO = dtSingleLTBill.Rows[5][0].ToString();
            slt.L6_SERVDET_SANC_LOAD = string.IsNullOrEmpty(dtSingleLTBill.Rows[5][1].ToString()) ? "" : dtSingleLTBill.Rows[5][1].ToString();
            slt.L6_ACTUAL_DEMAND = string.IsNullOrEmpty(dtSingleLTBill.Rows[5][2].ToString()) ? "" : dtSingleLTBill.Rows[5][2].ToString();
            slt.L6_TARIFF_DESCR = string.IsNullOrEmpty(dtSingleLTBill.Rows[5][3].ToString()) ? "" : dtSingleLTBill.Rows[5][3].ToString();
            slt.L6_EXCESS_DEMAND = string.IsNullOrEmpty(dtSingleLTBill.Rows[5][4].ToString()) ? "" : dtSingleLTBill.Rows[5][4].ToString();
            slt.L6_SUPPLY_VOLTAGE = dtSingleLTBill.Rows[5][5].ToString();
            slt.L6_MTRDET_LF_PERC = dtSingleLTBill.Rows[5][6].ToString();
            slt.L6_BILL_TYPE = dtSingleLTBill.Rows[5][7].ToString();
            slt.L6_Avg_Power_Factor = dtSingleLTBill.Rows[5][8].ToString();
            slt.L6_bill_demand = string.IsNullOrEmpty(dtSingleLTBill.Rows[5][9].ToString()) ? "" : dtSingleLTBill.Rows[5][9].ToString();
            slt.L6_Kvah_Indicator = dtSingleLTBill.Rows[5][11].ToString();
            slt.L6_LT_Metering_Flag = dtSingleLTBill.Rows[5][12].ToString();
            //Line 6 End
            #endregion

            #region Line-7
            //Line 7 Starts
            LineNo = "7";
            slt.L7_Due_Date = dtSingleLTBill.Rows[6][0].ToString();
            slt.L7_BillDt = dtSingleLTBill.Rows[6][1].ToString();
            //slt.L7_PrevReadDt = dtSingleLTBill.Rows[6][2].ToString();
            int YYYY, MM, DD;
            YYYY = int.Parse(dtSingleLTBill.Rows[6][2].ToString().Split('-')[2]);
            MM = int.Parse(dtSingleLTBill.Rows[6][2].ToString().Split('-')[1]);
            DD = int.Parse(dtSingleLTBill.Rows[6][2].ToString().Split('-')[0]);
            DateTime PreviousDate = new DateTime(YYYY, MM, DD);
            slt.L7_PrevReadDt = (PreviousDate.AddDays(-1)).ToString("dd-MM-yy");
            slt.L7_ReaDt = dtSingleLTBill.Rows[6][3].ToString();
            slt.L7_LastPymtDate = dtSingleLTBill.Rows[6][4].ToString();
            slt.L7_LastPayementAmount = dtSingleLTBill.Rows[6][5].ToString().Trim('�');
            slt.L7_LastPayementMode = dtSingleLTBill.Rows[6][6].ToString();
            //Line 7 End
            #endregion

            #region Line-8
            //Line 8 Starts
            LineNo = "8";
            slt.L8_FixedCharge = dtSingleLTBill.Rows[7][0].ToString();
            slt.L8_EnergyCharge = dtSingleLTBill.Rows[7][1].ToString();
            slt.L8_ACCharge = dtSingleLTBill.Rows[7][2].ToString();
            slt.L8_GovTax = dtSingleLTBill.Rows[7][3].ToString();
            slt.L8_MinCharge = dtSingleLTBill.Rows[7][4].ToString();
            slt.L8_ServdetTotbBdtOthr = dtSingleLTBill.Rows[7][5].ToString();
            slt.L8_PowerFactorAdj = dtSingleLTBill.Rows[7][6].ToString();
            slt.L8_TODCharges = dtSingleLTBill.Rows[7][7].ToString();
            slt.L8_TODCharges = slt.L8_TODCharges.Contains("-") ? ("-" + slt.L8_TODCharges.Replace("-", "")) : slt.L8_TODCharges;
            slt.L8_RegulatoryCharge_1 = dtSingleLTBill.Rows[7][8].ToString();
            slt.L8_RegulatoryCharge_2 = dtSingleLTBill.Rows[7][9].ToString();
            slt.L8_RebateIncurredCurrentMonth = dtSingleLTBill.Rows[7][10].ToString();
            slt.L8_AmountPayableBeforeDueDate = dtSingleLTBill.Rows[7][11].ToString();
            slt.L8_AmountPayableBeforeDueDate = slt.L8_AmountPayableBeforeDueDate.Contains("CR") ? ("-" + slt.L8_AmountPayableBeforeDueDate.Replace("CR", "")) : (slt.L8_AmountPayableBeforeDueDate.Contains("-")? ("-" + slt.L8_AmountPayableBeforeDueDate.Replace("-", "")) : slt.L8_AmountPayableBeforeDueDate);
            slt.L8_TNo = dtSingleLTBill.Rows[7][12].ToString().Trim('�');
            slt.L8_ParkingAmount = dtSingleLTBill.Rows[7][13].ToString();
            if (!string.IsNullOrEmpty(slt.L8_ParkingAmount))
            {
                slt.L8_ParkingAmountCeilied = Convert.ToDecimal(slt.L8_ParkingAmount);
            }
            if (!string.IsNullOrEmpty(slt.L8_AmountPayableBeforeDueDate))
            {
                slt.L8_AmountPayableBeforeDueDateCeilied = Math.Ceiling(Convert.ToDecimal(slt.L8_AmountPayableBeforeDueDate));
            }

            //Line 8 End
            #endregion

            #region Line-9
            //Line 9 Starts
            LineNo = "9";
            slt.L9_TotDbArr = dtSingleLTBill.Rows[8][0].ToString();
            slt.L9_CurrBillAmt = dtSingleLTBill.Rows[8][1].ToString();
            slt.L9_CurrBillAmt = slt.L9_CurrBillAmt.Contains("-") ? ("-" + slt.L9_CurrBillAmt.Replace("-", "")) : slt.L9_CurrBillAmt;
            slt.L9_Int_Tpl = dtSingleLTBill.Rows[8][2].ToString();
            slt.L9_ArrsTpl = dtSingleLTBill.Rows[8][3].ToString();
            slt.L9_CurrBillAmtIntTplArrsTpl = dtSingleLTBill.Rows[8][4].ToString();
            slt.L9_AmountPayable = dtSingleLTBill.Rows[8][5].ToString();
            if (Convert.ToDouble(dtSingleLTBill.Rows[8][5].ToString()) < 0)
            {
                slt.L9_AmountPayable = "NOT TO PAY";
            }
            slt.L9_MessageIndication = dtSingleLTBill.Rows[8][6].ToString();
            slt.L9_MessageFlag = dtSingleLTBill.Rows[8][7].ToString().Trim('�');
            //Line 9 End
            #endregion

            #region Line-10
            //Line 10 Starts
            LineNo = "10";
            slt.L10_LFincentive = dtSingleLTBill.Rows[9][0].ToString();
            slt.L10_DisconnDate = dtSingleLTBill.Rows[9][1].ToString();
            slt.L10_TotArrUPPCLIntUPPCLIntArrUPPCL = Math.Round(Convert.ToDecimal(dtSingleLTBill.Rows[9][2].ToString()), 2).ToString();
            slt.L10_SecDeptBdt = dtSingleLTBill.Rows[9][3].ToString();
            slt.L10_DmdChgPenalty = dtSingleLTBill.Rows[9][4].ToString();
            slt.L10_UPPCL_ArrearAmount = dtSingleLTBill.Rows[9][5].ToString();
            slt.L10_UPPCLIntOnArrearAmount = dtSingleLTBill.Rows[9][6].ToString();
            slt.L10_TotArrUPPCLIntUPPCLIntArrUPPCLCeilied = Math.Ceiling(Convert.ToDecimal(slt.L10_TotArrUPPCLIntUPPCLIntArrUPPCL));

            slt.L10_TheftAmount = dtSingleLTBill.Rows[9][7].ToString();
            slt.L10_Mode = dtSingleLTBill.Rows[9][8].ToString();
            //Line 10 End
            #endregion

            #region Line-11
            //Line 11 Starts
            LineNo = "11";
            slt.L11_MonYear_1 = dtSingleLTBill.Rows[10][0].ToString();
            slt.L11_KWH_UNITS_1 = dtSingleLTBill.Rows[10][1].ToString();
            slt.L11_MonYear_2 = dtSingleLTBill.Rows[10][2].ToString();
            slt.L11_KWH_UNITS_2 = dtSingleLTBill.Rows[10][3].ToString();
            slt.L11_MonYear_3 = dtSingleLTBill.Rows[10][4].ToString();
            slt.L11_KWH_UNITS_3 = dtSingleLTBill.Rows[10][5].ToString();
            slt.L11_MonYear_4 = dtSingleLTBill.Rows[10][6].ToString();
            slt.L11_KWH_UNITS_4 = dtSingleLTBill.Rows[10][7].ToString();
            slt.L11_MonYear_5 = dtSingleLTBill.Rows[10][8].ToString();
            slt.L11_KWH_UNITS_5 = dtSingleLTBill.Rows[10][9].ToString();
            slt.L11_MonYear_6 = dtSingleLTBill.Rows[10][10].ToString();
            slt.L11_KWH_UNITS_6 = dtSingleLTBill.Rows[10][11].ToString();
            slt.L11_MonYear_7 = dtSingleLTBill.Rows[10][12].ToString();
            slt.L11_KWH_UNITS_7 = dtSingleLTBill.Rows[10][13].ToString();
            slt.L11_MonYear_8 = dtSingleLTBill.Rows[10][14].ToString();
            slt.L11_KWH_UNITS_8 = dtSingleLTBill.Rows[10][15].ToString();
            slt.L11_MonYear_9 = dtSingleLTBill.Rows[10][16].ToString();
            slt.L11_KWH_UNITS_9 = dtSingleLTBill.Rows[10][17].ToString();
            slt.L11_MonYear_10 = dtSingleLTBill.Rows[10][18].ToString();
            slt.L11_KWH_UNITS_10 = dtSingleLTBill.Rows[10][19].ToString();
            slt.L11_MonYear_11 = dtSingleLTBill.Rows[10][20].ToString();
            slt.L11_KWH_UNITS_11 = dtSingleLTBill.Rows[10][21].ToString();
            slt.L11_MonYear_12 = dtSingleLTBill.Rows[10][22].ToString();
            slt.L11_KWH_UNITS_12 = dtSingleLTBill.Rows[10][23].ToString();
            slt.L11_MonYear_13 = dtSingleLTBill.Rows[10][24].ToString();
            slt.L11_KWH_UNITS_13 = dtSingleLTBill.Rows[10][25].ToString();

            DataTable chrtData = new DataTable();
            chrtData.Columns.Add("MonthYear");
            chrtData.Columns.Add("Value", typeof(Int32));
            for (int i = 0; i < 25; i += 2)
            {
                var crg = chrtData.NewRow();
                
                if (MonthYear != (string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i])))
                {
                    crg["MonthYear"] = string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? Convert.ToString((i + 1) / 2) : Convert.ToString(dtSingleLTBill.Rows[20][i]);
                    crg["Value"] = string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[10][i + 1])) ? 0 : Convert.ToInt32(dtSingleLTBill.Rows[10][i + 1]);
                    chrtData.Rows.Add(crg.ItemArray);

                    MonthYear = string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i]);
                }
                else
                {
                    crg["MonthYear"] = MonthYear.Replace("-", "  ");
                    crg["Value"] = string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[10][i + 1])) ? 0 : Convert.ToInt32(dtSingleLTBill.Rows[10][i + 1]);

                    chrtData.Rows.Add(crg.ItemArray);
                    MonthYear = string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i]);
                }
            }
            MonthYear = "";
            slt.KWHgrph = chrtData;


            //Line 11 End
            #endregion

            #region Line-12
            //Line 12 Starts
            LineNo = "12";
            slt.L12_MTRSNO_METER1 = dtSingleLTBill.Rows[11][0].ToString();
            slt.L12_MTRSNO_METER_2_IF_AVAILABLE = dtSingleLTBill.Rows[11][1].ToString();
            //Line 12 End
            #endregion

            #region Line-13
            //Line 13 Starts
            LineNo = "13";
            slt.L13_M1_KWH_PRESREAD = dtSingleLTBill.Rows[12][0].ToString();
            slt.L13_M1_KVA_PRESREAD = dtSingleLTBill.Rows[12][1].ToString();
            //Line 13 End
            #endregion

            #region Line-14
            //Line 14 Starts
            LineNo = "14";
            slt.L14_M1_KWH_PASTREAD = dtSingleLTBill.Rows[13][0].ToString();
            slt.L14_M1_KVA_PASTREAD = dtSingleLTBill.Rows[13][1].ToString();
            //Line 14 End
            #endregion

            #region Line-15
            //Line 15 Starts
            LineNo = "15";
            slt.L15_M1_MultiplyingFactor_1 = dtSingleLTBill.Rows[14][0].ToString();
            slt.L15_M1_MultiplyingFactor_2 = dtSingleLTBill.Rows[14][1].ToString();
            slt.L15_Purpose = dtSingleLTBill.Rows[14][2].ToString();

            //Line 15 End
            #endregion

            #region Line-16
            //Line 16 Starts
            LineNo = "16";
            slt.L16_M1_KWH_UNITS = dtSingleLTBill.Rows[15][0].ToString();
            slt.L16_M1_KVA_UNITS = dtSingleLTBill.Rows[15][1].ToString();
            //Line 16 End
            #endregion

            #region Line-17
            //Line 17 Starts
            LineNo = "17";
            slt.L17_M2_KWH_PRESREAD = dtSingleLTBill.Rows[16][0].ToString();
            slt.L17_M2_KVA_PRESREAD = dtSingleLTBill.Rows[16][1].ToString();
            //Line 17 End
            #endregion

            #region Line-18
            //Line 18 Starts
            LineNo = "18";
            slt.L18_M2_KWH_PASTREAD = dtSingleLTBill.Rows[17][0].ToString();
            slt.L18_M2_KVA_PASTREAD = dtSingleLTBill.Rows[17][1].ToString();
            //Line 18 End
            #endregion

            #region Line-19
            //Line 19 Starts
            LineNo = "19";
            slt.L19_M2_Multiplying_Factor_1 = dtSingleLTBill.Rows[18][0].ToString();
            slt.L19_M2_Multiplying_Factor_2 = dtSingleLTBill.Rows[18][1].ToString();
            //Line 19 End
            #endregion

            #region Line-20
            //Line 20 Starts
            LineNo = "20";
            slt.L20_M2_KWH_UNITS = dtSingleLTBill.Rows[19][0].ToString();
            slt.L20_M2_KVA_UNITS = dtSingleLTBill.Rows[19][1].ToString();
            //Line 20 End
            #endregion

            #region Line-21
            //Line 21 Starts
            LineNo = "21";
            slt.L21_MonYear_1 = dtSingleLTBill.Rows[20][0].ToString();
            slt.L21_KVA_UNITS_1 = dtSingleLTBill.Rows[20][1].ToString();
            slt.L21_MonYear_2 = dtSingleLTBill.Rows[20][2].ToString();
            slt.L21_KVA_UNITS_2 = dtSingleLTBill.Rows[20][3].ToString();
            slt.L21_MonYear_3 = dtSingleLTBill.Rows[20][4].ToString();
            slt.L21_KVA_UNITS_3 = dtSingleLTBill.Rows[20][5].ToString();
            slt.L21_MonYear_4 = dtSingleLTBill.Rows[20][6].ToString();
            slt.L21_KVA_UNITS_4 = dtSingleLTBill.Rows[20][7].ToString();
            slt.L21_MonYear_5 = dtSingleLTBill.Rows[20][8].ToString();
            slt.L21_KVA_UNITS_5 = dtSingleLTBill.Rows[20][9].ToString();
            slt.L21_MonYear_6 = dtSingleLTBill.Rows[20][10].ToString();
            slt.L21_KVA_UNITS_6 = dtSingleLTBill.Rows[20][11].ToString();
            slt.L21_MonYear_7 = dtSingleLTBill.Rows[20][12].ToString();
            slt.L21_KVA_UNITS_7 = dtSingleLTBill.Rows[20][13].ToString();
            slt.L21_MonYear_8 = dtSingleLTBill.Rows[20][14].ToString();
            slt.L21_KVA_UNITS_8 = dtSingleLTBill.Rows[20][15].ToString();
            slt.L21_MonYear_9 = dtSingleLTBill.Rows[20][16].ToString();
            slt.L21_KVA_UNITS_9 = dtSingleLTBill.Rows[20][17].ToString();
            slt.L21_MonYear_10 = dtSingleLTBill.Rows[20][18].ToString();
            slt.L21_KVA_UNITS_10 = dtSingleLTBill.Rows[20][19].ToString();
            slt.L21_MonYear_11 = dtSingleLTBill.Rows[20][20].ToString();
            slt.L21_KVA_UNITS_11 = dtSingleLTBill.Rows[20][21].ToString();
            slt.L21_MonYear_12 = dtSingleLTBill.Rows[20][22].ToString();
            slt.L21_KVA_UNITS_12 = dtSingleLTBill.Rows[20][23].ToString();
            slt.L21_MonYear_13 = dtSingleLTBill.Rows[20][24].ToString();
            slt.L21_KVA_UNITS_13 = dtSingleLTBill.Rows[20][25].ToString();
            DataTable KVAchrtData = new DataTable();
            KVAchrtData.Columns.Add("MonthYear");
            KVAchrtData.Columns.Add("Value", typeof(decimal));
            for (int i = 0; i < 25; i += 2)
            {
                //KVAchrtData.Rows.Add(new object[] { string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? Convert.ToString((i + 1) / 2) : Convert.ToString(dtSingleLTBill.Rows[20][i]), string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i + 1])) ? 0 : Convert.ToDecimal(dtSingleLTBill.Rows[20][i + 1]) });
                if (MonthYear != (string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i])))
                {
                    KVAchrtData.Rows.Add(new object[] { string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? Convert.ToString((i + 1) / 2) : Convert.ToString(dtSingleLTBill.Rows[20][i]), string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i + 1])) ? 0 : Convert.ToDecimal(dtSingleLTBill.Rows[20][i + 1]) });
                    MonthYear = string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i]);
                }
                else
                {
                    KVAchrtData.Rows.Add(new object[] { MonthYear.Replace("-", "  "), string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i + 1])) ? 0 : Convert.ToDecimal(dtSingleLTBill.Rows[20][i + 1]) });
                    MonthYear = string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i]);
                }
            }
            MonthYear = "";
            slt.KVAgrph = KVAchrtData;
            //Line 21 End
            #endregion



            if (String.Equals(slt.L1_TODOrNon_TODFlag, "1"))
            {
                #region Line-22
                //Line 22 Starts
                LineNo = "22";
                slt.L22_TOD1_KWH = dtSingleLTBill.Rows[21][0].ToString();
                slt.L22_TOD2_KWH = dtSingleLTBill.Rows[21][1].ToString();
                slt.L22_TOD3_KWH = dtSingleLTBill.Rows[21][2].ToString();
                slt.L22_TOD4_KWH = dtSingleLTBill.Rows[21][3].ToString();
                //Line 22 End
                #endregion

                #region Line-23
                //Line 23 Starts
                LineNo = "23";
                slt.L23_TOD1_KW = dtSingleLTBill.Rows[22][0].ToString();
                slt.L23_TOD2_KW = dtSingleLTBill.Rows[22][1].ToString();
                slt.L23_TOD3_KW = dtSingleLTBill.Rows[22][2].ToString();
                slt.L23_TOD4_KW = dtSingleLTBill.Rows[22][3].ToString();
                //Line 23 End
                #endregion

                #region Line-24
                //Line 24 Starts
                LineNo = "24";
                slt.L24_TOD1_KWH = dtSingleLTBill.Rows[23][0].ToString();
                slt.L24_TOD2_KWH = dtSingleLTBill.Rows[23][1].ToString();
                slt.L24_TOD3_KWH = dtSingleLTBill.Rows[23][2].ToString();
                slt.L24_TOD4_KWH = dtSingleLTBill.Rows[23][3].ToString();
                //Line 24 End
                #endregion

                #region Line-25
                //Line 25 Starts
                LineNo = "25";
                slt.L25_TOD1_KW = dtSingleLTBill.Rows[24][0].ToString();
                slt.L25_TOD2_KW = dtSingleLTBill.Rows[24][1].ToString();
                slt.L25_TOD3_KW = dtSingleLTBill.Rows[24][2].ToString();
                slt.L25_TOD4_KW = dtSingleLTBill.Rows[24][3].ToString();
                //Line 25 End
                #endregion
            }

            #region Lines-26-31
            LineNo = "26";
            slt.L26_MESSAGE1 = dtSingleLTBill.Rows[25][0].ToString();
            LineNo = "27";
            slt.L27_MESSAGE2 = dtSingleLTBill.Rows[26][0].ToString();
            LineNo = "28";
            slt.L28_MESSAGE3 = dtSingleLTBill.Rows[27][0].ToString();
            LineNo = "29";
            slt.L29_MESSAGE4 = dtSingleLTBill.Rows[28][0].ToString();
            LineNo = "30";
            slt.L30_MESSAGE5 = dtSingleLTBill.Rows[29][0].ToString();
            LineNo = "31";
            slt.L31_MESSAGE6 = dtSingleLTBill.Rows[30][0].ToString();
            #endregion

            LineNo = "6";
            #region TemplateConditionalWithSTHindi
            DataView DVTemplateConditionalWithSTHindi = new DataView();
            DVTemplateConditionalWithSTHindi = TemplateConditionalWithSTHindi.DefaultView;
            DVTemplateConditionalWithSTHindi.RowFilter = "[1] = '" + slt.L6_TARIFF_DESCR + "'";

            DataTable TemplateConditionalWithSTHindiCopy = DVTemplateConditionalWithSTHindi.ToTable();
            for(int i=0; i< TemplateConditionalWithSTHindiCopy.Rows.Count; i++)
            {
                if (slt.L6_MeasureContractDemand.ToUpper() == "KW" && Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)>= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
                {
                    slt.L33_MESSAGE7 += TemplateConditionalWithSTHindiCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (slt.L6_MeasureContractDemand.ToUpper() == "KVA" && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD))*(0.9)) >= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD))*(0.9)) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
                {
                    slt.L33_MESSAGE7 += TemplateConditionalWithSTHindiCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (slt.L6_MeasureContractDemand.ToUpper() == "HP" && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD))*(0.746)) >= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD))*(0.746)) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
                {
                    slt.L33_MESSAGE7 += TemplateConditionalWithSTHindiCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
            }
            
            if (!string.IsNullOrEmpty(slt.L33_MESSAGE7))
            {
                slt.L33_MESSAGE7 = slt.L33_MESSAGE7.Replace('�', ' ');
                slt.L33_MESSAGE7 = slt.L33_MESSAGE7.TrimEnd(' ');
                slt.L33_MESSAGE7 = slt.L33_MESSAGE7.TrimEnd('\r');
                slt.L33_MESSAGE7 = slt.L33_MESSAGE7.TrimEnd('\n');
            }
            #endregion

            #region TemplateConditionalWithSTEnglish
            DataView DVTemplateConditionalWithSTEnglish = new DataView();
            DVTemplateConditionalWithSTEnglish = TemplateConditionalWithSTEnglish.DefaultView;
            DVTemplateConditionalWithSTEnglish.RowFilter = "[1] = '" + slt.L6_TARIFF_DESCR + "'";

            DataTable TemplateConditionalWithSTEnglishCopy = DVTemplateConditionalWithSTEnglish.ToTable();
            for (int i = 0; i < TemplateConditionalWithSTEnglishCopy.Rows.Count; i++)
            {
                if (slt.L6_MeasureContractDemand.ToUpper() == "KW" && Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
                {
                    slt.L34_MESSAGE8 += TemplateConditionalWithSTEnglishCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (slt.L6_MeasureContractDemand.ToUpper() == "KVA" && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD))*(0.9)) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD))*(0.9)) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
                {
                    slt.L34_MESSAGE8 += TemplateConditionalWithSTEnglishCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (slt.L6_MeasureContractDemand.ToUpper() == "HP" && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD))*(0.746)) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD))*(0.746)) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
                {
                    slt.L34_MESSAGE8 += TemplateConditionalWithSTEnglishCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
            }
            if (!string.IsNullOrEmpty(slt.L34_MESSAGE8))
            {
                slt.L34_MESSAGE8 = slt.L34_MESSAGE8.TrimEnd(' ');
                slt.L34_MESSAGE8 = slt.L34_MESSAGE8.TrimEnd('\r');
                slt.L34_MESSAGE8 = slt.L34_MESSAGE8.TrimEnd('\n');
            }
            #endregion

            #region TemplateConditionalWithServiceNoHindi
            DataView DVTemplateConditionalWithServiceNoHindi = new DataView();
            DVTemplateConditionalWithServiceNoHindi = TemplateConditionalWithServiceNoHindi.DefaultView;
            DVTemplateConditionalWithServiceNoHindi.RowFilter = "[1] = '" + slt.L6_SERVDET_SERVNO + "'";

            DataTable TemplateConditionalWithServiceNoHindiCopy = DVTemplateConditionalWithServiceNoHindi.ToTable();
            for (int i = 0; i < TemplateConditionalWithServiceNoHindiCopy.Rows.Count; i++)
            {
                slt.L35_MESSAGE9 += TemplateConditionalWithServiceNoHindiCopy.Rows[i]["2"].ToString().Trim('�') + " \r\n";
            }
            
            if (!string.IsNullOrEmpty(slt.L35_MESSAGE9))
            {
                slt.L35_MESSAGE9 = slt.L35_MESSAGE9.Replace('�', ' ');
                slt.L35_MESSAGE9 = slt.L35_MESSAGE9.TrimEnd(' ');
                slt.L35_MESSAGE9 = slt.L35_MESSAGE9.TrimEnd('\r');
                slt.L35_MESSAGE9 = slt.L35_MESSAGE9.TrimEnd('\n');
            }
            #endregion

            #region TemplateConditionalWithServiceNoEnglish
            DataView DVTemplateConditionalWithServiceNoEnglish = new DataView();
            DVTemplateConditionalWithServiceNoEnglish = TemplateConditionalWithServiceNoEnglish.DefaultView;
            DVTemplateConditionalWithServiceNoEnglish.RowFilter = "[1] = '" + slt.L6_SERVDET_SERVNO + "'";

            DataTable TemplateConditionalWithServiceNoEnglishCopy = DVTemplateConditionalWithServiceNoEnglish.ToTable();
            for (int i = 0; i < TemplateConditionalWithServiceNoEnglishCopy.Rows.Count; i++)
            {
                slt.L36_MESSAGE10 += TemplateConditionalWithServiceNoEnglishCopy.Rows[i]["2"].ToString().Trim('�') + " \r\n";
            }
            if (!string.IsNullOrEmpty(slt.L36_MESSAGE10))
            {
                slt.L36_MESSAGE10 = slt.L36_MESSAGE10.TrimEnd('\n');
                slt.L36_MESSAGE10 = slt.L36_MESSAGE10.TrimEnd('\r');
                slt.L36_MESSAGE10 = slt.L36_MESSAGE10.TrimEnd(' ');
            }


            #endregion

            #region Line-32
            //Line 32 Starts
            LineNo = "32";
            slt.L32_BarCode = dtSingleLTBill.Rows[31][0].ToString();
            //Line 32 End
            #endregion

            #region Line-37
            LineNo = "37";
            slt.L37_MonYear_1 = dtSingleLTBill.Rows[36][0].ToString();
            slt.L37_PowerFactor_1 = dtSingleLTBill.Rows[36][1].ToString();
            slt.L37_MonYear_2 = dtSingleLTBill.Rows[36][2].ToString();
            slt.L37_PowerFactor_2 = dtSingleLTBill.Rows[36][3].ToString();
            slt.L37_MonYear_3 = dtSingleLTBill.Rows[36][4].ToString();
            slt.L37_PowerFactor_3 = dtSingleLTBill.Rows[36][5].ToString();
            slt.L37_MonYear_4 = dtSingleLTBill.Rows[36][6].ToString();
            slt.L37_PowerFactor_4 = dtSingleLTBill.Rows[36][7].ToString();
            slt.L37_MonYear_5 = dtSingleLTBill.Rows[36][8].ToString();
            slt.L37_PowerFactor_5 = dtSingleLTBill.Rows[36][9].ToString();
            slt.L37_MonYear_6 = dtSingleLTBill.Rows[36][10].ToString();
            slt.L37_PowerFactor_6 = dtSingleLTBill.Rows[36][11].ToString();
            slt.L37_MonYear_7 = dtSingleLTBill.Rows[36][12].ToString();
            slt.L37_PowerFactor_7 = dtSingleLTBill.Rows[36][13].ToString();
            slt.L37_MonYear_8 = dtSingleLTBill.Rows[36][14].ToString();
            slt.L37_PowerFactor_8 = dtSingleLTBill.Rows[36][15].ToString();
            slt.L37_MonYear_9 = dtSingleLTBill.Rows[36][16].ToString();
            slt.L37_PowerFactor_9 = dtSingleLTBill.Rows[36][17].ToString();
            slt.L37_MonYear_10 = dtSingleLTBill.Rows[36][18].ToString();
            slt.L37_PowerFactor_10 = dtSingleLTBill.Rows[36][19].ToString();
            slt.L37_MonYear_11 = dtSingleLTBill.Rows[36][20].ToString();
            slt.L37_PowerFactor_11 = dtSingleLTBill.Rows[36][21].ToString();
            slt.L37_MonYear_12 = dtSingleLTBill.Rows[36][22].ToString();
            slt.L37_PowerFactor_12 = dtSingleLTBill.Rows[36][23].ToString();
            slt.L37_MonYear_13 = dtSingleLTBill.Rows[36][24].ToString();
            slt.L37_PowerFactor_13 = dtSingleLTBill.Rows[36][25].ToString();
            DataTable PFchrtData = new DataTable();
            PFchrtData.Columns.Add("MonthYear");
            PFchrtData.Columns.Add("Value", typeof(decimal));
            for (int i = 0; i <= 25; i += 2)
            {
                //PFchrtData.Rows.Add(new object[] {
                //    string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[36][i]))?Convert.ToString((i+1)/2):Convert.ToString(dtSingleLTBill.Rows[36][i]),
                //    string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[36][i + 1])) ? 0 : Convert.ToDecimal(dtSingleLTBill.Rows[36][i + 1]) });

                if (MonthYear != (string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i])))
                {
                    PFchrtData.Rows.Add(new object[] {
                    string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i]))?Convert.ToString((i+1)/2):Convert.ToString(dtSingleLTBill.Rows[20][i]),
                    string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[36][i + 1])) ? 0 : Convert.ToDecimal(dtSingleLTBill.Rows[36][i + 1]) });

                    MonthYear = string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i]);
                }
                else
                {
                    PFchrtData.Rows.Add(new object[] {
                    MonthYear.Replace("-","  "),
                    string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[36][i + 1])) ? 0 : Convert.ToDecimal(dtSingleLTBill.Rows[36][i + 1]) });

                    MonthYear = string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i]);
                }
            }
            MonthYear = "";
            slt.PFgrph = PFchrtData;
            Console.WriteLine("LTMD Line 37 parsed");
            #endregion
            #endregion

            #region Custom Fields
            var meter = slt.L12_MTRSNO_METER_2_IF_AVAILABLE.Trim() != "" ? slt.L12_MTRSNO_METER_2_IF_AVAILABLE : slt.L12_MTRSNO_METER1;
            slt.TopPanel_Row_1 = slt.L1_MonthYear + " / " + slt.L1_Zone + " / " + slt.L1_BU + " / " + slt.L1_Route + " / " + slt.L1_BillSequenceNo + " / " + slt.L1_SubRoute;
            slt.TopPanel_Row_2 = "Meter No. : " + meter;
            slt.TopPanel_Row_3 = "T No. : " + slt.L8_TNo;
            slt.TopPanel_Row_4 = "Bill Date : " + slt.L7_BillDt;
            if (String.Equals(slt.L1_TODOrNon_TODFlag, "1"))
            {
                slt.TopPanel_Row_5 = "Bill Days : " + slt.L10_Mode;
                slt.TopPanel_Row_6 = "11 KV FEEDER : " + slt.L1_FeederName;
            }
            else
                slt.TopPanel_Row_5 = "11 KV FEEDER : " + slt.L1_FeederName;


            //slt.L10_TotArrUPPCLIntUPPCLIntArrUPPCL = string.IsNullOrEmpty(dtSingleLTBill.Rows[9][2].ToString()) ? "0" : Math.Round(Convert.ToDecimal(dtSingleLTBill.Rows[9][2].ToString()) + (decimal).49, 0, MidpointRounding.AwayFromZero).ToString();
            //slt.L8_AmountPayableBeforeDueDate = string.IsNullOrEmpty(dtSingleLTBill.Rows[7][10].ToString()) ? "0" : Math.Round(Convert.ToDecimal(dtSingleLTBill.Rows[7][10].ToString()) + (decimal).49, 0, MidpointRounding.AwayFromZero).ToString();
            //slt.L8_ParkingAmount = string.IsNullOrEmpty(dtSingleLTBill.Rows[7][12].ToString()) ? "0" : Math.Round(Convert.ToDecimal(dtSingleLTBill.Rows[7][12].ToString()) + (decimal).49, 0, MidpointRounding.AwayFromZero).ToString();

            //dtSingleLTBill.Rows[9][2].ToString();
            Console.WriteLine("Custom Fields calculated");
            #endregion

            return slt;
        }

        PaperSourceCollection printerSources;
        void NonTOD_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            e.PrintDocument.DefaultPageSettings.PaperSource = e.PrintDocument.PrinterSettings.PaperSources[cbNonTODTraySource.SelectedIndex];
            //e.PrintDocument.DefaultPageSettings.PrinterResolution = e.PrintDocument.PrinterSettings.PrinterResolutions[i];
            //e.PrintDocument.PrintPage += PrintDocument_PrintPage;
            if (e.PrintDocument.PrinterSettings.CanDuplex)
                e.PrintDocument.PrinterSettings.Duplex = Duplex.Default;
            printerSources = e.PrintDocument.PrinterSettings.PaperSources;
        }

        void Seperator_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            e.PrintDocument.DefaultPageSettings.PaperSource = e.PrintDocument.PrinterSettings.PaperSources[cbSeparatorTraySource.SelectedIndex];
            // e.PrintDocument.DefaultPageSettings.PrinterResolution = e.PrintDocument.PrinterSettings.PrinterResolutions[i];
            e.PrintDocument.PrintPage += PrintDocument_PrintPage;
            if (e.PrintDocument.PrinterSettings.CanDuplex)
                e.PrintDocument.PrinterSettings.Duplex = Duplex.Default;
        }

        void TOD_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            e.PrintDocument.DefaultPageSettings.PaperSource = e.PrintDocument.PrinterSettings.PaperSources[cbTODTraySource.SelectedIndex];
            //e.PrintDocument.DefaultPageSettings.PrinterResolution = e.PrintDocument.PrinterSettings.PrinterResolutions[i];
            //e.PrintDocument.PrintPage += PrintDocument_PrintPage;
            if (e.PrintDocument.PrinterSettings.CanDuplex)
                e.PrintDocument.PrinterSettings.Duplex = Duplex.Default;
        }

        void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.PageSettings.PrinterSettings.Duplex = Duplex.Vertical;
        }

        private void cbDefaultPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            AppFunctions.ShowWaitForm("Loading Paper Sources...");
            PrintDocument printDoc = new PrintDocument();
            cbNonTODTraySource.Properties.Items.Clear();
            cbTODTraySource.Properties.Items.Clear();
            cbSeparatorTraySource.Properties.Items.Clear();
            printDoc.PrinterSettings.PrinterName = cbDefaultPrinter.SelectedText;
            PaperSourceCollection ps = printDoc.PrinterSettings.PaperSources;

            for (int i = 0; i < ps.Count; i++)
            {
                PaperSource pkSource = printDoc.PrinterSettings.PaperSources[i];
                cbNonTODTraySource.Properties.Items.Add(ps[i].SourceName);
            }
            for (int i = 0; i < printDoc.PrinterSettings.PaperSources.Count; i++)
            {
                PaperSource pkSource = printDoc.PrinterSettings.PaperSources[i];
                cbSeparatorTraySource.Properties.Items.Add(ps[i].SourceName);
            }
            for (int i = 0; i < printDoc.PrinterSettings.PaperSources.Count; i++)
            {
                PaperSource pkSource = printDoc.PrinterSettings.PaperSources[i];
                cbTODTraySource.Properties.Items.Add(ps[i].SourceName);
            }
            cbNonTODTraySource.SelectedIndex = 0;
            cbTODTraySource.SelectedIndex = 0;
            cbSeparatorTraySource.SelectedIndex = 0;

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
                    //LoadStaticData.ProcessedBillData(ofdMsg.FileName);
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
                    XtraMessageBox.Show("Total Bill in this file " + singleLTMDBills.Length.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AppFunctions.ShowWaitForm("Generating LTMD Bills Now..!!");

                    StartPrinting_LTMDBills(singleLTMDBills, sb.Name);
                }
            }
        }

        private void Printers_Refresh_Button_Click(object sender, EventArgs e)
        {
            AppFunctions.ShowWaitForm("Please wait..");
            cbDefaultPrinter.SelectedIndex = -1;
            cbDefaultPrinter.Properties.Items.Clear();
            cbNonTODTraySource.Properties.Items.Clear();
            cbTODTraySource.Properties.Items.Clear();
            cbSeparatorTraySource.Properties.Items.Clear();
            foreach (var printers in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                cbDefaultPrinter.Properties.Items.Add(printers);
            }
            AppFunctions.CloseWaitForm();
        }

        private bool ValidatetxtFile(string[] bills)
        {
            try
            {
                int BillNo = 0;
                foreach (var bill in bills)
                {
                    BillNo++;
                    DataTable dtSingleLTBill = ParseAsDataTable.LTMD_FileTxtToDataTable(bill, BillNo, "LTMD");
                    if (dtSingleLTBill.Rows.Count == 37)
                    {
                        SingleLTMDBill slt = parseSingleLTMDBill(dtSingleLTBill);
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

