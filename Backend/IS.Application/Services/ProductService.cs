using IS.Application.DTOs.Products;
using IS.Application.Interfaces;
using IS.Domain.Entities;
using IS.Domain.Exceptions;
using IS.Domain.Interfaces;
using IS.Shared.Constants;

namespace IS.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductDto>> GetAllByUserAsync(int userId)
        {
            var products = await _unitOfWork.Products.GetActiveProductsByUserAsync(userId);
            return products.Select(MapToDto);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetProductWithSuppliersAsync(id)
                ?? throw new NotFoundException(nameof(Product), id);

            return MapToDto(product);
        }

        public async Task<ProductDto> CreateAsync(int userId, CreateProductDto dto)
        {
            var product = new Product
            {
                UserId = userId,
                Name = dto.Name,
                Description = dto.Description,
                ProductSuppliers = dto.Suppliers.Select(s => new ProductSupplier
                {
                    SupplierId = s.SupplierId,
                    Price = s.Price,
                    Stock = s.Stock,
                    BatchNumber = s.BatchNumber
                }).ToList()
            };

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(product.Id);
        }

        public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _unitOfWork.Products.GetProductWithSuppliersAsync(id)
                ?? throw new NotFoundException(nameof(Product), id);

            var auditLogs = new List<AuditLog>();

            if (product.Name != dto.Name)
                auditLogs.Add(CreateAuditLog(AppConstants.TableNames.Product, id, AppConstants.FieldNames.Name, product.Name, dto.Name));

            if (product.Description != dto.Description)
                auditLogs.Add(CreateAuditLog(AppConstants.TableNames.Product, id, AppConstants.FieldNames.Description, product.Description, dto.Description));

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.UpdatedAt = DateTime.Now;

            product.ProductSuppliers.Clear();
            product.ProductSuppliers = dto.Suppliers.Select(s => new ProductSupplier
            {
                SupplierId = s.SupplierId,
                Price = s.Price,
                Stock = s.Stock,
                BatchNumber = s.BatchNumber
            }).ToList();

            if (auditLogs.Any())
                await _unitOfWork.AuditLogs.AddRangeAsync(auditLogs);

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Product), id);

            product.IsDeleted = true;
            product.DeletedAt = DateTime.Now;

            var auditLog = CreateAuditLog(AppConstants.TableNames.Product, id, AppConstants.FieldNames.IsDeleted, "false", "true");
            await _unitOfWork.AuditLogs.AddAsync(auditLog);

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();
        }

        private static AuditLog CreateAuditLog(string table, int recordId, string field, string? oldValue, string? newValue)
        {
            return new AuditLog
            {
                TableName = table,
                RecordId = recordId,
                FieldName = field,
                OldValue = oldValue,
                NewValue = newValue,
                Action = AppConstants.AuditActions.Update
            };
        }

        private static ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                Suppliers = product.ProductSuppliers.Select(ps => new ProductSupplierDto
                {
                    Id = ps.Id,
                    SupplierId = ps.SupplierId,
                    SupplierName = ps.Supplier.Name,
                    Price = ps.Price,
                    Stock = ps.Stock,
                    BatchNumber = ps.BatchNumber
                }).ToList()
            };
        }
    }
}