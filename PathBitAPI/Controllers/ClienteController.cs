using PathBitAPI.Models;
using PathBitAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace PathBitAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly ClienteService _ClienteService;
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;
    private string GetUrlRabbitMQ { get { return this._configuration.GetValue<string>("Fila:RabbitMQ"); } }

    public ClientesController(IConfiguration configuration,ClienteService ClienteService, ILogger<ClientesController> logger)
    {
        _configuration = configuration;
        _ClienteService = ClienteService;
        _logger = logger;
    }


    #region Usar Depois
    [HttpGet("ListAll")]
    public async Task<List<Cliente>> Get()
    {

        return await _ClienteService.GetAsync();

    }

    [HttpGet("ConsultById/{id:length(24)}")]
    public async Task<ActionResult<Cliente>> Get(string id)
    {
        var Cliente = await _ClienteService.GetAsync(id);

        if (Cliente is null)
        {
            return NotFound();
        }

        return Cliente;
    }
    #endregion

    [HttpPost("Register")]
    public async Task<IActionResult> Post(Cliente newCliente)
    {
        try
        {
            newCliente.CPF = new string(newCliente.CPF.Where(char.IsDigit).ToArray());                // Remove caracteres não numéricos
            newCliente.Telefone = new string(newCliente.Telefone.Where(char.IsDigit).ToArray());      // Remove caracteres não numéricos

            string Retorno = PathBitAPI.Models.ClienteValidacao.ValidarDados(newCliente);
            if (Retorno != "ok") return StatusCode(400, Retorno);

            try
            {

                await _ClienteService.CreateAsync(newCliente);

            }
            catch (Exception e)
            {
                _logger.LogError("Erro ao Inserir no MongoDB:  " + e.Message);
                return StatusCode(500, "Ocorreu um erro, tende novamente mais tarde");
            }
            try
            {

                bool retorno = new RabbitMQProducer(GetUrlRabbitMQ).SendEvent(newCliente);

            }
            catch (Exception e)
            {
                _logger.LogError("Erro ao enviar evento ao RabbotMQ" + e.Message);
                return StatusCode(500, "Ocorreu um erro, tende novamente mais tarde");
            }

            return CreatedAtAction(nameof(Get), new { id = newCliente.Id }, newCliente);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return StatusCode(500, "Ocorreu um erro, tende novamente mais tarde");
        }
       

        
    }

    #region Usar Depois

    [HttpPut("UpdateById/{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Cliente updatedCliente)
    {
        var Cliente = await _ClienteService.GetAsync(id);

        if (Cliente is null)
        {
            return NotFound();
        }

        updatedCliente.Id = Cliente.Id;

        await _ClienteService.UpdateAsync(id, updatedCliente);

        return NoContent();
    }

    [HttpDelete("DeletebyId{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var Cliente = await _ClienteService.GetAsync(id);

        if (Cliente is null)
        {
            return NotFound();
        }

        await _ClienteService.RemoveAsync(id);

        return NoContent();
    }

    #endregion


}