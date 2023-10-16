using Amazon.Runtime.Internal.Auth;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace PathBitAPI.Models
{
    public class RabbitMQProducer
    {

        #region Construtor
        private readonly string _conexao;
        public RabbitMQProducer(string Conexao)
        {
            _conexao = Conexao;
        }
        #endregion
        public bool SendEvent(Cliente Cliente)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri(uriString: _conexao)
                };
                using var connection = factory.CreateConnection();

                using var channel = connection.CreateModel();
                ;
                channel.QueueDeclare("demo", durable: true, exclusive: false, autoDelete: false, arguments: null);
                var messege = new { Evento = "Novo Cliente Inserido", Cliente.Seguranca.Email, Cliente.Nome };

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messege));

                channel.BasicPublish("", "demo", null, body);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }

}

