using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeckLinkAPI;
using System.Windows.Forms;

namespace DLKeyFillLib
{
    public class DLHelper
    {

        public static List<IDeckLink> GetDeckLinks()
        {
            List<IDeckLink> outList = new List<IDeckLink>();

            // Create the COM instance
            IDeckLinkIterator deckLinkIterator = new CDeckLinkIterator();
            if (deckLinkIterator == null) {
                MessageBox.Show("This application requires the DeckLink drivers installed.\nPlease install the Blackmagic DeckLink drivers to use the features of this application", "Error");
                Environment.Exit(1);
            }

            while (true) {
                IDeckLink deckLink;
                deckLinkIterator.Next(out deckLink);
                if (deckLink == null) break;
                outList.Add(deckLink);
            }

            return outList;
        }

        public static List<IDeckLinkDisplayMode> GetDisplayModes(IDeckLink deckLink)
        {
            List<IDeckLinkDisplayMode> outList = new List<IDeckLinkDisplayMode>();

            IDeckLinkOutput m_deckLinkOutput = (IDeckLinkOutput)deckLink;

            IDeckLinkDisplayModeIterator displayModeIterator;

            m_deckLinkOutput.GetDisplayModeIterator(out displayModeIterator);

            while (true) {
                IDeckLinkDisplayMode deckLinkDisplayMode;

                displayModeIterator.Next(out deckLinkDisplayMode);
                if (deckLinkDisplayMode == null)
                    break;

                outList.Add(deckLinkDisplayMode);
            }

            return outList;
        }

        public static List<DeckLinkItem> GetDeckLinkItems()
        {
            List<DeckLinkItem> outList = new List<DeckLinkItem>();


            // Create the COM instance
            IDeckLinkIterator deckLinkIterator = new CDeckLinkIterator();
            if (deckLinkIterator == null) {
                MessageBox.Show("This application requires the DeckLink drivers installed.\nPlease install the Blackmagic DeckLink drivers to use the features of this application", "Error");
                Environment.Exit(1);
            }


            while (true) {
                IDeckLink deckLink;
                deckLinkIterator.Next(out deckLink);
                if (deckLink == null) break;

                outList.Add(new DeckLinkItem(deckLink));
            }

            return outList;
        }


        public static List<DisplayModeItem> GetDisplayModeItems(IDeckLink m_deckLink)
        {
            List<DisplayModeItem> outList = new List<DisplayModeItem>();

            IDeckLinkOutput m_deckLinkOutput = (IDeckLinkOutput)m_deckLink;

            IDeckLinkDisplayModeIterator displayModeIterator;

            m_deckLinkOutput.GetDisplayModeIterator(out displayModeIterator);

            while (true) {
                IDeckLinkDisplayMode deckLinkDisplayMode;

                displayModeIterator.Next(out deckLinkDisplayMode);
                if (deckLinkDisplayMode == null)
                    break;

                outList.Add(new DisplayModeItem(deckLinkDisplayMode));
            }

            return outList;
        }
    }

    public struct DisplayModeItem
    {
        public IDeckLinkDisplayMode displayMode;

        public DisplayModeItem(IDeckLinkDisplayMode displayMode)
        {
            this.displayMode = displayMode;
        }

        public override string ToString()
        {
            string strn;
            displayMode.GetName(out strn);
            string str = String.Format("{0},{1} : {2}", displayMode.GetWidth(), displayMode.GetHeight(), strn);

            return str;
        }
    }

    public struct DeckLinkItem
    {
        public IDeckLink deckLink;

        public DeckLinkItem(IDeckLink deckLink)
        {
            this.deckLink = deckLink;
        }

        public override string ToString()
        {
            string str;
            deckLink.GetModelName(out str);
            return str;
        }
    }
}
