dotnet publish -c Release -v m --self-contained -p:PublishSingleFile=true -r linux-x64
dotnet publish -c Release -v m --self-contained -p:PublishSingleFile=true -r win-x64
dotnet publish -c Release -v m --self-contained -p:PublishSingleFile=true -r win-x86