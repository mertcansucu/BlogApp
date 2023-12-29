using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Models
{//entity(burda entity kullanmamamızın nedeni ekstra bilgilerinde olması) ve model arasındaki fark taşıyacak olduğumuz bilgileri taşıyacak olan bir sınıf
    public class LoginViewModel
    {
        [Required]//zorunlu alan
        [EmailAddress]
        [Display(Name = "Eposta")]
        public string? Email { get; set; }

        [Required]
        [StringLength(10,ErrorMessage ="{0} alanı en az {2} karakter uzunluğunda olmalıdır ve en çokta {1} olmalıdır.",MinimumLength =4)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string? Password { get; set; }
    }
}