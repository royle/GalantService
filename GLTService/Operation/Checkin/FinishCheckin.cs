using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql;
using MySql.Data.MySqlClient;
using Galant.DataEntity;
using GLTService.DBConnector;
using GLTService.Operation.BaseEntity;

namespace GLTService.Operation.Checkin
{
    public class FinishCheckin : BaseOperator
    {
        public FinishCheckin(DataOperator data) : base(data) 
        {
            this.paperOp = new BaseEntity.Paper(data);
        }

        private GLTService.Operation.BaseEntity.Paper paperOp;

        public void CheckinData(Galant.DataEntity.Result.FinishCheckin checkIn)
        {
            foreach (Galant.DataEntity.Paper p in checkIn.WorkDoneList)
            {
                Galant.DataEntity.Paper db_p = paperOp.SearchById(p.PaperId) as Galant.DataEntity.Paper;
                if (db_p.PaperSubStatus != PaperSubState.InTransit)
                {
                    throw new Galant.DataEntity.WCFFaultException(9001, "Data Out Of Date", "数据已过期,请返回刷新后再试。");
                }
                this.SaveStore(p);
                this.SaveCheckIn(p);
                this.UpdatePaperStatus(p);//更新状态
                this.UpdatePaperSalary(p);//修改配送费用
            }

            foreach (Galant.DataEntity.Paper  pc in checkIn.CheckinCollections)
            {
                this.paperOp.FinishCollections(pc);
            }
        }

        /// <summary>
        /// 修改库存
        /// </summary>
        /// <param name="p"></param>
        private void SaveStore(Galant.DataEntity.Paper p)
        {
            if (p.PaperSubStatus == PaperSubState.InTransit)
                return;
            GLTService.Operation.BaseEntity.Store storeOpe = new BaseEntity.Store(this.Operator);
            foreach (Galant.DataEntity.Package pack in p.Packages)
            {
                Galant.DataEntity.Store store = new Galant.DataEntity.Store() { EntityID=p.DeliverB.EntityId,ProductID=pack.Product.ProductId,ProductCount = (pack.Count *-1),Bound =1};
                storeOpe.AddNewData(store);//抵消配送员之前的实体库存
                
            }
            Galant.DataEntity.EventLog peventLog = new Galant.DataEntity.EventLog()
            {
                EntityID = this.Operator.EntityOperator == null ? null : this.Operator.EntityOperator.EntityId,
                AtStation = this.Operator.EntityOperator == null ? null : this.Operator.EntityOperator.CurerentStationID,
                PaperId = p.PaperId,
                InsertTime = DateTime.Now,
                RelationEntity = p.DeliverB.EntityId,
                EventType = "CKI-B",
                EventData = "订单归班"
            };
            this.AddEvent(peventLog);

            if (p.ReturnBulk != null)
            {
                foreach (Galant.DataEntity.Package pack in p.ReturnBulk)//抵消配送员返回的空桶
                {
                    Galant.DataEntity.Store store = new Galant.DataEntity.Store() { EntityID = p.DeliverB.EntityId, ProductID = pack.Product.ProductId, ProductCount = (pack.Count * -1), Bound = 0 };
                    storeOpe.AddNewData(store);
                    Galant.DataEntity.EventLog eventLog = new Galant.DataEntity.EventLog()
                    {
                        EntityID = this.Operator.EntityOperator == null ? null : this.Operator.EntityOperator.EntityId,
                        AtStation = this.Operator.EntityOperator == null ? null : this.Operator.EntityOperator.CurerentStationID, 
                        PaperId = p.PaperId, InsertTime = DateTime.Now, RelationEntity = p.DeliverB.EntityId, 
                        EventType = "CKI-B-BULK", EventData = "归班" + pack.Product.ReturnName + pack.Count + " 个，现金：" + pack.Amount 
                    };
                    this.AddEvent(eventLog);
                    if (p.ContactB.PayType == Galant.DataEntity.PayType.After)//抵消后付费客户的空桶库存
                    {
                        Galant.DataEntity.Store storeCustomer = new Galant.DataEntity.Store() { EntityID = p.ContactB.EntityId, ProductID = pack.Product.ProductId, ProductCount = (pack.Count * -1), Bound = 0 };
                        storeOpe.AddNewData(storeCustomer);
                    }
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="p"></param>
        private void SaveCheckIn(Galant.DataEntity.Paper p)
        {
            if (p.PaperSubStatus == PaperSubState.InTransit)
                return;
            GLTService.Operation.BaseEntity.PaperCheckin paperCheckin = new BaseEntity.PaperCheckin(this.Operator);
            if (p.ReturnCash > 0 || p.ReturnCash < 0)//记录订单返回的现金
            {
                Galant.DataEntity.PaperCheckin pCheckinCash = new Galant.DataEntity.PaperCheckin() { PaperId = p.PaperId, CheckinAmount = p.ReturnCash, CheckinType = CheckinType.Cash };
                paperCheckin.AddNewData(pCheckinCash);
            }

            if (p.ReturnTicket == null)
                return;
            
            foreach (Galant.DataEntity.Package pack in p.Packages)//记录订单返回的水票
            {
                Galant.DataEntity.PaperCheckin pCheckinTicket = new Galant.DataEntity.PaperCheckin() { PaperId = p.PaperId, CheckinType = CheckinType.Ticket, ProductId=pack.Product.ProductId,ProductCount=pack.Count };
                paperCheckin.AddNewData(pCheckinTicket);
            }
            
        }

        /// <summary>
        /// 修改归班订单的状态
        /// </summary>
        /// <param name="p"> 归班的订单</param>
        private void UpdatePaperStatus(Galant.DataEntity.Paper p)
        {
            Galant.DataEntity.PaperSubState? subStatus = p.PaperSubStatus == Galant.DataEntity.PaperSubState.NextActionAssured ? Galant.DataEntity.PaperSubState.FinishGood : p.PaperSubStatus;
            this.paperOp.FinishPaper(p, subStatus);
        }

        private void UpdatePaperSalary(Galant.DataEntity.Paper p)
        {
            string sqlUpdate = "update papers set salary = @salary where paper_id = @paper_id ";
            List<MySqlParameter> mps1 = new List<MySqlParameter>();
            mps1.Add(new MySqlParameter("@paper_id", p.PaperId));
            mps1.Add(new MySqlParameter("@salary", p.Salary));
            MySqlHelper.ExecuteNonQuery(this.Operator.myConnection, sqlUpdate, mps1.ToArray());
        }

        


    }
}