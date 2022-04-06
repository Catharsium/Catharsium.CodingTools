using Atlassian.Jira;
namespace Catharsium.CodingTools.Tools.Jira.Models;

public class IssueAdapter
{
    private Issue InternalIssue { get; }


    public IssueAdapter(Issue issue)
    {
        this.InternalIssue = issue;
    }


    public string Key => this.InternalIssue.Key.Value;

    public string Summary => this.InternalIssue.Summary;

    public string JiraIdentifier => this.InternalIssue.JiraIdentifier;


    public async Task<Worklog> AddWorklogAsync(Worklog worklog, WorklogStrategy worklogStrategy = WorklogStrategy.AutoAdjustRemainingEstimate, string newEstimate = null, CancellationToken token = default)
    {
        return await this.InternalIssue.AddWorklogAsync(worklog, worklogStrategy, newEstimate, token);
    }


    public override string ToString()
    {
        return $"{this.InternalIssue.Key}\t{this.InternalIssue.Summary}";
    }
}