using JwtHomework.Entities;
using System.Net;
using System.Net.Mail;

namespace JwtHomework.EmailService
{
    public static class EmailSender
    {
        public static void Send(Account account)
        {
            SmtpClient smtp = new SmtpClient();       
            smtp.Host ="smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("denememailservice@gmail.com", "cgppknzuimwbjzjj");

            MailMessage mail = new MailMessage("denememailservice@gmail.com", account.Email);
            mail.Subject = "Bilgilendirme.";
            mail.Body = $"Kulanıcı Başarıyla Kayıt olmuştur. \n \n Kullanıcı UserName : {account.UserName} \n  Kullanıcı İsmi : {account.Name} ";


            
            try
            {
                smtp.Send(mail);
            }
            catch (Exception)
            {

                Console.WriteLine($"Mesaj gönderilemedi.");
            }

        }

    }
}
