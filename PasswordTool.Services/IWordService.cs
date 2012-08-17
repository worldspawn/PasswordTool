using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
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

        public Task<IEnumerable<WordItem>> RandomWords(int? minimumWordLength, int? maximumResults)
        {
            var client = new HttpClient();
            var path = "randomWords?hasDictionaryDef=true&minCorpusCount=1000";
            if (minimumWordLength.HasValue)
                path = string.Format("{0}&minLength={1}", path, minimumWordLength);
            if (maximumResults.HasValue)
                path = string.Format("{0}&limit={1}", path, maximumResults);

            var requestUri = new Uri(ServiceUri, new Uri(path, UriKind.Relative));
            client.DefaultRequestHeaders.Add("api_key", ConfigurationManager.AppSettings["WordnikAPIKey"]);

            var task = client.GetAsync(requestUri);
            return Task.Factory.StartNew(() =>
                                      {
                                          task.Wait(new TimeSpan(0, 0, 30));
                                          var response = task.Result;
                                          if (response.IsSuccessStatusCode)
                                          {
                                              var contentTask = response.Content.ReadAsStringAsync();
                                              contentTask.Wait();
                                              var content = contentTask.Result;
                                              return
                                                  Newtonsoft.Json.JsonConvert.DeserializeObject<WordItem[]>(
                                                      content,
                                                      new JsonSerializerSettings()
                                                          {
                                                              ContractResolver =
                                                                  new CamelCasePropertyNamesContractResolver()
                                                          }).AsEnumerable();
                                          }

                                          return null;
                                      });

        }
    }
}
