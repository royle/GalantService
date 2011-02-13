using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                return @"insert into papers(
paper_id,Status,SubState,Holder,Bound,Contact_a,Contact_b,Contact_c,Deliver_a,Deliver_b,Deliver_c,Deliver_a_time,Deliver_b_time,Deliver_c_time,Start_time,Finish_time,Salary,Comment,Type,Next_Route,Mobile_Status)
values (
@paper_id,@Status,@SubState,@Holder,@Bound,@Contact_a,@Contact_b,@Contact_c,@Deliver_a,@Deliver_b,@Deliver_c,@Deliver_a_time,@Deliver_b_time,@Deliver_c_time,@Start_time,@Finish_time,@Salary,@Comment,@Type,@Next_Route,@Mobile_Status)";
            }
        }

        public override string KeyId
        {
            get
            {
                return "PaperId";
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
        }

        protected override void SetTableName()
        {
            TableName = "papers";
        }
    }
}