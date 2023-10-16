using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace PathBitAPI.Models;

public class Cliente
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required(ErrorMessage = "{0} is Required")]
    public string Nome { get; set; } = null!;


    [Required(ErrorMessage = "{0} is Required")]
    public DateTime Nascimento { get; set; }


    [Required(ErrorMessage = "{0} is Required")]
    public string CPF { get; set; } = null!;

    [Required(ErrorMessage = "{0} is Required")]
    public string Telefone { get; set; } = null!;

    [Required(ErrorMessage = "{0} is Required")]
    public DadosFinanceiros Financeiro { get; set; } = null!;

    [Required(ErrorMessage = "{0} is Required")]
    public DadosEndereco Endereco { get; set; } = null!;


    [Required(ErrorMessage = "{0} is Required")]
    public DadosSeguranca Seguranca { get; set; } = null!;


    [DataType(DataType.Date)]
    public DateTime DataCadastro { get; set; }

    public bool Deletado { get; set; }

    [DataType(DataType.Date)]
    public DateTime DataDeletado { get; set; }

    [DataType(DataType.Date)]
    public DateTime DataUltimoUpdate { get; set; }
}
public class DadosFinanceiros
{
    [Required(ErrorMessage = "{0} is Required")]
    [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
    public Double Renda { get; set; } = 0.0;


    [Required(ErrorMessage = "{0} is Required")]
    [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
    public Double Patrimonio { get; set; } = 0.0;
}

public class DadosEndereco
{

    [Required(ErrorMessage = "{0} is Required")]
    public string Rua { get; set; } = null!;


    [Required(ErrorMessage = "{0} is Required")]
    public string Numero { get; set; } = null!;


    [Required(ErrorMessage = "{0} is Required")]
    public string Bairro { get; set; } = null!;


    [Required(ErrorMessage = "{0} is Required")]
    public string Cidade { get; set; } = null!;


    [Required(ErrorMessage = "{0} is Required")]
    public string Estado { get; set; } = null!;


    [Required(ErrorMessage = "{0} is Required")]
    public string CEP { get; set; } = null!;
}

public class DadosSeguranca
{
    [Required(ErrorMessage = "{0} is required")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "{0} is required")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = null!;

    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare("Senha")]
    public string ConfirmacaoSenha { get; set; } = null!;
}
