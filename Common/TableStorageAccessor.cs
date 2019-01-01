using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Threading.Tasks;

namespace JasonMrX.MyTinyUrl.Common
{
    public class TableStorageAccessor
    {
        private static readonly Lazy<TableStorageAccessor> instance =
            new Lazy<TableStorageAccessor>(() => new TableStorageAccessor());
        public static TableStorageAccessor Instance => instance.Value;

        private CloudTable _myTinyUrlsTable;

        private TableStorageAccessor() 
        {
            var storageKey = Environment.GetEnvironmentVariable("AzureTableStorageKey");
            var storageAccount = new CloudStorageAccount(
                new StorageCredentials("mytinyurlstorage", storageKey), true);
            var tableClient = storageAccount.CreateCloudTableClient();
            _myTinyUrlsTable = tableClient.GetTableReference("MyTinyUrlsTable");
            bool success = _myTinyUrlsTable.CreateIfNotExistsAsync().Result;
        }

        public async Task InsertAsync(string tinyUrlKey, string originalUrl)
        {
            var tinyUrlMapEntity = new TinyUrlMapEntity(tinyUrlKey, originalUrl);
            var insertOperation = TableOperation.Insert(tinyUrlMapEntity);
            await _myTinyUrlsTable.ExecuteAsync(insertOperation);
        }
    }
}