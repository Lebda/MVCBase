using System;
using System.Collections.Generic;
using System.Linq;

namespace IdenityHelp.Infrastructure
{
    public class RoleNamesHolder
    {
        public RoleNamesHolder(params string[] roleNames)
        {
            if (roleNames == null)
            {
                m_roleNames = new List<string>();
            }
            else
            {
                m_roleNames = roleNames.ToList();
            }
        }

        #region MEMBERS
        readonly IList<string> m_roleNames;
        #endregion

        #region PROPERTIES
        public IList<string> RoleNames
        {
            get { return m_roleNames; }
        }
        #endregion

    }
}
