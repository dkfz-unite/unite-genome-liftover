using Microsoft.AspNetCore.Mvc;

namespace Unite.Genome.Liftover.Web.Controllers;

[Route("/api")]
public class DefaultController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        var date = DateTime.UtcNow;

        return Ok(date);
    }
}
