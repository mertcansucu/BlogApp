using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();//Controller ları kullanmak için

builder.Services.AddDbContext<BlogContext>(options =>{//veritabı bağlantılarını yapacağım yeri yazdım => BlogContext.cs
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("mysql_connection");
    /*
        sql_connection =>  bu bilgiyi appsettings.Development.json dan aldım:
    */
    // options.UseSqlite(connectionString);//sqllite tan kaptıp mysql e geçtim
    //bu işlemlerden sonra BlogContext ile birleştirdiğim entityleri veritabına aktarıcam bunu migration ile yaptım

    //mysql bağlantı
    var version = new MySqlServerVersion(new Version(8,0,35));
    options.UseMySql(connectionString,version);
});

builder.Services.AddScoped<IPostRepository, EfPostRepository>();//ben burda diyorum ki IPostRepository ben sanalı göderdiğimde sen bana EfPostRepository ile gerçek halini bana gönder,AddScoped olmasının nedeni her http reqository aynı nesneyi gönderir yani her http requestinde bir nesne yollar
builder.Services.AddScoped<ITagRepository, EfTagRepository>();
builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();

var app = builder.Build();

app.UseStaticFiles();//wwwroot altındaki dosyalar http isteklerini karşılar yani erişimi açtım

SeedData.TestVerileriniDoldur(app);//app aracılığıyla Services e ulaşıp içerindeki BlogContext bilgisini alıcam

// app.MapGet("/", () => "Hello World!"); ana sayfaya gelen routing var onu kapattım

//ben ekranda url kısmını değiştirmek istiyorum onun için eklemeler yapıcam ve benim şstediğim şekli:
//localhost://posts/react-dersleri
//localhost://posts/php-dersleri
//post icin iliskilendirme
app.MapControllerRoute(
    name: "post_details",
    pattern: "posts/details/{url}",//posts sabit yer diğer kısım ise sayfanın urlsini çekmek olacak
    defaults: new {controller = "Posts",action="Details"}
);
//tag icin iliskilendirme,tag bilgilerine göre post bilgilerini listeleme
//localhost://posts/tag/php-dersleri
app.MapControllerRoute(
    name: "posts_by_tag",//tag bilgilerine göre post bilgilerini listeleme
    pattern: "posts/tag/{tag}",//posts ve tag sabit yer diğer kısım ise sayfanın urlsini çekmek olacak burda bu şekilde yapmamın nedeni postun urlsini değil tag in urlsini almak,url yerine tag yazmamın nedeni karışmaması için
    defaults: new {controller = "Posts",action="Index"}
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
