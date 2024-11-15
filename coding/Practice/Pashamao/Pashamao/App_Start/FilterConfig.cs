using System.Web;
using System.Web.Mvc;

namespace Pashamao
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters( GlobalFilterCollection filters )
        {
            filters.Add ( new HandleErrorAttribute () );
        }
    }
}
