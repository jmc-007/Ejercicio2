using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ejercicio2.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required."),StringLength(maximumLength: 50)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required."),StringLength(maximumLength: 50)]
        public string? LastName { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Salario debe ser mayor a 0.")]
        public decimal Salary { get; set; }
    }
}
