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
                        new Tag { Text = "web programlama" },
                        new Tag { Text = "backend" },
                        new Tag { Text = "frontend" },
                        new Tag { Text = "fullstack" },
                        new Tag { Text = "php" }
                    );
                    context.SaveChanges();
                }

                if(!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { UserName = "mertcansucu"},
                        new User { UserName = "esrasucu"}
                    );
                    context.SaveChanges();
                }

                if(!context.Posts.Any())
                {
                    /*
                        bilgileri güncellediğimde veri tabanını yeniden yüklemek için
                            dotnet ef database drop --force
                            dotnet watch run

                    */
                    context.Posts.AddRange(
                        new Post {
                            Title = "Asp.net core",
                            Content = "Asp.net core dersleri",
                            IsActive = true,
                            PubilshedOn = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "1.jpg",
                            UserId = 1,
                        },
                        new Post {
                            Title = "Php",
                            Content = "Php core dersleri",
                            IsActive = true,
                            PubilshedOn = DateTime.Now.AddDays(-20),
                            Tags = context.Tags.Take(2).ToList(),
                            Image = "2.jpg",
                            UserId = 1
                        },
                        new Post {
                            Title = "Django",
                            Content = "Django dersleri",
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