-- spTransactions_List.sql
CREATE PROCEDURE [dbo].[spTransactions_List]
    @From DATETIME2 = NULL, @To DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT t.Id, t.ItemId, i.Name AS ItemName, i.Code AS ItemCode,
           t.[Type], t.Quantity, t.UnitPrice, t.Notes, t.CreatedBy, t.CreatedAt
    FROM dbo.Transactions t JOIN dbo.Items i ON i.Id = t.ItemId
    WHERE (@From IS NULL OR t.CreatedAt >= @From)
      AND (@To   IS NULL OR t.CreatedAt <  DATEADD(DAY, 1, @To))
    ORDER BY t.CreatedAt DESC;
END