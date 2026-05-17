namespace PatriSystem.Domain.Common
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new();
        public T? Result { get; set; }
        public Exception? Exception { get; set; }

        public static Response<T> Failure(Exception? ex, string message = "Ha ocurrido un error al generar la solicitud")
        {
            return new Response<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = ex != null
                    ? new List<string> { ex.Message }
                    : new List<string> { message },
                Exception = ex
            };
        }

        public static Response<T> Failure(string message, List<string>? errors = null)
        {
            return new Response<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                Exception = null
            };
        }

        public static Response<T> Success(T result, string message = "Tarea realizada con éxito")
        {
            return new Response<T>
            {
                IsSuccess = true,
                Message = message,
                Result = result,
                Exception = null
            };
        }

        public static Response<T> Success(string message = "Tarea realizada con éxito")
        {
            return new Response<T>
            {
                IsSuccess = true,
                Message = message,
                Exception = null
            };
        }
    }
}