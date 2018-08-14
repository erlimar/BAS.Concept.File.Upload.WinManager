namespace BAS.Concept.File.Upload.Client
{
    public class WebResponse<T>
        where T : class
    {
        public int StatusCode { get; set; }
        public string StatusText { get; set; }
        public T Data { get; set; }
    }
}
