using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Entity
{
    public class User
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Image {get; set;}
        public List<Post> Posts { get; set; } = new List<Post>();//bir kullanıcı birden fazla post yayınlayacağı için liste şeklinde bağlantıyı sağladım
        public List<Comment> Comments { get; set; } = new List<Comment>();//Bir kullanıı bir posta birden fazla yorum yapabildiği için liste şeklinde bağladım
    }
}