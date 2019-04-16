using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace StudentManagment
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine()); 
        }
        protected void Application_Error()
        {
            var obj = Server.GetLastError();
        }

        protected void Application_EndRequest()
        {
            var statusCode = HttpContext.Current.Response.StatusCode;

            switch (statusCode)
            {
                case 400:
                    Response.Clear();
                    Response.Redirect("Error");
                    //bad request
                    break;
                case 500:
                    Response.Clear();
                    Response.Redirect("Error");
                    //Server Error
                    break;
                case 404:
                    Response.Clear();
                    Response.Redirect("Error");
                    //Page not found  
                    break;

                default:

                    break;
            }

        }
    }
}