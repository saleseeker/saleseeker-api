
CREATE   VIEW [dbo].[vwSiteItemPriceMonth]
AS
SELECT        dbo.SiteItem.SiteItemID, MIN(dbo.ScrapedItem.PriceIncVAT) AS MinPriceIncVAT, MAX(dbo.ScrapedItem.PriceIncVAT) AS MaxPriceIncVAT, AVG(dbo.ScrapedItem.PriceIncVAT) AS AvgPriceIncVAT
FROM            dbo.SiteItem INNER JOIN
                         dbo.ScrapedItem ON dbo.SiteItem.SiteItemID = dbo.ScrapedItem.SiteItemID
WHERE        (dbo.ScrapedItem.ScrapedDateTime > DATEADD(m, - 1, GETDATE()))
GROUP BY dbo.SiteItem.SiteItemID
