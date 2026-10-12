CREATE TABLE [dbo].[Items] 
( 
    [Id]           INT            IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name]         NVARCHAR(100)  NOT NULL, 
    [Code]         NVARCHAR(50)   NOT NULL UNIQUE, 
    [Brand]        NVARCHAR(50)   NOT NULL, 
    [UnitPrice]    DECIMAL(10,2)  NOT NULL, 
    [Platform]     NVARCHAR(50)   NOT NULL DEFAULT 'Unknown', 
    [Condition]    NVARCHAR(20)   NOT NULL DEFAULT 'Good', 
    [Quantity]     INT            NOT NULL DEFAULT 0, 
    [ReorderLevel] INT            NOT NULL DEFAULT 2 
);