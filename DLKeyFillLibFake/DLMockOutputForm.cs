using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace DLKeyFillLibFake
{
    public partial class DLMockOutputForm : Form
    {
        private Size _bitmapSize;
        public Size BitmapSize
        {
            get { return _bitmapSize; }
            set { _bitmapSize = value; NewSize(); }
        }

        private Bitmap _frameBitmap;
        public Bitmap FrameBitmap
        {
            get { return _frameBitmap; }
            set { _frameBitmap = value; pictureBox1.Image = _frameBitmap; }
        }

        public double Ratio { get; set; }

        public DLMockOutputForm()
        {
            InitializeComponent();

            
        }

        private void NewSize()
        {
            this.ClientSize = _bitmapSize;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Size = _bitmapSize;

        }

        private void DLMockOutputForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                Thread thread = new Thread(LoadImageFile);
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
        }

        private void LoadImageFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK) {
                try {
                    Bitmap bitmap = new Bitmap(ofd.FileName);
                    pictureBox1.BackgroundImage = bitmap;
                }
                catch {
                }
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                Thread thread = new Thread(LoadImageFile);
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
        }
    }
}
