using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DLIDefinitions;
using System.Drawing;
using DeckLinkAPI;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace DLKeyFillLib
{
    public class DLDualOutput : IDLDualOutput
    {

        // IDLKeyFillOutput members
        public int DisplayWidth { get; set; }
        public int DisplayHeight { get; set; }
        public int PreRollFrames { get; set; }
        public Action<Graphics,Graphics> DrawFrameGFXCallback { get; set; }

        // DeckLink outputs
        private DLDirect output_A;
        private DLDirect output_B;

        private Bitmap _surfaceBitmap_A;
        private Graphics _surfaceGFX_A;

        private Bitmap _surfaceBitmap_B;
        private Graphics _surfaceGFX_B;

        private IntPtr outputBuffer_A;
        private IntPtr outputBuffer_B;

        private Rectangle _surfaceRectangle;

        private bool clearScreenRequest;

        private Stopwatch watch = new Stopwatch();
        private int frameCounter = 0;

        private PreviewWindow previewWindow_A = null;
        private PreviewWindow previewWindow_B = null;

        private bool previewOn = false;
       

        public void ShowPreviewWindow(bool show)
        {
            if (show)
            {
                if (previewWindow_A == null)
                {
                    previewWindow_A = new PreviewWindow();
                    previewWindow_A.InitPreview(DisplayWidth, DisplayHeight);
                }
                previewWindow_A.Show();

                if (previewWindow_B == null)
                {
                    previewWindow_B = new PreviewWindow();
                    previewWindow_B.InitPreview(DisplayWidth, DisplayHeight);
                }
                previewWindow_B.Show();

                previewOn = true;

            }
            else
            {
                previewOn = false;
                previewWindow_A.Hide();
                previewWindow_B.Hide();
            }
        }

        public DLDualOutput(int DL1Idx, int DL2Idx, int DL1DisplayModeIdx, int DL2DisplayModeIdx)
        {
            PreRollFrames = 8;
            ConfigureDeckLinks(DL1Idx, DL2Idx, DL1DisplayModeIdx, DL2DisplayModeIdx); 
        }

        public void ConfigureDeckLinks(int DL1Idx, int DL2Idx, int DL1DisplayModeIdx, int DL2DisplayModeIdx)
        {
            IDeckLink dl1 = DLHelper.GetDeckLinks()[DL1Idx];
            IDeckLink dl2 = DLHelper.GetDeckLinks()[DL2Idx];

            IDeckLinkDisplayMode dl1DisplayMode = DLHelper.GetDisplayModes(dl1)[DL1DisplayModeIdx];
            IDeckLinkDisplayMode dl2DisplayMode = DLHelper.GetDisplayModes(dl2)[DL2DisplayModeIdx];

            output_A = new DLDirect(dl1, dl1DisplayMode);
            output_B = new DLDirect(dl2, dl2DisplayMode);

            output_A.FrameDrawnCallback = NextFrame;

            DisplayWidth = output_A.DisplayWidth;
            DisplayHeight = output_A.DisplayHeight;
        }

        public void Clear()
        {
            clearScreenRequest = true;
        }

        public void NextFrame(int frameCount)
        {
            // get the frame
            try {
                if (DrawFrameGFXCallback != null)
                    DrawFrameGFXCallback(_surfaceGFX_A, _surfaceGFX_B);
            }
            catch (InvalidOperationException ex) {
                Trace.WriteLine("@@ DLKeyFillOutputWK_DrawFrame @@ " + ex.Message.ToString());
            }


            if (previewOn) {
                try {      
                    previewWindow_A.ActiveBitmap = _surfaceBitmap_A;
                    previewWindow_B.ActiveBitmap = _surfaceBitmap_B;

                }
                catch (InvalidOperationException ex) {
                    Trace.WriteLine("@@ DLKeyFillOutputWK_PreviewCall @@ " + ex.Message.ToString());
                }
            }
            else {
                if (clearScreenRequest) {
                    _surfaceGFX_A.Clear(Color.Transparent);
                    _surfaceGFX_B.Clear(Color.Transparent);

                    CopyToBuffers();
                    clearScreenRequest = false;
                }
                else {
                    try {
                        CopyToBuffers();
                    }
                    catch (InvalidOperationException ex) {
                        Trace.WriteLine("@@ DLKeyFillOutputWK_CopyToBuffers @@ " + ex.Message.ToString());
                    }
                }

            }
           
            output_A.ScheduleFrame();
            output_B.ScheduleFrame();

            frameCounter++;
        }

        private void CopyToBuffers()
        {
            System.Drawing.Imaging.BitmapData bmpData_A =
                _surfaceBitmap_A.LockBits(_surfaceRectangle, System.Drawing.Imaging.ImageLockMode.ReadWrite, _surfaceBitmap_A.PixelFormat);

            System.Drawing.Imaging.BitmapData bmpData_B =
                _surfaceBitmap_B.LockBits(_surfaceRectangle, System.Drawing.Imaging.ImageLockMode.ReadWrite, _surfaceBitmap_B.PixelFormat);

            //Get the address of the first line.
            IntPtr Scan0_A = bmpData_A.Scan0;
            int stride_A = bmpData_A.Stride;

            IntPtr Scan0_B = bmpData_B.Scan0;
            int stride_B = bmpData_B.Stride;

            unsafe {
                byte* oa = (byte*)(void*)outputBuffer_A;
                byte* ob = (byte*)(void*)outputBuffer_B;
                byte* src_A = (byte*)(void*)Scan0_A;
                byte* src_B = (byte*)(void*)Scan0_B;

                for (int y = 0; y < DisplayHeight; ++y) {
                    for (int x = 0; x < DisplayWidth; ++x) {

                        oa[0] = src_A[0];
                        oa[1] = src_A[1];
                        oa[2] = src_A[2];
                        oa[3] = src_A[3];

                        ob[0] = src_B[0];
                        ob[1] = src_B[1];
                        ob[2] = src_B[2];
                        ob[3] = src_B[3];

                        oa += 4;
                        ob += 4; 
                        src_A += 4;
                        src_B += 4;

                    }
                }
            }

            _surfaceBitmap_A.UnlockBits(bmpData_A);
            _surfaceBitmap_B.UnlockBits(bmpData_B);

        }

        public void Start()
        {
            output_A.PreRollFrameCount = PreRollFrames;
            output_B.PreRollFrameCount = PreRollFrames;

            for (int i = 0; i < PreRollFrames; i++) {
                output_A.PrerollFrame();
                output_B.PrerollFrame();
            }

            _surfaceBitmap_A = new Bitmap(DisplayWidth, DisplayHeight, PixelFormat.Format32bppPArgb);
            _surfaceBitmap_B = new Bitmap(DisplayWidth, DisplayHeight, PixelFormat.Format32bppPArgb);

            _surfaceRectangle = new Rectangle(0, 0, DisplayWidth, DisplayHeight);

            output_A.TheFrame.GetBytes(out outputBuffer_A);
            output_B.TheFrame.GetBytes(out outputBuffer_B);

            _surfaceGFX_A = Graphics.FromImage(_surfaceBitmap_A);
            _surfaceGFX_B = Graphics.FromImage(_surfaceBitmap_B);


            frameCounter = 0;
        }

        public double GetActualPerFrameLength()
        {
            double time = (double)watch.ElapsedMilliseconds / (double)frameCounter;
            return time;
        }

        public void Stop()
        {
            output_A.Stop();
            output_B.Stop();
        }

    }
}