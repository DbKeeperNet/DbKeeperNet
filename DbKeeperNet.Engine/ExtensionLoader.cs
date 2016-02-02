using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Helper class which loads and executes Initialize() method of all
    /// classes implementing IExtension interface. 
    /// </summary>
    /// <remarks>
    /// Extension may be located also in external assemblies. In this
    /// case is path absolute or relative to entry assembly. 
    /// If entry assembly can't be determined, path is relative to
    /// this assembly.
    /// </remarks>
    /// <see cref="IExtension"/>
    public static class ExtensionLoader
    {
        /// <summary>
        /// Load all configured assemblies with extension
        /// within defined context.
        /// </summary>
        /// <param name="context">Context to which may be extensions registered.</param>
        public static void LoadExtensions(IUpdateContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            foreach (var e in context.ConfigurationSection.Extensions)
                LoadExtensionsFromAssembly(context, e.Assembly);
        }

        private static void LoadExtensionsFromAssembly(IUpdateContext context, string assemblyName)
        {
            var assembly = Assembly.Load(new AssemblyName(assemblyName));
            var assemblyTypes = assembly.DefinedTypes;

            var interfaceType = typeof(IExtension);

            foreach (var type in assemblyTypes)
            {
                if ((type.ImplementedInterfaces.Contains(interfaceType)) && (!type.IsInterface))
                {
                    IExtension extension = (IExtension)Activator.CreateInstance(type.GetElementType());
                    extension.Initialize(context);
                }
            }
        }
    }
}
