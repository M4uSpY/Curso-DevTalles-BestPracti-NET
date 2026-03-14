using System;
using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.DTOs;

public class CreateCategoryDTO
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(50, ErrorMessage = "El nombre no puede tener mas de 50 caracteres.")]
    [MinLength(3, ErrorMessage = "El nombre no puede tener menos de 3 caracteres.")]
    public string Name { get; set; } = string.Empty;

    public int edad { get; set; }
}
