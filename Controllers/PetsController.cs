using LHPets.Data;
using LHPets.Models;
using Microsoft.AspNetCore.Mvc;

namespace LHPets.Controllers;

[ApiController]
[Route("api/pets")]
public class PetsController : ControllerBase
{
    private readonly CadastroEmMemoria _cadastro;

    public PetsController(CadastroEmMemoria cadastro)
    {
        _cadastro = cadastro;
    }

    // GET /api/pets
    [HttpGet]
    [ProducesResponseType(typeof(List<Pet>), StatusCodes.Status200OK)]
    public ActionResult<List<Pet>> Listar()
    {
        return Ok(_cadastro.ListarPets());
    }

    // POST /api/pets
    [HttpPost]
    [ProducesResponseType(typeof(Pet), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Pet> Cadastrar([FromBody] Pet pet)
    {
        var novoPet = _cadastro.AdicionarPet(pet);

        if (novoPet is null)
        {
            return BadRequest(new { mensagem = "O ClienteId informado não corresponde a um cliente cadastrado." });
        }

        return StatusCode(StatusCodes.Status201Created, novoPet);
    }

    // GET /api/pets/cliente/{clienteId}
    [HttpGet("cliente/{clienteId:int}")]
    [ProducesResponseType(typeof(List<Pet>), StatusCodes.Status200OK)]
    public ActionResult<List<Pet>> ListarPorCliente(int clienteId)
    {
        return Ok(_cadastro.ListarPetsPorCliente(clienteId));
    }
}
