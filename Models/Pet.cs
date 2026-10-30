using System.ComponentModel.DataAnnotations;

namespace LHPets.Models;

public class Pet
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do pet é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A espécie do pet é obrigatória.")]
    public string Especie { get; set; } = string.Empty;

    [Required(ErrorMessage = "A raça do pet é obrigatória.")]
    public string Raca { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Informe um ClienteId válido.")]
    public int ClienteId { get; set; }
}
