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
    public class DLAccessForSingle
    {
        private IDLSingleOutput output;

        private DLSetingsForSingle settings;
        public DLSetingsForSingle Settings
        {
            get { return settings; }
            set { settings = value; }
        }
        
        public Action<Graphics> FrameCallback { get; set; }

        private string uniqueId;

        private bool isFake;

        private bool setForKeyedBottom;

        public DLAccessForSingle(string uniqueId)
        {
            settings = DLSetingsForSingle.Load(uniqueId);

            this.uniqueId = uniqueId;
        }

        public void SetForKeyedOnTop()
        {

            var t = (DLSingleOutput)output;
            t.KeyFromBottomHalf = true;
        }

        public void NoKeyedOnTop()
        {
            var t = (DLSingleOutput)output;
            t.KeyFromBottomHalf = false;
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
            output = new DLSingleOutput(settings.DLIndex, settings.DLDMIndex);
            output.DrawFrameGFXCallback = CallBack;
            output.Start();
        }

        public void ConnectFakeDecklink()
        {
            isFake = true;
            int ms = (int)((double)1 / settings.FakeDLFPS * 1000);
            output = new DLSingleOutputFake(settings.FakeDLWidth, settings.FakeDLHeight, ms, Settings.HideFakeKeyWindow);
            output.DrawFrameGFXCallback = CallBack;
            output.Start();
        }

        public void ShowPreviewWindow(bool show)
        {
            output.ShowPreviewWindow(show);
        }

        public void ShowDecklinkSettings()
        {
            var sf = new DLSettingsFormForSingle();
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
