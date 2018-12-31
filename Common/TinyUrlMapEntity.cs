using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace JasonMrX.MyTinyUrl.Common
{
    public class TinyUrlMapEntity : TableEntity
    {
        public TinyUrlMapEntity(string tinyUrlKey, string originalUrl) 
        {
            this.PartitionKey = tinyUrlKey;
            this.RowKey = originalUrl;
        }
    }
}