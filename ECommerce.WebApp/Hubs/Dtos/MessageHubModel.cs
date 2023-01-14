namespace ECommerce.WebApp.Hubs.Dtos
{
    public class MessageHubModel
    {
        public int FromUserId { get; set;}
        public int ToUserId { get; set;}
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string PhoneNumber { get; set; }
    }
}
