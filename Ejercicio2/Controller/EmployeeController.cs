using Ejercicio2.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace Ejercicio2.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        //Inyectamos las dependencias
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ApplicationDbContext context, ILogger<EmployeeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        [Route("Agregar")]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            //Intentaremos agregar el nuevo registro a la base
            try
            {
                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex) {
                //Este mensaje de error es diferente para identificar que hubo un error en la conexión
                string BDerror = $"Ocurrió un error al tratar de agregar el registro.";
                _logger.LogError(ex, BDerror);
                return StatusCode(500, new { message = BDerror });
            }
        }

        [HttpGet]
        [Route("Lista de Empleados")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
            try
            {
                var employees = _context.Employees.ToList();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                //Este mensaje de error es diferente para identificar que hubo un error en la conexión
                string BDerror = $"Ocurrió un error al tratar de agregar el registro.";
                _logger.LogError(ex, BDerror);
                return StatusCode(500, new { message = BDerror });
            }
        }

        [HttpGet]
        [Route("Buscar Empleado")]
        public async Task<IActionResult> GetEmployeebyId(int id)
        {
            try
            {
                Employee employee = await _context.Employees.FindAsync(id);
                // Validamos que se haya encontrado el empleado
                if (employee == null)
                {
                    //Este error saldrá cuando no se haya encontrado el empleado
                    string error = $"Empleado con ID: {id} noencontrado.";
                    _logger.LogWarning(error);
                    return NotFound(new { message = error });
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                //Este mensaje de error es diferente para identificar que hubo un error en la conexión
                string BDerror = $"Ocurrió un error al tratar de conectarse a la base.";
                _logger.LogError(ex, BDerror);
                return StatusCode(500, new { message = BDerror });
            }
        }

        [HttpPut]
        [Route("Modificar Empleado")]
        public async Task<IActionResult> EditEmployee(int id, Employee employee)
        {
            try
            {
                var actualemployee = await _context.Employees.FindAsync(id);
                //Validamos que se haya encontrado el empleado para poder modificarlo
                if (actualemployee == null)
                {
                    //Este error saldrá cuando no se haya encontrado el empleado
                    string error = $"Empleado con ID: {id} noencontrado.";
                    _logger.LogWarning(error);
                    return NotFound(new { message = error });
                }
                actualemployee.FirstName = employee.FirstName;
                actualemployee.LastName = employee.LastName;
                actualemployee.Salary = employee.Salary;

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                //Este mensaje de error es diferente para identificar que hubo un error en la conexión
                string BDerror = $"Ocurrió un error al tratar de conectarse a la base.";
                _logger.LogError(ex, BDerror);
                return StatusCode(500, new { message = BDerror });
            }
        }

        [HttpDelete]
        [Route("Borrar Empleado")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var delete = await _context.Employees.FindAsync(id);
                //Validamos que se haya encontrado el empleado para poder borrarlo
                if (delete == null)
                {
                    //Este error saldrá cuando no se haya encontrado el empleado
                    string error = $"Empleado con ID: {id} noencontrado.";
                    _logger.LogWarning(error);
                    return NotFound(new { message = error });
                }
                _context.Employees.Remove(delete);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                //Este mensaje de error es diferente para identificar que hubo un error en la conexión
                string BDerror = $"Ocurrió un error al tratar de conectarse a la base.";
                _logger.LogError(ex, BDerror);
                return StatusCode(500, new { message = BDerror });
            }
        }
    }
}
