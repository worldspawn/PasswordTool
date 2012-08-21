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
    }
}