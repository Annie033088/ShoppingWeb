using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Models
{
    public class User
    {
        public int UID { get; set; }
        public string Account { get; set; }
        public string Hash { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public int RoleId { get; set; }

    }
}