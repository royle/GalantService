using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity
{
    [DataContract]
    public class Route:BaseData
    {
        public Route() : base() { }

        private int routeId;

        public int RouteId
        {
            get { return routeId; }
            set { routeId = value; }
        }
        private string rountName;

        public string RountName
        {
            get { return rountName; }
            set { rountName = value; }
        }
        private Entity fromEntity;

        public Entity FromEntity
        {
            get { return fromEntity; }
            set { fromEntity = value; }
        }
        public int? FromEntityId
        {
            get { return FromEntity.EntityId; }
        }
        private Entity toEntity;

        public Entity ToEntity
        {
            get { return toEntity; }
            set { toEntity = value; }
        }
        public int? ToEntityId
        {
            get { return ToEntity.EntityId; }
        }

        private bool isFinally;
        public bool IsFinally
        {
            get { return isFinally; }
            set { isFinally = value; }
        }
    }
}
