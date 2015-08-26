@echo off
rem This batch will prepare ZIP packages with sources and binaries
rem Verify properties in Properties.xml and override them in UserProperties.xml
rem if you need
msbuild.exe DbKeeperNet.msbuild /Target:Packages,NuGetPackages /l:FileLogger,Microsoft.Build.Engine;logfile=Packages.log