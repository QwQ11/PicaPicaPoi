using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivApi
{
    public class Illust
    {
        public Illust() { }
        public Illust(string id,string imageUrl)
        {
            this.Id = id;
            this.ImageUrl = imageUrl;
        }
        public string Id { get; set; }
        public string ImageUrl { get; set; }
    }
}
