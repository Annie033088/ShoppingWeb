using System;

namespace Pashamao.Models
{
    [Serializable]
    public class UserSessionModel
    {
        public int UID { get; set; }
        public int RoleId { get; set; }
    }
}