using BusinessLayer.Interfaces;
using BusinessLayer.ModelFactories;
using BusinessLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace BusinessLayer.Tests
{
    [TestClass]
    public class EmployeeInfoReceiverTests
    {
        private IEmployeeInfoReceiver _employeeInfo;
        private IEmployeeInfoReceiver EmployeeInfo
        {
            get
            {
                if (_employeeInfo == null)
                {
                    _employeeInfo = new EmployeeInfoReceiver(new EmployeeFactory());
                }

                return _employeeInfo;
            }
        }

        [TestMethod]
        public void GetAllEmployeesTest()
        {
            // Act.
            var employees = EmployeeInfo.GetAllEmployees();

            // Assert.
            Assert.IsNotNull(employees);
            Assert.AreEqual(100, employees.ToList().Count());
        }

        [TestMethod]
        public void GetEmployeeDetailsTest()
        {
            // Act.
            var employee = EmployeeInfo.GetEmployeeDetails(1);

            // Assert.
            Assert.IsNotNull(employee);
            Assert.IsNotNull(employee.EmployeeId);
            Assert.IsNotNull(employee.EmployeeName);
            Assert.IsNotNull(employee.Department);
        }

        [TestMethod]
        public void GetEmployeeDetailsNullTest()
        {
            // Act.
            var employee = EmployeeInfo.GetEmployeeDetails(0);

            // Assert.
            Assert.IsNull(employee);
        }

        [TestMethod]
        public void AddEmployeeTest()
        {
            // Arrange.
            EmployeeModel empModel = new EmployeeModel() { EmployeeId = 200, EmployeeName = "B", Department = "IT" };

            // Act.
            EmployeeInfo.AddEmployee(empModel);

            // Assert.
            var employees = EmployeeInfo.GetAllEmployees();
            Assert.IsNotNull(employees);
            Assert.AreEqual(101, employees.Count());
        }

        [TestMethod]
        public void DeleteEmployeeTest()
        {
            // Act.
            EmployeeInfo.DeleteEmployee(1);

            // Assert.
            var employee = EmployeeInfo
                            .GetAllEmployees()
                            .ToList()
                            .Find(emp => emp.EmployeeId == 1);

            Assert.IsNull(employee);
        }
    }
}
