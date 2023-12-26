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
        public IActionResult Index(){
            return View(new PostsViewModel
            {// index.cshtml sayfasına bilgileri gönderdim
                Posts = _postRepository.Posts.ToList(),
                // Tags = _tagsRepository.Tags.ToList()

            }); 
        }

        public async Task<IActionResult> Details(int? id){
            return View(await _postRepository.Posts.FirstOrDefaultAsync(p => p.PostId == id));
        }
    }
}