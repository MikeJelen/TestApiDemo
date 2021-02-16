using System.Net;

namespace TestApiDemo.Exceptions
{
    public class InvalidServiceException : CustomException
    {
        public InvalidServiceException(string message) : base(HttpStatusCode.Forbidden, message)
        {
        }
    }
}

