using AntiPlagiatus.Models.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiPlagiatus.Providers
{
    public interface IHistoryProvider
    {
        void SetUserToken(string userToken);
        Task Refresh();
        ObservableCollection<HistoryItem> GetHistory();
        Task AddItem(HistoryItem item);
        Task RemoveItem(HistoryItem item);
        Task Clear();
        HistoryItem GetItemByContentAndIgnores(string content, List<IgnoreRule> ignoreRules);
        Task Initialize();
    }
}
