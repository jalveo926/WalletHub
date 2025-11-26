namespace WalletHub.Services.Interface
{
    public interface IPasswordHashService
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }
}
