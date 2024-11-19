using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Models
{
    public class User
    {
        public int UID { get; set; }
        public int Account { get; set; }
        public int Hash { get; set; }
        public int Name { get; set; }
        public int Status { get; set; }
        public int RoleId { get; set; }

    }
}