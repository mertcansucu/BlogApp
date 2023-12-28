using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController: Controller
    {
        // private readonly BlogContext _context;//BlogContext ten nesne ürettim
        // //veri tabanı bilgilerini çekmek için
        // public PostsController(BlogContext context){
        //     _context = context;
        // }bu injection yöntemi bunun yerine interface aracılığı ile yapıp bağımlılıkları ortadan kaldırıp ayrıca yazdığım kodları tek bir yerden kontrol etmiş oldum

        //**Postun içinde tag bilgileri olduğu için onuda ekledim
        //ancak iki tablodan veri göndereceğim için bunları ortak bir models(PostsViewModel) içinde çağırıp index sayfasına ekleyerek yaptım

        //bu yöntemden sonra ben component kullanarak veri çektim onun için bu bilgileri sidim

        private IPostRepository _postRepository;
        
        public PostsController(IPostRepository postRepository){
            _postRepository = postRepository;
            // _tagsRepository = tagsRepository;
        }
        public async Task<IActionResult> Index(string tag){//burdaki tag url olarak kullanıyorum karışıklık olmasın diye böyle dedim
            var posts = _postRepository.Posts;//Iquerayble bir bilgi yani veri tabanından bilgileri şuan almıyorum sadece bağlantıyı sağladım
            if (!string.IsNullOrEmpty(tag))//postaki tag bilgisi boş değilse
            {
                posts = posts.Where(x => x.Tags.Any(t => t.Url == tag));//her bir postun taglerine bakıyorum taglerin içerisinde ise tag le eşleşen bir kayıt varsa bu durumda o postu geriye döndere posts ile alıyorum,burda veri tabanı çalışmıyor sadece bilgileri aldım
            }
            return View(new PostsViewModel
            {// index.cshtml sayfasına bilgileri gönderdim
                Posts = await posts.ToListAsync()//burda diyorum ki ya bütün bilgileri gönder ya da if ile koşul sağlanırsa ordaki istediğim bilgileri sadece bana gönder

            }); 
        }

        //önceden url de id bilgisi vardı ben bunu url ile değiştirdim
        public async Task<IActionResult> Details(string url){
            return View(await _postRepository
            .Posts
            .Include(x => x.Tags)//joinleme yaptım
            .FirstOrDefaultAsync(p => p.Url == url));
        }
    }
}