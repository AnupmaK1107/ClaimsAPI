using ClaimsApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsApplication.Repositories
{
    public interface IClaimsRepository
    {
        IEnumerable<Claims> GetClaimsList();
        int AddClaim(Claims claimToAdd);
        int DeleteClaim(int claimId);
    }
}
