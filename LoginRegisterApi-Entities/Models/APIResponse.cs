using System.Net;

namespace LoginRegisterApi_Entities.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public object Result { get; set; }

        public List<string> ErrorMessages { get; set; }

        public bool IsSuccess { get; set; }
    }
}
