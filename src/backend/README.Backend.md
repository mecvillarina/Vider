# API/Jobs App Guide

## Prerequisites
- Read first the infrastructure setup posted on the main [README.md](../../README.md)
- C#/.NET experience at a beginner level
- Knowledge on Azure Function Apps
- Knowledge on Provisioning Azure Resources 
- Local installations of the .NET SDK (6.0) and Visual Studio 2022
- Command-line interface (CLI) tools for Entity Framework Core using this [Guide](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

## Azure Cloud Prerequisites
- Create/Provision Azure Function App
- Create/Provision Azure Storage Account
- Create/Provision Azure SQL Database

## Instructions
- After completing the Azure Cloud Prerequisites
- Open Vider.Backend.sln on Visual Studio
- On **WebApi/WebApi** Project, create a file and named it **localsettings.json** and copy the following:
	- ```json
		{
		  "IsEncrypted": false,
		  "ConnectionStrings": {
			"DataDbContext": "[MSSQLDatabaseConnectionString]"
		  },
		  "Values": {
			"FUNCTIONS_WORKER_RUNTIME": "dotnet",
			"JwtAudience": "[JWTAudience]",
			"JwtIssuer": "[JWTIssuer]",
			"JwtSecret": "[JWTSecret]",
			"AzureStorageAccount": "[AzureStorageAccountAccessKey]",
			"AzureWebJobsStorage": "[AzureStorageAccountAccessKey]",
			"FaucetEndpoint": "https://faucet-nft.ripple.com",
			"XPRLEndpoint": "[XRPLNFTDEVServerEndpoint]",
			"AzureStorageCdn": "[AzureStorageBaseUrl]"
		  },
		  "Host": {
			"LocalHttpPort": 7071,
			"CORS": "*"
		  }
		}
      ```
	- Update the following properties (required):
		- **[MSSQLDatabaseConnectionString]** - Azure SQL Database/MS SQL Database Connection String
		- **[JWTAudience]** - any values you like, example "AppAudience" 
		- **[JwtIssuer]** - any values you like, example "AppIssuer"
		- **[JWTSecret]** - random alphanumeric string - length must be exactly 64 lower case characters
		- **[AzureStorageAccountAccessKey]** - Your Azure Account Storage Access Key. Example(DefaultEndpointsProtocol=https;AccountName=;AccountKey=;EndpointSuffix=core.windows.net)
		- **[XRPLNFTDEVServerEndpoint]** - Your Ripple NFT DEV Server, see [XRPL.org](https://xrpl.org/parallel-networks.html)
		- **[AzureStorageBaseUrl]** - Your Azure Storage Account Base Url or CDN Base URL that is connected to your Azure Storage Account
	
	- When you're finish with **localsettings.json**, make a copy of it and paste it on **WebApi/WebJob** Project on Visual Studio

- To create or update the tables of the database, go to ***src/backend*** directory, open a terminal from there and execute the following command:
	- **dotnet ef database update -p .\Infrastructure\Infrastructure.csproj -s .\WebApi\WebApi.csproj**
		
- Set **WebApi/WebApi** Project as Startup Project
- Click Run or Press F5.
- If you wish to publish the API to Azure Function App, please see [Guide](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-your-first-function-visual-studio#publish-the-project-to-azure)