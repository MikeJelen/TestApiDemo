using System.Net;

namespace TestApiDemo.Exceptions
{
    public class BadRequestException : CustomException
    {
        public BadRequestException(string message) : base(HttpStatusCode.BadRequest, message)
        {
        }
    }
}
