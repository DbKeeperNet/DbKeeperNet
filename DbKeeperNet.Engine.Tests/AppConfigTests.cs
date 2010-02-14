using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Configuration;

namespace DbKeeperNet.Engine.Tests
{
    [TestFixture]
    public class AppConfigTests
    {
        [Test]
        public void TestDefaultValues()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "TestDefaultValues.config";
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap,
                                    ConfigurationUserLevel.None);
            DbKeeperNetConfigurationSection section = (DbKeeperNetConfigurationSection)config.GetSection("dbkeeper.net");

            Assert.That(section.LoggingService, Is.EqualTo("dummy"));

            Assert.That(section.Extensions.Count, Is.EqualTo(1));

            Assert.That(section.DatabaseServiceMappings.Count, Is.EqualTo(0));
            Assert.That(section.UpdateScripts.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestReadingConfig()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "TestConfiguredValues.config";
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap,
                                    ConfigurationUserLevel.None);
            DbKeeperNetConfigurationSection section = (DbKeeperNetConfigurationSection)config.GetSection("dbkeeper.net");

            Assert.That(section.LoggingService, Is.EqualTo("fx"));

            Assert.That(section.Extensions.Count, Is.EqualTo(2));
            Assert.That(section.Extensions[0].Assembly, Is.EqualTo("MyAsm.dll"));
            Assert.That(section.Extensions[1].Assembly, Is.EqualTo("MyAsm2.dll"));

            Assert.That(section.DatabaseServiceMappings.Count, Is.EqualTo(2));
            Assert.That(section.DatabaseServiceMappings[0].ConnectString, Is.EqualTo("mock"));
            Assert.That(section.DatabaseServiceMappings[0].DatabaseService, Is.EqualTo("MockDriver"));
            Assert.That(section.DatabaseServiceMappings[1].ConnectString, Is.EqualTo("mssql"));
            Assert.That(section.DatabaseServiceMappings[1].DatabaseService, Is.EqualTo("MsSql"));

            Assert.That(section.UpdateScripts.Count, Is.EqualTo(2));
            Assert.That(section.UpdateScripts[0].Provider, Is.EqualTo("svc"));
            Assert.That(section.UpdateScripts[0].Location, Is.EqualTo("asm1.xml"));
            Assert.That(section.UpdateScripts[1].Provider, Is.EqualTo("svc"));
            Assert.That(section.UpdateScripts[1].Location, Is.EqualTo("asm2.xml"));
        }
    }
}
