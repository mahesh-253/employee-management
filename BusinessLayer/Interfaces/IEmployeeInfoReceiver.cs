using BusinessLayer.Models;
using System.Collections.Generic;

namespace BusinessLayer.Interfaces
{
    public interface IEmployeeInfoReceiver
    {
        /// <summary>
        /// Gets all employees.
        /// </summary>
        /// <returns>Returns the list of employees.</returns>
        IEnumerable<EmployeeModel> GetAllEmployees();

        /// <summary>
        /// Gets the requested employee details.
        /// </summary>
        /// <param name="id">The employee id.</param>
        /// <returns>Returns employee object.</returns>
        EmployeeModel GetEmployeeDetails(int id);

        /// <summary>
        /// Deletes the employee with the given id.
        /// </summary>
        /// <param name="id">The employee id.</param>
        void DeleteEmployee(int id);

        /// <summary>
        /// Adds the employee to the employees list.
        /// </summary>
        /// <param name="employee">The employee object.</param>
        void AddEmployee(EmployeeModel employee);
    }
}
