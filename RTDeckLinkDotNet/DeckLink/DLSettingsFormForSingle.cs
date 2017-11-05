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
    public partial class DLSettingsFormForSingle : Form
    {
        private bool foundDL = false;

        public DLSetingsForSingle Settings { get; set; }

        public DLSettingsFormForSingle()
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

            }
            catch
            {
                foundDL = false;
                lstDL1.Items.Add("No DeckLinks found.");
                lstDL1DM.Items.Add("No DeckLinks found.");
            }

            if (foundDL) {
                // try to set selected index to the stored value in the settings - may in some cases be invalid
                try {
                    lstDL1.SelectedIndex = Settings.DLIndex;
                    lstDL1DM.SelectedIndex = Settings.DLDMIndex;
                }
                catch {
                    if (lstDL1.Items.Count > 0)  lstDL1.SelectedIndex = 0;
                    if (lstDL1DM.Items.Count > 0) lstDL1DM.SelectedIndex = 0;
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
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           
            if (foundDL)
            {
                Settings.DLIndex = lstDL1.SelectedIndex;

                Settings.DLDMIndex = lstDL1DM.SelectedIndex;
  
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



    }
}
