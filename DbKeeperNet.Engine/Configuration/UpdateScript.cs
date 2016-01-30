namespace DbKeeperNet.Engine.Configuration
{
    public class UpdateScript : IUpdateScriptConfigurationElement
    {
        public string Location { get; set; }
        public string Provider { get; set; }
    }
}