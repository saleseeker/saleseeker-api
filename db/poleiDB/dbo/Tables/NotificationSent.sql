CREATE TABLE [dbo].[NotificationSent] (
    [NotificationSentID] INT             IDENTITY (1, 1) NOT NULL,
    [UserID]             INT             NOT NULL,
    [SentDateTIme]       DATETIME        NOT NULL,
    [SentAddress]        VARCHAR (100)   NOT NULL,
    [ScrapedItemID]      INT             NOT NULL,
    [NotifiedPrice]      NUMERIC (10, 2) NOT NULL,
    CONSTRAINT [PK_NotificationSent] PRIMARY KEY CLUSTERED ([NotificationSentID] ASC),
    CONSTRAINT [FK_NotificationSent_ScrapedItem] FOREIGN KEY ([ScrapedItemID]) REFERENCES [dbo].[ScrapedItem] ([ScrapedItemID]),
    CONSTRAINT [FK_NotificationSent_SubscribedUser] FOREIGN KEY ([UserID]) REFERENCES [dbo].[SubscribedUser] ([UserID])
);

