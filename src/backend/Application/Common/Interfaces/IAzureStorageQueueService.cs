﻿namespace Application.Common.Interfaces
{
    public interface IAzureStorageQueueService
    {
        void InsertMessage(string queueName, string message);
    }
}