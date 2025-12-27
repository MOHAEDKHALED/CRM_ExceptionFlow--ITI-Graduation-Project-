using System.Text;
using System.Text.Json;
using CRM.Application.Common.Interfaces;
using CRM.Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace CRM.Infrastructure.Services;

public class AIRecommendationService : IAIRecommendationService
{
    private readonly HttpClient _httpClient;
    private readonly IExceptionRepository _exceptionRepository;
    private readonly IConfiguration _configuration;
    
    public AIRecommendationService(
        HttpClient httpClient,
        IExceptionRepository exceptionRepository,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _exceptionRepository = exceptionRepository;
        _configuration = configuration;
    }
    
    public async Task<AIRecommendationResponse> GetRecommendationAsync(
        int exceptionId, 
        CancellationToken cancellationToken = default)
    {
        var exception = await _exceptionRepository.GetByIdAsync(exceptionId, cancellationToken);
        
        if (exception == null)
            throw new ArgumentException("Exception not found");
        
        var n8nWebhookUrl = _configuration["N8N:WebhookUrl"];
        
        if (string.IsNullOrEmpty(n8nWebhookUrl))
            throw new InvalidOperationException("N8N Webhook URL is not configured");
        
        var requestData = new
        {
            exceptionId = exception.Id,
            projectId = exception.ProjectId,
            module = exception.Module,
            description = exception.Description,
            title = exception.Title
        };
        
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestData),
            Encoding.UTF8,
            "application/json"
        );
        
        try
        {
            var response = await _httpClient.PostAsync(n8nWebhookUrl, jsonContent, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new Exception($"N8N returned error: {response.StatusCode} - {errorContent}");
            }
            
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            
            // Clean response if needed
            var jsonStart = responseContent.IndexOf('{');
            var jsonEnd = responseContent.LastIndexOf('}');
            
            if (jsonStart >= 0 && jsonEnd > jsonStart)
            {
                responseContent = responseContent.Substring(jsonStart, jsonEnd - jsonStart + 1);
            }
            
            var result = JsonSerializer.Deserialize<AIRecommendationResponse>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            
            if (result == null)
                throw new Exception("Failed to deserialize N8N response");
            
            // Clean text
            result.Recommendation = CleanText(result.Recommendation);
            result.Reasoning = CleanText(result.Reasoning ?? "");
            
            return result;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Failed to connect to N8N: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new Exception($"Failed to parse N8N response: {ex.Message}", ex);
        }
    }
    
    private string CleanText(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;
        
        text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ");
        text = text.Replace("'", "''");
        text = text.Trim();
        
        if (text.Length > 3000)
            text = text.Substring(0, 3000);
        
        return text;
    }
}


