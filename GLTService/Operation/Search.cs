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
            GLTService.Operation.BaseEntity.Entity entity = new BaseEntity.Entity(data);
            searchResult.ResultData = entity.GetEntitysByConditions(data, searchResult.SearchCondition);
            return searchResult;
        }
        /// <summary>
        /// 获取归班运送单列表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="searchResult"></param>
        /// <returns></returns>
        public Galant.DataEntity.Result.FinishingListResult SearchFinishingList(Operation.BaseEntity.DataOperator data, Galant.DataEntity.Result.FinishingListResult searchResult)
        {
            GLTService.Operation.BaseEntity.Paper paper = new BaseEntity.Paper(data);
            searchResult.ResultData = paper.GetFinishList(data, searchResult.SearchCondition.Station);
            return searchResult;
        }

        public Galant.DataEntity.Production.Result SearchProduction(Operation.BaseEntity.DataOperator data, Galant.DataEntity.Production.Result result)
        {
            GLTService.Operation.BaseEntity.Product product = new BaseEntity.Product(data);
            result.ResultData = product.SearchProductes(data, result.SearchCondition);
            return result;
        }

        public Galant.DataEntity.Assign.Result SearchCenterRoute(Operation.BaseEntity.DataOperator data, Galant.DataEntity.Assign.Result result)
        {
            Assign.CenterAssign center = new Assign.CenterAssign(data);
            result.ResultData = center.ReadCenterAssgin();
            result.Entities = center.ReadAllStationEntity();
            return result;
        }
    }
}