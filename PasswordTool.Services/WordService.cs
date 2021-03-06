using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PasswordTool.Services.ServiceModels;

namespace PasswordTool.Services
{
    public class WordService : IWordService
    {
        private readonly string _worknikAPIKey;
        private readonly Uri _serviceUri;

        public WordService(string worknikAPIKey, Uri serviceUri)//https://api.wordnik.com//v4/words.json/
        {
            _worknikAPIKey = worknikAPIKey;
            _serviceUri = serviceUri;
            if (worknikAPIKey == null) throw new ArgumentNullException("worknikAPIKey");
            if (serviceUri == null) throw new ArgumentNullException("serviceUri");
        }

        #region IWordService Members

        public Task<IEnumerable<WordItem>> RandomWords(int? minimumWordLength, int? maximumResults, int? maximumWordLength = 8, int? corpusCount = 1000)
        {
            var client = new HttpClient();
            string path = "randomWords?hasDictionaryDef=true";
            if (corpusCount.HasValue)
                path = string.Format("{0}&minCorpusCount={1}", path, corpusCount);
            if (maximumWordLength.HasValue)
                path = string.Format("{0}&maxLength={1}", path, maximumWordLength);
            if (minimumWordLength.HasValue)
                path = string.Format("{0}&minLength={1}", path, minimumWordLength);
            if (maximumResults.HasValue)
                path = string.Format("{0}&limit={1}", path, maximumResults);

            var requestUri = new Uri(_serviceUri, new Uri(path, UriKind.Relative));
            client.DefaultRequestHeaders.Add("api_key", _worknikAPIKey);

            Task<HttpResponseMessage> task = client.GetAsync(requestUri);
            return Task.Factory.StartNew(() =>
                                             {
                                                 task.Wait(new TimeSpan(0, 0, 30));
                                                 HttpResponseMessage response = task.Result;
                                                 if (response.IsSuccessStatusCode)
                                                 {
                                                     Task<string> contentTask = response.Content.ReadAsStringAsync();
                                                     contentTask.Wait(new TimeSpan(0, 0, 30));
                                                     string content = contentTask.Result;
                                                     return
                                                         JsonConvert.DeserializeObject<WordItem[]>(
                                                             content,
                                                             new JsonSerializerSettings
                                                                 {
                                                                     ContractResolver =
                                                                         new CamelCasePropertyNamesContractResolver()
                                                                 }).AsEnumerable();
                                                 }

                                                 return null;
                                             });
        }

        #endregion
    }
}