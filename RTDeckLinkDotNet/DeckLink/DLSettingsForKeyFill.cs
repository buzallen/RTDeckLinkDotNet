using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AppCommon
{
    [Serializable]
    public class DLSettingsForKeyFill
    {

        public int DL1Index { get; set; }

        public int DL2Index { get; set; }

        public int DL1DMIndex { get; set; }

        public int DL2DMIndex { get; set; }

        public bool RightToLeft { get; set; }

        public int FakeDLWidth { get; set; }

        public int FakeDLHeight { get; set; }

        public int FakeDLFPS { get; set; }

        public bool HideFakeKeyWindow { get; set; }

        public void Save(string id)
        {
            var name = System.AppDomain.CurrentDomain.FriendlyName;

            string adp = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + name;

            if (!Directory.Exists(adp)) {
                Directory.CreateDirectory(adp);
            }

            adp += "\\" + id + "-decklink_settings.xml";
            StreamWriter writer = new StreamWriter(adp);
            XmlSerializer xms = new XmlSerializer(typeof(DLSettingsForKeyFill));
            xms.Serialize(writer, this);
            writer.Close();
        }


        public static DLSettingsForKeyFill Load(string id)
        {
            DLSettingsForKeyFill returnSettings = null;
            var name = System.AppDomain.CurrentDomain.FriendlyName;
            string adp = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + name;
            adp += "\\" + id + "-decklink_settings.xml";
            if (File.Exists(adp)) {

                try {
                    StreamReader reader = new StreamReader(adp);
                    XmlSerializer xms = new XmlSerializer(typeof(DLSettingsForKeyFill));
                    returnSettings = (DLSettingsForKeyFill)xms.Deserialize(reader);
                    reader.Close();
                }
                catch {
                    returnSettings = GetNewShowSettings();
                }
            }
            else {

                return GetNewShowSettings();
            }

            return returnSettings;

        }

        public static DLSettingsForKeyFill GetNewShowSettings()
        {
            var settings = new DLSettingsForKeyFill();

            return settings;
        }
    }
}
