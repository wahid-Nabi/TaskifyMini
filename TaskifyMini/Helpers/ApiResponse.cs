namespace TaskifyMini.Helpers
{
    public class ApiResponse<T> 
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null!;
        public T? Data { get; set; }
    }
}
