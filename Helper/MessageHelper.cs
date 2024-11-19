namespace shoppetApi.Helper
{
    public static class MessageHelper
    {
        public static string Success(string entity, string action)
        {
            return string.Format(MessageConstants.SuccessMessage, entity, action);
        }

        public static string Failure(string entity, string action)
        {
            return string.Format(MessageConstants.FailureMessage, entity, action);
        }

        public static string Exception(string entity, string action, string exception)
        {
            return string.Format(MessageConstants.ExceptionMessage, entity, action, exception);
        }

        public static string ValidationError(string field)
        {
            return string.Format(MessageConstants.ValidationErrorMessage, field);
        }

        public static string AlreadyExists(string field)
        {
            return string.Format(MessageConstants.AlreadyExistsMessage, field);
        }

        public static string ErrorOccurred(string message)
        {
            return string.Format(MessageConstants.ErrorOccuredMessage, message);
        }

        public static string NotFound(string field)
        {
            return string.Format(MessageConstants.NotFoundMessage, field);
        }

        public static string Message(string message)
        {
            return message;
        }
    }
}
