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
    public class DLAccessForDual
    {
        private IDLDualOutput output;

        private DLSettingsForDual settings;
        public DLSettingsForDual Settings
        {
            get { return settings; }
            set { settings = value; }
        }   

        public Action<Graphics,Graphics> FrameCallback { get; set; }

        private string uniqueId;

        private bool isFake;

        public DLAccessForDual(string uniqueId)
        {
            settings = DLSettingsForDual.Load(uniqueId);
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
            output = new DLDualOutput(settings.DL1Index, settings.DL2Index, settings.DL1DMIndex, settings.DL2DMIndex);
            output.DrawFrameGFXCallback = CallBack;
            output.Start();
        }

        public void ConnectFakeDecklink()
        {
            isFake = true;
            int ms = (int)((double)1 / settings.FakeDLFPS * 1000);
            output = new DLDualOutputFake(settings.FakeDLWidth, settings.FakeDLHeight, ms);
            output.DrawFrameGFXCallback = CallBack;
            output.Start();
        }

        public void ShowPreviewWindow(bool show)
        {
            output.ShowPreviewWindow(show);
        }

        public void ShowDecklinkSettings()
        {
            var sf = new DLSettingsFormForDual();
            sf.Settings = settings;
            if (sf.ShowDialog() == DialogResult.OK) {
                settings.Save(uniqueId);
            }
        }

        public void CallBack(Graphics ga, Graphics gb)
        {
            FrameCallback(ga, gb);
        }

        public void Close()
        {
            if (isFake)
                output.Stop();
        }

    }
}
