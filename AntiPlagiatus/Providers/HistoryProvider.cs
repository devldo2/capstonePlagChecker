using AntiPlagiatus.Helpers.Extensions;
using AntiPlagiatus.Models.API;
using AntiPlagiatus.Models.UI;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AntiPlagiatus.Providers
{
    public class HistoryProvider : IHistoryProvider
    {
        private readonly Mapper mapper;
        private bool isInitialized;
        private string userToken;
        private ObservableCollection<HistoryItem> items = new ObservableCollection<HistoryItem>();

        public HistoryProvider()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<APIReport, HistoryItem>().ForMember(item => item.Status, opt => opt.MapFrom(source => source.Status.ParseReportStatus()))
                                                           .ForMember(item => item.Date, opt => opt.MapFrom(source => DateTime.Parse(source.Date)));
                cfg.CreateMap<APIContent, Content>().ForMember(item => item.Origin, opt => opt.MapFrom(source => source.Origin.ParseContentTextOrigin()));
                cfg.CreateMap<APIIgnoreRule, IgnoreRule>().ForMember(item => item.Type, opt => opt.MapFrom(source => source.Type.ParseIgnoreRuleType()));
                cfg.CreateMap<APIDomain, Domain>();
                cfg.CreateMap<APILayerByDomain, LayerByDomain>().ForMember(item => item.Words, opt => opt.MapFrom(source => source.StringWords.Split(',', StringSplitOptions.None).Where(item => item.IsInt32()).Select(item => Int32.Parse(item)).ToList()));

                cfg.CreateMap<HistoryItem, APIReport>().ForMember(item => item.Status, opt => opt.MapFrom(source => source.Status.ToString()))
                                                           .ForMember(item => item.Date, opt => opt.MapFrom(source => source.Date.ToString()));
                cfg.CreateMap<Content, APIContent>().ForMember(item => item.Origin, opt => opt.MapFrom(source => source.Origin.ToString()));
                cfg.CreateMap<IgnoreRule, APIIgnoreRule>().ForMember(item => item.Type, opt => opt.MapFrom(source => source.Type.ToString()));
                cfg.CreateMap<Domain, APIDomain>();
                cfg.CreateMap<LayerByDomain, APILayerByDomain>().ForMember(item => item.StringWords, opt => opt.MapFrom(source => string.Join(",", source.Words.Select(value => value))));
            });
            mapper = new Mapper(config);
        }
        public void SetUserToken(string userToken)
        {
            this.userToken = userToken;
        }
        public async Task Refresh()
        {
            if (isInitialized && !string.IsNullOrEmpty(userToken))
            {
                var apiresult = await WebApiProvider.GetHistory(userToken);
                if (!string.IsNullOrEmpty(apiresult.ErrorMessage))
                    throw new Exception(apiresult.ErrorMessage);
                else
                {
                    var serverItems = apiresult.Content as List<APIReport>;
                    if (serverItems != null)
                    {
                        var mappedItems = mapper.Map<List<HistoryItem>>(serverItems);
                        if (mappedItems?.Count > 0)
                        {
                            if (items.Count > 0)
                            {
                                foreach (var report in mappedItems)
                                    if (!items.Contains(report)) this.items.Add(report);
                            }
                            else foreach (var item in mappedItems)
                                    this.items.Add(item);
                            this.items = new ObservableCollection<HistoryItem>(this.items.OrderByDescending(item => item.Date));
                        }
                    }
                }
            }
        }

        public ObservableCollection<HistoryItem> GetHistory()
        {
            return isInitialized ? items : null;
        }

        public async Task AddItem(HistoryItem item)
        {
            if (isInitialized && !string.IsNullOrEmpty(userToken)
                && this.GetItemByContentAndIgnores(item.Content.Text, item.IgnoreRules) == null)
            {
                items.Insert(0, item);
                var serverObj = mapper.Map<APIReport>(item);
                serverObj.UserToken = userToken;
                var apiresult = await WebApiProvider.SaveHistoryItem(serverObj);
                if (!string.IsNullOrEmpty(apiresult.ErrorMessage))
                    throw new Exception(apiresult.ErrorMessage);
            }
        }
        public async Task RemoveItem(HistoryItem item)
        {
            if (isInitialized && !string.IsNullOrEmpty(userToken))
            {
                items.Remove(item);
                var serverObj = mapper.Map<APIReport>(item);
                serverObj.UserToken = userToken;
                var apiresult = await WebApiProvider.RemoveHistoryItem(serverObj);
                if (!string.IsNullOrEmpty(apiresult.ErrorMessage))
                    throw new Exception(apiresult.ErrorMessage);
            }
        }
        public async Task Clear()
        {
            if (isInitialized && !string.IsNullOrEmpty(userToken) && items.Count > 0)
            {
                items.Clear();

                var apiresult = await WebApiProvider.ClearHistory(userToken);
                if (!string.IsNullOrEmpty(apiresult.ErrorMessage))
                    throw new Exception(apiresult.ErrorMessage);
            }
        }
        public HistoryItem GetItemByContentAndIgnores(string content, List<IgnoreRule> ignoreRules)
        {
            return isInitialized ? items.FirstOrDefault(item => item.Content.Text.ToLower() == content.ToLower() && item.IgnoreRules.Except(ignoreRules).ToList().Count == 0) : null;
        }
        public async Task Initialize()
        {
            if (!string.IsNullOrEmpty(userToken))
            {
                var apiresult = await WebApiProvider.GetHistory(userToken);
                if (string.IsNullOrEmpty(apiresult.ErrorMessage))
                {
                    var serverItems = apiresult.Content as List<APIReport>;
                    if (serverItems != null)
                    {
                        var mappedItems = mapper.Map<List<HistoryItem>>(serverItems);
                        if (mappedItems?.Count > 0)
                        {
                            if (items.Count > 0)
                            {
                                foreach (var report in mappedItems)
                                    if (!items.Contains(report)) this.items.Add(report);
                            }
                            else foreach (var item in mappedItems)
                                    this.items.Add(item);
                            this.items = new ObservableCollection<HistoryItem>(this.items.OrderByDescending(item => item.Date));
                        }
                    }
                }
            }
            isInitialized = true;
        }
    }
}
