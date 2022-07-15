CREATE TABLE [dbo].[Item] (
    [ItemID]          INT           NOT NULL,
    [CategoryID]      INT           NOT NULL,
    [ItemName]        VARCHAR (200) NOT NULL,
    [ItemPhotoURL]    VARCHAR (300) NULL,
    [ItemDescription] VARCHAR (MAX) NULL,
    [ItemBarcode]     VARCHAR (100) NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED ([ItemID] ASC),
    CONSTRAINT [FK_Item_Category] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[Category] ([CategoryID])
);

