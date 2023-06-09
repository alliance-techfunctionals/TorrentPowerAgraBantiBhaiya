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
using System.Threading;
using System.Windows.Forms;
using static System.Drawing.Printing.PrinterSettings;

namespace AT.Print
{
    public partial class Print_HT1 : UserControl
    {
        public Print_HT1()
        {
            InitializeComponent();
            BindPrinters();
        }
        string textFileName;
        string mVImagePath;
        string ServiceNo = "";
        string LineNo = "";
        string MonthYear = "";
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


        string[] singleHTBills;

        private void SbPrintBill_Click(object sender, EventArgs e)
        {
            var sb = sender as SimpleButton;
            if (sb.Name == "sbPrintBill")
                if (cbDefaultPrinter.SelectedIndex == -1 || cbNonTODTraySource.SelectedIndex == -1 ||
                                cbTODTraySource.SelectedIndex == -1 || cbSeparatorTraySource.SelectedIndex == -1)
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
                    if (contents.StartsWith("HT|"))
                    {
                        singleHTBills = contents.Split(new String[] { "HT|" }, StringSplitOptions.RemoveEmptyEntries);
                        if (!select_mVImg())
                        {
                            AppFunctions.CloseWaitForm();
                            return;
                        }
                        XtraMessageBox.Show("Total Bills in this file: " + singleHTBills.Length.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AppFunctions.ShowWaitForm("Validating HT Bills Now before I generate the PDF files !!");
                        if (ValidatetxtFile(singleHTBills))
                        {
                            /*
                            if (singleHTBills.Count() > 9 && sb.Name == "sbSavePDF")
                            {
                                int i = singleHTBills.Count() / 3;
                                //System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(StartPrinting_LTBills));
                                Thread myNewThread1 = new Thread(() => StartPrinting_HTBills(singleHTBills, sb.Name, i * 0, i * 1, "1"));
                                myNewThread1.Start();
                                Thread myNewThread2 = new Thread(() => StartPrinting_HTBills(singleHTBills, sb.Name, i * 1, i * 2, "1"));
                                myNewThread2.Start();
                                Thread myNewThread3 = new Thread(() => StartPrinting_HTBills(singleHTBills, sb.Name, i * 2, singleHTBills.Count(), "1"));
                                myNewThread3.Start();
                            }
                            else
                            */
                            {
                                Thread myNewThread1 = new Thread(() => StartPrinting_HTBills(singleHTBills, sb.Name, 0, singleHTBills.Count() - 1, "1"));
                                myNewThread1.Start();
                            }
                            //StartPrinting_HTBills(singleHTBills, sb.Name);
                            
                        }
                        else
                        {
                            //XtraMessageBox.Show("There is some error in txt file for Service no:" + ServiceNo);
                            AppFunctions.CloseWaitForm();
                            return;
                        }
                        AppFunctions.CloseWaitForm();
                        XtraMessageBox.Show(singleHTBills.Count() + " bills has been parsed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        AppFunctions.CloseWaitForm();
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

        //private void StartPrinting_HTBills(string[] Bills, string Name)
        void StartPrinting_HTBills(string[] Bills, string Name, int Initial, int Final, string FolderName)
        {
            string LotNo = "InitialLot";
            string LotNoCopy = "InitialLot";
            string TOD_NonTODFlag = "";
            int BillNo = 1, Counter = 1, ParsedBills = 0;
            DataTable dtSingleHTBill=new DataTable();
            XtraReport NonTODReport = new XtraReport();
            XtraReport TODReport = new XtraReport();
            NonTODReport.PrinterName = cbDefaultPrinter.Text;
            TODReport.PrinterName = cbDefaultPrinter.Text;
            NonTODReport.CreateDocument();
            TODReport.CreateDocument();
            string FileName = AppFunctions.ProcessedBillData();
            XtraReport collectorReport = new XtraReport
            {
                DisplayName = "HT Print",
            };
            
            //foreach (var Bill in Bills)
            int z = Initial - 1;
            while (z < Final)
            {
                z++;
                dtSingleHTBill = DSBill.Tables[z];
                try
                {
                    //AppFunctions.ShowWaitForm("Generating HT Bills Now..!!");
                    List<SingleHTBill> lstformattedbills = new List<SingleHTBill>();

                    //DataTable dtSingleHTBill = ParseAsDataTable.HT_FileTxtToDataTable(Bill);

                    if ((LotNoCopy != dtSingleHTBill.Rows[0][4].ToString().Trim() || Counter == 51 || TOD_NonTODFlag != dtSingleHTBill.Rows[0][7].ToString().Trim()) && LotNoCopy != "InitialLot" && TOD_NonTODFlag != "")
                    {
                        //ReportPrintTool printTool = new ReportPrintTool(collectorReport);
                        //printTool.ShowPreview();
                        MemoryStream ms = new MemoryStream();
                        var buffer = ms.GetBuffer();
                        Array.Clear(buffer, 0, buffer.Length);
                        ms.Position = 0;
                        ms.SetLength(0);
                        ms.Capacity = 0; // <<< 
                        //rpta.Print(cbDefaultPrinter.Text);
                        collectorReport.Print(cbDefaultPrinter.Text);
                        collectorReport.Pages.Clear();
                        Counter = 1;
                        collectorReport.Dispose();

                    }

                    if (LotNo != dtSingleHTBill.Rows[0][4].ToString().Trim())
                    {
                        if (Name != "sbSavePDF")
                        {
                            LotNo = (String)dtSingleHTBill.Rows[0][4];
                            LotNoCopy = (String)dtSingleHTBill.Rows[0][4];
                            SingleHTBill billSaprator = new SingleHTBill();
                            billSaprator.Sap_Zone = "Zone No. " + dtSingleHTBill.Rows[0][1];
                            billSaprator.Sap_LotNo = "LOT No. " + dtSingleHTBill.Rows[0][4];
                            billSaprator.Sap_GrpNo = "Group No. " + dtSingleHTBill.Rows[0][2];
                            lstformattedbills.Add(billSaprator);

                            Rpt_Saprator sap_rpt = new Rpt_Saprator
                            {
                                DataSource = lstformattedbills
                            };

                            sap_rpt.CreateDocument();
                            sap_rpt.ShowPrintMarginsWarning = false;
                            sap_rpt.PrinterName = cbDefaultPrinter.Text;
                            sap_rpt.PrintingSystem.StartPrint += sap_print;
                            sap_rpt.Print(cbDefaultPrinter.Text);
                            lstformattedbills.Clear();
                            //ParsedBills++;


                        }
                    }

                    SingleHTBill sht = parseSingleHTBill(dtSingleHTBill);
                    TOD_NonTODFlag = sht.L1_TODOrNon_TODFlag;
                    sht.MVPicture = mVImagePath;
                    lstformattedbills.Add(sht);

                    #region HT-PDF Non-TOD

                    if (Name == "sbSavePDF" && String.Equals(sht.L1_TODOrNon_TODFlag, "0"))
                    {

                        AT.Print.PDF.Rpt_HTPDF rptsd1 = new AT.Print.PDF.Rpt_HTPDF
                        {
                            DataSource = lstformattedbills,
                        };
                        
                        #region WaterMark Picture Front Page PDF Non-TOD
                        DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkFrontNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
                        pictureWatermarkFrontNonTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Non_TOD_Front_Page.png");
                        pictureWatermarkFrontNonTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        pictureWatermarkFrontNonTOD.ImageTiling = false;
                        pictureWatermarkFrontNonTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Zoom;
                        pictureWatermarkFrontNonTOD.ImageTransparency = 0;
                        pictureWatermarkFrontNonTOD.ShowBehind = true;
                        //pictureWatermark.PageRange = "2,4";
                        rptsd1.Watermark.CopyFrom(pictureWatermarkFrontNonTOD);
                        #endregion
                        
                        rptsd1.CreateDocument(false);
                        AT.Print.PDF.rpt_HT_Back rpts = new AT.Print.PDF.rpt_HT_Back
                        {
                            DataSource = lstformattedbills,
                        };

                        #region WaterMark Picture Back Page PDF Non-TOD
                        DevExpress.XtraPrinting.Drawing.Watermark pictureWatermarkBackNonTOD = new DevExpress.XtraPrinting.Drawing.Watermark();
                        pictureWatermarkBackNonTOD.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(Application.StartupPath + "\\Contents\\CategorySlabImages\\Duplex_Non_Tod_Back_Page.png");
                        pictureWatermarkBackNonTOD.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        pictureWatermarkBackNonTOD.ImageTiling = false;
                        pictureWatermarkBackNonTOD.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
                        pictureWatermarkBackNonTOD.ImageTransparency = 0;
                        pictureWatermarkBackNonTOD.ShowBehind = true;
                        //pictureWatermark.PageRange = "2,4";
                        rpts.Watermark.CopyFrom(pictureWatermarkBackNonTOD);
                        #endregion

                        rpts.CreateDocument(false);
                        rptsd1.ModifyDocument(x => { x.AddPages(rpts.Pages); });
                        DevExpress.XtraPrinting.Page myPage2 = rptsd1.Pages[1];
                        myPage2.AssignWatermark(pictureWatermarkBackNonTOD);
                        string billdate = lstformattedbills.FirstOrDefault().L1_MonthYear;
                        string ServiceNo = lstformattedbills.FirstOrDefault().L6_SERVDET_SERVNO;
                        
                       // DateTime.TryParseExact(billdate, "MMMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime billDate);
                       
                        var outputfolder = "C://Bills//HTFiles//" + billdate + "//" + textFileName;
                        OutputFolderPath OFP = new OutputFolderPath();
                        outputfolder = OFP.LoadLocation() + "//HTFiles//" + billdate + "//" + textFileName; ;
                        if (!Directory.Exists(outputfolder))
                            Directory.CreateDirectory(outputfolder);
                        //var OutPutFolder = 
                        if (Directory.Exists(outputfolder))
                        {
                            //rptsd.ExportToPdf("C://Bills//HTFiles//" + billdate + "//" + textFileName + "//" + ServiceNo + ".pdf");
                            rptsd1.ExportToPdf(outputfolder + "//" + ServiceNo + ".pdf");
                            rptsd1.Dispose();
                        }
                        ParsedBills++;
                        //AppFunctions.CloseWaitForm();
                    }
                    #endregion

                    #region Print non-Tod_HT

                    else if (string.Equals(sht.L1_TODOrNon_TODFlag, "0"))
                    {
                        PrinterSettings ps = new PrinterSettings() { PrinterName = cbDefaultPrinter.Text };
                        using (Graphics g = ps.CreateMeasurementGraphics(ps.DefaultPageSettings))
                        {
                            Margins MinMargins = DevExpress.XtraPrinting.Native.DeviceCaps.GetMinMargins(g);
                            Console.WriteLine("Minimum Margins for " + ps.PrinterName + ": " + MinMargins.ToString());
                        }
                        AT.Print.Rpt_HT_Print rpta = new Rpt_HT_Print
                        {
                            DataSource = lstformattedbills,
                            DisplayName = sht.L6_SERVDET_SERVNO,
                        };
                        rpta.Watermark.ImageTransparency = 255;
                        rpta.PrinterName = cbDefaultPrinter.SelectedItem.ToString();    //the printername property should be specified before creating a document (which is performed using the xtrareport.createdocument method)
                        rpta.PrintingSystem.Document.Name = sht.L6_SERVDET_SERVNO;
                        rpta.CreateDocument();
                        AT.Print.Rpt_HT_Print_Back rptb = new AT.Print.Rpt_HT_Print_Back
                        {
                            DataSource = lstformattedbills,
                        };
                        rptb.CreateDocument();
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
                        AppFunctions.CloseWaitForm();
                        ParsedBills++;
                    }
                    #endregion

                    #region  HT-pdf with tod

                    else if (Name == "sbSavePDF" && string.Equals(sht.L1_TODOrNon_TODFlag, "1"))
                    {
                        AT.Print.PDF.Rpt_HT_TodPDF rptsd = new AT.Print.PDF.Rpt_HT_TodPDF
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


                        AT.Print.PDF.rpt_HT_BackPDF rpts = new AT.Print.PDF.rpt_HT_BackPDF
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
                        rpts.Watermark.CopyFrom(pictureWatermarkBackTOD);
                        #endregion

                        rpts.CreateDocument(false);
                        rptsd.ModifyDocument(x => { x.AddPages(rpts.Pages); });
                        DevExpress.XtraPrinting.Page myPage = rptsd.Pages[1];
                        myPage.AssignWatermark(pictureWatermarkBackTOD);
                        string billdate = lstformattedbills.FirstOrDefault().L1_MonthYear;
                        string serviceno = lstformattedbills.FirstOrDefault().L6_SERVDET_SERVNO;
                        //DateTime.TryParseExact(billdate, "mm-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime billDate);
                        var outputfolder = "C://Bills//HTFiles//" + billdate + "//" + textFileName;
                        OutputFolderPath OFP = new OutputFolderPath();
                        outputfolder = OFP.LoadLocation() + "//HTFiles//" + billdate + "//" + textFileName; ;
                        if (!Directory.Exists(outputfolder))
                            Directory.CreateDirectory(outputfolder);
                        //var OutPutFolder = 
                        if (Directory.Exists(outputfolder))
                        {
                            //rptsd.ExportToPdf("C://Bills//HTFiles//" + billdate + "//" + textFileName + "//" + serviceno + ".pdf");
                            rptsd.ExportToPdf(outputfolder + "//" + ServiceNo + ".pdf");
                            rptsd.Dispose();
                        }

                        ParsedBills++;
                        //AppFunctions.CloseWaitForm();
                    }
                    #endregion

                    #region HT-print with tod

                    else if (string.Equals(sht.L1_TODOrNon_TODFlag, "1"))
                    {
                        AT.Print.Rpt_HT_TOD_Print rpta = new AT.Print.Rpt_HT_TOD_Print
                        {
                            DataSource = lstformattedbills,
                            DisplayName = sht.L6_SERVDET_SERVNO,
                        };
                        
                        rpta.Watermark.ImageTransparency = 255;
                        rpta.PrinterName = cbDefaultPrinter.SelectedItem.ToString();    //the printername property should be specified before creating a document (which is performed using the xtrareport.createdocument method)
                        rpta.PrintingSystem.Document.Name = sht.L6_SERVDET_SERVNO;
                        rpta.CreateDocument();
                        AT.Print.Rpt_HT_TOD_Print_Back rptb = new AT.Print.Rpt_HT_TOD_Print_Back
                        {
                            DataSource = lstformattedbills,
                        };

                        rptb.CreateDocument();
                        rpta.ModifyDocument(x => { x.AddPages(rptb.Pages); });
                        //rpta.PrintingSystem.StartPrint += TOD_StartPrint;

                        #region exporting to pdf
                        //rpta.Print(cbDefaultPrinter.Text);
                        #endregion
                        collectorReport.PrintingSystem.StartPrint += TOD_StartPrint;
                        collectorReport.Pages.AddRange(rpta.Pages);
                        if (Bills.Count() == BillNo && LotNoCopy != "InitialLot")
                        {
                            collectorReport.Print(cbDefaultPrinter.Text);
                            collectorReport.Pages.Clear();
                        }
                        AppFunctions.CloseWaitForm();
                        //rpta.Print();
                        ParsedBills++;
                    }
                    #endregion
                    else
                        Console.WriteLine("could not find tod flag in bill: " + sht.L6_SERVDET_SERVNO);

                }
                catch (System.OutOfMemoryException)
                {
                    //XtraMessageBox.Show("Error Parsing Bill " + BillNo + " of the given file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AppFunctions.LogError("Error Parsing Service No. " + ServiceNo + " of the given file due to out of memory.");
                    AppFunctions.LogProcessedBill(Convert.ToString(dtSingleHTBill.Rows[0][1]), Convert.ToString(dtSingleHTBill.Rows[0][4]), Convert.ToString(dtSingleHTBill.Rows[0][2]), Convert.ToString(dtSingleHTBill.Rows[0][5]), ServiceNo, FileName, "No");
                    //SaveFile(Convert.ToString(ServiceNo));
                    System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                    GC.Collect();
                    GC.RemoveMemoryPressure(1024 * 1024);
                    break;
                }
                catch (Exception ex)
                {
                    AppFunctions.LogError(ex);
                    AppFunctions.LogProcessedBill(Convert.ToString(dtSingleHTBill.Rows[0][1]), Convert.ToString(dtSingleHTBill.Rows[0][4]), Convert.ToString(dtSingleHTBill.Rows[0][2]), Convert.ToString(dtSingleHTBill.Rows[0][5]), ServiceNo, FileName, "No");
                    AppFunctions.CloseWaitForm();
                    //XtraMessageBox.Show("Error Parsing Bill " + BillNo + " of the given file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                AppFunctions.LogProcessedBill(Convert.ToString(dtSingleHTBill.Rows[0][1]), Convert.ToString(dtSingleHTBill.Rows[0][4]), Convert.ToString(dtSingleHTBill.Rows[0][2]), Convert.ToString(dtSingleHTBill.Rows[0][5]), ServiceNo, FileName, "Yes");
                BillNo++;
            }
            DSBill.Reset();
            DSBill.Dispose();
            //AppFunctions.CloseWaitForm();
            //XtraMessageBox.Show(ParsedBills + " Bills Parsed Successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //TODReport.PrintingSystem.StartPrint += TOD_StartPrint;
            // NonTODReport.PrintingSystem.StartPrint += NonTOD_StartPrint;
            // TODReport.ExportToPdf("C://Bills//TOD.pdf");
            // NonTODReport.ExportToPdf("C://Bills//NonTOD.pdf");



        }

        SingleHTBill parseSingleHTBill(DataTable dtSingleHTBill)
        {
            
            SingleHTBill sht = new SingleHTBill();
            #region --Lines
            #region Line-1
            ServiceNo = dtSingleHTBill.Rows[5][0].ToString();
            //Line 1 Starts
            LineNo = "1";
            sht.L1_BillType = "HT";
            sht.L1_MonthYear = dtSingleHTBill.Rows[0][0].ToString();
            sht.L1_Zone = dtSingleHTBill.Rows[0][1].ToString();
            sht.L1_BU = dtSingleHTBill.Rows[0][2].ToString();
            sht.L1_PC = dtSingleHTBill.Rows[0][3].ToString();
            sht.L1_Route = dtSingleHTBill.Rows[0][4].ToString();
            sht.L1_Bill_seq_no = dtSingleHTBill.Rows[0][5].ToString();
            sht.L1_FeederName = dtSingleHTBill.Rows[0][6].ToString();
            sht.L1_TODOrNon_TODFlag = dtSingleHTBill.Rows[0][7].ToString();
            sht.L1_AKY_indicator = dtSingleHTBill.Rows[0][8].ToString();
            sht.L1_DisconnectionMSGPrintingIMMEDIATE = dtSingleHTBill.Rows[0][9].ToString();
            sht.L1_BillingCode = dtSingleHTBill.Rows[0][10].ToString();
            if (dtSingleHTBill.Rows[0][11].ToString() == "")
            {
                sht.L1_Customer_PAN = dtSingleHTBill.Rows[0][11].ToString();
            }
            else
            {
                sht.L1_Customer_PAN = "PAN: " + dtSingleHTBill.Rows[0][11].ToString();
            }
            //Line 1 End
            #endregion

            #region Line-2
            //Line 2 Starts
            LineNo = "2";
            sht.L2_Name = dtSingleHTBill.Rows[1][0].ToString().Trim('�');
            //Line 2 End
            #endregion

            #region Line-3
            //Line 3 Starts
            LineNo = "3";
            sht.L3_Addr1 = dtSingleHTBill.Rows[2][0].ToString().Trim('�');
            //Line 3 End
            #endregion

            #region Line-4
            //Line 4 Starts
            LineNo = "4";
            sht.L4_Addr2 = dtSingleHTBill.Rows[3][0].ToString().Trim('�');
            //Line 4 End
            #endregion

            #region Line-5
            //Line 5 Starts
            LineNo = "5";
            sht.L5_Addr3 = dtSingleHTBill.Rows[4][0].ToString().Trim('�');
            //Line 5 End
            #endregion

            #region Line-6
            //Line 6 Starts
            LineNo = "6";
            sht.L6_MeasureContractDemand = dtSingleHTBill.Rows[5][10].ToString();
            sht.L6_SERVDET_SERVNO = dtSingleHTBill.Rows[5][0].ToString();
            sht.L6_SERVDET_SANC_LOAD = string.IsNullOrEmpty(dtSingleHTBill.Rows[5][1].ToString()) ? "" : dtSingleHTBill.Rows[5][1].ToString();
            sht.L6_bill_demand = string.IsNullOrEmpty(dtSingleHTBill.Rows[5][2].ToString()) ? "" : dtSingleHTBill.Rows[5][2].ToString();
            sht.L6_ACTUAL_DEMAND = string.IsNullOrEmpty(dtSingleHTBill.Rows[5][3].ToString()) ? "" : dtSingleHTBill.Rows[5][3].ToString();
            sht.L6_TARIFF_DESCR = string.IsNullOrEmpty(dtSingleHTBill.Rows[5][4].ToString()) ? "" : dtSingleHTBill.Rows[5][4].ToString();
            sht.L6_EXCESS_DEMAND = string.IsNullOrEmpty(dtSingleHTBill.Rows[5][5].ToString()) ? "" : dtSingleHTBill.Rows[5][5].ToString();
            sht.L6_SUPPLY_VOLTAGE = dtSingleHTBill.Rows[5][6].ToString();
            sht.L6_BILL_PF = dtSingleHTBill.Rows[5][7].ToString();
            sht.L6_MTRDET_LF_PERC = dtSingleHTBill.Rows[5][8].ToString();
            sht.L6_BILL_TYPE = dtSingleHTBill.Rows[5][9].ToString();
            sht.L6_MeasureContractDemand = dtSingleHTBill.Rows[5][10].ToString();
            sht.L6_Kvah_Indicator = dtSingleHTBill.Rows[5][11].ToString();
            sht.L6_LT_Metering_Flag = dtSingleHTBill.Rows[5][12].ToString();
            //Line 6 End
            #endregion

            #region Line-7
            //Line 7 Starts
            LineNo = "7";
            sht.L7_Due_Date = dtSingleHTBill.Rows[6][0].ToString();
            sht.L7_BillDt = dtSingleHTBill.Rows[6][1].ToString();
            int YYYY, MM, DD;
            YYYY = int.Parse(dtSingleHTBill.Rows[6][2].ToString().Split('-')[2]);
            MM = int.Parse(dtSingleHTBill.Rows[6][2].ToString().Split('-')[1]);
            DD = int.Parse(dtSingleHTBill.Rows[6][2].ToString().Split('-')[0]);
            DateTime PreviousDate = new DateTime(YYYY, MM, DD);
            sht.L7_PrevReadDt = (PreviousDate.AddDays(-1)).ToString("dd-MM-yy");
            sht.L7_ReaDt = dtSingleHTBill.Rows[6][3].ToString();
            sht.L7_LastPymtDate = dtSingleHTBill.Rows[6][4].ToString();
            sht.L7_LastPayementAmount = dtSingleHTBill.Rows[6][5].ToString().Trim('�');
            sht.L7_LastPayementMode = dtSingleHTBill.Rows[6][6].ToString();
            //Line 7 End
            #endregion

            #region Line-8
            //Line 8 Starts
            LineNo = "8";
            sht.L8_FixedCharge = dtSingleHTBill.Rows[7][0].ToString();
            sht.L8_EnergyCharge = dtSingleHTBill.Rows[7][1].ToString();
            sht.L8_TODCharges = dtSingleHTBill.Rows[7][2].ToString();
            sht.L8_TODCharges = sht.L8_TODCharges.Contains("-") ? ("-" + sht.L8_TODCharges.Replace("-", "")) : sht.L8_TODCharges;
            sht.L8_ACCharge = dtSingleHTBill.Rows[7][3].ToString();
            sht.L8_GovTax = dtSingleHTBill.Rows[7][4].ToString();
            sht.L8_MinCharge = dtSingleHTBill.Rows[7][5].ToString();
            sht.L8_ServdetTotbBdtOthr = dtSingleHTBill.Rows[7][6].ToString();
            sht.L8_RegulatoryCharge_1 = dtSingleHTBill.Rows[7][7].ToString();
            sht.L8_RegulatoryCharge_2 = dtSingleHTBill.Rows[7][8].ToString();
            sht.L8_RebateIncurredCurrentMonth = dtSingleHTBill.Rows[7][9].ToString();
            sht.L8_AmountPayableBeforeDueDate = dtSingleHTBill.Rows[7][10].ToString();
            sht.L8_AmountPayableBeforeDueDate = sht.L8_AmountPayableBeforeDueDate.Contains("CR") ? ("-" + sht.L8_AmountPayableBeforeDueDate.Replace("CR", "")) : (sht.L8_AmountPayableBeforeDueDate.Contains("-") ? ("-" + sht.L8_AmountPayableBeforeDueDate.Replace("-", "")) : sht.L8_AmountPayableBeforeDueDate);
            sht.L8_TNo = dtSingleHTBill.Rows[7][11].ToString().Trim('�');
            //sht.L8_ParkingAmount = dtSingleHTBill.Rows[7][12].ToString();
            sht.L8_ParkingAmount = Math.Ceiling(Convert.ToDecimal(string.IsNullOrEmpty(dtSingleHTBill.Rows[7][12].ToString()) ? "0" : dtSingleHTBill.Rows[7][12].ToString())).ToString();
            sht.L8_Subsidy_Charges = dtSingleHTBill.Rows[7][13].ToString();
            sht.L8_Solar_Export_Energy = dtSingleHTBill.Rows[7][14].ToString();
            sht.L8_Intrest_Amount = dtSingleHTBill.Rows[7][15].ToString();
            //Line 8 End
            #endregion

            #region Line-9
            //Line 9 Starts
            LineNo = "9";
            sht.L9_TotDbArr = dtSingleHTBill.Rows[8][0].ToString();
            sht.L9_CurrBillAmt = dtSingleHTBill.Rows[8][1].ToString();
            sht.L9_CurrBillAmt = sht.L9_CurrBillAmt.Contains("-") ? ("-" + sht.L9_CurrBillAmt.Replace("-", "")) : sht.L9_CurrBillAmt;
            sht.L9_Int_Tpl = dtSingleHTBill.Rows[8][2].ToString();
            sht.L9_ArrsTpl = dtSingleHTBill.Rows[8][3].ToString();
            sht.L9_CurrBillAmtIntTplArrsTpl = dtSingleHTBill.Rows[8][4].ToString();
            sht.L9_Amount_Payable = dtSingleHTBill.Rows[8][5].ToString();

            if (Convert.ToDouble(dtSingleHTBill.Rows[8][5].ToString()) < 0)
            {
                sht.L9_Amount_Payable = "NOT TO PAY";
            }
           
            //Line 9 End
            #endregion

            #region Line-10
            //Line 10 Starts
            LineNo = "10";
            sht.L10_LFincentive = dtSingleHTBill.Rows[9][0].ToString();
            sht.L10_DisconnDate = dtSingleHTBill.Rows[9][1].ToString();
            sht.L10_TotArrUPPCLIntUPPCLIntArrUPPCL = dtSingleHTBill.Rows[9][2].ToString();
            sht.L10_TotArrUPPCLIntUPPCLIntArrUPPCL_Rounded = string.IsNullOrEmpty(dtSingleHTBill.Rows[9][2].ToString()) ? "0" : Math.Round(Convert.ToDecimal(dtSingleHTBill.Rows[9][2].ToString()) + (decimal).49, 0, MidpointRounding.AwayFromZero).ToString();
            sht.L10_SecDeptBdt = dtSingleHTBill.Rows[9][3].ToString();
            sht.L10_DmdChgPenalty = dtSingleHTBill.Rows[9][4].ToString();
            sht.L10_UPPCL_ArrearAmount = dtSingleHTBill.Rows[9][5].ToString();
            sht.L10_UPPCLIntOnArrearAmount = dtSingleHTBill.Rows[9][6].ToString();
            sht.L10_Mode = dtSingleHTBill.Rows[9][7].ToString();
            sht.L10_TheftAmount= dtSingleHTBill.Rows[9][8].ToString();
            //Line 10 End
            #endregion

            #region Line-11
            //Line 11 starts
            LineNo = "11";
            sht.L11_MTRSNO_1 = dtSingleHTBill.Rows[10][0].ToString();
            sht.L11_MTRSNO_2_IF_AVAILABLE = dtSingleHTBill.Rows[10][1].ToString();






            //Line 11 End
            #endregion

            #region Line-12-16
            //Line 12-16 starts
            LineNo = "12";
            sht.L12_KWH_PRESREAD = dtSingleHTBill.Rows[11][0].ToString();
            sht.L12_KVAH_PRESREAD = dtSingleHTBill.Rows[11][1].ToString();
            sht.L12_KVA_PRESREAD = dtSingleHTBill.Rows[11][2].ToString();
            LineNo = "13";
            sht.L13_KWH_PASTREAD = dtSingleHTBill.Rows[12][0].ToString();
            sht.L13_KVAH_PASTREAD = dtSingleHTBill.Rows[12][1].ToString();
            sht.L13_KVA_PASTREAD = dtSingleHTBill.Rows[12][2].ToString();
            sht.L13_Purpose = dtSingleHTBill.Rows[12][3].ToString();
            LineNo = "14";
            sht.L14_Multiplying_factor_KWH = dtSingleHTBill.Rows[13][0].ToString();
            sht.L14_Multiplying_factor_KVAH = dtSingleHTBill.Rows[13][1].ToString();
            sht.L14_Multiplying_factor_KVA = dtSingleHTBill.Rows[13][2].ToString();

            LineNo = "15";
            sht.L15_KWH_UNITS = dtSingleHTBill.Rows[14][0].ToString();
            sht.L15_KVAH_UNITS = dtSingleHTBill.Rows[14][1].ToString();
            sht.L15_KVA_UNITS = dtSingleHTBill.Rows[14][2].ToString();
            LineNo = "16";
            sht.L16_TOD1_KVAH_Units = dtSingleHTBill.Rows[15][0].ToString();
            sht.L16_TOD2_KVAH_Units = dtSingleHTBill.Rows[15][1].ToString();
            sht.L16_TOD3_KVAH_Units = dtSingleHTBill.Rows[15][2].ToString();
            sht.L16_TOD4_KVAH_Units = dtSingleHTBill.Rows[15][3].ToString();
            //Line 12-16  end
            #endregion





            #region Line-17
            //Line 17 Starts
            LineNo = "17";
            sht.L17_TOD1_KVA_Units = dtSingleHTBill.Rows[16][0].ToString();
            sht.L17_TOD2_KVA_Units = dtSingleHTBill.Rows[16][1].ToString();
            sht.L17_TOD3_KVA_Units = dtSingleHTBill.Rows[16][2].ToString();
            sht.L17_TOD4_KVA_Units = dtSingleHTBill.Rows[16][3].ToString();
            //Line 17 End
            #endregion

            #region Line-18
            //Line 18 Starts
            LineNo = "18";
            sht.L18_KWH_PRESREAD = dtSingleHTBill.Rows[17][0].ToString();
            sht.L18_KVAH_PRESREAD = dtSingleHTBill.Rows[17][1].ToString();
            sht.L18_KVA_PRESREAD = dtSingleHTBill.Rows[17][2].ToString();
            //Line 18 End
            #endregion

            #region Line-19
            //Line 19 Starts
            LineNo = "19";
            sht.L19_KWH_PASTREAD = dtSingleHTBill.Rows[18][0].ToString();
            sht.L19_KVAH_PASTREAD = dtSingleHTBill.Rows[18][1].ToString();
            sht.L19_KVA_PASTREAD = dtSingleHTBill.Rows[18][2].ToString();
            //Line 19 End
            #endregion

            #region Line-20
            //Line 20 Starts
            LineNo = "20";
            sht.L20_Multiplying_Factor_KWH = dtSingleHTBill.Rows[19][0].ToString();
            sht.L20_Multiplying_Factor_KVAH = dtSingleHTBill.Rows[19][1].ToString();
            sht.L20_Multiplying_Factor_KVA = dtSingleHTBill.Rows[19][2].ToString();
            //Line 20 End
            #endregion

            #region Line-21
            //Line 21 Starts
            LineNo = "21";
            sht.L21_KWH_UNITS = dtSingleHTBill.Rows[20][0].ToString();
            sht.L21_KVAH_UNITS = dtSingleHTBill.Rows[20][1].ToString();
            sht.L21_KVA_UNITS = dtSingleHTBill.Rows[20][2].ToString();

            //Line 21 End
            #endregion




            #region Line-22
            //Line 22 Starts
            LineNo = "22";
            sht.L22_TOD1_KVAH_Units = dtSingleHTBill.Rows[21][0].ToString();
            sht.L22_TOD2_KVAH_Units = dtSingleHTBill.Rows[21][1].ToString();
            sht.L22_TOD3_KVAH_Units = dtSingleHTBill.Rows[21][2].ToString();
            sht.L22_TOD4_KVAH_Units = dtSingleHTBill.Rows[21][3].ToString();
            //Line 22 End
            #endregion

            #region Line-23
            //Line 23 Starts
            LineNo = "23";
            sht.L23_TOD1_KVA_Units = dtSingleHTBill.Rows[22][0].ToString();
            sht.L23_TOD2_KVA_Units = dtSingleHTBill.Rows[22][1].ToString();
            sht.L23_TOD3_KVA_Units = dtSingleHTBill.Rows[22][2].ToString();
            sht.L23_TOD4_KVA_Units = dtSingleHTBill.Rows[22][3].ToString();

            //Line 23 End
            #endregion

            #region Line-24
            //Line 24 Starts
            LineNo = "24";
            sht.L24_MonYear_1 = dtSingleHTBill.Rows[23][0].ToString();
            sht.L24_KVA_UNITS_1 = dtSingleHTBill.Rows[23][0].ToString();
            sht.L24_MonYear_2 = dtSingleHTBill.Rows[23][1].ToString();
            sht.L24_KVA_UNITS_2 = dtSingleHTBill.Rows[23][1].ToString();
            sht.L24_MonYear_3 = dtSingleHTBill.Rows[23][2].ToString();
            sht.L24_KVA_UNITS_3 = dtSingleHTBill.Rows[23][2].ToString();
            sht.L24_MonYear_4 = dtSingleHTBill.Rows[23][3].ToString();
            sht.L24_KVA_UNITS_4 = dtSingleHTBill.Rows[23][3].ToString();
            sht.L24_MonYear_5 = dtSingleHTBill.Rows[23][4].ToString();
            sht.L24_KVA_UNITS_5 = dtSingleHTBill.Rows[23][4].ToString();
            sht.L24_MonYear_6 = dtSingleHTBill.Rows[23][5].ToString();
            sht.L24_KVA_UNITS_6 = dtSingleHTBill.Rows[23][5].ToString();
            sht.L24_MonYear_7 = dtSingleHTBill.Rows[23][6].ToString();
            sht.L24_KVA_UNITS_7 = dtSingleHTBill.Rows[23][6].ToString();

            DataTable KVAchrtData = new DataTable();
            KVAchrtData.Columns.Add("MonthYear");
            KVAchrtData.Columns.Add("Value", typeof(decimal));
            for (int i = 0; i < 25; i += 2)
            {
                if (MonthYear != (string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[23][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleHTBill.Rows[23][i])))
                {
                    KVAchrtData.Rows.Add(new object[] { string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[23][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleHTBill.Rows[23][i]), string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[23][i + 1])) ? 0 : Convert.ToDecimal(dtSingleHTBill.Rows[23][i + 1]) });
                    MonthYear = string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[23][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleHTBill.Rows[23][i]);
                }
                else
                {
                    KVAchrtData.Rows.Add(new object[] { MonthYear.Replace("-","  "), string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[23][i + 1])) ? 0 : Convert.ToDecimal(dtSingleHTBill.Rows[23][i + 1]) });
                    MonthYear = string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[23][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleHTBill.Rows[23][i]);
                }
            }
            MonthYear = "";
            sht.KVAgrph = KVAchrtData;

            // DataTable chrtData = new DataTable();
            // chrtData.Columns.Add("MonthYear");
            // chrtData.Columns.Add("Value", typeof(Int32));
            // for (int i = 0; i <= 25; i += 2)
            // {
            //     var crg = chrtData.NewRow();
            //     crg["MonthYear"] = Convert.ToString(dtSingleHTBill.Rows[23][i]);
            //     crg["Value"] = string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[23][i + 1])) ? 0 : Convert.ToInt32(dtSingleHTBill.Rows[23][i + 1]);
            //
            //     chrtData.Rows.Add(crg.ItemArray);
            // }
            //
            // sht.KVAgrph = chrtData;
            //















            //Line 24 end
            #endregion

            #region Line-25
            //Line 25 Starts
            LineNo = "25";
            sht.L25_MonYear_1 = dtSingleHTBill.Rows[24][0].ToString();
            sht.L25_KVAH_UNITS_1 = dtSingleHTBill.Rows[24][0].ToString();
            sht.L25_MonYear_2 = dtSingleHTBill.Rows[24][1].ToString();
            sht.L25_KVAH_UNITS_2 = dtSingleHTBill.Rows[24][1].ToString();
            sht.L25_MonYear_3 = dtSingleHTBill.Rows[24][2].ToString();
            sht.L25_KVAH_UNITS_3 = dtSingleHTBill.Rows[24][2].ToString();
            sht.L25_MonYear_4 = dtSingleHTBill.Rows[24][3].ToString();
            sht.L25_KVAH_UNITS_4 = dtSingleHTBill.Rows[24][3].ToString();
            sht.L25_MonYear_5 = dtSingleHTBill.Rows[24][4].ToString();
            sht.L25_KVAH_UNITS_5 = dtSingleHTBill.Rows[24][4].ToString();
            sht.L25_MonYear_6 = dtSingleHTBill.Rows[24][5].ToString();
            sht.L25_KVAH_UNITS_6 = dtSingleHTBill.Rows[24][5].ToString();
            sht.L25_MonYear_7 = dtSingleHTBill.Rows[24][6].ToString();
            sht.L25_KVAH_UNITS_7 = dtSingleHTBill.Rows[24][6].ToString();

            DataTable KVAchrtData_1 = new DataTable();
            KVAchrtData_1.Columns.Add("MonthYear");
            KVAchrtData_1.Columns.Add("Value", typeof(decimal));
            for (int i = 0; i < 25; i += 2)
            {
                //KVAchrtData_1.Rows.Add(new object[] { string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[24][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleHTBill.Rows[24][i]), string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[24][i + 1])) ? 0 : Convert.ToDecimal(dtSingleHTBill.Rows[24][i + 1]) });
                if (MonthYear != (string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[23][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleHTBill.Rows[23][i])))
                {
                    KVAchrtData_1.Rows.Add(new object[] { string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[23][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleHTBill.Rows[23][i]), string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[24][i + 1])) ? 0 : Convert.ToDecimal(dtSingleHTBill.Rows[24][i + 1]) });
                    MonthYear = string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[23][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleHTBill.Rows[23][i]);
                }
                else
                {
                    KVAchrtData_1.Rows.Add(new object[] { MonthYear.Replace("-", "  "), string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[24][i + 1])) ? 0 : Convert.ToDecimal(dtSingleHTBill.Rows[24][i + 1]) });
                    MonthYear = string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[23][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleHTBill.Rows[23][i]);
                }
            }
            MonthYear = "";
                sht.KVAHgrph = KVAchrtData_1;
            //Line 25 End
            #endregion


            #region Lines-26-31
            //Line 26-31 Starts
            LineNo = "26";
            sht.L26_MESSAGE1 = dtSingleHTBill.Rows[25][0].ToString();
            LineNo = "27";
            sht.L27_MESSAGE2 = dtSingleHTBill.Rows[26][0].ToString();
            LineNo = "28";
            sht.L28_MESSAGE3 = dtSingleHTBill.Rows[27][0].ToString();
            LineNo = "29";
            sht.L29_MESSAGE4 = dtSingleHTBill.Rows[28][0].ToString();
            LineNo = "30";
            sht.L30_MESSAGE5 = dtSingleHTBill.Rows[29][0].ToString();
            LineNo = "31";
            sht.L31_MESSAGE6 = dtSingleHTBill.Rows[30][0].ToString();
            //Line 26-31 End
            #endregion

            LineNo = "6";
            #region TemplateConditionalWithSTHindi
            DataView DVTemplateConditionalWithSTHindi = new DataView();
            DVTemplateConditionalWithSTHindi = TemplateConditionalWithSTHindi.DefaultView;
            DVTemplateConditionalWithSTHindi.RowFilter = "[1] = '" + sht.L6_TARIFF_DESCR + "'";

            DataTable TemplateConditionalWithSTHindiCopy = DVTemplateConditionalWithSTHindi.ToTable();
            for (int i = 0; i < TemplateConditionalWithSTHindiCopy.Rows.Count; i++)
            {
                if (sht.L6_MeasureContractDemand.ToUpper() == "KW" && Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD) >= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
                {
                    sht.L33_MESSAGE7 += TemplateConditionalWithSTHindiCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (sht.L6_MeasureContractDemand.ToUpper() == "KVA" && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD))*(0.9)) >= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD))*(0.9)) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
                {
                    sht.L33_MESSAGE7 += TemplateConditionalWithSTHindiCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (sht.L6_MeasureContractDemand.ToUpper() == "HP" && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD))*(0.746)) >= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD))*(0.746)) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
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
                if (sht.L6_MeasureContractDemand.ToUpper() == "KW" && Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
                {
                    sht.L34_MESSAGE8 += TemplateConditionalWithSTEnglishCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (sht.L6_MeasureContractDemand.ToUpper() == "KVA" && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD))*(0.9)) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD))*(0.9)) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
                {
                    sht.L34_MESSAGE8 += TemplateConditionalWithSTEnglishCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (sht.L6_MeasureContractDemand.ToUpper() == "HP" && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD))*(0.746)) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD))*(0.746)) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
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
            //Line 32 Starts
            LineNo = "32";
            if (dtSingleHTBill.Rows.Count >= 32)
                sht.L32_BarCode = dtSingleHTBill.Rows[31][0].ToString();
            //Line 32 End
            #endregion

            #region Line-37
            //Line 33-36 Starts
            LineNo = "33";
            sht.L33_ForGST = dtSingleHTBill.Rows[32][0].ToString();
            LineNo = "34";
            sht.L34_ForGST = dtSingleHTBill.Rows[33][0].ToString();
            LineNo = "35";
            sht.L35_ForGST = dtSingleHTBill.Rows[34][0].ToString();
            LineNo = "36";
            sht.L36_ForGST = dtSingleHTBill.Rows[35][0].ToString();
            //Line 33-36 end
            #endregion

            //Line 37 starts
            LineNo = "37";
            sht.L37_Last_13_months_Power_factor_for_graph = dtSingleHTBill.Rows[36][0].ToString();

            DataTable PFchrtData = new DataTable();
            PFchrtData.Columns.Add("MonthYear");
            PFchrtData.Columns.Add("Value", typeof(decimal));
            
            for (int i = 0; i <= 25; i += 2)
            {
                //PFchrtData.Rows.Add(new object[] {
                //    string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[36][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleHTBill.Rows[36][i]),
                //    string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[36][i + 1])) ? 0 : Convert.ToDecimal(dtSingleHTBill.Rows[36][i + 1]) });
                if (MonthYear != (string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[23][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleHTBill.Rows[23][i])))
                {
                    PFchrtData.Rows.Add(new object[] {string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[23][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleHTBill.Rows[23][i]), string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[36][i + 1])) ? 0 : Convert.ToDecimal(dtSingleHTBill.Rows[36][i + 1]) });
                    MonthYear = string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[23][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleHTBill.Rows[23][i]);
                }
                else
                {
                    PFchrtData.Rows.Add(new object[] { MonthYear.Replace("-","  "), string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[36][i + 1])) ? 0 : Convert.ToDecimal(dtSingleHTBill.Rows[36][i + 1]) });
                    MonthYear = string.IsNullOrEmpty(Convert.ToString(dtSingleHTBill.Rows[23][i])) ? ((i / 2) + 1).ToString() : Convert.ToString(dtSingleHTBill.Rows[23][i]);
                }
            }
            MonthYear = "";
            sht.PFgrph = PFchrtData;

            Console.WriteLine("HT Line 37 parsed");
            //Line 37 end
            #endregion


            #region Custom Fields
            var meter = sht.L11_MTRSNO_2_IF_AVAILABLE.Trim() != "" ? sht.L11_MTRSNO_2_IF_AVAILABLE : sht.L11_MTRSNO_1;
            sht.TopPanel_Row_1 = sht.L1_MonthYear + " / " + sht.L1_Zone + " / " + sht.L1_BU + " / " + sht.L1_Route + "/ / " + sht.L1_Bill_seq_no;
            sht.TopPanel_Row_2 = "Meter No. : " + meter;
            sht.TopPanel_Row_3 = "T No. : " + sht.L8_TNo;
            sht.TopPanel_Row_4 = "Bill Date : " + sht.L7_BillDt;
            if (String.Equals(sht.L1_TODOrNon_TODFlag, "1"))
            {
                sht.TopPanel_Row_5 = "Bill Days : " + sht.L10_Mode;
                sht.TopPanel_Row_6 = "11 KV FEEDER : " + sht.L1_FeederName;
            }
            else
                sht.TopPanel_Row_5 = "11 KV FEEDER : " + sht.L1_FeederName;


            //sht.L9_CurrBillAmtIntTplArrsTpl = Math.Ceiling(Convert.ToDecimal(dtSingleHTBill.Rows[8][4].ToString())).ToString();
            //sht.L8_AmountPayableBeforeDueDate = Math.Ceiling(Convert.ToDecimal(dtSingleHTBill.Rows[7][10].ToString())).ToString();
            //sht.L8_AmountPayableBeforeDueDate = dtSingleHTBill.Rows[7][10].ToString();
            //sht.L8_ParkingAmount = Math.Ceiling(Convert.ToDecimal(string.IsNullOrEmpty(dtSingleHTBill.Rows[7][12].ToString()) ? "0" : dtSingleHTBill.Rows[7][12].ToString())).ToString();
            

            Console.WriteLine("Custom Fields calculated");




            #endregion

            return sht;
        }

        PaperSourceCollection printerSources;
        void NonTOD_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            e.PrintDocument.DefaultPageSettings.PaperSource = e.PrintDocument.PrinterSettings.PaperSources[cbNonTODTraySource.SelectedIndex];
            // e.PrintDocument.DefaultPageSettings.PrinterResolution = e.PrintDocument.PrinterSettings.PrinterResolutions[i];
            e.PrintDocument.PrintPage += PrintDocument_PrintPage;
            if (e.PrintDocument.PrinterSettings.CanDuplex)
                e.PrintDocument.PrinterSettings.Duplex = Duplex.Default;
            printerSources = e.PrintDocument.PrinterSettings.PaperSources;
        }

        void sap_print(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            e.PrintDocument.DefaultPageSettings.PaperSource = e.PrintDocument.PrinterSettings.PaperSources[cbSeparatorTraySource.SelectedIndex];
            //e.PrintDocument.DefaultPageSettings.PrinterResolution = e.PrintDocument.PrinterSettings.PrinterResolutions[i];
            e.PrintDocument.PrintPage += PrintDocument_PrintPage;
            if (e.PrintDocument.PrinterSettings.CanDuplex)
                e.PrintDocument.PrinterSettings.Duplex = Duplex.Vertical;
        }
        void TOD_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            e.PrintDocument.DefaultPageSettings.PaperSource = e.PrintDocument.PrinterSettings.PaperSources[cbTODTraySource.SelectedIndex];
            //  e.PrintDocument.DefaultPageSettings.PrinterResolution = e.PrintDocument.PrinterSettings.PrinterResolutions[i];
            e.PrintDocument.PrintPage += PrintDocument_PrintPage;
            if (e.PrintDocument.PrinterSettings.CanDuplex)
                e.PrintDocument.PrinterSettings.Duplex = Duplex.Default;
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
                    XtraMessageBox.Show("Total Bill in this file " + singleHTBills.Length.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AppFunctions.ShowWaitForm("Generating Bill..!!");

                    //StartPrinting_HTBills(singleHTBills, sb.Name);
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

        private bool ValidatetxtFile(string[] Bills)
        {
            try
            {
                int BillNo = 0;
                DSBill.Dispose();
                DSBill.Reset();
                foreach (var Bill in Bills)
                {
                    BillNo++;
                    //DataTable dtSingleHTBill = ParseAsDataTable.HT_FileTxtToDataTable(Bill, BillNo, "HT");
                    DSBill.Tables.Add(ParseAsDataTable.HT_FileTxtToDataTable(Bill, BillNo, "HT"));
                    //if (dtSingleHTBill.Rows.Count == 37)
                    if (DSBill.Tables[BillNo - 1].Rows.Count == 37)
                    {
                        SingleHTBill sht = parseSingleHTBill(DSBill.Tables[BillNo - 1]);
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
