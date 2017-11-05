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
    public class DLSettingsForDual
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
            XmlSerializer xms = new XmlSerializer(typeof(DLSettingsForDual));
            xms.Serialize(writer, this);
            writer.Close();
        }


        public static DLSettingsForDual Load(string id)
        {
            DLSettingsForDual returnSettings = null;
            var name = System.AppDomain.CurrentDomain.FriendlyName;
            string adp = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + name;
            adp += "\\" + id + "-decklink_settings.xml";
            if (File.Exists(adp)) {

                try {
                    StreamReader reader = new StreamReader(adp);
                    XmlSerializer xms = new XmlSerializer(typeof(DLSettingsForDual));
                    returnSettings = (DLSettingsForDual)xms.Deserialize(reader);
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

        public static DLSettingsForDual GetNewShowSettings()
        {
            var settings = new DLSettingsForDual();

            return settings;
        }
    }
}
