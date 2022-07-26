using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtHomework.Base
{
    public class SecurityKeyHelper
    {
        //Kullandıgımız securityKey'i Asp Net jwt servislerinin anliyacagı hale getirmemeiz gerekiyor bu yüzden string ifadeyi byte array formatına çeviriyoruz buradaki method sayesinde.
        public static SecurityKey CreateSecurityKey(string securityKey)
        {
            //SymmetricSecurityKey ile Security  Key'in simetriğini alıyoruz.
            //Encoding.UTF8.GetBytes(securityKey) ile string ifadeyi byte array'e dönüştürür.
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}
