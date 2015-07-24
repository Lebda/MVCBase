using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace IdenityHelp.Infrastructure
{
    public static class RoleNames
    {
        static RoleNames()
        {
            EditOperationUserNames = new string[] { c_architectRoleName, c_adminRoleName, c_dataMakerRoleName };
            EditOperationRoleNames = new string[] { c_architectRoleName, c_adminRoleName, c_dataMakerRoleName };
            DeleteOperationRoleNames = new string[] { c_architectRoleName, c_adminRoleName };
            CreateOperationRoleNames = new string[] { c_architectRoleName, c_adminRoleName };
            // ROLE
            CreateRoleRoleNames = new string[] { c_architectRoleName, c_adminRoleName };
            EditRoleRoleNames = new string[] { c_architectRoleName };
            DeleteRoleRoleNames = new string[] { c_architectRoleName };
            DetailRoleRoleNames = new string[] { c_architectRoleName, c_adminRoleName };
            // USER
            CreateUserRoleNames = new string[] { c_architectRoleName, c_adminRoleName };
            DetailUserRoleNames = new string[] { c_architectRoleName, c_adminRoleName };
            DeleteUserRoleNames = new string[] { c_architectRoleName, c_adminRoleName };
            EditUserRoleNames = new string[] { c_architectRoleName, c_adminRoleName }; 
        }

        #region MEMBERS
        #endregion
        
        #region ROLE NAMES
        public const string c_architectRoleName = "Architect";
        public const string c_adminRoleName = "Admin";
        public const string c_dataMakerRoleName = "DataMaker";
        public const string c_userRoleName = "User";
        #endregion
        
        #region PROPERTIES
        public static string[] EditOperationUserNames { get; private set; }
        public static string[] EditOperationRoleNames { get; private set; }
        public static string[] DeleteOperationRoleNames { get; private set; }
        public static string[] CreateOperationRoleNames { get; private set; }
        public static string[] CreateRoleRoleNames { get; private set; }
        public static string[] CreateUserRoleNames { get; private set; }
        public static string[] DetailRoleRoleNames { get; private set; }
        public static string[] DetailUserRoleNames { get; private set; }
        public static string[] EditRoleRoleNames { get; private set; }
        public static string[] EditUserRoleNames { get; private set; }
        public static string[] DeleteRoleRoleNames { get; private set; }
        public static string[] DeleteUserRoleNames { get; private set; }
        #endregion
        
        #region INTERFACE
        static public bool IsEditableRole(string roleName)
        {
            switch (roleName)
            {
                case c_architectRoleName:
                case c_adminRoleName:
                case c_dataMakerRoleName:
                case c_userRoleName:
                    return false;
                default:
                    return true;
            }
        }
        static public bool AllowedRoleNames(IList<string> roleNames, IPrincipal user)
        {
            return roleNames.Any(item => user.IsInRole(item));
        }
        #endregion
    }
}