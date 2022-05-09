
namespace ECommerce.Application.Services.User.Dtos
{
    public class UserUpdateRequest
    {
        public int UserId { get; set; }
        public string UserMail { get; set; }
        public string UserFullName { get; set; }
        public string UserAddress { get; set; }
        public string UserWardCode { get; set; }
        public string UserDistrictCode { get; set; }
        public string UserCityCode { get; set; }
    }
}
