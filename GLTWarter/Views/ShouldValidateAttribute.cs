using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLTWarter.Views
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    class ShouldValidateAttribute : Attribute
    {
        public ShouldValidateAttribute() : this(0) { }
        public ShouldValidateAttribute(int order)
        {
            this.Order = order;
        }
        public int Order { get; set; }
    }
}
