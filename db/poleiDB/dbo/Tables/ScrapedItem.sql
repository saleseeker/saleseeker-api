CREATE TABLE [dbo].[ScrapedItem] (
    [ScrapedItemID]   INT             IDENTITY (1, 1) NOT NULL,
    [SiteItemID]      INT             NOT NULL,
    [PriceIncVAT]     NUMERIC (10, 2) NOT NULL,
    [PriceExVAT]      NUMERIC (10, 2) NULL,
    [ScrapedDateTime] DATETIME        NOT NULL,
    CONSTRAINT [PK_ScrapedItem] PRIMARY KEY CLUSTERED ([ScrapedItemID] ASC),
    CONSTRAINT [FK_ScrapedItem_SiteItem] FOREIGN KEY ([SiteItemID]) REFERENCES [dbo].[SiteItem] ([SiteItemID])
);

