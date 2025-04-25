using DevExpress.XtraEditors;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AT.Print.Utils
{
    public static class ParseAsDataTable
    {
        static int LineNo = 0;
        static string ServiceNoLine = "";
        public static DataTable LT_FileTxtToDataTable(string path, int BillNo = 0, string BillType = "Bill")
        {
            int LineNo = 0;
            DataTable dt = new DataTable();
            try
            {
                StringReader sr = new StringReader(path);
                int TotalLines = path.Split('\n').Length - 1;
                if (!string.IsNullOrEmpty(path.Split('\n')[5].Split('|')[0].ToString()) && !string.IsNullOrWhiteSpace(path.Split('\n')[5].Split('|')[0].ToString()))
                {
                    ServiceNoLine = path.Split('\n')[5].Split('|')[0].ToString();
                }
                if (TotalLines != 36)
                {
                    XtraMessageBox.Show("Bill No: " + BillNo + " and Service No: " + ServiceNoLine + " has not 36 rows.");
                    AppFunctions.CloseWaitForm();
                    return dt;
                }

                string[] fields;
                dt.Columns.Add("1");
                dt.Columns.Add("2");
                dt.Columns.Add("3");
                dt.Columns.Add("4");
                dt.Columns.Add("5");
                dt.Columns.Add("6");
                dt.Columns.Add("7");
                dt.Columns.Add("8");
                dt.Columns.Add("9");
                dt.Columns.Add("10");
                dt.Columns.Add("11");
                dt.Columns.Add("12");
                dt.Columns.Add("13");
                dt.Columns.Add("14");
                dt.Columns.Add("15");
                dt.Columns.Add("16");
                dt.Columns.Add("17");
                dt.Columns.Add("18");
                dt.Columns.Add("19");
                dt.Columns.Add("20");
                dt.Columns.Add("21");
                dt.Columns.Add("22");
                dt.Columns.Add("23");
                dt.Columns.Add("24");
                dt.Columns.Add("25");
                dt.Columns.Add("26");
                dt.Columns.Add("27");
                dt.Columns.Add("28");
                while (sr.Peek() > 0)
                {
                    LineNo++;

                    var field = sr.ReadLine();
                    fields = field.Split('|');
                    DataRow dr = dt.NewRow();
                    if (fields != null)
                    {
                        switch (LineNo)
                        {
                            case 1:
                                {
                                    if (fields.Length != 14)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 1 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if ((fields[9].ToString() == "") || (fields[9].ToString() != "0" && fields[9].ToString() != "1"))
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 1 column 10th is blank or value differ from 0 or 1");
                                        return dt;
                                    }
                                    else if ((fields[11].ToString() == "") || (fields[11].ToString() != "0" && fields[11].ToString() != "1"))
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 1 column 12th is either blank or value differ from 0 or 1");
                                        return dt;
                                    }
                                    else if (fields[12].ToString() == "")
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 1 column 13th is blank");
                                        return dt;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 2 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 3 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 4:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 4 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 5:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 5 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 6:
                                {
                                    if (fields.Length != 12)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 6 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 7:
                                {
                                    if (fields.Length != 7)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 7 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 8:
                                {
                                    if (fields.Length != 16)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 8 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 9:
                                {
                                    if (fields.Length != 8)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 9 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 10:
                                {
                                    if (fields.Length != 9)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 10 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 11:
                                {
                                    if (fields.Length != 26)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 11 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 12:
                                {
                                    if (fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 12 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 13:
                                {
                                    if (fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 13 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 14:
                                {
                                    if (fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 14 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 15:
                                {
                                    if (fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 15 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 16:
                                {
                                    if (fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 16 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 17:
                                {
                                    if (fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 17 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 18:
                                {
                                    if (fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 18 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 19:
                                {
                                    if (fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 19 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 20:
                                {
                                    if (fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 20 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 21:
                                {
                                    if (fields.Length != 26)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 21 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 22:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 22 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 23:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 23 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 24:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 24 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 25:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 25 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 26:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 26 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 27:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 27 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 28:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 28 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 29:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 29 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 30:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 30 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 31:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 31 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 32:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 32 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 33:
                                {
                                    if (fields.Length != 6)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 33 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 34:
                                {
                                    if (fields.Length != 21)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 34 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 35:
                                {
                                    if (fields.Length != 21)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 35 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 36:
                                {
                                    if (fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 36 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                        }

                        for (int i = 0; i < fields.Length; i++)
                        {
                            dr[i] = fields[i].ToString().Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                AppFunctions.LogError(ex);
                XtraMessageBox.Show("Problem creating Datatable!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public static DataTable LTMD_FileTxtToDataTable(string path, int BillNo = 0, string BillType = "Bill")
        {
            int LineNo = 0;
            DataTable dt = new DataTable();
            try
            {
                StringReader sr = new StringReader(path);
                int TotalLines = path.Split('\n').Length - 1;
                if (!string.IsNullOrEmpty(path.Split('\n')[5].Split('|')[0].ToString()) && !string.IsNullOrWhiteSpace(path.Split('\n')[5].Split('|')[0].ToString()))
                {
                    ServiceNoLine = path.Split('\n')[5].Split('|')[0].ToString();
                }
                if (TotalLines != 37)
                {
                    XtraMessageBox.Show("Bill No: " + BillNo + " and Service No: " + ServiceNoLine + " has not 37 rows.");
                    AppFunctions.CloseWaitForm();
                    return dt;
                }
                string[] fields;
                dt.Columns.Add("1");
                dt.Columns.Add("2");
                dt.Columns.Add("3");
                dt.Columns.Add("4");
                dt.Columns.Add("5");
                dt.Columns.Add("6");
                dt.Columns.Add("7");
                dt.Columns.Add("8");
                dt.Columns.Add("9");
                dt.Columns.Add("10");
                dt.Columns.Add("11");
                dt.Columns.Add("12");
                dt.Columns.Add("13");
                dt.Columns.Add("14");
                dt.Columns.Add("15");
                dt.Columns.Add("16");
                dt.Columns.Add("17");
                dt.Columns.Add("18");
                dt.Columns.Add("19");
                dt.Columns.Add("20");
                dt.Columns.Add("21");
                dt.Columns.Add("22");
                dt.Columns.Add("23");
                dt.Columns.Add("24");
                dt.Columns.Add("25");
                dt.Columns.Add("26");
                dt.Columns.Add("27");
                dt.Columns.Add("28");
                while (sr.Peek() > 0)
                {
                    LineNo++;

                    var field = sr.ReadLine();
                    fields = field.Split('|');
                    DataRow dr = dt.NewRow();
                    if (fields != null)
                    {
                        switch (LineNo)
                        {
                            case 1:
                                {
                                    if (fields.Length != 15)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 1 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if ((fields[10].ToString() == "") || (fields[10].ToString() != "0" && fields[10].ToString() != "1"))
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 1 column 11th is blank or value differ from 0 or 1");
                                        return dt;
                                    }
                                    else if ((fields[12].ToString() == "") || (fields[12].ToString() != "0" && fields[12].ToString() != "1"))
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 1 column 13th is either blank or value differ from 0 or 1");
                                        return dt;
                                    }
                                    else if (fields[13].ToString() == "")
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 1 column 14th is blank");
                                        return dt;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 2 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 3 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 4:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 4 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 5:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 5 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 6:
                                {
                                    if (fields.Length != 13)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 6 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 7:
                                {
                                    if (fields.Length != 7)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 7 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 8:
                                {
                                    if (fields.Length != 17)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 8 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 9:
                                {
                                    if (fields.Length != 8)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 9 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 10:
                                {
                                    if (fields.Length != 9)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 10 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 11:
                                {
                                    if (fields.Length != 26)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 11 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 12:
                                {
                                    if (fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 12 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 13:
                                {
                                    if (fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 13 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 14:
                                {
                                    if (fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 14 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 15:
                                {
                                    if (fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 15 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 16:
                                {
                                    if (fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 16 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 17:
                                {
                                    if (fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 17 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 18:
                                {
                                    if (fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 18 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 19:
                                {
                                    if (fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 19 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 20:
                                {
                                    if (fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 20 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 21:
                                {
                                    if (fields.Length != 26)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 21 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 22:

                                {
                                    if (fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 22 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 23:
                                {
                                    if (fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 23 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 24:
                                {
                                    if (fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 24 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 25:
                                {
                                    if (fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 25 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 26:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 26 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 27:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 27 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 28:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 28 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 29:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 29 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 30:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 30 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 31:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 31 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 32:
                                {
                                    if (fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 32 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 33:
                                {
                                    if (fields.Length != 6)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 33 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 34:
                                {
                                    if (fields.Length != 21)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 34 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 35:
                                {
                                    if (fields.Length != 21)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 35 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 36:
                                {
                                    if (fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 36 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 37:
                                {
                                    if (fields.Length < 26)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 37 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                        }

                        for (int i = 0; i < fields.Length; i++)
                        {
                            dr[i] = fields[i].ToString().Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                AppFunctions.LogError(ex);
                XtraMessageBox.Show("Problem creating Datatable!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public static DataTable HT_FileTxtToDataTable(string path, int BillNo = 0, string BillType = "Bill")
        {
            int LineNo = 0;
            DataTable dt = new DataTable();
            string DoubleMeterValue = "";
            decimal value;
            try
            {
                StringReader sr = new StringReader(path);
                int TotalLines = path.Split('\n').Length - 1;
                if (!string.IsNullOrEmpty(path.Split('\n')[5].Split('|')[0].ToString()) && !string.IsNullOrWhiteSpace(path.Split('\n')[5].Split('|')[0].ToString()))
                {
                    ServiceNoLine = path.Split('\n')[5].Split('|')[0].ToString();
                }
                if (BillType == "HT" && TotalLines != 37)
                {
                    XtraMessageBox.Show("Bill No: " + BillNo + " and Service No: " + ServiceNoLine + " has not 37 rows.");
                    AppFunctions.CloseWaitForm();
                    return dt;
                }
                string[] fields;
                dt.Columns.Add("1");
                dt.Columns.Add("2");
                dt.Columns.Add("3");
                dt.Columns.Add("4");
                dt.Columns.Add("5");
                dt.Columns.Add("6");
                dt.Columns.Add("7");
                dt.Columns.Add("8");
                dt.Columns.Add("9");
                dt.Columns.Add("10");
                dt.Columns.Add("11");
                dt.Columns.Add("12");
                dt.Columns.Add("13");
                dt.Columns.Add("14");
                dt.Columns.Add("15");
                dt.Columns.Add("16");
                dt.Columns.Add("17");
                dt.Columns.Add("18");
                dt.Columns.Add("19");
                dt.Columns.Add("20");
                dt.Columns.Add("21");
                dt.Columns.Add("22");
                dt.Columns.Add("23");
                dt.Columns.Add("24");
                dt.Columns.Add("25");
                dt.Columns.Add("26");
                dt.Columns.Add("27");
                dt.Columns.Add("28");
                dt.Columns.Add("29");
                dt.Columns.Add("30");
                dt.Columns.Add("31");
                dt.Columns.Add("32");
                dt.Columns.Add("33");
                dt.Columns.Add("34");
                dt.Columns.Add("35");
                dt.Columns.Add("36");
                dt.Columns.Add("37");
                while (sr.Peek() > 0)
                {
                    LineNo++;
                    var field = sr.ReadLine();
                    fields = field.Split('|');
                    DataRow dr = dt.NewRow();
                    if (fields != null)
                    {
                        switch (LineNo)
                        {
                            case 1:
                                {
                                    if (BillType == "HT" && fields.Length != 12)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 1 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if ((BillType == "HT" && fields[7].ToString() == "") || (BillType == "HT" && fields[7].ToString() != "0" && fields[7].ToString() != "1"))
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 1 column 8th is blank or value differ from 0 or 1");
                                        return dt;
                                    }
                                    else if ((BillType == "HT" && fields[9].ToString() == "") || (BillType == "HT" && fields[9].ToString() != "0" && fields[9].ToString() != "1"))
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 1 column 10th is either blank or value differ from 0 or 1");
                                        return dt;
                                    }
                                    else if (BillType == "HT" && fields[10].ToString() == "")
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 1 column 11th is blank");
                                        return dt;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (BillType == "HT" && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 2 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    if (BillType == "HT" && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 3 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 4:
                                {
                                    if (BillType == "HT" && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 4 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 5:
                                {
                                    if (BillType == "HT" && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 5 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 6:
                                {
                                    if (BillType == "HT" && fields.Length != 13)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 6 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 7:
                                {
                                    if (BillType == "HT" && fields.Length != 7)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 7 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 8:
                                {
                                    if (BillType == "HT" && fields.Length != 16)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 8 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 9:
                                {
                                    if (BillType == "HT" && fields.Length != 6)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 9 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 10:
                                {
                                    if (BillType == "HT" && fields.Length != 9)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 10 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 11:
                                {
                                    if (BillType == "HT" && fields.Length != 2)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 11 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    DoubleMeterValue = fields[1].ToString().Trim();
                                }
                                break;
                            case 12:
                                {
                                    if (BillType == "HT" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 12 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 13:
                                {
                                    if (BillType == "HT" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 13 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 14:
                                {
                                    if (BillType == "HT" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 14 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 15:
                                {
                                    if (BillType == "HT" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 15 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 16:
                                {
                                    if (BillType == "HT" && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 16 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 17:
                                {
                                    if (BillType == "HT" && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 17 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 18:
                                {
                                    if (BillType == "HT" && fields.Length != 4 && DoubleMeterValue == "")
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 18 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT" && fields.Length != 3 && DoubleMeterValue != "")
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 18 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 19:
                                {
                                    if (BillType == "HT" && fields.Length != 4 && DoubleMeterValue == "")
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 19 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT" && fields.Length != 3 && DoubleMeterValue != "")
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 19 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 20:
                                {
                                    if (BillType == "HT" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 20 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 21:
                                {
                                    if (BillType == "HT" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 21 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 22:
                                {
                                    if (BillType == "HT" && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 22 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 23:
                                {
                                    if (BillType == "HT" && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 23 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 24:
                                {
                                    if (BillType == "HT" && fields.Length != 26)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 24 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 25:
                                {
                                    if (BillType == "HT" && fields.Length != 26)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 25 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 26:
                                {
                                    if (BillType == "HT" && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 26 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 27:
                                {
                                    if (BillType == "HT" && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 27 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 28:
                                {
                                    if (BillType == "HT" && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 28 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 29:
                                {
                                    if (BillType == "HT" && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 29 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 30:
                                {
                                    if (BillType == "HT" && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 30 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 31:
                                {
                                    if (BillType == "HT" && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 31 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 32:
                                {
                                    if (BillType == "HT" && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 32 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 33:
                                {
                                    if (BillType == "HT" && fields.Length != 6)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 33 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 34:
                                {
                                    if (BillType == "HT" && fields.Length != 21)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 34 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 35:
                                {
                                    if (BillType == "HT" && fields.Length != 21)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 35 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 36:
                                {
                                    if (BillType == "HT" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 36 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 37:
                                {

                                    for (int Chart = 0; Chart < fields.Length - 1; Chart += 2)
                                    {
                                        if (!string.IsNullOrEmpty(Convert.ToString(fields[Chart + 1])) && !Decimal.TryParse(fields[Chart + 1], out value))
                                        {
                                            AppFunctions.CloseWaitForm();
                                            XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 37 has string value for chart inspite of numeric value on " + (Chart + 1) + "  seprator.");
                                            return dt;
                                        }
                                    }
                                    if (BillType == "HT" && fields.Length < 26)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 37 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                        }
                        for (int i = 0; i < fields.Length; i++)
                        {
                            dr[i] = fields[i].ToString().Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                AppFunctions.LogError(ex);
                XtraMessageBox.Show("Problem creating Datatable!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }
            return dt;
        }

        public static DataTable LTMD_Solar_FileTxtToDataTable(string path, int BillNo = 0, string BillType = "Bill")
        {
            int LineNo = 0;
            DataTable dt = new DataTable();
            int TotalLines = path.Split('\n').Length - 1;
            if (!string.IsNullOrEmpty(path.Split('\n')[5].Split('|')[0].ToString()) && !string.IsNullOrWhiteSpace(path.Split('\n')[5].Split('|')[0].ToString()))
            {
                ServiceNoLine = path.Split('\n')[5].Split('|')[0].ToString();
            }
            if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && TotalLines != 56)
            {
                XtraMessageBox.Show("Bill No: " + BillNo + " and Service No: " + ServiceNoLine + " has not 56 rows.");
                AppFunctions.CloseWaitForm();
                return dt;
            }
            try
            {
                StringReader sr = new StringReader(path);
                string[] fields;
                dt.Columns.Add("1");
                dt.Columns.Add("2");
                dt.Columns.Add("3");
                dt.Columns.Add("4");
                dt.Columns.Add("5");
                dt.Columns.Add("6");
                dt.Columns.Add("7");
                dt.Columns.Add("8");
                dt.Columns.Add("9");
                dt.Columns.Add("10");
                dt.Columns.Add("11");
                dt.Columns.Add("12");
                dt.Columns.Add("13");
                dt.Columns.Add("14");
                dt.Columns.Add("15");
                dt.Columns.Add("16");
                dt.Columns.Add("17");
                dt.Columns.Add("18");
                dt.Columns.Add("19");
                dt.Columns.Add("20");
                while (sr.Peek() > 0)
                {
                    LineNo++;
                    var field = sr.ReadLine();
                    fields = field.Split('|');
                    DataRow dr = dt.NewRow();
                    if (fields != null)
                    {
                        switch (LineNo)
                        {
                            case 1:
                                {
                                    if (BillType == "LTMD Solar" && fields.Length != 10)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 1 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 7)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 1 has " + fields.Length + " columns only.");
                                        return dt;

                                    }
                                    else if (BillType == "LT Solar" && fields.Length != 9)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 1 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 2 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 3 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 4:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 4 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 5:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 5 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 6:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar") && fields.Length != 13)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 6 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if ((BillType == "LT Solar") && fields.Length != 12)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 6 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 7:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 6)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + " and row 7 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 8:
                                {
                                    if (BillType == "LTMD Solar" && fields.Length != 17)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 8 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if ((BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 16)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 8 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 9:
                                {
                                    if ((BillType == "LTMD Solar" ||BillType == "LT Solar") && fields.Length != 8)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 9 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 6)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 9 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 10:
                                {
                                    if ((BillType == "LTMD Solar" ||BillType == "LT Solar") && fields.Length != 9)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 10 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 8)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 10 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 11:
                                {
                                    if ((BillType == "LTMD Solar" ||BillType == "LT Solar")&& fields.Length != 14)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 11 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 2)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 11 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 12:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "LT Solar") && fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 12 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 12 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 13:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "LT Solar") && fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 13 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 13 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 14:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "LT Solar") && fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 14 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 14 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 15:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 15 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 16:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "LT Solar") && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 16 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 16 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 17:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "LT Solar") && fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 17 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 17 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 18:
                                {
                                    if ((BillType == "LTMD Solar"  || BillType == "LT Solar") && fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 18 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 18 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 19:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "LT Solar") && fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 19 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 19 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 20:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "LT Solar") && fields.Length != 3)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 20 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 20 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 21:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "LT Solar" )&& fields.Length != 14)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 21 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 21 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 22:
                                {
                                    if (BillType == "LTMD Solar" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 22 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 22 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "LT Solar" && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 22 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 23:
                                {
                                    if (BillType == "LTMD Solar" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 23 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 23 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "LT Solar" && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 23 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 24:
                                {
                                    if (BillType == "LTMD Solar" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 24 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 14)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 24 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "LT Solar" && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 24 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 25:
                                {
                                    if (BillType == "LTMD Solar" && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 25 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "HT Solar" && fields.Length != 14)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 25 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                    else if (BillType == "LT Solar" && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 25 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 26:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 26 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 27:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 27 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 28:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 28 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 29:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 29 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 30:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 30 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 31:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 31 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 32:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 32 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 33:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 16)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 33 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 34:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 34 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 35:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 35 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 36:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 36 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 37:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 1)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 37 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 38:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 38 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 39:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 39 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 40:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 40 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 41:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 4)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 41 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 42:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 9)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 42 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 43:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 9)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 43 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 44:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 44 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 45:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 45 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 46:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 15)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 46 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 47:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 15)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 47 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 48:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 15)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 48 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 49:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 17)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 49 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 50:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 50 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 51:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 51 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 52:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 52 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 53:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 5)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 53 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 54:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 6)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 54 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 55:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType == "LT Solar") && fields.Length != 15)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 55 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                            case 56:
                                {
                                    if ((BillType == "LTMD Solar" || BillType == "HT Solar" || BillType =="LT Solar") && fields.Length != 15)
                                    {
                                        AppFunctions.CloseWaitForm();
                                        XtraMessageBox.Show("Bill No: " + BillNo + ", Service No. " + ServiceNoLine + "  and row 56 has " + fields.Length + " columns only.");
                                        return dt;
                                    }
                                }
                                break;
                        }
                        for (int i = 0; i < fields.Length; i++)
                        {
                            dr[i] = fields[i].ToString().Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                AppFunctions.LogError(ex);
                XtraMessageBox.Show("Problem creating Datatable!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }
            return dt;
        }

        public static DataTable TemplateConditionalWithSTHindi()
        {
            string contents = File.ReadAllText(Application.StartupPath + "\\Contents\\CategorySlabImages\\Template Conditional With ST Hindi.txt");
            DataTable dt = new DataTable();
            try
            {
                StringReader sr = new StringReader(contents);
                string[] fields;
                dt.Columns.Add("1", typeof(string));
                dt.Columns.Add("2", typeof(string));
                dt.Columns.Add("3", typeof(string));
                dt.Columns.Add("4", typeof(string));

                while (sr.Peek() > 0)
                {
                    var field = sr.ReadLine();
                    fields = field.Split('|');
                    DataRow dr = dt.NewRow();
                    if (fields != null)
                    {
                        for (int i = 0; i < fields.Length; i++)
                        {
                            dr[i] = fields[i].ToString().Trim('�');
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                AppFunctions.LogError(ex);
                XtraMessageBox.Show("Problem creating Datatable!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }

            return dt;

        }

        public static DataTable TemplateConditionalWithSTEnglish()
        {
            string contents = File.ReadAllText(Application.StartupPath + "\\Contents\\CategorySlabImages\\Template Conditional With ST English.txt");

            DataTable dt = new DataTable();
            try
            {
                StringReader sr = new StringReader(contents);
                string[] fields;
                dt.Columns.Add("1", typeof(string));
                dt.Columns.Add("2", typeof(string));
                dt.Columns.Add("3", typeof(string));
                dt.Columns.Add("4", typeof(string));

                while (sr.Peek() > 0)
                {
                    var field = sr.ReadLine();
                    fields = field.Split('|');
                    DataRow dr = dt.NewRow();
                    if (fields != null)
                    {
                        for (int i = 0; i < fields.Length; i++)
                        {
                            dr[i] = fields[i].ToString().Trim('�');
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                AppFunctions.LogError(ex);
                XtraMessageBox.Show("Problem creating Datatable!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }
            return dt;

        }

        public static DataTable TemplateConditionalWithServiceNoHindi()
        {
            string contents = File.ReadAllText(Application.StartupPath + "\\Contents\\CategorySlabImages\\Template Conditional With Service No Hindi.txt");

            DataTable dt = new DataTable();
            try
            {
                StringReader sr = new StringReader(contents);
                string[] fields;
                dt.Columns.Add("1", typeof(string));
                dt.Columns.Add("2", typeof(string));

                while (sr.Peek() > 0)
                {
                    var field = sr.ReadLine();
                    fields = field.Split('|');
                    DataRow dr = dt.NewRow();
                    if (fields != null)
                    {
                        for (int i = 0; i < fields.Length; i++)
                        {
                            dr[i] = fields[i].ToString().Trim('�');
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                AppFunctions.LogError(ex);
                XtraMessageBox.Show("Problem creating Datatable!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }
            return dt;

        }

        public static DataTable TemplateConditionalWithServiceNoEnglish()
        {
            string contents = File.ReadAllText(Application.StartupPath + "\\Contents\\CategorySlabImages\\Template Conditional With Service No English.txt");

            DataTable dt = new DataTable();
            try
            {
                StringReader sr = new StringReader(contents);
                string[] fields;
                dt.Columns.Add("1", typeof(string));
                dt.Columns.Add("2", typeof(string));

                while (sr.Peek() > 0)
                {
                    var field = sr.ReadLine();
                    fields = field.Split('|');
                    DataRow dr = dt.NewRow();
                    if (fields != null)
                    {
                        for (int i = 0; i < fields.Length; i++)
                        {
                            dr[i] = fields[i].ToString().Trim('�');
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                AppFunctions.LogError(ex);
                XtraMessageBox.Show("Problem creating Datatable!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }
            return dt;

        }


    }
}