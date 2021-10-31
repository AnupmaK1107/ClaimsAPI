//using ClaimsApplication.Models;
//using Microsoft.Azure.Cosmos;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ClaimsApplication.Repositories
//{   
//    public class ClaimsRepository : IClaimsRepository
//    {
//        private Container _container;

//        public ClaimsRepository(CosmosClient cosmosDbClient,
//        string databaseName,
//        string containerName)
//        {
//            _container = cosmosDbClient.GetContainer(databaseName, containerName);
//        }

        
//        public IEnumerable<Claims> GetClaimsList()
//        {
//            var claimsList = new List<Claims>();
//            //claimLists.Add(new Claims
//            //{
//            //    Id = 1,
//            //    Year = 2014,
//            //    DamageCost = 7.9M,
//            //    Name = "test",
//            //    Type = "test"
//            //});
//            //claimLists.Add(new Claims
//            //{
//            //    Id = 2,
//            //    Year = 2015,
//            //    DamageCost = 9M,
//            //    Name = "test1",
//            //    Type = "test1"
//            //});
//            //return claimLists;
//            var query = _container.GetItemQueryIterator<Claims>(new QueryDefinition(""));
//            while (query.HasMoreResults)
//            {
//                var response = query.ReadNextAsync().GetAwaiter().GetResult();
//                claimsList.AddRange(response.ToList());
//            }
//        }

//        public int AddClaim(Claims claimToAdd)
//        {
//            //List<Claims> listOfClaims = GetClaimsList().ToList();
//            //listOfClaims.Add(claimToAdd);
//            //return claimToAdd.Id;
//            Random random = new Random();
//            claimToAdd.Id = random.Next();
//            _container.CreateItemAsync(claimToAdd, new PartitionKey(claimToAdd.Id));
//            return claimToAdd.Id;
//        }

//        public int DeleteClaim(int claimId)
//        {
//            List<Claims> listOfClaims = GetClaimsList().ToList();
//            var itemToRemove = listOfClaims.Single(r => r.Id == claimId);
//            listOfClaims.Remove(itemToRemove);
//            return claimId;
//        }
//    }
//}
