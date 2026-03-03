using Microsoft.AspNetCore.Mvc;
using Unite.Genome.Liftover.Web.Models;
using Unite.Genome.Liftover.Web.Services;

namespace Unite.Genome.Liftover.Web.Controllers;

[Route("/api/liftover")]
public class LiftoverController : Controller
{
    private readonly LiftoverService _liftoverService;
    private readonly ILogger<LiftoverController> _logger;


    public LiftoverController(LiftoverService liftoverService, ILogger<LiftoverController> logger)
    {
        _liftoverService = liftoverService;
        _logger = logger;
    }


    [HttpPost()]
    [Consumes("application/json")]
    public async Task<IActionResult> Post([FromQuery]string from, [FromQuery]string to, [FromBody]PositionModel[] positions)
    {
        if (positions == null || positions.Length == 0)
            return BadRequest("No positions provided");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            using var directory = new TempDirectory();
            var inputFilePath = Path.Combine(directory.Path, "input.bed");
            var outputFilePath = Path.Combine(directory.Path, "output.bed");
            var unmappedFilePath = Path.Combine(directory.Path, "unmapped.bed");

            using var inputFileStream = new FileStream(Path.Combine(directory.Path, "input.bed"), FileMode.Create, FileAccess.Write);
            using var outputFileStream = new FileStream(Path.Combine(directory.Path, "output.bed"), FileMode.Open, FileAccess.Read);

            await Request.Body.CopyToAsync(inputFileStream);
            await _liftoverService.Liftover(from, to, inputFilePath, outputFilePath, unmappedFilePath);

            var output = await System.IO.File.ReadAllTextAsync(outputFilePath);
            var unmapped = await System.IO.File.ReadAllTextAsync(unmappedFilePath);

            return Ok(output);
        }
        catch
        {
            return BadRequest("Could not read request body");
        }
    }
}

public class TempDirectory : IDisposable
{
    public string Path { get; }

    public TempDirectory()
    {
        Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());

        Directory.CreateDirectory(Path);
    }

    public void Dispose()
    {
        try
        {
            Directory.Delete(Path, true);
        }
        catch
        {
            
        }
    }
}
