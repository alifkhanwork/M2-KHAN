
CREATE PROCEDURE [dbo].[spTransaction_GetAll] 
    @From DATETIME2 = NULL, @To DATETIME2 = NULL 
AS 
    SELECT t.Id, t.ItemId, i.Name AS ItemName, i.Code AS ItemCode, 
           t.[Type], t.Quantity, t.UnitPrice, t.Notes, t.CreatedBy, t.CreatedAt 
    FROM Transactions t JOIN Items i ON i.Id = t.ItemId 
    WHERE (@From IS NULL OR t.CreatedAt >= @From) 
      AND (@To   IS NULL OR t.CreatedAt <  DATEADD(DAY, 1, @To)) 
    ORDER BY t.CreatedAt DESC; 