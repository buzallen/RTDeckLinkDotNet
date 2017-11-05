using DLIDefinitions;
using DLKeyFillLib;
using DLKeyFillLibFake;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppCommon
{
    public class DLAccessForKeyFill
    {
        private IDLKeyFillOutput output;

        private DLSettingsForKeyFill settings;
        public DLSettingsForKeyFill Settings
        {
            get { return settings; }
            set { settings = value; }
        }
        

        public Action<Graphics> FrameCallback { get; set; }

        private string uniqueId;

        private bool isFake;

        public DLAccessForKeyFill(string uniqueId)
        {
            settings = DLSettingsForKeyFill.Load(uniqueId);
            this.uniqueId = uniqueId;
        }

        public void ConnectRealDecklink()
        {
            isFake = false;
            Thread thread = new Thread(InitMTAThreadForRealDeckLink);
            thread.SetApartmentState(ApartmentState.MTA);
            thread.Start();
        }

        private void InitMTAThreadForRealDeckLink()
        {
            output = new DLKeyFillOutput(settings.DL1Index, settings.DL2Index, settings.DL1DMIndex, settings.DL2DMIndex);
            output.DrawFrameGFXCallback = CallBack;
            output.Start();
        }

        public void ConnectFakeDecklink()
        {
            isFake = true;
            int ms = (int)((double)1 / settings.FakeDLFPS * 1000);
            output = new DLKeyFillOutputWKFake(settings.FakeDLWidth, settings.FakeDLHeight, ms, Settings.HideFakeKeyWindow);
            output.DrawFrameGFXCallback = CallBack;
            output.Start();
        }

        public void ShowPreviewWindow(bool show)
        {
            output.ShowPreviewWindow(show);
        }

        public void ShowDecklinkSettings()
        {
            var sf = new DLSettingsFormForKeyFill();
            sf.Settings = settings;
            if (sf.ShowDialog() == DialogResult.OK) {
                settings.Save(uniqueId);
            }
        }

        public void CallBack(Graphics g)
        {
            FrameCallback(g);
        }

        public void Close()
        {
            if (isFake)
                output.Stop();
        }

    }
}
