using System.ComponentModel.DataAnnotations;

namespace Empleados.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres")]
        public string Apellido { get; set; }
        
        [Required(ErrorMessage = "El email es obligatorio")]
        [StringLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; }
        
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres")]
        public string Telefono { get; set; }
        
        [Required(ErrorMessage = "El salario es obligatorio")]
        [Range(0, 999999.99, ErrorMessage = "El salario debe estar entre 0 y 999,999.99")]
        public decimal Salario { get; set; }
        
        [Required(ErrorMessage = "La fecha de ingreso es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; } = DateTime.Now;
    }
}