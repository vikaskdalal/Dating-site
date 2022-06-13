namespace DotNetCoreAngular.Errors
{
    public class ApiException
    {
        public ApiException(int statusCode, string message = null, string details = null, string innerException = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
            InnerException = innerException;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public string InnerException { get; set; }
    }
}
