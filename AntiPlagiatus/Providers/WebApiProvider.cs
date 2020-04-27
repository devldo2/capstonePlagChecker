using AntiPlagiatus.Models;
using AntiPlagiatus.Models.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace AntiPlagiatus.Providers
{
    public static class WebApiProvider
    {
        private const string URL = "http://localhost:5000/api";
        private static readonly HttpClient httpClient = new HttpClient();
        public static async Task<APIResult> RegisterUser(string email, string password)
        {
            APIResult result = new APIResult();
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{URL}/User/Register/"))
            {
                var user = new APIUser() { Login = email, Password = password, DefaultTheme = Theme.Light.ToString() };
                var json = DataContractSerializer.SerializeObject(user);
                using (var stringContent = new StringContent(json))
                {
                    request.Content = stringContent;
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None).ConfigureAwait(false))
                        {
                            var responseContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                            StreamReader reader = new StreamReader(responseContent);

                            if (response.StatusCode == HttpStatusCode.OK)
                                result.Content = DataContractSerializer.DeserializeObject<APIUser>(reader.ReadToEnd());
                            else
                                result.ErrorMessage = DataContractSerializer.DeserializeObject<string>(reader.ReadToEnd());
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = $"The Server is Unavailable. Please, contact with the server administrator. Error occured: {ex.Message}";
                    }
                }
            }
            return result;
        }
        public static async Task<APIResult> UpdateTheme(string userToken, string defaultTheme)
        {
            APIResult result = new APIResult();
            using (var request = new HttpRequestMessage(HttpMethod.Put, $"{URL}/User/UpdateTheme/"))
            {
                var user = new APIUser() { DefaultTheme = defaultTheme, Token = userToken };
                var json = DataContractSerializer.SerializeObject(user);
                using (var stringContent = new StringContent(json))
                {
                    request.Content = stringContent;
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None).ConfigureAwait(false))
                        {
                            var responseContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                            StreamReader reader = new StreamReader(responseContent);
                            if (response.StatusCode == HttpStatusCode.OK)
                                result.Content = DataContractSerializer.DeserializeObject<APIUser>(reader.ReadToEnd());
                            else
                                result.ErrorMessage = DataContractSerializer.DeserializeObject<string>(reader.ReadToEnd());

                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = $"The Server is Unavailable. Please, contact with the server administrator. Error occured: {ex.Message}";
                    }
                }
            }
            return result;
        }
        public static async Task<APIResult> LoginUser(string email, string password)
        {
            APIResult result = new APIResult();
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{URL}/User/Login/"))
            {
                var user = new APIUser() { Login = email, Password = password };
                var json = DataContractSerializer.SerializeObject(user);
                using (var stringContent = new StringContent(json))
                {
                    request.Content = stringContent;
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None).ConfigureAwait(false))
                        {
                            var responseContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                            StreamReader reader = new StreamReader(responseContent);
                            if (response.StatusCode == HttpStatusCode.OK)
                                result.Content = DataContractSerializer.DeserializeObject<APIUser>(reader.ReadToEnd());
                            else
                                result.ErrorMessage = DataContractSerializer.DeserializeObject<string>(reader.ReadToEnd());
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = $"The Server is Unavailable. Please, contact with the server administrator. Error occured: {ex.Message}";
                    }
                }
            }
            return result;
        }
        public static async Task<APIResult> IsUserExist(string userToken)
        {
            APIResult result = new APIResult();
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{URL}/User/CheckUser/"))
            {
                var user = new APIUser() { Token = userToken };
                var json = DataContractSerializer.SerializeObject(user);
                using (var stringContent = new StringContent(json))
                {
                    request.Content = stringContent;
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None).ConfigureAwait(false))
                        {
                            var responseContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                            StreamReader reader = new StreamReader(responseContent);
                            if (response.StatusCode == HttpStatusCode.OK)
                                result.Content = DataContractSerializer.DeserializeObject<APIUser>(reader.ReadToEnd());
                            else
                                result.ErrorMessage = DataContractSerializer.DeserializeObject<string>(reader.ReadToEnd());
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = $"The Server is Unavailable. Please, contact with the server administrator. Error occured: {ex.Message}";
                    }
                }
            }
            return result;
        }
        public static async Task<APIResult> GetHistory(string userToken)
        {
            APIResult result = new APIResult();
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{URL}/History/GetAllHistory/"))
            {
                var user = new APIUser() { Token = userToken };
                var json = DataContractSerializer.SerializeObject(user);
                using (var stringContent = new StringContent(json))
                {
                    request.Content = stringContent;
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None).ConfigureAwait(false))
                        {
                            var responseContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                            StreamReader reader = new StreamReader(responseContent);
                            if (response.StatusCode == HttpStatusCode.OK)
                                result.Content = DataContractSerializer.DeserializeObject<List<APIReport>>(reader.ReadToEnd());
                            else
                                result.ErrorMessage = DataContractSerializer.DeserializeObject<string>(reader.ReadToEnd());
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = $"The Server is Unavailable. Please, contact with the server administrator. Error occured: {ex.Message}";
                    }
                }
            }
            return result;
        }
        public static async Task<APIResult> SaveHistoryItem(APIReport report)
        {
            APIResult result = new APIResult();
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{URL}/History/SaveHistoryItem/"))
            {
                var json = DataContractSerializer.SerializeObject(report);
                using (var stringContent = new StringContent(json))
                {
                    request.Content = stringContent;
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None).ConfigureAwait(false))
                        {
                            if (response.StatusCode != HttpStatusCode.OK)
                            {
                                var responseContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                                StreamReader reader = new StreamReader(responseContent);
                                result.ErrorMessage = DataContractSerializer.DeserializeObject<string>(reader.ReadToEnd());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = $"The Server is Unavailable. Please, contact with the server administrator. Error occured: {ex.Message}";
                    }
                }
            }
            return result;
        }
        public static async Task<APIResult> RemoveHistoryItem(APIReport report)
        {
            APIResult result = new APIResult();
            using (var request = new HttpRequestMessage(HttpMethod.Delete, $"{URL}/History/RemoveHistoryItem/"))
            {
                var json = DataContractSerializer.SerializeObject(report);
                using (var stringContent = new StringContent(json))
                {
                    request.Content = stringContent;
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None).ConfigureAwait(false))
                        {
                            if (response.StatusCode != HttpStatusCode.OK)
                            {
                                var responseContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                                StreamReader reader = new StreamReader(responseContent);
                                result.ErrorMessage = DataContractSerializer.DeserializeObject<string>(reader.ReadToEnd());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = $"The Server is Unavailable. Please, contact with the server administrator. Error occured: {ex.Message}";
                    }
                }
            }
            return result;
        }
        public static async Task<APIResult> ClearHistory(string userToken)
        {
            APIResult result = new APIResult();
            using (var request = new HttpRequestMessage(HttpMethod.Delete, $"{URL}/History/ClearHistory/"))
            {
                var user = new APIUser() { Token = userToken };
                var json = DataContractSerializer.SerializeObject(user);
                using (var stringContent = new StringContent(json))
                {
                    request.Content = stringContent;
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None).ConfigureAwait(false))
                        {
                            if (response.StatusCode != HttpStatusCode.OK)
                            {
                                var responseContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                                StreamReader reader = new StreamReader(responseContent);
                                result.ErrorMessage = DataContractSerializer.DeserializeObject<string>(reader.ReadToEnd());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = $"The Server is Unavailable. Please, contact with the server administrator. Error occured: {ex.Message}";
                    }
                }
            }
            return result;
        }
    }
}
