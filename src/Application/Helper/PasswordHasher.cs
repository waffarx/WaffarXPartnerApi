using System.Security.Cryptography;


namespace WaffarXPartnerApi.Application.Helper;
public class PasswordHasher : IPasswordHasher
{
    public (string hashedPassword, string hashKey) HashPassword(string password)
    {
        // Generate a random salt (hash key)
        byte[] salt = new byte[16];
        RandomNumberGenerator.Fill(salt);

        // Hash the password with the salt
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(20);

        // Convert salt to string for storage
        string hashKey = Convert.ToBase64String(salt);

        // Convert hash to string for storage
        string hashedPassword = Convert.ToBase64String(hash);

        return (hashedPassword, hashKey);
    }

    public bool VerifyPassword(string password, string hashedPassword, string hashKey)
    {
        // Convert the stored salt back to bytes
        byte[] salt = Convert.FromBase64String(hashKey);

        // Hash the input password with the same salt
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(20);

        // Convert to string for comparison
        string newHashedPassword = Convert.ToBase64String(hash);

        // Compare the hashed passwords
        return hashedPassword == newHashedPassword;
    }
}
