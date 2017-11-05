using AppCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTDeckLinkDotNet_Sample
{
    public partial class Form1 : Form
    {
        private DLAccessForKeyFill keyFillOutput;
        private Font font = new Font(new FontFamily("Arial"), 100, FontStyle.Bold);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            keyFillOutput = new DLAccessForKeyFill("demo-key-fill");
            keyFillOutput.FrameCallback = Draw;
        }

        private void deckLinkSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            keyFillOutput.ShowDecklinkSettings();
        }

        private void startRealToolStripMenuItem_Click(object sender, EventArgs e)
        {
            keyFillOutput.ConnectRealDecklink();
        }

        private void startFakeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            keyFillOutput.ConnectFakeDecklink();
        }

        public void Draw(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.Blue);
            g.DrawString("Hello", font, Brushes.White, 100, 100);
        }

    }
}
