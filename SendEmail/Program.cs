using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using System.Text;
using Newtonsoft.Json;
using MongoDB.Driver.Core.Configuration;

namespace SendEmail
{
    internal class Program
    {
        static void Main(string[] args)
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\net6.0\\",""))
            .AddJsonFile("appsettings.json")
            .Build();

            string connectionString = configuration.GetConnectionString("RabbitMQ");

            var factory = new ConnectionFactory
            {
                Uri = new Uri(connectionString)
            };
            using var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();
            ;
            channel.QueueDeclare("demo", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                await SendEmailToClient(message);

                Console.WriteLine(message);
            };



            channel.BasicConsume("demo", true, consumer);

            Console.ReadLine();
        }


        static async Task SendEmailToClient(string message)
        {
            // Replace with your SendGrid API Key
            //Senha = "Z4ReX26qXVhrDUEJ9wTkVay";

            string apiKey = "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";

            _Evento dados = JsonConvert.DeserializeObject<_Evento>(message);

            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("mgfonseca1999@gmail.com", "PathBit Test");

            var to = new EmailAddress(dados.Email,  dados.Nome);
            var subject = "Hello from PathBit!";
            var plainTextContent = @$"Olá {dados.Nome},

                                    O seu cadastro está em análise e em breve você receberá um e-mail com novas atualizações sobre seu cadastro.

                                    Atenciosamente,

                                    Equipe PATHBIT";
            var htmlContent = "<p>This is an HTML email sent from SendGrid.</p>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                Console.WriteLine("Email sent successfully!");
            }
            else
            {
                Console.WriteLine($"Failed to send email. Status code: {response.StatusCode}");
            }
        }

        internal class _Evento
        {
            public string Evento { get; set; }

            public string Email { get; set; }

            public string Nome { get; set; }
        }

    }
}