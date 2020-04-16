using PFW_CW1.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PFW_CW1.Controllers
{
    public class CausesController : Controller
    {
        private readonly PFW_DBEntities db = new PFW_DBEntities();

        // GET: Causes
        public ActionResult Index()
        {
            var causes = db.causes.Include(c => c.members);
            return View(causes.ToList());
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
                return RedirectToAction("Index");
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
                return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}