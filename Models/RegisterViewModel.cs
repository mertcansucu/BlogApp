using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Models
{//entity(burda entity kullanmamamızın nedeni ekstra bilgilerinde olması) ve model arasındaki fark taşıyacak olduğumuz bilgileri taşıyacak olan bir sınıf
    public class RegisterViewModel
    {
        [Required]//zorunlu alan
        [Display(Name = "Kullanıcı Adı")]
        public string? UserName { get; set; }

        [Required]//zorunlu alan
        [Display(Name = "Ad Soyad")]
        public string? Name { get; set; }

        [Required]//zorunlu alan
        [EmailAddress]
        [Display(Name = "Eposta")]
        public string? Email { get; set; }
        
        [Display(Name ="Resim")]
        public string Image{ get; set; } = string.Empty;

        [Required]
        [StringLength(10,ErrorMessage ="{0} alanı en az {2} karakter uzunluğunda olmalıdır ve en çokta {1} olmalıdır.",MinimumLength =4)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Parolanız eşleşmiyor")]
        [Display(Name = "Parola Tekarar")]
        public string? ConfirmPassword { get; set; }
    }
}