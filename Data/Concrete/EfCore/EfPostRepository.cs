using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

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

        public void EditPost(Post post)
        {
            var entity = _context.Posts.FirstOrDefault(i => i.PostId == post.PostId);

            if (entity != null)
            {
                entity.Title = post.Title;
                entity.Description = post.Description;
                entity.Content = post.Content;
                entity.Url = post.Url;
                entity.IsActive = post.IsActive;

                _context.SaveChanges();//oluşturduğum bu metodu postcont içinde kullanıcam ve güncellemeyi yapıcam

            }
        }

        public void EditPost(Post post, int[] tagIds)
        {
            var entity = _context.Posts.Include(i => i.Tags).FirstOrDefault(i => i.PostId == post.PostId);//Include(i => i.Tags) ifadesini kullanmazsam, Post nesnesi yüklenirken Tags nesneleri yüklenmez. Bu durumda, posta ait etiketler hafızada olmadığı için, bu etiketleri güncellemeye çalıştığımda da hata alırım bunu ekleyerek hatayı engelleyip güncellemeyi yaptım

            if (entity != null)
            {
                entity.Title = post.Title;
                entity.Description = post.Description;
                entity.Content = post.Content;
                entity.Url = post.Url;
                entity.IsActive = post.IsActive;

                entity.Tags = _context.Tags.Where(tag => tagIds.Contains(tag.TagId)).ToList();//burdaki kodda ben veritabnına sorgu yaptım diyorum ki ben seçtiğim etiketleri al ve veritabanına bak orda eşleşen aynı idli olanlar varsa seçilmemiş olsada buna göre güncelle diyorum

                _context.SaveChanges();//oluşturduğum bu metodu postcont içinde kullanıcam ve güncellemeyi yapıcam

            }
        }
    }
}