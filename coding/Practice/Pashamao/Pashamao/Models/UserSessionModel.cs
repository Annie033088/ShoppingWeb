using System;

namespace Pashamao.Models
{
    [Serializable]
    public class UserSessionModel
    {
        public int UserId { get; set; }

        public long UserPermission { get; set; }
    }
}