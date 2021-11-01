using ClaimsApplication.Models;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClaimsApplication.Services
{
    public class ClaimsService : IClaimsService
    {
        private Container _container;

        public ClaimsService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }


        public IEnumerable<Claims> GetAllClaims()
        {
            var claimsList = new List<Claims>();
            var query = _container.GetItemQueryIterator<Claims>(new QueryDefinition("SELECT * FROM c"));
            while (query.HasMoreResults)
            {
                var response = query.ReadNextAsync().GetAwaiter().GetResult();
                claimsList.AddRange(response.ToList());
            }
            return claimsList;
        }

        public int AddClaim(Claims claimToAdd)
        {
            var claimIdList = GetAllClaims().Select(x => x.Id).ToList();
            var finalClaimIdList = claimIdList.Select(int.Parse).ToList();
            int claimId = (claimIdList.Count != 0) ? (finalClaimIdList.Max()+1) : 1;
            claimToAdd.Id = claimId.ToString();
            _container.CreateItemAsync(claimToAdd, new PartitionKey(claimToAdd.Id));
            return (int.Parse(claimToAdd.Id));
        }

        public int DeleteClaim(string claimId)
        {
            _container.DeleteItemAsync<Claims>(claimId, new PartitionKey(claimId));
            return int.Parse(claimId);
        }
    }
}
