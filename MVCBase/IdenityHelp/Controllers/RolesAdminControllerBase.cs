using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IdenityHelp.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace IdenityHelp.Controllers
{
    /// <summary>
    /// HttpContext.GetOwinContext() is null till ctor si done !
    /// </summary>
    public class RolesAdminControllerBase<TroleManager, Trole, TuserManager, Tuser, TviewModel> : Controller
        where Tuser : IdentityUser
        where Trole : IdentityRole
        where TroleManager : RoleManager<Trole>
        where TuserManager : UserManager<Tuser>
        where TviewModel : class
    {
        protected delegate void UpdateModelDelegate(TviewModel viewModel, Trole model);
        protected delegate void UpdateViewModelDelegate(Trole model, TviewModel viewModel);
        protected RolesAdminControllerBase(TuserManager userManager, TroleManager roleManager,
            Func<TviewModel> viewModelCreator, Func<Trole> modelCreator, UpdateModelDelegate modelUpdate, UpdateViewModelDelegate viewModelUpdate)
            : base()
        {
            m_userManager = userManager;
            m_roleManager = roleManager;
            m_viewModelCreator = viewModelCreator;
            m_modelCreator = modelCreator;
            m_updateModel = modelUpdate;
            m_updateViewModel = viewModelUpdate;
        }
        protected RolesAdminControllerBase(Func<TviewModel> viewModelCreator, Func<Trole> modelCreator, UpdateModelDelegate modelUpdate, UpdateViewModelDelegate viewModelUpdate)
        {
            m_viewModelCreator = viewModelCreator;
            m_modelCreator = modelCreator;
            m_updateModel = modelUpdate;
            m_updateViewModel = viewModelUpdate;
        }
        
        #region PRIVATE
        ActionResult RedirectionInternal(Func<RedirectToRouteResult> redirection = null)
        {
            if (redirection == null)
            {
                return RedirectToAction("Index");
            }
            return redirection();
        }
        #endregion
        
        #region MEMBERS
        private TuserManager m_userManager;
        private TroleManager m_roleManager;
        private readonly Func<TviewModel> m_viewModelCreator;
        private readonly Func<Trole> m_modelCreator;
        private readonly UpdateModelDelegate m_updateModel;
        private readonly UpdateViewModelDelegate m_updateViewModel;
        #endregion
        
        #region OwinContext
        protected TuserManager UserManagerBase
        {
            get
            {
                if (m_userManager == null)
                {
                    return m_userManager ?? HttpContext.GetOwinContext().GetUserManager<TuserManager>();
                }
                return m_userManager;
            }
            private set { m_userManager = value; }
        }
        protected TroleManager RoleManagerBase
        {
            get
            {
                if (m_roleManager == null)
                {
                    return m_roleManager ?? HttpContext.GetOwinContext().Get<TroleManager>();
                }
                return m_roleManager;
            }
            private set { m_roleManager = value; }
        }
        #endregion
        
        #region INDEX
        /// <summary>
        /// “The LINQ expression node type 'Invoke' is not supported in LINQ to Entities” - stumped!
        /// http://stackoverflow.com/questions/5284912/the-linq-expression-node-type-invoke-is-not-supported-in-linq-to-entities
        /// </summary>
        /// <param name="viewModelCreator">conventer to view model</param>
        /// <returns></returns>
        protected ActionResult IndexBase()
        {
            var query = RoleManagerBase.Roles
                                       .ToList()
                                       .Select((role) =>
                                       {
                                           var viewModel = m_viewModelCreator();
                                           m_updateViewModel(role, viewModel);
                                           return viewModel;
                                       });
            return View(query);
        }
        #endregion
        
        #region DETAILS
        // GET: ControllerName/Details/5
        protected async Task<ActionResult> DetailsBase(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManagerBase.FindByIdAsync(id);
            // Get the list of Users in this Role
            var users = new List<Tuser>();
            
            // Get the list of Users in this Role
            foreach (var user in UserManagerBase.Users.ToList())
            {
                if (await UserManagerBase.IsInRoleAsync(user.Id, role.Name))
                {
                    users.Add(user);
                }
            }
            
            ViewBag.Users = users;
            ViewBag.UserCount = users.Count();
            var viewModel = m_viewModelCreator();
            m_updateViewModel(role, viewModel);
            return View(viewModel);
        }
        #endregion
        
        #region EDIT CRUD
        // GET: /Roles/Edit/Admin
        protected async Task<ActionResult> EditBase(string id, Func<string, ViewResult> invalidRoleModificationView)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManagerBase.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            if (RoleNames.IsEditableRole(role.Name) || User.IsInRole(RoleNames.c_architectRoleName))
            {
                var viewModel = m_viewModelCreator();
                if (m_updateViewModel != null)
                {
                    m_updateViewModel(role, viewModel);
                }
                return View(viewModel);
            }
            return invalidRoleModificationView(String.Format("Only user in architect role can modificate {0} !", role.Name));
        }
        // POST: ControllerName/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        protected async Task<ActionResult> EditPostBase(string id, Func<string, ViewResult> invalidRoleModificationView, Func<RedirectToRouteResult> redirection = null, params string[] properties2Update)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManagerBase.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            if (RoleNames.IsEditableRole(role.Name) || User.IsInRole(RoleNames.c_architectRoleName))
            {
                var viewModel = m_viewModelCreator();
                if (TryUpdateModel(viewModel, "", properties2Update))
                {
                    try
                    {
                        if (m_updateModel != null)
                        {
                            m_updateModel(viewModel, role);
                        }
                        await RoleManagerBase.UpdateAsync(role);
                        return RedirectionInternal(redirection);
                    }
                    catch (DataException /* dex */)
                    {
                        //Log the error (uncomment dex variable name and add a line here to write a log.
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    }
                }
                return View(viewModel);
            }
            return invalidRoleModificationView("Only user in architect role can modificate architect role !");
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
        protected async Task<ActionResult> CreateBase(TviewModel viewModel, Func<RedirectToRouteResult> redirection = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = m_modelCreator();
                    if (m_updateModel != null)
                    {
                        m_updateModel(viewModel, model);   
                    }
                    var roleresult = await RoleManagerBase.CreateAsync(model);
                    if (!roleresult.Succeeded)
                    {
                        ModelState.AddModelError("", roleresult.Errors.First());
                        return View(viewModel);
                    }
                    return RedirectionInternal(redirection);
                }
            }
            catch (DataException)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            //SafeCall(callBack, item);
            return View(viewModel);
        }
        #endregion
        
        #region DELETE CRUD
        // GET: ControllerName/Delete/5
        protected async Task<ActionResult> DeleteBase(string id, Func<string, ViewResult> invalidRoleModificationView)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManagerBase.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            if (RoleNames.IsEditableRole(role.Name) || User.IsInRole(RoleNames.c_architectRoleName))
            {
                var viewModel = m_viewModelCreator();
                if (m_updateViewModel != null)
                {
                    m_updateViewModel(role, viewModel);
                }
                return View(viewModel);
            }
            return invalidRoleModificationView(String.Format("Only user in architect role can modificate {0} !", role.Name));
        }
        // POST: ControllerName/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedBase(
            string id,
            Func<string, ViewResult> invalidRoleModificationView,
            Func<RedirectToRouteResult> redirection = null)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var role = await RoleManagerBase.FindByIdAsync(id);
                if (role == null)
                {
                    return HttpNotFound();
                }
                if (RoleNames.IsEditableRole(role.Name) || User.IsInRole(RoleNames.c_architectRoleName))
                {
                    IdentityResult result = await RoleManagerBase.DeleteAsync(role);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First());
                        return View();
                    }
                    return RedirectionInternal(redirection);
                }
                return invalidRoleModificationView(String.Format("Only user in architect role can modificate {0} !", role.Name));
            }
            return View();
        }
        #endregion
    }
}