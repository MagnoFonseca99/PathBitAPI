using PathBitAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PathBitAPI.Services;

public class ClienteService
{
    private readonly IMongoCollection<Cliente> _ClientesCollection;

    public ClienteService(
        IOptions<DataBaseSettings> ClienteStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            ClienteStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            ClienteStoreDatabaseSettings.Value.DatabaseName);

        _ClientesCollection = mongoDatabase.GetCollection<Cliente>(
            ClienteStoreDatabaseSettings.Value.CollectionName);
    }

    public async Task<List<Cliente>> GetAsync() =>
        await _ClientesCollection.Find(_ => true && _.Deletado == false).ToListAsync();

    public async Task<Cliente?> GetAsync(string id) =>
        await _ClientesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Cliente newCliente)
    {
        try
        {
            newCliente.Deletado = false;
            newCliente.DataCadastro = DateTime.Now;
            newCliente.DataDeletado = DateTime.Now;
            await _ClientesCollection.InsertOneAsync(newCliente);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task UpdateAsync(string id, Cliente updatedCliente)
    {
        updatedCliente.DataUltimoUpdate = DateTime.Now;
        await _ClientesCollection.ReplaceOneAsync(x => x.Id == id, updatedCliente);
    }

    public async Task RemoveAsync(string id)
    {
        Cliente updatedCliente = await _ClientesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        updatedCliente.Deletado = true;
        updatedCliente.DataDeletado = DateTime.Now;
        await _ClientesCollection.ReplaceOneAsync(x => x.Id == id, updatedCliente);
    }
}