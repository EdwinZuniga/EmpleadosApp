using System.Collections.Generic;
using System.Threading.Tasks;
using Empleados.Models;

namespace Empleados.DataAccess.Repositories
{
    public interface IEmpleadoRepository
    {
        Task<IEnumerable<Empleado>> GetAllAsync();
        Task<Empleado> GetByIdAsync(int id);
        Task<Empleado> CreateAsync(Empleado empleado);
        Task<Empleado> UpdateAsync(Empleado empleado);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExisteEmailAsync(string email, int idExcluir = 0);
    }
}