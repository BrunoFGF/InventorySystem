using System.ComponentModel.DataAnnotations;

namespace IS.Application.DTOs.Products
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(300, ErrorMessage = "La descripción no puede exceder 300 caracteres.")]
        public string Description { get; set; } = string.Empty;

        public List<CreateProductSupplierDto> Suppliers { get; set; } = new();
    }
}