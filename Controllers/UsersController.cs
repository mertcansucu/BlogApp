using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class UsersController: Controller
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository){
            _userRepository = userRepository;
        }

        public IActionResult Login(){
            if (User.Identity!.IsAuthenticated)//eğer login girişi yaptıysam ben linkten bile login ekranına gitmeme gerek kalmaz onu engelleyip onun yerine gideceği sayfayı ekledim
            {
                return RedirectToAction("Index","Posts");
            }
            return View();
        }

        public IActionResult Register(){
            return View();
        }
        [HttpPost]
         public async Task<IActionResult> Register(RegisterViewModel model,IFormFile imageFile){
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    var extension = Path.GetExtension(imageFile.FileName);//dosyanın uzantısını alır ; mesela burda imageFile.FileName buna bakar abc.jpg ise "jpg" kısmını alır
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");//burda random isim oluşturup(Guid.NewGuid()) üste dosyadan aldığım uzatıyı ekliyorum direk
                    var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img", randomFileName);

                    //resim eklenmesinde sorun olmazsa burda stremi oluşturuyorum çünkü kapsamdan çıktığında bellekten silinsin diye
                    using(var stream = new FileStream(path, FileMode.Create)){
                    await imageFile.CopyToAsync(stream);//ilgili dizine kopyaladım resmi ve çalışması için Task<IActionResult> yaptım
                    }
                    model.Image = randomFileName;
                }

                var user = await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName || x.Email == model.Email);
                if (user == null)
                {
                    _userRepository.CreateUser(new Entity.User{
                        UserName = model.UserName,
                        Name = model.Name,
                        Email = model.Email,
                        Image = model.Image,
                        Password = model.Password
                    });
                    return RedirectToAction("Login");
                }else
                {
                    ModelState.AddModelError("","Username ya da Email kullanılmadı.");
                }
                
            }
            return View(model);
        }

        public async Task<IActionResult> Logout(){
            //layout içinde oluşturduğum logout butonunu aktifleştirdim
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);//öncedec oluşturulmuş cookie leri sildim
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model){
            if (ModelState.IsValid)
            {
                var isUser = _userRepository.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);//kullanıcı bilgisi veri tabanındaki bilgileriyle eşleşip eşleşmediğini kontrol ettim ve doğruysa if bloğunda işleticem
                if (isUser != null)
                {
                    //eğer kullanıcı varsa onun claimini oluşturdum yani bir kullanıcın özelliklerini barındırıyor ve ben bunları uygulama tarafına taşıyorum
                    var userClaims =  new List<Claim>();

                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier,isUser.UserId.ToString()));//NameIdentifier kullanıcının id sini tutar
                    userClaims.Add(new Claim(ClaimTypes.Name,isUser.UserName ?? ""));//ClaimTypes.Name,isUser.UserName ?? "" => kullanıcının ad bilgisini alıyorum ama eğer boşsa onun yerine boş olarak kaydet diyorum
                    userClaims.Add(new Claim(ClaimTypes.GivenName,isUser.Name ?? ""));//GivenName ikini ad gibi düşün soyadı da kaydede bilirsin bu şekilde
                    userClaims.Add(new Claim(ClaimTypes.UserData,isUser.Image ?? ""));//img bilgisini almak için giriş yapan kullanıcının

                    if (isUser.Email == "mrtcnscc@gmail.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role,"admin"));//bu mail adresinde ki kişiyi admin belirledim
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);//bu üste oluşturduğum claim listesini cooki ye aktardım

                    var authProperties = new AuthenticationProperties{
                        IsPersistent = true //bu kısım bizim beni hatırla kısmı yani kullanıcı çıkış yapmadığı sürece kullanıcı girişli olarak kullanabiliyor
                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);//öncedec oluşturulmuş cookie leri sildim

                    //uygulamaya giriş
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                    return RedirectToAction("Index","Posts");


                }else
            {
                ModelState.AddModelError("","Kullanıcı adı veya şifre yanlış");
            }
                
            }
            return View(model);
        }
    }
}