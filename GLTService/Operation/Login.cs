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
            Entity entity = new Entity(data);
            Route route = new Route(data);
            Product product = new Product(data);
            Galant.DataEntity.Production.Search pSearch = new Galant.DataEntity.Production.Search();            
            Galant.DataEntity.AppStatusCach cach = new Galant.DataEntity.AppStatusCach();
            cach.StaffCurrent = entity.Authorize(data, staff.Alias, staff.Password, true);
            cach.Entities = entity.GetAllAvailableEntitys(data);
            cach.Routes = route.GetAllRoutes(data);
            cach.Products = product.SearchProductes(data, pSearch);
            return cach;
        }

    }
}