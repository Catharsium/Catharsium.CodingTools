using Atlassian.Jira;
namespace Catharsium.CodingTools.Tools.Jira.Models;

public class WorklogAdapter
{
    public Worklog InternalWorklog { get; }
    public IssueAdapter Issue { get; }


    public WorklogAdapter() { }


    public WorklogAdapter(Worklog worklog, IssueAdapter issue)
    {
        this.InternalWorklog = worklog;
        this.Issue = issue;
    }


    public virtual string Id => this.InternalWorklog.Id;

    public virtual string Author => this.InternalWorklog.Author;

    public virtual DateTime StartDate => this.InternalWorklog.StartDate.Value;

    public virtual long TimeSpentInSeconds => this.InternalWorklog.TimeSpentInSeconds;


    public override string ToString()
    {
        var timespan = TimeSpan.FromSeconds(this.InternalWorklog.TimeSpentInSeconds);
        return $"{this.InternalWorklog.StartDate:yyyy-MM-dd}\t{timespan.TotalHours}";
    }


    public string ToReferenceString()
    {
        var timespan = TimeSpan.FromSeconds(this.InternalWorklog.TimeSpentInSeconds);
        var space = this.Issue.Key.Length < 8
            ? "\t"
            : "";
        return $"{this.Issue.Key}\t{space}{timespan.TotalHours}";
    }
}