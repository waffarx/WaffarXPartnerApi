using System.Text;

namespace WaffarXPartnerApi.Application.Common.DTOs.Helper;
public class RandomGenerator
{
    // Instantiate random number generator.  
    // It is better to keep a single Random instance 
    // and keep using Next on the same instance.  
    private readonly Random _random = new Random();

    // Generates a random number within a range.      
    public int RandomNumber(int min, int max)
    {
        return _random.Next(min, max);
    }

    // Generates a random string with a given size.    
    public string RandomString(int size)
    {

        Random ran = new Random();

        String b = "abcdefghijklmnopqrstuvwxyz0123456789";
        String sc = "!@#$%^&*~";
        String random = "";

        for (int i = 0; i < size; i++)
        {
            int a = ran.Next(b.Length); //string.Lenght gets the size of string
            random = random + b.ElementAt(a);
        }
        for (int j = 0; j < 2; j++)
        {
            int sz = ran.Next(sc.Length);
            random = random + sc.ElementAt(sz);
        }
        return random;
    }

    // Generates a random password.  
    // 4-LowerCase + 4-Digits + 2-UpperCase  
    public string RandomPassword()
    {
        var passwordBuilder = new StringBuilder();

        // 4-Letters lower case   
        passwordBuilder.Append(RandomString(4));

        // 4-Digits between 1000 and 9999  
        passwordBuilder.Append(RandomNumber(1000, 9999));

        // 2-Letters upper case  
        passwordBuilder.Append(RandomString(2));
        return passwordBuilder.ToString();
    }
}
