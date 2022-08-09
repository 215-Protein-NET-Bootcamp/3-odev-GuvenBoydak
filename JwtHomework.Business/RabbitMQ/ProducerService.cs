using JwtHomework.Entities;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace JwtHomework.Business
{
    public static class ProducerService
    {
        public static void Producer(Account account)
        {
            //Baglantıyı Oluşturuyoruz..
            ConnectionFactory factory = new ConnectionFactory();
            //RabbitMQ instance’ına bağlantı kurabilmek için AMQP URL'sini veriyoruz.
            factory.Uri = new Uri("amqps://uugrsvlg:PSvS-Cmg29qI_ldEpVMnDBHe2FjoI6EE@hawk.rmq.cloudamqp.com/uugrsvlg");
            //Oluşturdugumuz factory ile bir connection oluşturuyoruz.
            using IConnection connection=factory.CreateConnection();
            //Oluşturdugumuz connection ile bir Channel(Kanal) oluşturduk.
            using IModel channel = connection.CreateModel();

            //Oluşturdugumuz Channel(Kanal) ile QueueDeclare metodu kullanarak mesajlarımızı göndericemiz bir kuyruk oluşturuyoruz.
            //Queue = oluşturulcak kuyruk ismi  
            //durable = RabbitMQ sunucuları restart atılırsa veriler kaybolabilir.True yaparsak veriler kalıcı hale getiriliyor.
            //exclusive = Oluşturulan kuyruga birden fazla kanalın baglanıp baglanmıyacagını belirtiyoruz.
            //autoDelete = tüm mesajlar bitince kuyruğu otomatik imha edilsini veya edilmemesini belirtiyoruz.
            channel.QueueDeclare("email_queue", false, false, false);

            //Parametreden gelen degeri önce Json formata serialize ediyoruz.
            string data = JsonSerializer.Serialize(account);
            //Serialize edilen degeri Kuyruga byte[] olarak göndermemeiz gerekiyor. Bu yüzden byte[] e çeviriyoruz.
            Byte[] bytes = Encoding.UTF8.GetBytes(data);

            //Kanal üzerinden BasicPublish ile kuyruga mesajlarımızı gönderiyoruz.
            //exchange = Mesajların hangi kuruga gitmesini istiyosak ona göre bir exchange tipi belitiyoruz ve exchange sayesinde mesajler kuruklara iletiliyor.Boş bıraktıgımız için default exchange olan Fanout exchange i kullanıyoruz.
            //routingKey = Kuyruga Yönlendirilecek mesajlar işlemi için bir key oluşturuyoruz ve bu sayede exchangeler bu key degerine göre kuyruklara mesajları iletiyor.
            //body = Gönderilecek mesaj(Byte[] tipinde olmalıdır.)
            channel.BasicPublish("", "email_queue", body: bytes);
        }
    }
}
