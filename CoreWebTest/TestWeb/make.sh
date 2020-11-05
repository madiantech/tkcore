cd ..
cd ..
git pull
cd User/YJC.Toolkit.SimpleUser.Data.Core
dotnet build YJC.Toolkit.SimpleUser.Data.Core.31.csproj
cd ..
cd ..
cd CoreWebTest/TestWeb
dotnet publish -r linux-x64 -f netcoreapp3.1 -c Debug ToolkitWeb.31.dll.csproj 
mkdir bin/Debug/linux-x64/publish/Modules
cp -rf bin/Debug/Modules/*.dll bin/Debug/linux-x64/publish/Modules
cp -rf Xml/ bin/Debug/linux-x64/publish/
rm -rf bin/Debug/linux-x64/publish/Xml/razor/temp
rm -f bin/Debug/linux-x64/publish/Xml/*.xml
cp -f docker/xml/*.xml bin/Debug/linux-x64/publish/Xml
docker build -t tk5.5-demo:2.5 .
docker tag tk5.5-demo:2.5 cjiangyong/tk5.5-demo:2.5
docker rmi -f $(docker images --filter dangling=true -q)
