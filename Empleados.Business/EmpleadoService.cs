using Empleados.DataAccess.Repositories;
using Empleados.Models;

namespace Empleados.Business.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepository _empleadoRepository;

        public EmpleadoService(IEmpleadoRepository empleadoRepository)
        {
            _empleadoRepository = empleadoRepository;
        }

        public async Task<IEnumerable<Empleado>> GetAllEmpleadosAsync()
        {
            return await _empleadoRepository.GetAllAsync();
        }

        public async Task<Empleado> GetEmpleadoByIdAsync(int id)
        {
            var empleado = await _empleadoRepository.GetByIdAsync(id);
            if (empleado == null)
                throw new KeyNotFoundException($"No se encontró empleado con ID {id}");
            
            return empleado;
        }

        public async Task<Empleado> CreateEmpleadoAsync(Empleado empleado)
        {
            // Validar email único
            if (await _empleadoRepository.ExisteEmailAsync(empleado.Email))
                throw new InvalidOperationException($"Ya existe un empleado con el email {empleado.Email}");
            
            return await _empleadoRepository.CreateAsync(empleado);
        }

        public async Task<Empleado> UpdateEmpleadoAsync(int id, Empleado empleado)
        {
            // Verificar que existe
            var existeEmpleado = await _empleadoRepository.GetByIdAsync(id);
            if (existeEmpleado == null)
                throw new KeyNotFoundException($"No se encontró empleado con ID {id}");
            
            // Validar email único
            if (await _empleadoRepository.ExisteEmailAsync(empleado.Email, id))
                throw new InvalidOperationException($"Ya existe otro empleado con el email {empleado.Email}");
            
            empleado.Id = id;
            return await _empleadoRepository.UpdateAsync(empleado);
        }

        public async Task<bool> DeleteEmpleadoAsync(int id)
        {
            var existeEmpleado = await _empleadoRepository.GetByIdAsync(id);
            if (existeEmpleado == null)
                throw new KeyNotFoundException($"No se encontró empleado con ID {id}");
            
            return await _empleadoRepository.DeleteAsync(id);
        }
    }
}