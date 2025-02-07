namespace Pashamao.Models
{
    public class Member
    {
        /// <summary>
        /// 會員Id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手機
        /// </summary>
        public int Phone { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 錯號
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 累計點數
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// 等級
        /// </summary>
        public int Level { get; set; }
    }
}