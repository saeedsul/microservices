using Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BaseController : ControllerBase
    { 
        protected IActionResult Success<T>(T data, string message) =>
             Ok(new ApiResponse<T> { Success = true, Message = message, Data = data });

        protected IActionResult Failure<T>(string message) =>
            NotFound(new ApiResponse<T> { Success = false, Message = message, Data = default });

        protected IActionResult BadRequest<T>(string message) =>
            BadRequest(new ApiResponse<T> { Success = false, Message = message, Data = default });

    }
}
