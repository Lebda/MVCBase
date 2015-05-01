using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EFHelp.Abstract;

namespace EFHelp.Concrete
{
    public class ControllerHelper<T> : Controller where T : class
    {
        public ControllerHelper(IGenericRepository<T> repo)
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
        
        // GET: ControllerName
        protected ActionResult Index()
        {
            var test = m_repo.SelectAll();
            return View(test);
        }
        // GET: ControllerName/Details/5
        protected ActionResult Details(int? id)
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
        // GET: ControllerName/Create
        protected ActionResult Create()
        {
            return View();
        }

        // POST: ControllerName/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        protected ActionResult Create(T item, Action<object> callBack, object selectedID = null, Func<RedirectToRouteResult> redirection = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    m_repo.Insert(item);
                    m_repo.SaveChanges();
                    if (redirection == null)
                    {
                        return RedirectToAction("Index");
                    }
                    return redirection();
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
    }
}