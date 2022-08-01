// See https://aka.ms/new-console-template for more information
using JwtHomework.EmailService;
using JwtHomework.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

//Consumar tarafında tüketecegimiz mesaj kurugunu burada tekrar declare ediyoruz ve bir connection ve kanal açıyoruz.

//Baglantıyı Oluşturuyoruz..
ConnectionFactory factory = new ConnectionFactory();
//RabbitMQ instance’ına bağlantı kurabilmek için AMQP URL'sini veriyoruz.
factory.Uri = new Uri("amqps://uugrsvlg:PSvS-Cmg29qI_ldEpVMnDBHe2FjoI6EE@hawk.rmq.cloudamqp.com/uugrsvlg");
//Oluşturdugumuz factory ile bir connection oluşturuyoruz.
using IConnection connection = factory.CreateConnection();
//Oluşturdugumuz connection ile bir Channel(Kanal) oluşturduk.
using IModel channel = connection.CreateModel();

//Oluşturdugumuz Channel(Kanal) ile QueueDeclare metodu kullanarak mesajları tüketecegimiz kuyrugu belirtiyoruz.
//Queue = oluşturulcak kuyruk ismi  
//durable = RabbitMQ sunucuları restart atılırsa veriler kaybolabilir.True yaparsak veriler kalıcı hale getiriliyor.
//exclusive = Oluşturulan kuyruga birden fazla kanalın baglanıp baglanmıyacagını belirtiyoruz.
//autoDelete = tüm mesajlar bitince kuyruğu otomatik imha edilsini veya edilmemesini belirtiyoruz.
channel.QueueDeclare("email_queue", false, false, false);

//Kuyruktaki mesajları yakalayacak bir event oluşturuyoruz.
EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

//BasicConsume ile event ile  yakaladıgımız mesaları tüketiyoruz.
//queue = oluşturulcak kuyruk ismi  
//autoAck =Kuruktan alınan mesajın silinip silinmemesini belirtiyoruz.
//consumer : Kuyruktaki mesajları yakalayacak Tüketici eventi.
channel.BasicConsume("email_queue", true, consumer);


//consumer.Received ile kuyruga gelen mesajı tüketigimiz event oluşturuyoruz.Kuyruga gelen mesajlar yakalanıcak ve işlenecegi yer.
//Buradaki  (s, e) ise  s => Object sender , e => BasicDeliverEventArgs e parametreletine karşşılık geliyor.
consumer.Received += (s, e) =>
{
    //e.Body.Span ile kuyruktan gelen mesaja ulaşabiliyoruz.(gelen bu mesaj byte[] olarak geliyor) Gelen mesajı önce string e ceviriyoruz.
    string serializData = Encoding.UTF8.GetString(e.Body.Span);
    //String'e cevrilen mesaji Deserialize ederek Account tipine dönüştürüyoruz.
    Account account = JsonSerializer.Deserialize<Account>(serializData);

    //Account Tipne dönüştürdügümüz mesajı arguman olarak EmailSender.Send(account) veriyoruz ve email gönderme işlemini başlatıryoruz.
    EmailSender.Send(account);
};

Console.Read();
