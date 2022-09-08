namespace Mango.Services.ProductsAPI.DTOs
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; } = true;

        public Object? Result { get; set; }

        public string? DisplayMessage { get; set; }

        public List<string>? ErrorMessages { get; set; }
    }
}
