CREATE PROCEDURE [dbo].[spItems_List]
AS
begin
    set nocount on;
    SELECT [Id], [Name], [Code], [Brand], [UnitPrice], [Platform], [Condition], [Quantity], [ReorderLevel]
    FROM dbo.Items;
end