using JCorePanelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SteamAuth;
using System.Windows;

namespace CS2Actions
{
    public class JCActions : JCAccountActionBase
    {
        public void LoadActions(JCSteamAccount account)
        {
            if (account == null) return;
            if (account.MaFile == null) return;
            AddAction(new JCAction("ShowCS2Profile", "Get CS2 Level", ShowCS2Profile, "CS2"));

        }
        Task OpenVertigoContolWindowAsync(CS2Profile cS2Profile)
        {
            var tcs = new TaskCompletionSource<object>();

            Application.Current.Dispatcher.Invoke(() =>
            {
                var vsYourFriendContol = new OneAccountProfile(cS2Profile);


                vsYourFriendContol.Closed += (sender, e) =>
                {
                    tcs.SetResult(null);
                };
                vsYourFriendContol.Show();
            });

            return tcs.Task;
        }
        private async Task ShowCS2Profile(JCSteamAccountInstance Account)
        {
            Account.SetInWork(true);

            Account.SetWorkStatus("Logging");
            await Account.AccountInfo.CheckSession();
            Thread thread = new Thread(async () =>
            {
                Task task = OpenVertigoContolWindowAsync(await GetCS2Profile(Account));
            });

            thread.SetApartmentState(ApartmentState.STA); // Устанавливаем режим однопоточной апартаментной модели
            thread.Start();
        }

        private async Task<CS2Profile> GetCS2Profile(JCSteamAccountInstance Account)
        {

            CS2Profile Profile = new CS2Profile();
            Account.SetWorkStatus("Getting Info");
            Profile.Login = Account.AccountInfo.Login;
            string text = await SteamWeb.GETRequest("https://steamcommunity.com/profiles/" + Account.AccountInfo.MaFile.Session.SteamID + "/" + "gcpd/730/", Account.AccountInfo.MaFile.Session.GetCookies());
            string nickname = @"<div\s+class=""generic_kv_line"">([^"">]+)</div>";
            Match UserAvatar = Regex.Match(text, nickname).NextMatch().NextMatch();
            string Regex_CSGOLevel = @"<div\s+class=""generic_kv_line"">\s+CS:GO\s+Profile\s+Rank:\s+(\d{1,2})\s+</div>";
            Profile.CS2_Level = Int32.Parse(Regex.Match(UserAvatar.Value, Regex_CSGOLevel).Groups[1].Value);
            UserAvatar = Regex.Match(text, nickname).NextMatch().NextMatch().NextMatch();
            Profile.CS2_CurrectXP = Int32.Parse(Regex.Match(UserAvatar.Value, @"<div\s+class=""generic_kv_line"">\s+Experience\ points\ earned\ towards\ next\ rank:\s+(\d+)\s+</div>").Groups[1].Value);

            Account.SetInWork(false);
            return Profile;
        }


    }
}
