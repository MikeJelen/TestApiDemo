using System.Diagnostics.CodeAnalysis;

namespace TestApiDemo.Models
{
    [ExcludeFromCodeCoverage]
    public class DemoResponse
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }
}
