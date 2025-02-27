using System.Collections.Generic;
using System.Threading.Tasks;
using Empleados.Models;

namespace Empleados.Business.Services
{
    public interface IEmpleadoService
    {
        Task<IEnumerable<Empleado>> GetAllEmpleadosAsync();
        Task<PaginatedResult<Empleado>> GetPagedEmpleadosAsync(int pageIndex, int pageSize, string searchTerm = null);
        Task<Empleado> GetEmpleadoByIdAsync(int id);
        Task<Empleado> CreateEmpleadoAsync(Empleado empleado);
        Task<Empleado> UpdateEmpleadoAsync(int id, Empleado empleado);
        Task<bool> DeleteEmpleadoAsync(int id);
    }
}