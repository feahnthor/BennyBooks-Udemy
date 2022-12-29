using YamlDotNet.Serialization;

namespace BennyBooks.Models
{
    public class PortfolioNamingConvention : INamingConvention
    {
        public string Apply(string value)
        {
            switch (value)
            {
                case "title":
                    return nameof(Portfolio.Title);
                case "description":
                    return nameof(Portfolio.Description);
                case "img":
                    return nameof(Portfolio.ImageUrl);
                case "technologies_used":
                    return nameof(Portfolio.TechnologiesUsed);
                case "project_url":
                    return nameof(Portfolio.ProjectUrl);
                default:
                    return value;
            }
        }
    }
}
