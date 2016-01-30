using System;
using System.IO;
using System.Reflection;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Registration of new update steps
    /// </summary>
    /// <see cref="CustomUpdateStepHandlerService"/>
    /// <see cref="UpdateDbStepHandlerService"/>
    public class DefaultUpdateStepsExtension : IExtension
    {
        /// <summary>
        /// Method invoked for extension initialization. Register
        /// all provided services and new preconditions here.
        /// </summary>
        /// <param name="context">Current update context</param>
        /// <see cref="IUpdateContext.RegisterPrecondition"/>
        /// <see cref="IUpdateContext.RegisterDatabaseService"/>
        /// <see cref="IUpdateContext.RegisterLoggingService"/>
        public void Initialize(IUpdateContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            using(var schemaReader = new StreamReader(typeof(Updater).GetManifestResourceFromTypeAssembly(@"DbKeeperNet.Engine.Resources.Updates-1.0.xsd")))
            {
                context.RegisterSchema("http://code.google.com/p/dbkeepernet/Updates-1.0.xsd", schemaReader.ReadToEnd());    
            }
            
            context.RegisterUpdateStepHandler(new CustomUpdateStepHandlerService());
            context.RegisterUpdateStepHandler(new UpdateDbStepHandlerService(new SqlScriptSplitter()));
        }
    }
}
