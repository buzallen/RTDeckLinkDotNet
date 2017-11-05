using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DLKeyFillLib
{
    public partial class PreviewWindow : Form
    {
        private Bitmap _activeBitmap;
        public Bitmap ActiveBitmap
        {
            set {
                _activeBitmap = value;
                this.Invoke((MethodInvoker)delegate { Invalidate(); });
            }
        }


        public PreviewWindow()
        {
            InitializeComponent();

            this.SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.DoubleBuffer, true);
        }

        public void InitPreview(int width, int height)
        {
            this.ClientSize = new Size(width, height);
            //pictureBox.Size = new Size(width, height);
            //pictureBox.Location = new Point(0, 0);
        }

        private void PreviewWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void PreviewWindow_Paint(object sender, PaintEventArgs e)
        {
            
            if (_activeBitmap != null) {
                try {
                    e.Graphics.Clear(Color.Black);
                    e.Graphics.DrawImageUnscaled(_activeBitmap, ClientRectangle);
                }
                catch {

                }
            }
        }

        
    }
}
