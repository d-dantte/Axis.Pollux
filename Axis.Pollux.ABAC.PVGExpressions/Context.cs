using Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sigma = Axis.Sigma.Core;

namespace Axis.Pollux.ABAC.PVGExpressions
{
    public class TargetContext: ExpressionContext
    {
        public TargetContext()
        {
            //imports here

            Owner = this;
        }

        public sigma.Action action { get; set; }
        public sigma.Resource resource { get; set; }
        public sigma.Subject subject { get; set; }
    }

    public abstract class AttributeExpressionContext<V>: ExpressionContext
    where V: sigma.IAttributeContainer
    {
        protected AttributeExpressionContext()
        {
            //imports here

            Owner = this;
        }

        public abstract void SetValue(V value);
    }

    public class ActionContext : AttributeExpressionContext<sigma.Action>
    {
        internal ActionContext()
        { }

        public sigma.Action action { get; set; }

        public override void SetValue(sigma.Action value) => action = value;
    }

    public class EnvironmentContext : AttributeExpressionContext<sigma.Environment>
    {
        internal EnvironmentContext()
        { }

        public sigma.Environment environment { get; set; }
        public sigma.Environment env
        {
            get { return this.environment; }
            set { environment = value; }
        }

        public override void SetValue(sigma.Environment value) => env = value;
    }

    public class ResourceContext : AttributeExpressionContext<sigma.Resource>
    {
        internal ResourceContext()
        { }

        public sigma.Resource resource { get; set; }

        public override void SetValue(sigma.Resource value) => resource = value;
    }

    public class SubjectContext : AttributeExpressionContext<sigma.Subject>
    {
        internal SubjectContext()
        { }

        public sigma.Subject subject { get; set; }

        public override void SetValue(sigma.Subject value) => subject = value;
    }
}
