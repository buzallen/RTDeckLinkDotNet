using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DLKeyFillLib;

namespace AppCommon
{
    public partial class DLSettingsFormForDual : Form
    {
        private bool foundDL = false;

        public DLSettingsForDual Settings { get; set; }

        public DLSettingsFormForDual()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
           
            // try to load the decklinks into the listboxes
            try
            {
                PopulateDLInfo();
                foundDL = true;

                if (lstDL1.Items.Count == 0) {
                    lstDL1.Items.Add("No DeckLinks found.");
                }

                if (lstDL2.Items.Count == 0) {
                    lstDL2.Items.Add("No DeckLinks found.");
                }
            }
            catch
            {
                foundDL = false;
                lstDL1.Items.Add("No DeckLinks found.");
                lstDL2.Items.Add("No DeckLinks found.");
                lstDL1DM.Items.Add("No DeckLinks found.");
                lstDL2DM.Items.Add("No DeckLinks found.");
            }

            if (foundDL) {
                // try to set selected index to the stored value in the settings - may in some cases be invalid
                try {
                    lstDL1.SelectedIndex = Settings.DL1Index;
                    lstDL2.SelectedIndex = Settings.DL2Index;
                    lstDL1DM.SelectedIndex = Settings.DL1DMIndex;
                    lstDL2DM.SelectedIndex = Settings.DL2DMIndex;
                }
                catch {
                    if (lstDL1.Items.Count > 0)  lstDL1.SelectedIndex = 0;
                    if (lstDL2.Items.Count > 0)  lstDL2.SelectedIndex = 0;
                    if (lstDL1DM.Items.Count > 0) lstDL1DM.SelectedIndex = 0;
                    if (lstDL2DM.Items.Count > 0) lstDL2DM.SelectedIndex = 0;
                }
            }

            chkRightToLeft.Checked = Settings.RightToLeft;
            nudWidth.Value = Settings.FakeDLWidth;
            nudHeight.Value = Settings.FakeDLHeight;
            nudFPS.Value = Settings.FakeDLFPS;
        }

        private void PopulateDLInfo()
        {
            List<DeckLinkItem> dls = DLKeyFillLib.DLHelper.GetDeckLinkItems();
            foreach (DeckLinkItem dli in dls)
            {
                lstDL1.Items.Add(dli);
                lstDL2.Items.Add(dli);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           
            if (foundDL)
            {
                Settings.DL1Index = lstDL1.SelectedIndex;
                Settings.DL2Index = lstDL2.SelectedIndex;
                Settings.DL1DMIndex = lstDL1DM.SelectedIndex;
                Settings.DL2DMIndex = lstDL2DM.SelectedIndex;
            }

            Settings.RightToLeft = chkRightToLeft.Checked;
            Settings.FakeDLWidth = (int)nudWidth.Value;
            Settings.FakeDLHeight = (int)nudHeight.Value;
            Settings.FakeDLFPS = (int)nudFPS.Value;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void lstDL1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstDL1.SelectedIndex > -1)
            {
                try {
                    DeckLinkItem dli = (DeckLinkItem)lstDL1.SelectedItem;
                    List<DisplayModeItem> dmis = DLHelper.GetDisplayModeItems(dli.deckLink);
                    lstDL1DM.Items.Clear();
                    foreach (DisplayModeItem dmi in dmis) {
                        lstDL1DM.Items.Add(dmi);
                    }
                }
                catch { }
            }
        }

        private void lstDL2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                if (lstDL2.SelectedIndex > -1) {
                    DeckLinkItem dli = (DeckLinkItem)lstDL2.SelectedItem;
                    List<DisplayModeItem> dmis = DLHelper.GetDisplayModeItems(dli.deckLink);
                    lstDL2DM.Items.Clear();
                    foreach (DisplayModeItem dmi in dmis) {
                        lstDL2DM.Items.Add(dmi);
                    }
                }
            }
            catch { }
        }

        private void lstDL2DM_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstDL1DM_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
