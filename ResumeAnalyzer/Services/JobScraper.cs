using HtmlAgilityPack;
using System.Net.Http;

public class JobScraper{

    private readonly HttpClient _httpClient;

    public JobScraper(){
        _httpClient = new HttpClient();
    }
    public async Task<List<JobPosting>> ScrapeJobs(string jobTitle, string location)
    {
        string searchUrl = $"https://ca.linkedin.com/jobs/search?keywords=software%20developer&location=Brampton&position=1&pageNum=0";
        var jobListings = new List<JobPosting>();

        var response = await _httpClient.GetStringAsync(searchUrl);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(response);

        var jobNodes = htmlDocument.DocumentNode.SelectNodes("//div[starts-with(@class, \"jobsearch-JobCountAndSortPane-jobCount\")]");

        if (jobNodes != null)
        {
            foreach (var jobNode in jobNodes)
            {
                string title = jobNode.SelectSingleNode(".//h2/a")?.InnerText.Trim();
                string link = jobNode.SelectSingleNode(".//h2/a")?.GetAttributeValue("href", "").Trim();
                string description = jobNode.SelectSingleNode(".//p")?.InnerText.Trim();

                jobListings.Add(new JobPosting
                {
                    Title = title,
                    Link = link,
                    Description = description
                });
            }
        }

        return jobListings;
    }

    public class JobPosting
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
    }
}