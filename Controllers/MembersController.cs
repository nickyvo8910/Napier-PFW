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

        
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}