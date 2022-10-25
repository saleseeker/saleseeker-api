using saleseeker_data;

namespace saleseeker_api.UI.Models
{
    public class UISite
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Logo { get; set; }
        public string? Url { get; set; }

        public UISite()
        {

        }
        public UISite(int id, string name, string logo, string url)
        {
            Id = id;
            Name = name;
            Logo = logo;
            Url = url;
        }
        public UISite? Site(SSDbContext _context, int id)
        {
           return _context.Sites.Where(site => site.SiteId == id)
                                .Select(s => new UISite
                                {
                                    Id = s.SiteId,
                                    Name = s.SiteName,
                                    Logo = s.SiteLogoUrl,
                                    Url = s.SiteHomeUrl
                                })
                                .FirstOrDefault();
        }

        public List<UISite> Sites(SSDbContext _context)
        {
            return _context.Sites
                            .Select(s => new UISite
                                {
                                    Id = s.SiteId,
                                    Name = s.SiteName,
                                    Logo = s.SiteLogoUrl,
                                    Url = s.SiteHomeUrl
                                })
                            .ToList() ?? new List<UISite>();
        }
    }
}
