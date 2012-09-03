using System.Collections.Generic;
using System.Threading.Tasks;
using PasswordTool.Services.ServiceModels;

namespace PasswordTool.Services
{
    public interface IWordService
    {
        Task<IEnumerable<WordItem>> RandomWords(int? minimumWordLength, int? maximumResults, int? maximumWordLength = 8,
                                                int? corpusCount = 1000);
    }
}