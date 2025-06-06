using Agrigate.Api.Core.Queries;
using Agrigate.Api.Core.ValueTypes;

namespace Agrigate.Api.Crops.Messages;

/// <summary>
/// Queries to retrieve crop-related data
/// </summary>
public static class CropQueries
{
    /// <summary>
    /// Query to retrieve CropDetail records
    /// </summary>
    /// <param name="Params"></param>
    public sealed record QueryCropDetail(NonEmptyString Uri, QueryParams Params);
}