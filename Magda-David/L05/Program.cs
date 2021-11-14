using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models;

namespace L05
{
    class Program
    {
        private static CloudTableClient tableClient;
        private static CloudTable studentsTable, metricsTable;
        
        static void Main(string[] args)
        {
            Task.Run(async () => { await Initialize(); })
                .GetAwaiter()
                .GetResult();
            
        }

        static async Task Initialize()
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=datcmagdadavid;AccountKey=o8QTWbPyJ7DgSv+YwtUEvCYBwPo2LOcxD7xQqtTw5xGdZ/F0gNapXluVHs2I2arDLMSPl1t96l6D8YZKM3idsQ==;EndpointSuffix=core.windows.net";
            var account = CloudStorageAccount.Parse(storageConnectionString);
            tableClient = account.CreateCloudTableClient();

            studentsTable = tableClient.GetTableReference("studenti");
            metricsTable = tableClient.GetTableReference("metrics");

            await studentsTable.CreateIfNotExistsAsync();
            await metricsTable.CreateIfNotExistsAsync();

            //await AddNewStudent();
            // await EditStudent();

            await AddNewMetrics();
    }

    private static async Task AddNewStudent()
    {
        var student = new StudentEntity("UVT", "1955578555556");
        student.FirstName = "Adrian";
        student.LastName = "Iliescu";
        student.Year = 2;
        student.Faculty = "INFO";

        var insertOperation = TableOperation.Insert(student);

        await studentsTable.ExecuteAsync(insertOperation);
    }

    private static async Task AddNewMetrics()
    {
        
        TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();

        TableContinuationToken token = null;

        List<string> facultyList = new List<string>();
        
        int countStudents;

        MetricEntity metric;

        do
        {
            TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query, token);
            token = resultSegment.ContinuationToken;
           
            // Make a list of string with all faculties
            foreach (StudentEntity entity in resultSegment.Results)
            {
                if(!facultyList.Contains(entity.PartitionKey))
                   facultyList.Add(entity.PartitionKey);
            }

            // For each faculty count the number of students and insert metrics in metric table

            foreach (var faculty in facultyList)
            {
                 countStudents = 0;
                 foreach (StudentEntity entity in resultSegment.Results)
                 {
                     if (entity.PartitionKey == faculty)
                          countStudents++;
                 }

                 metric = new MetricEntity(faculty, DateTime.Now.ToLongDateString()+" "+DateTime.Now.ToLongTimeString());
                 metric.Count = countStudents;

                var insertOperation = TableOperation.Insert(metric);
                 await metricsTable.ExecuteAsync(insertOperation);

            }
            
            // Add metric for all students

            countStudents = 0;
            foreach (StudentEntity entity in resultSegment.Results)
            {
                countStudents++;
            }
            metric = new MetricEntity("General", DateTime.Now.ToLongDateString()+" "+DateTime.Now.ToLongTimeString());
            metric.Count = countStudents;

            var insertOperation_ = TableOperation.Insert(metric);
            await metricsTable.ExecuteAsync(insertOperation_);

        }while (token != null);

    }


}

    
}
