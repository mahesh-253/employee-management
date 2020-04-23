using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Employee_Management.Controllers
{
    /// <summary>
    /// Represents the Employee Controller class.
    /// </summary>
    [Authorize]
    public class EmployeesController : ApiController
    {
        private IEmployeeInfoReceiver _employeesInfo;
        private const string EXPECTING_500 = "expecting500";

        /// <summary>
        /// Instantiates the Employees Controller class.
        /// </summary>
        /// <param name="employeesInfo">The employees information receiver object.</param>
        public EmployeesController(IEmployeeInfoReceiver employeesInfo)
        {
            _employeesInfo = employeesInfo;
        }

        /// <summary>
        /// Gets all employees list from the model.
        /// </summary>
        /// <returns>List of employees.</returns>
        [HttpGet]
        public HttpResponseMessage Get()
        {
            if (IsRequestExpecting500(Request))
            {
                return ReturnInternalServerError(Constants.InternalServerErrorMessage);
            }

            if (!(IsAllowableAcceptHeader(Request)))
            {
                return ReturnBadRequest(Constants.BadRequestErrorMessage);
            }

            try
            {
                List<EmployeeModel> employees = _employeesInfo
                                                .GetAllEmployees()
                                                .ToList();

                if (employees.Count() == 0)
                {
                    return ReturnInternalServerError(Constants.InternalServerErrorWhileRetrievingEmployees);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, employees);
                }
            }
            catch (Exception ex)
            {
                return ReturnInternalServerError(ex.Message);
            }
        }

        /// <summary>
        /// Gets the employee details.
        /// </summary>
        /// <param name="id">The employee id.</param>
        /// <returns>The employee details.</returns>
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            if (id <= 0)
            {
                return ReturnBadRequest(Constants.IdCannotBeZeroErrorMessage);
            }

            if (IsRequestExpecting500(Request))
            {
                return ReturnInternalServerError(Constants.InternalServerErrorMessage);
            }

            if (!(IsAllowableAcceptHeader(Request)))
            {
                return ReturnBadRequest(Constants.BadRequestErrorMessage);
            }

            try
            {
                EmployeeModel employee = _employeesInfo.GetEmployeeDetails(id);
                if (employee == null)
                {
                    return ReturnNotFoundError(id);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, employee);
                }
            }
            catch (Exception ex)
            {
                return ReturnInternalServerError(ex.Message);
            }
        }

        /// <summary>
        /// Post an employee object to the employess list.
        /// </summary>
        /// <param name="value">The employee object from body in string format.</param>
        /// <returns>Returns the http response message.</returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] EmployeeModel employee)
        {
            if (employee == null || !(IsAllowableAcceptHeader(Request)))
            {
                return ReturnBadRequest(Constants.NotAValidBodyErrorMessage);
            }

            if (IsRequestExpecting500(Request))
            {
                return ReturnInternalServerError(Constants.InternalServerErrorMessage);
            }

            try
            {
                _employeesInfo.AddEmployee(employee);

                return Request.CreateResponse(HttpStatusCode.Created,
                    Constants.AddedEmployeeMessage);
            }
            catch (Exception ex)
            {
                return ReturnInternalServerError(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the employee from the list.
        /// </summary>
        /// <param name="id">The employee id to be deleted.</param>
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
            {
                return ReturnBadRequest(Constants.NegativeEmployeeIdErrorMessage);
            }
            if (IsRequestExpecting500(Request))
            {
                return ReturnInternalServerError(Constants.InternalServerErrorMessage);
            }
            if (!(IsAllowableAcceptHeader(Request)))
            {
                return ReturnBadRequest(Constants.BadRequestErrorMessage);
            }

            try
            {
                EmployeeModel empModel = _employeesInfo.GetEmployeeDetails(id);
                if (empModel == null)
                {
                    return ReturnNotFoundError(id);
                }

                _employeesInfo.DeleteEmployee(id);

                return Request.CreateResponse(HttpStatusCode.NoContent,
                    $"The user with user id {empModel.EmployeeId} has been deleted successfully");
            }
            catch (Exception ex)
            {
                return ReturnInternalServerError(ex.Message);
            }
        }

        /// <summary>
        /// Verifies whether the request contains the allowable accept header.
        /// As of now, we are supporting only application/json in case request is received from any tool.
        /// Allowing text/html in case the request comes from web UI.
        /// In case the request does not contain any of the accept headers, Bad request is returned with 400 status code.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <returns>Returns whether the request has allowable accept header.</returns>
        internal bool IsAllowableAcceptHeader(HttpRequestMessage request)
        {
            return request
                .Headers
                .Accept
                .Where(x =>
                x.MediaType == "application/json" ||
                x.MediaType == "text/html")
                .Count() > 0;
        }

        /// <summary>
        /// User can request for internal server error explicitly using query strings.
        /// The method verifies whether the user is expecting an internal server error.
        /// This has been added to return internal server error explicitly.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <returns>Returns whether the user is expecting an internal server error.</returns>
        internal bool IsRequestExpecting500(HttpRequestMessage request)
        {
            var queryStringCollection = HttpUtility.ParseQueryString(request.RequestUri.Query);

            return !string.IsNullOrWhiteSpace(queryStringCollection[EXPECTING_500]) ? 
                    Convert.ToBoolean(queryStringCollection[EXPECTING_500]) : 
                    false;
        }

        /// <summary>
        /// Creates a bad request error response and 
        /// returns the http response message.
        /// </summary>
        /// <param name="message">The message to display to the user.</param>
        /// <returns>Returns the http response message.</returns>
        private HttpResponseMessage ReturnBadRequest(string message)
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                message);
        }

        /// <summary>
        /// Creates a internal server error response and 
        /// returns the http response message.
        /// </summary>
        /// <returns>Returns the http response message.</returns>
        private HttpResponseMessage ReturnInternalServerError(string message)
        {
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                message);
        }

        /// <summary>
        /// Method to return not found error.
        /// </summary>
        /// <returns>Returns http response message.</returns>
        private HttpResponseMessage ReturnNotFoundError(int id)
        {
            return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                string.Format(Constants.NotFoundErrorMessage, id.ToString()));
        }
    }
}
