using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Pashamao.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage ="帳號不得為空")]
        [RegularExpression ( "^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d]{8,20}$", ErrorMessage = "請輸入8~20位英文數字" )]
        public string Username { get; set; }

        [Required ( ErrorMessage = "密碼不得為空" )]
        [RegularExpression ( "^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d]{8,20}$", ErrorMessage = "請輸入8~20位英文數字" )]
        public string UserPwd { get; set; }

    }
}
