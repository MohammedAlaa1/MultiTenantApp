using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Data;
using MultiTenantApp.Models;
using MultiTenantApp.Services;

namespace MultiTenantApp.Controllers
{
    [Route("api/department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly TenantService _tenantService;
        private readonly ApplicationDbContext _context;

        public DepartmentController(TenantService tenantService, ApplicationDbContext context)
        {
            _tenantService = tenantService;
            _context = context;
        }

        [HttpPost("create")]
        public IActionResult CreateDepartment([FromBody] DepartmentDTO departmantDTO) {

            try
            {
                var Department = new Department();
                Department.DepName = departmantDTO.DepName;
                //var tenantId = _tenantService.GetTenantId();
                //Department.TenantId = tenantId;

                _context.Departments.Add(Department);
                _context.SaveChanges();

                return Ok(new { message = "Department Created Successfully", data = Department });
            }catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            } 
        }

        [HttpGet("{id}")]
        public IActionResult GetDepartmentById(int id)
        {

            try
            {
                var tenantId = _tenantService.GetTenantId();

                var department = _context.Departments.FirstOrDefault(e => e.Id == id);

                if (department == null)
                {
                    return NotFound("Employee not found for this tenant.");
                }

                return Ok(department);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }

        }
    }
}
