using Atlassian.Jira;
namespace Catharsium.CodingTools.Models;

public class WorklogAdapter
{
    private readonly Worklog worklog;
    private readonly IssueAdapter issue;


    public WorklogAdapter(Worklog worklog, IssueAdapter issue)
    {
        this.worklog = worklog;
        this.issue = issue;
    }


    public string Id => this.worklog.Id;

    public DateTime StartDate => this.worklog.StartDate.Value;

    public long TimeSpentInSeconds => this.worklog.TimeSpentInSeconds;



    public override string ToString()
    {
        var timespan = TimeSpan.FromSeconds(this.worklog.TimeSpentInSeconds);
        return $"{this.worklog.StartDate:yyyy-MM-dd}\t{timespan.Hours:0}:{timespan.Minutes:00}";
    }


    public string ToReferenceString()
    {
        var timespan = TimeSpan.FromSeconds(this.worklog.TimeSpentInSeconds);
        var space = this.issue.Key.Length < 8
            ? "\t"
            : "";
        return $"{this.issue.Key}\t{space}{timespan.Hours:0}:{timespan.Minutes:00}";
    }
}