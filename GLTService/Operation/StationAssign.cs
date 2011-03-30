using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLTService.Operation.BaseEntity;

namespace GLTService.Operation
{
    public class StationAssign:BaseOperator
    {
        public StationAssign(DataOperator data) : base(data) { }

        public readonly string sqlSelect = @"SELECT PAPER_ID,SUBSTATE,HOLDER,BOUND,CONTACT_A,CONTACT_B,`TYPE`,NEXT_ROUTE FROM PAPERS
WHERE `TYPE`= 1 AND HOLDER = '{0}' AND SUBSTATE <= 36 ";


    }
}