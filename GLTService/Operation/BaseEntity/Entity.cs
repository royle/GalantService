using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using GLTService.DBConnector;

namespace GLTService.Operation.BaseEntity
{
    public class Entity:BaseOperator
    {
        public Entity(DataOperator data)
            : base(data)
        { }

        public Entity()
        {}

        public override string SqlAddNewSql
        {
            get
            {
                return SqlInsertDataSql;
//                return @"INSERT INTO entities(
//                Alias,Password,Full_Name,Home_phone,Cell_phone1,Cell_phone2,Type,
//                Address_Family,Address_Child,Comment,Deposit,Pay_type,Route_Station,Able_flag)
//                VALUES (
//                @Alias,@Password,@Full_Name,@Home_phone,@Cell_phone1,@Cell_phone2,@Type,
//                @Address_Family,@Address_Child,@Comment,@Deposit,@Pay_type,@Route_Station,@Able_flag)";
                
            }
        }

        public override string SqlUpdateSql
        {
            get
            {
                return @"UPDATE entities SET 
                Alias = @Alias,Password = @Password,Full_Name = @Full_Name,Home_phone = @Home_phone,Cell_phone1 = @Cell_phone1,Cell_phone2 = @Cell_phone2,Type = @Type ,
                Address_Family =@Address_Family ,Address_Child =@Address_Child  ,Comment =@Comment,Deposit =@Deposit  ,Pay_type =@Pay_type  ,Route_Station = @Route_Station ,Able_flag =@Able_flag  
                WHERE entity_id = @entity_id 
                ";

            }
        }

        public override string KeyId
        {
            get
            {
                return "Entity_id";
            }
        }

        public Galant.DataEntity.Entity MappingRow(DataRow dr)
        {
            if (dr == null)
                return null;
            Galant.DataEntity.Entity entity = new Galant.DataEntity.Entity();
            if (string.IsNullOrEmpty(dr["Entity_id"].ToString()))
                entity.EntityId = null;
            else
                entity.EntityId =  Convert.ToInt32(dr["Entity_id"]);
            entity.Alias = dr["Alias"].ToString();
            entity.Password = dr["Password"].ToString();
            entity.FullName = dr["Full_Name"].ToString();
            entity.HomePhone = dr["Home_phone"].ToString();
            entity.CellPhoneOne = dr["Cell_phone1"].ToString();
            entity.CellPhoneTwo = dr["Cell_phone2"].ToString();
            if (string.IsNullOrEmpty(dr["Type"].ToString()))
                entity.EntityType = Galant.DataEntity.EntityType.Station;
            else
                entity.EntityType = (Galant.DataEntity.EntityType)dr["Type"];
            entity.AddressFamily = dr["Address_Family"].ToString();
            entity.AddressChild = dr["Address_Child"].ToString();
            entity.Comment = dr["Comment"].ToString();
            if (string.IsNullOrEmpty(dr["Deposit"].ToString()))
                entity.Deposit = null;
            else
                entity.Deposit = Convert.ToDecimal(dr["Deposit"]);

            if (string.IsNullOrEmpty(dr["Pay_type"].ToString()))
                entity.PayType = Galant.DataEntity.PayType.AtTime;
            else
                entity.PayType = (Galant.DataEntity.PayType)dr["Pay_type"];

            if (string.IsNullOrEmpty(dr["Route_Station"].ToString()))
                entity.RountStation = null;
            else
                entity.RountStation = Convert.ToInt32(dr["Route_Station"]);

            if (string.IsNullOrEmpty(dr["Able_flag"].ToString()))
                entity.AbleFlag = false;
            else
                entity.AbleFlag = Convert.ToBoolean(dr["Able_flag"]);
            return entity;
        }

        protected override void MappingDataName()
        {
            DicDataMapping.Add("EntityId", "Entity_id");
            DicDataMapping.Add("Alias", "Alias");
            DicDataMapping.Add("Password", "Password");
            DicDataMapping.Add("FullName", "Full_Name");
            DicDataMapping.Add("HomePhone", "Home_phone");
            DicDataMapping.Add("CellPhoneOne", "Cell_phone1");
            DicDataMapping.Add("CellPhoneTwo", "Cell_phone2");
            DicDataMapping.Add("EntityType", "Type");
            DicDataMapping.Add("AddressFamily", "Address_Family");
            DicDataMapping.Add("AddressChild", "Address_Child");
            DicDataMapping.Add("Comment", "Comment");
            DicDataMapping.Add("Deposit", "Deposit");
            DicDataMapping.Add("PayType", "Pay_type");
            DicDataMapping.Add("RountStation", "Route_Station");
            DicDataMapping.Add("AbleFlag", "Able_flag");
        }

        protected override void MappingInsertName()
        {
            DicInsertMapping.Add("Alias", "Alias");
            DicInsertMapping.Add("Password", "Password");
            DicInsertMapping.Add("FullName", "Full_Name");
            DicInsertMapping.Add("HomePhone", "Home_phone");
            DicInsertMapping.Add("CellPhoneOne", "Cell_phone1");
            DicInsertMapping.Add("CellPhoneTwo", "Cell_phone2");
            DicInsertMapping.Add("EntityType", "Type");
            DicInsertMapping.Add("AddressFamily", "Address_Family");
            DicInsertMapping.Add("AddressChild", "Address_Child");
            DicInsertMapping.Add("Comment", "Comment");
            DicInsertMapping.Add("Deposit", "Deposit");
            DicInsertMapping.Add("PayType", "Pay_type");
            DicInsertMapping.Add("RountStation", "Route_Station");
            DicInsertMapping.Add("AbleFlag", "Able_flag");
        }

        protected override void MappingUpdateName()
        {
            DicUpdateMapping.Add("EntityId", "Entity_id");
            DicUpdateMapping.Add("Alias", "Alias");
            DicUpdateMapping.Add("Password", "Password");
            DicUpdateMapping.Add("FullName", "Full_Name");
            DicUpdateMapping.Add("HomePhone", "Home_phone");
            DicUpdateMapping.Add("CellPhoneOne", "Cell_phone1");
            DicUpdateMapping.Add("CellPhoneTwo", "Cell_phone2");
            DicUpdateMapping.Add("EntityType", "Type");
            DicUpdateMapping.Add("AddressFamily", "Address_Family");
            DicUpdateMapping.Add("AddressChild", "Address_Child");
            DicUpdateMapping.Add("Comment", "Comment");
            DicUpdateMapping.Add("Deposit", "Deposit");
            DicUpdateMapping.Add("PayType", "Pay_type");
            DicUpdateMapping.Add("RountStation", "Route_Station");
            DicUpdateMapping.Add("AbleFlag", "Able_flag");
        }

        protected override void SetTableName()
        {
            TableName = "entities";
        }

        /// <summary>
        /// 用户密码检查授权
        /// </summary>
        /// <param name="data">数据库操作对象</param>
        /// <param name="Alias">用户名</param>
        /// <param name="Password">密码</param>
        /// <param name="IsDetail">是否返回用户的权限,及库存等信息</param>
        /// <returns></returns>
        public Galant.DataEntity.Entity Authorize(DataOperator data, string Alias,string Password, bool IsDetail)
        {
            string SqlSearch = this.BuildSearchSQL() + " WHERE Alias = '" + Alias + "' AND Password = '" + Password + "' AND type = 2";
            DataTable dt = SqlHelper.ExecuteDataset(data.myConnection, CommandType.Text, SqlSearch).Tables[0];
            if (dt.Rows.Count > 0)
            {
                Galant.DataEntity.Entity entity = this.MappingRow(dt.Rows[0]);
                if(!entity.AbleFlag)
                    throw new Galant.DataEntity.WCFFaultException(998, "User Overdue", "用户已经停用");
                return this.GetEntityByID(data, entity.EntityId.ToString(), IsDetail);
            }
            throw new Galant.DataEntity.WCFFaultException(999, "Authorize Fail", "用户名不存在或密码错误");
        }

       
        
        /// <summary>
        /// 获取单个用户对象
        /// </summary>
        /// <param name="data"></param>
        /// <param name="EntityId"></param>
        /// <param name="IsDetail"></param>
        /// <returns></returns>
        public Galant.DataEntity.Entity GetEntityByID(DataOperator data, string EntityId, bool IsDetail)
        {
            string SqlSearch = this.BuildSearchSQL() + " WHERE Entity_id = " + EntityId;
            DataTable dt = SqlHelper.ExecuteDataset(data.myConnection, CommandType.Text, SqlSearch).Tables[0];
            if (dt.Rows.Count > 0)
            {
                Galant.DataEntity.Entity entity = this.MappingRow(dt.Rows[0]);
                if (!IsDetail)
                    return entity;
                if (entity.EntityType == Galant.DataEntity.EntityType.Staff)
                {
                    Role role = new Role();
                    entity.Roles = role.GetRolesByEntityID(data, EntityId);
                }
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 根据用户名获取实体,不区分大小写
        /// </summary>
        /// <param name="data"></param>
        /// <param name="alias"></param>
        /// <param name="IsDetail"></param>
        /// <returns></returns>
        public Galant.DataEntity.Entity GetEntityByAlias(DataOperator data,string alias,bool IsDetail)
        {
            if (string.IsNullOrWhiteSpace(alias))
                return null;

            string SqlSearch = "SELECT * FROM ENTITIES WHERE UPPER(ALIAS) = '"+alias.ToUpper().Trim()+"'";
             DataTable dt = SqlHelper.ExecuteDataset(data.myConnection, CommandType.Text, SqlSearch).Tables[0];
             if (dt.Rows.Count > 0)
             {
                 Galant.DataEntity.Entity entity = this.MappingRow(dt.Rows[0]);
                 if (!IsDetail)
                     return entity;
                 if (entity.EntityType == Galant.DataEntity.EntityType.Staff)
                 {
                     Role role = new Role();
                     entity.Roles = role.GetRolesByEntityID(data, entity.EntityId.Value.ToString());
                 }
                 return entity;
             }
             return null;
        }

        /// <summary>
        /// 某用户名是否存在,不区分大小写
        /// </summary>
        /// <param name="data"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public bool CheckAliasExist(DataOperator data, string alias)
        {
            return this.GetEntityByAlias(data, alias, false) != null ;
        }
        

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Galant.DataEntity.Entity> GetAllAvailableEntitys(DataOperator data)
        {
            string SqlSearch = this.BuildSearchSQL() + " WHERE Able_flag = 1";
            DataTable dt = SqlHelper.ExecuteDataset(data.myConnection, CommandType.Text, SqlSearch).Tables[0];            
            if (dt.Rows.Count > 0)
            {
                List<Galant.DataEntity.Entity> entitys = new List<Galant.DataEntity.Entity>();
                foreach (DataRow dr in dt.Rows)
                {
                    Galant.DataEntity.Entity entity = this.MappingRow(dr) as Galant.DataEntity.Entity;
                    entitys.Add(entity);
                }
                return entitys;
            }
            return null;
        }

        /// <summary>
        /// 获取所有路线关联的实体
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Galant.DataEntity.Entity> GetRoutedEntitys(DataOperator data)
        {
            string SqlSearch = @"select e.* from routes as r ,entities as e where e.entity_id = r.from_entity or e.entity_id = r.to_entity";
            DataTable dt = SqlHelper.ExecuteDataset(data.myConnection, CommandType.Text, SqlSearch).Tables[0];
            if (dt.Rows.Count > 0)
            {
                List<Galant.DataEntity.Entity> entitys = new List<Galant.DataEntity.Entity>();
                foreach (DataRow dr in dt.Rows)
                {
                    Galant.DataEntity.Entity entity = this.MappingRow(dr) as Galant.DataEntity.Entity;
                    entitys.Add(entity);
                }
                return entitys;
            }
            return null;
        }

        /// <summary>
        /// 按条件查询符合条件的实体
        /// </summary>
        /// <param name="data"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public List<Galant.DataEntity.Entity> GetEntitysByConditions(DataOperator data, Galant.DataEntity.Search.SearchEntityCondition conditions)
        {
            string SqlSearch = @"SELECT e.* FROM entities AS e WHERE 1=1 ";
            if (conditions.Type.HasValue)
                SqlSearch += " AND type = " + Convert.ToInt32(conditions.Type.Value);
            if (!string.IsNullOrEmpty(conditions.Alias))
                SqlSearch += " AND UPPER(alias) like '%" + conditions.Alias.ToUpper().Trim() + "%' ";
            if(!string.IsNullOrEmpty(conditions.Name))
                SqlSearch += " AND UPPER(full_name) like '%" + conditions.Name.ToUpper().Trim() + "%' ";
            if (!string.IsNullOrEmpty(conditions.Phone))
                SqlSearch += " AND (home_phone like '%" + conditions.Phone.Trim() + "%' OR cell_phone1 like '%" + conditions.Phone.Trim() + "%' OR cell_phone2 like '%" + conditions.Phone.Trim() + "%' )";
            if (!conditions.IsStop)
                SqlSearch += " AND able_flag = 1";

            DataTable dt = SqlHelper.ExecuteDataset(data.myConnection, CommandType.Text, SqlSearch).Tables[0];
            if (dt.Rows.Count > 0)
            {
                List<Galant.DataEntity.Entity> entitys = new List<Galant.DataEntity.Entity>();
                foreach (DataRow dr in dt.Rows)
                {
                    Galant.DataEntity.Entity entity = this.MappingRow(dr) as Galant.DataEntity.Entity;
                    entitys.Add(entity);
                }
                return entitys;
            }
            return null;
        }

        public Galant.DataEntity.Entity SaveEntity(DataOperator data, Galant.DataEntity.Entity entity)
        {
            if (entity.EntityType == Galant.DataEntity.EntityType.Staff && entity.Roles != null)//权限检查
            {
                foreach (Galant.DataEntity.Role r in entity.Roles)
                {
                    if (r.Station == null)
                    {
                        throw new Galant.DataEntity.WCFFaultException(1235, "Station Alias Not Exist", "站点为空，请输入正确的站名");
                    }
                    else
                    {
                        Galant.DataEntity.Entity station = this.GetEntityByAlias(data, r.Station.Alias, false);
                        if (station == null || station.EntityType != Galant.DataEntity.EntityType.Station)
                            throw new Galant.DataEntity.WCFFaultException(1235, "Station Alias Not Exist", "站点(" + station.Alias + ")不存在，请输入正确的站名");
                        else
                            r.Station = station;
                    }

                }
            }

            if (!entity.EntityId.HasValue)
            {
                if (entity.IsAliasAllowed && this.CheckAliasExist(data, entity.Alias))
                    throw new Galant.DataEntity.WCFFaultException(1234, "User Name Exist", "用户名(" + entity.Alias + ")已存在，请输入其它用户名");
                this.AddNewData(entity);
                DataTable dt = SqlHelper.ExecuteDataset(data.mytransaction, CommandType.Text, "SELECT * FROM entities WHERE entity_id = (SELECT LAST_INSERT_ID() AS entity_id)", this.BuildParameteres(entity)).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    entity.EntityId = this.MappingRow(dt.Rows[0]).EntityId;
                    if (entity.EntityType == Galant.DataEntity.EntityType.Client && string.IsNullOrEmpty(entity.Alias))
                    {
                        this.AutoGenerateAlias(data, entity);
                    }
                }
            }
            else
            {
                SqlHelper.ExecuteNonQuery(data.mytransaction, CommandType.Text, this.SqlUpdateSql, this.BuildUpdateParameteres(entity));
            }

            Role role = new Role();
            role.AddRolesForEntity(data, entity);
            return entity;
        }
        /// <summary>
        /// 自动生成客户代码,(Entity_id 左填充0),长度11
        /// </summary>
        /// <param name="data"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string AutoGenerateAlias(DataOperator data, Galant.DataEntity.Entity entity)
        {
            string sqlUpdate = @"UPDATE entities SET ALIAS =  CONCAT('C',entity_id) WHERE entity_id = @entity_id";
            Dictionary<string, string> dictGenerateAlias = new Dictionary<string, string>();
            dictGenerateAlias.Add("EntityId", "Entity_id");
            SqlHelper.ExecuteNonQuery(data.mytransaction, CommandType.Text, sqlUpdate, this.BuildParameteres(entity, dictGenerateAlias, false, true));
            entity.Alias = "C"+entity.EntityId.ToString();
            return "C" + entity.EntityId.ToString();
        }
        
    }
}
