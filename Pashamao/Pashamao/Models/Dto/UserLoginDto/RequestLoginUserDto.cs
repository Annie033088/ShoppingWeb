using System.ComponentModel.DataAnnotations;

namespace Pashamao.Models.Dto.UserLoginDto
{
    public class RequestLoginUserDto
    {
        [Required(ErrorMessage = "帳號不得為空")]
        [RegularExpression("^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d]{8,20}$", ErrorMessage = "請輸入8~20位英文數字")]
        public string Account { get; set; }

        [Required(ErrorMessage = "密碼不得為空")]
        [RegularExpression("^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d]{8,20}$", ErrorMessage = "請輸入8~20位英文數字")]
        public string Pwd { get; set; }

    }
}
