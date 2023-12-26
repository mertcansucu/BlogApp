using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;

namespace BlogApp.Models
{
    public class PostsViewModel
    {//veri taşımak için kullanıcam
        public List<Post> Posts { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();
    }
}