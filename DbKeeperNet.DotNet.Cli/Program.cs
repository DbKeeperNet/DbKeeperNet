using System.IO;
using System.Xml;
using System.Xml.Serialization;
using McMaster.Extensions.CommandLineUtils;

namespace DbKeeperNet.DotNet.Cli
{
    class Program
    {
        private const string HelpOption = "-? | -h | --help";
        private const string DriverOption = "-d | --driver";
        private const string DriverDescription = "Database driver [sqlite|mssql|mysql|firebird|all]";

        static int Main(string[] args)
        {
            var app = new CommandLineApplication {Description = "DbKeeperNet Command Line Interface"};

            app.HelpOption(HelpOption);
            /*
            app.Command("init", c =>
            {
                c.Description = "Initializes new project for DbKeeperNet upgrades";
                c.HelpOption(HelpOption);

                c.Option(DriverOption, DriverDescription, CommandOptionType.SingleValue,
                    o => { });
                c.Option("-c | --console", "Create console application", CommandOptionType.NoValue);
            });
            */
            app.Command("new", c =>
            {
                c.Description = "Creates new DbKeeperNet upgrade script";
                c.HelpOption(HelpOption);
                var assemblyOption = c.Option("-a | --assembly", "Define assembly attribute value", CommandOptionType.SingleValue);
                var versionOption = c.Option("-v | --version", "Sets the update version", CommandOptionType.SingleValue);
                var driverOption = c.Option(DriverOption, DriverDescription, CommandOptionType.SingleValue);

                c.OnExecute(() =>
                {
                    var assembly = assemblyOption.Value() ?? "DbKeeperNet.SampleAssembly";
                    var version = versionOption.Value() ?? "1.00";
                    var driver = driverOption.Value() ?? "all";

                    WriteUpdateScript(assembly, version, driver);
                });
            });

            return app.Execute(args);
        }

        private static void WriteUpdateScript(string assembly, string version, string driver)
        {
            var updates = new Updates
            {
                AssemblyName = assembly,
                DefaultPreconditions = new[]
                {
                    new PreconditionType
                    {
                        FriendlyName = "Update step executed",
                        Precondition = "StepNotExecuted"
                    }
                },
                Update = new[]
                {
                    new UpdateType
                    {
                        FriendlyName = $"Upgrade {version}",
                        Version = version,
                        UpdateStep = new UpdateStepBaseType[]
                        {
                            new UpdateDbStepType
                            {
                                FriendlyName = "Sample step",
                                Id = 1,
                                AlternativeStatement = new[]
                                {
                                    new UpdateDbAlternativeStatementType
                                    {
                                        DbType = driver,
                                        Value = "\ncreate table x\n(\n    id int not null\n)\n"
                                    }
                                }
                            }
                        }
                    }
                }
            };
            var serializer = new XmlSerializer(updates.GetType());
            using (var writer = File.CreateText("out.xml"))
            {
                serializer.Serialize(
                    writer,
                    updates,
                    new XmlSerializerNamespaces(
                        new[]
                        {
                            new XmlQualifiedName("upd", "http://code.google.com/p/dbkeepernet/Updates-1.0.xsd"),
                            new XmlQualifiedName("xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                        }));
            }
        }
    }
}
