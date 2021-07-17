namespace AT.Print
{
    partial class ATWaitForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ATProgressPanel = new DevExpress.XtraWaitForm.ProgressPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ATProgressPanel
            // 
            this.ATProgressPanel.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.ATProgressPanel.Appearance.ForeColor = System.Drawing.Color.OrangeRed;
            this.ATProgressPanel.Appearance.Options.UseBackColor = true;
            this.ATProgressPanel.Appearance.Options.UseForeColor = true;
            this.ATProgressPanel.AppearanceCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ATProgressPanel.AppearanceCaption.Options.UseFont = true;
            this.ATProgressPanel.AppearanceDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ATProgressPanel.AppearanceDescription.Options.UseFont = true;
            this.ATProgressPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ATProgressPanel.ImageHorzOffset = 20;
            this.ATProgressPanel.LineAnimationElementType = DevExpress.Utils.Animation.LineAnimationElementType.Triangle;
            this.ATProgressPanel.Location = new System.Drawing.Point(0, 17);
            this.ATProgressPanel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.ATProgressPanel.Name = "ATProgressPanel";
            this.ATProgressPanel.Size = new System.Drawing.Size(471, 39);
            this.ATProgressPanel.TabIndex = 0;
            this.ATProgressPanel.Text = "Please Wait ";
            this.ATProgressPanel.WaitAnimationType = DevExpress.Utils.Animation.WaitingAnimatorType.Ring;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.ATProgressPanel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 14, 0, 14);
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(471, 73);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // ATWaitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(471, 73);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "ATWaitForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraWaitForm.ProgressPanel ATProgressPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
