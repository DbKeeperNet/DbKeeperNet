rem if "%1"  "" goto startBuild
echo Enter a target from build script (SrcZip,Build,BinZip)

:startBuild
msbuild.exe DbKeeperNet.msbuild /Target:%1 /l:FileLogger,Microsoft.Build.Engine;logfile=DbKeeperNetBuild.log /p:Configuration=Debug

:end
