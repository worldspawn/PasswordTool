using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PasswordTool.Web.Models
{
    public class Password
    {
        public string OriginalPassword { get; set; }
        public string[] PasswordParts { get; set; }
        public byte[] HashSalt { get; set; }
        public byte[] Hash { get; set; }
        public string HashAsString
        {
            get { return Convert.ToBase64String(Hash, Base64FormattingOptions.None); }
        }

        public string HashSaltAsString
        {
            get { return Convert.ToBase64String(HashSalt); }
        }
    }
}