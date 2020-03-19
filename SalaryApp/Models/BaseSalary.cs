using System.ComponentModel.DataAnnotations.Schema;

namespace SalaryApp.Models
{
    public partial class BaseSalary
    {
        [Column("PositionID")]
        public int PositionId { get; set; }
        public byte Years { get; set; }
        public int Value { get; set; }

        [ForeignKey(nameof(PositionId))]
        [InverseProperty(nameof(Models.Position.BaseSalary))]
        public virtual Position Position { get; set; }
    }
}
