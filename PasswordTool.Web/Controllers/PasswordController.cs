using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using PasswordTool.Services;
using PasswordTool.Web.Models;

namespace PasswordTool.Web.Controllers
{
    public class PasswordController : Controller
    {
        private readonly IWordService _wordService;

        public PasswordController(IWordService wordService)
        {
            _wordService = wordService;
        }
        //
        // GET: /Password/

        [HttpGet]
        public ActionResult Index()
        {
            return View();
            //Password result = new Password();
            
            //var wordTask = _wordService.RandomWords(5, 3);
            //var random = new RNGCryptoServiceProvider();
            //byte[] salt = new byte[4];
            //random.GetBytes(salt);

            //result.HashSalt = salt;

            //using (var sha512 = new SHA512Managed())
            //{
            //    wordTask.Wait();
            //    var words = wordTask.Result;
            //    if (words == null)
            //        return new EmptyResult();
            //    result.PasswordParts = words.Select(w => w.Word.ToLower()).ToArray();
            //    var password = string.Join(string.Empty, result.PasswordParts);
            //    var passwordBytes = System.Text.UTF8Encoding.UTF8.GetBytes(password).ToArray();
            //    var hash = sha512.ComputeHash(passwordBytes);
            //    salt.CopyTo(hash, hash.Length - salt.Length);

            //    result.OriginalPassword = password;
            //    result.Hash = hash;
            //}

            //return View(result);
        }

        [HttpGet]
        public ActionResult GeneratePassword(int minimumWordLength, int wordCommonalityWeighting, int noWords, int saltLength)
        {
            throw new NotImplementedException();
        }
    }
}
