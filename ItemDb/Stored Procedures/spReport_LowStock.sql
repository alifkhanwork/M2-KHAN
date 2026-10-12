
CREATE PROCEDURE [dbo].[spReport_LowStock] 
AS 
    SELECT * FROM Items WHERE Quantity <= ReorderLevel ORDER BY Quantity; 