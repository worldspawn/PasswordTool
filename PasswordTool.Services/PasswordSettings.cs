namespace PasswordTool.Services
{
    public class PasswordSettings
    {
        public PasswordSettings()
        {
            Length = 10;
            MaximumNonAlphaChars = MinimumNonAlphaChars = 3;
            MaximumUpperCase = MinimumUpperCase = 2;
            MaximumDigits = MinimumDigits = 1;
            IsUsKeyboard = true;
        }

        public bool IsUsKeyboard { get; set; }
        public int Length { get; set; }
        public int MinimumNonAlphaChars { get; set; }
        public int MaximumNonAlphaChars { get; set; }
        public int MinimumUpperCase { get; set; }
        public int MaximumUpperCase { get; set; }
        public int MinimumDigits { get; set; }
        public int MaximumDigits { get; set; }
    }
}