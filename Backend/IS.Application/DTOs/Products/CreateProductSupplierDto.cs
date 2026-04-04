using System.ComponentModel.DataAnnotations;

namespace IS.Application.DTOs.Products
{
    public class CreateProductSupplierDto
    {
        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        [MaxLength(50)]
        public string BatchNumber { get; set; } = string.Empty;
    }
}