using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaryApp.Models
{
    public partial class Employee
    {
        public Employee()
        {
            InverseManager = new HashSet<Employee>();
        }

        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public short Employed { get; set; }
        [Required, Column("PositionID")]
        public int PositionId { get; set; }
        [Column("ManagerID")]
        public int? ManagerId { get; set; }
        public byte Rating { get; set; }
        public byte YearsEmployed { get; set; }
        public byte PrevRating1 { get; set; }
        public byte PrevRating2 { get; set; }
        /// <summary>
        /// Gets recalculated when updating relevant fields. Calculation is performed in "Salary" view.
        /// </summary>
        public int? Salary { get; set; }

        [ForeignKey(nameof(ManagerId))]
        [InverseProperty(nameof(InverseManager))]
        public virtual Employee Manager { get; set; }
        [ForeignKey(nameof(PositionId))]
        [InverseProperty(nameof(Models.Position.Employee))]
        public virtual Position Position { get; set; }
        [InverseProperty(nameof(Manager))]
        public ICollection<Employee> InverseManager { get; set; }
    }
}
