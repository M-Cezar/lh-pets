using LHPets.Data;
using LHPets.Models;
using Microsoft.AspNetCore.Mvc;

namespace LHPets.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly CadastroEmMemoria _cadastro;

    public ClientesController(CadastroEmMemoria cadastro)
    {
        _cadastro = cadastro;
    }

    // GET /api/clientes
    [HttpGet]
    [ProducesResponseType(typeof(List<Cliente>), StatusCodes.Status200OK)]
    public ActionResult<List<Cliente>> Listar()
    {
        return Ok(_cadastro.ListarClientes());
    }

    // POST /api/clientes
    [HttpPost]
    [ProducesResponseType(typeof(Cliente), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Cliente> Cadastrar([FromBody] Cliente cliente)
    {
        var novoCliente = _cadastro.AdicionarCliente(cliente);
        return StatusCode(StatusCodes.Status201Created, novoCliente);
    }
}
