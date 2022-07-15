CREATE TABLE [dbo].[SiteItem] (
    [SiteItemID] INT           IDENTITY (1, 1) NOT NULL,
    [SiteID]     INT           NOT NULL,
    [ItemID]     INT           NOT NULL,
    [ItemURL]    VARCHAR (300) NOT NULL,
    CONSTRAINT [PK_SiteItem] PRIMARY KEY CLUSTERED ([SiteItemID] ASC),
    CONSTRAINT [FK_SiteItem_Item] FOREIGN KEY ([ItemID]) REFERENCES [dbo].[Item] ([ItemID]),
    CONSTRAINT [FK_SiteItem_Site] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[Site] ([SiteID])
);

