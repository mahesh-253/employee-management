using BusinessLayer.Interfaces;
using BusinessLayer.ModelFactories;
using BusinessLayer.Models;
using DataLayer.Database;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Tests
{
    [TestClass]
    public class DBContextTest
    {
        [TestMethod]
        public void GetEmployeesTest()
        {
            // Arrange.
            IEmployeeFactory empFactory = new EmployeeFactory();
            IDBContext dbContext = new DBContext();

            // Act.
            List<EmployeeModel> employees = dbContext
                                            .GetEmployees()
                                            .Select(emp => empFactory.GetEmployeeModelFromEmployee(emp))
                                            .ToList();

            // Assert.
            Assert.IsNotNull(employees);
            Assert.AreEqual(100, employees.Count);
        }

        [TestMethod]
        public void GetEmployeeDetailsTest()
        {
            // Arrange.
            IDBContext dbContext = new DBContext();

            // Act.
            Employee employee = dbContext.GetEmployeeDetails(1);

            // Assert.
            Assert.IsNotNull(employee);
            Assert.IsNotNull(employee.EmployeeId);
            Assert.IsNotNull(employee.EmployeeName);
            Assert.IsNotNull(employee.Department);
        }

        [TestMethod]
        public void DeleteEmployeeTest()
        {
            // Arrange.
            IDBContext dbContext = new DBContext();
            Employee employee = dbContext.GetEmployeeDetails(1);

            // Act.
            dbContext.DeleteEmployee(employee);

            // Assert.
            List<Employee> employees = dbContext.GetEmployees().ToList();

            Assert.IsNotNull(employees);
            Assert.AreEqual(99, employees.Count);
        }

        [TestMethod]
        public void AddEmployeeTest()
        {
            // Arrange.
            IEmployeeFactory empFactory = new EmployeeFactory();
            IDBContext dbContext = new DBContext();
            EmployeeModel employee = new EmployeeModel() { EmployeeId = 200, EmployeeName = "B", Department = "IT" };

            // Act.
            dbContext.AddEmployee(empFactory.GetEmployeeFromModel(employee));

            // Assert.
            List<Employee> employees = dbContext.GetEmployees().ToList();

            Assert.IsNotNull(employees);
            Assert.AreEqual(101, employees.Count);
        }
    }
}
