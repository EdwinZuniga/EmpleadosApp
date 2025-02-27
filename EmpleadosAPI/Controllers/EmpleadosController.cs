using Empleados.Business.Services;
using Empleados.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmpleadosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmpleadosController : ControllerBase
    {
        private readonly IEmpleadoService _empleadoService;
        private readonly ILogger<EmpleadosController> _logger;

        public EmpleadosController(IEmpleadoService empleadoService, ILogger<EmpleadosController> logger)
        {
            _empleadoService = empleadoService;
            _logger = logger;
        }

        // GET: api/Empleados
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
        {
            try
            {
                var empleados = await _empleadoService.GetAllEmpleadosAsync();
                return Ok(empleados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los empleados");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al procesar la solicitud");
            }
        }

        // GET: api/Empleados/2
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Empleado>> GetEmpleado(int id)
        {
            try
            {
                var empleado = await _empleadoService.GetEmpleadoByIdAsync(id);
                return Ok(empleado);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el empleado con ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al procesar la solicitud");
            }
        }

        // POST: api/Empleados
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Empleado>> PostEmpleado(Empleado empleado)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var nuevoEmpleado = await _empleadoService.CreateEmpleadoAsync(empleado);
                return CreatedAtAction(nameof(GetEmpleado), new { id = nuevoEmpleado.Id }, nuevoEmpleado);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo empleado");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al procesar la solicitud");
            }
        }

        // PUT: api/Empleados/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutEmpleado(int id, Empleado empleado)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != empleado.Id)
                {
                    return BadRequest("ID de empleado no coincide con el ID de la ruta");
                }

                var empleadoActualizado = await _empleadoService.UpdateEmpleadoAsync(id, empleado);
                return Ok(empleadoActualizado);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el empleado con ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al procesar la solicitud");
            }
        }

        // DELETE: api/Empleados/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            try
            {
                await _empleadoService.DeleteEmpleadoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar el empleado con ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al procesar la solicitud");
            }
        }
    }
}