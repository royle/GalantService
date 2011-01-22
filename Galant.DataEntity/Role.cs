using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galant.DataEntity
{
    public enum RoleType
    {   }
    public class Role:BaseData
    {
        private int roleId;
        public int RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        private Entity station;
        public Entity Station
        {
            get { return station; }
            set { station = value; }
        }

        private RoleType roleType;
        public RoleType RoleType
        {
            get { return roleType; }
            set { roleType = value; }
        }
    }
}
