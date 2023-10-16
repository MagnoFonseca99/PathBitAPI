using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.IO;
using System.Net.Mail;

namespace PathBitAPI.Models
{
    public class ClienteValidacao
    {

        public static string ValidarDados(Cliente newCliente)
        {
            if (newCliente.Id != null || newCliente.Deletado == true) return "'Id' ou flag 'Deletado' não deve ser enviado para criação";
          
            if (!ValidarEmail(newCliente.Seguranca.Email)) return "Email invalido";
           
            if (DateTime.Now.AddYears(-18).Date < newCliente.Nascimento) return "Apenas maiores de 18 anos permitido";
           
            if((newCliente.Financeiro.Renda + newCliente.Financeiro.Patrimonio) < 1000) return  "Renda + Patrimonio deve ser maior que 1000";

            if (!ValidarOnzeDigitosNumericos(newCliente.CPF)) return "CPF Inválido";

            if (!ValidarOnzeDigitosNumericos(newCliente.Telefone)) return "Telefone Inválido";


            return ("ok");
        }
        static bool ValidarOnzeDigitosNumericos(string cpf)
        {
            // Verifique se o CPF tem exatamente 
            if (cpf.Length == 11)
            {
                return true;
            }
            return false;
        }

        public static bool ValidarEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return mailAddress.Address == email;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}
