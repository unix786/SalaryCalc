using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaryApp.Models
{
    public partial class Position
    {
        public Position()
        {
            BaseSalary = new HashSet<BaseSalary>();
            Employee = new HashSet<Employee>();
        }

        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [InverseProperty(nameof(Models.BaseSalary.Position))]
        public ICollection<BaseSalary> BaseSalary { get; set; }
        [InverseProperty(nameof(Models.Employee.Position))]
        public ICollection<Employee> Employee { get; set; }
    }
}
