using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete
{
    public class EfPostRepository : IPostRepository
    {//IPostRepository interface i Iplement edecek olan concrete versiyonunu oluşturdum
    //alttaki hali implement hali olarak gelen hali ctrl+. ile ekledim sonra veritabanı bağlantısı sağlayıp o verileri çektim
    //bunu böyle yapmamın nedeni istediğim yerde çağırıp kullanabilmek diğer şekilde her yerde ayrı ayrı oluşturmam gerekiyordu

        private BlogContext _context;
        public EfPostRepository(BlogContext context)
        {
            _context = context;
        }
        
        public IQueryable<Post> Posts => _context.Posts;

        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }
    }
}