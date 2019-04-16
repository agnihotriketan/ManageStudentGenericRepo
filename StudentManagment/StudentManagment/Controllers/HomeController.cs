using System.Web.Mvc;

namespace StudentManagment.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            GetRouteData();

            return View();
        }

        private void GetRouteData()
        {
            string area = Request.RequestContext.RouteData.DataTokens.ContainsKey("area") ? Request.RequestContext.RouteData.DataTokens["area"].ToString() : "";
            string controller = Request.RequestContext.RouteData.Values["controller"].ToString();
            string action = Request.RequestContext.RouteData.Values["action"].ToString();

            ViewBag.RouteDataInfo = "Area-" + area + "Controller-" + controller + "Action-" + action;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


       
    }
}
