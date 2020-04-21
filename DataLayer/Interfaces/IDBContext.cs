using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface IDBContext
    {
        /// <summary>
        /// Gets all employees list.
        /// </summary>
        /// <returns>Returns the list of all employees.</returns>
        IEnumerable<Employee> GetEmployees();

        /// <summary>
        /// Gets the employee details as requested from the user.
        /// </summary>
        /// <param name="id">The employee id.</param>
        /// <returns>Returns the employee.</returns>
        Employee GetEmployeeDetails(int id);

        /// <summary>
        /// Deletes the employee request.
        /// </summary>
        /// <param name="employee">The employee object to be removed.</param>
        void DeleteEmployee(Employee employee);

        /// <summary>
        /// Adds an employee to the employees list.
        /// </summary>
        /// <param name="employee">The employee object.</param>
        void AddEmployee(Employee employee);
    }
}