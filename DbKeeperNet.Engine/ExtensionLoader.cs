using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Helper class which loads and executes Initialize() method of all
    /// classes implementing IExtension interface. 
    /// Extension may be located also in external assemblies. In this
    /// case is path absolute or relative to entry assembly. 
    /// If entry assembly can't be determined, path is relative to
    /// this assembly.
    /// </summary>
    /// <see cref="IExtension"/>
    public sealed class ExtensionLoader
    {
        private ExtensionLoader()
        {
        }
        /// <summary>
        /// Load all configured assemblies with extension
        /// within defined context.
        /// </summary>
        /// <param name="context">Context to which may be extensions registered.</param>
        public static void LoadExtensions(IUpdateContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            foreach (ExtensionConfigurationElement e in context.ConfigurationSection.Extensions)
                LoadExtensionsFromAssembly(context, e.Assembly);
        }

        private static void LoadExtensionsFromAssembly(IUpdateContext context, string assemblyName)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            Type[] assemblyTypes = assembly.GetExportedTypes();
            Type interfaceType = typeof(IExtension);

            foreach (Type type in assemblyTypes)
            {
                if ((interfaceType.IsAssignableFrom(type)) && (!type.IsInterface))
                {
                    IExtension extension = (IExtension)Activator.CreateInstance(type);
                    extension.Initialize(context);
                }
            }
        }
    }
}
