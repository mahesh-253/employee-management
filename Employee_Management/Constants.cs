namespace Employee_Management
{
    public class Constants
    {
        /// <summary>
        /// Holds constant for the string InternalServerErrorMessage.
        /// </summary>
        public const string InternalServerErrorMessage = "An internal server error is received. Please try again later.";
        
        /// <summary>
        /// Holds constant for the string BadRequestErrorMessage.
        /// </summary>
        public const string BadRequestErrorMessage = "The requested accept header(s) is not accepted. Only application/json format is supported.";
        
        /// <summary>
        /// Holds constant for the string NotFoundErrorMessage.
        /// </summary>
        public const string NotFoundErrorMessage = "No employee found with the given employee id {0}.";
        
        /// <summary>
        /// Holds constant for the string NotAValidBodyErrorMessage.
        /// </summary>
        public const string NotAValidBodyErrorMessage = "The body is not in a valid format.";
        
        /// <summary>
        /// Holds constant for the string AddedEmployeeMessage.
        /// </summary>
        public const string AddedEmployeeMessage = "A new employee has been added succesfully.";
        
        /// <summary>
        /// Holds constant for the string NegativeEmployeeIdErrorMessage.
        /// </summary>
        public const string NegativeEmployeeIdErrorMessage = "User id cannot be a negative number";
        
        /// <summary>
        /// Holds constant for the string InternalServerErrorWhileRetrievingEmployees.
        /// </summary>
        public const string InternalServerErrorWhileRetrievingEmployees = "Some internal error received while retrieving the employees list. Please try again later";

        /// <summary>
        /// Holds constant for the string IdCannotBeZeroErrorMessage.
        /// </summary>
        public const string IdCannotBeZeroErrorMessage = "The id cannot be less than or equal to zero. Please pass a valid employee id";
    }
}