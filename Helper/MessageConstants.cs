using System.Configuration;

namespace shoppetApi.Helper
{
    public static class MessageConstants
    {
        public const string createdMessage = "created";
        public const string creatingMessage = "creating";
        public const string fetchedMessage = "fetched";
        public const string fetchingMessage = "fetching";
        public const string updatedMessage = "updated";
        public const string updatingMessage = "updating";
        public const string deletedMessage = "deleted";
        public const string deletingMessage = "deleting";


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
        public const string NoUser = "No User Found";

        public const string AlredyExistsSpeciesMessage = "Species already exists";
        public const string NotExistsSpeciesMessage = "Species Not Exists";

        public const string AlreadyExistsBreed = "Breed Already Exists";
        public const string NotExistsBreed = "Breed Not Exists";

        public const string WrongGender = "Please Select the right gender";

        public const string PetNotExists = "Pet Not Exists";

        public const string NoOrderFoundMessage = "No Orders Yet";
        public const string InvalidOperationMessage = "Invalid Operation Performed";
        public const string OrderAlreadyCancelledMessage = "Order is already cancelled";
        public const string AlreadyStatusUpdated = "Order is already {0}";
    }
}

