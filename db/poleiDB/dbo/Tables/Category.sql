CREATE TABLE [dbo].[Category] (
    [CategoryID]          INT           IDENTITY (1, 1) NOT NULL,
    [ParentCategoryID]    INT           NULL,
    [CategoryName]        VARCHAR (100) NOT NULL,
    [CategoryDescription] VARCHAR (200) NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([CategoryID] ASC),
    CONSTRAINT [FK_Category_Category] FOREIGN KEY ([ParentCategoryID]) REFERENCES [dbo].[Category] ([CategoryID])
);

