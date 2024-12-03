namespace Pashamao.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Account { get; set; }
        public string Pwd { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string SessionId { get; set; }
    }
}