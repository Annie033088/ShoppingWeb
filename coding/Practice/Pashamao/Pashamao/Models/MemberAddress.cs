using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Models
{
    public class MemberAddress
    {
        public int AddressId { get; set; }
        public int MemberId {  get; set; }
        public string City {  get; set; }
        public string District { get; set; }
        public string Detail { get; set; }
        public string PostalCode { get; set; }
    }
}