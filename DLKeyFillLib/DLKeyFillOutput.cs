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
    public class DLKeyFillOutput : IDLKeyFillOutput
    {

        // IDLKeyFillOutput members
        public int DisplayWidth { get; set; }
        public int DisplayHeight { get; set; }
        public int PreRollFrames { get; set; }
        public Action<Graphics> DrawFrameGFXCallback { get; set; }

        // DeckLink outputs
        private DLDirect outputFill;
        private DLDirect outputKey;

        private Bitmap _surfaceBitmap;
        private Graphics _surfaceGFX;

        private IntPtr fillBuffer;
        private IntPtr keyBuffer;

        private Rectangle _surfaceRectangle;

        private bool clearScreenRequest;

        private Stopwatch watch = new Stopwatch();
        private int frameCounter = 0;

        private PreviewWindow previewWindow = null;
        private bool previewOn = false;
        
        public DLKeyFillOutput()
        {
            PreRollFrames = 4;
        }

        public void ShowPreviewWindow(bool show)
        {
            if (show)
            {
                if (previewWindow == null)
                {
                    previewWindow = new PreviewWindow();
                    previewWindow.InitPreview(DisplayWidth, DisplayHeight);
                }
                previewWindow.Show();
                previewOn = true;

            }
            else
            {
                previewOn = false;
                previewWindow.Hide();
            }
        }

        public DLKeyFillOutput(int DL1Idx, int DL2Idx, int DL1DisplayModeIdx, int DL2DisplayModeIdx)
        {
            PreRollFrames = 4;
            ConfigureDeckLinks(DL1Idx, DL2Idx, DL1DisplayModeIdx, DL2DisplayModeIdx); 
        }

        public void ConfigureDeckLinks(int DL1Idx, int DL2Idx, int DL1DisplayModeIdx, int DL2DisplayModeIdx)
        {
            IDeckLink dl1 = DLHelper.GetDeckLinks()[DL1Idx];
            IDeckLink dl2 = DLHelper.GetDeckLinks()[DL2Idx];

            IDeckLinkDisplayMode dl1DisplayMode = DLHelper.GetDisplayModes(dl1)[DL1DisplayModeIdx];
            IDeckLinkDisplayMode dl2DisplayMode = DLHelper.GetDisplayModes(dl2)[DL2DisplayModeIdx];

            outputFill = new DLDirect(dl1, dl1DisplayMode);
            outputKey = new DLDirect(dl2, dl2DisplayMode);

            outputFill.FrameDrawnCallback = NextFrame;
            outputKey.IsKey = true;

            DisplayWidth = outputFill.DisplayWidth;
            DisplayHeight = outputFill.DisplayHeight;
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
                    DrawFrameGFXCallback(_surfaceGFX);
            }
            catch (InvalidOperationException ex) {
                Trace.WriteLine("@@ DLKeyFillOutputWK_DrawFrame @@ " + ex.Message.ToString());
            }


            if (previewOn) {
                try {
                    
                    previewWindow.ActiveBitmap = _surfaceBitmap;
                }
                catch (InvalidOperationException ex) {
                    Trace.WriteLine("@@ DLKeyFillOutputWK_PreviewCall @@ " + ex.Message.ToString());
                }
            }
            else {
                if (clearScreenRequest) {
                    _surfaceGFX.Clear(Color.Transparent);
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
           
            outputFill.ScheduleFrame();
            outputKey.ScheduleFrame();

            frameCounter++;
        }

        private void CopyToBuffers()
        {
            System.Drawing.Imaging.BitmapData bmpData =
                _surfaceBitmap.LockBits(_surfaceRectangle, System.Drawing.Imaging.ImageLockMode.ReadWrite, _surfaceBitmap.PixelFormat);

            //Get the address of the first line.
            IntPtr Scan0 = bmpData.Scan0;
            int stride = bmpData.Stride;

            unsafe {
                byte* fb = (byte*)(void*)fillBuffer;
                byte* kb = (byte*)(void*)keyBuffer;
                byte* src = (byte*)(void*)Scan0;
                for (int y = 0; y < DisplayHeight; ++y) {
                    for (int x = 0; x < DisplayWidth; ++x) {

                        fb[0] = src[0];
                        fb[1] = src[1];
                        fb[2] = src[2];
                        fb[3] = src[3];

                        kb[0] = src[3];
                        kb[1] = src[3];
                        kb[2] = src[3];
                        kb[3] = 255;

                        fb += 4;
                        kb += 4; 
                        src += 4;
                    }
                }
            }

            _surfaceBitmap.UnlockBits(bmpData);
        }

        public void Start()
        {
            outputFill.PreRollFrameCount = PreRollFrames;
            outputKey.PreRollFrameCount = PreRollFrames;

            for (int i = 0; i < PreRollFrames; i++) {
                outputFill.PrerollFrame();
                outputKey.PrerollFrame();
            }

            _surfaceBitmap = new Bitmap(DisplayWidth, DisplayHeight, PixelFormat.Format32bppPArgb);
            _surfaceRectangle = new Rectangle(0, 0, DisplayWidth, DisplayHeight);

            outputFill.TheFrame.GetBytes(out fillBuffer);
            outputKey.TheFrame.GetBytes(out keyBuffer);

            _surfaceGFX = Graphics.FromImage(_surfaceBitmap);

            frameCounter = 0;
            watch.Reset();
            watch.Start();

        }

        public double GetActualPerFrameLength()
        {
            double time = (double)watch.ElapsedMilliseconds / (double)frameCounter;
            return time;
        }

        public void Stop()
        {
            outputFill.Stop();
            outputKey.Stop();
        }

    }
}