set nuget=..\.nuget\nuget.exe

rmdir /S/Q tmp\nuget

mkdir tmp\nuget\DbKeeperNet\lib\net20\cs-CZ
xcopy /y DbKeeperNet.Engine\bin\release\DbKeeperNet.Engine.dll tmp\nuget\DbKeeperNet\lib\net20\
xcopy /y DbKeeperNet.Engine\bin\release\cs-CZ\*.* tmp\nuget\DbKeeperNet\lib\net20\cs-CZ
xcopy /y DbKeeperNet.nuspec tmp\nuget\DbKeeperNet

mkdir tmp\nuget\DbKeeperNet.Extensions.Log4Net\lib\net20
xcopy /y DbKeeperNet.Extensions.Log4Net\bin\release\DbKeeperNet.Extensions.Log4Net.dll tmp\nuget\DbKeeperNet.Extensions.Log4Net\lib\net20
xcopy /y DbKeeperNet.Extensions.Log4Net.nuspec tmp\nuget\DbKeeperNet.Extensions.Log4Net

%nuget% pack tmp\nuget\DbKeeperNet\DbKeeperNet.nuspec
%nuget% pack tmp\nuget\DbKeeperNet.Extensions.Log4Net\DbKeeperNet.Extensions.Log4Net.nuspec

