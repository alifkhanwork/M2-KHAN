CREATE PROCEDURE [dbo].[spTransactions_Create] 
    @ItemId INT, @Type NVARCHAR(20), @Quantity INT, 
    @UnitPrice DECIMAL(10,2), @Notes NVARCHAR(200), @CreatedBy NVARCHAR(100) 
AS 
BEGIN 
    SET NOCOUNT ON; 
    BEGIN TRY 
        BEGIN TRANSACTION; 
        DECLARE @Current INT; 
        SELECT @Current = Quantity FROM Items WITH (UPDLOCK) WHERE Id = @ItemId; 
        IF @Current IS NULL THROW 50001, 'Item not found.', 1; 
        IF @Type = 'SALE' AND @Quantity > @Current 
            THROW 50002, 'Insufficient stock.', 1; 
  
        UPDATE Items
         SET Quantity = CASE WHEN @Type = 'SALE' THEN Quantity - @Quantity 
                            ELSE Quantity + @Quantity END 
        WHERE Id = @ItemId; 
  
        INSERT INTO Transactions (ItemId, [Type], Quantity, UnitPrice, Notes, CreatedBy) 
        VALUES (@ItemId, @Type, @Quantity, @UnitPrice, @Notes, @CreatedBy); 
        COMMIT; 
    END TRY 
    BEGIN CATCH 
        IF @@TRANCOUNT > 0 ROLLBACK; 
        THROW; 
    END CATCH 
END