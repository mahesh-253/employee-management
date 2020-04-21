using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using DataLayer.Database;
using DataLayer.Interfaces;
using DataLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer
{
    public class EmployeeInfoReceiver : IEmployeeInfoReceiver
    {
        private IDBContext _dbContext = null;
        private IEmployeeFactory _empFactory;

        /// <summary>
        /// Gets the db context.
        /// </summary>
        public IDBContext DBContext
        {
            get
            {
                if (_dbContext == null)
                {
                    _dbContext = new DBContext();
                }
                return _dbContext;
            }
        }

        /// <summary>
        /// Initializes a new instance of the employee repository.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        /// <param name="empFactory">The employee factory.</param>
        public EmployeeInfoReceiver(IEmployeeFactory empFactory)
        {
            _empFactory = empFactory;
        }

        /// <summary>
        /// Gets all employees.
        /// </summary>
        /// <returns>Returns the list of employees.</returns>
        public IEnumerable<EmployeeModel> GetAllEmployees()
        {
            return DBContext
                    .GetEmployees()
                    .Select(emp => _empFactory.GetEmployeeModelFromEmployee(emp));
        }

        /// <summary>
        /// Gets the requested employee details.
        /// </summary>
        /// <param name="id">The employee id.</param>
        /// <returns>Returns employee object.</returns>
        public EmployeeModel GetEmployeeDetails(int id)
        {
            if (id == 0)
            {
                return null;
            }

            Employee emp = DBContext.GetEmployeeDetails(id);
            if (emp != null)
            {
                return _empFactory.GetEmployeeModelFromEmployee(emp);
            }

            return null;
        }

        /// <summary>
        /// Deletes the employee with the given id.
        /// </summary>
        /// <param name="id">The employee id.</param>
        public void DeleteEmployee(int id)
        {
            // Get the requested employee details.
            Employee employee = DBContext
                                .GetEmployees()
                                .ToList()
                                .Find(emp => emp.EmployeeId == id);

            if (employee != null)
            {
                // Delete employee using db context.
                DBContext.DeleteEmployee(employee);
            }
        }

        /// <summary>
        /// Adds the employee to the employees list.
        /// </summary>
        /// <param name="employee">The employee object.</param>
        public void AddEmployee(EmployeeModel employee)
        {
            Employee emp = _empFactory.GetEmployeeFromModel(employee);
            DBContext.AddEmployee(emp);
        }
    }
}
