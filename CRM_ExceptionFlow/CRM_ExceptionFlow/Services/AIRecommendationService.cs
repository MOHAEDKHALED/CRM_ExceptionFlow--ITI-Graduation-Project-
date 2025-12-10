using CRM_ExceptionFlow.Data;
using CRM_ExceptionFlow.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace CRM_ExceptionFlow.Services
{
    public class AIRecommendationService : IAIRecommendationService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AIRecommendationService(
            HttpClient httpClient,
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _context = context;
            _configuration = configuration;
        }

        public async Task<AIRecommendationResponse> GetRecommendationAsync(int exceptionId)
        {
            var exception = await _context.Exceptions
                .Include(e => e.ReportedByUser)
                .FirstOrDefaultAsync(e => e.Id == exceptionId);

            if (exception == null)
                throw new ArgumentException("Exception not found");

            var n8nWebhookUrl = _configuration["N8N:WebhookUrl"];

            if (string.IsNullOrEmpty(n8nWebhookUrl))
            {
                throw new InvalidOperationException("N8N Webhook URL is not configured");
            }

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
                var response = await _httpClient.PostAsync(n8nWebhookUrl, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new System.Exception($"N8N returned error: {response.StatusCode} - {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                // Log the response
                Console.WriteLine($"N8N Raw Response: {responseContent}");

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

                // Clean text before saving
                result.Recommendation = CleanText(result.Recommendation);
                result.Reasoning = CleanText(result.Reasoning ?? "");

                //// Save to database
                //var aiRecommendation = new AIRecommendation
                //{
                //    ExceptionId = exceptionId,
                //    RecommendationText = result.Recommendation,
                //    ConfidenceScore = result.Confidence,
                //    Model = result.Model ?? "N8N",
                //    Source = result.Source ?? "N8N",
                //    IsFromDatabase = result.IsFromDatabase,
                //    GeneratedAt = DateTime.UtcNow
                //};

                //_context.AIRecommendations.Add(aiRecommendation);
                //await _context.SaveChangesAsync();

                return result;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
                throw new System.Exception($"Failed to connect to N8N: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Error: {ex.Message}");
                throw new System.Exception($"Failed to parse N8N response: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                throw new System.Exception($"Failed to get AI recommendation: {ex.Message}");
            }
        }

        // method to clean text
        private string CleanText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            // Remove extra whitespace and newlines
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ");

            // Escape single quotes for SQL
            text = text.Replace("'", "''");

            // Trim
            text = text.Trim();

            // Limit to 3000 characters
            if (text.Length > 3000)
            {
                text = text.Substring(0, 3000);
            }

            return text;
        }
    }
}