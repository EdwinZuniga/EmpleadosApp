using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Empleados.Models;
using Microsoft.EntityFrameworkCore;

namespace Empleados.DataAccess.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly ApplicationDbContext _context;

        public EmpleadoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Empleado>> GetAllAsync()
        {
            return await _context.Empleados.ToListAsync();
        }

        public async Task<PaginatedResult<Empleado>> GetPagedAsync(int pageIndex, int pageSize, string searchTerm = null)
        {
            // Asegurarse de que pageIndex es al menos 1
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            
            var query = _context.Empleados.AsQueryable();
            
            // Aplicar búsqueda si hay un término de búsqueda
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(e => 
                    e.Nombre.ToLower().Contains(searchTerm) || 
                    e.Apellido.ToLower().Contains(searchTerm) || 
                    e.Email.ToLower().Contains(searchTerm));
            }
            
            // Obtener el número total de elementos
            var totalCount = await query.CountAsync();
            
            // Aplicar paginación
            var items = await query
                .OrderBy(e => e.Apellido)
                .ThenBy(e => e.Nombre)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            // Crear y devolver el resultado paginado
            return new PaginatedResult<Empleado>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        // Los otros métodos permanecen igual
        public async Task<Empleado> GetByIdAsync(int id)
        {
            return await _context.Empleados.FindAsync(id);
        }

        public async Task<Empleado> CreateAsync(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();
            return empleado;
        }

        public async Task<Empleado> UpdateAsync(Empleado empleado)
        {
            _context.Entry(empleado).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return empleado;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
                return false;

            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExisteEmailAsync(string email, int idExcluir = 0)
        {
            return await _context.Empleados
                .AnyAsync(e => e.Email == email && e.Id != idExcluir);
        }
    }
}