namespace shoppetApi.Helper
{
    public class APIResponse<T>
    {
        public bool Success { get; set; }
        public string Message {  get; set; }
        public T? Data { get; set; }

        public static APIResponse<T> CreateResponse(bool success, string message, T? data)
        {
            return new APIResponse<T>
            {
                Success = success,
                Message = message,
                Data = data
            };
        }
    }

}
