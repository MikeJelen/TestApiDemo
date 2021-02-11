use MikeDemo
go

DROP TABLE IF EXISTS [dbo].[Product]
GO

CREATE TABLE [dbo].[Product] (
    [ProductId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (200) NOT NULL,
    [CreatedOn] DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductId] ASC)
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [UIX_Product_Name]
    ON [dbo].[Product]([Name] ASC);
GO