using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PFW_CW_2.Models;

namespace PFW_CW_2.Controllers
{
    public class CausesController : Controller
    {
        private readonly PFW_DBEntities db = new PFW_DBEntities();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult RetractResult(int? id)
        {
            if (Session["crrUsername"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            else
            {
                if (id != null && db.causes.Find(id)!=null)
                {
                    causes causes = db.causes.Find(id);
                    causes.status = -1;
                    db.Entry(causes).State = EntityState.Modified;
                    var sqlResult = db.SaveChanges();
                    if (sqlResult == 1)
                    {
                        TempData["SQlMsg"] = "Cause Retraction Succeeded. " + sqlResult + " record has been updated.";
                        return RedirectToAction("CauseAdminIndex", "Causes");
                    }

                    TempData["SQLError"] = "Cause Retraction Failed. " + sqlResult + "record has been updated.";
                    return RedirectToAction("CauseAdminIndex", "Causes");
                }
                else
                {
                    TempData["SQLError"] = "Invalid Id. Cause Retraction Failed.";
                    return RedirectToAction("CauseAdminIndex", "Causes");
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult CreateNewCause(
            [Bind(Include = "causeId,author,title,startDate,endDate,status,description,photoLnk")]
            causes causes, HttpPostedFileBase photoLnk)
        {
            if (Session["crrUsername"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (new MembersController().GetLoginDetails(Session["crrUsername"].ToString()) != null)
                    {
                        if (photoLnk != null)
                        {
                            var pic = Path.GetFileNameWithoutExtension(photoLnk.FileName);
                            var ext = Path.GetExtension(photoLnk.FileName);
                            var path = Path.Combine(
                                Server.MapPath("~/Content/img/cause/"), pic + ext);
                            if (System.IO.File.Exists(path))
                            {
                                pic += "_1";
                                path = Path.Combine(
                                    Server.MapPath("~/Content/img/cause/"), pic + ext);
                            }

                            // file is uploaded
                            photoLnk.SaveAs(path);
                            causes.photoLnk = pic + ext;
                        }
                        else
                        {
                            causes.photoLnk = "default.jpg";
                        }

                        db.causes.Add(causes);
                        var sqlResult = db.SaveChanges();
                        if (sqlResult == 1)
                        {
                            TempData["SQlMsg"] = "Cause Creation Succeeded. " + sqlResult + " record has been updated.";
                            return RedirectToAction("MyNewCause", "Home");
                        }

                        TempData["SQLError"] = "Cause Creation Failed. " + sqlResult + "record has been updated.";
                        return RedirectToAction("MyNewCause", "Home");
                    }
                    else
                    {
                        //Invalid user ( Possibility of an attack)
                        TempData["SQLError"] = "Invalid User. Cause Creation Failed. Please Log In and try again.";
                        return RedirectToAction("MyNewCause", "Home");
                    }
                }
                TempData["SQLError"] = "Invalid Model. Cause Creation Failed.";
                return RedirectToAction("MyNewCause", "Home");
            }
        }


        public IEnumerable<causes> GetPartialCauses(int loadItemNum)
        {
            return db.causes.Where(c => c.status != 0 && c.status != -1).Include(c => c.members)
                .OrderByDescending(causes => causes.startDate).Take(loadItemNum)
                .ToList();
        }

        public PartialViewResult PartialCauseResult(int loadItemNum)
        {
            return PartialView("CausePartialView", GetPartialCauses(loadItemNum));
        }


        // GET: Causes


        public ActionResult CauseAdminIndex()
        {
            if (Session["crrUsername"] == null) return new HomeController().Index();
            var member = new MembersController().GetLoginDetails(Session["crrUsername"]);

            if (member != null && member.memberType == 1)
            {
                var causes = db.causes.Include(c => c.members).OrderByDescending(c => c.startDate)
                    .ToList(); ;
                return View(causes.ToList());
            }

            Session.Abandon();
            return new HomeController().Index();
        }

        // GET: Causes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var causes = db.causes.Find(id);
            if (causes == null) return HttpNotFound();
            return View(causes);
        }

       
        
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}