using LHPets.Models;

namespace LHPets.Data;

/// <summary>
/// Armazena clientes e pets em List&lt;T&gt;, sem banco de dados.
/// Os dados são perdidos quando a aplicação é encerrada.
/// </summary>
public class CadastroEmMemoria
{
    private readonly object _controle = new();
    private readonly List<Cliente> _clientes = new();
    private readonly List<Pet> _pets = new();
    private int _proximoClienteId = 1;
    private int _proximoPetId = 1;

    public List<Cliente> ListarClientes()
    {
        lock (_controle)
        {
            return _clientes.ToList();
        }
    }

    public Cliente AdicionarCliente(Cliente cliente)
    {
        lock (_controle)
        {
            cliente.Id = _proximoClienteId++;
            _clientes.Add(cliente);
            return cliente;
        }
    }

    public List<Pet> ListarPets()
    {
        lock (_controle)
        {
            return _pets.ToList();
        }
    }

    public List<Pet> ListarPetsPorCliente(int clienteId)
    {
        lock (_controle)
        {
            return _pets.Where(pet => pet.ClienteId == clienteId).ToList();
        }
    }

    public Pet? AdicionarPet(Pet pet)
    {
        lock (_controle)
        {
            // Só cadastra um pet se o cliente informado existir.
            if (!_clientes.Any(cliente => cliente.Id == pet.ClienteId))
            {
                return null;
            }

            pet.Id = _proximoPetId++;
            _pets.Add(pet);
            return pet;
        }
    }
}
