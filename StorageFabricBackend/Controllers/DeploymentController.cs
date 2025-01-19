using Microsoft.AspNetCore.Mvc;
using StorageFabricBackend.Models;
using StorageFabricBackend.Services;

[ApiController]
[Route("api/[controller]")]
public class DeploymentController : ControllerBase {
    private readonly TerraformService _terraformService;

    public DeploymentController(TerraformService terraformService) {
        _terraformService = terraformService;
    }

    [HttpPost("deploy")]
    public IActionResult Deploy([FromBody] DeploymentRequest request) {
        if (string.IsNullOrEmpty(request.Region) || request.NodeCount <= 0) {
            return BadRequest("Invalid input parameters.");
        }

        string result;

        try {
            result = _terraformService.RunTerraformScript("StorageFabricResourceGroup", request.Region, request.NodeCount);
        } catch (Exception ex) {
            return StatusCode(500, new { message = "Deployment failed", error = ex.Message });
        }

        return Ok(new { message = "Deployment initiated", terraformOutput = result });
    }

    [HttpDelete("destroy")]
    public IActionResult Destroy() {
        try {
            var result = _terraformService.DestroyTerraformResources();
            return Ok(new { message = "Resources destroyed", terraformOutput = result });
        } catch (Exception ex) {
            return StatusCode(500, new { message = "Destroy failed", error = ex.Message });
        }
    }
}