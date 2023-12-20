using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public class BlogContext:DbContext
    {//veritabını bağlantılarını burda sağlıyorum bağlantıları sağlarken=>appsettings.Development ve program.cs eklentiler yaptım
        public BlogContext(DbContextOptions<BlogContext> options): base(options){

        }
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<User> Users => Set<User>();
    }
}