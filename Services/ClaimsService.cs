using ClaimsApplication.Models;
using ClaimsApplication.Repositories;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClaimsApplication.Services
{
    public class ClaimsService : IClaimsService
    {
        //private IClaimsRepository _claimsrepository;
        private Container _container;

        public ClaimsService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        //public ClaimsService(IClaimsRepository claimsrepository)
        //{
        //    _claimsrepository = claimsrepository;
        //}

        public IEnumerable<Claims> GetAllClaims()
        {
            //var claimsList = _claimsrepository.GetClaimsList();
            //return claimsList;
            var claimsList = new List<Claims>();
            var query = _container.GetItemQueryIterator<Claims>(new QueryDefinition("SELECT * FROM c"));
            while (query.HasMoreResults)
            {
                var response = query.ReadNextAsync().GetAwaiter().GetResult();
                claimsList.AddRange(response.ToList());
            }
            return claimsList;
        }

        public string AddClaim(Claims claimToAdd)
        {
            //int ClaimId = _claimsrepository.AddClaim(claimToAdd);
            //if (ClaimId != 0)
            //{
            //    return ClaimId;
            //}
            //return 0;
            Guid randomVal = new Guid();
            claimToAdd.Id = randomVal.ToString();
            _container.CreateItemAsync(claimToAdd, new PartitionKey(claimToAdd.Id));
            return claimToAdd.Id;
        }

        public string DeleteClaim(string claimId)
        {
            //_claimsrepository.DeleteClaim(claimId);
            //return "Claim Deleted Successfully";
            _container.DeleteItemAsync<Claims>(claimId, new PartitionKey(claimId));
            return claimId;
        }
    }
}
