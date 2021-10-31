using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsApplication.Models
{
    public class Audit
    {
        public int ClaimId { get; set; }
        public string Timestamp { get; set; }
        public string Operation { get; set; }
    }
}
