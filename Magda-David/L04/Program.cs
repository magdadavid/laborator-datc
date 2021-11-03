using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models;

namespace L04
{
    class Program
    {
        private static CloudTableClient tableClient;
        private static CloudTable studentsTable;
        
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

            await studentsTable.CreateIfNotExistsAsync();

            //await AddNewStudent();
            // await EditStudent();

            await GetAllStudents();
    }

    private static async Task AddNewStudent()
    {
        var student = new StudentEntity("UPT", "1955555555556");
        student.FirstName = "Ciprian";
        student.LastName = "Popescu";
        student.Year = 3;
        student.Faculty = "INFO";

        var insertOperation = TableOperation.Insert(student);

        await studentsTable.ExecuteAsync(insertOperation);
    }

    private static async Task GetAllStudents()
    {
        Console.WriteLine("Univeristate\tCNP\tNume\tAn\tFacultate");
        TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();

        TableContinuationToken token = null;
        do
        {
            TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query, token);
            token = resultSegment.ContinuationToken;

            foreach (StudentEntity entity in resultSegment.Results)
            {
                Console.WriteLine("{0}\t{1}\t{2} {3}\t{4}\t{5}", entity.PartitionKey, entity.RowKey, entity.FirstName, entity.LastName, entity.Year, entity.Faculty );
            }
        }while (token != null);

    }


    }

    
}
