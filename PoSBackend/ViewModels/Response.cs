namespace PoSBackend.ViewModels
{
    public class Response<T>
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; } = default(T);
    }
}
