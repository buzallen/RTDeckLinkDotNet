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
    public class DLSingleOutput : IDLSingleOutput
    {

        // IDLKeyFillOutput members
        public int DisplayWidth { get; set; }
        public int DisplayHeight { get; set; }
        public int PreRollFrames { get; set; }
        public Action<Graphics> DrawFrameGFXCallback { get; set; }

        // DeckLink outputs
        private DLDirect output;

        private Bitmap _surfaceBitmap;
        private Graphics _surfaceGFX;

        private IntPtr fillBuffer;

        private Rectangle _surfaceRectangle;

        private bool clearScreenRequest;

        private Stopwatch watch = new Stopwatch();
        private int frameCounter = 0;

        private PreviewWindow previewWindow = null;
        private bool previewOn = false;


        public bool KeyFromBottomHalf { get; set; }

        public DLSingleOutput()
        {
            PreRollFrames = 20;
            KeyFromBottomHalf = true;
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

        public DLSingleOutput(int DLIdx, int DLDisplayModeIdx)
        {
            PreRollFrames = 4;
            ConfigureDeckLinks(DLIdx, DLDisplayModeIdx); 
        }

        public void ConfigureDeckLinks(int DLIdx, int DLDisplayModeIdx)
        {
            IDeckLink dl = DLHelper.GetDeckLinks()[DLIdx];

            IDeckLinkDisplayMode dl1DisplayMode = DLHelper.GetDisplayModes(dl)[DLDisplayModeIdx];

            output = new DLDirect(dl, dl1DisplayMode);

            output.FrameDrawnCallback = NextFrame;

            DisplayWidth = output.DisplayWidth;
            DisplayHeight = output.DisplayHeight;
        }

        public void Clear()
        {
            clearScreenRequest = true;
        }



        public void NextFrame(int frameCount)
        {
            // get the frame
            try {
                DrawFrameGFXCallback?.Invoke(_surfaceGFX);
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
           
            output.ScheduleFrame();

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
                byte* src = (byte*)(void*)Scan0;
                for (int y = 0; y < DisplayHeight; ++y) {
                    for (int x = 0; x < DisplayWidth; ++x) {

                        fb[0] = src[0];
                        fb[1] = src[1];
                        fb[2] = src[2];
                        fb[3] = src[3];

                        fb += 4;

                        src += 4;
                    }
                }
            }
            if (KeyFromBottomHalf) {
                unsafe {
                    byte* src = (byte*)(void*)fillBuffer;
                    byte* dest = (byte*)(void*)fillBuffer;

                    src += ((DisplayHeight / 2) * DisplayWidth * 4);
                    for (int y = 0; y < DisplayHeight/2; ++y) {
                        for (int x = 0; x < DisplayWidth; ++x) {
                            dest[0] = src[3];
                            dest[1] = src[3];
                            dest[2] = src[3];
                            dest[3] = 255;
                            dest += 4;
                            src += 4;
                        }
                    }
                }
            }

            _surfaceBitmap.UnlockBits(bmpData);
        }

        public void Start()
        {
            output.PreRollFrameCount = PreRollFrames;

            for (int i = 0; i < PreRollFrames; i++) {
                output.PrerollFrame();
            }

            _surfaceBitmap = new Bitmap(DisplayWidth, DisplayHeight, PixelFormat.Format32bppPArgb);
            _surfaceRectangle = new Rectangle(0, 0, DisplayWidth, DisplayHeight);

            output.TheFrame.GetBytes(out fillBuffer);

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
            output.Stop();
        }

    }
}