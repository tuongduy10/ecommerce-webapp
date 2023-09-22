
namespace ECommerce.Application.BaseServices.User.Dtos
{
    public class UpdatePasswordRequest
    {
        public int UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string RePassword { get; set; }
    }
}
