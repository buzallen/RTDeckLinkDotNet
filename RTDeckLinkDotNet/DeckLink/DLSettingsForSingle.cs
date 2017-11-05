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
    public class DLSetingsForSingle
    {

        public int DLIndex { get; set; }

        public int DLDMIndex { get; set; }

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
            XmlSerializer xms = new XmlSerializer(typeof(DLSetingsForSingle));
            xms.Serialize(writer, this);
            writer.Close();
        }


        public static DLSetingsForSingle Load(string id)
        {
            DLSetingsForSingle returnSettings = null;
            var name = System.AppDomain.CurrentDomain.FriendlyName;
            string adp = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + name;
            adp += "\\" + id + "-decklink_settings.xml";
            if (File.Exists(adp)) {

                try {
                    StreamReader reader = new StreamReader(adp);
                    XmlSerializer xms = new XmlSerializer(typeof(DLSetingsForSingle));
                    returnSettings = (DLSetingsForSingle)xms.Deserialize(reader);
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

        public static DLSetingsForSingle GetNewShowSettings()
        {
            var settings = new DLSetingsForSingle();

            return settings;
        }
    }
}
