namespace Neama.Api.Errors
{
    public class ApiExceptionResponse : ApiResponse
    {
        public string? Details { get; set; }

        public ApiExceptionResponse(string message = null , string details = null) : base(500,message)
        {
            Details = details;
        }
    }
}
