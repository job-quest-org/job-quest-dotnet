using Microsoft.AspNetCore.Mvc;
using JQ.BusinessLayer;
using job_quest_dotnet.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace JQ.Controllers
{
    [ApiController]
    [Route("api/Candidate")]
    public class CandidateController : ControllerBase
    {
        private readonly ILogger<CandidateController> _logger;
        private readonly CandidateBL _candidateBL;

        public CandidateController(ILogger<CandidateController> logger, CandidateBL candidateBL)
        {
            _logger = logger;
            _candidateBL = candidateBL;
        }
        [Authorize]
        [HttpGet("GetCandidateCount")]
        public async Task<int> CandidateCount()
        {
            var principal = HttpContext.User.Identity as ClaimsIdentity;

            int res = await _candidateBL.GetCandidateCount();
            return res;
        }

        [HttpGet("GetCandidateList")]
        public async Task<List<string>> CandidateList()
        {
            return await _candidateBL.GetCandidateList();
        }
    }
}
