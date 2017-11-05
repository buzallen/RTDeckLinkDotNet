using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using DLIDefinitions;
using System.Drawing.Imaging;
using System.Diagnostics;


namespace DLKeyFillLibFake
{
    public class DLKeyFillOutputWKFake : IDLKeyFillOutput
    {
        // IDLKeyFillOutput members
        public int DisplayWidth { get; set; }
        public int DisplayHeight { get; set; }
        public int FPS { get; set; }
        public int PreRollFrames { get; set; }
        public Action<Graphics> DrawFrameGFXCallback { get; set; }

        private Bitmap _surfaceBitmap;
        private Bitmap _keyBitmap;
        private Graphics _surfaceGFX;

        private bool clearScreenRequest;

        // This timer fakes the next frame callback from the decklink hardware
        private Timer timer = new Timer();

        // Key and fill fake outputs
        private DLMockOutputForm dlFill = new DLMockOutputForm();
        private DLMockOutputForm dlKey = new DLMockOutputForm();

        // Next frame arguments
        private NextFrameArgs nextFrameArgs = new NextFrameArgs();

        private Stopwatch watch = new Stopwatch();
        private int frameCounter = 0;
        private bool hideKey;

        public DLKeyFillOutputWKFake(int width, int height, int frameLength, bool hideKey)
        {
            PreRollFrames = 4;
            DisplayWidth = width;
            DisplayHeight = height;
            timer.Interval = frameLength;
            ConfigureDeckLinks(0, 0, 0, 0);
            this.hideKey = hideKey;
        }

        public void ShowPreviewWindow(bool show)
        {
            // preview window not relevant for Fake decklinks
        }

        public void ConfigureDeckLinks(int DL1Idx, int DL2Idx, int DL1DisplayModeIdx, int DL2DisplayModeIdx)
        {
            // this is a fake class so this would only apply in the real decklink interface outputs
            timer.Tick += timer_Tick;
        }

        public void Clear()
        {
            clearScreenRequest = true;
        }

        public void Start()
        {
            _surfaceBitmap = new Bitmap(DisplayWidth, DisplayHeight, PixelFormat.Format32bppPArgb);
            _keyBitmap = new Bitmap(DisplayWidth, DisplayHeight, PixelFormat.Format32bppPArgb);
            _surfaceGFX = Graphics.FromImage(_surfaceBitmap);

            dlFill.BitmapSize = new Size(DisplayWidth, DisplayHeight);
            dlKey.BitmapSize = new Size(DisplayWidth, DisplayHeight);

            if (!hideKey)
                dlKey.Show();
            dlFill.Show();

            double ratio = (double)DisplayWidth / (double)DisplayHeight;
            double startingWidth = 600.0;
            double startingHeight = 600.0 / ratio;

            Size clSize = new Size((int)startingWidth, (int)startingHeight);
            dlKey.ClientSize = clSize;
            dlFill.ClientSize = clSize;

            dlFill.Ratio = ratio;
            dlKey.Ratio = ratio;

            dlFill.Text = "Fill";
            dlKey.Text = "Key";


            timer.Start();

            watch.Reset();
            watch.Start();
        }

        public void Stop()
        {
            timer.Stop();
            dlFill.Close();
            dlKey.Close();
        }


        void timer_Tick(object sender, EventArgs e)
        {
            if (clearScreenRequest == true) {
                _surfaceGFX.Clear(Color.Transparent);
                clearScreenRequest = false;
            }
            else if (DrawFrameGFXCallback != null) {
                DrawFrameGFXCallback(_surfaceGFX);
            }

            BitmapData bmDataF = _surfaceBitmap.LockBits(new Rectangle(0, 0, _surfaceBitmap.Width, _surfaceBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
            System.IntPtr Scan0F = bmDataF.Scan0;

            BitmapData bmDataK = _keyBitmap.LockBits(new Rectangle(0, 0, _keyBitmap.Width, _keyBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
            System.IntPtr Scan0K = bmDataK.Scan0;

            unsafe {
                byte* src = (byte*)(void*)Scan0F;
                byte* kb = (byte*)(void*)Scan0K;
                for (int y = 0; y < DisplayHeight; ++y) {
                    for (int x = 0; x < DisplayWidth; ++x) {

                        kb[0] = src[3];
                        kb[1] = src[3];
                        kb[2] = src[3];
                        kb[3] = 255;

                        kb += 4;
                        src += 4;
                    }
                }
            }

            _surfaceBitmap.UnlockBits(bmDataF);
            _keyBitmap.UnlockBits(bmDataK);

            dlFill.FrameBitmap = _surfaceBitmap;
            dlKey.FrameBitmap = _keyBitmap;

            frameCounter++;

        }

        public double GetActualPerFrameLength()
        {
            double time = (double)watch.ElapsedMilliseconds / (double)frameCounter;
            return time;
        }
        
    }
}
