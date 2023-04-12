using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjecPRN.Models
{
    public partial class Person
    {
        public Person()
        {
            Rates = new HashSet<Rate>();
        }

        public int PersonId { get; set; }

        [Required(ErrorMessage = "Tên người dùng không được để trống !!!")]
        [Display(Name = "Fullname")]
        public string Fullname { get; set; }


        [Required(ErrorMessage = "Giới tính không được để trống !!!")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }


        [Required(ErrorMessage = "EMail  không được để trống !!!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu  không được để trống !!!")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public int? Type { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Rate> Rates { get; set; }
    }
}
