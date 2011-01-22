using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galant.DataEntity
{
    public class Route:BaseData
    {
        private int routeId;

        public int RouteId
        {
            get { return routeId; }
            set { routeId = value; }
        }
        private string RountName;

        public string RountName1
        {
            get { return RountName; }
            set { RountName = value; }
        }
        private Entity fromEntity;

        public Entity FromEntity
        {
            get { return fromEntity; }
            set { fromEntity = value; }
        }
        private Entity toEntity;

        public Entity ToEntity
        {
            get { return toEntity; }
            set { toEntity = value; }
        }
        private bool isFinally;

        public bool IsFinally
        {
            get { return isFinally; }
            set { isFinally = value; }
        }
    }
}
