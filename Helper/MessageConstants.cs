using System.Configuration;

namespace shoppetApi.Helper
{
    public static class MessageConstants
    {
        public const string SuccessMessage = "{0} has been successfully {1}.";
        public const string FailureMessage = "An error occurred while {1} the {0}.";
        public const string ExceptionMessage = "An exception occurred while {1} the {0}: {2}.";
        public const string ValidationErrorMessage = "The {0} is invalid. Please check the {0}.";
        public const string AlreadyExistsMessage = "{0} already exists.";
        public const string ErrorOccuredMessage = "An unexpected error occurred: {0}.";
        public const string NotFoundMessage = "{0} not found.";

        public const string InvalidId = "Id is Invalid";
        public const string NullId = "Id can not be null";
        public const string DataNotFound = "Data not found";

        public const string UnAuthenticatedUser = "User is not authenticated";
        public const string UnAuthorizedUser = "User is not authorized to access";

        public const string AlredyExistsSpecies = "Species already exists";
        public const string NotExistsSpecies = "Species Not Exists";
        public const string AlreadyExistsBreed = "Breed Already Exists";
    }
}

