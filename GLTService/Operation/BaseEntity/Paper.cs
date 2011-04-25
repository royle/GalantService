using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;
using GLTService.DBConnector;

namespace GLTService.Operation.BaseEntity
{
    public class Paper:BaseOperator
    {
        public Paper(DataOperator data)
            : base(data) { }

        public override string SqlAddNewSql
        {
            get
            {
                return base.SqlInsertDataSql;
//                return @"insert into papers(
//paper_id,Status,SubState,Holder,Bound,Contact_a,Contact_b,Contact_c,Deliver_a,Deliver_b,Deliver_c,Deliver_a_time,Deliver_b_time,Deliver_c_time,Start_time,Finish_time,Salary,Comment,Type,Next_Route,Mobile_Status)
//values (
//@paper_id,@Status,@SubState,@Holder,@Bound,@Contact_a,@Contact_b,@Contact_c,@Deliver_a,@Deliver_b,@Deliver_c,@Deliver_a_time,@Deliver_b_time,@Deliver_c_time,@Start_time,@Finish_time,@Salary,@Comment,@Type,@Next_Route,@Mobile_Status)";
            }
        }

        public override string KeyId
        {
            get
            {
                return "paper_id";
            }
        }

        protected override void MappingDataName()
        {
            DicDataMapping.Add("PaperId", "paper_id");
            DicDataMapping.Add("PaperStatus", "Status");
            DicDataMapping.Add("PaperSubStatus", "SubState");
            DicDataMapping.Add("Holder", "Holder");
            DicDataMapping.Add("Bound", "Bound");
            DicDataMapping.Add("ContactA", "Contact_a");
            DicDataMapping.Add("ContactB", "Contact_b");
            DicDataMapping.Add("ContactC", "Contact_c");
            DicDataMapping.Add("DeliverA", "Deliver_a");
            DicDataMapping.Add("DeliverB", "Deliver_b");
            DicDataMapping.Add("DeliverC", "Deliver_c");
            DicDataMapping.Add("DeliverATime", "Deliver_a_time");
            DicDataMapping.Add("DeliverBTime", "Deliver_b_time");
            DicDataMapping.Add("DeliverCTime", "Deliver_c_time");
            DicDataMapping.Add("StartTime", "Start_time");
            DicDataMapping.Add("FinishTime", "Finish_time");
            DicDataMapping.Add("Salary", "Salary");
            DicDataMapping.Add("Comment", "Comment");
            DicDataMapping.Add("PaperType", "Type");
            DicDataMapping.Add("NextRoute", "Next_Route");
            DicDataMapping.Add("MobileStatus", "Mobile_Status");
            DicDataMapping.Add("IsCollection", "is_collection");
        }

        protected override void SetTableName()
        {
            TableName = "papers";
        }
        /// <summary>
        /// 获取清单中未完成订单的列表
        /// </summary>
        /// <param name="dataOper"></param>
        /// <param name="checkinData"></param>
        /// <returns></returns>
        public Galant.DataEntity.Result.FinishCheckin GetCheckinPaperListByCollection(DataOperator dataOper, Galant.DataEntity.Result.FinishCheckin checkinData)
        {
            List<Galant.DataEntity.Paper> papers = new List<Galant.DataEntity.Paper>();
            foreach (Galant.DataEntity.Paper p in checkinData.CheckinCollections)
            {
                papers.AddRange(this.GetPaperChilders(dataOper, p));
            }
            Package package = new Package(dataOper);
            foreach (Galant.DataEntity.Paper p in papers)
            {
                foreach (Galant.DataEntity.Package pa in package.GetPackagesByPaper(dataOper, p.PaperId))
                {
                    if (p.Packages == null)
                        p.Packages = new System.Collections.ObjectModel.ObservableCollection<Galant.DataEntity.Package>();
                    p.Packages.Add(pa);
                }
            }
            papers.RemoveAll(p => p.PaperSubStatus != Galant.DataEntity.PaperSubState.InTransit);
            checkinData.CheckinPapers = papers;
            return checkinData;
        }
        /// <summary>
        /// 获取未完成清单列表
        /// </summary>
        /// <param name="dataOper"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        public List<Galant.DataEntity.Paper> GetFinishList(DataOperator dataOper, Galant.DataEntity.Entity station)
        {
            string SqlSearch = @"select p.* from papers as p join event_logs as e on p.paper_id = e.paper_id 
                where p.is_collection and e.event_id = 
                (select max(event_id) from event_logs where event_type ='CKO-B' and paper_id = p.paper_id)
                and p.substate = @substate 
                and e.at_station = @at_station";
            List<MySqlParameter> paras = new List<MySqlParameter>();
            paras.Add(new MySqlParameter("@substate", (int)Galant.DataEntity.PaperSubState.InTransit));
            paras.Add(new MySqlParameter("@at_station", station.EntityId));

            DataTable dt = SqlHelper.ExecuteDataset(dataOper.myConnection, CommandType.Text, SqlSearch,paras.ToArray()).Tables[0];
            List<Galant.DataEntity.Paper> papers = new List<Galant.DataEntity.Paper>();
            foreach (DataRow dr in dt.Rows)
            {
                papers.Add(MappingRow(dr, dataOper));
            }
            return papers;
        }

       
        public Galant.DataEntity.Result.ResultPapersByID SearchPapersByID(Galant.DataEntity.Result.ResultPapersByID search)
        {
            string SqlSearch = @"select p.* from papers as p left join origin_paper_links as op on p.paper_id = op.paper_id where p.paper_id like @paper_id or op.origin_name like @paper_id ";
            List<MySqlParameter> paras = new List<MySqlParameter>();
            paras.Add(new MySqlParameter("@paper_id", search.PaperId));
            DataTable dt = SqlHelper.ExecuteDataset(this.Operator.myConnection, CommandType.Text, SqlSearch, paras.ToArray()).Tables[0];
            List<Galant.DataEntity.Paper> papers = new List<Galant.DataEntity.Paper>();
            foreach (DataRow dr in dt.Rows)
            {
                Galant.DataEntity.Paper p = MappingRow(dr, this.Operator, false);
                papers.Add(p);
            }
            search.Papers = papers;
            return search;
        }
        /// <summary>
        /// 获取订单详细信息 包含历程
        /// </summary>
        /// <param name="paper"></param>
        /// <returns></returns>
        public Galant.DataEntity.Paper SearchPaperByPaperID(Galant.DataEntity.Paper paper)
        {
            string SqlSearch = @"select p.* from papers as p where p.paper_id = @paper_id";
            List<MySqlParameter> paras = new List<MySqlParameter>();
            paras.Add(new MySqlParameter("@paper_id", paper.PaperId));
            DataTable dt = SqlHelper.ExecuteDataset(this.Operator.myConnection, CommandType.Text, SqlSearch, paras.ToArray()).Tables[0];
            EventLog eventOp = new EventLog(this.Operator);
            foreach (DataRow dr in dt.Rows)
            {
                paper = MappingRow(dr, this.Operator, false);
                paper.EventLogs = eventOp.GetEventByPaperID(paper.PaperId);
                break;
            }
            return paper;
        }

        public Galant.DataEntity.Paper SearchPaperByPaperID(string paperId)
        {
            string SqlSearch = @"select p.* from papers as p where p.paper_id = @paper_id";
            List<MySqlParameter> paras = new List<MySqlParameter>();
            paras.Add(new MySqlParameter("@paper_id", paperId));
            DataTable dt = SqlHelper.ExecuteDataset(this.Operator.myConnection, CommandType.Text, SqlSearch, paras.ToArray()).Tables[0];
            EventLog eventOp = new EventLog(this.Operator);
            Galant.DataEntity.Paper paper = null;
            foreach (DataRow dr in dt.Rows)
            {
                paper = MappingRow(dr, this.Operator);
                break;
            }
            return paper;
        }

        public Galant.DataEntity.PaperOperation.PaperRevertFinishingRequest ProcessRevertFinishing(Galant.DataEntity.PaperOperation.PaperRevertFinishingRequest request)
        {
            Galant.DataEntity.Paper p = this.SearchPaperByPaperID(request.PaperId);
            if (p.PaperStatus != Galant.DataEntity.PaperStatus.Finish)
            {
                throw new Galant.DataEntity.WCFFaultException(1110, "Data Out of date", "数据刚被他人修改过,请刷新后再试");
            }
            Galant.DataEntity.EventLog e = new Galant.DataEntity.EventLog()
            {
                AtStation = this.Operator.EntityOperator.CurerentStationID,
                EventType = "RVF",
                InsertTime = System.DateTime.Now,
                RelationEntity = this.Operator.EntityOperator.CurerentStationID,
                PaperId = request.PaperId,
                EntityID = this.Operator.EntityOperator.EntityId,
                EventData = "回复归班前状态原因：" + request.Note
            };
            return null;
        }

        public Galant.DataEntity.PaperOperation.PaperForcedReturnRequest ProcessForceretrun(Galant.DataEntity.PaperOperation.PaperForcedReturnRequest request)
        {
            Galant.DataEntity.Paper p = this.SearchPaperByPaperID(request.PaperId);
            if (p.PaperStatus == Galant.DataEntity.PaperStatus.Finish)
            {
                throw new Galant.DataEntity.WCFFaultException(1110, "Data Out of date", "数据刚被他人修改过,请刷新后再试");
            }
            this.FinishPaper(p, Galant.DataEntity.PaperSubState.FinishWithCancel);
            Package package = new Package(this.Operator);
            
            Galant.DataEntity.EventLog e = new Galant.DataEntity.EventLog()
            {
                AtStation = this.Operator.EntityOperator.CurerentStationID,
                EventType = "FR",
                InsertTime = System.DateTime.Now,
                RelationEntity = this.Operator.EntityOperator.CurerentStationID,
                PaperId = request.PaperId,
                EntityID = this.Operator.EntityOperator.EntityId,
                EventData="取消配送原因："+request.Note 
            };
            this.AddEvent(e);
            GLTService.Operation.BaseEntity.Store storeOpe = new BaseEntity.Store(this.Operator);
            if (p.DeliverB != null)
            {
                foreach (Galant.DataEntity.Package pack in package.GetPackagesByPaper(this.Operator, p.PaperId))
                {
                    Galant.DataEntity.Store store = new Galant.DataEntity.Store() { EntityID = p.DeliverB.EntityId, ProductID = pack.Product.ProductId, ProductCount = (pack.Count * -1), Bound = 1 };
                    storeOpe.AddNewData(store);//抵消配送员之前的实体库存
                }
            }
            return request;
        }


        private List<Galant.DataEntity.Paper> GetPaperChilders(DataOperator dataOper, Galant.DataEntity.Paper paper)
        {
            if (!paper.IsCollection)
                return null;
            string SqlSearch = @"select p.* from papers as p where p.paper_id in (select paper_id from paper_links as pl where pl.parent_id in (select link_id from paper_links where paper_id = @paper_id) and able_flag)";
            List<MySqlParameter> paras = new List<MySqlParameter>();
            paras.Add(new MySqlParameter("@paper_id", paper.PaperId));
            DataTable dt = SqlHelper.ExecuteDataset(dataOper.myConnection, CommandType.Text, SqlSearch, paras.ToArray()).Tables[0];
            List<Galant.DataEntity.Paper> papers = new List<Galant.DataEntity.Paper>();
            foreach (DataRow dr in dt.Rows)
            {
                papers.Add(MappingRow(dr, dataOper));
            }
            return papers;
        }

        public override Galant.DataEntity.BaseData MappingRow(DataRow row)
        {
            return this.MappingRow(row, this.Operator);
        }

        public Galant.DataEntity.Paper MappingRow(DataRow dr, DataOperator dataOper)
        {
            return this.MappingRow(dr, dataOper, true);
        }

        private Galant.DataEntity.Paper MappingRow(DataRow dr, DataOperator dataOper, bool IsSmiple)
        {
            if (dr == null)
                return null;
            Entity entityOper = new Entity(dataOper);
            Galant.DataEntity.Paper paper = new Galant.DataEntity.Paper();
            if (!string.IsNullOrEmpty(dr["paper_id"].ToString()))
                paper.PaperId = dr["paper_id"].ToString();
            if (!string.IsNullOrEmpty(dr["status"].ToString()))
                paper.PaperStatus = (Galant.DataEntity.PaperStatus)(Convert.ToInt32(dr["status"]));
            if (!string.IsNullOrEmpty(dr["substate"].ToString()))
                paper.PaperSubStatus = (Galant.DataEntity.PaperSubState)(Convert.ToInt32(dr["substate"]));
            if (!string.IsNullOrEmpty(dr["holder"].ToString()))
                paper.Holder = entityOper.GetEntityByID(dataOper, dr["holder"].ToString(), false);
            if (!string.IsNullOrEmpty(dr["bound"].ToString()))
                paper.Bound = (Galant.DataEntity.PaperBound)(Convert.ToInt32(dr["bound"]));
            if (!string.IsNullOrEmpty(dr["contact_a"].ToString()))
                paper.ContactA = entityOper.GetEntityByID(dataOper, dr["contact_a"].ToString(), false);
            if (!string.IsNullOrEmpty(dr["contact_b"].ToString()))
                paper.ContactB = entityOper.GetEntityByID(dataOper, dr["contact_b"].ToString(), false);
            if (!string.IsNullOrEmpty(dr["contact_c"].ToString()))
                paper.ContactC = entityOper.GetEntityByID(dataOper, dr["contact_c"].ToString(), false);
            if (IsSmiple)
            {
                if (!string.IsNullOrEmpty(dr["deliver_a"].ToString()))
                    paper.DeliverA = entityOper.GetEntityByID(dataOper, dr["deliver_a"].ToString(), false);
                if (!string.IsNullOrEmpty(dr["deliver_b"].ToString()))
                    paper.DeliverB = entityOper.GetEntityByID(dataOper, dr["deliver_b"].ToString(), false);
                if (!string.IsNullOrEmpty(dr["deliver_c"].ToString()))
                    paper.DeliverC = entityOper.GetEntityByID(dataOper, dr["deliver_c"].ToString(), false);
            }
            if (!string.IsNullOrEmpty(dr["deliver_a_time"].ToString()))
                paper.DeliverATime = Convert.ToDateTime(dr["deliver_a_time"]);
            if (!string.IsNullOrEmpty(dr["deliver_b_time"].ToString()))
                paper.DeliverBTime = Convert.ToDateTime(dr["deliver_b_time"]);
            if (!string.IsNullOrEmpty(dr["deliver_c_time"].ToString()))
                paper.DeliverCTime = Convert.ToDateTime(dr["deliver_c_time"]);

            if (!string.IsNullOrEmpty(dr["start_time"].ToString()))
                paper.StartTime = Convert.ToDateTime(dr["start_time"]);
            if (!string.IsNullOrEmpty(dr["finish_time"].ToString()))
                paper.FinishTime = Convert.ToDateTime(dr["finish_time"]);

            if (!string.IsNullOrEmpty(dr["salary"].ToString()))
                paper.Salary = Convert.ToDecimal(dr["salary"]);
            paper.Comment = dr["comment"].ToString();

            if (!string.IsNullOrEmpty(dr["type"].ToString()))
                paper.PaperType = (Galant.DataEntity.PaperType)(Convert.ToInt32(dr["type"]));
            if (IsSmiple)
            {
                if (!string.IsNullOrEmpty(dr["next_route"].ToString()))
                {
                    Route r = new Route(dataOper);
                    paper.NextRoute = r.GetRouteByID(dr["next_route"].ToString(), dataOper);
                }
            }

            if (!string.IsNullOrEmpty(dr["mobile_status"].ToString()))
                paper.MobileStatus = (Galant.DataEntity.PaperSubState)(Convert.ToInt32(dr["mobile_status"]));

            if (!string.IsNullOrEmpty(dr["is_collection"].ToString()))
                paper.IsCollection = Convert.ToBoolean(dr["is_collection"]);
            if (paper.IsCollection)
                paper.ChildPapers = this.GetPaperChilders(dataOper, paper);
            return paper;
        }

        /// <summary>
        /// 检查运送单下是否还有订单(如订单全部都已经入库,则完成运送单)
        /// </summary>
        /// <param name="pc">运送单</param>
        public void FinishCollections(Galant.DataEntity.Paper pc)
        {
            string checkSql = @"SELECT count(1) AS count FROM PAPER_LINKS P WHERE 
parent_id IN (SELECT link_id FROM PAPER_LINKS WHERE PAPER_ID = @paper_id AND ABLE_FLAG ) AND 
ABLE_FLAG";
            List<MySqlParameter> paras = new List<MySqlParameter>();
            paras.Add(new MySqlParameter("@paper_id", pc.PaperId));
            DataTable dt = SqlHelper.ExecuteDataset(this.Operator.mytransaction, CommandType.Text, checkSql, paras.ToArray()).Tables[0];

            if (dt.Rows[0]["count"].ToString() == "0")//完成清单
            {
                this.FinishPaper(pc,Galant.DataEntity.PaperSubState.FinishGood);
            }
        }
        /// <summary>
        /// 完成订单
        /// </summary>
        /// <param name="p"></param>
        /// <param name="subStatus"></param>
        public void FinishPaper(Galant.DataEntity.Paper p, Galant.DataEntity.PaperSubState? subStatus)
        {
            subStatus = subStatus == null ? Galant.DataEntity.PaperSubState.FinishGood : subStatus;
            Galant.DataEntity.PackageState status=(Galant.DataEntity.PackageState)(((int)subStatus) >> 4);
            string sqlUpdate = "UPDATE papers SET status = @status, substate = @substate, finish_time = @finish_time WHERE paper_id = @paper_id";
            List<MySqlParameter> mps = new List<MySqlParameter>();
            mps.Add(new MySqlParameter("@status", (int)status));
            mps.Add(new MySqlParameter("@substate", (int)subStatus));
            mps.Add(new MySqlParameter("@paper_id", p.PaperId));
            mps.Add(new MySqlParameter("@finish_time", DateTime.Now));
            MySqlHelper.ExecuteNonQuery(this.Operator.myConnection, sqlUpdate, mps.ToArray());

            string sqlDisablePaperLink = "update paper_links set able_flag = false where paper_id = @paper_id ";
            List<MySqlParameter> mps1 = new List<MySqlParameter>();
            mps1.Add(new MySqlParameter("@paper_id", p.PaperId));
            MySqlHelper.ExecuteNonQuery(this.Operator.myConnection, sqlDisablePaperLink, mps1.ToArray());
        }

        public override bool AddNewData(Galant.DataEntity.BaseData data)
        {
            Galant.DataEntity.Paper paper = data as Galant.DataEntity.Paper;
            paper.PaperId = this.GenerateId();
            if (paper.ContactB.IsNew)
            {
                Entity entity = new Entity(this.Operator);
                if (paper.Packages != null && paper.Packages.FirstOrDefault() != null && paper.Packages.FirstOrDefault().Product != null && paper.Packages.FirstOrDefault().Product.ProductType == Galant.DataEntity.ProductEnum.Delivery)
                {
                    paper.ContactB.EntityType = Galant.DataEntity.EntityType.Receiver;
                    if (paper.ContactA != null)
                    {
                        paper.ContactA.EntityType = Galant.DataEntity.EntityType.Receiver;
                        entity.SaveEntity(this.Operator, paper.ContactA);
                    }
                }
                else
                {
                    paper.ContactB.EntityType = Galant.DataEntity.EntityType.Client;
                }
                entity.SaveEntity(this.Operator, paper.ContactB);
            }
            base.AddNewData(data);

            Package opPackage = new Package(this.Operator);
            foreach (Galant.DataEntity.Package pg in paper.Packages)
            {
                pg.PackageType = Galant.DataEntity.PackageState.New;
                pg.PaperId = paper.PaperId;
                opPackage.AddNewData(pg);
            }
            
            Galant.DataEntity.EventLog eventLog = new Galant.DataEntity.EventLog()
            { EntityID = this.Operator.EntityOperator == null ? null : this.Operator.EntityOperator.EntityId,
              RelationEntity = this.Operator.EntityOperator == null ? null : this.Operator.EntityOperator.EntityId,
              AtStation = this.Operator.EntityOperator == null ? null : this.Operator.EntityOperator.CurerentStationID, 
                EventData="订单建立", EventType="S-Create", InsertTime=DateTime.Now, PaperId= paper.PaperId  };
            this.AddEvent(eventLog);
            return true;
        }

        public void AddNewCollectionData(Galant.DataEntity.BaseData data)
        {
            Galant.DataEntity.Paper paper = data as Galant.DataEntity.Paper;
            paper.PaperId = this.GenerateId();
            base.AddNewData(paper);
        }

        private string GenerateId()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks).ToUpper();
        }

        public override MySqlParameter[] BuildParameteres(Galant.DataEntity.BaseData data, Dictionary<string, string> DicData, bool isInsert, bool isUpdate)
        {
            List<String> conditions = new List<string>();
            List<String> parameters = new List<string>();
            List<String> updateParamters = new List<string>();
            List<MySqlParameter> param = new List<MySqlParameter>();
            String keyParam = String.Empty;

            foreach (System.Reflection.PropertyInfo info in data.GetType().GetProperties())
            {
                string key = string.Empty;
                string propertyName = info.Name;

                if (DicData.TryGetValue(propertyName, out key))
                {
                    object obj = info.GetValue(data, null);
                    if (obj == null)
                        continue;

                    string parameterName = "@" + key;

                    if (obj.GetType().BaseType == typeof(Enum))
                    {
                        param.Add(new MySqlParameter(parameterName, (int)obj));
                    }
                    else if(obj.GetType().BaseType==typeof(Galant.DataEntity.BaseData))
                    {
                        param.Add(new MySqlParameter(parameterName, (obj as Galant.DataEntity.BaseData).QueryId));
                    }
                    else
                    {
                        param.Add(new MySqlParameter(parameterName, obj));
                    }
                    conditions.Add(key);
                    parameters.Add(parameterName);
                    if (key == this.KeyId)
                    {
                        keyParam = key + "=" + parameterName;
                    }
                    else
                    {
                        updateParamters.Add(key + "=" + parameterName);
                       
                    }
                }
            }
            if (isUpdate)
            {
                SqlUpdateDataSql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, string.Join(" ,", updateParamters), keyParam);
            }
            if (isInsert)
            {
                SqlInsertDataSql = string.Format("INSERT INTO {0} ({1}) VALUES({2})", TableName, String.Join(" ,", conditions), String.Join(" ,", parameters));
            }
            return param.ToArray();
        }
    }
}