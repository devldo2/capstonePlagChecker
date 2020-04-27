using AntiPlagiatus.Models;
using AntiPlagiatus.Models.UI;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AntiPlagiatus.Providers
{
    public static class WebPlagiatusApiProvider
    {
        private const string TOKEN = "TOKEN";
        private const string URL = "https://api.advego.com/json/antiplagiat";

        private static readonly HttpClient httpClient = new HttpClient();
        public static async Task<CheckResponse> CheckText(string content, string operationId, List<IgnoreRule> rules)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{URL}/add/"))
            {
                var parameters = new RequestObj()
                {
                    jsonrpc = "2.0",
                    method = "unique_text_add",
                    @params = new Param() { token = TOKEN, text = content?.ToLower() },
                    id = operationId
                };

                if (rules?.Count > 0)
                {
                    var ignores = new List<string>();
                    foreach (var item in rules)
                    {
                        switch (item.Type)
                        {
                            case IgnoreType.URL:
                                ignores.Add($"u:{item.Url}");
                                break;
                            case IgnoreType.Domain:
                                ignores.Add($"b:{item.Url}");
                                break;
                            default:
                                break;
                        }
                    }
                    parameters.@params.options = new Option() { ignore_rules = ignores };
                }

                var json = DataContractSerializer.SerializeObject(parameters);
                using (var stringContent = new StringContent(json))
                {
                    request.Content = stringContent;
                    try
                    {
                        using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None))
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                //successful response from api
                                //response can contain error code and message in the result object
                                string resultText = System.Text.Encoding.UTF8.GetString(await response.Content.ReadAsByteArrayAsync());
                                return DataContractSerializer.DeserializeObject<CheckResponse>(resultText);
                            }
                            else
                            {
                                //exception during the response
                                return new CheckResponse()
                                {
                                    result = new CheckResponseResult()
                                    {
                                        error = (int)response.StatusCode,
                                        error_msg = response.ReasonPhrase
                                    }
                                };
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        //exception not with api but with request
                        throw ex;
                    }
                }
            }
        }
        public static async Task<ReportResponse> GetReport(string key, string operationId)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{URL}/get/"))
            {
                var parameters = new RequestObj()
                {
                    id = operationId,
                    jsonrpc = "2.0",
                    method = "unique_check",
                    @params = new Param() { token = TOKEN, report_json = "1", key = key }
                };

                var json = DataContractSerializer.SerializeObject(parameters);
                using (var stringContent = new StringContent(json))
                {
                    request.Content = stringContent;
                    try
                    {
                        //var resultText = "{\"result\":{\"status\":\"done\",\"is_fixed\":null,\"is_public\":null,\"report\":{\"len\":214,\"rewrite_per_bin\":[100],\"equal_words\":[1,4,5,7,9,10,11,13,14,16,17,19,21,22,24,25,26,27,28,31,33,34,35,37],\"bad_words\":[],\"urls_stats\":null,\"layers\":[{\"rewrite\":100,\"equality\":100,\"words\":[1,4,5,7,9,10,11,13,14,16,17,19,21,22,24,25,26,27,28,31,33,34,35,37],\"uri\":\"https://www.roxyappsdev.com/about\",\"shingles\":[0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37]},{\"equality\":95,\"words\":[1,4,5,7,9,10,11,13,14,16,17,19,21,22,24,25,26,27,28,31,33,34,35,37],\"uri\":\"https://hi-in.facebook.com/RoxyAppsDev/groups/?ref=page_internal\",\"rewrite\":100,\"shingles\":[0,1,2,3,4,5,6,7,8,9,10,11,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37]},{\"shingles\":[0,1,2,3,4,5,6,7,8,9,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37],\"rewrite\":100,\"equality\":84,\"uri\":\"https://www.facebook.com/RoxyAppsDev/about/\",\"words\":[1,4,5,7,9,10,11,13,14,16,17,19,21,22,24,25,26,27,28,31,33,34,35,37]},{\"shingles\":[31,32,33,34,35,36,37],\"equality\":18,\"words\":[1,4,9,25,28,31,33,34,35,37],\"uri\":\"https://www.xodo.com/about_us.html\",\"rewrite\":42},{\"shingles\":[31,32,33,34,35,36,37],\"rewrite\":29,\"equality\":18,\"words\":[1,28,31,33,34,35,37],\"uri\":\"https://www.bctechbase.com/company/2712.html\"},{\"rewrite\":25,\"equality\":18,\"uri\":\"https://webcache.googleusercontent.com/search?q=cache:https://www.zoominfo.com/c/xodo-technologies-inc/369040034\",\"words\":[1,31,33,34,35,37],\"shingles\":[31,32,33,34,35,36,37]},{\"uri\":\"https://sharaonweb.weebly.com/blog/category/all/2\",\"words\":[4,7,9,10,14,16,25,27,28,31,33,34,35,37],\"equality\":0,\"rewrite\":58,\"shingles\":[]},{\"shingles\":[],\"equality\":0,\"uri\":\"https://www.pdfpro.co/edit-pdf\",\"words\":[5,7,21,22,25,27,28,31,33,34],\"rewrite\":42},{\"uri\":\"https://www.dailymail.co.uk/sciencetech/article-8130447/Snapchat-releases-mental-health-support-tool-early-coronavirus-crisis.html\",\"words\":[1,4,5,7,9,26,27,28,31,34],\"equality\":0,\"rewrite\":42,\"shingles\":[]},{\"rewrite\":38,\"equality\":0,\"uri\":\"https://AppAgg.com/windows/utilitiesandtools/pdf-assistant-pro-31346167.html?hl=ru\",\"words\":[4,5,25,26,27,28,31,33,35],\"shingles\":[]},{\"words\":[4,5,14,17,25,27,28,31,33],\"uri\":\"https://appadvice.com/app/pdf-expert-pro-create-read-annotate-edit-pdfs/1211027742\",\"equality\":0,\"rewrite\":38,\"shingles\":[]},{\"shingles\":[],\"uri\":\"https://en.freedownloadmanager.org/users-choice/Sign_Pro_Pdf.html\",\"words\":[4,5,9,25,27,28,33,34],\"equality\":0,\"rewrite\":33},{\"rewrite\":33,\"words\":[4,19,21,22,24,26,28,31],\"uri\":\"https://context.reverso.net/%D0%BF%D0%B5%D1%80%D0%B5%D0%B2%D0%BE%D0%B4/%D0%B0%D0%BD%D0%B3%D0%BB%D0%B8%D0%B9%D1%81%D0%BA%D0%B8%D0%B9-%D1%80%D1%83%D1%81%D1%81%D0%BA%D0%B8%D0%B9/wide+range+of+tasks\",\"equality\":0,\"shingles\":[]},{\"equality\":0,\"uri\":\"https://censor.net.ua/en/news/3079621/domestic_helicopters_can_help_solve_wide_range_of_military_and_civilian_tasks_nsdc_chief_says_photos\",\"words\":[1,17,19,21,22,24,31,34],\"rewrite\":33,\"shingles\":[]},{\"shingles\":[],\"rewrite\":29,\"equality\":0,\"words\":[4,5,25,26,27,28,31],\"uri\":\"https://ru.freedownloadmanager.org/Windows-PC/PDF-Assistant-PRO-FREE.html\"},{\"shingles\":[],\"rewrite\":29,\"equality\":0,\"uri\":\"https://rutracker.org/forum/viewtopic.php?t=4330303\",\"words\":[5,25,27,28,31,33,37]},{\"shingles\":[],\"equality\":0,\"uri\":\"https://www.sites.google.com/site/organicspectato/does-it-work-pdf-to-doc-team-license-free-version\",\"words\":[5,25,26,27,28,31,34],\"rewrite\":29},{\"shingles\":[],\"rewrite\":29,\"uri\":\"https://www.filehorse.com/download-novapdf-pro/\",\"words\":[5,10,11,25,27,28,35],\"equality\":0},{\"shingles\":[],\"rewrite\":29,\"equality\":0,\"words\":[19,21,22,24,26,28,31],\"uri\":\"https://context.reverso.net/translation/english-russian/a+wide+range+of+tasks\"},{\"shingles\":[],\"uri\":\"https://helpx.adobe.com/premiere-pro/how-to/proxy-media.html\",\"words\":[5,27,28,31,34,35],\"equality\":0,\"rewrite\":25},{\"rewrite\":25,\"equality\":0,\"words\":[7,27,28,31,34,35],\"uri\":\"https://blog.pond5.com/4723-5-time-saving-tips-for-organizing-your-premiere-pro-projects/\",\"shingles\":[]},{\"shingles\":[],\"rewrite\":25,\"equality\":0,\"uri\":\"https://macpaw.com/how-to/adobe-premiere-slow\",\"words\":[5,27,28,31,34,35]},{\"rewrite\":25,\"equality\":0,\"uri\":\"https://www.convertpro.net/docs/working-referrer-detection-convert-pro/\",\"words\":[4,5,27,28,31,34],\"shingles\":[]},{\"rewrite\":25,\"equality\":0,\"uri\":\"http://www.software4pc.ru/download/file-all-converter-pro-89649\",\"words\":[5,25,27,28,31,35],\"shingles\":[]},{\"shingles\":[],\"equality\":0,\"words\":[21,22,24,28,31,34],\"uri\":\"https://appsource.microsoft.com/ru-ru/product/office/WA200000710?src=office&tab=Overview\",\"rewrite\":25},{\"shingles\":[],\"uri\":\"https://www.presse-blog.com/2019/08/28/the-suitable-solution-for-every-testing-task-wide-range-of-leak-and-flow-testers-for-various-applications/\",\"words\":[9,11,16,21,22,24],\"equality\":0,\"rewrite\":25},{\"equality\":0,\"words\":[4,5,7,13,28,31],\"uri\":\"https://stackoverflow.com/questions/10436890/recommended-way-to-provide-users-with-a-terms-and-conditions-dialog/10890531\",\"rewrite\":25,\"shingles\":[]},{\"shingles\":[],\"rewrite\":25,\"equality\":0,\"words\":[4,5,7,28,31,37],\"uri\":\"https://blog.travelpayouts.com/en/25-best-affiliate-programs/\"},{\"shingles\":[],\"rewrite\":21,\"equality\":0,\"uri\":\"https://www.virtualassistantassistant.com/tasks-assistant\",\"words\":[4,5,24,26,31]},{\"shingles\":[],\"rewrite\":21,\"words\":[4,7,27,28,31],\"uri\":\"https://www.facebook.com/israelprogramcenter/posts/1476939119128697\",\"equality\":0},{\"shingles\":[],\"equality\":0,\"uri\":\"https://www.linguee.com/english-russian/translation/provide+users+with+information.html\",\"words\":[1,4,5,9,31],\"rewrite\":21},{\"shingles\":[],\"rewrite\":21,\"uri\":\"https://www.linguee.ru/%D0%B0%D0%BD%D0%B3%D0%BB%D0%B8%D0%B9%D1%81%D0%BA%D0%B8%D0%B9-%D1%80%D1%83%D1%81%D1%81%D0%BA%D0%B8%D0%B9/%D0%BF%D0%B5%D1%80%D0%B5%D0%B2%D0%BE%D0%B4/provide+users+with+the+ability.html\",\"words\":[4,5,7,26,31],\"equality\":0},{\"shingles\":[],\"equality\":0,\"words\":[4,5,7,9,28],\"uri\":\"https://medium.com/@pressrelease_94329/song365-us-aiming-to-provide-users-with-great-mp3-song-downloads-cd98d01acfeb\",\"rewrite\":21}],\"domains_cnt\":31,\"layers_cnt\":33,\"captchas\":0,\"equal_shingle_words\":[0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37],\"word_bins\":[0,38],\"equal_shingles\":[1,4,5,7,9,10,11,13,14,16,17,19,21,22,24,25,26,27,28,31],\"error_pages\":11,\"id\":\"12479505\",\"found_pages\":33,\"checked_phrases\":5,\"error_phrases\":0,\"lang\":\"russian\",\"word_count\":38,\"equality_per_bin\":[100],\"sym_bins\":[0,214],\"progress\":97,\"rewrite\":100,\"text_fragments\":[\"\",\"Our\",\" \",\"mission\",\" \",\"is\",\" \",\"to\",\" \",\"provide\",\" \",\"users\",\" \",\"with\",\" \",\"great\",\" \",\"and\",\" \",\"reliable\",\" \",\"Windows\",\" \",\"10\",\" \",\"and\",\" \",\"Android\",\" \",\"apps\",\", \",\"that\",\" \",\"would\",\" \",\"help\",\" \",\"to\",\" \",\"solve\",\" \",\"a\",\" \",\"wide\",\" \",\"range\",\" \",\"of\",\" \",\"tasks\",\". \",\"PDF\",\" \",\"Assistant\",\" \",\"PRO\",\" \",\"allows\",\" \",\"you\",\" \",\"to\",\" \",\"work\",\" \",\"with\",\" \",\"PDFs\",\" \",\"quickly\",\", \",\"easily\",\", \",\"and\",\" \",\"conveniently\",\".\"],\"equality\":100,\"checked_pages\":57,\"layers_by_domain\":[{\"rewrite\":100,\"domain\":\"www.roxyappsdev.com\",\"layers\":[{\"equality\":100,\"uri\":\"https://www.roxyappsdev.com/about\",\"words\":[1,4,5,7,9,10,11,13,14,16,17,19,21,22,24,25,26,27,28,31,33,34,35,37],\"rewrite\":100,\"shingles\":[0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37]}],\"equality\":100},{\"layers\":[{\"shingles\":[0,1,2,3,4,5,6,7,8,9,10,11,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37],\"equality\":95,\"words\":[1,4,5,7,9,10,11,13,14,16,17,19,21,22,24,25,26,27,28,31,33,34,35,37],\"uri\":\"https://hi-in.facebook.com/RoxyAppsDev/groups/?ref=page_internal\",\"rewrite\":100}],\"equality\":95,\"rewrite\":100,\"domain\":\"hi-in.facebook.com\"},{\"domain\":\"www.facebook.com\",\"rewrite\":100,\"equality\":84,\"layers\":[{\"rewrite\":100,\"words\":[1,4,5,7,9,10,11,13,14,16,17,19,21,22,24,25,26,27,28,31,33,34,35,37],\"uri\":\"https://www.facebook.com/RoxyAppsDev/about/\",\"equality\":84,\"shingles\":[0,1,2,3,4,5,6,7,8,9,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37]},{\"shingles\":[],\"words\":[4,7,27,28,31],\"uri\":\"https://www.facebook.com/israelprogramcenter/posts/1476939119128697\",\"equality\":0,\"rewrite\":21}]},{\"layers\":[{\"uri\":\"https://www.xodo.com/about_us.html\",\"words\":[1,4,9,25,28,31,33,34,35,37],\"equality\":18,\"rewrite\":42,\"shingles\":[31,32,33,34,35,36,37]}],\"equality\":18,\"rewrite\":42,\"domain\":\"www.xodo.com\"},{\"layers\":[{\"rewrite\":29,\"equality\":18,\"words\":[1,28,31,33,34,35,37],\"uri\":\"https://www.bctechbase.com/company/2712.html\",\"shingles\":[31,32,33,34,35,36,37]}],\"equality\":18,\"rewrite\":29,\"domain\":\"www.bctechbase.com\"},{\"domain\":\"webcache.googleusercontent.com\",\"rewrite\":25,\"equality\":18,\"layers\":[{\"words\":[1,31,33,34,35,37],\"uri\":\"https://webcache.googleusercontent.com/search?q=cache:https://www.zoominfo.com/c/xodo-technologies-inc/369040034\",\"equality\":18,\"rewrite\":25,\"shingles\":[31,32,33,34,35,36,37]}]},{\"equality\":0,\"layers\":[{\"shingles\":[],\"rewrite\":58,\"uri\":\"https://sharaonweb.weebly.com/blog/category/all/2\",\"words\":[4,7,9,10,14,16,25,27,28,31,33,34,35,37],\"equality\":0}],\"domain\":\"sharaonweb.weebly.com\",\"rewrite\":58},{\"layers\":[{\"shingles\":[],\"uri\":\"https://www.pdfpro.co/edit-pdf\",\"words\":[5,7,21,22,25,27,28,31,33,34],\"equality\":0,\"rewrite\":42}],\"equality\":0,\"rewrite\":42,\"domain\":\"www.pdfpro.co\"},{\"layers\":[{\"shingles\":[],\"rewrite\":42,\"equality\":0,\"uri\":\"https://www.dailymail.co.uk/sciencetech/article-8130447/Snapchat-releases-mental-health-support-tool-early-coronavirus-crisis.html\",\"words\":[1,4,5,7,9,26,27,28,31,34]}],\"equality\":0,\"rewrite\":42,\"domain\":\"www.dailymail.co.uk\"},{\"layers\":[{\"shingles\":[],\"uri\":\"https://AppAgg.com/windows/utilitiesandtools/pdf-assistant-pro-31346167.html?hl=ru\",\"words\":[4,5,25,26,27,28,31,33,35],\"equality\":0,\"rewrite\":38}],\"equality\":0,\"rewrite\":38,\"domain\":\"AppAgg.com\"},{\"domain\":\"appadvice.com\",\"rewrite\":38,\"equality\":0,\"layers\":[{\"shingles\":[],\"uri\":\"https://appadvice.com/app/pdf-expert-pro-create-read-annotate-edit-pdfs/1211027742\",\"words\":[4,5,14,17,25,27,28,31,33],\"equality\":0,\"rewrite\":38}]},{\"rewrite\":33,\"domain\":\"en.freedownloadmanager.org\",\"layers\":[{\"equality\":0,\"uri\":\"https://en.freedownloadmanager.org/users-choice/Sign_Pro_Pdf.html\",\"words\":[4,5,9,25,27,28,33,34],\"rewrite\":33,\"shingles\":[]}],\"equality\":0},{\"rewrite\":33,\"domain\":\"context.reverso.net\",\"layers\":[{\"equality\":0,\"uri\":\"https://context.reverso.net/%D0%BF%D0%B5%D1%80%D0%B5%D0%B2%D0%BE%D0%B4/%D0%B0%D0%BD%D0%B3%D0%BB%D0%B8%D0%B9%D1%81%D0%BA%D0%B8%D0%B9-%D1%80%D1%83%D1%81%D1%81%D0%BA%D0%B8%D0%B9/wide+range+of+tasks\",\"words\":[4,19,21,22,24,26,28,31],\"rewrite\":33,\"shingles\":[]},{\"equality\":0,\"words\":[19,21,22,24,26,28,31],\"uri\":\"https://context.reverso.net/translation/english-russian/a+wide+range+of+tasks\",\"rewrite\":29,\"shingles\":[]}],\"equality\":0},{\"equality\":0,\"layers\":[{\"rewrite\":33,\"equality\":0,\"uri\":\"https://censor.net.ua/en/news/3079621/domestic_helicopters_can_help_solve_wide_range_of_military_and_civilian_tasks_nsdc_chief_says_photos\",\"words\":[1,17,19,21,22,24,31,34],\"shingles\":[]}],\"domain\":\"censor.net.ua\",\"rewrite\":33},{\"equality\":0,\"layers\":[{\"equality\":0,\"words\":[4,5,25,26,27,28,31],\"uri\":\"https://ru.freedownloadmanager.org/Windows-PC/PDF-Assistant-PRO-FREE.html\",\"rewrite\":29,\"shingles\":[]}],\"domain\":\"ru.freedownloadmanager.org\",\"rewrite\":29},{\"rewrite\":29,\"domain\":\"rutracker.org\",\"layers\":[{\"shingles\":[],\"equality\":0,\"uri\":\"https://rutracker.org/forum/viewtopic.php?t=4330303\",\"words\":[5,25,27,28,31,33,37],\"rewrite\":29}],\"equality\":0},{\"equality\":0,\"layers\":[{\"rewrite\":29,\"words\":[5,25,26,27,28,31,34],\"uri\":\"https://www.sites.google.com/site/organicspectato/does-it-work-pdf-to-doc-team-license-free-version\",\"equality\":0,\"shingles\":[]}],\"domain\":\"www.sites.google.com\",\"rewrite\":29},{\"domain\":\"www.filehorse.com\",\"rewrite\":29,\"equality\":0,\"layers\":[{\"shingles\":[],\"rewrite\":29,\"equality\":0,\"words\":[5,10,11,25,27,28,35],\"uri\":\"https://www.filehorse.com/download-novapdf-pro/\"}]},{\"layers\":[{\"equality\":0,\"uri\":\"https://helpx.adobe.com/premiere-pro/how-to/proxy-media.html\",\"words\":[5,27,28,31,34,35],\"rewrite\":25,\"shingles\":[]}],\"equality\":0,\"rewrite\":25,\"domain\":\"helpx.adobe.com\"},{\"rewrite\":25,\"domain\":\"blog.pond5.com\",\"layers\":[{\"shingles\":[],\"equality\":0,\"uri\":\"https://blog.pond5.com/4723-5-time-saving-tips-for-organizing-your-premiere-pro-projects/\",\"words\":[7,27,28,31,34,35],\"rewrite\":25}],\"equality\":0},{\"layers\":[{\"shingles\":[],\"equality\":0,\"uri\":\"https://macpaw.com/how-to/adobe-premiere-slow\",\"words\":[5,27,28,31,34,35],\"rewrite\":25}],\"equality\":0,\"rewrite\":25,\"domain\":\"macpaw.com\"},{\"equality\":0,\"layers\":[{\"rewrite\":25,\"uri\":\"https://www.convertpro.net/docs/working-referrer-detection-convert-pro/\",\"words\":[4,5,27,28,31,34],\"equality\":0,\"shingles\":[]}],\"domain\":\"www.convertpro.net\",\"rewrite\":25},{\"equality\":0,\"layers\":[{\"shingles\":[],\"words\":[5,25,27,28,31,35],\"uri\":\"http://www.software4pc.ru/download/file-all-converter-pro-89649\",\"equality\":0,\"rewrite\":25}],\"domain\":\"www.software4pc.ru\",\"rewrite\":25},{\"equality\":0,\"layers\":[{\"shingles\":[],\"equality\":0,\"uri\":\"https://appsource.microsoft.com/ru-ru/product/office/WA200000710?src=office&tab=Overview\",\"words\":[21,22,24,28,31,34],\"rewrite\":25}],\"domain\":\"appsource.microsoft.com\",\"rewrite\":25},{\"layers\":[{\"shingles\":[],\"rewrite\":25,\"equality\":0,\"words\":[9,11,16,21,22,24],\"uri\":\"https://www.presse-blog.com/2019/08/28/the-suitable-solution-for-every-testing-task-wide-range-of-leak-and-flow-testers-for-various-applications/\"}],\"equality\":0,\"rewrite\":25,\"domain\":\"www.presse-blog.com\"},{\"layers\":[{\"shingles\":[],\"rewrite\":25,\"uri\":\"https://stackoverflow.com/questions/10436890/recommended-way-to-provide-users-with-a-terms-and-conditions-dialog/10890531\",\"words\":[4,5,7,13,28,31],\"equality\":0}],\"equality\":0,\"rewrite\":25,\"domain\":\"stackoverflow.com\"},{\"equality\":0,\"layers\":[{\"shingles\":[],\"rewrite\":25,\"words\":[4,5,7,28,31,37],\"uri\":\"https://blog.travelpayouts.com/en/25-best-affiliate-programs/\",\"equality\":0}],\"domain\":\"blog.travelpayouts.com\",\"rewrite\":25},{\"equality\":0,\"layers\":[{\"equality\":0,\"uri\":\"https://www.virtualassistantassistant.com/tasks-assistant\",\"words\":[4,5,24,26,31],\"rewrite\":21,\"shingles\":[]}],\"domain\":\"www.virtualassistantassistant.com\",\"rewrite\":21},{\"rewrite\":21,\"domain\":\"www.linguee.com\",\"layers\":[{\"rewrite\":21,\"equality\":0,\"words\":[1,4,5,9,31],\"uri\":\"https://www.linguee.com/english-russian/translation/provide+users+with+information.html\",\"shingles\":[]}],\"equality\":0},{\"equality\":0,\"layers\":[{\"shingles\":[],\"rewrite\":21,\"equality\":0,\"words\":[4,5,7,26,31],\"uri\":\"https://www.linguee.ru/%D0%B0%D0%BD%D0%B3%D0%BB%D0%B8%D0%B9%D1%81%D0%BA%D0%B8%D0%B9-%D1%80%D1%83%D1%81%D1%81%D0%BA%D0%B8%D0%B9/%D0%BF%D0%B5%D1%80%D0%B5%D0%B2%D0%BE%D0%B4/provide+users+with+the+ability.html\"}],\"domain\":\"www.linguee.ru\",\"rewrite\":21},{\"domain\":\"medium.com\",\"rewrite\":21,\"equality\":0,\"layers\":[{\"words\":[4,5,7,9,28],\"uri\":\"https://medium.com/@pressrelease_94329/song365-us-aiming-to-provide-users-with-great-mp3-song-downloads-cd98d01acfeb\",\"equality\":0,\"rewrite\":21,\"shingles\":[]}]}]},\"hash_url\":\"qdMA387U\"},\"jsonrpc\":\"2.0\",\"id\":\"1\"}";
                        using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None).ConfigureAwait(false))
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                //successful response from api
                                //response can contain error code and message in the result object
                                string resultText = System.Text.Encoding.UTF8.GetString(await response.Content.ReadAsByteArrayAsync());
                                return DataContractSerializer.DeserializeObject<ReportResponse>(resultText);
                            }
                            else
                            {
                                //exception during the response
                                return new ReportResponse()
                                {
                                    result = new ReportResponseResult()
                                    {
                                        error = (int)response.StatusCode,
                                        error_msg = response.ReasonPhrase
                                    }
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //exception not with api but with request
                        throw ex;
                    }
                }
            }
        }
    }
}
