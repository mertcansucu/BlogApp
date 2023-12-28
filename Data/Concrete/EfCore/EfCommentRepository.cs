using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete
{
    public class EfCommentRepository : ICommentRepository
    {//ITagRepository interface i Iplement edecek olan concrete versiyonunu oluşturdum
    //alttaki hali implement hali olarak gelen hali ctrl+. ile ekledim sonra veritabanı bağlantısı sağlayıp o verileri çektim
    //bunu böyle yapmamın nedeni istediğim yerde çağırıp kullanabilmek diğer şekilde her yerde ayrı ayrı oluşturmam gerekiyordu

        private BlogContext _context;
        public EfCommentRepository(BlogContext context)
        {
            _context = context;
        }
        
        public IQueryable<Comment> Comments => _context.Comments;

        public void CreateComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }
    }
}