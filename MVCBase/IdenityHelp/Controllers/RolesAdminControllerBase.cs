using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using IdenityHelp.ViewModels.Roles;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdenityHelp.Controllers
{
    /// <summary>
    /// HttpContext.GetOwinContext() is null till ctor si done !
    /// </summary>
    public class RolesAdminControllerBase<Tuser, Trole, TroleViewModel> : Controller
        where Tuser : IdentityUser
        where Trole : IdentityRole
        where TroleViewModel : class
    {
        protected RolesAdminControllerBase()
        {
        }
        protected void AttachData(UserManager<Tuser> userManager, RoleManager<Trole> roleManager)
        {
            UserManagerBase = userManager;
            RoleManagerBase = roleManager;
        }

        #region MEMBERS
        private UserManager<Tuser> m_userManager;
        private RoleManager<Trole> m_roleManager;
        #endregion

        #region PROPERITES
        private UserManager<Tuser> UserManagerBase
        {
            get
            {
                if (m_userManager == null)
                {
                    throw new NotSupportedException("UserManager has to be set !");
                }
                return m_userManager;
            }
            set
            {
                m_userManager = value;
            }
        }
        private RoleManager<Trole> RoleManagerBase
        {
            get
            {
                if (m_roleManager == null)
                {
                    throw new NotSupportedException("RoleManager has to be set !");
                }
                return m_roleManager;
            }
            set
            {
                m_roleManager = value;
            }
        }
        #endregion


        #region INDEX
        /// <summary>
        /// “The LINQ expression node type 'Invoke' is not supported in LINQ to Entities” - stumped!
        /// http://stackoverflow.com/questions/5284912/the-linq-expression-node-type-invoke-is-not-supported-in-linq-to-entities
        /// </summary>
        /// <param name="viewModelCreator">conventer to view model</param>
        /// <returns></returns>
        protected ActionResult IndexBase(Expression<Func<Trole, TroleViewModel>> viewModelCreator)
        {
            var query = RoleManagerBase.Roles.Select(viewModelCreator);
            return View(query.ToList());
        }
        #endregion

        #region EDIT CRUD
        // GET: /Roles/Edit/Admin
        // (viewModel, role) => {}
        protected async Task<ActionResult> EditBase(string id, TroleViewModel roleModel, Action<TroleViewModel, Trole> callBack = null)
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
            if (callBack != null)
            {
                callBack(roleModel, role);   
            }
            return View(roleModel);
            //RoleViewModel roleModel = new RoleViewModel { Id = role.Id, Name = role.Name };

            //// Update the new Description property for the ViewModel:
            //roleModel.Description = role.Description;

            //return View(roleModel);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        protected async Task<ActionResult> EditBase([Bind(Include = "Name,Id,Description")] RoleViewModel roleModel)
        {
            if (ModelState.IsValid)
            {
                var role = await RoleManagerBase.FindByIdAsync(roleModel.Id);
                role.Name = roleModel.Name;

                // Update the new Description property:
                //role.Description = roleModel.Description;

                await RoleManagerBase.UpdateAsync(role);
                return RedirectToAction("Index");
            }
            return View();
        }
        #endregion
        //

        //
        // GET: /Roles/Details/5
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
            return View(role);
        }

        //
        // GET: /Roles/Create
        protected ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        //[HttpPost]
        //protected async Task<ActionResult> CreateBase(RoleViewModel roleViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var role = new Trole(roleViewModel.Name);
        //        // Save the new Description property:
        //        role.Description = roleViewModel.Description;
        //        var roleresult = await RoleManager.CreateAsync(role);
        //        if (!roleresult.Succeeded)
        //        {
        //            ModelState.AddModelError("", roleresult.Errors.First());
        //            return View();
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        //

        //
        // GET: /Roles/Delete/5
        protected async Task<ActionResult> DeleteBase(string id)
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
            return View(role);
        }

        //
        // POST: /Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        protected async Task<ActionResult> DeleteConfirmedBase(string id, string deleteUser)
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
                IdentityResult result = await RoleManagerBase.DeleteAsync(role);
                //if (deleteUser != null)
                //{
                //    result = await RoleManager.DeleteAsync(role);
                //}
                //else
                //{
                //    result = await RoleManager.DeleteAsync(role);
                //}
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
