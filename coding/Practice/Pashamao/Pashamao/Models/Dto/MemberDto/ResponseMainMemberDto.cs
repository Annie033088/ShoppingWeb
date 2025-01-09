namespace Pashamao.Models.Dto.MemberDto
{
    public class ResponseMainMemberDto
    {
        public ResponseMainMemberDto(Member member)
        {
            MemberId = member.MemberId;
            Email = member.Email;
            Phone = member.Phone;
            MemberName = member.MemberName;
            Status = member.Status;
            Level = member.Level;
        }

        /// <summary>
        /// 會員Id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手機
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 等級
        /// </summary>
        public int Level { get; set; }
    }
}