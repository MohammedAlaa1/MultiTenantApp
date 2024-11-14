using Microsoft.AspNetCore.Mvc;
using MultiTenantApp.Data;
using MultiTenantApp.Models;
using MultiTenantApp.Services;

namespace MultiTenantApp.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly TenantService _tenantService;
        private readonly ApplicationDbContext _context;

        public EmployeeController(TenantService tenantService, ApplicationDbContext context)
        {
            _tenantService = tenantService;
            _context = context;
        }

        [HttpPost("create")]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {

            try
            {
                var tenantId = _tenantService.GetTenantId();
                employee.TenantId = tenantId;

                _context.Employees.Add(employee);
                _context.SaveChanges();

                return Ok(new { message = "Employee Created Successfully", data = employee });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id) {

            try
            {
                var tenantId = _tenantService.GetTenantId();

                var employee = _context.Employees.FirstOrDefault(e => e.Id == id && e.TenantId == tenantId);

                if (employee == null)
                {
                    return NotFound("Employee not found for this tenant.");
                }

                return Ok(employee);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }

        }
    }
}
