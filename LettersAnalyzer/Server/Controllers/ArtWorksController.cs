using LettersAnalyzer.Server.Interfaces;
using LettersAnalyzer.Server.Workers;
using LettersAnalyzer.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace LettersAnalyzer.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtWorksController : ControllerBase
    {
        private readonly IArtWorkService _artWorkService;
        private readonly IFrequencyCounter _frequencyCounter;
        private readonly SeedDataHelper _seedDataHelper;
        public ArtWorksController(IFrequencyCounter frequencyCounter, IArtWorkService artWorkService, SeedDataHelper seedDataHelper)
        {
            _artWorkService = artWorkService;
            _frequencyCounter = frequencyCounter;
            _seedDataHelper = seedDataHelper;
        }

        [HttpGet("/api/start")]
        public async Task<IActionResult> InitData()
        {
            await _seedDataHelper.LoadInitialData();
            return Ok("peremiga");
        }

        [HttpGet]
        public async Task<List<ArtWork>> Get(CancellationToken cancellationToken)
        {
            return await _artWorkService.GetAllArtWorksAsync(cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult> AddArtWork(
            [FromBody] ArtWork artWork,
            CancellationToken cancellationToken)
        {
            await _artWorkService.AddArtWorkAsync(artWork, cancellationToken);
            var artWorkBody = new ArtWorkBody() 
            { 
                ArtWorkId = artWork.Id,
                Body = artWork.Body,
            };
            await _artWorkService.AddArtWorkBodyAsync(artWorkBody, cancellationToken);
            artWork.ArtWorkBodyId = artWorkBody.Id;
            await _artWorkService.UpdateArtWorkAsync(artWork, cancellationToken);
            await _frequencyCounter.ProcessBodyAsync(artWorkBody, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteArtWork(
            string id,
            CancellationToken cancellationToken)
        {
            await _artWorkService.DeleteArtWorkAsync(id, cancellationToken);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateArtWork(
            string id,
            [FromBody] ArtWork artWork,
            CancellationToken cancellationToken)
        {
            artWork.Id = id;
            await _artWorkService.UpdateArtWorkAsync(artWork, cancellationToken);
            return Ok();
        }

        [HttpPut("bodies/{id}")]
        public async Task<ActionResult> UpdateArtWorkBody(
            string id,
            [FromBody] ArtWorkBody artWorkBody,
            CancellationToken cancellationToken)
        {
            artWorkBody.Id = id;
            await _artWorkService.UpdateArtWorkBodyAsync(artWorkBody, cancellationToken);
            await _frequencyCounter.ProcessBodyAsync(artWorkBody, cancellationToken);
            return Ok();
        }

        [HttpGet("/api/report")]
        public async Task<FrequencyReport> GetReport( 
            CancellationToken cancellationToken,
            [FromQuery] string groupBy = "century")
        {
            groupBy = groupBy.ToLower();
            return groupBy switch
            {
                "author" => await _artWorkService.GetFrequencyReportAsync(
                    it => it.Author ?? "", groupBy, cancellationToken),
                
                "country" => await _artWorkService.GetFrequencyReportAsync(
                    it => it.Country ?? "", groupBy, cancellationToken),
                
                _ => await _artWorkService.GetFrequencyReportAsync(
                    it => it.Century.ToString(), groupBy, cancellationToken),
            };
        }
    }
}