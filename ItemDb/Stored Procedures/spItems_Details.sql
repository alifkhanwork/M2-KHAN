CREATE PROCEDURE [dbo].[spItems_Details]
    @id int
AS
begin
    set nocount on;
    SELECT [Id], [Name], [Code], [Brand], [UnitPrice], [Platform], [Condition], [Quantity], [ReorderLevel]
    FROM dbo.Items WHERE Id = @id;
end