using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PasswordTool.Services.ServiceModels;

namespace PasswordTool.Services
{
    public interface IWordService
    {
        Task<IEnumerable<WordItem>> RandomWords(int? minimumWordLength, int? maximumResults);
    }

    public class WordService : IWordService
    {
        private readonly Uri ServiceUri = new Uri("https://api.wordnik.com//v4/words.json/");

        #region IWordService Members

        public Task<IEnumerable<WordItem>> RandomWords(int? minimumWordLength, int? maximumResults)
        {
            var client = new HttpClient();
            string path = "randomWords?hasDictionaryDef=true&minCorpusCount=1000&maxLength=8";
            if (minimumWordLength.HasValue)
                path = string.Format("{0}&minLength={1}", path, minimumWordLength);
            if (maximumResults.HasValue)
                path = string.Format("{0}&limit={1}", path, maximumResults);

            var requestUri = new Uri(ServiceUri, new Uri(path, UriKind.Relative));
            client.DefaultRequestHeaders.Add("api_key", ConfigurationManager.AppSettings["WordnikAPIKey"]);

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