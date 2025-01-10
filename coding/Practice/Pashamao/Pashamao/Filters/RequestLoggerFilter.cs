using NLog;
using Pashamao.Models;
using System;
using System.Web;
using System.Web.Mvc;


namespace Pashamao.Filters
{
    public class RequestLoggerFilter: ActionFilterAttribute
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string controllerName = filterContext.RouteData.Values["controller"].ToString();
                string actionName = filterContext.RouteData.Values["action"].ToString();
                logger.Trace($"Executing Controller: {controllerName}, Action: {actionName}");
                base.OnActionExecuting(filterContext);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                string controllerName = filterContext.RouteData.Values["controller"].ToString();
                string actionName = filterContext.RouteData.Values["action"].ToString();
                logger.Trace($"Executed Controller: {controllerName}, Action: {actionName}");
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }
    }
}
