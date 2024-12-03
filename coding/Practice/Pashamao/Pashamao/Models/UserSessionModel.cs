using System;

namespace Pashamao.Models
{
    [Serializable]
    public class UserSessionModel
    {
        public int UID { get; set; }

        public long UserPermission { get; set; }
    }
}