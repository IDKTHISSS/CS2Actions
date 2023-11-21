using JCorePanelBase.Structures;
using JCorePanelBase;
using JCoreTradeFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSGOActions.Tasks
{

    public class SendItems : JCEventBase
    {
        public string Name = "SendAllItems";
        public string Description = "TestDesc";

        public SendItems(List<JCEventProperty> PropertiesList) : base(PropertiesList)
        {
            AddProperty(new JCEventProperty("TradeLink", "Here your trade link"));
            AddProperty(new JCEventProperty("TimeBetweenSend", "20"));
        }
        public new void EventBody(List<JCSteamAccountInstance> Accounts)
        {
            int Sended = 0;
            foreach (var account in Accounts)
            {
                Sended++;
                if (account.AccountInfo.MaFile == null)
                {
                    SetError($"MaFile not found {account.AccountInfo.Login}");
                    continue;
                }
                SetStatus($"Getting {account.AccountInfo.Login}");
                List<CSGOItem> Inventory = Trade.GetCSGOInventoryAsync(account.AccountInfo).GetAwaiter().GetResult();
                if (Inventory == null) continue;
                Inventory.RemoveAll(item => !item.IsTradeble);
                SetStatus($"Found {Inventory.Count} items.");
                if (Inventory.Count == 0)
                {
                    SetStatus($"No items {Sended}/{Accounts.Count}");
                    Thread.Sleep(Int32.Parse(GetPropertie("TimeBetweenSend")) * 1000);
                    continue;
                }
                SetStatus($"Creating offer...");
                _ = Trade.SendTrade(account, GetPropertie("TradeLink"), Inventory).GetAwaiter().GetResult();
                SetStatus($"Sended {Sended}/{Accounts.Count}");
                Thread.Sleep(Int32.Parse(GetPropertie("TimeBetweenSend")) * 1000);
            }
        }
    }

}
