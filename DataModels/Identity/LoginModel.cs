using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Identity
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı boş olamaz!")]
        [DataType(DataType.Text, ErrorMessage = "Lütfen geçerli bir kullanıcı adı giriniz!")]
        [Display(Name = "Kullanıcı Adı")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Şifre boş olamaz!")]
        [DataType(DataType.Password, ErrorMessage = "Lütfen geçerli bir şifre giriniz!")]
        [Display(Name = "Şifre")]
        public string? Password { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz!")]
        [Display(Name = "E-Posta Adresi")]
        public string? Email { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool? RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
