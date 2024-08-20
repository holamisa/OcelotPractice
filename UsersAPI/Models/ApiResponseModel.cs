using System.Net;

namespace UsersAPI.Models
{
    public class ApiResponseModel
    {
        public int Code { get; set; }
        public string? Message { get; set; }

        public static ApiResponseModel CREATED()
        {
            return new ApiResponseModel
            {
                Code = (int)HttpStatusCode.Created,
                Message = "성공"
            };
        }

        public static ApiResponseModel OK()
        {
            return new ApiResponseModel
            {
                Code = (int)HttpStatusCode.OK,
                Message = "성공",
            };
        }

        public static ApiResponseModel ERROR(int errorCode, string message)
        {
            return new ApiResponseModel
            {
                Code = errorCode,
                Message = message
            };
        }
    }

    public class ApiResponseModel<T> : ApiResponseModel
    {
        public T? Data { get; set; }

        public static ApiResponseModel<T> CREATED(T? data)
        {
            return new ApiResponseModel<T>
            {
                Code = (int)HttpStatusCode.Created,
                Message = "성공",
                Data = data
            };
        }

        public static ApiResponseModel<T> OK(T? data)
        {
            return new ApiResponseModel<T>
            {
                Code = (int)HttpStatusCode.OK,
                Message = "성공",
                Data = data
            };
        }
    }
}
