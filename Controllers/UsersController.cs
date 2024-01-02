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

namespace BlogApp.Controllers
{
    public class UsersController: Controller
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository){
            _userRepository = userRepository;
        }

        public IActionResult Login(){
            return View();
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