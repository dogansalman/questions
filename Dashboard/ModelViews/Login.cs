using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuestionsSYS.ModelViews
{
    public class Login
    {
        [Required]
        [StringLength(255)]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

    }
}