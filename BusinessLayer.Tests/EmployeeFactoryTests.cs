using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLayer.ModelFactories;
using BusinessLayer.Models;
using DataLayer.Models;

namespace BusinessLayer.Tests
{
    [TestClass]
    public class EmployeeFactoryTests
    {
        [TestMethod]
        public void GetEmployeeModelFromEmployeeTest()
        {
            // Arrange.
            Employee emp = new Employee()
            {
                EmployeeId = 1,
                EmployeeName = "A",
                Department = "R&D"
            };

            // Act.
            EmployeeFactory empFactory = new EmployeeFactory();
            var empModel = empFactory.GetEmployeeModelFromEmployee(emp);

            // Assert.
            Assert.IsNotNull(empModel);
            Assert.IsInstanceOfType(empModel, typeof(EmployeeModel));
            Assert.AreEqual(emp.EmployeeId, empModel.EmployeeId);
            Assert.AreEqual(emp.EmployeeName, empModel.EmployeeName);
            Assert.AreEqual(emp.Department, empModel.Department);
        }

        [TestMethod]
        public void GetEmployeeFromEmployeeModelTest()
        {
            // Arrage
            EmployeeModel empModel = new EmployeeModel()
            {
                EmployeeId = 1,
                EmployeeName = "A",
                Department = "R&D"
            };

            // Act.
            EmployeeFactory empFactory = new EmployeeFactory();
            var emp = empFactory.GetEmployeeFromModel(empModel);

            // Assert.
            Assert.IsNotNull(emp);
            Assert.IsInstanceOfType(emp, typeof(Employee));
            Assert.AreEqual(empModel.EmployeeId, emp.EmployeeId);
            Assert.AreEqual(empModel.EmployeeName, emp.EmployeeName);
            Assert.AreEqual(empModel.Department, emp.Department);
        }
    }
}
