using Azure.Messaging.ServiceBus;
using ClaimsApplication.Models;
using ClaimsApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClaimsApplication.Controllers
{
    [Route("api/claims")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private IClaimsService _claimsService;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _clientSender;
        private const string QUEUE_NAME = "getatestqueue";

        public ClaimsController(IClaimsService claimsService)
        {
            _claimsService = claimsService;
            var connectionString = "Endpoint=sb://getatest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=54sVS1+162kQF5rkJnO2ofdHPui334eLdFN10MHL988=";
            _client = new ServiceBusClient(connectionString);
            _clientSender = _client.CreateSender(QUEUE_NAME);
        }

        [HttpGet("getClaimTypes")]
        public ActionResult GetClaimTypes()
        {
            List<ClaimTypes> claimTypesList = new List<ClaimTypes>();
            claimTypesList.Add(new ClaimTypes { Id = 1, Name = "a" });
            claimTypesList.Add(new ClaimTypes { Id = 2, Name = "b" });
            claimTypesList.Add(new ClaimTypes { Id = 3, Name = "c" });
            return Ok(claimTypesList);
        }

        [HttpGet("getClaimsList")]
        public ActionResult GetClaims()
        {
            var claimsList = _claimsService.GetAllClaims();
            if (claimsList.ToList().Count() == 0)
            {
                return Ok("No Claim Found");
            }
            return Ok(claimsList);
        }

        [HttpPost("addClaim")]
        public ActionResult AddClaim([FromBody] Claims claimToBeAdded)
        {
            if (claimToBeAdded.DamageCost > 100.00M)
            {
                return Ok("Cannot add claim with damage cost more than 100.000");
            }
            else if (claimToBeAdded.Year > DateTime.Now.Year)
            {
                return Ok("Cannot add claim with Year in Future");
            }
            else if (claimToBeAdded.Year < DateTime.Now.AddYears(-10).Year)
            {
                return Ok("Cannot add claim having year value more than 10 years back");
            }
            else
            {
                var ClaimAddedResponse = _claimsService.AddClaim(claimToBeAdded);
                var messageToSend = new Audit()
                {
                    ClaimId = ClaimAddedResponse,
                    Operation = "New Claim Added",
                    Timestamp = DateTime.Now
                };
                SendMessage(messageToSend);
                return Ok(ClaimAddedResponse);
            }            
        }

        [HttpDelete("deleteClaim/{claimId}")]
        public ActionResult DeleteClaim(string claimId)
        {
            var ClaimDeletedResponse = "";
            var ClaimsList = _claimsService.GetAllClaims();
            var IsClaimPresent = ClaimsList.FirstOrDefault(x => x.Id == claimId);
            if (IsClaimPresent != null)
            {
                ClaimDeletedResponse = _claimsService.DeleteClaim(claimId);
                var messageToSend = new Audit()
                {
                    ClaimId = claimId,
                    Operation = "Claim Deleted",
                    Timestamp = DateTime.Now
                };
                SendMessage(messageToSend);
            }
            else
            {
                ClaimDeletedResponse = "Claim Id not found";
            }
            return Ok(ClaimDeletedResponse);
        }

        private async void SendMessage(Audit messageToSend)
        {
            ServiceBusMessage message = new ServiceBusMessage(JsonConvert.SerializeObject(messageToSend));
            await _clientSender.SendMessageAsync(message).ConfigureAwait(false);
        }
    }
}
