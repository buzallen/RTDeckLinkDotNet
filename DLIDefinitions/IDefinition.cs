using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DLIDefinitions
{
    public class NextFrameArgs : EventArgs
    {
        private int _frameCount;
        public int FrameCount
        {
        	get {	return _frameCount; }
        	set { _frameCount = value;	}
        }
    }

    public interface IDLKeyFillOutput
    {
        int DisplayWidth { get; set; }
        int DisplayHeight { get; set; }
        int PreRollFrames { get; set; }
        Action<Graphics> DrawFrameGFXCallback { get; set; }
        void ShowPreviewWindow(bool show);
        void Start();
        void Stop();
        void Clear();
        double GetActualPerFrameLength();
        void ConfigureDeckLinks(int DL1Idx, int DL2Idx, int DL1DisplayModeIdx, int DL2DisplayModeIdx);
    }

    public interface IDLDualOutput
    {
        int DisplayWidth { get; set; }
        int DisplayHeight { get; set; }
        int PreRollFrames { get; set; }
        Action<Graphics,Graphics> DrawFrameGFXCallback { get; set; }
        void ShowPreviewWindow(bool show);
        void Start();
        void Stop();
        void Clear();
        double GetActualPerFrameLength();
        void ConfigureDeckLinks(int DL1Idx, int DL2Idx, int DL1DisplayModeIdx, int DL2DisplayModeIdx);
    }

    public interface IDLSingleOutput
    {
        int DisplayWidth { get; set; }
        int DisplayHeight { get; set; }
        int PreRollFrames { get; set; }
        Action<Graphics> DrawFrameGFXCallback { get; set; }
        void ShowPreviewWindow(bool show);
        void Start();
        void Stop();
        void Clear();
        double GetActualPerFrameLength();
        void ConfigureDeckLinks(int DLIdx, int DLDisplayModeIdx);
    }

}
