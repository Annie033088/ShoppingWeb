using System.ComponentModel.DataAnnotations;

namespace Pashamao.Models.Dto.UserDto
{
    public class RequestCreateUserDto
    {
        [Required(ErrorMessage = "帳號不得為空")]
        [RegularExpression("^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d]{8,20}$", ErrorMessage = "請輸入8~20位英文數字")]
        public string Account { get; set; }

        [Required(ErrorMessage = "密碼不得為空")]
        [RegularExpression("^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d]{8,20}$", ErrorMessage = "請輸入8~20位英文數字")]
        public string Pwd { get; set; }

        [RegularExpression("^.{0,10}$", ErrorMessage = "至多十位數")]
        public string Name { get; set; }

        public string DropDownRole { get; set; }
    }
}