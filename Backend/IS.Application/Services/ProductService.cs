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

            var auditLogs = new List<AuditLog>
            {
                CreateAuditLog(AppConstants.TableNames.Product, product.Id, AppConstants.FieldNames.Name, null, product.Name, AppConstants.AuditActions.Create),
                CreateAuditLog(AppConstants.TableNames.Product, product.Id, AppConstants.FieldNames.Description, null, product.Description, AppConstants.AuditActions.Create)
            };
            await _unitOfWork.AuditLogs.AddRangeAsync(auditLogs);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(product.Id);
        }

        public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _unitOfWork.Products.GetProductWithSuppliersAsync(id)
                ?? throw new NotFoundException(nameof(Product), id);

            var auditLogs = new List<AuditLog>();

            if (product.Name != dto.Name)
                auditLogs.Add(CreateAuditLog(AppConstants.TableNames.Product, id, AppConstants.FieldNames.Name, product.Name, dto.Name, AppConstants.AuditActions.Update));

            if (product.Description != dto.Description)
                auditLogs.Add(CreateAuditLog(AppConstants.TableNames.Product, id, AppConstants.FieldNames.Description, product.Description, dto.Description, AppConstants.AuditActions.Update));

            auditLogs.AddRange(BuildSupplierAuditLogs(id, product.ProductSuppliers, dto.Suppliers));

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.UpdatedAt = DateTime.Now;

            _unitOfWork.Products.RemoveProductSuppliers(product.ProductSuppliers.ToList());
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

        private static IEnumerable<AuditLog> BuildSupplierAuditLogs(int productId, IEnumerable<ProductSupplier> oldSuppliers, IEnumerable<CreateProductSupplierDto> newSuppliers)
        {
            var logs = new List<AuditLog>();
            var tableName = $"{AppConstants.TableNames.ProductSupplier}/ProductId";

            var oldMap = oldSuppliers.ToDictionary(s => s.BatchNumber);
            var newMap = newSuppliers.ToDictionary(s => s.BatchNumber);

            foreach (var (batch, oldPs) in oldMap)
            {
                if (!newMap.TryGetValue(batch, out var newPs))
                {
                    logs.Add(CreateAuditLog(tableName, productId, AppConstants.FieldNames.BatchNumber, batch, null, AppConstants.AuditActions.Delete));
                    continue;
                }

                if (oldPs.SupplierId != newPs.SupplierId)
                    logs.Add(CreateAuditLog(tableName, productId, AppConstants.FieldNames.SupplierId, oldPs.SupplierId.ToString(), newPs.SupplierId.ToString(), AppConstants.AuditActions.Update));

                if (oldPs.Price != newPs.Price)
                    logs.Add(CreateAuditLog(tableName, productId, AppConstants.FieldNames.Price, oldPs.Price.ToString(), newPs.Price.ToString(), AppConstants.AuditActions.Update));

                if (oldPs.Stock != newPs.Stock)
                    logs.Add(CreateAuditLog(tableName, productId, AppConstants.FieldNames.Stock, oldPs.Stock.ToString(), newPs.Stock.ToString(), AppConstants.AuditActions.Update));
            }

            foreach (var (batch, newPs) in newMap)
            {
                if (!oldMap.ContainsKey(batch))
                    logs.Add(CreateAuditLog(tableName, productId, AppConstants.FieldNames.BatchNumber, null, batch, AppConstants.AuditActions.Create));
            }

            return logs;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Product), id);

            product.IsDeleted = true;
            product.DeletedAt = DateTime.Now;

            var auditLog = CreateAuditLog(AppConstants.TableNames.Product, id, AppConstants.FieldNames.IsDeleted, "false", "true", AppConstants.AuditActions.Delete);
            await _unitOfWork.AuditLogs.AddAsync(auditLog);

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();
        }

        private static AuditLog CreateAuditLog(string table, int recordId, string field, string? oldValue, string? newValue, string action)
        {
            return new AuditLog
            {
                TableName = table,
                RecordId = recordId,
                FieldName = field,
                OldValue = oldValue,
                NewValue = newValue,
                Action = action
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