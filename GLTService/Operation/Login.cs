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

        public Galant.DataEntity.Entity LoginTest(string id)
        {
            Entity entity = new Entity(this.Operator);
            return (Galant.DataEntity.Entity)entity.SearchByKeyId(id)[0];

        }
    }
}