using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IdenityHelp.Infrastrucutre;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace IdenityHelp.Controllers
{
    /// <summary>
    /// HttpContext.GetOwinContext() is null till ctor si done !
    /// </summary>
    public class UsersAdminControllerBase<TroleManager, Trole, TuserManager, Tuser> : Controller
        where Tuser : IdentityUser
        where Trole : IdentityRole
        where TroleManager : RoleManager<Trole>
        where TuserManager : UserManager<Tuser>
    {
        protected UsersAdminControllerBase(TuserManager userManager, TroleManager roleManager)
            : base()
        {
            m_userManager = userManager;
            m_roleManager = roleManager;
        }
        protected UsersAdminControllerBase()
        {
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
        void RemoveArchitectRole(IList<string> roleNames)
        {
            roleNames.ToList().RemoveAll(item => item == RoleNames.c_architectRoleName);
        }
        #endregion
        
        #region ROLES FUNCTIONS
        protected IList<Trole> GetRoles4Aplication()
        {
            var roles = RoleManagerBase.Roles.ToList();
            roles.RemoveAll(item => item.Name == RoleNames.c_architectRoleName);               
            return roles;
        }
        protected IList<string> GetRoles4User(string id)
        {
            if (id == null)
            {
                return null;
            }
            var user = UserManagerBase.FindById(id);
            if (user == null)
            {
                return null;
            }
            var userRoles = UserManagerBase.GetRoles(user.Id);
            return userRoles;
        }
        #endregion
        
        #region MEMBERS
        private TuserManager m_userManager;
        private TroleManager m_roleManager;
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
        /// <returns>@model IEnumerable<Tuser></returns>
        protected async Task<ActionResult> IndexBase<TviewModel>(Func<Tuser, IList<string>, TviewModel> createAndUpdateViewModel)
        {
            var queryUser = await UserManagerBase.Users.ToListAsync();
            var query =
                queryUser.Select((item) =>
                {
                    var viewModel = createAndUpdateViewModel(item, GetRoles4User(item.Id));
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
            var user = await UserManagerBase.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleNames = await UserManagerBase.GetRolesAsync(user.Id);
            
            return View(user);
        }
        #endregion
        
        #region EDIT CRUD
        // GET: /Roles/Edit/Admin
        protected async Task<ActionResult> EditBase<TviewModel>(string id, Func<Tuser, IList<string>, TviewModel> createAndUpdateViewModel)
            where TviewModel : class
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManagerBase.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            
            var userRoles = await UserManagerBase.GetRolesAsync(user.Id);
            RemoveArchitectRole(userRoles);
            
            return View(createAndUpdateViewModel(user, userRoles));
        }
        // POST: ControllerName/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        protected async Task<ActionResult> EditPostBase<TviewModel>(
            string id,
            Action<TviewModel, Tuser> updateModel,
            Func<TviewModel> viewModelCreator,
            string[] selectedRole,
            Func<RedirectToRouteResult> redirection = null,
            params string[] properties2Update)
            where TviewModel : class
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManagerBase.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var viewModel = viewModelCreator();
            if (TryUpdateModel(viewModel, "", properties2Update))
            {
                try
                {
                    if (updateModel != null)
                    {
                        updateModel(viewModel, user);
                    }
                    //await UserManagerBase.UpdateAsync(user); // shloud it be called ?
                    
                    var userRoles = await UserManagerBase.GetRolesAsync(user.Id);
                    
                    selectedRole = selectedRole ?? new string[] { };
                    
                    RemoveArchitectRole(selectedRole);
                    
                    var result = await UserManagerBase.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());
                    
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First());
                        return View(viewModel);
                    }
                    result = await UserManagerBase.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());
                    
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First());
                        return View(viewModel);
                    }
                    
                    return RedirectionInternal(redirection);
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            ModelState.AddModelError("", "Something failed.");
            return View(viewModel);
        }
        #endregion
        
        #region CREATE CRUD
        // GET: ControllerName/Create
        protected async Task<ActionResult> CreateBase()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManagerBase.Roles.ToListAsync(), "Name", "Name");
            return View();
        }
        // POST: ControllerName/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        protected async Task<ActionResult> CreateBase<TviewModel>(
            TviewModel viewModel,
            string password,
            Func<TviewModel, Tuser> updateAndCreateModel,
            string[] selectedRoles,
            Func<RedirectToRouteResult> redirection = null)
            where TviewModel : class
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = updateAndCreateModel(viewModel);
                    
                    var adminresult = await UserManagerBase.CreateAsync(user, password);
                    
                    //Add User to the selected Roles 
                    if (adminresult.Succeeded)
                    {
                        if (selectedRoles != null)
                        {
                            var result = await UserManagerBase.AddToRolesAsync(user.Id, selectedRoles);
                            if (!result.Succeeded)
                            {
                                ModelState.AddModelError("", result.Errors.First());
                                ViewBag.RoleId = new SelectList(await RoleManagerBase.Roles.ToListAsync(), "Name", "Name");
                                return View();
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", adminresult.Errors.First());
                        ViewBag.RoleId = new SelectList(RoleManagerBase.Roles, "Name", "Name");
                        return View();
                    }
                    return RedirectToAction("Index");
                }
                ViewBag.RoleId = new SelectList(RoleManagerBase.Roles, "Name", "Name");
                return RedirectionInternal(redirection);
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
        protected async Task<ActionResult> DeleteBase(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManagerBase.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        // POST: ControllerName/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedBase(string id, Func<RedirectToRouteResult> redirection = null)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var user = await UserManagerBase.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                IdentityResult result = await UserManagerBase.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectionInternal(redirection);
            }
            return View();
        }
        #endregion
    }
}