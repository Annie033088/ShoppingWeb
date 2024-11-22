using System;

namespace Pashamao.Models
{
    [Serializable]
    public class SessionModel
    {
        public int UID { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }
    }
}