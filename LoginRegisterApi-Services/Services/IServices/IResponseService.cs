using LoginRegisterApi_Entities.Models;
using System.Net;


namespace LoginRegisterApi_Services.Services.IServices
{
    public interface IResponseService
    {
        APIResponse GetResponse(object Result, List<string> ErrorMessages, bool IsSuccess, HttpStatusCode StatusCode);
    }
}
