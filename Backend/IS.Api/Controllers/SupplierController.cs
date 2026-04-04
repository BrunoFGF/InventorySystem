using IS.Application.DTOs.Suppliers;
using IS.Application.Interfaces;
using IS.Shared.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<SupplierDto>>>> GetAll()
        {
            var suppliers = await _supplierService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<SupplierDto>>.Ok(suppliers));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SupplierDto>>> GetById(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            return Ok(ApiResponse<SupplierDto>.Ok(supplier));
        }
    }
}