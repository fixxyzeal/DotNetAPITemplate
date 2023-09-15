namespace BL.ViewModels
{
    public class ResultViewModel
    {
        public bool Success { get; set; }

        public int HttpStatusCode { get; set; }
        public string? Message { get; set; }

        public object? Data { get; set; }
    }
}