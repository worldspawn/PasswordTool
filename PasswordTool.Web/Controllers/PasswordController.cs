using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PasswordTool.Services;
using PasswordTool.Web.Models;
using PasswordTool.Web.Results;

namespace PasswordTool.Web.Controllers
{
    public class PasswordController : Controller
    {
        private readonly IWordService _wordService;
        private readonly IHashService _hashService;

        public PasswordController(IWordService wordService, IHashService hashService)
        {
            _wordService = wordService;
            _hashService = hashService;
        }

        [HttpGet]
        [OutputCache(Duration=int.MaxValue,VaryByParam = "none")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [OutputCache(Duration=0, NoStore = true)]
        public JsonResult<Password> GeneratePassword(PasswordRequest passwordRequest)
        {
            if (passwordRequest.WordCount > 6)
                return null;

            if (passwordRequest.HashLength > 1024)
                return null;

            if (passwordRequest.SaltLength > passwordRequest.HashLength * 0.5m)
                return null;

            if (passwordRequest.Iterations > 10000)
                return null;

            byte[] passPhraseBytes;
            var salt = _hashService.CreateSalt(passwordRequest.SaltLength);
            string[] passwordElements;

            if (passwordRequest.SourceType == SourceType.Auto)
            {
                var wordTask = _wordService.RandomWords(passwordRequest.MinimumWordLength, passwordRequest.WordCount,
                                                        passwordRequest.MaximumWordLength,
                                                        passwordRequest.WordComplexity);
                wordTask.Wait();
                passwordElements = wordTask.Result.Select(w => w.Word).ToArray();
                passPhraseBytes = Encoding.UTF8.GetBytes(string.Join(string.Empty, passwordElements));
            }
            else
            {
                passPhraseBytes = Encoding.UTF8.GetBytes(string.Join(string.Empty, passwordRequest.PassPhrase));
                passwordElements = new[] {passwordRequest.PassPhrase};
            }

            var hash = _hashService.Hash(passPhraseBytes, salt, passwordRequest.Iterations, passwordRequest.HashLength + 4);//pad length as we will remove the version number

            var password = new Password
                               {
                                   Hash = hash.Skip(4).ToArray(),//crop the version number from the start
                                   HashSalt = salt,
                                   PasswordElements = passwordElements,
                                   SourceType = passwordRequest.SourceType
                               };
            
            return new JsonResult<Password>(password);
        }
    }
}
