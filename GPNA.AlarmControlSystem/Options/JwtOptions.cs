using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GPNA.AlarmControlSystem.Options;

public class JwtOptions
{
    public const string ISSUER = "ACSService"; // издатель токена
    public const string AUDIENCE = "ACS"; // потребитель токена
    public const string KEY = "AlarmControlSystemSecretDamnToken"; // ключ для шифрации
    public const int LIFETIME = 20; // время жизни токена - 1 минута

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}