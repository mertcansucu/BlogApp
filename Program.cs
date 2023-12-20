using BlogApp.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlogContext>(options =>{//veritabı bağlantılarını yapacağım yeri yazdım => BlogContext.cs
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("sql_connection");
    /*
        sql_connection =>  bu bilgiyi appsettings.Development.json dan aldım:
    */
    options.UseSqlite(connectionString);
    //bu işlemlerden sonra BlogContext ile birleştirdiğim entityleri veritabına aktarıcam bunu migration ile yaptım
});

var app = builder.Build();

SeedData.TestVerileriniDoldur(app);//app aracılığıyla Services e ulaşıp içerindeki BlogContext bilgisini alıcam

app.MapGet("/", () => "Hello World!");

app.Run();
