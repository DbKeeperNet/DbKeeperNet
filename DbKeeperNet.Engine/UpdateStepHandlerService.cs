using System;
using System.Globalization;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Base helper class implementing <see cref="IUpdateStepHandlerService"/>
    /// </summary>
    /// <typeparam name="T">Handled XML element represented by .NET class</typeparam>
    public abstract class UpdateStepHandlerService<T> : IUpdateStepHandlerService
        where T : UpdateStepBaseType
    {
        /// <summary>
        /// Type of .NET class representing the given XML element
        /// </summary>
        public Type HandledType { get { return typeof (T); } }

        /// <summary>
        /// Method called to handle the given <see cref="IUpdateStepHandlerService.HandledType"/>.
        /// </summary>
        /// <param name="updateStep">An instance of <see cref="IUpdateStepHandlerService.HandledType"/></param>
        /// <param name="context">Update context providing necessary session data for upgrade execution</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "CustomUpdateStepType")]
        public void Handle(UpdateStepBaseType updateStep, IUpdateContext context)
        {
            var castedStep = updateStep as T;
            if (castedStep == null) throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, "Cannot cast update step to {0}", typeof(T)));

            Handle(castedStep, context);
        }

        /// <summary>
        /// Handler method called with properly casted update step.
        /// </summary>
        /// <param name="castedStep">Update step casted to <typeparamref name="T"/></param>
        /// <param name="context">Update context</param>
        protected abstract void Handle(T castedStep, IUpdateContext context);

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "Update step handler for {0}", typeof (T).Name);
        }
    }
}