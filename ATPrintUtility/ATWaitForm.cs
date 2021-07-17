using DevExpress.XtraWaitForm;
using System;

namespace AT.Print
{
    public partial class ATWaitForm : WaitForm
    {
        public ATWaitForm()
        {
            InitializeComponent();
            this.ATProgressPanel.AutoHeight = true;
        }

        #region Overrides

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.ATProgressPanel.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.ATProgressPanel.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum WaitFormCommand
        {
        }
    }
}