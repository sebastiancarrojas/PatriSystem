namespace PatriSystem.Domain.Common
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new();
        public T? Result { get; set; }
        public string? ExceptionMessage { get; set; }

        public static Response<T> Failure(Exception? ex, string message = "Ha ocurrido un error al generar la solicitud")
        {
            return new Response<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = ex != null
                    ? new List<string> { ex.Message }
                    : new List<string> { message },
                ExceptionMessage = ex?.Message
            };
        }

        public static Response<T> Failure(string message, List<string>? errors = null)
        {
            return new Response<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                ExceptionMessage = null
            };
        }

        public static Response<T> Success(T result, string message = "Tarea realizada con éxito")
        {
            return new Response<T>
            {
                IsSuccess = true,
                Message = message,
                Result = result,
                ExceptionMessage = null
            };
        }

        public static Response<T> Success(string message = "Tarea realizada con éxito")
        {
            return new Response<T>
            {
                IsSuccess = true,
                Message = message,
                ExceptionMessage = null
            };
        }
    }
}