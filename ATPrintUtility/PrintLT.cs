using AT.Print;
using AT.Print.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Printing;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using static System.Drawing.Printing.PrinterSettings;
using DevExpress.XtraPrinting.Drawing;
using System.Management;
using DevExpress.XtraPrinting;

using iTextSharp.text;
using iTextSharp.text.pdf;
using Document = iTextSharp.text.Document;
using System.Web;
using System.Web.UI.HtmlControls;

namespace AT.Print
{
    public partial class PrintLT : UserControl
    {
        int pae = 0;
        public PrintLT()
        {
            InitializeComponent();
            BindPrinters();
        }
        string textFileName;
        string mVImagePath;
        string ServiceNo = "";
        string MonthYear = "";
        String LineNo = "";
        DataSet DSBill = new DataSet();
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
        bool HavingSaperator = false;
        string[] singleLTBills;

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
                    if (String.Equals(this.Name, "PrintLT") && contents.StartsWith("LT|"))
                    {
                        singleLTBills = contents.Split(new String[] { "LT|" }, StringSplitOptions.RemoveEmptyEntries);
                        if (!select_mVImg())
                        {
                            AppFunctions.CloseWaitForm();
                            return;
                        }
                        XtraMessageBox.Show("Total Bill in this file " + singleLTBills.Length.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AppFunctions.ShowWaitForm("Generating LT Bills Now..!!");
                        var sb = sender as SimpleButton;
                        if (ValidatetxtFile(singleLTBills))
                        {

                            StartPrinting_LTBills(singleLTBills, sb.Name, 0, singleLTBills.Count() - 1, "1");
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

        #region noInUse
        //void StartPrinting_LTBillsNew(string[] Bills, string Name, int Initial, int Final, string FolderName)
        ////void StartPrinting_LTBills(object callback)
        //{
        //    string LotNo = "InitialLot";
        //    string LotNoCopy = "InitialLot";
        //    int BillNo = 1, Counter = 1, ParsedBills = 0;

        //    collectorReport = new XtraReport
        //    {
        //        DisplayName = "LT Print",
        //    };


        //    List<int> inlist = Enumerable.Range(0, Final).ToList();


        //    Parallel.ForEach(inlist, new ParallelOptions { MaxDegreeOfParallelism = 50 }, z =>
        //    {

        //        DataTable dtSingleLTBill = DSBill.Tables[z];
        //        try
        //        {
        //            List<SingleLTBill> lstformattedbills = new List<SingleLTBill>();

        //            //DataTable dtSingleLTBill = ParseAsDataTable.LT_FileTxtToDataTable(Bill);
        //            if ((LotNoCopy != dtSingleLTBill.Rows[0][4].ToString().Trim() || Counter == 51) && LotNoCopy != "InitialLot")
        //            {
        //                MemoryStream ms = new MemoryStream();
        //                var buffer = ms.GetBuffer();
        //                Array.Clear(buffer, 0, buffer.Length);
        //                ms.Position = 0;
        //                ms.SetLength(0);
        //                ms.Capacity = 0; // <<< 
        //                //rpta.Print(cbDefaultPrinter.Text);
        //                collectorReport.Print(cbDefaultPrinter.Text);
        //                for (int i = collectorReport.Pages.Count - 1; i > -1; i--)
        //                    collectorReport.Pages.RemoveAt(i);
        //                Counter = 1;
        //                collectorReport.Dispose();

        //            }
        //            if (LotNo != dtSingleLTBill.Rows[0][4].ToString().Trim())
        //            {
        //                if (Name != "sbSavePDF")
        //                {
        //                    LotNo = dtSingleLTBill.Rows[0][4].ToString().Trim();
        //                    LotNoCopy = dtSingleLTBill.Rows[0][4].ToString().Trim();
        //                    SingleLTBill billSaprator = new SingleLTBill();
        //                    billSaprator.Sap_Zone = "Zone No. " + Convert.ToString(dtSingleLTBill.Rows[0][1]);
        //                    billSaprator.Sap_LotNo = "LOT No. " + Convert.ToString(dtSingleLTBill.Rows[0][4]);
        //                    billSaprator.Sap_GrpNo = "Group No. " + Convert.ToString(dtSingleLTBill.Rows[0][2]);
        //                    billSaprator.lblSapratorNote = "Banner Page for the Start of the LOT";
        //                    lstformattedbills.Add(billSaprator);

        //                    Rpt_Saprator sap_rpt = new Rpt_Saprator
        //                    {
        //                        DataSource = lstformattedbills
        //                    };
        //                    sap_rpt.CreateDocument();
        //                    sap_rpt.ShowPrintMarginsWarning = false;
        //                    sap_rpt.PrinterName = cbDefaultPrinter.Text;
        //                    sap_rpt.PrintingSystem.StartPrint += Seperator_StartPrint;
        //                    //collectorReport.Pages.AddRange(sap_rpt.Pages);
        //                    sap_rpt.Print(cbDefaultPrinter.Text);
        //                    lstformattedbills.Clear();
        //                    //ParsedBills++;

        //                }
        //            }

        //            SingleLTBill slt = parseSingleLTBill(dtSingleLTBill);
        //            slt.MVPicture = mVImagePath;
        //            lstformattedbills.Add(slt);

        //            if (Name == "sbSavePDF")
        //            {
        //                AT.Print.PDF.Rpt_LTPDF rptsd = new AT.Print.PDF.Rpt_LTPDF
        //                {
        //                    DataSource = lstformattedbills,
        //                    // ShowPrintStatusDialog = false,
        //                    //ShowPreviewMarginLines = false

        //                };

        //                #region WaterMark Picture Front Page PDF Non-TOD
        //                DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkFrontNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
        //                pictureWatermarkFrontNonTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Non_TOD_Front_Page.png");
        //                pictureWatermarkFrontNonTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
        //                pictureWatermarkFrontNonTOD.ImageTiling = false;
        //                pictureWatermarkFrontNonTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
        //                pictureWatermarkFrontNonTOD.ImageTransparency = 0;
        //                pictureWatermarkFrontNonTOD.ShowBehind = true;
        //                //pictureWatermark.PageRange = "2,4";
        //                rptsd.Watermark.CopyFrom(pictureWatermarkFrontNonTOD);
        //                #endregion

        //                rptsd.CreateDocument(false);


        //                AT.Print.PDF.rpt_LT_Back rpts = new AT.Print.PDF.rpt_LT_Back
        //                {
        //                    DataSource = lstformattedbills,
        //                    //ShowPrintStatusDialog = false,
        //                    // ShowPreviewMarginLines = false

        //                };

        //                #region WaterMark Picture Back Page PDF Non-TOD
        //                DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkBackNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
        //                pictureWatermarkBackNonTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Non_TOD_Back_Page.png");
        //                pictureWatermarkBackNonTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
        //                pictureWatermarkBackNonTOD.ImageTiling = false;
        //                pictureWatermarkBackNonTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
        //                pictureWatermarkBackNonTOD.ImageTransparency = 0;
        //                pictureWatermarkBackNonTOD.ShowBehind = true;
        //                //pictureWatermark.PageRange = "2,4";
        //                rpts.Watermark.CopyFrom(pictureWatermarkBackNonTOD);
        //                #endregion

        //                rpts.CreateDocument(false);

        //                //rpts.Watermark.ImageTransparency = 0;

        //                //rpts.DesignerOptions.ShowPrintingWarnings = false;
        //                //rpts.DesignerOptions.ShowExportWarnings = false;
        //                //rpts.ShowPrintMarginsWarning = false;
        //                //rpts.CreateDocument();
        //                rptsd.ModifyDocument(x => { x.AddPages(rpts.Pages); });
        //                DevExpress.XtraPrinting.Page myPage2 = rptsd.Pages[1];
        //                myPage2.AssignWatermark(pictureWatermarkBackNonTOD);
        //                string billdate = lstformattedbills.FirstOrDefault().L1_MonthYear;
        //                string ServiceNo = lstformattedbills.FirstOrDefault().L6_SERVDET_SERVNO;
        //                //DateTime.TryParseExact(billdate, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime billdate);
        //                var outputfolder = "C://Bills//LT Files//" + billdate + "//" + textFileName;
        //                OutputFolderPath OFP = new OutputFolderPath();
        //                outputfolder = OFP.LoadLocation() + "//LT Files//" + billdate + "//" + textFileName; ;
        //                if (!Directory.Exists(outputfolder))
        //                    Directory.CreateDirectory(outputfolder);
        //                //var OutPutFolder = 
        //                if (Directory.Exists(outputfolder))
        //                {
        //                    new PdfStreamingExporter(rptsd, true).Export(outputfolder + "//" + ServiceNo + ".pdf");
        //                }
        //                AppFunctions.CloseWaitForm();
        //                ParsedBills++;
        //            }
        //            else
        //            {
        //                PrinterSettings ps = new PrinterSettings() { PrinterName = cbDefaultPrinter.Text };
        //                using (Graphics g = ps.CreateMeasurementGraphics(ps.DefaultPageSettings))
        //                {
        //                    Margins MinMargins = DevExpress.XtraPrinting.Native.DeviceCaps.GetMinMargins(g);
        //                    Console.WriteLine("Minimum Margins for " + ps.PrinterName + ": " + MinMargins.ToString());
        //                }

        //                AT.Print.Rpt_LT_Print rpta = new AT.Print.Rpt_LT_Print
        //                {
        //                    DataSource = lstformattedbills,
        //                    ShowPrintStatusDialog = false,
        //                    ShowPreviewMarginLines = false

        //                };

        //                rpta.DrawWatermark = false;
        //                rpta.Watermark.ImageTransparency = 250;
        //                rpta.DesignerOptions.ShowPrintingWarnings = false;
        //                rpta.DesignerOptions.ShowExportWarnings = false;
        //                rpta.ShowPrintMarginsWarning = false;
        //                //rpta.Print();
        //                AT.Print.Rpt_LT_Print_Back rptb = new AT.Print.Rpt_LT_Print_Back
        //                {
        //                    DataSource = lstformattedbills,
        //                    ShowPrintStatusDialog = false,
        //                    ShowPreviewMarginLines = false

        //                };
        //                rpta.CreateDocument();
        //                rptb.DrawWatermark = false;
        //                rptb.Watermark.ImageTransparency = 250;
        //                rptb.DesignerOptions.ShowPrintingWarnings = false;
        //                rptb.DesignerOptions.ShowExportWarnings = false;
        //                rptb.ShowPrintMarginsWarning = false;

        //                rptb.CreateDocument();
        //                rpta.ModifyDocument(x => { x.AddPages(rptb.Pages); });
        //                collectorReport.PrintingSystem.StartPrint += NonTOD_StartPrint;
        //                collectorReport.PrinterName = cbDefaultPrinter.Name;
        //                collectorReport.Pages.AddRange(rpta.Pages);
        //                //if (Bills.Count() == BillNo && LotNoCopy != "InitialLot")
        //                if (DSBill.Tables.Count == BillNo && LotNoCopy != "InitialLot")
        //                {
        //                    collectorReport.Print(cbDefaultPrinter.Text);
        //                    for (int i = collectorReport.Pages.Count - 1; i > -1; i--)
        //                        collectorReport.Pages.RemoveAt(i);
        //                }

        //                //rpta.Print(cbDefaultPrinter.Text);
        //                AppFunctions.CloseWaitForm();
        //                ParsedBills++;
        //                Counter++;
        //            }

        //            // };

        //        }
        //        catch (System.OutOfMemoryException)
        //        {
        //            System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
        //            GC.Collect();
        //            GC.RemoveMemoryPressure(1024 * 1024);
        //        }
        //        catch (Exception ex)
        //        {
        //            AppFunctions.LogError(ex);
        //            AppFunctions.CloseWaitForm();
        //            XtraMessageBox.Show("Error Parsing Bill " + BillNo + " of the given file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            Console.WriteLine(ex.Message);
        //        }

        //        BillNo++;
        //        //} // end While
        //    });
        //    DSBill.Reset();
        //    DSBill.Dispose();
        //    XtraMessageBox.Show(ParsedBills + " Bills Parsed Successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    AppFunctions.CloseWaitForm();

        //}
        #endregion

        void StartPrinting_LTBills(string[] Bills, string Name, int Initial, int Final, string FolderName)

        {
            pae = 0;
            string LotNo = "InitialLot";
            string LotNoCopy = "InitialLot";
            int BillNo = 1, Counter = 1, ParsedBills = 0;
            //string Name = "sbSavePDF";
            string FileName = AppFunctions.ProcessedBillData();
            var collectorReport = new XtraReport()
            {
                DisplayName = "LT Print",
            };






            DataTable dtSingleLTBill = new DataTable();
            List<SingleLTBill> lstformattedbills;
            MemoryStream ms = new MemoryStream();
            SingleLTBill billSaprator = new SingleLTBill();

            int z = Initial - 1;
            while (z < Final)
            {
                z++;
                dtSingleLTBill = DSBill.Tables[z];
                try
                {
                    lstformattedbills = new List<SingleLTBill>();

                    //if ((LotNoCopy != dtSingleLTBill.Rows[0][4].ToString().Trim()) && LotNoCopy != "InitialLot")                      // Complete Loat
                    if ((LotNoCopy != dtSingleLTBill.Rows[0][4].ToString().Trim() || Counter == 51) && LotNoCopy != "InitialLot")       // 51 Pages Loat
                    {
                        pae = 0;
                        var buffer = ms.GetBuffer();
                        Array.Clear(buffer, 0, buffer.Length);
                        ms.Position = 0;
                        ms.SetLength(0);
                        ms.Capacity = 0;

                        collectorReport.PrintProgress += new DevExpress.XtraPrinting.PrintProgressEventHandler(CR_PrintProgress);
                        //collectorReport.BeforePrint += BeforePrint;
                        collectorReport.Print(cbDefaultPrinter.Text);
                        HavingSaperator = false;
                        collectorReport.Pages.Clear();
                        Counter = 1;
                        collectorReport.Dispose();
                    }

                    if (LotNo != dtSingleLTBill.Rows[0][4].ToString().Trim())
                    {
                        LotNo = dtSingleLTBill.Rows[0][4].ToString().Trim();
                        LotNoCopy = dtSingleLTBill.Rows[0][4].ToString().Trim();

                        billSaprator.Sap_Zone = "Zone No. " + Convert.ToString(dtSingleLTBill.Rows[0][1]);
                        billSaprator.Sap_LotNo = "LOT No. " + Convert.ToString(dtSingleLTBill.Rows[0][4]);
                        billSaprator.Sap_GrpNo = "Group No. " + Convert.ToString(dtSingleLTBill.Rows[0][2]);
                        billSaprator.lblSapratorNote = "Banner Page for the Start of the LOT";
                        lstformattedbills.Add(billSaprator);

                        Rpt_Saprator sap_rpt = new Rpt_Saprator
                        {
                            DataSource = lstformattedbills
                        };
                        sap_rpt.CreateDocument();
                        sap_rpt.ShowPrintMarginsWarning = false;
                        sap_rpt.PrinterName = cbDefaultPrinter.Text;
                        HavingSaperator = true;
                        collectorReport.Pages.AddRange(sap_rpt.Pages);
                        lstformattedbills.Clear();
                    }

                    SingleLTBill slt = parseSingleLTBill(dtSingleLTBill);
                    slt.MVPicture = mVImagePath;
                    lstformattedbills.Add(slt);

                    PrinterSettings ps = new PrinterSettings(){ PrinterName = cbDefaultPrinter.Text }; 
                    PrinterResolution printerresolution = new PrinterResolution
                    {
                        Kind = PrinterResolutionKind.Custom, 
                        X = 1200, 
                        Y = 1200  
                    };
                    ps.DefaultPageSettings.PrinterResolution = printerresolution;
                    using (Graphics g = ps.CreateMeasurementGraphics(ps.DefaultPageSettings))
                    {
                        Margins MinMargins = DevExpress.XtraPrinting.Native.DeviceCaps.GetMinMargins(g);
                        Console.WriteLine("Minimum Margins for " + ps.PrinterName + ": " + MinMargins.ToString());
                    }

                    AT.Print.Rpt_LT_Print rpta = new AT.Print.Rpt_LT_Print
                    {
                        DataSource = lstformattedbills,
                        ShowPrintStatusDialog = false,
                        ShowPreviewMarginLines = false
                    };

                    rpta.DrawWatermark = false;
                    rpta.Watermark.ImageTransparency = 250;
                    rpta.DesignerOptions.ShowPrintingWarnings = false;
                    rpta.DesignerOptions.ShowExportWarnings = false;
                    rpta.ShowPrintMarginsWarning = false;
                    AT.Print.Rpt_LT_Print_Back rptb = new AT.Print.Rpt_LT_Print_Back
                    {
                        DataSource = lstformattedbills,
                        ShowPrintStatusDialog = false,
                        ShowPreviewMarginLines = false
                    };
                    rpta.CreateDocument();
                    rptb.DrawWatermark = false;
                    rptb.Watermark.ImageTransparency = 250;
                    rptb.DesignerOptions.ShowPrintingWarnings = false;
                    rptb.DesignerOptions.ShowExportWarnings = false;
                    rptb.ShowPrintMarginsWarning = false;

                    rptb.CreateDocument();
                    rpta.ModifyDocument(x => { x.AddPages(rptb.Pages); });
                    collectorReport.PrinterName = cbDefaultPrinter.Name;
                    collectorReport.Pages.AddRange(rpta.Pages);

                    if (DSBill.Tables.Count == BillNo && LotNoCopy != "InitialLot")
                    {
                        /*
                        string printerName = cbDefaultPrinter.Text;
                        string query = string.Format("SELECT * from Win32_Printer WHERE Name LIKE '%{0}'", printerName);

                        using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                        using (ManagementObjectCollection coll = searcher.Get())
                        {
                            try
                            {
                                foreach (ManagementObject printer in coll)
                                {
                                    foreach (PropertyData property in printer.Properties)
                                    {
                                        Console.WriteLine(string.Format("{0}: {1}", property.Name, property.Value));
                                    }
                                }
                            }
                            catch (ManagementException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        */


                        //ReportPrintTool printTool = new ReportPrintTool(collectorReport);
                        //printTool.ShowPreview();

                        collectorReport.PrintProgress += new DevExpress.XtraPrinting.PrintProgressEventHandler(CR_PrintProgress);
                        //collectorReport.BeforePrint += BeforePrint;
                        collectorReport.Print(cbDefaultPrinter.Text);
                        collectorReport.Pages.Clear();
                        HavingSaperator = false;
                        rpta.Dispose();
                        rptb.Dispose();
                    }

                    AppFunctions.CloseWaitForm();
                    ParsedBills++;
                    Counter++;
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
                    AppFunctions.LogError("exception at Service No." + ServiceNo, ex);
                    AppFunctions.LogProcessedBill(Convert.ToString(dtSingleLTBill.Rows[0][1]), Convert.ToString(dtSingleLTBill.Rows[0][4]), Convert.ToString(dtSingleLTBill.Rows[0][2]), Convert.ToString(dtSingleLTBill.Rows[0][5]), ServiceNo, FileName, "No");
                    AppFunctions.CloseWaitForm();
                    //XtraMessageBox.Show("Error Parsing Bill " + BillNo + " of the given file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }

                AppFunctions.LogProcessedBill(Convert.ToString(dtSingleLTBill.Rows[0][1]), Convert.ToString(dtSingleLTBill.Rows[0][4]), Convert.ToString(dtSingleLTBill.Rows[0][2]), Convert.ToString(dtSingleLTBill.Rows[0][5]), ServiceNo, FileName, "Yes");

                BillNo++;

            }

            DSBill.Reset();
            DSBill.Dispose();
            XtraMessageBox.Show(ParsedBills + " Bills Parsed Successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            AppFunctions.CloseWaitForm();
        }

        public static void SaveFile(string data)
        {
            string dateTime = DateTime.Now.ToString("ddMMMMyyyy HHmmss");
            string dirParameter = Application.StartupPath + "\\Contents\\CategorySlabImages\\Processed_Bills\\Torrent_Processed_Bill_ATF" + dateTime + ".csv";
            string Msg = data;
            FileStream fParameter = new FileStream(dirParameter, FileMode.Create, FileAccess.Write);
            StreamWriter m_WriterParameter = new StreamWriter(fParameter);
            m_WriterParameter.BaseStream.Seek(0, SeekOrigin.End);
            m_WriterParameter.Write(Msg);
            m_WriterParameter.Flush();
            m_WriterParameter.Close();
        }

        private void BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var d = sender as PrintDocument;
        }
        SingleLTBill parseSingleLTBill(DataTable dtSingleLTBill)
        {


            SingleLTBill slt = new SingleLTBill();
            #region --Lines
            #region Line-1
            ServiceNo = dtSingleLTBill.Rows[5][0].ToString();
            //Line 1 Starts
            LineNo = "1";
            slt.L1_BillType = "LT";
            slt.L1_MonthYear = dtSingleLTBill.Rows[0][0].ToString();
            slt.L1_Zone = dtSingleLTBill.Rows[0][1].ToString();
            slt.L1_BU = dtSingleLTBill.Rows[0][2].ToString();
            slt.L1_PC = dtSingleLTBill.Rows[0][3].ToString();
            slt.L1_Route = dtSingleLTBill.Rows[0][4].ToString();
            slt.L1_BillSequenceNo = dtSingleLTBill.Rows[0][5].ToString();
            slt.L1_ReadingSequence = dtSingleLTBill.Rows[0][6].ToString();
            slt.L1_PowerFactorMSGIndicator = dtSingleLTBill.Rows[0][7].ToString();
            slt.L1_FeederName = dtSingleLTBill.Rows[0][8].ToString();
            slt.L1_TODOrNon_TODFlag = dtSingleLTBill.Rows[0][19].ToString();
            slt.L1_AKY_indicator = dtSingleLTBill.Rows[0][10].ToString();
            slt.L1_DisconnectionMSGPrintingIMMEDIATE = dtSingleLTBill.Rows[0][11].ToString();
            slt.L1_BillingCode = dtSingleLTBill.Rows[0][12].ToString();
            if (dtSingleLTBill.Rows[0][13].ToString().Trim() == "")
            {
                slt.L1_Customer_PAN = dtSingleLTBill.Rows[0][13].ToString();
            }
            else
            {
                slt.L1_Customer_PAN = "PAN: " + dtSingleLTBill.Rows[0][13].ToString();
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
            slt.L8_RegulatoryCharge_1 = dtSingleLTBill.Rows[7][7].ToString();
            slt.L8_RegulatoryCharge_2 = dtSingleLTBill.Rows[7][8].ToString();
            slt.L8_RebateIncurredCurrentMonth = dtSingleLTBill.Rows[7][9].ToString();
            slt.L8_AmountPayableBeforeDueDate = dtSingleLTBill.Rows[7][10].ToString();
            slt.L8_AmountPayableBeforeDueDate = slt.L8_AmountPayableBeforeDueDate.Contains("CR") ? ("-" + slt.L8_AmountPayableBeforeDueDate.Replace("CR", "")) : (slt.L8_AmountPayableBeforeDueDate.Contains("-") ? ("-" + slt.L8_AmountPayableBeforeDueDate.Replace("-", "")) : slt.L8_AmountPayableBeforeDueDate);
            slt.L8_TNo = dtSingleLTBill.Rows[7][11].ToString().Trim('�');
            slt.L8_ParkingAmount = dtSingleLTBill.Rows[7][12].ToString();
            slt.L8_Subsidy_Charges = dtSingleLTBill.Rows[7][13].ToString();
            slt.L8_Solar_Export_Energy = dtSingleLTBill.Rows[7][14].ToString();
            slt.L8_GreenTariff_Charges = dtSingleLTBill.Rows[7][15].ToString();
          //  slt.L8_Intrest_Amount = dtSingleLTBill.Rows[7][15].ToString();
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
            slt.L10_TotArrUPPCLIntUPPCLIntArrUPPCL = dtSingleLTBill.Rows[9][2].ToString();
            slt.L10_SecDeptBdt = dtSingleLTBill.Rows[9][3].ToString();
            slt.L10_DmdChgPenalty = dtSingleLTBill.Rows[9][4].ToString();
            slt.L10_UPPCL_ArrearAmount = dtSingleLTBill.Rows[9][5].ToString();
            slt.L10_UPPCLIntOnArrearAmount = dtSingleLTBill.Rows[9][6].ToString();
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
            for (int i = 0; i <= 25; i += 2)
            {
                var crg = chrtData.NewRow();

                if (MonthYear != (string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i])))
                {
                    crg["MonthYear"] = string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i]);
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

            for (int i = 0; i <= 25; i += 2)
            {
                //KVAchrtData.Rows.Add(new object[] { string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i]), string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i + 1])) ? 0 : Convert.ToDecimal(dtSingleLTBill.Rows[20][i + 1]) });
                if (MonthYear != (string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i])))
                {
                    KVAchrtData.Rows.Add(new object[] { string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i]), string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i + 1])) ? 0 : Convert.ToDecimal(dtSingleLTBill.Rows[20][i + 1]) });
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

            #region Line-22
            //Line 22 Starts
            LineNo = "22";
            slt.L22_MESSAGE1 = dtSingleLTBill.Rows[21][0].ToString();
            //Line 22 End
            #endregion

            #region Line-23
            //Line 23 Starts
            LineNo = "23";
            if (dtSingleLTBill.Rows.Count >= 23)
                slt.L23_MESSAGE2 = dtSingleLTBill.Rows[22][0].ToString();
            //Line 23 End
            #endregion

            #region Line-24
            //Line 24 Starts
            LineNo = "24";
            if (dtSingleLTBill.Rows.Count >= 24)
                slt.L24_MESSAGE3 = dtSingleLTBill.Rows[23][0].ToString();
            //Line 24 End
            #endregion

            #region Line-25
            //Line 25 Starts
            LineNo = "25";
            if (dtSingleLTBill.Rows.Count >= 25)
                slt.L25_MESSAGE4 = dtSingleLTBill.Rows[24][0].ToString();
            //Line 25 End
            #endregion

            #region Line-26
            //Line 26 Starts
            LineNo = "26";
            if (dtSingleLTBill.Rows.Count >= 26)
                slt.L26_MESSAGE5 = dtSingleLTBill.Rows[25][0].ToString();
            //Line 26 End
            #endregion

            #region Line-27
            //Line 27 Starts
            LineNo = "27";
            if (dtSingleLTBill.Rows.Count >= 27)
                slt.L27_MESSAGE6 = dtSingleLTBill.Rows[26][0].ToString();
            //Line 27 End
            #endregion

            #region Line-28
            //Line 28 Starts
            LineNo = "28";
            if (dtSingleLTBill.Rows.Count >= 28)
                slt.L28_MESSAGE7 = dtSingleLTBill.Rows[27][0].ToString();
            //Line 28 End
            #endregion

            #region Line-29
            //Line 29 Starts
            LineNo = "29";
            if (dtSingleLTBill.Rows.Count >= 29)
                slt.L29_MESSAGE8 = dtSingleLTBill.Rows[28][0].ToString();
            //Line 29 End
            #endregion

            #region Line-30
            //Line 30 Starts
            LineNo = "30";
            if (dtSingleLTBill.Rows.Count >= 30)
                slt.L30_MESSAGE9 = dtSingleLTBill.Rows[29][0].ToString();
            //Line 30 End
            #endregion

            #region Line-31
            //Line 31 Starts
            LineNo = "31";
            if (dtSingleLTBill.Rows.Count >= 31)
                slt.L31_MESSAGE10 = dtSingleLTBill.Rows[30][0].ToString();
            //Line 31 End
            #endregion

            LineNo = "6";
            #region TemplateConditionalWithSTHindi
            //DataView DVTemplateConditionalWithSTHindi = new DataView();
            //DVTemplateConditionalWithSTHindi = TemplateConditionalWithSTHindi.DefaultView;
            //DVTemplateConditionalWithSTHindi.RowFilter = "[1] = '" + slt.L6_TARIFF_DESCR + "'";
            DataTable TemplateConditionalWithSTHindiCopy = new DataTable();
            var RowsTemplateConditionalWithSTHindi = TemplateConditionalWithSTHindi.AsEnumerable().Where(row => row.Field<string>("1") == slt.L6_TARIFF_DESCR);
            if (RowsTemplateConditionalWithSTHindi.Any())
            {
                TemplateConditionalWithSTHindiCopy = RowsTemplateConditionalWithSTHindi.CopyToDataTable<DataRow>();
            }
            //TemplateConditionalWithSTHindiCopy = DVTemplateConditionalWithSTHindi.ToTable();
            for (int i = 0; i < TemplateConditionalWithSTHindiCopy.Rows.Count; i++)
            {
                if (slt.L6_MeasureContractDemand.ToUpper() == "KW" && Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD) >= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
                {
                    slt.L33_MESSAGE7 += TemplateConditionalWithSTHindiCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (slt.L6_MeasureContractDemand.ToUpper() == "KVA" && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)) * (0.9)) >= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)) * (0.9)) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
                {
                    slt.L33_MESSAGE7 += TemplateConditionalWithSTHindiCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (slt.L6_MeasureContractDemand.ToUpper() == "HP" && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)) * (0.746)) >= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)) * (0.746)) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
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
            //DataView DVTemplateConditionalWithSTEnglish = new DataView();
            //DVTemplateConditionalWithSTEnglish = TemplateConditionalWithSTEnglish.DefaultView;
            //DVTemplateConditionalWithSTEnglish.RowFilter = "[1] = '" + slt.L6_TARIFF_DESCR + "'";
            DataTable TemplateConditionalWithSTEnglishCopy = new DataTable();
            var RowsTemplateConditionalWithSTEnglish = TemplateConditionalWithSTEnglish.AsEnumerable().Where(row => row.Field<string>("1") == slt.L6_TARIFF_DESCR);
            if (RowsTemplateConditionalWithSTEnglish.Any())
            {
                TemplateConditionalWithSTEnglishCopy = RowsTemplateConditionalWithSTEnglish.CopyToDataTable<DataRow>();
            }
            //DataTable TemplateConditionalWithSTEnglishCopy = DVTemplateConditionalWithSTEnglish.ToTable();

            for (int i = 0; i < TemplateConditionalWithSTEnglishCopy.Rows.Count; i++)
            {
                if (slt.L6_MeasureContractDemand.ToUpper() == "KW" && Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
                {
                    slt.L34_MESSAGE8 += TemplateConditionalWithSTEnglishCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (slt.L6_MeasureContractDemand.ToUpper() == "KVA" && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)) * (0.9)) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)) * (0.9)) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
                {
                    slt.L34_MESSAGE8 += TemplateConditionalWithSTEnglishCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (slt.L6_MeasureContractDemand.ToUpper() == "HP" && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)) * (0.746)) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)) * (0.746)) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
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
            //DataView DVTemplateConditionalWithServiceNoHindi = new DataView();
            //DVTemplateConditionalWithServiceNoHindi = TemplateConditionalWithServiceNoHindi.DefaultView;
            //DVTemplateConditionalWithServiceNoHindi.RowFilter = "[1] = '" + slt.L6_SERVDET_SERVNO + "'";
            DataTable TemplateConditionalWithServiceNoHindiCopy = new DataTable();
            var RowsTemplateConditionalWithServiceNoHindi = TemplateConditionalWithServiceNoHindi.AsEnumerable().Where(row => row.Field<string>("1") == slt.L6_SERVDET_SERVNO);
            if (RowsTemplateConditionalWithServiceNoHindi.Any())
            {
                TemplateConditionalWithServiceNoHindiCopy = RowsTemplateConditionalWithServiceNoHindi.CopyToDataTable<DataRow>();
            }
            //DataTable TemplateConditionalWithServiceNoHindiCopy = DVTemplateConditionalWithServiceNoHindi.ToTable();
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
            DataTable TemplateConditionalWithServiceNoEnglishCopy = new DataTable();
            var RowsTemplateConditionalWithServiceNoEnglish = TemplateConditionalWithServiceNoEnglish.AsEnumerable().Where(row => row.Field<string>("1") == slt.L6_SERVDET_SERVNO);
            if (RowsTemplateConditionalWithServiceNoEnglish.Any())
            {
                TemplateConditionalWithServiceNoEnglishCopy = RowsTemplateConditionalWithServiceNoEnglish.CopyToDataTable<DataRow>();
            }
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
            LineNo = "32";
            if (dtSingleLTBill.Rows.Count >= 32)
                slt.L32_BarCode = dtSingleLTBill.Rows[31][0].ToString();
            #endregion

            #region Custom Fields
            var meter = slt.L12_MTRSNO_METER_2_IF_AVAILABLE.Trim() != "" ? slt.L12_MTRSNO_METER_2_IF_AVAILABLE : slt.L12_MTRSNO_METER1;
            slt.TopPanel_Row_1 = slt.L1_MonthYear + " / " + slt.L1_Zone + " / " + slt.L1_BU + " / " + slt.L1_Route + " / " + slt.L1_ReadingSequence + " / " + Convert.ToInt32(slt.L1_BillSequenceNo).ToString().PadLeft(5, '0');
            slt.TopPanel_Row_2 = "Meter No. " + meter;
            slt.TopPanel_Row_3 = "T No. " + slt.L8_TNo;
            slt.TopPanel_Row_4 = "Bill Date  " + slt.L7_BillDt;
            slt.TopPanel_Row_5 = "11 KV FEEDER :" + slt.L1_FeederName;


            slt.L10_TotArrUPPCLIntUPPCLIntArrUPPCL_Rounded = string.IsNullOrEmpty(dtSingleLTBill.Rows[9][2].ToString()) ? "0" : Math.Round(Convert.ToDecimal(dtSingleLTBill.Rows[9][2].ToString()) + (decimal).49, 0, MidpointRounding.AwayFromZero).ToString();

            slt.L8_AmountPayableBeforeDueDate_Rounded = string.IsNullOrEmpty(dtSingleLTBill.Rows[7][10].ToString()) ? "0" : ((Convert.ToDecimal(dtSingleLTBill.Rows[7][10].ToString())) > 0 ? Math.Round(Convert.ToDecimal(dtSingleLTBill.Rows[7][10].ToString()) + (decimal).49, 0, MidpointRounding.AwayFromZero).ToString() : Math.Round(Convert.ToDecimal(dtSingleLTBill.Rows[7][10].ToString()) + (decimal).49, 0, MidpointRounding.AwayFromZero).ToString());
            slt.L8_ParkingAmount_Rounded = string.IsNullOrEmpty(dtSingleLTBill.Rows[7][12].ToString()) ? "0" : Math.Round(Convert.ToDecimal(dtSingleLTBill.Rows[7][12].ToString()), 0, MidpointRounding.AwayFromZero).ToString();

            #endregion

            #endregion
            return slt;
        }

        PaperSourceCollection printerSources;

        //void NonTOD_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        //{

        //    e.PrintDocument.DefaultPageSettings.PaperSource = e.PrintDocument.PrinterSettings.PaperSources[cbNonTODTraySource.SelectedIndex];
        //    if (e.PrintDocument.PrinterSettings.CanDuplex)
        //        e.PrintDocument.PrinterSettings.Duplex = Duplex.Vertical;
        //    printerSources = e.PrintDocument.PrinterSettings.PaperSources;
        //}
        //void Seperator_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        //{
        //    e.PrintDocument.DefaultPageSettings.PaperSource = e.PrintDocument.PrinterSettings.PaperSources[cbSeparatorTraySource.SelectedIndex];
        //    if (e.PrintDocument.PrinterSettings.CanDuplex)
        //        e.PrintDocument.PrinterSettings.Duplex = Duplex.Vertical;
        //}

        //void CollectorReport_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        //{
        //    string A = e.PrintDocument.DocumentName;
        //    e.PrintDocument.DefaultPageSettings.PaperSource = e.PrintDocument.PrinterSettings.PaperSources[cbNonTODTraySource.SelectedIndex];
        //    if (e.PrintDocument.PrinterSettings.CanDuplex)
        //        e.PrintDocument.PrinterSettings.Duplex = Duplex.Default;
        //    printerSources = e.PrintDocument.PrinterSettings.PaperSources;
        //}
        //private void PrintDocument_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        //{ }
        //

        private void CR_PrintProgress(object sender, PrintProgressEventArgs e)
        {

            if (HavingSaperator && e.PageIndex < 2)
            {
                e.PageSettings.PaperSource = printerSources[cbSeparatorTraySource.SelectedIndex];
            }
            else
            {
                e.PageSettings.PaperSource = printerSources[cbNonTODTraySource.SelectedIndex];
            }
        }

        private void cbDefaultPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            AppFunctions.ShowWaitForm("Please wait we are searching for printer trays.!!");
            PrintDocument printDoc = new PrintDocument();
            cbNonTODTraySource.Properties.Items.Clear();
            cbSeparatorTraySource.Properties.Items.Clear();
            printDoc.PrinterSettings.PrinterName = cbDefaultPrinter.SelectedText;
            printerSources = printDoc.PrinterSettings.PaperSources;
            PaperSourceCollection ps = printDoc.PrinterSettings.PaperSources;
            for (int i = 0; i < ps.Count; i++)
            {
                PaperSource pkSource = printDoc.PrinterSettings.PaperSources[i];
                cbNonTODTraySource.Properties.Items.Add(ps[i].SourceName);
                cbSeparatorTraySource.Properties.Items.Add(ps[i].SourceName);

            }
            cbNonTODTraySource.SelectedIndex = 0;
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
                    XtraMessageBox.Show("Total Bill in this file " + singleLTBills.Length.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AppFunctions.ShowWaitForm("Generating Bill..!!");


                }
            }
        }

        SingleLTBill validateSingleLTBill(DataTable dtSingleLTBill)
        {


            SingleLTBill slt = new SingleLTBill();
            #region --Lines
            #region Line-1
            ServiceNo = dtSingleLTBill.Rows[5][0].ToString();
            //Line 1 Starts
            //LineNo = "1";
            //slt.L1_BillType = "LT";
            //slt.L1_MonthYear = dtSingleLTBill.Rows[0][0].ToString();
            //slt.L1_Zone = dtSingleLTBill.Rows[0][1].ToString();
            //slt.L1_BU = dtSingleLTBill.Rows[0][2].ToString();
            //slt.L1_PC = dtSingleLTBill.Rows[0][3].ToString();
            //slt.L1_Route = dtSingleLTBill.Rows[0][4].ToString();
            //slt.L1_BillSequenceNo = dtSingleLTBill.Rows[0][5].ToString();
            //slt.L1_ReadingSequence = dtSingleLTBill.Rows[0][6].ToString();
            //slt.L1_PowerFactorMSGIndicator = dtSingleLTBill.Rows[0][7].ToString();
            //slt.L1_FeederName = dtSingleLTBill.Rows[0][8].ToString();
            //slt.L1_TODOrNon_TODFlag = dtSingleLTBill.Rows[0][19].ToString();
            //slt.L1_AKY_indicator = dtSingleLTBill.Rows[0][10].ToString();
            //slt.L1_DisconnectionMSGPrintingIMMEDIATE = dtSingleLTBill.Rows[0][11].ToString();
            //slt.L1_BillingCode = dtSingleLTBill.Rows[0][12].ToString();
            //Line 1 End
            #endregion

            #region Line-2
            //Line 2 Starts
            //LineNo = "2";
            //slt.L2_Name = dtSingleLTBill.Rows[1][0].ToString().Trim('�');
            //Line 2 End
            #endregion

            #region Line-3
            //Line 3 Starts
            //LineNo = "3";
            //slt.L3_Addr1 = dtSingleLTBill.Rows[2][0].ToString().Trim('�');
            //Line 3 End
            #endregion

            #region Line-4
            //Line 4 Starts
            //LineNo = "4";
            //slt.L4_Addr2 = dtSingleLTBill.Rows[3][0].ToString().Trim('�');
            //Line 4 End
            #endregion

            #region Line-5
            //Line 5 Starts
            //LineNo = "5";
            //slt.L5_Addr3 = dtSingleLTBill.Rows[4][0].ToString().Trim('�');
            //Line 5 End
            #endregion

            #region Line-6
            //Line 6 Starts
            //LineNo = "6";
            //slt.L6_MeasureContractDemand = dtSingleLTBill.Rows[5][10].ToString();
            //slt.L6_SERVDET_SERVNO = dtSingleLTBill.Rows[5][0].ToString();
            //slt.L6_SERVDET_SANC_LOAD = string.IsNullOrEmpty(dtSingleLTBill.Rows[5][1].ToString()) ? "" : dtSingleLTBill.Rows[5][1].ToString();
            //slt.L6_ACTUAL_DEMAND = string.IsNullOrEmpty(dtSingleLTBill.Rows[5][2].ToString()) ? "" : dtSingleLTBill.Rows[5][2].ToString();
            //slt.L6_TARIFF_DESCR = string.IsNullOrEmpty(dtSingleLTBill.Rows[5][3].ToString()) ? "" : dtSingleLTBill.Rows[5][3].ToString();
            //slt.L6_EXCESS_DEMAND = string.IsNullOrEmpty(dtSingleLTBill.Rows[5][4].ToString()) ? "" : dtSingleLTBill.Rows[5][4].ToString();
            //slt.L6_SUPPLY_VOLTAGE = dtSingleLTBill.Rows[5][5].ToString();
            //slt.L6_MTRDET_LF_PERC = dtSingleLTBill.Rows[5][6].ToString();
            //slt.L6_BILL_TYPE = dtSingleLTBill.Rows[5][7].ToString();
            //slt.L6_Avg_Power_Factor = dtSingleLTBill.Rows[5][8].ToString();
            //slt.L6_bill_demand = string.IsNullOrEmpty(dtSingleLTBill.Rows[5][9].ToString()) ? "" : dtSingleLTBill.Rows[5][9].ToString();
            //slt.L6_Kvah_Indicator = dtSingleLTBill.Rows[5][11].ToString();
            //Line 6 End
            #endregion

            #region Line-7
            //Line 7 Starts
            //LineNo = "7";
            //slt.L7_Due_Date = dtSingleLTBill.Rows[6][0].ToString();
            //slt.L7_BillDt = dtSingleLTBill.Rows[6][1].ToString();
            //slt.L7_PrevReadDt = dtSingleLTBill.Rows[6][2].ToString();
            //slt.L7_ReaDt = dtSingleLTBill.Rows[6][3].ToString();
            //slt.L7_LastPymtDate = dtSingleLTBill.Rows[6][4].ToString();
            //slt.L7_LastPayementAmount = dtSingleLTBill.Rows[6][5].ToString();
            //slt.L7_LastPayementMode = dtSingleLTBill.Rows[6][6].ToString();
            //Line 7 End
            #endregion

            #region Line-8
            //Line 8 Starts
            LineNo = "8 Column 11 ";
            //slt.L8_FixedCharge = dtSingleLTBill.Rows[7][0].ToString();
            //slt.L8_EnergyCharge = dtSingleLTBill.Rows[7][1].ToString();
            //slt.L8_ACCharge = dtSingleLTBill.Rows[7][2].ToString();
            //slt.L8_GovTax = dtSingleLTBill.Rows[7][3].ToString();
            //slt.L8_MinCharge = dtSingleLTBill.Rows[7][4].ToString();
            //slt.L8_ServdetTotbBdtOthr = dtSingleLTBill.Rows[7][5].ToString();
            //slt.L8_PowerFactorAdj = dtSingleLTBill.Rows[7][6].ToString();
            //slt.L8_RegulatoryCharge_1 = dtSingleLTBill.Rows[7][7].ToString();
            //slt.L8_RegulatoryCharge_2 = dtSingleLTBill.Rows[7][8].ToString();
            //slt.L8_RebateIncurredCurrentMonth = dtSingleLTBill.Rows[7][9].ToString();
            slt.L8_AmountPayableBeforeDueDate = Convert.ToDecimal(dtSingleLTBill.Rows[7][10].ToString()).ToString();
            //slt.L8_TNo = dtSingleLTBill.Rows[7][11].ToString().Trim('�');
            //slt.L8_ParkingAmount = dtSingleLTBill.Rows[7][12].ToString();
            //Line 8 End
            #endregion

            #region Line-9
            //Line 9 Starts
            //LineNo = "9";
            //slt.L9_TotDbArr = dtSingleLTBill.Rows[8][0].ToString();
            //slt.L9_CurrBillAmt = dtSingleLTBill.Rows[8][1].ToString();
            //slt.L9_Int_Tpl = dtSingleLTBill.Rows[8][2].ToString();
            //slt.L9_ArrsTpl = dtSingleLTBill.Rows[8][3].ToString();
            //slt.L9_CurrBillAmtIntTplArrsTpl = dtSingleLTBill.Rows[8][4].ToString();
            //slt.L9_AmountPayable = dtSingleLTBill.Rows[8][5].ToString();
            //slt.L9_MessageIndication = dtSingleLTBill.Rows[8][6].ToString();
            //slt.L9_MessageFlag = dtSingleLTBill.Rows[8][7].ToString().Trim('�');
            //Line 9 End
            #endregion

            #region Line-10
            //Line 10 Starts
            LineNo = "10";
            //slt.L10_LFincentive = dtSingleLTBill.Rows[9][0].ToString();
            //slt.L10_DisconnDate = dtSingleLTBill.Rows[9][1].ToString();
            //slt.L10_TotArrUPPCLIntUPPCLIntArrUPPCL = dtSingleLTBill.Rows[9][2].ToString();
            //slt.L10_SecDeptBdt = dtSingleLTBill.Rows[9][3].ToString();
            //slt.L10_DmdChgPenalty = dtSingleLTBill.Rows[9][4].ToString();
            //slt.L10_UPPCL_ArrearAmount = dtSingleLTBill.Rows[9][5].ToString();
            //slt.L10_UPPCLIntOnArrearAmount = dtSingleLTBill.Rows[9][6].ToString();
            //slt.L10_TheftAmount = dtSingleLTBill.Rows[9][7].ToString();
            //slt.L10_Mode = dtSingleLTBill.Rows[9][8].ToString();
            //Line 10 End
            #endregion

            #region Line-11
            //Line 11 Starts
            LineNo = "11";
            //slt.L11_MonYear_1 = dtSingleLTBill.Rows[10][0].ToString();
            //slt.L11_KWH_UNITS_1 = dtSingleLTBill.Rows[10][1].ToString();
            //slt.L11_MonYear_2 = dtSingleLTBill.Rows[10][2].ToString();
            //slt.L11_KWH_UNITS_2 = dtSingleLTBill.Rows[10][3].ToString();
            //slt.L11_MonYear_3 = dtSingleLTBill.Rows[10][4].ToString();
            //slt.L11_KWH_UNITS_3 = dtSingleLTBill.Rows[10][5].ToString();
            //slt.L11_MonYear_4 = dtSingleLTBill.Rows[10][6].ToString();
            //slt.L11_KWH_UNITS_4 = dtSingleLTBill.Rows[10][7].ToString();
            //slt.L11_MonYear_5 = dtSingleLTBill.Rows[10][8].ToString();
            //slt.L11_KWH_UNITS_5 = dtSingleLTBill.Rows[10][9].ToString();
            //slt.L11_MonYear_6 = dtSingleLTBill.Rows[10][10].ToString();
            //slt.L11_KWH_UNITS_6 = dtSingleLTBill.Rows[10][11].ToString();
            //slt.L11_MonYear_7 = dtSingleLTBill.Rows[10][12].ToString();
            //slt.L11_KWH_UNITS_7 = dtSingleLTBill.Rows[10][13].ToString();
            //slt.L11_MonYear_8 = dtSingleLTBill.Rows[10][14].ToString();
            //slt.L11_KWH_UNITS_8 = dtSingleLTBill.Rows[10][15].ToString();
            //slt.L11_MonYear_9 = dtSingleLTBill.Rows[10][16].ToString();
            //slt.L11_KWH_UNITS_9 = dtSingleLTBill.Rows[10][17].ToString();
            //slt.L11_MonYear_10 = dtSingleLTBill.Rows[10][18].ToString();
            //slt.L11_KWH_UNITS_10 = dtSingleLTBill.Rows[10][19].ToString();
            //slt.L11_MonYear_11 = dtSingleLTBill.Rows[10][20].ToString();
            //slt.L11_KWH_UNITS_11 = dtSingleLTBill.Rows[10][21].ToString();
            //slt.L11_MonYear_12 = dtSingleLTBill.Rows[10][22].ToString();
            //slt.L11_KWH_UNITS_12 = dtSingleLTBill.Rows[10][23].ToString();
            //slt.L11_MonYear_13 = dtSingleLTBill.Rows[10][24].ToString();
            //slt.L11_KWH_UNITS_13 = dtSingleLTBill.Rows[10][25].ToString();

            DataTable chrtData = new DataTable();
            chrtData.Columns.Add("MonthYear");
            chrtData.Columns.Add("Value", typeof(Int32));
            for (int i = 0; i <= 25; i += 2)
            {
                LineNo = "11 column " + (i + 1) + " ";

                var crg = chrtData.NewRow();

                if (MonthYear != (string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i])))
                {
                    crg["MonthYear"] = string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i]);
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
            //LineNo = "13";
            //slt.L13_M1_KWH_PRESREAD = dtSingleLTBill.Rows[12][0].ToString();
            //slt.L13_M1_KVA_PRESREAD = dtSingleLTBill.Rows[12][1].ToString();
            //Line 13 End
            #endregion

            #region Line-14
            //Line 14 Starts
            //LineNo = "14";
            //slt.L14_M1_KWH_PASTREAD = dtSingleLTBill.Rows[13][0].ToString();
            //slt.L14_M1_KVA_PASTREAD = dtSingleLTBill.Rows[13][1].ToString();
            //Line 14 End
            #endregion

            #region Line-15
            //Line 15 Starts
            //LineNo = "15";
            //slt.L15_M1_MultiplyingFactor_1 = dtSingleLTBill.Rows[14][0].ToString();
            //slt.L15_M1_MultiplyingFactor_2 = dtSingleLTBill.Rows[14][1].ToString();
            //slt.L15_Purpose = dtSingleLTBill.Rows[14][2].ToString();

            //Line 15 End
            #endregion

            #region Line-16
            //Line 16 Starts
            //LineNo = "16";
            //slt.L16_M1_KWH_UNITS = dtSingleLTBill.Rows[15][0].ToString();
            //slt.L16_M1_KVA_UNITS = dtSingleLTBill.Rows[15][1].ToString();
            //Line 16 End
            #endregion

            #region Line-17
            //Line 17 Starts
            //LineNo = "17";
            //slt.L17_M2_KWH_PRESREAD = dtSingleLTBill.Rows[16][0].ToString();
            //slt.L17_M2_KVA_PRESREAD = dtSingleLTBill.Rows[16][1].ToString();
            //Line 17 End
            #endregion

            #region Line-18
            //Line 18 Starts
            //LineNo = "18";
            //slt.L18_M2_KWH_PASTREAD = dtSingleLTBill.Rows[17][0].ToString();
            //slt.L18_M2_KVA_PASTREAD = dtSingleLTBill.Rows[17][1].ToString();
            //Line 18 End
            #endregion

            #region Line-19
            //Line 19 Starts
            //LineNo = "19";
            //slt.L19_M2_Multiplying_Factor_1 = dtSingleLTBill.Rows[18][0].ToString();
            //slt.L19_M2_Multiplying_Factor_2 = dtSingleLTBill.Rows[18][1].ToString();
            //Line 19 End
            #endregion

            #region Line-20
            //Line 20 Starts
            //LineNo = "20";
            //slt.L20_M2_KWH_UNITS = dtSingleLTBill.Rows[19][0].ToString();
            //slt.L20_M2_KVA_UNITS = dtSingleLTBill.Rows[19][1].ToString();
            //Line 20 End
            #endregion

            #region Line-21
            //Line 21 Starts
            LineNo = "21";
            //slt.L21_MonYear_1 = dtSingleLTBill.Rows[20][0].ToString();
            //slt.L21_KVA_UNITS_1 = dtSingleLTBill.Rows[20][1].ToString();

            //slt.L21_MonYear_2 = dtSingleLTBill.Rows[20][2].ToString();
            //slt.L21_KVA_UNITS_2 = dtSingleLTBill.Rows[20][3].ToString();

            //slt.L21_MonYear_3 = dtSingleLTBill.Rows[20][4].ToString();
            //slt.L21_KVA_UNITS_3 = dtSingleLTBill.Rows[20][5].ToString();

            //slt.L21_MonYear_4 = dtSingleLTBill.Rows[20][6].ToString();
            //slt.L21_KVA_UNITS_4 = dtSingleLTBill.Rows[20][7].ToString();

            //slt.L21_MonYear_5 = dtSingleLTBill.Rows[20][8].ToString();
            //slt.L21_KVA_UNITS_5 = dtSingleLTBill.Rows[20][9].ToString();

            //slt.L21_MonYear_6 = dtSingleLTBill.Rows[20][10].ToString();
            //slt.L21_KVA_UNITS_6 = dtSingleLTBill.Rows[20][11].ToString();

            //slt.L21_MonYear_7 = dtSingleLTBill.Rows[20][12].ToString();
            //slt.L21_KVA_UNITS_7 = dtSingleLTBill.Rows[20][13].ToString();

            //slt.L21_MonYear_8 = dtSingleLTBill.Rows[20][14].ToString();
            //slt.L21_KVA_UNITS_8 = dtSingleLTBill.Rows[20][15].ToString();

            //slt.L21_MonYear_9 = dtSingleLTBill.Rows[20][16].ToString();
            //slt.L21_KVA_UNITS_9 = dtSingleLTBill.Rows[20][17].ToString();

            //slt.L21_MonYear_10 = dtSingleLTBill.Rows[20][18].ToString();
            //slt.L21_KVA_UNITS_10 = dtSingleLTBill.Rows[20][19].ToString();

            //slt.L21_MonYear_11 = dtSingleLTBill.Rows[20][20].ToString();
            //slt.L21_KVA_UNITS_11 = dtSingleLTBill.Rows[20][21].ToString();

            //slt.L21_MonYear_12 = dtSingleLTBill.Rows[20][22].ToString();
            //slt.L21_KVA_UNITS_12 = dtSingleLTBill.Rows[20][23].ToString();

            //slt.L21_MonYear_13 = dtSingleLTBill.Rows[20][24].ToString();
            //slt.L21_KVA_UNITS_13 = dtSingleLTBill.Rows[20][25].ToString();


            DataTable KVAchrtData = new DataTable();
            KVAchrtData.Columns.Add("MonthYear");
            KVAchrtData.Columns.Add("Value", typeof(decimal));

            for (int i = 0; i <= 25; i += 2)
            {
                LineNo = "21 column " + (i + 1) + " ";

                //KVAchrtData.Rows.Add(new object[] { string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i]), string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i + 1])) ? 0 : Convert.ToDecimal(dtSingleLTBill.Rows[20][i + 1]) });
                if (MonthYear != (string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i])))
                {
                    KVAchrtData.Rows.Add(new object[] { string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleLTBill.Rows[20][i]), string.IsNullOrEmpty(Convert.ToString(dtSingleLTBill.Rows[20][i + 1])) ? 0 : Convert.ToDecimal(dtSingleLTBill.Rows[20][i + 1]) });
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

            #region Line-22
            //Line 22 Starts
            LineNo = "22 column 1";
            slt.L22_MESSAGE1 = dtSingleLTBill.Rows[21][0].ToString();
            //Line 22 End
            #endregion

            #region Line-23
            //Line 23 Starts
            LineNo = "23 column 1";
            if (dtSingleLTBill.Rows.Count >= 23)
                slt.L23_MESSAGE2 = dtSingleLTBill.Rows[22][0].ToString();
            //Line 23 End
            #endregion

            #region Line-24
            //Line 24 Starts
            LineNo = "24 column 1";
            if (dtSingleLTBill.Rows.Count >= 24)
                slt.L24_MESSAGE3 = dtSingleLTBill.Rows[23][0].ToString();
            //Line 24 End
            #endregion

            #region Line-25
            //Line 25 Starts
            LineNo = "25 column 1";
            if (dtSingleLTBill.Rows.Count >= 25)
                slt.L25_MESSAGE4 = dtSingleLTBill.Rows[24][0].ToString();
            //Line 25 End
            #endregion

            #region Line-26
            //Line 26 Starts
            LineNo = "26 column 1";
            if (dtSingleLTBill.Rows.Count >= 26)
                slt.L26_MESSAGE5 = dtSingleLTBill.Rows[25][0].ToString();
            //Line 26 End
            #endregion

            #region Line-27
            //Line 27 Starts
            LineNo = "27 column 1";
            if (dtSingleLTBill.Rows.Count >= 27)
                slt.L27_MESSAGE6 = dtSingleLTBill.Rows[26][0].ToString();
            //Line 27 End
            #endregion

            #region Line-28
            //Line 28 Starts
            LineNo = "28 column 1";
            if (dtSingleLTBill.Rows.Count >= 28)
                slt.L28_MESSAGE7 = dtSingleLTBill.Rows[27][0].ToString();
            //Line 28 End
            #endregion

            #region Line-29
            //Line 29 Starts
            LineNo = "29 column 1";
            if (dtSingleLTBill.Rows.Count >= 29)
                slt.L29_MESSAGE8 = dtSingleLTBill.Rows[28][0].ToString();
            //Line 29 End
            #endregion

            #region Line-30
            //Line 30 Starts
            LineNo = "30 column 1";
            if (dtSingleLTBill.Rows.Count >= 30)
                slt.L30_MESSAGE9 = dtSingleLTBill.Rows[29][0].ToString();
            //Line 30 End
            #endregion

            #region Line-31
            //Line 31 Starts
            LineNo = "31 column 1";
            if (dtSingleLTBill.Rows.Count >= 31)
                slt.L31_MESSAGE10 = dtSingleLTBill.Rows[30][0].ToString();
            //Line 31 End
            #endregion


            LineNo = "6";
            #region TemplateConditionalWithSTHindi
            //DataView DVTemplateConditionalWithSTHindi = new DataView();
            //DVTemplateConditionalWithSTHindi = TemplateConditionalWithSTHindi.DefaultView;
            //DVTemplateConditionalWithSTHindi.RowFilter = "[1] = '" + slt.L6_TARIFF_DESCR + "'";
            DataTable TemplateConditionalWithSTHindiCopy = new DataTable();
            var RowsTemplateConditionalWithSTHindi = TemplateConditionalWithSTHindi.AsEnumerable().Where(row => row.Field<string>("1") == slt.L6_TARIFF_DESCR);
            if (RowsTemplateConditionalWithSTHindi.Any())
            {
                TemplateConditionalWithSTHindiCopy = RowsTemplateConditionalWithSTHindi.CopyToDataTable<DataRow>();
            }
            //TemplateConditionalWithSTHindiCopy = DVTemplateConditionalWithSTHindi.ToTable();
            for (int i = 0; i < TemplateConditionalWithSTHindiCopy.Rows.Count; i++)
            {
                if (slt.L6_MeasureContractDemand.ToUpper() == "KW" && Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD) >= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
                {
                    slt.L33_MESSAGE7 += TemplateConditionalWithSTHindiCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (slt.L6_MeasureContractDemand.ToUpper() == "KVA" && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)) * (0.9)) >= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)) * (0.9)) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
                {
                    slt.L33_MESSAGE7 += TemplateConditionalWithSTHindiCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (slt.L6_MeasureContractDemand.ToUpper() == "HP" && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)) * (0.746)) >= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)) * (0.746)) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
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
            //DataView DVTemplateConditionalWithSTEnglish = new DataView();
            //DVTemplateConditionalWithSTEnglish = TemplateConditionalWithSTEnglish.DefaultView;
            //DVTemplateConditionalWithSTEnglish.RowFilter = "[1] = '" + slt.L6_TARIFF_DESCR + "'";
            DataTable TemplateConditionalWithSTEnglishCopy = new DataTable();
            var RowsTemplateConditionalWithSTEnglish = TemplateConditionalWithSTEnglish.AsEnumerable().Where(row => row.Field<string>("1") == slt.L6_TARIFF_DESCR);
            if (RowsTemplateConditionalWithSTEnglish.Any())
            {
                TemplateConditionalWithSTEnglishCopy = RowsTemplateConditionalWithSTEnglish.CopyToDataTable<DataRow>();
            }
            //DataTable TemplateConditionalWithSTEnglishCopy = DVTemplateConditionalWithSTEnglish.ToTable();

            for (int i = 0; i < TemplateConditionalWithSTEnglishCopy.Rows.Count; i++)
            {
                if (slt.L6_MeasureContractDemand.ToUpper() == "KW" && Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
                {
                    slt.L34_MESSAGE8 += TemplateConditionalWithSTEnglishCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (slt.L6_MeasureContractDemand.ToUpper() == "KVA" && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)) * (0.9)) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)) * (0.9)) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
                {
                    slt.L34_MESSAGE8 += TemplateConditionalWithSTEnglishCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (slt.L6_MeasureContractDemand.ToUpper() == "HP" && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)) * (0.746)) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(slt.L6_SERVDET_SANC_LOAD)) * (0.746)) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
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
            //DataView DVTemplateConditionalWithServiceNoHindi = new DataView();
            //DVTemplateConditionalWithServiceNoHindi = TemplateConditionalWithServiceNoHindi.DefaultView;
            //DVTemplateConditionalWithServiceNoHindi.RowFilter = "[1] = '" + slt.L6_SERVDET_SERVNO + "'";
            //DataTable TemplateConditionalWithServiceNoHindiCopy = new DataTable();
            //var RowsTemplateConditionalWithServiceNoHindi = TemplateConditionalWithServiceNoHindi.AsEnumerable().Where(row => row.Field<string>("1") == slt.L6_SERVDET_SERVNO);
            //if (RowsTemplateConditionalWithServiceNoHindi.Any())
            //{
            //    TemplateConditionalWithServiceNoHindiCopy = RowsTemplateConditionalWithServiceNoHindi.CopyToDataTable<DataRow>();
            //}
            ////DataTable TemplateConditionalWithServiceNoHindiCopy = DVTemplateConditionalWithServiceNoHindi.ToTable();
            //for (int i = 0; i < TemplateConditionalWithServiceNoHindiCopy.Rows.Count; i++)
            //{
            //    slt.L35_MESSAGE9 += TemplateConditionalWithServiceNoHindiCopy.Rows[i]["2"].ToString().Trim('�') + " \r\n";
            //}
            //if (!string.IsNullOrEmpty(slt.L35_MESSAGE9))
            //{
            //    slt.L35_MESSAGE9 = slt.L35_MESSAGE9.Replace('�', ' ');
            //    slt.L35_MESSAGE9 = slt.L35_MESSAGE9.TrimEnd(' ');
            //    slt.L35_MESSAGE9 = slt.L35_MESSAGE9.TrimEnd('\r');
            //    slt.L35_MESSAGE9 = slt.L35_MESSAGE9.TrimEnd('\n');
            //}
            #endregion

            #region TemplateConditionalWithServiceNoEnglish
            //DataTable TemplateConditionalWithServiceNoEnglishCopy = new DataTable();
            //var RowsTemplateConditionalWithServiceNoEnglish = TemplateConditionalWithServiceNoEnglish.AsEnumerable().Where(row => row.Field<string>("1") == slt.L6_SERVDET_SERVNO);
            //if (RowsTemplateConditionalWithServiceNoEnglish.Any())
            //{
            //    TemplateConditionalWithServiceNoEnglishCopy = RowsTemplateConditionalWithServiceNoEnglish.CopyToDataTable<DataRow>();
            //}
            //for (int i = 0; i < TemplateConditionalWithServiceNoEnglishCopy.Rows.Count; i++)
            //{
            //    slt.L36_MESSAGE10 += TemplateConditionalWithServiceNoEnglishCopy.Rows[i]["2"].ToString().Trim('�') + " \r\n";
            //}
            //if (!string.IsNullOrEmpty(slt.L36_MESSAGE10))
            //{
            //    slt.L36_MESSAGE10 = slt.L36_MESSAGE10.TrimEnd('\n');
            //    slt.L36_MESSAGE10 = slt.L36_MESSAGE10.TrimEnd('\r');
            //    slt.L36_MESSAGE10 = slt.L36_MESSAGE10.TrimEnd(' ');
            //}
            #endregion


            #region Line-32
            LineNo = "32";
            if (dtSingleLTBill.Rows.Count >= 32)
                slt.L32_BarCode = dtSingleLTBill.Rows[31][0].ToString();
            #endregion

            #region Custom Fields
            var meter = slt.L12_MTRSNO_METER_2_IF_AVAILABLE.Trim() != "" ? slt.L12_MTRSNO_METER_2_IF_AVAILABLE : slt.L12_MTRSNO_METER1;
            slt.TopPanel_Row_1 = slt.L1_MonthYear + " / " + slt.L1_Zone + " / " + slt.L1_BU + " / " + slt.L1_Route + " / " + slt.L1_ReadingSequence + " / " + Convert.ToInt32(slt.L1_BillSequenceNo).ToString().PadLeft(5, '0');
            slt.TopPanel_Row_2 = "Meter No. " + meter;
            slt.TopPanel_Row_3 = "T No. " + slt.L8_TNo;
            slt.TopPanel_Row_4 = "Bill Date  " + slt.L7_BillDt;
            slt.TopPanel_Row_5 = "11 KV FEEDER :" + slt.L1_FeederName;

            LineNo = "10";
            slt.L10_TotArrUPPCLIntUPPCLIntArrUPPCL_Rounded = string.IsNullOrEmpty(dtSingleLTBill.Rows[9][2].ToString()) ? "0" : Math.Round(Convert.ToDecimal(dtSingleLTBill.Rows[9][2].ToString()) + (decimal).49, 0, MidpointRounding.AwayFromZero).ToString();
            LineNo = "8";
            slt.L8_AmountPayableBeforeDueDate_Rounded = string.IsNullOrEmpty(dtSingleLTBill.Rows[7][10].ToString()) ? "0" : ((Convert.ToDecimal(dtSingleLTBill.Rows[7][10].ToString())) > 0 ? Math.Round(Convert.ToDecimal(dtSingleLTBill.Rows[7][10].ToString()) + (decimal).49, 0, MidpointRounding.AwayFromZero).ToString() : Math.Round(Convert.ToDecimal(dtSingleLTBill.Rows[7][10].ToString()) + (decimal).49, 0, MidpointRounding.AwayFromZero).ToString());
            slt.L8_ParkingAmount_Rounded = string.IsNullOrEmpty(dtSingleLTBill.Rows[7][12].ToString()) ? "0" : Math.Round(Convert.ToDecimal(dtSingleLTBill.Rows[7][12].ToString()), 0, MidpointRounding.AwayFromZero).ToString();

            #endregion

            #endregion
            return slt;
        }

        private bool ValidatetxtFile(string[] bills)
        {
            SingleLTBill slt;
            try
            {
                DSBill.Dispose();
                DSBill.Reset();
                int BillNo = 0;
                //DataTable dtSingleLTBill;
                foreach (var bill in bills)
                {
                    BillNo++;
                    DSBill.Tables.Add(ParseAsDataTable.LT_FileTxtToDataTable(bill, BillNo, "LT"));
                    //dtSingleLTBill = ParseAsDataTable.LT_FileTxtToDataTable(bill, BillNo, "LT");
                    if (DSBill.Tables[BillNo - 1].Rows.Count == 36)
                    //if(dtSingleLTBill.Rows.Count == 36)
                    {
                        slt = validateSingleLTBill(DSBill.Tables[BillNo - 1]);
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

        private void sbSavePDF_Click(object sender, EventArgs e)
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
                    if (String.Equals(this.Name, "PrintLT") && contents.StartsWith("LT|"))
                    {
                        singleLTBills = contents.Split(new String[] { "LT|" }, StringSplitOptions.RemoveEmptyEntries);
                        if (!select_mVImg())
                        {
                            AppFunctions.CloseWaitForm();
                            return;
                        }

                        XtraMessageBox.Show("Total Bill in this file " + singleLTBills.Length.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AppFunctions.ShowWaitForm("Validating LT Bills Now before I generate the PDF files !!");
                        var sb = sender as SimpleButton;
                        if (ValidatetxtFile(singleLTBills))
                        {
                            int i = -1;
                            while (i < singleLTBills.Count())
                            {
                                GeneratePDFFormatsForLTBillsNewOptimized(i + 1);
                                i += 50;
                            }
                            DSBill.Reset();
                            DSBill.Dispose();
                            AppFunctions.CloseWaitForm();

                            XtraMessageBox.Show(singleLTBills.Count() + " bills has been parsed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            AppFunctions.CloseWaitForm();
                            return;
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("It seeems that you have chosen a wrong file,\n Try again and Pick correct File!!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
        }


        void GeneratePDFFormatsForLTBillsNewOptimized(int startingBillNumber)
        {

            XtraReport collectorReport = new XtraReport
            {
                DisplayName = "LT Print",
            };


            List<int> inlist = Enumerable.Range(startingBillNumber, 50).ToList();
            Parallel.ForEach(inlist, z =>
            {
                if (z < DSBill.Tables.Count)
                {
                    DataTable dtSingleLTBill = DSBill.Tables[z];
                    if (dtSingleLTBill.Rows.Count != 0)
                    {
                        try
                        {
                            List<SingleLTBill> lstformattedbills = new List<SingleLTBill>();

                            SingleLTBill slt = parseSingleLTBill(dtSingleLTBill);

                            //iTextSharpGeneratePDF(slt); //28 December 2021



                            slt.MVPicture = mVImagePath;
                            lstformattedbills.Add(slt);

                            using (AT.Print.PDF.Rpt_LTPDF rptsd = new AT.Print.PDF.Rpt_LTPDF
                            {
                                DataSource = lstformattedbills
                            })
                            {
                                #region WaterMark Picture Front Page PDF Non-TOD
                                DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkFrontNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
                                pictureWatermarkFrontNonTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Non_TOD_Front_Page.png");
                                pictureWatermarkFrontNonTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                                pictureWatermarkFrontNonTOD.ImageTiling = false;
                                pictureWatermarkFrontNonTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
                                pictureWatermarkFrontNonTOD.ImageTransparency = 0;
                                pictureWatermarkFrontNonTOD.ShowBehind = true;
                                rptsd.Watermark.CopyFrom(pictureWatermarkFrontNonTOD);
                                #endregion

                                rptsd.CreateDocument(false);

                                using (AT.Print.PDF.rpt_LT_Back rpts = new AT.Print.PDF.rpt_LT_Back
                                {
                                    DataSource = lstformattedbills
                                })
                                {
                                    #region WaterMark Picture Back Page PDF Non-TOD
                                    DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkBackNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
                                    pictureWatermarkBackNonTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Non_TOD_Back_Page.png");
                                    pictureWatermarkBackNonTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                                    pictureWatermarkBackNonTOD.ImageTiling = false;
                                    pictureWatermarkBackNonTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
                                    pictureWatermarkBackNonTOD.ImageTransparency = 0;
                                    pictureWatermarkBackNonTOD.ShowBehind = true;
                                    rpts.Watermark.CopyFrom(pictureWatermarkBackNonTOD);
                                    #endregion


                                    rpts.CreateDocument(false);

                                    rptsd.ModifyDocument(x => { x.AddPages(rpts.Pages); });
                                    DevExpress.XtraPrinting.Page myPage2 = rptsd.Pages[1];
                                    myPage2.AssignWatermark(pictureWatermarkBackNonTOD);
                                    string billdate = lstformattedbills.FirstOrDefault().L1_MonthYear;
                                    string ServiceNo = lstformattedbills.FirstOrDefault().L6_SERVDET_SERVNO;
                                    var outputfolder = "C://Bills//LT Files//" + billdate + "//" + textFileName;
                                    OutputFolderPath OFP = new OutputFolderPath();
                                    outputfolder = OFP.LoadLocation() + "//LT Files//" + billdate + "//" + textFileName;
                                    if (!Directory.Exists(outputfolder))
                                        Directory.CreateDirectory(outputfolder);
                                    //var OutPutFolder = 
                                    if (Directory.Exists(outputfolder))
                                    {
                                        rptsd.ExportToPdf(outputfolder + "//" + ServiceNo + ".pdf");
                                    }
                                }
                            }

                        }
                        catch (System.OutOfMemoryException)
                        {
                            System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                            GC.Collect();
                            GC.RemoveMemoryPressure(1024 * 1024);
                        }
                        catch (Exception ex)
                        {
                            AppFunctions.LogError(ex);
                            AppFunctions.CloseWaitForm();
                            XtraMessageBox.Show("Error Parsing Bill Service No. " + ServiceNo + " of the given file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            });

        }


        void GeneratePDFFormatsForLTBills(string[] Bills, string Name, int Initial, int Final, string FolderName)
        {
            int ParsedBills = 0;

            XtraReport collectorReport = new XtraReport
            {
                DisplayName = "LT Print",
            };

            Final = 1000;
            if (Final > 200)
            {
                List<int> inlist = Enumerable.Range(0, Final / 2).ToList();
                List<int> inlist1 = Enumerable.Range(Final / 2 + 1, Final / 2 + 1).ToList();

                Parallel.ForEach(inlist, z =>
                {
                    if (z < DSBill.Tables.Count)
                    {
                        DataTable dtSingleLTBill = DSBill.Tables[z];
                        if (dtSingleLTBill.Rows.Count != 0)
                        {
                            try
                            {
                                List<SingleLTBill> lstformattedbills = new List<SingleLTBill>();

                                SingleLTBill slt = parseSingleLTBill(dtSingleLTBill);
                                slt.MVPicture = mVImagePath;
                                lstformattedbills.Add(slt);

                                using (AT.Print.PDF.Rpt_LTPDF rptsd = new AT.Print.PDF.Rpt_LTPDF
                                {
                                    DataSource = lstformattedbills,
                                    // ShowPrintStatusDialog = false,
                                    //ShowPreviewMarginLines = false

                                })
                                {
                                    #region WaterMark Picture Front Page PDF Non-TOD
                                    DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkFrontNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
                                    pictureWatermarkFrontNonTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Non_TOD_Front_Page.png");
                                    pictureWatermarkFrontNonTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                                    pictureWatermarkFrontNonTOD.ImageTiling = false;
                                    pictureWatermarkFrontNonTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
                                    pictureWatermarkFrontNonTOD.ImageTransparency = 0;
                                    pictureWatermarkFrontNonTOD.ShowBehind = true;
                                    rptsd.Watermark.CopyFrom(pictureWatermarkFrontNonTOD);
                                    #endregion



                                    //pictureWatermark.PageRange = "2,4";



                                    rptsd.CreateDocument(false);

                                    using (AT.Print.PDF.rpt_LT_Back rpts = new AT.Print.PDF.rpt_LT_Back
                                    {
                                        DataSource = lstformattedbills,
                                        //ShowPrintStatusDialog = false,
                                        // ShowPreviewMarginLines = false

                                    })
                                    {
                                        #region WaterMark Picture Back Page PDF Non-TOD
                                        DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkBackNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
                                        pictureWatermarkBackNonTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Non_TOD_Back_Page.png");
                                        pictureWatermarkBackNonTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                                        pictureWatermarkBackNonTOD.ImageTiling = false;
                                        pictureWatermarkBackNonTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
                                        pictureWatermarkBackNonTOD.ImageTransparency = 0;
                                        pictureWatermarkBackNonTOD.ShowBehind = true;
                                        //pictureWatermark.PageRange = "2,4";
                                        rpts.Watermark.CopyFrom(pictureWatermarkBackNonTOD);
                                        #endregion


                                        rpts.CreateDocument(false);

                                        rptsd.ModifyDocument(x => { x.AddPages(rpts.Pages); });
                                        DevExpress.XtraPrinting.Page myPage2 = rptsd.Pages[1];
                                        myPage2.AssignWatermark(pictureWatermarkBackNonTOD);
                                        string billdate = lstformattedbills.FirstOrDefault().L1_MonthYear;
                                        string ServiceNo = lstformattedbills.FirstOrDefault().L6_SERVDET_SERVNO;
                                        var outputfolder = "C://Bills//LT Files//" + billdate + "//" + textFileName;
                                        OutputFolderPath OFP = new OutputFolderPath();
                                        outputfolder = OFP.LoadLocation() + "//LT Files//" + billdate + "//" + textFileName;
                                        if (!Directory.Exists(outputfolder))
                                            Directory.CreateDirectory(outputfolder);
                                        //var OutPutFolder = 
                                        if (Directory.Exists(outputfolder))
                                        {
                                            rptsd.ExportToPdf(outputfolder + "//" + ServiceNo + ".pdf");
                                        }
                                    }


                                    ParsedBills++;
                                    //if (ParsedBills % 1500 == 0)
                                    //{
                                    //    System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                                    //    GC.Collect();
                                    //}
                                }

                            }
                            catch (System.OutOfMemoryException)
                            {
                                System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                                GC.Collect();
                                GC.RemoveMemoryPressure(1024 * 1024);
                            }
                            catch (Exception ex)
                            {
                                AppFunctions.LogError(ex);
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show("Error Parsing Bill " + (ParsedBills + 1) + " of the given file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                });

                Parallel.ForEach(inlist1, z =>
                {
                    if (z < DSBill.Tables.Count)
                    {
                        DataTable dtSingleLTBill = DSBill.Tables[z];
                        if (dtSingleLTBill.Rows.Count != 0)
                        {
                            try
                            {
                                List<SingleLTBill> lstformattedbills = new List<SingleLTBill>();

                                SingleLTBill slt = parseSingleLTBill(dtSingleLTBill);
                                slt.MVPicture = mVImagePath;
                                lstformattedbills.Add(slt);

                                using (AT.Print.PDF.Rpt_LTPDF rptsd = new AT.Print.PDF.Rpt_LTPDF
                                {
                                    DataSource = lstformattedbills,
                                    // ShowPrintStatusDialog = false,
                                    //ShowPreviewMarginLines = false

                                })
                                {
                                    #region WaterMark Picture Front Page PDF Non-TOD
                                    DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkFrontNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
                                    pictureWatermarkFrontNonTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Non_TOD_Front_Page.png");
                                    pictureWatermarkFrontNonTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                                    pictureWatermarkFrontNonTOD.ImageTiling = false;
                                    pictureWatermarkFrontNonTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
                                    pictureWatermarkFrontNonTOD.ImageTransparency = 0;
                                    pictureWatermarkFrontNonTOD.ShowBehind = true;
                                    rptsd.Watermark.CopyFrom(pictureWatermarkFrontNonTOD);
                                    #endregion



                                    //pictureWatermark.PageRange = "2,4";



                                    rptsd.CreateDocument(false);

                                    using (AT.Print.PDF.rpt_LT_Back rpts = new AT.Print.PDF.rpt_LT_Back
                                    {
                                        DataSource = lstformattedbills,
                                        //ShowPrintStatusDialog = false,
                                        // ShowPreviewMarginLines = false

                                    })
                                    {
                                        #region WaterMark Picture Back Page PDF Non-TOD
                                        DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkBackNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
                                        pictureWatermarkBackNonTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Non_TOD_Back_Page.png");
                                        pictureWatermarkBackNonTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                                        pictureWatermarkBackNonTOD.ImageTiling = false;
                                        pictureWatermarkBackNonTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
                                        pictureWatermarkBackNonTOD.ImageTransparency = 0;
                                        pictureWatermarkBackNonTOD.ShowBehind = true;
                                        //pictureWatermark.PageRange = "2,4";
                                        rpts.Watermark.CopyFrom(pictureWatermarkBackNonTOD);
                                        #endregion


                                        rpts.CreateDocument(false);

                                        rptsd.ModifyDocument(x => { x.AddPages(rpts.Pages); });
                                        DevExpress.XtraPrinting.Page myPage2 = rptsd.Pages[1];
                                        myPage2.AssignWatermark(pictureWatermarkBackNonTOD);
                                        string billdate = lstformattedbills.FirstOrDefault().L1_MonthYear;
                                        string ServiceNo = lstformattedbills.FirstOrDefault().L6_SERVDET_SERVNO;
                                        var outputfolder = "C://Bills//LT Files//" + billdate + "//" + textFileName;
                                        OutputFolderPath OFP = new OutputFolderPath();
                                        outputfolder = OFP.LoadLocation() + "//LT Files//" + billdate + "//" + textFileName;
                                        if (!Directory.Exists(outputfolder))
                                            Directory.CreateDirectory(outputfolder);
                                        //var OutPutFolder = 
                                        if (Directory.Exists(outputfolder))
                                        {
                                            rptsd.ExportToPdf(outputfolder + "//" + ServiceNo + ".pdf");
                                        }
                                    }
                                    ParsedBills++;
                                    //if (ParsedBills % 100 == 0)
                                    //{
                                    //    System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                                    //    GC.Collect();
                                    //}
                                }
                                //long mem = GC.GetTotalMemory(true);
                            }
                            catch (System.OutOfMemoryException)
                            {
                                System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                                GC.Collect();
                                GC.RemoveMemoryPressure(1024 * 1024);
                            }
                            catch (Exception ex)
                            {
                                AppFunctions.LogError(ex);
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show("Error Parsing Bill " + (ParsedBills + 1) + " of the given file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                });


                DSBill.Reset();
                DSBill.Dispose();
                AppFunctions.CloseWaitForm();
                XtraMessageBox.Show(ParsedBills + " Bills Parsed Successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                List<int> inlist = Enumerable.Range(0, Final).ToList();

                Parallel.ForEach(inlist, z =>
                //inlist.ForEach(z =>
                {

                    DataTable dtSingleLTBill = DSBill.Tables[z];
                    try
                    {
                        List<SingleLTBill> lstformattedbills = new List<SingleLTBill>();

                        SingleLTBill slt = parseSingleLTBill(dtSingleLTBill);
                        slt.MVPicture = mVImagePath;
                        lstformattedbills.Add(slt);

                        AT.Print.PDF.Rpt_LTPDF rptsd = new AT.Print.PDF.Rpt_LTPDF
                        {
                            DataSource = lstformattedbills,
                            // ShowPrintStatusDialog = false,
                            //ShowPreviewMarginLines = false

                        };
                        #region WaterMark Picture Front Page PDF Non-TOD
                        DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkFrontNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
                        pictureWatermarkFrontNonTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Non_TOD_Front_Page.png");
                        pictureWatermarkFrontNonTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        pictureWatermarkFrontNonTOD.ImageTiling = false;
                        pictureWatermarkFrontNonTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
                        pictureWatermarkFrontNonTOD.ImageTransparency = 0;
                        pictureWatermarkFrontNonTOD.ShowBehind = true;
                        rptsd.Watermark.CopyFrom(pictureWatermarkFrontNonTOD);
                        #endregion

                        //pictureWatermark.PageRange = "2,4";
                        rptsd.CreateDocument(false);

                        AT.Print.PDF.rpt_LT_Back rpts = new AT.Print.PDF.rpt_LT_Back
                        {
                            DataSource = lstformattedbills,
                            //ShowPrintStatusDialog = false,
                            // ShowPreviewMarginLines = false

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
                        rpts.Watermark.CopyFrom(pictureWatermarkBackNonTOD);
                        #endregion


                        rpts.CreateDocument(false);

                        rptsd.ModifyDocument(x => { x.AddPages(rpts.Pages); });
                        DevExpress.XtraPrinting.Page myPage2 = rptsd.Pages[1];
                        myPage2.AssignWatermark(pictureWatermarkBackNonTOD);
                        string billdate = lstformattedbills.FirstOrDefault().L1_MonthYear;
                        string ServiceNo = lstformattedbills.FirstOrDefault().L6_SERVDET_SERVNO;
                        var outputfolder = "C://Bills//LT Files//" + billdate + "//" + textFileName;
                        OutputFolderPath OFP = new OutputFolderPath();
                        outputfolder = OFP.LoadLocation() + "//LT Files//" + billdate + "//" + textFileName;
                        if (!Directory.Exists(outputfolder))
                            Directory.CreateDirectory(outputfolder);
                        //var OutPutFolder = 
                        if (Directory.Exists(outputfolder))
                        {
                            rptsd.ExportToPdf(outputfolder + "//" + ServiceNo + ".pdf");
                        }
                        rpts.Dispose();
                        rptsd.Dispose();
                        ParsedBills++;
                    }
                    catch (System.OutOfMemoryException)
                    {
                        System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                        GC.Collect();
                        GC.RemoveMemoryPressure(1024 * 1024);
                    }
                    catch (Exception ex)
                    {
                        AppFunctions.LogError(ex);
                        AppFunctions.CloseWaitForm();
                        XtraMessageBox.Show("Error Parsing Bill " + (ParsedBills + 1) + " of the given file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine(ex.Message);
                    }
                });

                DSBill.Reset();
                DSBill.Dispose();
                AppFunctions.CloseWaitForm();
                XtraMessageBox.Show(ParsedBills + " Bills Parsed Successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }


        AT.Print.PDF.Rpt_LTPDF rptsd = new AT.Print.PDF.Rpt_LTPDF();
        AT.Print.PDF.rpt_LT_Back rpts = new AT.Print.PDF.rpt_LT_Back();

        List<SingleLTBill> lstformattedbills = new List<SingleLTBill>();

        DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkFrontNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
        DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkBackNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();

        async void GeneratePDFFormatsForLTBillsSir(string[] Bills, string Name, int Initial, int Final, string FolderName)
        {

            int processedBills = 0;

            List<int> inlist = Enumerable.Range(0, Final).ToList();

            lstformattedbills = new List<SingleLTBill>();

            rptsd = new AT.Print.PDF.Rpt_LTPDF();
            rpts = new AT.Print.PDF.rpt_LT_Back();



            pictureWatermarkFrontNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
            pictureWatermarkFrontNonTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Non_TOD_Front_Page.png");
            pictureWatermarkFrontNonTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            pictureWatermarkFrontNonTOD.ImageTiling = false;
            pictureWatermarkFrontNonTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
            pictureWatermarkFrontNonTOD.ImageTransparency = 0;
            pictureWatermarkFrontNonTOD.ShowBehind = true;

            pictureWatermarkBackNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
            pictureWatermarkBackNonTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Non_TOD_Back_Page.png");
            pictureWatermarkBackNonTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            pictureWatermarkBackNonTOD.ImageTiling = false;
            pictureWatermarkBackNonTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
            pictureWatermarkBackNonTOD.ImageTransparency = 0;
            pictureWatermarkBackNonTOD.ShowBehind = true;


            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            //Parallel.ForEach(inlist, new ParallelOptions { MaxDegreeOfParallelism = 10 }, z =>
            while (processedBills < Final)
            {
                await Task.Run(() => generatebill(processedBills));
                processedBills++;
                //DataTable dtLTBill = DSBill.Tables[processedBills];

                //try
                //{
                //    SingleLTBill slt = parseSingleLTBill(dtLTBill);

                //    slt.MVPicture = mVImagePath;
                //    lstformattedbills.Add(slt);
                //    //rptsd.DataSource = slt;
                //    rptsd.DataSource = lstformattedbills;
                //    rptsd.Watermark.CopyFrom(pictureWatermarkFrontNonTOD);
                //    rptsd.CreateDocument();
                //    //rpts.DataSource = slt;
                //    rpts.DataSource = lstformattedbills;
                //    rpts.Watermark.CopyFrom(pictureWatermarkBackNonTOD);

                //    rpts.CreateDocument();

                //    rptsd.ModifyDocument(x => { x.AddPages(rpts.Pages); });

                //    DevExpress.XtraPrinting.Page myPage2 = rptsd.Pages[1];
                //    myPage2.AssignWatermark(pictureWatermarkBackNonTOD);

                //    string billDate = slt.L1_MonthYear;
                //    string serviceNo = slt.L6_SERVDET_SERVNO;

                //    var outputfolder = "C://Bills//LT Files//" + billDate + "//" + textFileName;
                //    OutputFolderPath OFP = new OutputFolderPath();
                //    outputfolder = OFP.LoadLocation() + "//LT Files//" + billDate + "//" + textFileName; 

                //    if (!Directory.Exists(outputfolder))
                //        Directory.CreateDirectory(outputfolder);


                //    new PdfStreamingExporter(rptsd, true).Export(outputfolder + "//" + serviceNo + ".pdf");
                //    processedBills +=1;
                //    lstformattedbills.Clear();
                //}
                //catch (System.OutOfMemoryException)
                //{
                //    System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                //    GC.Collect();
                //    GC.RemoveMemoryPressure(1024 * 1024);
                //}

                //catch (Exception ex)
                //{
                //    AppFunctions.LogError(ex);
                //    AppFunctions.CloseWaitForm();
                //    XtraMessageBox.Show("Error Parsing Bill " + processedBills + " of the given file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    Console.WriteLine(ex.Message);
                //}

            }
            watch.Stop();

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            DSBill.Reset();
            DSBill.Dispose();
            XtraMessageBox.Show(processedBills + " Bills Processed Successfully in minutes " + watch.ElapsedMilliseconds, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            AppFunctions.CloseWaitForm();

        }

        public void generatebill(int processedBills)
        {


            DataTable dtLTBill = DSBill.Tables[processedBills];

            try
            {
                SingleLTBill slt = parseSingleLTBill(dtLTBill);

                slt.MVPicture = mVImagePath;
                lstformattedbills.Add(slt);
                //rptsd.DataSource = slt;
                rptsd.DataSource = lstformattedbills;
                rptsd.Watermark.CopyFrom(pictureWatermarkFrontNonTOD);
                rptsd.CreateDocument();
                //rpts.DataSource = slt;
                rpts.DataSource = lstformattedbills;
                rpts.Watermark.CopyFrom(pictureWatermarkBackNonTOD);

                rpts.CreateDocument();

                rptsd.ModifyDocument(x => { x.AddPages(rpts.Pages); });

                DevExpress.XtraPrinting.Page myPage2 = rptsd.Pages[1];
                myPage2.AssignWatermark(pictureWatermarkBackNonTOD);

                string billDate = slt.L1_MonthYear;
                string serviceNo = slt.L6_SERVDET_SERVNO;

                var outputfolder = "C://Bills//LT Files//" + billDate + "//" + textFileName;
                OutputFolderPath OFP = new OutputFolderPath();
                outputfolder = OFP.LoadLocation() + "//LT Files//" + billDate + "//" + textFileName;

                if (!Directory.Exists(outputfolder))
                    Directory.CreateDirectory(outputfolder);


                new PdfStreamingExporter(rptsd, true).Export(outputfolder + "//" + serviceNo + ".pdf");
                processedBills += 1;
                lstformattedbills.Clear();
                System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect();
                GC.RemoveMemoryPressure(1024 * 1024);
            }
            catch (System.OutOfMemoryException)
            {
                System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect();
                GC.RemoveMemoryPressure(1024 * 1024);
            }

            catch (Exception ex)
            {
                AppFunctions.LogError(ex);
                AppFunctions.CloseWaitForm();
                XtraMessageBox.Show("Error Parsing Bill " + processedBills + " of the given file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }
        }

    }
    
}

