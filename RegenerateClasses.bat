rem this batch will regenerate classes defined by XSD schemas
set XSD_PATH="C:\Program Files (x86)\Microsoft Visual Studio 8\SDK\v2.0\Bin\xsd.exe"

cd DbKeeperNet.Engine\Resources
%XSD_PATH% Updates-1.0.xsd /n:DbKeeperNet.Engine /c /l:cs /o:.

move Updates-1_0.cs ..\XmlClasses.cs
