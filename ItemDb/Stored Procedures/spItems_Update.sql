CREATE PROCEDURE [dbo].[spItems_Update]
    @id int, @name nvarchar(100), @code nvarchar(50), @brand nvarchar(50), @unitPrice decimal(10,2),
    @platform nvarchar(50), @condition nvarchar(20), @quantity int, @reorderLevel int
AS
begin
    UPDATE dbo.Items
    SET Name=@name, Code=@code, Brand=@brand, UnitPrice=@unitPrice,
        Platform=@platform, [Condition]=@condition, Quantity=@quantity, ReorderLevel=@reorderLevel
    WHERE Id=@id
end