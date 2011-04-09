using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using GLTService.DBConnector;
using MySql.Data.MySqlClient;
using GLTService.Operation;
using GLTService.Operation.BaseEntity;
using GLTService.Operation.Assign;

namespace GLTService
{
    public class ProcessSwitch
    {
        public static Galant.DataEntity.BaseData ProcessRequest(Galant.DataEntity.BaseData DetailObj, Galant.DataEntity.Entity staff, string OperationType)
        {
            Operation.BaseEntity.DataOperator dataOper = new Operation.BaseEntity.DataOperator();
            dataOper.CreateConnectionAndTransaction();
            Galant.DataEntity.BaseData returnData = null;
            try
            {
                Entity entity = new Entity(dataOper);
                Galant.DataEntity.Entity AuthorizedStaff = entity.Authorize(dataOper, staff.Alias, staff.Password, false);

                GLTService.Operation.Search search;

                switch (OperationType)
                {
                    case "Login":
                        GLTService.Operation.Login log = new Operation.Login();
                        returnData = log.InitAppCach(dataOper, AuthorizedStaff);
                        break;
                    case "SearchEntity":
                        search = new Search();
                        returnData = search.SearchEntitys(dataOper, DetailObj as Galant.DataEntity.Result.SearchEntityResult);
                        break;
                    case "SearchProduction":
                        search = new Search();
                        returnData = search.SearchProduction(dataOper, DetailObj as Galant.DataEntity.Production.Result);
                        break;
                    case "SearchCenterRoute":
                        search = new Search();
                        returnData = search.SearchCenterRoute(dataOper, DetailObj as Galant.DataEntity.Assign.Result);
                        break;
                    case "SearchStationAssign":
                        search = new Search();
                        returnData = search.SearchStationAssign(dataOper, DetailObj as Galant.DataEntity.StationAssign.Result);
                        break;
                    case "SearchFinishingList":
                        search = new Search();
                        returnData = search.SearchFinishingList(dataOper, DetailObj as Galant.DataEntity.Result.FinishingListResult);
                        break;
                    case "CheckinFinish":
                        Paper paper = new Paper(dataOper);
                        returnData = paper.GetCheckinPaperListByCollection(dataOper, DetailObj as Galant.DataEntity.Result.FinishCheckin);
                        break;
                    case "Refresh":
                        returnData = ProcessRefresh(dataOper, DetailObj);
                        break;
                    case "Save":
                    case "SaveStationAssign":
                        returnData = ProcessSave(dataOper, DetailObj);
                        break;
                    case "SaveRoute":
                        returnData = ProcessSave(dataOper, DetailObj);
                        break;
                    default:
                        break;
                }
                dataOper.CommitAndClose();
            }
            
            catch (Exception ex)
            {
                dataOper.RollBackAndClose();
                throw ex;
            }
            return returnData == null ? DetailObj : returnData;
        }

        private static Galant.DataEntity.BaseData ProcessRefresh(DataOperator dataOper,Galant.DataEntity.BaseData detailObj)
        {
            if (detailObj is Galant.DataEntity.Entity)
            {
                Entity entity = new Entity(dataOper);
                detailObj = entity.GetEntityByID(dataOper, detailObj.QueryId, true);
            }
            return detailObj;
        }

        private static Galant.DataEntity.BaseData ProcessSave(DataOperator dataOper, Galant.DataEntity.BaseData detailObj)
        {
            if (detailObj is Galant.DataEntity.Entity)
            {
                Entity op = new Entity(dataOper);
                detailObj = op.SaveEntity(dataOper,detailObj as Galant.DataEntity.Entity);
            }
            else if (detailObj is Galant.DataEntity.Product)
            {
                Product op = new Product(dataOper);
                if (((Galant.DataEntity.Product)detailObj).ProductId == 0)
                    op.AddNewData(detailObj);
                else
                    op.UpdateData(detailObj);
            }
            else if (detailObj is Galant.DataEntity.Paper)
            {
                Paper op = new Paper(dataOper);
                if (string.IsNullOrEmpty((detailObj as Galant.DataEntity.Paper).PaperId))
                {
                    op.AddNewData(detailObj);
                }
                else
                {

                }
            }
            else if (detailObj is Galant.DataEntity.Assign.Result)
            {
                CenterAssign assign = new CenterAssign(dataOper);
                assign.UpdatePaper(((Galant.DataEntity.Assign.Result)detailObj).ResultData);
            }
            else if (detailObj is Galant.DataEntity.Result.FinishCheckin)
            {
                GLTService.Operation.Checkin.FinishCheckin checkIn = new Operation.Checkin.FinishCheckin(dataOper);
                checkIn.CheckinData(detailObj as Galant.DataEntity.Result.FinishCheckin);
            }
            else if (detailObj is Galant.DataEntity.StationAssign.Result)
            {
                GLTService.Operation.StationAssign op = new StationAssign(dataOper);
                op.UpdatePaper((Galant.DataEntity.StationAssign.Result)detailObj);
            }
            return detailObj;
        }
    }
}
