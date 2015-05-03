using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EFHelp.Abstract;

namespace EFHelp.Concrete.ControllerHelp
{
    public class ConrollerBase<T> : Controller where T : class
    {
        public ConrollerBase(IGenericRepository<T> repo)
        {
            m_repo = repo;
        }
        
        #region MEMBERS
        protected readonly IGenericRepository<T> m_repo;
        #endregion
        
        #region PROTECTED
        protected Trepo Repository<Trepo>() where Trepo : class, IGenericRepository<T>
        {
            return m_repo as Trepo;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_repo.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region PRIVATE 
        ActionResult RedirectionInternal(Func<RedirectToRouteResult> redirection = null)
        {
            if (redirection == null)
            {
                return RedirectToAction("Index");
            }
            return redirection();
        }
        void SafeCall(Action job)
        {
            if (job == null)
            {
                return;
            }
            job();
        }
        void SafeCall(Action<T> job, T item)
        {
            if (job == null)
            {
                return;
            }
            job(item);
        }
        #endregion

        #region EDIT CRUD
        // GET: ControllerName/Edit/5
        protected ActionResult EditBase(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = m_repo.SelectByID(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        // POST: ControllerName/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        protected ActionResult EditPostBase(int? id, Func<RedirectToRouteResult> redirection = null, Action<T> imageUpdate = null, params string[] properties2Update)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item2Update = m_repo.SelectByID(id);
            if (TryUpdateModel(item2Update, "", properties2Update))
            {
                try
                {
                    SafeCall(imageUpdate, item2Update);
                    m_repo.SaveChanges();
                    return RedirectionInternal(redirection);
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(item2Update);
        }
        #endregion

        #region CREATE CRUD
        // GET: ControllerName/Create
        protected ActionResult CreateBase()
        {
            return View();
        }

        // POST: ControllerName/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        protected ActionResult CreateBase(T item, Action<object> callBack, object selectedID = null, Func<RedirectToRouteResult> redirection = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    m_repo.Insert(item);
                    m_repo.SaveChanges();
                    return RedirectionInternal(redirection);
                }
            }
            catch (DataException dex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            if (callBack != null)
            {
                callBack(selectedID);
            }
            return View(item);
        }
        #endregion
     
        // GET: ControllerName
        protected ActionResult IndexBase()
        {
            var test = m_repo.SelectAll();
            return View(test);
        }
        // GET: ControllerName/Details/5
        protected ActionResult DetailsBase(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = m_repo.SelectByID(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
    }
}