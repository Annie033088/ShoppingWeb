using Pashamao.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace Pashamao.Repositories
{
    public class SqlRepository
    {
        protected string ConnStr
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString; }
        }

        protected User Userinfo { get; set; }


    }
}