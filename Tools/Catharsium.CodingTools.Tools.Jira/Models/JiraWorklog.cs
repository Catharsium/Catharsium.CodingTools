using Atlassian.Jira;

namespace Catharsium.CodingTools.Tools.Jira.Models;

public class JiraWorklog
{
    public Worklog InnerWorklog { get; set; }
    public JiraIssue InnerIssue { get; set; }

    public string Id { get; set; }

    public string Author { get; set; }

    public DateTime StartDate { get; set; }

    public long TimeSpentInSeconds { get; set; }


    public override string ToString()
    {
        var timespan = TimeSpan.FromSeconds(this.InnerWorklog.TimeSpentInSeconds);
        return $"{this.InnerWorklog.StartDate:yyyy-MM-dd}\t{timespan.TotalHours}";
    }


    public string ToReferenceString()
    {
        var timespan = TimeSpan.FromSeconds(this.InnerWorklog.TimeSpentInSeconds);
        var space = this.InnerIssue.Key.Length < 8
            ? "\t"
            : "";
        return $"{this.InnerIssue.Key}\t{space}{timespan.TotalHours}";
    }
}