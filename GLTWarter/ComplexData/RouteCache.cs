using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Galant.DataEntity;
using System.Timers;
using System.Globalization;
using System.Collections;

namespace GLTWarter.ComplexData
{
    public class RouteCache
    {
        static object lockSingleton = new object();
        static RouteCache m_Singleton;
        static public RouteCache Singleton
        {
            get {
                if (m_Singleton == null)
                {
                    lock (lockSingleton)
                    {
                        if (m_Singleton == null)
                        {
                            m_Singleton = new RouteCache();
                        }
                    }
                }
                return m_Singleton;
            }
        }

        //bool searchStarted;
        //Timer retry;

        //public RouteCache()
        //{
        //    retry = new Timer(3000);
        //    retry.AutoReset = false;
        //    retry.Elapsed += new ElapsedEventHandler(retry_Elapsed);
        //}

        //void retry_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    StartSearch();
        //}

        //public void StartFirstSearch()
        //{
        //    lock (this)
        //    {
        //        if (searchStarted || retry.Enabled) return;
        //        StartSearch();
        //    }
        //}

        //public void StartSearch()
        //{
        //    lock (this)
        //    {
        //        if (searchStarted) return;
        //        RouteCacheSearch search = new RouteCacheSearch();
        //        search.BeginSearch(SearchCallback, search);
        //        searchStarted = true;
        //    }
        //}

        //void SearchCallback(IAsyncResult asr)
        //{
        //    lock (this)
        //    {
        //        searchStarted = false;
        //    }

        //    RouteCacheSearch search = (RouteCacheSearch)asr.AsyncState;

        //    try
        //    {
        //        List<Route> data = search.EndSearch(asr, typeof(ResultData<Data.Route>)) as ResultData<Data.Route>;
        //        if (data.Data != null)
        //        {
        //            CookedRoutes cooked = new CookedRoutes();
        //            cooked.Routes = data.Data;
        //            foreach (Route r in cooked.Routes)
        //            {
        //                if (r.FromEntity != null)
        //                {
        //                    int k = r.FromEntity.EntityId.Value;
        //                    if (!cooked.RoutesFrom.ContainsKey(k)) { cooked.RoutesFrom[k] = new List<Route>(); }
        //                    cooked.RoutesFrom[k].Add(r);
        //                }
        //                if (r.ToEntity != null)
        //                {
        //                    int k = r.ToEntity.EntityId.Value;
        //                    if (!cooked.RoutesTo.ContainsKey(k)) { cooked.RoutesTo[k] = new List<Route>(); }
        //                    cooked.RoutesTo[k].Add(r);
        //                }
        //                cooked.RoutesMap[r.RouteId.Value] = r;
        //            }
        //            CookedRoutes = cooked;

        //            retry.Interval = 3600 * 1000;
        //            retry.Enabled = true;
        //            return;
        //        }
        //    }
        //    catch (System.Net.WebException)
        //    {
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    retry.Interval = 3000;
        //    retry.Enabled = true;
        //}

        CookedRoutes cookedRoutes = new CookedRoutes();
        public CookedRoutes CookedRoutes
        {
            get { return cookedRoutes; }
            set
            {
                lock (this)
                {
                    cookedRoutes = value;
                }
            }
        }

        static public Route[] AllRoutes
        {
            get { return Singleton.CookedRoutes.Routes; }
        }

        static public Route[] AllRoutesFrom(Entity start)
        {
            HashSet<string> visited = new HashSet<string>();
            List<Route> routes = new List<Route>();
            FetchRoutes(start, Singleton.CookedRoutes, routes, visited);
            return routes.ToArray();
        }

        static public IEnumerable<Route> OneHopRoutesFrom(Entity start)
        {
            CookedRoutes cooked = Singleton.CookedRoutes;
            if (start == null || !start.EntityId.HasValue || !cooked.RoutesFrom.ContainsKey(start.EntityId.Value)) return new Route[0];
            return cooked.RoutesFrom[start.EntityId.Value];
        }

        static public IEnumerable<Route> OneHopRoutesTo(Entity end)
        {
            CookedRoutes cooked = Singleton.CookedRoutes;
            if (end == null || !end.EntityId.HasValue || !cooked.RoutesTo.ContainsKey(end.EntityId.Value)) return new Route[0];
            return cooked.RoutesTo[end.EntityId.Value];
        }

        /// <summary>
        /// 返回从开始站点到目的站点的最短路线集合
        /// </summary>
        /// <param name="fromEntity"></param>
        /// <param name="toEntity"></param>
        /// <returns></returns>
        static public List<Route> GetRouteFrom(Entity fromEntity, Entity toEntity)
        {
            return AnalyzeRoute.GetRouteFrom(fromEntity, toEntity);
        }

        static private void FetchRoutes(Entity start, CookedRoutes cooked, List<Route> routes, HashSet<string> visited)
        {
            if (!visited.Contains(start.QueryId))
            {
                visited.Add(start.QueryId);
                List<Route> discover = new List<Route>();
                if (cooked.RoutesFrom.ContainsKey(start.EntityId.Value))
                {
                    foreach (Route r in cooked.RoutesFrom[start.EntityId.Value])
                    {
                        if (r.ToEntity == null)
                        {
                            // Only Last Mile
                            routes.Add(r);
                        }
                        else
                        {
                            // Or Internal Transfer Type
                            if (r.ToEntity.EntityType == EntityType.Station && !visited.Contains(r.ToEntity.QueryId)) // Only LastMile or Transfer
                            {
                                routes.Add(r);
                            }
                            if (!r.IsFinally) { discover.Add(r); }
                        }
                    }
                }
                foreach (Route r in discover)
                {
                    FetchRoutes(r.ToEntity, cooked, routes, visited);
                }
            }
        }

        static public IEnumerable<Route> PathTo(Entity start, Route end)
        {
            if (start == null || end == null || (end.ToEntity != null && end.ToEntity.EntityEquals(start)))
            {
                return new Route[0];
            }
            CookedRoutes cooked = Singleton.CookedRoutes;
            Dictionary<int, Route> visited = new Dictionary<int, Route>();
            Queue<Route> queue = new Queue<Route>();

            visited.Add(start.EntityId.Value, null);
            if (cooked.RoutesFrom.ContainsKey(start.EntityId.Value))
            {
                cooked.RoutesFrom[start.EntityId.Value].ForEach(y => queue.Enqueue(y));
            }

            while (queue.Count > 0)
            {
                Route r = queue.Dequeue();
                if (r.EntityEquals(end))
                {
                    // Winner is found. Backtrack!
                    LinkedList<Route> result = new LinkedList<Route>();
                    while (r != null)
                    {
                        result.AddFirst(r);
                        r = visited[r.FromEntity.EntityId.Value];
                    }
                    return result;
                }
                else
                {
                    // If we didn't visit here before, mark it now
                    if (r.ToEntity != null && !visited.ContainsKey(r.ToEntity.EntityId.Value))
                    {
                        visited[r.ToEntity.EntityId.Value] = r;
                        if (!r.IsFinally)
                        {
                            // Push more into the search queue if it's not an edge
                            if (cooked.RoutesFrom.ContainsKey(r.ToEntity.EntityId.Value))
                            {
                                cooked.RoutesFrom[r.ToEntity.EntityId.Value].ForEach(y => queue.Enqueue(y));
                            }
                        }
                    }
                }
            }
            // Nothing is found
            return new Route[0];
        }

        public Route[] GetRoutes(int[] id)
        {
            lock (this)
            {
                return (from i in id select GetRoute(i)).ToArray();
            }
        }
        public Route GetRoute(int id)
        {
            CookedRoutes cooked = CookedRoutes;
            if (cooked.RoutesMap.ContainsKey(id))
            {
                return cooked.RoutesMap[id];
            }
            else
            {
                return MakeDefaultRoute(id);
            }
        }

        Route MakeDefaultRoute(int id)
        {
            Route r = new Route();
            r.RouteId = id;
            r.RountName = String.Format(CultureInfo.InvariantCulture, Resource.converterRouteCacheNewRoute, id);
            r.IsLoading = false;
            return r;
        }
    }

    public class RouteCacheSearch : BaseData
    {
        public RouteCacheSearch() : base("Route") { }
        public List<Route> Routes
        { get; set; }
    }

    public class CookedRoutes
    {
        public CookedRoutes()
        {
            this.Routes = new Route[0];
            this.RoutesFrom = new Dictionary<int, List<Route>>();
            this.RoutesTo = new Dictionary<int, List<Route>>();
            this.RoutesMap = new Dictionary<int, Route>();
        }
        public Route[] Routes { get; set; }
        public Dictionary<int, List<Route>> RoutesFrom { get; set; }
        public Dictionary<int, List<Route>> RoutesTo { get; set; }
        public Dictionary<int, Route> RoutesMap { get; set; }
    }


    public class AnalyzeRoute
    {
        #region 成员变量       
        /// <summary>
        /// 所有站点
        /// </summary>
        private static List<Entity> allStation;
        /// <summary>
        /// 所有站点
        /// </summary>
        private static List<Entity> AllStation
        {
            get { return allStation; }
            set { allStation = value; }
        }
        private static List<Entity> InitAllStation()
        {
            List<Entity> allFromStation = (from r in RouteCache.AllRoutes where r.FromEntity !=null select r.FromEntity).Union(from r in RouteCache.AllRoutes where r.ToEntity != null select r.ToEntity).Distinct().ToList();
            return allFromStation;
        }

        #endregion

        #region 最短路线计算
        /// <summary>
        /// 计算重开始站点到各站点的最短路线
        /// </summary>
        /// <param name="startEntity"></param>
        /// <returns></returns>
        private static List<Node> InitNodes(Entity startEntity)
        {
            List<Node> RouteNodes = new List<Node>();//未处理的站点集合
            List<Node> RouteNodesBackup = new List<Node>();//已处理的站点集合
            Dictionary<int?, Node> NodeDict = new Dictionary<int?, Node>();
            AllStation = InitAllStation();
            foreach (Entity e in AllStation)
            {
                Node d = new Node();
                d.cost = e.EntityId == startEntity.EntityId ? 0 : int.MaxValue;
                d.ToEntity = e;
                RouteNodes.Add(d);
                NodeDict.Add(e.EntityId, d);
            }
            while (true)
            {
                Node rdFrom = (from r in RouteNodes orderby r.cost select r).FirstOrDefault();
                if (rdFrom == null || rdFrom.cost >= int.MaxValue)
                    break;
                foreach (Route route in RouteCache.OneHopRoutesFrom(rdFrom.ToEntity))
                {
                    if (route.ToEntity == null)
                        continue;
                    Node rdTo = NodeDict[route.ToEntity.EntityId];
                    if (rdTo != null && rdTo.cost > rdFrom.cost + 1)
                    {
                        rdTo.cost = rdFrom.cost + 1;
                        rdTo.PrevRoute = route;
                    }
                }
                RouteNodes.Remove(rdFrom);
                RouteNodesBackup.Add(rdFrom);
            }
            RouteNodesBackup = (from r in RouteNodesBackup orderby r.cost select r).ToList();
            FinalizeRoutePath(RouteNodesBackup);
            return RouteNodesBackup;
        }
        /// <summary>
        /// 获取路线列表
        /// </summary>
        /// <param name="routeRoutes"></param>
        private static void FinalizeRoutePath(List<Node> routeNodes)
        {
            foreach (Node r in routeNodes)
            {
                if (r.PrevRoute == null)
                    continue;
                r.Routes.Insert(0, r.PrevRoute);
                Node rPerv = (from rp in routeNodes where rp.ToEntity.EntityId == r.PrevRoute.FromEntity.EntityId select rp).FirstOrDefault();
                while (rPerv != null && rPerv.PrevRoute != null)
                {
                    r.Routes.Insert(0, rPerv.PrevRoute);
                    rPerv = (from rp in routeNodes where rp.ToEntity.EntityId == rPerv.PrevRoute.FromEntity.EntityId select rp).FirstOrDefault();
                }
            }
        }
        #endregion

        /// <summary>
        /// 返回从开始站点到目的站点的最短路线集合
        /// </summary>
        /// <param name="fromEntity"></param>
        /// <param name="toEntity"></param>
        /// <returns></returns>
        public static List<Route> GetRouteFrom(Entity fromEntity, Entity toEntity)
        {
            List<Node> RouteNodes = InitNodes(fromEntity);
            Node routeDijk = (from rd in RouteNodes where rd.ToEntity.EntityId == toEntity.EntityId select rd).FirstOrDefault();
            return routeDijk != null ?
                    routeDijk.Routes.Count > 0 ? routeDijk.Routes : null
                    : null;
        }

        /// <summary>
        /// 最短路线结构
        /// </summary>
        private class Node
        {
            public Entity ToEntity;
            public Route PrevRoute;
            public List<Route> Routes = new List<Route>();
            public int cost;
        }
    }
}
