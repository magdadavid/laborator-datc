using Microsoft.WindowsAzure.Storage.Table;

namespace Models
{
    public class MetricEntity : TableEntity
    {
        public MetricEntity(string university, string date)
        {
            this.PartitionKey = university;
            this.RowKey = date;
        }

        public MetricEntity() {}

        public int Count { get; set; }

    }
}