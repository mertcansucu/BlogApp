using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
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
        private ICommentRepository _commentRepository;

        private ITagRepository _tagRepository;
        
        public PostsController(IPostRepository postRepository, ICommentRepository commentRepository,ITagRepository tagRepository){
            _postRepository = postRepository;
            // _tagsRepository = tagsRepository;
            _commentRepository = commentRepository;
            _tagRepository = tagRepository;
        }
        public async Task<IActionResult> Index(string tag){//burdaki tag url olarak kullanıyorum karışıklık olmasın diye böyle dedim

            

            var posts = _postRepository.Posts.Where(i => i.IsActive);//Isactive ekleyerek sadece true olanları göster dedim
            //Iquerayble bir bilgi yani veri tabanından bilgileri şuan almıyorum sadece bağlantıyı sağladım
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
            .Include(x => x.User)//postu yayınlayan kişinin bilgisini almak için ekledim
            //doğrudan ulaştıklarım include olur ama theninclude farklı bir yere geçip ordaki bilgiyi almak istediğimde öyle yaparım
            .Include(x => x.Tags)//joinleme yaptım
            .Include(x => x.Comments)//o postla ilgili yorumları ekledim join ile
            .ThenInclude(x => x.User)//bunu böyle yapmamın nedeni commentinde user bilgisini alıp onun içindeki resmi çekmek için
            .FirstOrDefaultAsync(p => p.Url == url));
        }

        [HttpPost]
        public JsonResult AddComment(int PostId, string Text){
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var avatarImg = User.FindFirstValue(ClaimTypes.UserData);

            var entity = new Comment{//yorum ekleme
                PostId = PostId,
                Text = Text,
                PublishedOn = DateTime.Now,
                UserId = int.Parse(userId ?? "")
            };
            _commentRepository.CreateComment(entity);
            /*json ile ajax request yaptığım için sayfayı json döndürücemki sayfa yenilenmedem yorum eklensin
                // return Redirect("/posts/details/" + Url);
                //2. yol bunu programcs de post_details direkt alıp o kısmı otomatik dolturttum
                // return RedirectToRoute("post_details", new{url = Url});
            */
            return Json(new{//yorum yapan kişi
                username,
                Text,
                entity.PublishedOn,
                // entity.User.Image bunu direk kullanıcın img ile kullanıcam
                avatarImg
            });
            
        }

        [Authorize] // kullanıvı giriş yapmadan post ekleme yapmasını engellemek için bunu ekledim
        public IActionResult Create(){
            return View();
        }

        [HttpPost]
        [Authorize] // kullanıvı giriş yapmadan post ekleme yapmasını engellemek için bunu ekledim
        public IActionResult Create(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //kullanıcın idsine göre eklemek için aldım

                _postRepository.CreatePost(
                    new Post{
                        Title = model.Title,
                        Content = model.Content,
                        Url = model.Url,
                        UserId = int.Parse(userId ?? ""),
                        PubilshedOn = DateTime.Now,
                        Image = "1.jpg",
                        IsActive = false // admin onayından sonra görünmesini yapacağım için false dedim başta
                    }
                );
                return RedirectToAction("Index");

            }
            return View(model);
        }

        [Authorize] // kullanıcı giriş yapmadan post ekleme yapmasını engellemek için bunu ekledim
        public async Task<IActionResult> List(){//bu listenin amacı admin kişisi tüm postları görebiliyor
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);

            var posts = _postRepository.Posts;//tüm post bilgilerimni aldım ama veritabnı ile çağırmadım

            if (string.IsNullOrEmpty(role))//burda user rolu olmayanları bul dedim ve onlara sadece kendi paylaştığı postları görsün dedim
            {
                posts = posts.Where(i => i.UserId == userId);
            }
            return View(await posts.ToListAsync());//veri tabanıyla bağlantı sağlayıp postları çağırdım
        }

        [Authorize] // kullanıcı giriş yapmadan post ekleme yapmasını engellemek için bunu ekledim
        public IActionResult Edit(int? id){
            if (id == null)
            {
                return NotFound();
            }

            var post = _postRepository.Posts.Include(x => x.Tags).FirstOrDefault(i => i.PostId == id);//sayfa yüklenirken kullanıcın önceden seçtiği tagler varsa onlarda gelsin diye
            if (post == null)
            {
                return NotFound();
            }

            ViewBag.Tags = _tagRepository.Tags.ToList();//taglaeri alıp onları postlarla ilişkilendirme yapıcam 

            return View(new PostCreateViewModel {
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                Url = post.Url,
                IsActive = post.IsActive,
                Tags = post.Tags
            });
        }

        [Authorize] // kullanıcı giriş yapmadan post ekleme yapmasını engellemek için bunu ekledim
        [HttpPost]
        public IActionResult Edit(PostCreateViewModel model){
            if (ModelState.IsValid)
            {
                var entityToUpdate = new Post{
                    PostId = model.PostId,
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url
                };

                if(User.FindFirstValue(ClaimTypes.Role) == "admin"){//admin girip is active durumu güncellesin dedim
                    entityToUpdate.IsActive = model.IsActive;
                }

                _postRepository.EditPost(entityToUpdate);//efpostrepository e gönderip güncellemeyi gerçekleştirdim
                return RedirectToAction("List");
            }
            return View(model);
        }
    }
}