using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Registration.Service.Models
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "密碼至少6個字元!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "兩次密碼輸入不一致")]
        public string ConfirmPassword { get; set; }
                
        [Required]
        public string Name { get; set; }

        public IEnumerable<SelectListItem> Role { get; set; }

        public int RoleId { get; set; }

    }
}
