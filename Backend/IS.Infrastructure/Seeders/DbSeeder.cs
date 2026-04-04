using IS.Domain.Entities;
using IS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IS.Infrastructure.Seeders
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(InventoryDbContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var user = new User
            {
                FirstName = "Admin",
                LastName = "Sistema",
                Phone = "0999999999",
                Email = "admin@inventory.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123*")
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var suppliers = new List<Supplier>
            {
                new() { Name = "Proveedor A", Email = "proveedora@mail.com", Phone = "0991111111" },
                new() { Name = "Proveedor B", Email = "proveedorb@mail.com", Phone = "0992222222" },
                new() { Name = "Proveedor C", Email = "proveedorc@mail.com", Phone = "0993333333" }
            };

            await context.Suppliers.AddRangeAsync(suppliers);
            await context.SaveChangesAsync();

            var products = new List<Product>
            {
                new() { Name = "Monitor 50 pulgadas 4K", Description = "Monitor LED 4K Ultra HD", UserId = user.Id },
                new() { Name = "Equipo de Sonido 20000W", Description = "Sistema de audio profesional", UserId = user.Id }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            var productSuppliers = new List<ProductSupplier>
            {
                new() { ProductId = products[0].Id, SupplierId = suppliers[0].Id, Price = 250m, Stock = 10, BatchNumber = "L-2024-001" },
                new() { ProductId = products[0].Id, SupplierId = suppliers[1].Id, Price = 300m, Stock = 5, BatchNumber = "L-2024-002" },
                new() { ProductId = products[0].Id, SupplierId = suppliers[2].Id, Price = 200m, Stock = 15, BatchNumber = "L-2024-003" },
                new() { ProductId = products[1].Id, SupplierId = suppliers[0].Id, Price = 150m, Stock = 8, BatchNumber = "L-2024-004" },
                new() { ProductId = products[1].Id, SupplierId = suppliers[1].Id, Price = 200m, Stock = 3, BatchNumber = "L-2024-005" },
                new() { ProductId = products[1].Id, SupplierId = suppliers[2].Id, Price = 100m, Stock = 20, BatchNumber = "L-2024-006" }
            };

            await context.ProductSuppliers.AddRangeAsync(productSuppliers);
            await context.SaveChangesAsync();
        }
    }
}