CREATE PROCEDURE [dbo].[sp_SearchSimilarExceptions]
    @Module NVARCHAR(50),
    @Keywords NVARCHAR(200),
    @TopResults INT = 5
AS
BEGIN
    SET NOCOUNT ON;

    -- Clean inputs
    SET @Keywords = TRIM(@Keywords);
    
    -- Execute search query
    SELECT TOP (@TopResults)
        e.Id AS ExceptionId,
        e.Module,
        e.Title,
        e.Description,
        e.ResolutionNotes, -- Also useful context if available
        r.RecommendationText,
        r.ConfidenceScore,
        CAST(CASE WHEN r.WasHelpful = 1 THEN 1 ELSE 0 END AS BIT) AS WasHelpful
    FROM Exceptions e
    INNER JOIN AIRecommendations r ON e.Id = r.ExceptionId
    WHERE 
        -- Filter by Module if provided and not empty
        (@Module IS NULL OR @Module = '' OR e.Module = @Module)
        AND 
        -- Search keywords in Title or Description
        (
            @Keywords IS NULL OR @Keywords = '' 
            OR e.Description LIKE '%' + @Keywords + '%' 
            OR e.Title LIKE '%' + @Keywords + '%'
        )
    ORDER BY 
        -- Prioritize helpful recommendations and high confidence
        CASE WHEN r.WasHelpful = 1 THEN 1 ELSE 0 END DESC,
        r.ConfidenceScore DESC,
        e.ReportedAt DESC;
END
GO
