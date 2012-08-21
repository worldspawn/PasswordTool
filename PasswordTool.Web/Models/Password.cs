using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PasswordTool.Web.Models
{
    public class Password
    {
        public string[] PasswordElements { get; set; }
        public byte[] HashSalt { get; set; }
        public byte[] Hash { get; set; }

        public string HashBase64
        {
            get
            {
                if (Hash == null)
                    return null;

                return Convert.ToBase64String(Hash);
            }
        }

        public string HashSaltBase64
        {
            get
            {
                if (HashSalt == null)
                    return null;

                return Convert.ToBase64String(HashSalt);
            }
        }

        public SourceType SourceType { get; set; }
    }
}