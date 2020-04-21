using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using DataLayer.Models;
using System.Net.Http;

namespace BusinessLayer.ModelFactories
{
    /// <summary>
    /// Represents the Employee factory class.
    /// </summary>
    public class EmployeeFactory : IEmployeeFactory
    {
        /// <summary>
        /// Maps the employee object retrieved from database to the employee model.
        /// </summary>
        /// <param name="employee">The employee object.</param>
        /// <returns>Returns the employee model.</returns>
        public EmployeeModel GetEmployeeModelFromEmployee(Employee employee)
        {
            return new EmployeeModel()
            {
                EmployeeId = employee.EmployeeId,
                EmployeeName = employee.EmployeeName,
                Department = employee.Department
            };
        }

        /// <summary>
        /// Gets the employee object from employee model.
        /// </summary>
        /// <param name="employee">The employee model.</param>
        /// <returns>Returns the employee object.</returns>
        public Employee GetEmployeeFromModel(EmployeeModel employee)
        {
            return new Employee()
            {
                EmployeeId = employee.EmployeeId,
                EmployeeName = employee.EmployeeName,
                Department = employee.Department
            };
        }
    }
}