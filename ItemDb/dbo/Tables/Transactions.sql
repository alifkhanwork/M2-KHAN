CREATE TABLE [dbo].[Transactions] 
( 
    [Id]        INT           IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [ItemId]    INT           NOT NULL FOREIGN KEY REFERENCES [dbo].[Items]([Id]), 
    [Type]      NVARCHAR(20)  NOT NULL,   -- SALE, STOCKIN, ADJUST 
    [Quantity]  INT           NOT NULL, 
    [UnitPrice] DECIMAL(10,2) NOT NULL, 
    [Notes]     NVARCHAR(200) NULL, 
    [CreatedBy] NVARCHAR(100) NOT NULL, 
    [CreatedAt] DATETIME2     NOT NULL DEFAULT SYSDATETIME() 
);