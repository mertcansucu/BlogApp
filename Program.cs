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

var app = builder.Build();

app.UseStaticFiles();//wwwroot altındaki dosyalar http isteklerini karşılar yani erişimi açtım

SeedData.TestVerileriniDoldur(app);//app aracılığıyla Services e ulaşıp içerindeki BlogContext bilgisini alıcam

// app.MapGet("/", () => "Hello World!"); ana sayfaya gelen routing var onu kapattım
app.MapDefaultControllerRoute();

app.Run();
