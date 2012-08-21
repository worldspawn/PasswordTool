using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PasswordTool.Web.Models
{
    public class PasswordRequest
    {
        public int WordCount { get; set; }
        public int WordComplexity { get; set; }
        public int MinimumWordLength { get; set; }
        public int MaximumWordLength { get; set; }
        public int HashLength { get; set; }
        public int SaltLength { get; set; }
        public int Iterations { get; set; }
        public SourceType SourceType { get; set; }
        public string PassPhrase { get; set; }
    }

    public enum SourceType
    {
        Auto,
        Manual
    }
}