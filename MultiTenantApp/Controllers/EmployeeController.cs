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
        public IActionResult CreateEmployee([FromBody] EmployeeDTO employeeDTO)
        {

            try
            {
                var employee = new Employee();
                employee.EmpName = employeeDTO.Name;
                //var tenantId = _tenantService.GetTenantId();
               // employeeDTO.TenantId = tenantId;
               //employee.TenantId = tenantId;

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
                //var tenantId = _tenantService.GetTenantId();

                var employee = _context.Employees.FirstOrDefault(e => e.Id == id);

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
