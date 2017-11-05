namespace AppCommon
{
    partial class DLSettingsFormForDual
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
            if (disposing && (components != null)) {
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
            this.chkRightToLeft = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.lstDL1 = new System.Windows.Forms.ListBox();
            this.lstDL2 = new System.Windows.Forms.ListBox();
            this.lstDL1DM = new System.Windows.Forms.ListBox();
            this.lstDL2DM = new System.Windows.Forms.ListBox();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.nudFPS = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFPS)).BeginInit();
            this.SuspendLayout();
            // 
            // chkRightToLeft
            // 
            this.chkRightToLeft.AutoSize = true;
            this.chkRightToLeft.Location = new System.Drawing.Point(212, 389);
            this.chkRightToLeft.Name = "chkRightToLeft";
            this.chkRightToLeft.Size = new System.Drawing.Size(88, 17);
            this.chkRightToLeft.TabIndex = 0;
            this.chkRightToLeft.Text = "Right To Left";
            this.chkRightToLeft.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(200, 420);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(121, 36);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lstDL1
            // 
            this.lstDL1.FormattingEnabled = true;
            this.lstDL1.Location = new System.Drawing.Point(12, 12);
            this.lstDL1.Name = "lstDL1";
            this.lstDL1.Size = new System.Drawing.Size(156, 108);
            this.lstDL1.TabIndex = 2;
            this.lstDL1.SelectedIndexChanged += new System.EventHandler(this.lstDL1_SelectedIndexChanged);
            // 
            // lstDL2
            // 
            this.lstDL2.FormattingEnabled = true;
            this.lstDL2.Location = new System.Drawing.Point(174, 12);
            this.lstDL2.Name = "lstDL2";
            this.lstDL2.Size = new System.Drawing.Size(156, 108);
            this.lstDL2.TabIndex = 3;
            this.lstDL2.SelectedIndexChanged += new System.EventHandler(this.lstDL2_SelectedIndexChanged);
            // 
            // lstDL1DM
            // 
            this.lstDL1DM.FormattingEnabled = true;
            this.lstDL1DM.Location = new System.Drawing.Point(12, 126);
            this.lstDL1DM.Name = "lstDL1DM";
            this.lstDL1DM.Size = new System.Drawing.Size(156, 212);
            this.lstDL1DM.TabIndex = 4;
            this.lstDL1DM.SelectedIndexChanged += new System.EventHandler(this.lstDL1DM_SelectedIndexChanged);
            // 
            // lstDL2DM
            // 
            this.lstDL2DM.FormattingEnabled = true;
            this.lstDL2DM.Location = new System.Drawing.Point(174, 126);
            this.lstDL2DM.Name = "lstDL2DM";
            this.lstDL2DM.Size = new System.Drawing.Size(156, 212);
            this.lstDL2DM.TabIndex = 5;
            this.lstDL2DM.SelectedIndexChanged += new System.EventHandler(this.lstDL2DM_SelectedIndexChanged);
            // 
            // nudWidth
            // 
            this.nudWidth.Location = new System.Drawing.Point(71, 385);
            this.nudWidth.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.Size = new System.Drawing.Size(63, 20);
            this.nudWidth.TabIndex = 6;
            this.nudWidth.Value = new decimal(new int[] {
            720,
            0,
            0,
            0});
            // 
            // nudHeight
            // 
            this.nudHeight.Location = new System.Drawing.Point(71, 411);
            this.nudHeight.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.Size = new System.Drawing.Size(63, 20);
            this.nudHeight.TabIndex = 7;
            this.nudHeight.Value = new decimal(new int[] {
            576,
            0,
            0,
            0});
            // 
            // nudFPS
            // 
            this.nudFPS.Location = new System.Drawing.Point(71, 436);
            this.nudFPS.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.nudFPS.Name = "nudFPS";
            this.nudFPS.Size = new System.Drawing.Size(63, 20);
            this.nudFPS.TabIndex = 8;
            this.nudFPS.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 356);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Fake DeckLink";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 387);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Width";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 413);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Height";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 438);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "FPS";
            // 
            // DLSettingsFormForDual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 464);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudFPS);
            this.Controls.Add(this.nudHeight);
            this.Controls.Add(this.nudWidth);
            this.Controls.Add(this.lstDL2DM);
            this.Controls.Add(this.lstDL1DM);
            this.Controls.Add(this.lstDL2);
            this.Controls.Add(this.lstDL1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkRightToLeft);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DLSettingsFormForDual";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFPS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkRightToLeft;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ListBox lstDL1;
        private System.Windows.Forms.ListBox lstDL2;
        private System.Windows.Forms.ListBox lstDL1DM;
        private System.Windows.Forms.ListBox lstDL2DM;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.NumericUpDown nudFPS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}