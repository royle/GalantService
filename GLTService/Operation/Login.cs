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
            List<Galant.DataEntity.BaseData> datas = entity.SearchByKeyId(id);
            if (datas != null && datas.Count > 0)
            {
                return (Galant.DataEntity.Entity)datas[0]; 
            }
            throw new Galant.DataEntity.WCFFaultException(124, "Error Permission", "用户名或密码错误");
            Galant.DataEntity.Entity enReturn = new Galant.DataEntity.Entity();
            
            return enReturn;

        }
    }
}