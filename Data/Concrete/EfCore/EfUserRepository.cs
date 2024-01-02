using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete
{
    public class EfUserRepository : IUserRepository
    {//ITagRepository interface i Iplement edecek olan concrete versiyonunu oluşturdum
    //alttaki hali implement hali olarak gelen hali ctrl+. ile ekledim sonra veritabanı bağlantısı sağlayıp o verileri çektim
    //bunu böyle yapmamın nedeni istediğim yerde çağırıp kullanabilmek diğer şekilde her yerde ayrı ayrı oluşturmam gerekiyordu

        private BlogContext _context;
        public EfUserRepository(BlogContext context)
        {
            _context = context;
        }
        
        public IQueryable<User> Users => _context.Users;

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}