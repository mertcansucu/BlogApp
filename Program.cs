using BlogApp.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

SeedData.TestVerileriniDoldur(app);//app aracılığıyla Services e ulaşıp içerindeki BlogContext bilgisini alıcam

app.MapGet("/", () => "Hello World!");

app.Run();
