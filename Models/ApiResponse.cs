namespace ONSTEPS_API.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        //contrustor without parameter
        public ApiResponse() { }

        //constructor with parameter
        public ApiResponse(bool success, string message, T data)
        {
            Success=success;
            Message=message;
            Data=data;
        }
    }
}
