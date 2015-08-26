using System;
using System.Globalization;
using DbKeeperNet.Engine.Resources;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Handler of <see cref="CustomUpdateStepType"/>
    /// </summary>
    public class CustomUpdateStepHandlerService : UpdateStepHandlerService<CustomUpdateStepType>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        protected override void Handle(CustomUpdateStepType castedStep, IUpdateContext context)
        {
            Type type = Type.GetType(castedStep.Type);

            if (type == null)
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, UpdateStepVisitorMessages.CustomStepTypeNotFound, castedStep.Type));

            ICustomUpdateStep customStep = (ICustomUpdateStep)Activator.CreateInstance(type);
            customStep.ExecuteUpdate(context, castedStep.Param);
        }
    }
}