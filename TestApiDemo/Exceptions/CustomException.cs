using System;
using System.Net;

namespace TestApiDemo.Exceptions
{
    public class CustomException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        protected CustomException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
