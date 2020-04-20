using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using PFW_CW_2.Models;

namespace PFW_CW_2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
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
            if (Session["crrUsername"] != null) return View("Index");

            return View();
        }


        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogIn(string usrName, string pssWd)
        {
            if (Session["crrUsername"] != null)
            {
                Session.Abandon();
                return View("Index");
            }

            if (usrName == null)
                ViewBag.InvalidUsername = "Invalid Username. Please make sure that the username field is valid.";
            var members = new MembersController().GetLoginDetails(usrName);
            if (members == null)
            {
                ViewBag.NotValidUser = "Invalid User. Please make sure that the username field is valid.";
            }
            else
            {
                if (members.passwd == pssWd.Trim())
                {
                    Session.Clear();
                    Session["crrUsername"] = members.email;
                    Session["crrUser"] = members.first_name + " " + members.last_name;
                    Session.Timeout = 15;
                    return Index();
                }

                if (ViewBag.Failedcount == null)
                    ViewBag.Failedcount = 0;
                ViewBag.Failedcount += 1;
            }

            return View();
        }

        public ActionResult SignUp()
        {
            if (Session["crrUsername"] != null)
            {
                Session.Abandon();
                return View("Index");
            }

            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(
            [Bind(Include = "email,passwd,first_name,last_name,fst_add,snd_add,trd_add,photo,memberType")]
            members members, HttpPostedFileBase photo)
        {
            if (Session["crrUsername"] != null)
            {
                Session.Abandon();
                return View("Index");
            }

            if (ModelState.IsValid)
            {
                if (new MembersController().GetLoginDetails(members.email) == null)
                {
                    if (photo != null)
                    {
                        var pic = Path.GetFileNameWithoutExtension(photo.FileName);
                        var ext = Path.GetExtension(photo.FileName);
                        var path = Path.Combine(
                            Server.MapPath("~/Content/img/profile/"), pic+ext);
                        if (System.IO.File.Exists(path))
                        {
                            pic += "_1";
                            path = Path.Combine(
                                Server.MapPath("~/Content/img/profile/"), pic+ext);
                        }
                        // file is uploaded
                        photo.SaveAs(path);
                        members.photo = pic + ext;
                    }
                    else
                    {
                        members.photo = "default.jpg";
                    }


                    var sqlResult = new MembersController().InsertNewMember(members);
                    if (sqlResult == 1)
                    {
                        Session.Clear();
                        Session["crrUsername"] = members.email;
                        Session["crrUser"] = members.first_name + " " + members.last_name;
                        Session.Timeout = 15;
                        return Index();
                    }

                    ViewBag.SQlError = "Member Registration Failed. " + sqlResult + " has been updated.";
                }
                else
                {
                    ViewBag.SQlError = "Existed Username/Email Address.";
                }
            }

            return View(members);
        }

        public ActionResult MyProfile()
        {
            if (Session["crrUsername"] == null)
            {
                Session.Abandon();
                return View("Index");
            }

            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult MyCauses()
        {
            if (Session["crrUsername"] == null)
            {
                Session.Abandon();
                return View("Index");
            }

            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult MyNewCause()
        {
            if (Session["crrUsername"] == null)
            {
                Session.Abandon();
                return View("Index");
            }

            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult ErrorPage()
        {
            return View();
        }
    }
}