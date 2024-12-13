using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Models
{
    public class Member
    {
        public int MemberId { get; set; }

        public string Acct { get; set; }

        public string Pwd { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string MemberName { get; set; }

        public string Nickname { get; set; }

        public bool Status { get; set; }

        public int Points { get; set; }

        public int Level { get; set; }
    }
}