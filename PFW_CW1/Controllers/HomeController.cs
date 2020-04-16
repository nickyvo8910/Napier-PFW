using System;
using System.Web.Mvc;

namespace PFW_CW1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var causesController = new CausesController();
            return causesController.Index();
        }

        public ActionResult About()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult MoreInfo(int causeID)
        {
            return View();
        }

        public ActionResult LogIn()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SignUp()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult MyProfile()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult MyCauses()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult MyNewCause()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult ErrorPage()
        {
            return View();
        }
    }
}