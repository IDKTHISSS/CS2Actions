using JCorePanelBase.Structures;
using JCorePanelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace CS2Actions
{
    public class RunCS2Event : JCEventBase
    {
        public string Name = "RunCS2";

        public RunCS2Event(List<JCEventProperty> PropertiesList) : base(PropertiesList)
        {
            AddProperty(new JCEventProperty("LaunchOptions", "-language english -w 360 -h 270 -swapcores -noqueuedload -vrdisable -windowed -nopreload -limitvsconst -softparticlesdefaultoff -nohltv -noaafonts -nosound -novid"));
            AddProperty(new JCEventProperty("Config", "violence_hblood 0; sethdmodels 0; mat_disable_fancy_blending 1; r_dynamic 0; engine_no_focus_sleep 100;"));
            AddProperty(new JCEventProperty("TimeBetweenLaunch", "20"));
        }

        public void EventBody(List<JCSteamAccountInstance> Accounts)
        {
            File.WriteAllText(Path.GetDirectoryName(GlobalMenager.GetSteamPath()) + "\\steamapps\\common\\Counter-Strike Global Offensive\\game\\csgo\\cfg\\JCoreConfig.cfg", GetPropertie("LaunchOptions"));
            foreach (var account in Accounts)
            {
                SetStatus($"Starting {account.AccountInfo.Login}");
                _ = Steam.RunSteamWithParams(account, "-nofriendsui -noverifyfiles -nobootstrapupdate -skipinitialbootstrap -norepairfiles -overridepackageurl -disable-winh264 -silent -language english -applaunch 730 " + GetPropertie("LaunchOptions") + " +exec JCoreConfig");
                Thread.Sleep(Int32.Parse(GetPropertie("TimeBetweenLaunch")) * 1000);
            }
        }
    }
}
