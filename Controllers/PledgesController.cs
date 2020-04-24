using System.Web.Mvc;
using PFW_CW_2.Models;

namespace PFW_CW_2.Controllers
{
    public class PledgesController : Controller
    {
        private readonly PFW_DBEntities db = new PFW_DBEntities();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult PledgeAction([Bind(Include = "causeId,memberId,date")]
            pledges pledges)
        {
            if (Session["crrUsername"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            if (ModelState.IsValid)
            {
                if (db.pledges.Find(pledges.causeId,pledges.memberId)==null)
                {
                    db.pledges.Add(pledges);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Causes", new { id = pledges.causeId });
                }
                TempData["SQLError"] = "Duplicated signature recognised. You can only pledge your support once.";
                return RedirectToAction("Details", "Causes", new {id = pledges.causeId});
            }

            TempData["AuthenticationError"] = "Your login details are invalid. Please login again to proceed.";
            return RedirectToAction("Details", "Causes", new { id = pledges.causeId});
        }

       
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}