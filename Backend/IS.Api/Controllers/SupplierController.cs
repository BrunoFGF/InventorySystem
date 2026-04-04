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
    [Produces("application/json")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SupplierDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<IEnumerable<SupplierDto>>>> GetAll()
        {
            var suppliers = await _supplierService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<SupplierDto>>.Ok(suppliers));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<SupplierDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<SupplierDto>>> GetById(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            return Ok(ApiResponse<SupplierDto>.Ok(supplier));
        }
    }
}