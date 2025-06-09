using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ShopifySharp;
using ShopifySharp.Factories;

namespace Agrigate.Api.Testing.Controllers;

[ApiController]
[Tags("Test")]
[Route("Test")]
public class TestController : ControllerBase
{
    private readonly IGraphService _graphService;
    
    public TestController(IGraphServiceFactory factory)
    {
        var shopUrl = "xxx.myshopify.com";
        var token = "xxx";
        _graphService = factory.Create(shopUrl, token);
    }

    [HttpGet]
    public async Task<IActionResult> Test()
    {
        var query = """
        query GetProducts {
          products(first: 10) {
            nodes {
              id
              title
              category {
                id
                name
                fullName
              }
              createdAt
              updatedAt
              description
              onlineStoreUrl
              productType
              status
              totalInventory
              tracksInventory
              variants(first: 10) {
                nodes {
                  barcode
                  createdAt
                  id
                  displayName
                  inventoryQuantity
                  price
                  sku
                  title
                  updatedAt
                }
              }
            }
          }
        }
        """;
        
        var request = new GraphRequest
        {
            Query = query,
        };

        try
        {
            var result = await _graphService.PostAsync<ProductList>(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, statusCode: 500);
        }
    }
}

public class ProductList
{
    [JsonPropertyName("products")]
    public ProductInfo Products { get; set; }
}

public class ProductInfo
{
    [JsonPropertyName("nodes")]
    public List<ProductNode> Nodes { get; set; } 
}

public class ProductNode
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }
    
    [JsonPropertyName("updatedAt")]
    public DateTimeOffset UpdatedAt { get; set; }
    
    [JsonPropertyName("status")]
    public string Status { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
}