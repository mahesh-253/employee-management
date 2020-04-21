using BusinessLayer.Models;
using DataLayer.Models;

namespace BusinessLayer.Interfaces
{
    public interface IEmployeeFactory
    {
        /// <summary>
        /// Maps the employee object retrieved from database to the employee model.
        /// </summary>
        /// <param name="employee">The employee object.</param>
        /// <returns>Returns the employee model.</returns>
        EmployeeModel GetEmployeeModelFromEmployee(Employee employee);

        /// <summary>
        /// Gets the employee object from employee model.
        /// </summary>
        /// <param name="employee">The employee model.</param>
        /// <returns>Returns the employee object.</returns>
        Employee GetEmployeeFromModel(EmployeeModel employee);
    }
}