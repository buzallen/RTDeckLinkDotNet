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
    public class DLDualOutputFake : IDLDualOutput
    {
        // IDLKeyFillOutput members
        public int DisplayWidth { get; set; }
        public int DisplayHeight { get; set; }
        public int FPS { get; set; }
        public int PreRollFrames { get; set; }
        public Action<Graphics,Graphics> DrawFrameGFXCallback { get; set; }

        private Bitmap _surfaceBitmap_A;
        private Bitmap _surfaceBitmap_B;
        private Graphics _surfaceGFX_A;
        private Graphics _surfaceGFX_B;


        private bool clearScreenRequest;

        // This timer fakes the next frame callback from the decklink hardware
        private Timer timer = new Timer();

        // Key and fill fake outputs
        private DLMockOutputForm dlOutputWindowA = new DLMockOutputForm();
        private DLMockOutputForm dlOutputWindowB = new DLMockOutputForm();

        // Next frame arguments
        private NextFrameArgs nextFrameArgs = new NextFrameArgs();

        private Stopwatch watch = new Stopwatch();
        private int frameCounter = 0;

        public DLDualOutputFake(int width, int height, int frameLength)
        {
            PreRollFrames = 4;
            DisplayWidth = width;
            DisplayHeight = height;
            timer.Interval = frameLength;
            ConfigureDeckLinks(0, 0, 0, 0);
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
            _surfaceBitmap_A = new Bitmap(DisplayWidth, DisplayHeight, PixelFormat.Format32bppPArgb);
            _surfaceBitmap_B = new Bitmap(DisplayWidth, DisplayHeight, PixelFormat.Format32bppPArgb);


            _surfaceGFX_A = Graphics.FromImage(_surfaceBitmap_A);
            _surfaceGFX_B = Graphics.FromImage(_surfaceBitmap_B);


            dlOutputWindowA.BitmapSize = new Size(DisplayWidth, DisplayHeight);
            dlOutputWindowB.BitmapSize = new Size(DisplayWidth, DisplayHeight);

            double ratio = (double)DisplayWidth / (double)DisplayHeight;
            double startingWidth = 600.0;
            double startingHeight = 600.0 / ratio;

            Size clSize = new Size((int)startingWidth, (int)startingHeight);
            dlOutputWindowA.ClientSize = clSize;
            dlOutputWindowB.ClientSize = clSize;

            dlOutputWindowA.Ratio = ratio;
            dlOutputWindowB.Ratio = ratio;

            dlOutputWindowA.Text = "Output A";
            dlOutputWindowB.Text = "Output B";


            timer.Start();

            watch.Reset();
            watch.Start();
        }

        public void Stop()
        {
            timer.Stop();
            dlOutputWindowA.Close();
            dlOutputWindowB.Close();
        }


        void timer_Tick(object sender, EventArgs e)
        {
            if (clearScreenRequest == true) {
                _surfaceGFX_A.Clear(Color.Transparent);
                _surfaceGFX_B.Clear(Color.Transparent);

                clearScreenRequest = false;
            }
            else
            {
                DrawFrameGFXCallback?.Invoke(_surfaceGFX_A, _surfaceGFX_B);
            }

            dlOutputWindowA.FrameBitmap = _surfaceBitmap_A;
            dlOutputWindowB.FrameBitmap = _surfaceBitmap_B;

            frameCounter++;

        }

        public double GetActualPerFrameLength()
        {
            double time = (double)watch.ElapsedMilliseconds / (double)frameCounter;
            return time;
        }
        
    }
}
