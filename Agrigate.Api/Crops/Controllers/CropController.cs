using System.Net;
using Agrigate.Api.Crops.Actors;
using Agrigate.Api.Crops.Messages;
using Agrigate.Domain.Entities.Crops;
using Agrigate.Domain.Models;
using Akka.Actor;
using Akka.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Agrigate.Api.Crops.Controllers;

/// <summary>
/// Controller for handling Crop-related requests
/// </summary>
[ApiController]
[Tags("Crops")]
[Route("Crops")]
public class CropController : ControllerBase
{
    private readonly IActorRef _cropManager;
    private readonly ILogger<CropController> _logger;
    
    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="registry"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ApplicationException"></exception>
    public CropController(
        ILogger<CropController> logger,
        ActorRegistry registry
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        if (!registry.TryGet<CropManager>(out _cropManager))
            throw new ApplicationException($"{nameof(CropManager)} has not been registered");
    }

    #region Crop Details
    
    /// <summary>
    /// Retrieve Crop Details
    /// </summary>
    /// <param name="id">The ID of a particular CropDetail record</param>
    /// <returns>A CropDetail record</returns>
    /// <response code="200">Request Successful</response>
    [HttpGet("Detail")]
    [ProducesResponseType(typeof(CropDetail), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCropDetail([FromQuery] int? id)
    {
        return Ok(new CropDetail
        {
            Id = 1,
            Crop = "Test",
            Cultivar = "Variant",
            Dtm = 3,
            Created = DateTimeOffset.Now,
            Modified = DateTimeOffset.Now,
            IsDeleted = false
        });
    }

    /// <summary>
    /// Create a new CropDetail record
    /// </summary>
    /// <param name="detail">The details to save</param>
    /// <returns></returns>
    [HttpPost("Detail")]
    [ProducesResponseType(typeof(CropDetailBase), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> CreateCropDetail([FromBody] CropDetailBase detail)
    {
        try
        {
            var command = new CropCommands.CreateCropDetail(detail);
            var result = await _cropManager.Ask(command, TimeSpan.FromSeconds(5)) as CropDetailBase;
            return CreatedAtAction(nameof(GetCropDetail), new { id = 1 }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Unable to create CropDetail: {Error}", ex.Message);
            return Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError); 
        }
    }
    
    #endregion
    
    // #region Crops
    //
    //
    // [HttpPost()]
    // public async Task<IActionResult> CreateCrop()
    // {
    //     return Ok();
    // }
    //
    // #endregion
}