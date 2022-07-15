CREATE TABLE [dbo].[SubscribedUser] (
    [UserID]         INT           IDENTITY (1, 1) NOT NULL,
    [EmailAddress]   VARCHAR (100) NOT NULL,
    [FirstName]      VARCHAR (100) NULL,
    [Surname]        VARCHAR (100) NULL,
    [SubscribedDate] DATETIME      NOT NULL,
    [VerifiedDate]   DATETIME      NULL,
    [CellNumber]     VARCHAR(20)   NULL, 
    CONSTRAINT [PK_SubscribedUser] PRIMARY KEY CLUSTERED ([UserID] ASC)
);

