namespace DataLayer.Models
{
    /// <summary>
    /// Represents the employee class.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Gets or sents the EmployeeId.
        /// </summary>
        public int EmployeeId { get; set; }
        /// <summary>
        /// Gets or sents the EmployeeName.
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// Gets or sets the Department.
        /// </summary>
        public string Department { get; set; }
    }
}