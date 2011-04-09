using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLTService.DBConnector;
using Galant.DataEntity;

namespace GLTService.Operation.BaseEntity
{
    public class PaperLinks : BaseOperator
    {
        public PaperLinks(DataOperator data)
            : base(data)
        { }

        protected override void SetTableName()
        {
            base.TableName = "paper_links";
        }

        public override string SqlAddNewSql
        {
            get
            {
                return @"INSERT INTO `paper_links` (`Paper_id`,`Parent_id`,`Able_flag`) VALUES ( '{0}', '{1}', '1' );";
            }
        }

        public override string SqlUpdateSql
        {
            get
            {
                return @"UPDATE paper_links SET able_flag = {2} WHERE paper_id = '{0}' AND parent_id = '{1}';";
            }
        }

        public override string KeyId
        {
            get
            {
                return "link_id";
            }
        }

        
        public void BuildPaperTree(Galant.DataEntity.Paper paper)
        {
            if (!paper.IsCollection)
                return;

            string paperId = ExistLinkData(paper.PaperId);
            string sqlInsert;
            if (string.IsNullOrEmpty(paperId))
            {
                sqlInsert = string.Format(SqlAddNewSql, paper.PaperId, 0);
                SqlHelper.ExecuteNonQuery(Operator.mytransaction, System.Data.CommandType.Text, sqlInsert);
                paperId = ReadLastInsertId();
            }

            foreach (Galant.DataEntity.Paper info in paper.ChildPapers)
            {
                string linkid = ExistLinkData(info.PaperId);
                if (!string.IsNullOrEmpty(linkid))
                {
                    string sqlUpdate = string.Format(SqlUpdateSql, info.PaperId, paperId, false);
                    SqlHelper.ExecuteNonQuery(Operator.mytransaction, System.Data.CommandType.Text, sqlUpdate);
                }
                sqlInsert = string.Format(SqlAddNewSql, info.PaperId, paperId);
                SqlHelper.ExecuteNonQuery(Operator.mytransaction, System.Data.CommandType.Text, sqlInsert);
            }
        }

        string existLink = "SELECT parent_id FROM paper_links WHERE Paper_Id = '{0}' AND Able_flag = {1} LIMIT 1";
        private string ExistLinkData(String paperId)
        {
            string linkid = string.Empty;
            string sqlExist = string.Format(existLink, paperId, true);
            object obj = MySql.Data.MySqlClient.MySqlHelper.ExecuteScalar(Operator.myConnection, sqlExist);
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            {
                linkid = obj.ToString();
            }
            return linkid;
        }
    }
}