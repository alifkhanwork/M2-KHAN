
CREATE PROCEDURE [dbo].[spReport_Summary] 
AS 
    SELECT 
      (SELECT COUNT(*) FROM Items)                                        AS TotalItems, 
      (SELECT ISNULL(SUM(Quantity),0) FROM Items)                         AS TotalUnits, 
      (SELECT ISNULL(SUM(Quantity * UnitPrice),0) FROM Items)             AS InventoryValue, 
      (SELECT COUNT(*) FROM Items WHERE Quantity <= ReorderLevel)         AS LowStockCount, 
      (SELECT ISNULL(SUM(Quantity * UnitPrice),0) FROM Transactions 
         WHERE [Type]='SALE' AND CAST(CreatedAt AS DATE) = CAST(SYSDATETIME() AS DATE)) AS 
SalesToday; 