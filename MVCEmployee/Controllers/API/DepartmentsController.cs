using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCEmployee.Controllers;
using MVCEmployee.Models;

namespace MVCEmployee.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentsRepository;
        public DepartmentsController(IDepartmentRepository departmentRepository)
        {
            _departmentsRepository = departmentRepository;
        }
        [HttpGet]
        public async Task<ActionResult> GetDepartments()
        {
            try
            {
                return Ok(await _departmentsRepository.GetDepartments());
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving data from database");
            }

        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            try
            {
                var result = await _departmentsRepository.GetDepartment(id);
                if (result == null)
                {
                    return NotFound($"Department with id = {id} not found");
                }
                return result;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving data from database");
            }

        }
    }
}
