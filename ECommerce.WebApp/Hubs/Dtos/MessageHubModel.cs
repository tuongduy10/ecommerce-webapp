namespace ECommerce.WebApp.Hubs.Dtos
{
    public class MessageHubModel
    {
        public string FromName { get; set; }
        public string FromPhoneNumber { get; set; }
        public string ToPhoneNumber { get; set;}
        public string Message { get; set; }
    }

}
