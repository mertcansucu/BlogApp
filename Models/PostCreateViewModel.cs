using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Models
{//entity(burda entity kullanmamamızın nedeni ekstra bilgilerinde olması) ve model arasındaki fark taşıyacak olduğumuz bilgileri taşıyacak olan bir sınıf
    public class PostCreateViewModel
    {
        [Required]//zorunlu alan
        [Display(Name = "Başlık")]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "İçerik")]
        public string? Content { get; set; }

        [Required]
        [Display(Name = "Url")]
        public string? Url { get; set; }
    }
}