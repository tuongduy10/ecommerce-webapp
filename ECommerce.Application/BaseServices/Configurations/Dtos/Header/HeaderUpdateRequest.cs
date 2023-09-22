
namespace ECommerce.Application.BaseServices.Configurations.Dtos.Header
{
    public class HeaderUpdateRequest
    {
        public int HeaderId { get; set; }
        public string HeaderPosition { get; set; }
        public string HeaderName { get; set; }
        public byte? Status { get; set; }
    }
}
