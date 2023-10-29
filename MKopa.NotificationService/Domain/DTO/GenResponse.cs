using MKopa.NotificationService.Enums;

namespace MKopa.NotificationService.Domain.DTO
{
    public class GenResponse<T>
    {
        public virtual bool IsSuccess { get; set; }

        public T Result { get; set; }

        public virtual string Message { get; set; }

        public virtual string Error { get; set; }

        public virtual int StatCode { get; set; } = 200;


        public static GenResponse<T> Success(T result, StatusCodeEnum statusCode = StatusCodeEnum.OK, string message = null)
        {
            return new GenResponse<T>
            {
                IsSuccess = true,
                Result = result,
                Message = message,
                StatCode = (int)statusCode
            };
        }

        public static GenResponse<T> Failed(string error, StatusCodeEnum statusCode = StatusCodeEnum.BadRequest)
        {
            return new GenResponse<T>
            {
                IsSuccess = false,
                Error = error,
                StatCode = (int)statusCode
            };
        }
    }
}
