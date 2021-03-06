using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace MyTinyUrl.Service.Common
{
    public class TinyUrlMapEntity : TableEntity
    {
        public const string DEFAULT_ROWKEY = "DEFAULT_ROWKEY";

        public TinyUrlMapEntity() {}

        public TinyUrlMapEntity(string tinyUrlKey, string originalUrl, string rowKey = DEFAULT_ROWKEY)
        {
            this.PartitionKey = tinyUrlKey;
            this.RowKey = rowKey;
            this.OriginalUrl = originalUrl;
        }

        public string OriginalUrl { get; set; }
    }
}