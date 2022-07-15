CREATE TABLE [dbo].[Site] (
    [SiteID]      INT           IDENTITY (1, 1) NOT NULL,
    [SiteName]    VARCHAR (200) NOT NULL,
    [SiteHomeURL] VARCHAR (200) NOT NULL,
    [SiteLogoURL] VARCHAR (200) NULL,
    CONSTRAINT [PK_Site] PRIMARY KEY CLUSTERED ([SiteID] ASC)
);

