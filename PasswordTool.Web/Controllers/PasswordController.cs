using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PasswordTool.Services;
using PasswordTool.Web.Models;

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
        [OutputCache(NoStore = true)]
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

            var hash = _hashService.Hash(passPhraseBytes, salt, passwordRequest.Iterations, passwordRequest.HashLength);
            var password = new Password
                               {
                                   Hash = hash,
                                   HashSalt = salt,
                                   PasswordElements = passwordElements,
                                   SourceType = passwordRequest.SourceType
                               };
            
            return new JsonResult<Password>(password);
        }
    }

    public class JsonResult<TModel> : ActionResult
    {
        private readonly TModel _model;

        public JsonResult(TModel model)
        {
            _model = model;
        }

        public TModel Model
        {
            get { return _model; }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var payload = JsonConvert.SerializeObject(_model,
                                               new JsonSerializerSettings
                                                   {
                                                       ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                       Converters = new List<JsonConverter> { new StringEnumConverter() }
                                                   });
            context.HttpContext.Response.Write(payload);
        }
    }
}
