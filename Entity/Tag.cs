using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Entity
{
    public enum TagColors{//bunu yapmamın nedeni her bir etiketin kendine özgü rengi olsun diye
        primary,danger,warning,success,secondary
    }
    /*
        bunu ekledlğim için veri tabanını güncellemem lazım:
        dotnet ef migrations add UpdateColumnTagColor
        dotnet ef database drop --force
        dotnet watch run
    */
    public class Tag
    {
        public int TagId { get; set; }
        public string? Text { get; set; }
        public string? Url { get; set; }

        public TagColors? Color { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();//Her etiket birden fazla postla ilgili olabileceği için liste şeklinde bağladım
    }
}