namespace WaffarXPartnerApi.Application.Helper;
public interface IPasswordHasher
{
    (string hashedPassword, string hashKey) HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword, string hashKey);
}
