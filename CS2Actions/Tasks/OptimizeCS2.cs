using JCorePanelBase.Structures;
using JCorePanelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CS2Actions.Properties;
using System.Security.Principal;
using System.Windows.Media;
using System.IO.Compression;
using System.IO;

namespace CS2Actions.Tasks
{
    public class RunCS2Event : JCEventBase
    {
        public string Name = "Optimize CS2";

        public RunCS2Event(List<JCEventProperty> PropertiesList) : base(PropertiesList)
        {
        }

        public void EventBody(List<JCSteamAccountInstance> Accounts)
        {
            DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(GlobalMenager.GetSteamPath()) + "\\steamapps\\common\\Counter-Strike Global Offensive\\game\\csgo\\panorama\\videos");

            foreach (FileInfo file in directory.GetFiles())
            {
                if (file.Name.ToLower() == "intro.webm") continue;
                if (file.Name.ToLower() == "intro720p.webm") continue;
                if (file.Name.ToLower() == "intro-perfectworld.webm") continue;
                if (file.Name.ToLower() == "intro-perfectworld720p.webm") continue;

                file.Delete();
            }
            directory = new DirectoryInfo(Path.GetDirectoryName(GlobalMenager.GetSteamPath()) + "\\steamapps\\common\\Counter-Strike Global Offensive\\game\\csgo\\maps");

            foreach (FileInfo file in directory.GetFiles())
            {
                if (!file.Name.ToLower().Contains("_vanity")) continue;
                file.Delete();
            }
            foreach (var account in Accounts)
            {
                string outputFolder = Path.GetDirectoryName(GlobalMenager.GetSteamPath()) + "/userdata/" + (account.AccountInfo.MaFile.Session.SteamID - 76561197960265728).ToString();

                byte[] resourceBytes = CS2Settings.Settings;

                ExtractZipArchive(resourceBytes, outputFolder);
            }
        }
        private void ExtractZipArchive(byte[] zipBytes, string extractPath)
        {
            using (var stream = new MemoryStream(zipBytes))
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                foreach (var entry in archive.Entries)
                {
                    string entryPath = Path.Combine(extractPath, entry.FullName);
                    if (entry.FullName.EndsWith("/"))
                    {
                        // Если это директория, создаем соответствующую папку
                        Directory.CreateDirectory(entryPath);
                    }
                    else
                    {
                        // Если это файл, распаковываем его
                        entry.ExtractToFile(entryPath, true);
                    }
                }
            }
        }
    }
}
