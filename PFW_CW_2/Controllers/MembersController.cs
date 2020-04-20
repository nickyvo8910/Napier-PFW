using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PFW_CW_2.Models;

namespace PFW_CW_2.Controllers
{
    public class MembersController : Controller
    {
        private readonly PFW_DBEntities db = new PFW_DBEntities();


        public int InsertNewMember(members members)
        {
            db.members.Add(members);
            return db.SaveChanges();
        }

        //Get Member Login Details
        public members GetLoginDetails(object usrName)
        {
            if (usrName != null)
            {
                var members = db.members.Find(usrName);
                if (members != null) return members;
            }

            return null;
        }

        // GET: Members
        public ActionResult Index()
        {
            return View(db.members.ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var members = db.members.Find(id);
            if (members == null) return HttpNotFound();
            return View(members);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "email,passwd,first_name,last_name,fst_add,snd_add,trd_add,photo")]
            members members)
        {
            if (ModelState.IsValid)
            {
                db.members.Add(members);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(members);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var members = db.members.Find(id);
            if (members == null) return HttpNotFound();
            return View(members);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "email,passwd,first_name,last_name,fst_add,snd_add,trd_add,photo")]
            members members)
        {
            if (ModelState.IsValid)
            {
                db.Entry(members).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(members);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var members = db.members.Find(id);
            if (members == null) return HttpNotFound();
            return View(members);
        }

        // POST: Members/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var members = db.members.Find(id);
            db.members.Remove(members);
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