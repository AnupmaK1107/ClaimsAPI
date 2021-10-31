using ClaimsApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsApplication.Services
{
    public interface IClaimsService
    {
        IEnumerable<Claims> GetAllClaims();
        string AddClaim(Claims claimToAdd);
        string DeleteClaim(string claimId);
    }
}
