using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Galant;
using GLTService.Operation.BaseEntity;

namespace GLTService.Operation
{
    public class Login:DataOperator
    {
        public Login():this(new DataOperator())
        { }

        public Login(DataOperator data):base(data)
        {
        }

        public Galant.DataEntity.AppStatusCach InitAppCach(DataOperator data, Galant.DataEntity.Entity staff)
        {
            Entity entity = new Entity();
            Route route = new Route();
            Galant.DataEntity.AppStatusCach cach = new Galant.DataEntity.AppStatusCach();
            cach.StaffCurrent = entity.Authorize(data, staff.Alias, staff.Password, true);
            cach.Entities = entity.GetAllAvailableEntitys(data);
            cach.Routes = route.GetAllRoutes(data);
            return cach;
        }

    }
}