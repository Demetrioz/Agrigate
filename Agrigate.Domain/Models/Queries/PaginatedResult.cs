using System.Collections.Specialized;
using System.Text.Json.Serialization;
using System.Web;

namespace Agrigate.Domain.Models.Queries;

/// <summary>
/// A paginated result returned from a query
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginatedResult<T> where T : class, new()
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int ItemsPerPage { get; set; }
    public int TotalItems { get; set; }
    public string? NextPageUrl { get; set; }
    public string PreviousPageUrl { get; set; }
    public List<T> Items { get; set; } = [];
    
    private bool HasNextPage => CurrentPage < TotalPages;
    private bool HasPreviousPage => CurrentPage > 1;

    /// <summary>
    /// Parameterless constructor for serialization. Do not use!
    /// </summary>
    public PaginatedResult()
    {
    }

    public PaginatedResult(
        List<T> items,
        int totalItems,
        int currentPage,
        int itemsPerPage,
        string requestUrl)
    {
        Items = items;
        TotalItems = totalItems;
        CurrentPage = currentPage;
        ItemsPerPage = itemsPerPage;
        TotalPages = (int)Math.Ceiling(TotalItems / (double)itemsPerPage);
        
        if (CurrentPage > 1)
            PreviousPageUrl = GenerateRequestUrl(requestUrl)!;
        
        if (HasNextPage)
            NextPageUrl = GenerateRequestUrl(requestUrl, true);
    }

    private string? GenerateRequestUrl(string currentUrl, bool next = false)
    {
        var urlParts = currentUrl.Split('?');
        var url = urlParts[0];
        var queryString = urlParts.Length > 1 ? urlParts[1] : string.Empty;
        
        var queryParams = HttpUtility.ParseQueryString(queryString);
        if (queryParams.Get("page") == null)
            queryParams.Add("page", CurrentPage.ToString());

        switch (next)
        {
            case false when HasPreviousPage:
            {
                _ = int.TryParse(queryParams["page"], out var currentPage);
                --currentPage;
            
                if (currentPage > TotalPages)
                    currentPage = TotalPages;
            
                queryParams["page"] = currentPage.ToString();
                return BuildQueryString(url, queryParams);
            }
            
            case true when HasNextPage:
            {
                _ = int.TryParse(queryParams["page"], out var currentPage);
                ++currentPage;

                if (currentPage <= 1)
                    currentPage = CurrentPage + 1;
            
                queryParams["page"] = currentPage.ToString();
                return BuildQueryString(url, queryParams);
            }
            
            default:
                return null;
        }
    }
    
    private static string BuildQueryString(string url, NameValueCollection parameters)
    {
        var uriBuilder = new UriBuilder(url);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        foreach (string key in parameters)
            query[key] = parameters[key];
        
        uriBuilder.Query = query.ToString();
        return uriBuilder.ToString();
    }
}