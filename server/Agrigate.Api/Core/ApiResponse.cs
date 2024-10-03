using Microsoft.AspNetCore.Mvc;

namespace Agrigate.Api.Core;

/// <summary>
/// A helper to return various types of responses from
/// Farmstop Online
/// </summary>
public static class ApiResponse 
{
    public static IActionResult Success(object data) =>
        new OkObjectResult(new AgrigateResponse 
        {
            Status = ResponseStatus.Success,
            Data = data,
            Error = null
        });

    /// <summary>
    /// Returns a failure response
    /// </summary>
    /// <param name="error">The error returned from the API</param>
    /// <returns></returns>
    public static IActionResult Failure(string error) =>
        new OkObjectResult(new AgrigateResponse
        {
            Status = ResponseStatus.Failure,
            Data = null,
            Error = error
        });

    /// <summary>
    /// Returns a created response object
    /// </summary>
    /// <param name="data">The data returned from the API</param>
    /// <returns></returns>
    public static IActionResult Created(string location, object data) =>
        new CreatedResult(location, new AgrigateResponse
        {
            Status = ResponseStatus.Success,
            Data = data,
            Error = null
        });

    /// <summary>
    /// Returns a conflict response object
    /// </summary>
    /// <param name="error">The error returned from the API</param>
    /// <returns></returns>
    public static IActionResult Conflict(string error) =>
        new ConflictObjectResult(new AgrigateResponse
        {
            Status = ResponseStatus.Failure,
            Data = null,
            Error = error
        });

    /// <summary>
    /// Returns an unauthorized response object
    /// </summary>
    /// <returns></returns>
    public static IActionResult Unauthorized() =>
        new UnauthorizedObjectResult(new AgrigateResponse
        {
            Status = ResponseStatus.Failure,
            Data = null,
            Error = "Unauthorized"
        });

    /// <summary>
    /// Returns a bad request object response
    /// </summary>
    /// <param name="error">The error returned from the API</param>
    /// <returns></returns>
    public static IActionResult BadRequest(string error) =>
        new BadRequestObjectResult(new AgrigateResponse
        {
            Status = ResponseStatus.Failure,
            Data = null,
            Error = error
        });
}