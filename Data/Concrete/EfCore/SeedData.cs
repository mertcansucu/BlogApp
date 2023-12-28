using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData //static nesne tanımlamamak için
    {//burdaki amacım başlangıçta uygulamada kayıt olmadığı için ben içine otomatik kayıt ekledim
    //test verilerini veritabnına ekleyip test verileriiçin arayüz hazırlamaya gerek yok
        public static void TestVerileriniDoldur(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)//context üzerinden bir veri geliyor mu kondiye konrtol ettim veri geliyorsa işlem yaptım
            {
                if(context.Database.GetPendingMigrations().Any()){//eğer bizi bekleyen herhangi bir migration varsa uygula dedim
                    context.Database.Migrate();//uygulama her çalıştığında database sıfırdan oluşsun dedim
                }
                if(!context.Tags.Any())//herhangi bir tags yoksa yani kayıt yoksa, ben otomatik kayıt ekle dedim 
                {
                    context.Tags.AddRange(
                        new Tag { Text = "web programlama", Url = "web-programlama",Color = TagColors.warning },//eklediğim renkeleri etiketlerle eşleştirdim
                        new Tag { Text = "backend", Url = "backend",Color= TagColors.success},
                        new Tag { Text = "frontend", Url = "frontend",Color= TagColors.danger},
                        new Tag { Text = "fullstack", Url = "fullstack", Color= TagColors.secondary },
                        new Tag { Text = "php", Url = "php", Color= TagColors.primary }
                    );
                    context.SaveChanges();
                }

                if(!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { UserName = "mertcansucu",Image = "mcs.jpg"},
                        new User { UserName = "burakkotan",Image = "p1.jpg"},
                        new User { UserName = "selametsamli",Image = "p2.jpg"}
                    );
                    context.SaveChanges();
                }

                if(!context.Posts.Any())
                {
                    /*
                        bilgileri güncellediğimde veri tabanını yeniden yüklemek için
                            dotnet ef database drop --force
                            dotnet watch run

                        **url ekledim onu kodda söylemezsem hata verir onun içinde: 
                        dotnet ef migrations add UpdateUrlTables

                    */
                    context.Posts.AddRange(
                        new Post {
                            Title = "Asp.net core",
                            Content = "Asp.net core dersleri",
                            Url = "aspnet-core",
                            IsActive = true,
                            PubilshedOn = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "1.jpg",
                            UserId = 1,
                            Comments = new List<Comment>{
                                new Comment {Text = "İyi Bir Kurs",PublishedOn = DateTime.Now.AddDays(-20), UserId = 1},
                                new Comment {Text = "Çok faydalandığım bir kurs",PublishedOn = DateTime.Now.AddDays(-15), UserId = 2},
                                new Comment {Text = "Çok faydalandığım bir kurs",PublishedOn = DateTime.Now.AddDays(-10), UserId = 3}
                            }
                        },
                        new Post {
                            Title = "Php",
                            Content = "Php core dersleri",
                            Url = "php",
                            IsActive = true,
                            PubilshedOn = DateTime.Now.AddDays(-20),
                            Tags = context.Tags.Take(2).ToList(),//bu postu ilk dort etiketle eslestirdim
                            Image = "2.jpg",
                            UserId = 1
                        },
                        new Post {
                            Title = "Django",
                            Content = "Django dersleri",
                            Url = "django",
                            IsActive = true,
                            PubilshedOn = DateTime.Now.AddDays(-30),
                            Tags = context.Tags.Take(4).ToList(),
                            Image = "2.jpg",
                            UserId = 2
                        }
                        ,
                        new Post {
                            Title = "React Dersleri",
                            Content = "React dersleri",
                            Url = "react",
                            IsActive = true,
                            PubilshedOn = DateTime.Now.AddDays(-40),
                            Tags = context.Tags.Take(4).ToList(),
                            Image = "3.jpg",
                            UserId = 2
                        }
                        ,
                        new Post {
                            Title = "Angular",
                            Content = "Angular dersleri",
                            Url = "angular",
                            IsActive = true,
                            PubilshedOn = DateTime.Now.AddDays(-50),//güncel tarihten 50 gün öncesini ekledim
                            Tags = context.Tags.Take(4).ToList(),//ilk 4 etiketi al dedim
                            Image = "1.jpg",
                            UserId = 2
                        }
                        ,
                        new Post {
                            Title = "Web Tasarım",
                            Content = "Web tasarım dersleri",
                            Url = "web-tasarim",
                            IsActive = true,
                            PubilshedOn = DateTime.Now.AddDays(-60),
                            Tags = context.Tags.Take(4).ToList(),
                            Image = "2.jpg",
                            UserId = 2
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}