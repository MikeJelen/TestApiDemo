use MikeDemo
go

DROP TABLE IF EXISTS [dbo].[ProductInventory]
GO

CREATE TABLE [dbo].[ProductInventory] (
    [ProductInventoryId] INT           IDENTITY (1, 1) NOT NULL,
    [ProductId]          INT           NOT NULL,
    [Quantity]           INT           DEFAULT ((0)) NOT NULL,
    [CreatedOn]          DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [LastUpdateOn]       DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductInventoryId] ASC),
    CONSTRAINT [FK_ProductInventory_ToProduct] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId])
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [UIX_ProductInventory_ProductId]
    ON [dbo].[ProductInventory]([ProductId] ASC);
GO