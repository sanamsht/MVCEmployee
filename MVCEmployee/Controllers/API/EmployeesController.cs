using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCEmployee.Controllers;
using MVCEmployee.Models;

namespace MVCEmployee.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        [HttpGet]
        public async Task<ActionResult> GetEmployees()
        {
            try
            {
                return Ok(await _employeeRepository.GetEmployees());
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }

        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var result = await _employeeRepository.GetEmployee(id);
                if (result == null)
                {
                    return NotFound();
                }
                return result;

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }

        }
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }
                var emp = await _employeeRepository.GetEmployeeByEmail(employee.Email);
                if (emp != null)
                {
                    ModelState.AddModelError("Email", "Email already exist. Please try another one!");
                    return BadRequest(ModelState);
                }
                var createdEmployee = await _employeeRepository.AddEmployee(employee);

                return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.EmployeeId }, createdEmployee);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new employee record");
            }

        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
        {
            try
            {
                if (id != employee.EmployeeId)
                {
                    return BadRequest("Employee Id Mismatch");
                }
                var emp = await _employeeRepository.GetEmployee(id);
                if (emp == null)
                {
                    return NotFound($"Employee with id = {id} not found");
                }

                return await _employeeRepository.UpdateEmployee(employee);


            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating employee record");
            }

        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {

                var emp = await _employeeRepository.GetEmployee(id);
                if (emp == null)
                {
                    return NotFound($"Employee with id = {id} not found");
                }

                await _employeeRepository.DeleteEmployee(id);
                return Ok($"Employee with id = {id} deleted");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting employee record");
            }

        }
        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search(string? name, Gender? gender)
        {
            try
            {
                var result = await _employeeRepository.Search(name, gender);
                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound();

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving data from database");
            }

        }
    }
}
