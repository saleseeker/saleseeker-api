CREATE TABLE [dbo].[SubscribedItem] (
    [SubscribedItemID]      INT             IDENTITY (1, 1) NOT NULL,
    [UserID]                INT             NOT NULL,
    [ItemID]                INT             NOT NULL,
    [NotificationThreshold] NUMERIC (10, 2) NOT NULL,
    CONSTRAINT [PK_SubscribedItem] PRIMARY KEY CLUSTERED ([SubscribedItemID] ASC),
    CONSTRAINT [FK_SubscribedItem_Item] FOREIGN KEY ([ItemID]) REFERENCES [dbo].[Item] ([ItemID]),
    CONSTRAINT [FK_SubscribedItem_SubscribedUser] FOREIGN KEY ([UserID]) REFERENCES [dbo].[SubscribedUser] ([UserID])
);

