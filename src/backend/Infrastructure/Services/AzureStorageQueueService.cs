using Application.Common.Interfaces;
using Azure.Storage.Queues;

namespace Infrastructure.Services
{
    public class AzureStorageQueueService : IAzureStorageQueueService
    {
        private readonly IAzureStorageAccountService _azureStorageAccountService;
        public AzureStorageQueueService(IAzureStorageAccountService azureStorageAccountService)
        {
            _azureStorageAccountService = azureStorageAccountService;
        }

        private QueueClient GetQueueClient(string queueName)
        {
            QueueClient queueClient = new QueueClient(_azureStorageAccountService.StorageConnectionString, queueName);
            queueClient.CreateIfNotExists();
            return queueClient;
        }

        public void InsertMessage(string queueName, string message)
        {
            var queueClient = GetQueueClient(queueName);
            var b = System.Text.Encoding.UTF8.GetBytes(message);
            var message64Base = System.Convert.ToBase64String(b);
            queueClient.SendMessage(message64Base);
        }
    }
}
