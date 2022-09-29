namespace saleseeker_api.UI.Models
{
    public class UISite
    {
        public int Id { get; }
        public string? Name { get; }
        public string? Logo { get; }
        public string? Url { get; }

        public UISite (int id, string name, string logo, string url)
        {
            Id = id;
            Name = name;
            Logo = logo;
            Url = url;
        }
    }
}
