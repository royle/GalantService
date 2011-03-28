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

        private void CheckinData(Galant.DataEntity.Result.FinishCheckin checkIn)
        {
            foreach (Galant.DataEntity.Paper p in checkIn.WorkDoneList)
            {
                Galant.DataEntity.Paper db_p = paperOp.SearchById(p.PaperId) as Galant.DataEntity.Paper;
                if (db_p.PaperSubStatus != PaperSubState.InTransit)
                {
                    throw new Galant.DataEntity.WCFFaultException(9001, "Data Out Of Date", "数据已过期,请返回刷新后再试。");
                }
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
            foreach (Galant.DataEntity.Package pack in p.Packages)
            {
                
            }
        }

        private void UpdatePaperStatus(Galant.DataEntity.Paper p)
        {
            string sqlUpdate = "UPDATE papers SET substate = @substate WHERE paper_id = @paper_id";
            List<MySqlParameter> mps = new List<MySqlParameter>();
            mps.Add(new MySqlParameter("@substate", (int)p.PaperSubStatus));
            mps.Add(new MySqlParameter("@paper_id", p.PaperId));
            MySqlHelper.ExecuteNonQuery(this.Operator.myConnection, sqlUpdate, mps.ToArray());
        }


    }
}