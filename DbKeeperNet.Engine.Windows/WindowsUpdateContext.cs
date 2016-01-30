namespace DbKeeperNet.Engine.Windows
{
    /// <summary>
    /// Windows update context
    /// </summary>
    /// <remarks>
    /// Utilizes <c>App.Config</c> or <c>Web.Config</c>
    /// to perform configuration using <seealso cref="DbKeeperNetConfigurationSection"/> configuration section.
    /// </remarks>
    /// <see cref="DbKeeperNetConfigurationSection"/>
    public class WindowsUpdateContext : UpdateContext
    {
        public WindowsUpdateContext()
            : this(DbKeeperNetConfigurationSection.Current)
        {
            
        }

        public WindowsUpdateContext(IDbKeeperNetConfigurationSection section) : base(section)
        {
        }
    }
}