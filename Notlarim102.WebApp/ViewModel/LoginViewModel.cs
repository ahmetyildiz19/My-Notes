using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Notlarim102.WebApp.ViewModel
{
    public class LoginViewModel
    {
        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} Alanı boş geçilemez."), StringLength(30, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Username { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} Alanı boş geçilemez."), DataType(DataType.Password), StringLength(30, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Password { get; set; }
    }

}