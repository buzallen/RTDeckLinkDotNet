namespace RTDeckLinkDotNet_Sample
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deckLinkSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startRealToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startFakeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1088, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deckLinkSettingsToolStripMenuItem,
            this.startRealToolStripMenuItem,
            this.startFakeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // deckLinkSettingsToolStripMenuItem
            // 
            this.deckLinkSettingsToolStripMenuItem.Name = "deckLinkSettingsToolStripMenuItem";
            this.deckLinkSettingsToolStripMenuItem.Size = new System.Drawing.Size(240, 30);
            this.deckLinkSettingsToolStripMenuItem.Text = "Deck Link Settings";
            this.deckLinkSettingsToolStripMenuItem.Click += new System.EventHandler(this.deckLinkSettingsToolStripMenuItem_Click);
            // 
            // startRealToolStripMenuItem
            // 
            this.startRealToolStripMenuItem.Name = "startRealToolStripMenuItem";
            this.startRealToolStripMenuItem.Size = new System.Drawing.Size(240, 30);
            this.startRealToolStripMenuItem.Text = "Start Real";
            this.startRealToolStripMenuItem.Click += new System.EventHandler(this.startRealToolStripMenuItem_Click);
            // 
            // startFakeToolStripMenuItem
            // 
            this.startFakeToolStripMenuItem.Name = "startFakeToolStripMenuItem";
            this.startFakeToolStripMenuItem.Size = new System.Drawing.Size(240, 30);
            this.startFakeToolStripMenuItem.Text = "Start Fake";
            this.startFakeToolStripMenuItem.Click += new System.EventHandler(this.startFakeToolStripMenuItem_Click);

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 636);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deckLinkSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startRealToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startFakeToolStripMenuItem;
        private System.Windows.Forms.Label label1;
    }
}

