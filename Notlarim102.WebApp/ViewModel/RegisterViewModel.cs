using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Notlarim102.WebApp.ViewModel
{
    public class RegisterViewModel
    {
        [DisplayName("Kullanıcı Adı2"), Required(ErrorMessage = "{0} Alanı boş gecilemez"), StringLength(30, ErrorMessage = "{0} max.{1} karakter olmali.")]
        public string Username { get; set; }

        [DisplayName("Email"), Required(ErrorMessage = "{0} Alanı boş gecilemez"), StringLength(100, ErrorMessage = "{0} max.{1} karakter olmali."), EmailAddress(ErrorMessage = "{0} alanı için geçerli bir email adresi giriniz.")]
        public string Email { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} Alanı boş gecilemez"), DataType(DataType.Password), StringLength(30, ErrorMessage = "{0} max.{1} karakter olmali.")]
        public string Password { get; set; }
        [DisplayName("Şifre tekrarı"), Required(ErrorMessage = "{0} Alanı boş gecilemez"), DataType(DataType.Password), StringLength(30, ErrorMessage = "{0} max.{1} karakter olmali."),
            Compare("Password", ErrorMessage = "{0} ile {1} uyuşmuyor.")]
        public string RePassword { get; set; }
    }
}