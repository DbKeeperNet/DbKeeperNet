using System;
using DbKeeperNet.Engine.Resources;

namespace DbKeeperNet.Engine
{
    public partial class UpdateStepBaseType : IVisitableUpdateStep
    {
        public virtual void Accept(IUpdateStepVisitor visitor)
        {
            throw new InvalidOperationException(UpdaterMessages.UnsupportedUpdateStepType);
        }
    }

    public partial class AspNetAccountDeleteUpdateStepType
    {
        public override void Accept(IUpdateStepVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public partial class AspNetAccountCreateUpdateStepType
    {
        public override void Accept(IUpdateStepVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public partial class UpdateDbStepType
    {
        public override void Accept(IUpdateStepVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public partial class CustomUpdateStepType
    {
        public override void Accept(IUpdateStepVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public partial class AspNetRoleCreateUpdateStepType
    {
        public override void Accept(IUpdateStepVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}