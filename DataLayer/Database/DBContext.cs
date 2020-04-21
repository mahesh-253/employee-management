using DataLayer.Interfaces;
using DataLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Database
{
    // Tests pending.
    public class DBContext : IDBContext
    {
        private List<Employee> _employees = null;

        /// <summary>
        /// Initializes the new instance of DBContext.
        /// Initializing the employees list as well as any of the actions by the user can be executed.
        /// Implemented it like this as this class behaves as a mock of the database.
        /// </summary>
        public DBContext()
        {
            if (_employees == null)
            {
                _employees = PrepareEmployeesList().ToList();
            }
        }

        /// <summary>
        /// Gets all employees list.
        /// </summary>
        /// <returns>Returns the list of all employees.</returns>
        public IEnumerable<Employee> GetEmployees()
        {
            return _employees;
        }

        /// <summary>
        /// Gets the employee details as requested from the user.
        /// </summary>
        /// <param name="id">The employee id.</param>
        /// <returns>Returns the employee.</returns>
        public Employee GetEmployeeDetails(int id)
        {
            return _employees.Find(emp => emp.EmployeeId == id);
        }

        /// <summary>
        /// Deletes the employee request.
        /// </summary>
        /// <param name="employee">The employee object to be removed.</param>
        public void DeleteEmployee(Employee employee)
        {
            if (employee != null)
            {
                _employees.Remove(employee);
            }
        }

        /// <summary>
        /// Prepares the employees list on the fly and returns the list to the caller method.
        /// This actually mocks the database operations.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Employee> PrepareEmployeesList()
        {
            List<Employee> employees = new List<Employee>();
            string[] departments = new string[] { "Production", "Finance", "R&D",
                "Purchasing", "Marketing", "Sales", "HRD", "Accounting", "Technology",
                "Infrastructure", "IT" };

            for (int i = 1; i <= 100; i++)
            {
                employees
                    .Add(new Employee()
                    {
                        EmployeeId = i,
                        EmployeeName = string.Join("", "A", i.ToString()),
                        Department = departments[i % 11]
                    });
            }

            return employees;
        }

        /// <summary>
        /// Adds an employee to the employees list.
        /// </summary>
        /// <param name="employee">The employee object.</param>
        public void AddEmployee(Employee employee)
        {
            if (employee != null)
            {
                _employees.Add(employee);
            }
        }
    }
}