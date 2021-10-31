using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsApplication.Models
{
    public class Audit
    {
        public string ClaimId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Operation { get; set; }
    }
}
