using System.ComponentModel.DataAnnotations;

namespace LHPets.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CPF do cliente é obrigatório.")]
    public string Cpf { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail do cliente é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    public string Email { get; set; } = string.Empty;
}
