using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PFW_CW_2.Models;

namespace PFW_CW_2.Controllers
{
    public class CausesController : Controller
    {
        private readonly PFW_DBEntities db = new PFW_DBEntities();


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
                var causes = db.causes.Include(c => c.members);
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

        // GET: Causes/Create
        public ActionResult Create()
        {
            ViewBag.author = new SelectList(db.members, "email", "email");
            return View();
        }

        // POST: Causes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "causeId,author,title,startDate,endDate,status,description,photoLnk")]
            causes causes)
        {
            if (ModelState.IsValid)
            {
                db.causes.Add(causes);
                db.SaveChanges();
                return RedirectToAction("CauseAdminIndex", "Causes");
            }

            ViewBag.author = new SelectList(db.members, "email", "email", causes.author);
            return View(causes);
        }

        // GET: Causes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var causes = db.causes.Find(id);
            if (causes == null) return HttpNotFound();
            ViewBag.author = new SelectList(db.members, "email", "email", causes.author);
            return View(causes);
        }

        // POST: Causes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "causeId,author,title,startDate,endDate,status,description,photoLnk")]
            causes causes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(causes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CauseAdminIndex", "Causes");
            }

            ViewBag.author = new SelectList(db.members, "email", "email", causes.author);
            return View(causes);
        }

        // GET: Causes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var causes = db.causes.Find(id);
            if (causes == null) return HttpNotFound();
            return View(causes);
        }

        // POST: Causes/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var causes = db.causes.Find(id);
            db.causes.Remove(causes);
            db.SaveChanges();
            return RedirectToAction("CauseAdminIndex","Causes");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}