using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PFW_CW_2.Models;

namespace PFW_CW_2.Controllers
{
    public class PledgesController : Controller
    {
        private readonly PFW_DBEntities db = new PFW_DBEntities();

        // GET: Pledges
        public ActionResult Index()
        {
            var pledges = db.pledges.Include(p => p.causes).Include(p => p.members);
            return View(pledges.ToList());
        }

        // GET: Pledges/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var pledges = db.pledges.Find(id);
            if (pledges == null) return HttpNotFound();
            return View(pledges);
        }

        // GET: Pledges/Create
        public ActionResult Create()
        {
            ViewBag.causeId = new SelectList(db.causes, "causeId", "author");
            ViewBag.memberId = new SelectList(db.members, "email", "passwd");
            return View();
        }

        // POST: Pledges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "causeId,memberId,date")]
            pledges pledges)
        {
            if (ModelState.IsValid)
            {
                db.pledges.Add(pledges);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.causeId = new SelectList(db.causes, "causeId", "author", pledges.causeId);
            ViewBag.memberId = new SelectList(db.members, "email", "passwd", pledges.memberId);
            return View(pledges);
        }

        // GET: Pledges/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var pledges = db.pledges.Find(id);
            if (pledges == null) return HttpNotFound();
            ViewBag.causeId = new SelectList(db.causes, "causeId", "author", pledges.causeId);
            ViewBag.memberId = new SelectList(db.members, "email", "passwd", pledges.memberId);
            return View(pledges);
        }

        // POST: Pledges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "causeId,memberId,date")]
            pledges pledges)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pledges).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.causeId = new SelectList(db.causes, "causeId", "author", pledges.causeId);
            ViewBag.memberId = new SelectList(db.members, "email", "passwd", pledges.memberId);
            return View(pledges);
        }

        // GET: Pledges/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var pledges = db.pledges.Find(id);
            if (pledges == null) return HttpNotFound();
            return View(pledges);
        }

        // POST: Pledges/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var pledges = db.pledges.Find(id);
            db.pledges.Remove(pledges);
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