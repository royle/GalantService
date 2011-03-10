using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLTService.Operation
{
    public class Search
    {
        public Galant.DataEntity.Result.SearchEntityResult SearchEntitys(Operation.BaseEntity.DataOperator data, Galant.DataEntity.Result.SearchEntityResult searchResult)
        {
            GLTService.Operation.BaseEntity.Entity entity = new BaseEntity.Entity();
            searchResult.ResultData = entity.GetEntitysByConditions(data, searchResult.SearchCondition);
            return searchResult;
        }

        public Galant.DataEntity.Production.Result SearchProduction(Operation.BaseEntity.DataOperator data, Galant.DataEntity.Production.Result result)
        {
            GLTService.Operation.BaseEntity.Product product = new BaseEntity.Product();
            result.ResultData = product.SearchProductes(data, result.SearchCondition);
            return result;
        }
    }
}