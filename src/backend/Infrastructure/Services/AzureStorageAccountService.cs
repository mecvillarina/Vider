using Application.Common.Constants;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;

namespace Infrastructure.Services
{
    public class AzureStorageAccountService : IAzureStorageAccountService
    {
        public AzureStorageAccountService(IConfiguration configuration)
        {
            StorageConnectionString = configuration.GetValue<string>(SettingKeys.AzureStorageAccount);
        }

        public string StorageConnectionString { get; private set; }
    }
}
