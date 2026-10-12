CREATE PROCEDURE [dbo].[spItems_Insert]
    @name nvarchar(100), @code nvarchar(50), @brand nvarchar(50), @unitPrice decimal(10,2),
    @platform nvarchar(50), @condition nvarchar(20), @quantity int, @reorderLevel int
AS
begin
    INSERT INTO dbo.Items (Name, Code, Brand, UnitPrice, Platform, [Condition], Quantity, ReorderLevel)
    VALUES (@name, @code, @brand, @unitPrice, @platform, @condition, @quantity, @reorderLevel)
end