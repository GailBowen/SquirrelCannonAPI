I created a Sql Server docker

Powershell as administrator
	
& 'C:\Program Files\Docker\Docker\DockerCli.exe' -SwitchDaemon
	
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=***" -p 1433:1433 --name sql1 -h sql1 -d mcr.microsoft.com/mssql/server:2019-latest

appsettings.json:
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "ConnectionStrings": {
        "ConnectionString": "Data Source=localhost;Initial Catalog=SquirrelCannonDb;User=sa;Password=***;TrustServerCertificate=True"
    },
    "AllowedHosts": "*"
}


appsettings.Development.json:
{
  "DetailedErrors": true,
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}


https://localhost:44363/flashcard
