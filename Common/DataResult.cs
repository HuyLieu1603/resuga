namespace Dashboard.Common
{
    public class DataResult<T>
    {
        public DataResult()
        {
        }
        public DataResult(T data)
        {
            Data = data;
        }

        public DataResult(int status, string message)
        {
            Status = status;
            Message = message;
        }

        public DataResult(int status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        public DataResult(int status, string message, T data, int totalCount)
        {
            Status = status;
            Message = message;
            Data = data;
            TotalCount = (int)totalCount;
        }

        public int Status { get; set; } = 500;
        public string? Message { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
        public T? Data { get; set; }
        public int TotalCount { get; set; }
    }
}
