using LoginRegisterApi_Entities.Models;
using LoginRegisterApi_Services.Services.IServices;
using System.Net;


namespace LoginRegisterApi_Services.Services
{
    public class ResponseService : IResponseService
    {
        protected APIResponse _response;
        public ResponseService()
        {
            this._response = new APIResponse();
        }

        public APIResponse GetResponse(object Result, List<string> ErrorMessages, bool IsSuccess, HttpStatusCode StatusCode)
        {
            _response.IsSuccess = IsSuccess;
            _response.ErrorMessages = ErrorMessages;
            _response.StatusCode = StatusCode;
            _response.Result = Result;
            return _response;
        }
    }
}
