namespace SalaryApp.Models
{
    /// <summary>
    /// Used in calculation for <see cref="Employee.Salary"/>.
    /// </summary>
    public partial class Bonus
    {
        /// <summary>Value 0 to 5.</summary>
        public byte Score { get; set; }
        /// <summary>Percentage.</summary>
        public byte Value { get; set; }
    }
}
