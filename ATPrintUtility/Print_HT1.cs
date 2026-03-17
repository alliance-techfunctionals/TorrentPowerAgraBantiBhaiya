using AT.Print.PDF;
using AT.Print.Utils;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
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
        List<SingleHTBill> parsedBills = new List<SingleHTBill>();
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
                        var validatedBills = ValidatetxtFile(singleHTBills);
                        if (validatedBills != null)
                        {
                                StartPrinting_HTBills(validatedBills, sb.Name);

                          
                        }
                        else
                        {
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

        

        private void StartPrinting_HTBills(List<SingleHTBill> bills, string Name)
        {
            string LotNo = "InitialLot";
            string LotNoCopy = "InitialLot";
            string TOD_NonTODFlag = "";
            int BillNo = 1, Counter = 1, ParsedBills = 0;
            //DataTable dtSingleHTBill = new DataTable();
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

            foreach (var bill in bills)
            {
                try
                {
                    AppFunctions.ShowWaitForm("Generating Bill..!!");
                    List<SingleHTBill> lstformattedbills = new List<SingleHTBill>();
                    lstformattedbills.Add(bill);

                    //dtSingleHTBill = ParseAsDataTable.HT_FileTxtToDataTable(Bill);

                    //if ((LotNoCopy != dtSingleHTBill.Rows[0][4].ToString().Trim() || Counter == 51 || TOD_NonTODFlag != dtSingleHTBill.Rows[0][10].ToString().Trim()) && LotNoCopy != "InitialLot" && TOD_NonTODFlag != "")
                    if ((LotNoCopy != bill.Sap_LotNo || Counter == 51 || TOD_NonTODFlag != bill.L1_TODOrNon_TODFlag)
               && LotNoCopy != "InitialLot" && TOD_NonTODFlag != "")
                    
                        {
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


                        //if (LotNo != dtSingleHTBill.Rows[0][4].ToString().Trim())
                        if (LotNo != bill.Sap_LotNo)
                        {
                            if (Name != "sbSavePDF")
                            {
                                LotNo = bill.Sap_LotNo; //(String)dtSingleHTBill.Rows[0][4];
                                LotNoCopy = bill.Sap_LotNo;//dtSingleHTBill.Rows[0][4].ToString().Trim();
                                SingleHTBill billSaprator = new SingleHTBill();
                                billSaprator.Sap_Zone = "Zone No. " + bill.Sap_Zone;// dtSingleHTBill.Rows[0][1];
                                billSaprator.Sap_LotNo = "LOT No. " + bill.Sap_LotNo;  //dtSingleHTBill.Rows[0][4];
                                billSaprator.Sap_GrpNo = "Group No. " + bill.Sap_GrpNo;  //dtSingleHTBill.Rows[0][2];
                                lstformattedbills.Add(billSaprator);
                                Rpt_Saprator sap_rpt = new Rpt_Saprator
                                {
                                    DataSource = lstformattedbills
                                };
                                {
                                    sap_rpt.CreateDocument();
                                    sap_rpt.ShowPrintMarginsWarning = false;
                                    sap_rpt.PrinterName = cbDefaultPrinter.Text;
                                    sap_rpt.PrintingSystem.StartPrint += sap_print;
                                    sap_rpt.Print(cbDefaultPrinter.Text);
                                }
                                lstformattedbills.Clear();
                            }
                        }

                        // SingleHTBill slt = parseSingleHTBill(dtSingleHTBill);

                        TOD_NonTODFlag = bill.L1_TODOrNon_TODFlag;

                        //slt.MVPicture = mVImagePath;
                        //lstformattedbills.Add(slt);


                        bool isPDF = Name == "sbSavePDF";
                        // bool isPDF = Name == "sbSavePDF";
                        bool isPrint = Name == "sbPrintBill";

                        bill.MVPicture = mVImagePath;
                        lstformattedbills.Clear();
                        lstformattedbills.Add(bill);

                        Rpt_HT_TodPDF rpt = new Rpt_HT_TodPDF
                        {
                            DataSource = lstformattedbills,
                            DisplayName = bill.L6_SERVDET_SERVNO
                        };

                        var frontWatermark = GetWatermark("Duplex_Non_TOD_Front_Page.png");
                        var backWatermark = GetWatermark("Duplex_Non_TOD_Back_Page.png");

                        #region PDF CASE

                        if (isPDF)
                        {
                            rpt.Watermark.CopyFrom(frontWatermark);
                            rpt.CreateDocument(false);

                            AT.Print.PDF.rpt_HT_Back rptBack = new AT.Print.PDF.rpt_HT_Back
                            {
                                DataSource = lstformattedbills
                            };

                            rptBack.Watermark.CopyFrom(backWatermark);
                            rptBack.CreateDocument(false);

                            rpt.ModifyDocument(x => x.AddPages(rptBack.Pages));

                            if (rpt.Pages.Count > 1)
                                rpt.Pages[1].AssignWatermark(backWatermark);

                            string billdate = lstformattedbills.FirstOrDefault()?.L1_MonthYear;
                            string serviceNo = lstformattedbills.FirstOrDefault()?.L6_SERVDET_SERVNO;

                            OutputFolderPath OFP = new OutputFolderPath();
                            string outputFolder = OFP.LoadLocation() +
                                                  "//HTFiles//" + billdate + "//" + textFileName;

                            if (!Directory.Exists(outputFolder))
                                Directory.CreateDirectory(outputFolder);

                            rpt.ExportToPdf(outputFolder + "//" + serviceNo + ".pdf");

                            ParsedBills++;
                            AppFunctions.CloseWaitForm();
                        }

                        #endregion

                        #region PRINT CASE

                        else if (isPrint)
                        {
                            rpt.Watermark.ImageTransparency = 255;
                            rpt.PrinterName = cbDefaultPrinter.SelectedItem.ToString();
                            rpt.PrintingSystem.Document.Name = bill.L6_SERVDET_SERVNO;

                            rpt.CreateDocument();

                            AT.Print.PDF.rpt_HT_Back rptBack = new AT.Print.PDF.rpt_HT_Back
                            {
                                DataSource = lstformattedbills
                            };

                            rptBack.CreateDocument();
                            rpt.ModifyDocument(x => x.AddPages(rptBack.Pages));
                            collectorReport.PrintingSystem.StartPrint -= NonTOD_StartPrint;
                            collectorReport.PrintingSystem.StartPrint -= TOD_StartPrint;

                            if (TOD_NonTODFlag == "0")
                                collectorReport.PrintingSystem.StartPrint += NonTOD_StartPrint;
                            else
                                collectorReport.PrintingSystem.StartPrint += TOD_StartPrint;

                            collectorReport.Pages.AddRange(rpt.Pages);


                            if (bills.Count() == BillNo && LotNoCopy != "InitialLot")
                            {
                                collectorReport.Print(cbDefaultPrinter.Text);
                                collectorReport.Pages.Clear();
                            }

                            ParsedBills++;
                            AppFunctions.CloseWaitForm();
                        }

                        #endregion

                        #region SAFETY ELSE

                        else
                        {
                            AppFunctions.CloseWaitForm();

                            XtraMessageBox.Show(
                                "Unknown operation type for Bill: " + bill.L6_SERVDET_SERVNO,
                                Application.ProductName,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                            Console.WriteLine(
                                "Unknown operation type for Bill: " + bill.L6_SERVDET_SERVNO);
                        }

                        #endregion


                    }


                catch (System.OutOfMemoryException)
                {
                    AppFunctions.LogError("Error Parsing Service No. " + ServiceNo + " of the given file due to out of memory.");
                    AppFunctions.LogProcessedBill(Convert.ToString(bill.Sap_Zone), Convert.ToString(bill.Sap_LotNo), Convert.ToString(bill.Sap_GrpNo), Convert.ToString(bill.L6_SERVDET_SERVNO), ServiceNo, FileName, "No");
                    System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                    GC.Collect();
                    GC.RemoveMemoryPressure(1024 * 1024);
                    break;
                }
                catch (Exception ex)
                {
                    AppFunctions.LogError(ex);
                    AppFunctions.LogProcessedBill(Convert.ToString(bill.Sap_Zone), Convert.ToString(bill.Sap_LotNo), Convert.ToString(bill.Sap_GrpNo), Convert.ToString(bill.L6_SERVDET_SERVNO), ServiceNo, FileName, "No");
                    AppFunctions.CloseWaitForm();
                    break;
                }
                AppFunctions.LogProcessedBill(Convert.ToString(bill.Sap_Zone), Convert.ToString(bill.Sap_LotNo), Convert.ToString(bill.Sap_GrpNo), Convert.ToString(bill.L6_SERVDET_SERVNO), ServiceNo, FileName, "Yes");

                BillNo++;
            }

            XtraMessageBox.Show(ParsedBills + " Bills Parsed Successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            e.PrintDocument.DefaultPageSettings.PaperSource = e.PrintDocument.PrinterSettings.PaperSources[cbSeparatorTraySource.SelectedIndex];
            e.PrintDocument.PrintPage += PrintDocument_PrintPage;
            if (e.PrintDocument.PrinterSettings.CanDuplex)
                e.PrintDocument.PrinterSettings.Duplex = Duplex.Vertical;
        }
        void TOD_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            e.PrintDocument.DefaultPageSettings.PaperSource = e.PrintDocument.PrinterSettings.PaperSources[cbTODTraySource.SelectedIndex];
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

        private List<SingleHTBill> ValidatetxtFile(string[] Bills)  // need to work on it
        {
            var validatedBills = new List<SingleHTBill>();

            try
            {
                int billNo = 0;


                foreach (var billContent in Bills)
                {
                    billNo++;

                    if (!ValidateHTBill(billContent, billNo))
                        return null;


                    var bill = ParseSingleHTBill_Optimized(billContent);
                    if (bill == null)
                    {
                        XtraMessageBox.Show($"Parsing failed for Bill No: {billNo}");
                        return null;
                    }
                    validatedBills.Add(bill);

                }

                return validatedBills;
            }
            catch (Exception ex)
            {
                AppFunctions.CloseWaitForm();
                AppFunctions.LogError(ex);
                XtraMessageBox.Show(ex.Message.Replace('.', ' ') + "in txt file for Service no:" + ServiceNo + " and Line No." + LineNo);
                return null;
            }
        }

        private bool ValidateHTBill(string billText, int BillNo) //Done
        {
            try
            {
                string[] lines = billText.Split('\n');
                string DoubleMeterValue = "";
                decimal value;

                int totalLines = lines.Length - 1;

                if (totalLines != 37)
                {
                    XtraMessageBox.Show($"Bill No: {BillNo} has not 37 rows.");
                    return false;
                }

                string serviceNo = "";
                if (lines.Length > 5)
                {
                    var parts = lines[5].Split('|');
                    if (parts.Length > 0)
                        serviceNo = parts[0];
                }

                for (int i = 0; i < totalLines; i++)
                {
                    string[] fields = lines[i].Split('|');

                    switch (i + 1)
                    {
                        case 1:

                            if (fields.Length != 12)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row 1 has {fields.Length} columns only.");
                                return false;
                            }

                            if (fields[7] == "" || (fields[7] != "0" && fields[7] != "1"))
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} row 1 column 8th is blank or value differ from 0 or 1");
                                return false;
                            }

                            if (fields[9] == "" || (fields[9] != "0" && fields[9] != "1"))
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} row 1 column 10th is either blank or value differ from 0 or 1");
                                return false;
                            }

                            if (fields[10] == "")
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} row 1 column 11th blank.");
                                return false;
                            }

                            break;

                        case 2:
                        case 3:
                        case 4:
                        case 5:

                            if (fields.Length != 1)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row {i + 1} has {fields.Length} columns only.");
                                return false;
                            }

                            break;

                        case 6:

                            if (fields.Length != 13)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row 6 has {fields.Length} columns only.");
                                return false;
                            }

                            break;

                        case 7:

                            if (fields.Length != 7)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row 7 has {fields.Length} columns only.");
                                return false;
                            }

                            break;

                        case 8:

                            if (fields.Length != 16)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row 8 has {fields.Length} columns only.");
                                return false;
                            }

                            break;

                        case 9:

                            if (fields.Length != 6)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row 9 has {fields.Length} columns only.");
                                return false;
                            }

                            break;

                        case 10:

                            if (fields.Length != 9)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row 10 has {fields.Length} columns only.");
                                return false;
                            }

                            break;

                        case 11:

                            if (fields.Length != 2)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row 11 has {fields.Length} columns only.");
                                return false;
                            }

                            DoubleMeterValue = fields[1].Trim();

                            break;

                        case 12:
                        case 13:
                        case 14:
                        case 15:

                            if (fields.Length != 4)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row {i + 1} has {fields.Length} columns only.");
                                return false;
                            }

                            break;

                        case 16:
                        case 17:

                            if (fields.Length != 5)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row {i + 1} has {fields.Length} columns only.");
                                return false;
                            }

                            break;

                        case 18:
                        case 19:

                            if (DoubleMeterValue == "" && fields.Length != 4)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row {i + 1} has {fields.Length} columns only.");
                                return false;
                            }
                            else if (DoubleMeterValue != "" && fields.Length != 3)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row {i + 1} has {fields.Length} columns only.");
                                return false;
                            }

                            break;

                        case 20:
                        case 21:

                            if (fields.Length != 4)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row {i + 1} has {fields.Length} columns only.");
                                return false;
                            }

                            break;

                        case 22:
                        case 23:

                            if (fields.Length != 5)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row {i + 1} has {fields.Length} columns only.");
                                return false;
                            }

                            break;

                        case 24:
                        case 25:

                            if (fields.Length != 26)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row {i + 1} has {fields.Length} columns only.");
                                return false;
                            }

                            break;

                        case 26:
                        case 27:
                        case 28:
                        case 29:
                        case 30:
                        case 31:
                        case 32:

                            if (fields.Length != 1)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row {i + 1} has {fields.Length} columns only.");
                                return false;
                            }

                            break;

                        case 33:

                            if (fields.Length != 6)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row 33 has {fields.Length} columns only.");
                                return false;
                            }

                            break;

                        case 34:
                        case 35:

                            if (fields.Length != 21)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row {i + 1} has {fields.Length} columns only.");
                                return false;
                            }

                            break;

                        case 36:

                            if (fields.Length != 4)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row 36 has {fields.Length} columns only.");
                                return false;
                            }

                            break;

                        case 37:

                            for (int c = 0; c < fields.Length - 1; c += 2)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(fields[c + 1])) &&
                                    !Decimal.TryParse(fields[c + 1], out value))
                                {
                                    AppFunctions.CloseWaitForm();
                                    XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} row 37 has string value for chart inspite of numeric value on " + (c + 1) + "  seprator.");
                                    return false;
                                }
                            }

                            if (fields.Length < 26)
                            {
                                AppFunctions.CloseWaitForm();
                                XtraMessageBox.Show($"Bill No: {BillNo}, Service No. {serviceNo} and row 37 has {fields.Length} columns only.");
                                return false;
                            }

                            break;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                AppFunctions.LogError(ex);
                XtraMessageBox.Show("Problem occur while Validating txt file!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }



        SingleHTBill ParseSingleHTBill_Optimized(string billText)  // need to check
        {

            if (string.IsNullOrWhiteSpace(billText))
                return null;

            var lines = billText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            var rows = lines.Select(l => l.Split('|')).ToArray();

            SingleHTBill sht = new SingleHTBill();



            #region --Lines
            #region Line-1
            ServiceNo = GetCol(rows, 5, 0);  //check krna h 
            LineNo = "1";
            sht.L1_BillType = "HT";
            sht.L1_MonthYear = GetCol(rows, 0, 0);
            sht.L1_Zone = GetCol(rows, 0, 1);
            sht.L1_BU = GetCol(rows, 0, 2);
            sht.L1_PC = GetCol(rows, 0, 3);
            sht.L1_Route = GetCol(rows, 0, 4);
            sht.L1_Bill_seq_no = GetCol(rows, 0, 5);
            sht.L1_FeederName = GetCol(rows, 0, 6);
            sht.L1_TODOrNon_TODFlag = GetCol(rows, 0, 7);
            sht.L1_AKY_indicator = GetCol(rows, 0, 8);
            sht.L1_DisconnectionMSGPrintingIMMEDIATE = GetCol(rows, 0, 9);
            sht.L1_BillingCode = GetCol(rows, 0, 10);
            if (GetCol(rows, 0, 11) == "")
            {
                sht.L1_Customer_PAN = GetCol(rows, 0, 11);
            }
            else
            {
                sht.L1_Customer_PAN = "PAN: " + GetCol(rows, 0, 11);
            }
            #endregion

            #region Line-2
            LineNo = "2";
            sht.L2_Name = GetCol(rows, 1, 0).Trim('�');
            #endregion

            #region Line-3
            LineNo = "3";
            sht.L3_Addr1 = GetCol(rows, 2, 0).Trim('�');
            #endregion

            #region Line-4
            LineNo = "4";
            sht.L4_Addr2 = GetCol(rows, 3, 0).Trim('�');
            #endregion

            #region Line-5
            LineNo = "5";
            sht.L5_Addr3 = GetCol(rows, 4, 0).Trim('�');
            #endregion

            #region Line-6
            LineNo = "6";
            sht.L6_MeasureContractDemand = GetCol(rows, 5, 10);
            sht.L6_SERVDET_SERVNO = GetCol(rows, 5, 0);
            sht.L6_SERVDET_SANC_LOAD = string.IsNullOrEmpty(GetCol(rows, 5, 1)) ? "" : GetCol(rows, 5, 1);
            sht.L6_bill_demand = string.IsNullOrEmpty(GetCol(rows, 5, 2)) ? "" : GetCol(rows, 5, 2);
            sht.L6_ACTUAL_DEMAND = string.IsNullOrEmpty(GetCol(rows, 5, 3)) ? "" : GetCol(rows, 5, 3);
            sht.L6_TARIFF_DESCR = string.IsNullOrEmpty(GetCol(rows, 5, 4)) ? "" : GetCol(rows, 5, 4);
            sht.L6_EXCESS_DEMAND = string.IsNullOrEmpty(GetCol(rows, 5, 5)) ? "" : GetCol(rows, 5, 5);
            sht.L6_SUPPLY_VOLTAGE = GetCol(rows, 5, 6);
            sht.L6_Avg_Power_Factor = GetCol(rows, 5, 7);
            sht.L6_MTRDET_LF_PERC = GetCol(rows, 5, 8);
            sht.L6_BILL_TYPE = GetCol(rows, 5, 9);
            sht.L6_MeasureContractDemand = GetCol(rows, 5, 10);
            sht.L6_Kvah_Indicator = GetCol(rows, 5, 11);
            sht.L6_LT_Metering_Flag = GetCol(rows, 5, 12);
            #endregion

            #region Line-7
            LineNo = "7";
            sht.L7_Due_Date = GetCol(rows, 6, 0);
            sht.L7_BillDt = GetCol(rows, 6, 1);
            int YYYY, MM, DD;
            YYYY = int.Parse(GetCol(rows, 6, 2).Split('-')[2]);
            MM = int.Parse(GetCol(rows, 6, 2).Split('-')[1]);
            DD = int.Parse(GetCol(rows, 6, 2).Split('-')[0]);
            DateTime PreviousDate = new DateTime(YYYY, MM, DD);
            sht.L7_PrevReadDt = (PreviousDate.AddDays(-1)).ToString("dd-MM-yy");
            sht.L7_ReaDt = GetCol(rows, 6, 3);
            sht.L7_LastPymtDate = GetCol(rows, 6, 4);
            sht.L7_LastPayementAmount = GetCol(rows, 6, 5).Trim('�');
            sht.L7_LastPayementMode = GetCol(rows, 6, 6);
            #endregion

            #region Line-8
            LineNo = "8";
            sht.L8_FixedCharge = GetCol(rows, 7, 0);
            sht.L8_EnergyCharge = GetCol(rows, 7, 1);
            sht.L8_TODCharges = GetCol(rows, 7, 2);
            sht.L8_TODCharges = sht.L8_TODCharges.Contains("-") ? ("-" + sht.L8_TODCharges.Replace("-", "")) : sht.L8_TODCharges;
            sht.L8_ACCharge = GetCol(rows, 7, 3);
            sht.L8_GovTax = GetCol(rows, 7, 4);
            sht.L8_MinCharge = GetCol(rows, 7, 5);
            sht.L8_ServdetTotbBdtOthr = GetCol(rows, 7, 6);
            sht.L8_RegulatoryCharge_1 = GetCol(rows, 7, 7);
            sht.L8_RegulatoryCharge_2 = GetCol(rows, 7, 8);
            sht.L8_RebateIncurredCurrentMonth = GetCol(rows, 7, 9);
            sht.L8_AmountPayableBeforeDueDate = GetCol(rows, 7, 10);
            sht.L8_AmountPayableBeforeDueDate = sht.L8_AmountPayableBeforeDueDate.Contains("CR") ? ("-" + sht.L8_AmountPayableBeforeDueDate.Replace("CR", "")) : (sht.L8_AmountPayableBeforeDueDate.Contains("-") ? ("-" + sht.L8_AmountPayableBeforeDueDate.Replace("-", "")) : sht.L8_AmountPayableBeforeDueDate);
            sht.L8_TNo = GetCol(rows, 7, 11).Trim('�');
            sht.L8_ParkingAmount = Math.Ceiling(Convert.ToDecimal(string.IsNullOrEmpty(GetCol(rows, 7, 12)) ? "0" : GetCol(rows, 7, 12))).ToString();
            sht.L8_Subsidy_Charges = GetCol(rows, 7, 13);

            sht.L8_Solar_Export_Energy = GetCol(rows, 7, 14);
            sht.L8_GreenTariff_Charges = GetCol(rows, 7, 15);
            #endregion

            #region Line-9
            LineNo = "9";
            sht.L9_TotDbArr = GetCol(rows, 8, 0);
            sht.L9_CurrBillAmt = GetCol(rows, 8, 1);
            sht.L9_CurrBillAmt = sht.L9_CurrBillAmt.Contains("-") ? ("-" + sht.L9_CurrBillAmt.Replace("-", "")) : sht.L9_CurrBillAmt;
            sht.L9_Int_Tpl = GetCol(rows, 8, 2);
            sht.L9_ArrsTpl = GetCol(rows, 8, 3);
            sht.L9_CurrBillAmtIntTplArrsTpl = GetCol(rows, 8, 4);
            sht.L9_Amount_Payable = GetCol(rows, 8, 5);

            if (Convert.ToDouble(GetCol(rows, 8, 5)) < 0)
            {
                sht.L9_Amount_Payable = "NOT TO PAY";
            }
            #endregion

            #region Line-10
            LineNo = "10";
            sht.L10_LFincentive = GetCol(rows, 9, 0);
            sht.L10_DisconnDate = GetCol(rows, 9, 1);
            sht.L10_TotArrUPPCLIntUPPCLIntArrUPPCL = GetCol(rows, 9, 2);
            sht.L10_TotArrUPPCLIntUPPCLIntArrUPPCL_Rounded = string.IsNullOrEmpty(GetCol(rows, 9, 2)) ? "0" : Math.Round(Convert.ToDecimal(GetCol(rows, 9, 2)) + (decimal).49, 0, MidpointRounding.AwayFromZero).ToString();
            sht.L10_SecDeptBdt = GetCol(rows, 9, 3);
            sht.L10_DmdChgPenalty = GetCol(rows, 9, 4);
            sht.L10_UPPCL_ArrearAmount = GetCol(rows, 9, 5);
            sht.L10_UPPCLIntOnArrearAmount = GetCol(rows, 9, 6);
            sht.L10_Mode = GetCol(rows, 9, 7);
            //sht.L10_TheftAmount= GetCol(rows[9][8].ToString();
            sht.L10_FPPASurcharge = GetCol(rows, 9, 8);
            #endregion

            #region Line-11
            LineNo = "11";
            sht.L11_MTRSNO_1 = GetCol(rows, 10, 0);
            sht.L11_MTRSNO_2_IF_AVAILABLE = GetCol(rows, 10, 1);
            #endregion

            #region Line-12-16
            LineNo = "12";
            sht.L12_KWH_PRESREAD = GetCol(rows, 11, 0);
            sht.L12_KVAH_PRESREAD = GetCol(rows, 11, 1);
            sht.L12_KVA_PRESREAD = GetCol(rows, 11, 2);
            LineNo = "13";
            sht.L13_KWH_PASTREAD = GetCol(rows, 12, 0);
            sht.L13_KVAH_PASTREAD = GetCol(rows, 12, 1); ;
            sht.L13_KVA_PASTREAD = GetCol(rows, 12, 2);
            sht.L13_Purpose = GetCol(rows, 12, 3);
            LineNo = "14";
            sht.L14_Multiplying_factor_KWH = GetCol(rows, 13, 0);
            sht.L14_Multiplying_factor_KVAH = GetCol(rows, 13, 1); ;
            sht.L14_Multiplying_factor_KVA = GetCol(rows, 13, 2);

            LineNo = "15";
            sht.L15_KWH_UNITS = GetCol(rows, 14, 0);
            sht.L15_KVAH_UNITS = GetCol(rows, 14, 1);
            sht.L15_KVA_UNITS = GetCol(rows, 14, 2);
            LineNo = "16";
            sht.L16_TOD1_KVAH_Units = GetCol(rows, 15, 0);
            sht.L16_TOD2_KVAH_Units = GetCol(rows, 15, 1);
            sht.L16_TOD3_KVAH_Units = GetCol(rows, 15, 2);
            sht.L16_TOD4_KVAH_Units = GetCol(rows, 15, 3);
            #endregion

            #region Line-17
            LineNo = "17";
            sht.L17_TOD1_KVA_Units = GetCol(rows, 16, 0);
            sht.L17_TOD2_KVA_Units = GetCol(rows, 16, 1);
            sht.L17_TOD3_KVA_Units = GetCol(rows, 16, 2);
            sht.L17_TOD4_KVA_Units = GetCol(rows, 16, 3);
            #endregion

            #region Line-18
            LineNo = "18";
            sht.L18_KWH_PRESREAD = GetCol(rows, 17, 0);
            sht.L18_KVAH_PRESREAD = GetCol(rows, 17, 1);
            sht.L18_KVA_PRESREAD = GetCol(rows, 17, 2);
            #endregion

            #region Line-19
            LineNo = "19";
            sht.L19_KWH_PASTREAD = GetCol(rows, 18, 0);
            sht.L19_KVAH_PASTREAD = GetCol(rows, 18, 1);
            sht.L19_KVA_PASTREAD = GetCol(rows, 18, 2);
            #endregion

            #region Line-20
            LineNo = "20";
            sht.L20_Multiplying_Factor_KWH = GetCol(rows, 19, 0);
            sht.L20_Multiplying_Factor_KVAH = GetCol(rows, 19, 1);
            sht.L20_Multiplying_Factor_KVA = GetCol(rows, 19, 2);
            #endregion

            #region Line-21
            LineNo = "21";
            sht.L21_KWH_UNITS = GetCol(rows, 20, 0);
            sht.L21_KVAH_UNITS = GetCol(rows, 20, 1); ;
            sht.L21_KVA_UNITS = GetCol(rows, 20, 2);

            #endregion




            #region Line-22
            LineNo = "22";
            sht.L22_TOD1_KVAH_Units = GetCol(rows, 21, 0);
            sht.L22_TOD2_KVAH_Units = GetCol(rows, 21, 1);
            sht.L22_TOD3_KVAH_Units = GetCol(rows, 21, 2);
            sht.L22_TOD4_KVAH_Units = GetCol(rows, 21, 3);
            #endregion

            #region Line-23
            LineNo = "23";
            sht.L23_TOD1_KVA_Units = GetCol(rows, 22, 0);
            sht.L23_TOD2_KVA_Units = GetCol(rows, 22, 1);
            sht.L23_TOD3_KVA_Units = GetCol(rows, 22, 2);
            sht.L23_TOD4_KVA_Units = GetCol(rows, 22, 3);

            #endregion

            #region Line-24
            LineNo = "24";
            sht.L24_MonYear_1 = GetCol(rows, 23, 0);
            sht.L24_KVA_UNITS_1 = GetCol(rows, 23, 0);
            sht.L24_MonYear_2 = GetCol(rows, 23, 1);
            sht.L24_KVA_UNITS_2 = GetCol(rows, 23, 1);
            sht.L24_MonYear_3 = GetCol(rows, 23, 2);
            sht.L24_KVA_UNITS_3 = GetCol(rows, 23, 2);
            sht.L24_MonYear_4 = GetCol(rows, 23, 3);
            sht.L24_KVA_UNITS_4 = GetCol(rows, 23, 3);
            sht.L24_MonYear_5 = GetCol(rows, 23, 4);
            sht.L24_KVA_UNITS_5 = GetCol(rows, 23, 4);
            sht.L24_MonYear_6 = GetCol(rows, 23, 5);
            sht.L24_KVA_UNITS_6 = GetCol(rows, 23, 5);
            sht.L24_MonYear_7 = GetCol(rows, 23, 6);
            sht.L24_KVA_UNITS_7 = GetCol(rows, 23, 6);

            DataTable KVAchrtData = new DataTable();
            KVAchrtData.Columns.Add("MonthYear");
            KVAchrtData.Columns.Add("Value", typeof(decimal));
            for (int i = 0; i < 25; i += 2)
            {
                if (MonthYear != (string.IsNullOrEmpty(GetCol(rows, 23, i)) ? ((i / 2) + 1).ToString() : GetCol(rows, 23, i)))
                {
                    KVAchrtData.Rows.Add(new object[] { string.IsNullOrEmpty(GetCol(rows, 23, i)) ? ((i / 2) + 1).ToString() : GetCol(rows, 23, i), string.IsNullOrEmpty(GetCol(rows, 23, (i + 1))) ? 0 : Convert.ToDecimal(GetCol(rows, 23, (i + 1))) });
                    MonthYear = string.IsNullOrEmpty(GetCol(rows, 23, i)) ? ((i / 2) + 1).ToString() : GetCol(rows, 23, i);
                }
                else
                {
                    KVAchrtData.Rows.Add(new object[] { MonthYear.Replace("-", "  "), string.IsNullOrEmpty(GetCol(rows, 23, (i + 1))) ? 0 : Convert.ToDecimal(GetCol(rows, 23, (i + 1))) });
                    MonthYear = string.IsNullOrEmpty(GetCol(rows, 23, i)) ? ((i / 2) + 1).ToString() : GetCol(rows, 23, i);
                }
            }
            MonthYear = "";
            sht.KVAgrph = KVAchrtData;


            #endregion

            #region Line-25
            LineNo = "25";
            sht.L25_MonYear_1 = GetCol(rows, 24, 0);
            sht.L25_KVAH_UNITS_1 = GetCol(rows, 24, 0);
            sht.L25_MonYear_2 = GetCol(rows, 24, 1);
            sht.L25_KVAH_UNITS_2 = GetCol(rows, 24, 1);
            sht.L25_MonYear_3 = GetCol(rows, 24, 2);
            sht.L25_KVAH_UNITS_3 = GetCol(rows, 24, 2);
            sht.L25_MonYear_4 = GetCol(rows, 24, 3);
            sht.L25_KVAH_UNITS_4 = GetCol(rows, 24, 3);
            sht.L25_MonYear_5 = GetCol(rows, 24, 4);
            sht.L25_KVAH_UNITS_5 = GetCol(rows, 24, 4);
            sht.L25_MonYear_6 = GetCol(rows, 24, 5);
            sht.L25_KVAH_UNITS_6 = GetCol(rows, 24, 5);
            sht.L25_MonYear_7 = GetCol(rows, 24, 6);
            sht.L25_KVAH_UNITS_7 = GetCol(rows, 24, 6);

            DataTable KVAchrtData_1 = new DataTable();
            KVAchrtData_1.Columns.Add("MonthYear");
            KVAchrtData_1.Columns.Add("Value", typeof(decimal));
            for (int i = 0; i < 25; i += 2)
            {
                if (MonthYear != (string.IsNullOrEmpty(GetCol(rows, 23, i)) ? ((i / 2) + 1).ToString() : GetCol(rows, 23, i)))
                {
                    KVAchrtData_1.Rows.Add(new object[] { string.IsNullOrEmpty(GetCol(rows, 23, i)) ? ((i / 2) + 1).ToString() : GetCol(rows, 23, i), string.IsNullOrEmpty(GetCol(rows, 24, (i + 1))) ? 0 : Convert.ToDecimal(GetCol(rows, 24, (i + 1))) });
                    MonthYear = string.IsNullOrEmpty(GetCol(rows, 23, i)) ? ((i / 2) + 1).ToString() : GetCol(rows, 23, i);
                }
                else
                {
                    KVAchrtData_1.Rows.Add(new object[] { MonthYear.Replace("-", "  "), string.IsNullOrEmpty(GetCol(rows, 24, (i + 1))) ? 0 : Convert.ToDecimal(GetCol(rows, 24, (i + 1))) });
                    MonthYear = string.IsNullOrEmpty(GetCol(rows, 23, i)) ? ((i / 2) + 1).ToString() : GetCol(rows, 23, i);
                }
            }
            MonthYear = "";
            sht.KVAHgrph = KVAchrtData_1;
            #endregion


            #region Lines-26-31
            LineNo = "26";
            sht.L26_MESSAGE1 = GetCol(rows, 25, 0);
            LineNo = "27";
            sht.L27_MESSAGE2 = GetCol(rows, 26, 0);
            LineNo = "28";
            sht.L28_MESSAGE3 = GetCol(rows, 27, 0);
            LineNo = "29";
            sht.L29_MESSAGE4 = GetCol(rows, 28, 0);
            LineNo = "30";
            sht.L30_MESSAGE5 = GetCol(rows, 29, 0);
            LineNo = "31";
            sht.L31_MESSAGE6 = GetCol(rows, 30, 0);
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
                else if (sht.L6_MeasureContractDemand.ToUpper() == "KVA" && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD)) * (0.9)) >= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD)) * (0.9)) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
                {
                    sht.L33_MESSAGE7 += TemplateConditionalWithSTHindiCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (sht.L6_MeasureContractDemand.ToUpper() == "HP" && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD)) * (0.746)) >= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD)) * (0.746)) <= Convert.ToDouble(TemplateConditionalWithSTHindiCopy.Rows[i]["3"].ToString()))
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
                else if (sht.L6_MeasureContractDemand.ToUpper() == "KVA" && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD)) * (0.9)) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD)) * (0.9)) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
                {
                    sht.L34_MESSAGE8 += TemplateConditionalWithSTEnglishCopy.Rows[i]["4"].ToString().Trim('�') + " \r\n";
                }
                else if (sht.L6_MeasureContractDemand.ToUpper() == "HP" && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD)) * (0.746)) >= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["2"].ToString()) && ((Convert.ToDouble(sht.L6_SERVDET_SANC_LOAD)) * (0.746)) <= Convert.ToDouble(TemplateConditionalWithSTEnglishCopy.Rows[i]["3"].ToString()))
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

            sht.L32_BarCode = GetCol(rows, 31, 0);
            #endregion

            #region Line-37
            //Line 33-36 Starts
            LineNo = "33";
            sht.L33_ForGST = GetCol(rows, 32, 0);
            LineNo = "34";
            sht.L34_ForGST = GetCol(rows, 33, 0);
            LineNo = "35";
            sht.L35_ForGST = GetCol(rows, 34, 0);
            LineNo = "36";
            sht.L36_ForGST = GetCol(rows, 35, 0);
            //Line 33-36 end
            #endregion

            LineNo = "37";
            sht.L37_Last_13_months_Power_factor_for_graph = GetCol(rows, 36, 0);

            //removed pf chart logic

            //DataTable PFchrtData = new DataTable();
            //PFchrtData.Columns.Add("MonthYear");
            //PFchrtData.Columns.Add("Value", typeof(decimal));

            //for (int i = 0; i <= 25; i += 2)
            //{
            //    if (MonthYear != (string.IsNullOrEmpty(GetCol(rows, 23, i)) ? ((i / 2) + 1).ToString() : GetCol(rows, 23, i)))
            //    {
            //        PFchrtData.Rows.Add(new object[] { string.IsNullOrEmpty(GetCol(rows, 23, i)) ? ((i / 2) + 1).ToString() : GetCol(rows, 23, i), string.IsNullOrEmpty(GetCol(rows, 36, (i + 1))) ? 0 : Convert.ToDecimal(GetCol(rows, 36, (i + 1))) });
            //        MonthYear = string.IsNullOrEmpty(GetCol(rows, 23, i)) ? ((i / 2) + 1).ToString() : GetCol(rows, 23, i);
            //    }
            //    else
            //    {
            //        PFchrtData.Rows.Add(new object[] { MonthYear.Replace("-", "  "), string.IsNullOrEmpty(GetCol(rows, 36, (i + 1))) ? 0 : Convert.ToDecimal(GetCol(rows, 36, (i + 1))) });
            //        MonthYear = string.IsNullOrEmpty(GetCol(rows, 23, i)) ? ((i / 2) + 1).ToString() : GetCol(rows, 23, i);
            //    }
            //}
            //MonthYear = "";
            //sht.PFgrph = PFchrtData;

            Console.WriteLine("HT Line 37 parsed");
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

            Console.WriteLine("Custom Fields calculated");




            #endregion

            return sht;
        }



        private string GetCol(string[][] cols, int row, int col)
        {
            if (cols.Length > row && cols[row].Length > col)
                return cols[row][col]?.Trim() ?? "";

            return "";
        }

       
        private DevExpress.XtraPrinting.Drawing.Watermark GetWatermark(string imageName)
        {
            var watermark = new DevExpress.XtraPrinting.Drawing.Watermark();

            watermark.ImageSource =
                DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(
                    Application.StartupPath +
                    "\\Contents\\CategorySlabImages\\" + imageName);

            watermark.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            watermark.ImageTiling = false;
            watermark.ImageViewMode =
                DevExpress.XtraPrinting.Drawing.ImageViewMode.Clip;
            watermark.ImageTransparency = 0;
            watermark.ShowBehind = true;

            return watermark;
        }

    }
}
