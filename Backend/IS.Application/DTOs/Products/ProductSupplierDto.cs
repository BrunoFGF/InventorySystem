namespace IS.Application.DTOs.Products
{
    public class ProductSupplierDto
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
    }
}