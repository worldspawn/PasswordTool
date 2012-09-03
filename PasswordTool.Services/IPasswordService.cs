namespace PasswordTool.Services
{
    public interface IPasswordService
    {
        byte[] GeneratePassword();
        byte[] GeneratePassword(PasswordSettings settings);
    }
}