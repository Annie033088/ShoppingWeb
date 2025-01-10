using System.ComponentModel.DataAnnotations;

namespace Pashamao.Models.Dto.MemberDto
{
    public class RequestCreateMemberDto
    {
        [Required(ErrorMessage = "帳號不得為空")]
        [RegularExpression("^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d]{8,20}$", ErrorMessage = "請輸入8~20位英文數字")]
        public string Account { get; set; }

        [Required(ErrorMessage = "密碼不得為空")]
        [RegularExpression("^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d]{8,20}$", ErrorMessage = "請輸入8~20位英文數字")]
        public string Pwd { get; set; }

        [StringLength(254, ErrorMessage = "信箱長度應該小於254")]
        [RegularExpression("^([a-zA-Z0-9.-]{1,64}@[a-zA-Z0-9.-]{1,253}\\.[a-zA-Z0-9]{2,})$", ErrorMessage = "請輸入正確的信箱格式")]
        public string Email { get; set; }

        [Range(1000000000, 9999999999, ErrorMessage = "請輸入10位數字，或保持為空")]
        public int? Phone { get; set; }

        [Required(ErrorMessage = "名字不得為空")]
        [RegularExpression("^.{1,50}$", ErrorMessage = "至多50字")]
        public string MemberName { get; set; }

        [RegularExpression("^.{0,25}$", ErrorMessage = "至多25字")]
        public string Nickname { get; set; }
    }
}