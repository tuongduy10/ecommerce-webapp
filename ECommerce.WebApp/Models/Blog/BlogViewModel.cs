using System.Data;

namespace ECommerce.WebApp.Models.Blog
{
    public class BlogViewModel
    {
        public int Id { get; set; }
        public string Type {get; set;}
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
    }
}
