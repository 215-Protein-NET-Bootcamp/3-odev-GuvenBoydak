using System.Text;

namespace JwtHomework.Base
{
    public class HashingHelper
    {
        //Bir PasswordHash oluşturuyoruz parametre olarak verdigimiz ilk deger password,ikinci deger ise out ile bellekte bir passwordHash oluştuyor,üçüncü ise out ile bellekte bir passwordHash oluştuyor.Bellekte oluşan bu passwordHash ve passwordSalt ile diger yazdıgımız methodda kullanabiliyoruz.
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac=new System.Security.Cryptography.HMACSHA512())
            {
                //passwordSalt'a hmac.key degerini veriyoruz.Daha sonra passwordHash'i bunun sayesinde dogrulama kontrolu yapıyoruz.
                passwordSalt = hmac.Key;
                //Bir passwordHash oluşturuyoruz.Parametre olaraka verdigimiz Encoding.UTF8.GetBytes ile password'u byte arraye ceviriyor.
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        //ilk mettoddan bellekte oluşturdugumuz passwordHash ve passwordSalt kullanarak PasswordHash i Dogrulama kontrolu yapıyoruz.
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            //Yukarıda oluşturdugumuz passwordSalt'İ parametre olarak veiyoruz.
            using (var hmac=new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                //HMACSHA512(passwordSalt) sayesinde girilen password'u hashliyoruz.
                var computeHash =hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computeHash.Length; i++)
                {
                    //computeHash'i tek tek dönüyruz ve passwordHash ile aynımı kontrolu yapıyoruz.
                    if (computeHash[i] != passwordHash[i])
                        return false;
                }
            }
            return true;
        }
       
    }
}
