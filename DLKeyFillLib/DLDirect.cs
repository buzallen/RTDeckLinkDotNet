using System;
using System.Collections.Generic;
using System.Text;
using DeckLinkAPI;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;


namespace DLKeyFillLib
{

    public class DLDirect : IDeckLinkVideoOutputCallback
    {
        public Action<int> FrameDrawnCallback { get; set; }
        public IDeckLinkMutableVideoFrame TheFrame
        {
            get { return m_videoFramePlayback; }
        }
        public bool IsKey { get; set; }

        public int PreRollFrameCount { get; set; }

        private bool m_running;
        private IDeckLink m_deckLink;
        private IDeckLinkOutput m_deckLinkOutput;
        //
        private int m_frameWidth;
        private int m_frameHeight;
        private long m_frameDuration;
        private long m_frameTimescale;
        private uint m_framesPerSecond;

        private IDeckLinkMutableVideoFrame m_videoFramePlayback;
        private uint m_totalFramesScheduled;
        //
        private IDeckLinkDisplayMode displayMode;

        private bool preRollOn = false;

        public int DisplayWidth = 0;
        public int DisplayHeight = 0;
        public int FPS = 0;

        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public unsafe static extern void CopyMemory(IntPtr pDest, IntPtr pSrc, int length);


        public DLDirect(IDeckLink deckLink, IDeckLinkDisplayMode displayMode)
        {
            PreRollFrameCount = 4;

            this.displayMode = displayMode;
            this.m_deckLink = deckLink;

            DisplayWidth = displayMode.GetWidth();
            DisplayHeight = displayMode.GetHeight();

            m_running = false;

            // Get the IDeckLinkOutput interface
            m_deckLinkOutput = (IDeckLinkOutput)m_deckLink;

            // Provide this class as a delegate to the audio and video output interfaces
            m_deckLinkOutput.SetScheduledFrameCompletionCallback(this);

            m_frameWidth = displayMode.GetWidth();
            m_frameHeight = displayMode.GetHeight();

            displayMode.GetFrameRate(out m_frameDuration, out m_frameTimescale);

            // Calculate the number of frames per second, rounded up to the nearest integer.  For example, for NTSC (29.97 FPS), framesPerSecond == 30.
            m_framesPerSecond = (uint)((m_frameTimescale + (m_frameDuration - 1)) / m_frameDuration);
            FPS = (int)m_framesPerSecond;

            // Set the video output mode
            m_deckLinkOutput.EnableVideoOutput(displayMode.GetDisplayMode(), _BMDVideoOutputFlags.bmdVideoOutputFlagDefault);

            // Generate the frame used for playback
            m_deckLinkOutput.CreateVideoFrame(m_frameWidth, m_frameHeight, m_frameWidth * 4, _BMDPixelFormat.bmdFormat8BitBGRA, _BMDFrameFlags.bmdFrameFlagDefault, out m_videoFramePlayback);
            m_totalFramesScheduled = 0;

        }

        
        private void StartRunning()
        {
            m_running = true;
            m_deckLinkOutput.StartScheduledPlayback(0, m_frameTimescale, 1);
        }

        public void PrerollFrame()
        {
            // if this is the first call then set the flag that we are prerolling
            if (m_totalFramesScheduled == 0) preRollOn = true;

            // make sure we are in a preroll state and schedule frame if so
            if (preRollOn) {
                FillBlack(m_videoFramePlayback);
                ScheduleFrame();
            }

            // if we are over the requestd pre-roll frame count the disable the preroll and runn the cards
            if (m_totalFramesScheduled >= PreRollFrameCount) {
                preRollOn = false;
                StartRunning();
            }
        }

        public void ScheduleFrame()
        {
            if (m_running || preRollOn) {
                m_deckLinkOutput.ScheduleVideoFrame(m_videoFramePlayback, (m_totalFramesScheduled * m_frameDuration), m_frameDuration, m_frameTimescale);
                m_totalFramesScheduled += 1;
            }
        }

        void FillBlack(IDeckLinkVideoFrame theFrame)
        {
            IntPtr buffer;
            int width, height;
            UInt32[] bars = { 0x11111111, 0xAAAAAAAA, 0x11111111, 0xAAAAAAAA, 0x11111111, 0xAAAAAAAA, 0x11111111, 0xAAAAAAAA };
            int index = 0;

            theFrame.GetBytes(out buffer);
            width = theFrame.GetWidth();
            height = theFrame.GetHeight();

            for (uint y = 0; y < height; y++) {
                for (uint x = 0; x < width; x += 1) {
                    // Write directly into unmanaged buffer
                    Marshal.WriteInt32(buffer, index * 4, (Int32)bars[(x * 8) / width]);
                    index++;
                }
            }
        }

        // Explicit implementation of IDeckLinkVideoOutputCallback and IDeckLinkAudioOutputCallback
        void IDeckLinkVideoOutputCallback.ScheduledFrameCompleted(IDeckLinkVideoFrame completedFrame, _BMDOutputFrameCompletionResult result)
        {
            if (FrameDrawnCallback != null) FrameDrawnCallback((int)m_totalFramesScheduled);
        }

        void IDeckLinkVideoOutputCallback.ScheduledPlaybackHasStopped()
        {
        }

        public void Stop()
        {


            if (m_running) {
                m_running = false;
                //m_deckLinkOutput.DisableAudioOutput();


                long unused;
                m_deckLinkOutput.StopScheduledPlayback(0, out unused, 100);

                m_deckLinkOutput.DisableVideoOutput();

            }
        }
    }
}