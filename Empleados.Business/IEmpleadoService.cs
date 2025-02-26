using Empleados.Models;

namespace Empleados.Business.Services
{
    public interface IEmpleadoService
    {
        Task<IEnumerable<Empleado>> GetAllEmpleadosAsync();
        Task<Empleado> GetEmpleadoByIdAsync(int id);
        Task<Empleado> CreateEmpleadoAsync(Empleado empleado);
        Task<Empleado> UpdateEmpleadoAsync(int id, Empleado empleado);
        Task<bool> DeleteEmpleadoAsync(int id);
    }
}