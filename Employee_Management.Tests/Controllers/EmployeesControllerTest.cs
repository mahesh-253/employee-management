using BusinessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.ModelFactories;
using BusinessLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using Employee_Management.Controllers;
using Moq;
using System.Collections.Generic;
using System;

namespace Employee_Management.Tests.Controllers
{
    [TestClass]
    public class EmployeesControllerTest
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

        // Get employees tests.
        [TestMethod]
        public void GetEmployeesTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get,
                "http://fakehost/api/Employees");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Get();

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsNotNull(response.Content);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void GetEmployeesExceptionTests()
        {
            // Arrange.
            var empInfo = new Mock<IEmployeeInfoReceiver>();
            empInfo.Setup(task => task.GetAllEmployees()).Throws(new System.Exception("An error occurred"));

            EmployeesController controller = new EmployeesController(empInfo.Object);
            HttpConfiguration configuration = new HttpConfiguration();
            
            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get,
                "http://fakehost/api/Employees");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Get();

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void GetEmployeesWithZeroCountTests()
        {
            // Arrange.
            var empInfo = new Mock<IEmployeeInfoReceiver>();
            empInfo.Setup(task => task.GetAllEmployees()).Returns(new List<EmployeeModel>());

            EmployeesController controller = new EmployeesController(empInfo.Object);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get,
                "http://fakehost/api/Employees");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Get();

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void GetEmployeesInternalServerErrorTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get,
                "http://fakehost/api/Employees?expecting500=true");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Get();

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void GetEmployeesBadRequestErrorTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get,
                "http://fakehost/api/Employees");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "text/csv");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Get();

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        // Get Employee details tests.
        [TestMethod]
        public void GetEmployeeDetailsNotFoundErrorTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get,
                "http://fakehost/api/Employees/1000");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Get(1000);

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void GetEmployeeDetailsBadRequestErrorWithZeroEmployeeIdTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get,
                "http://fakehost/api/Employees/0");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Get(0);

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void GetEmployeeDetailsBadRequestErrorWithNonAllowedAcceptHeadersTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get,
                "http://fakehost/api/Employees/100");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "text/csv");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Get(100);

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void GetEmployeeDetailsInternalServerErrorTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get,
                "http://fakehost/api/Employees/10?expecting500=true");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Get(10);

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void GetEmployeeDetailsSuccessTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get,
                "http://fakehost/api/Employees/10");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Get(10);

            EmployeeModel employee = response.Content.ReadAsAsync<EmployeeModel>().Result;

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsNotNull(response.Content);
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsNotNull(employee);
        }

        [TestMethod]
        public void GetEmployeeDetailsExceptionTest()
        {
            // Arrange.
            int employeeId = 10;
            var empInfo = new Mock<IEmployeeInfoReceiver>();
            empInfo.Setup(task => task.GetEmployeeDetails(employeeId)).Throws(new Exception("An error occurred"));

            EmployeesController controller = new EmployeesController(empInfo.Object);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get,
                "http://fakehost/api/Employees/10");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Get(employeeId);

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        // Delete method tests.
        [TestMethod]
        public void ReturnNotFoundErrorDeleteTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Delete,
                "http://fakehost/api/Employees/1000");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Delete(1000);

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ReturnInternalServerErrorDeleteTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Delete,
                "http://fakehost/api/Employees/1000?expecting500=true");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Delete(1000);

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ReturnExceptionDeleteTest()
        {
            // Arrange.
            int employeeId = 10;
            var empInfo = new Mock<IEmployeeInfoReceiver>();
            empInfo.Setup(task => task.GetEmployeeDetails(employeeId)).Throws(new Exception("An error occurred"));

            EmployeesController controller = new EmployeesController(empInfo.Object);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Delete,
                "http://fakehost/api/Employees");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Delete(employeeId);

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ReturnBadRequestErrorDeleteTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Delete,
                "http://fakehost/api/Employees/0");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Delete(0);

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void EmployeeDeleteTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            int employeeIdToBeDeleted = 10;

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Delete,
                $"http://fakehost/api/Employees/{employeeIdToBeDeleted}");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Delete(employeeIdToBeDeleted);

            string responseMessage = response.Content.ReadAsStringAsync().Result;

            // Assert.
            Assert.AreEqual(responseMessage,
                $"\"The user with user id {employeeIdToBeDeleted} has been deleted successfully\"");
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NoContent);
            Assert.IsNotNull(response.Content);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void DeleteEmployeeDetailsBadRequestErrorWithNonAllowedAcceptHeadersTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Delete,
                "http://fakehost/api/Employees/100");

            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "text/csv");
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Delete(100);

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        // POST method tests.
        [TestMethod]
        public void AddEmployeeHappyPathTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post,
                "http://fakehost/api/Employees");

            string content = "{\"EmployeeId\": 121,\"EmployeeName\": \"B\",\"Department\": \"IT\"}";
            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");

            req.Content = new StringContent(content);
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Post(JsonConvert.DeserializeObject<EmployeeModel>(content));

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.Created);
            Assert.IsNotNull(response.Content);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void AddEmployeeExceptionTest()
        {
            var empInfo = new Mock<IEmployeeInfoReceiver>();
            empInfo.Setup(task => task.AddEmployee(It.IsAny<EmployeeModel>()))
                .Throws(new Exception("An error occurred"));

            EmployeesController controller = new EmployeesController(empInfo.Object);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post,
                "http://fakehost/api/Employees");

            string content = "{\"EmployeeId\": 121,\"EmployeeName\": \"B\",\"Department\": \"IT\"}";
            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");

            req.Content = new StringContent(content);
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Post(JsonConvert.DeserializeObject<EmployeeModel>(content));

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void AddEmployeeBadRequestTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post,
                "http://fakehost/api/Employees");

            string content = "{\"EmployeeId\": 121,\"EmployeeName\": \"B\",\"Department\": \"IT\"}";
            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");

            req.Content = new StringContent(content);
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Post(null);

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void AddEmployeeBadRequestWithAllowableAcceptHeadersTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post,
                "http://fakehost/api/Employees");

            string content = "{\"EmployeeId\": 121,\"EmployeeName\": \"B\",\"Department\": \"IT\"}";
            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "text/csv");

            req.Content = new StringContent(content);
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Post(JsonConvert.DeserializeObject<EmployeeModel>(content));

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void AddEmployeeInternalServerErrorTest()
        {
            EmployeesController controller = new EmployeesController(EmployeeInfo);
            HttpConfiguration configuration = new HttpConfiguration();

            // Prepare request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post,
                "http://fakehost/api/Employees?expecting500=true");

            string content = "{\"EmployeeId\": 121,\"EmployeeName\": \"B\",\"Department\": \"IT\"}";
            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = configuration;
            req.Headers.Add("Accept", "application/json");

            req.Content = new StringContent(content);
            controller.Request = req;

            // Invoke the URI.
            HttpResponseMessage response = controller.Post(JsonConvert.DeserializeObject<EmployeeModel>(content));

            // Assert.
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            Assert.IsNotNull(response.Content);
            Assert.IsFalse(response.IsSuccessStatusCode);
        }
    }
}
