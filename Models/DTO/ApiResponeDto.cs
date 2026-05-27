namespace QuickPay.Models.DTO
{
    public class ApiResponeDto<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
