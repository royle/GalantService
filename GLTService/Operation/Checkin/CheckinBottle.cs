using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLTService.Operation.BaseEntity;

namespace GLTService.Operation.Checkin
{
    public class CheckinBottle : BaseOperator
    {
        public CheckinBottle(DataOperator data)
            : base(data) { }

        public CheckinBottle()
            : base()
        { }

        public Galant.DataEntity.CheckinBottle Checkin(Galant.DataEntity.CheckinBottle p)
        {
            if (p.ReturnBulk != null)
            {
                GLTService.Operation.BaseEntity.Store storeOpe = new BaseEntity.Store(this.Operator);
                foreach (Galant.DataEntity.Package pack in p.ReturnBulk)//抵消配送员返回的空桶
                {
                    Galant.DataEntity.Store store = new Galant.DataEntity.Store() { EntityID = p.Holder.EntityId, ProductID = pack.Product.ProductId, ProductCount = (pack.Count * -1), Bound = 0 };
                    storeOpe.AddNewData(store);
                    Galant.DataEntity.EventLog eventLog = new Galant.DataEntity.EventLog()
                    {
                        EntityID = this.Operator.EntityOperator == null ? null : this.Operator.EntityOperator.EntityId,
                        AtStation = this.Operator.EntityOperator == null ? null : this.Operator.EntityOperator.CurerentStationID,
                        PaperId = "checkin",
                        InsertTime = DateTime.Now,
                        RelationEntity = p.Holder.EntityId,
                        EventType = "CKI-B-BULK",
                        EventData = "归班" + pack.Product.ReturnName + pack.Count + " 个，现金：" + pack.Amount
                    };
                    this.AddEvent(eventLog);
                    if (p.Holder.PayType == Galant.DataEntity.PayType.After)//抵消后付费客户的空桶库存
                    {
                        Galant.DataEntity.Store storeCustomer = new Galant.DataEntity.Store() { EntityID = p.Holder.EntityId, ProductID = pack.Product.ProductId, ProductCount = (pack.Count * -1), Bound = 0 };
                        storeOpe.AddNewData(storeCustomer);
                    }
                }
            }
            return p;
        }

    }
}