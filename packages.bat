@echo off
rem This batch will prepare ZIP packages with sources and binaries
rem Verify properties in Properties.xml and override them in UserProperties.xml
rem if you need
C:\WINDOWS\Microsoft.NET\Framework\v3.5\msbuild.exe DbKeeperNet.xml /Target:Packages /l:FileLogger,Microsoft.Build.Engine;logfile=Packages.log