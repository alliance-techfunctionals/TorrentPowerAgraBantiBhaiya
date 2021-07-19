using ATPrintUtility;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraSplashScreen;
using System;
using System.IO;
using System.Windows.Forms;

namespace AT.Print.Utils
{
    public class AppFunctions
    {
        public static void ShowWaitForm(string caption)
        {
            try
            {
                AppFunctions.DisplayStatus(caption);
                Application.DoEvents();
                if (SplashScreenManager.Default == null)
                {
                    SplashScreenManager.ShowForm(GetRootForm(), typeof(ATWaitForm));
                    SplashScreenManager.Default.SetWaitFormCaption(caption);

                }
                else
                {
                    if (!SplashScreenManager.Default.IsSplashFormVisible)
                    {
                        SplashScreenManager.ShowForm(GetRootForm(), typeof(ATWaitForm));
                        SplashScreenManager.Default.SetWaitFormCaption(caption);
                    }
                    else
                    {
                        SplashScreenManager.Default.SetWaitFormCaption(caption);
                    }
                }
            }
            catch (Exception ex)
            {
                AppFunctions.LogError(ex);
                Console.WriteLine(ex.Message);
            }
        }
        public static DockPanel GetDockPanelIfExists(string panelTag)
        {
            foreach (DockPanel panel in GetRootForm().GetDockManager().Panels)
            {
                if ((string)panel.Tag == panelTag)
                {
                    return panel;
                }
            }
            return null;
        }

        public static void OpenNewPanel(
            Control rootControl,
            DockingStyle style,
            string windowTitle,
            string tag,
            bool addAsATab = false,
            bool allowNewInstance = false)
        {
            DockPanel panel;
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                panel = null;
            }
            else if (allowNewInstance)
            {
                panel = null;
            }
            else
            {
                panel = GetDockPanelIfExists(tag);
            }

            if (panel == null)
            {
                panel = GetRootForm().GetDockManager().AddPanel(style);
                panel.Name = "";
                panel.Text = windowTitle;
                panel.Tag = tag;
                panel.TabsPosition = TabsPosition.Top;

                panel.DockedAsTabbedDocument = addAsATab;

                var createEditControl = rootControl;
                //createEditControl.Name = "";
                panel.FloatSize = createEditControl.Size;

                if (panel.FloatForm != null)
                {
                    panel.FloatForm.StartPosition = FormStartPosition.CenterScreen;
                }
                panel.ControlContainer.Controls.Add(createEditControl);
                createEditControl.Dock = DockStyle.Fill;
            }
            panel.Visibility = DockVisibility.Visible;
            panel.Select();
            panel.Focus();
        }
        private static MainForm _mainFormInstance;
        public static MainForm GetRootForm()
        {
            if (_mainFormInstance != null) return _mainFormInstance;

            foreach (var form in Application.OpenForms)
            {
                if (form.GetType() == typeof(MainForm))
                {
                    _mainFormInstance = (MainForm)form;
                }
            }
            return _mainFormInstance;
        }

        public static void CloseWaitForm()
        {
            if (SplashScreenManager.Default != null &&
                SplashScreenManager.Default.IsSplashFormVisible)
            {
                SplashScreenManager.CloseForm();
            }
            DisplayStatus("Completed");
        }
        public static void DisplayStatus(string status)
        {
            GetRootForm().ShowStatus(status);
        }

        public static void LogProcessedBill(string Zone, string Lot, string Group, string SerialNo, string str, string FileName,string Status )
        {
            string message = "";
            message += string.Format(Zone + "," + Lot + "," + Group + "," + SerialNo + "," + str + "," + Status) ;
            string path = Application.StartupPath + "\\Contents\\CategorySlabImages\\Processed_Bills\\" + FileName;
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }

        }

        public static string ProcessedBillData()
        {
            string dateTime = DateTime.Now.ToString("ddMMMMyyyy HHmmss");
            string filepath = Application.StartupPath + "\\Contents\\CategorySlabImages\\Processed_Bills\\";
            string filename = "Torrent_Processed_Bill_ATF" + dateTime + ".csv";

            try
            {
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                string completeFileName = filepath + filename;
                string message = string.Format("Zone,Lot,Group,SerialNo,ServiceNo,ProcessedYesOrNo");
                string path = Application.StartupPath + "\\Contents\\CategorySlabImages\\Processed_Bills\\" + filename;
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(message);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                AppFunctions.LogError("some error in writing the csv report ", ex);
            }
            return filename;

        }
        public static void LogError(string str)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += string.Format(str);
            string path = Application.StartupPath + "\\Contents\\CategorySlabImages\\ErrorLog.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }

        }

        public static void LogInfo(string str)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += string.Format(str);
            string path = Application.StartupPath + "\\Contents\\CategorySlabImages\\ErrorLog.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }

        }


        public static void LogError(string str, Exception ex)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += string.Format(str);
            message += string.Format("Message: {0}", ex.Message);
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += string.Format("Source: {0}", ex.Source);
            message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
            string path = Application.StartupPath + "\\Contents\\CategorySlabImages\\ErrorLog.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }

        }

        public static void LogError(Exception ex)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += string.Format("Message: {0}", ex.Message);
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += string.Format("Source: {0}", ex.Source);
            message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
            string path = Application.StartupPath + "\\Contents\\CategorySlabImages\\ErrorLog.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }

        }
    }
}
