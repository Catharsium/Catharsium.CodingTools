using Atlassian.Jira;

namespace Catharsium.CodingTools.Tools.Jira.Models;

public class JiraIssue
{
    public Issue InnerIssue { get; set; }
    public string Key { get; set; }
    public string Project { get; set; }
    public string Type { get; set; }
    public string Priority { get; set; }
    public string Summary { get; set; }
    public string JiraIdentifier { get; set; }
    public string[] Labels { get; set; }
    public Dictionary<string, string[]> CustomFields { get; set; }


    public async Task<Worklog> AddWorklogAsync(Worklog worklog, WorklogStrategy worklogStrategy = WorklogStrategy.AutoAdjustRemainingEstimate, string newEstimate = null, CancellationToken token = default)
    {
        return await this.InnerIssue.AddWorklogAsync(worklog, worklogStrategy, newEstimate, token);
    }


    public async Task DeleteWorklogAsync(JiraWorklog worklog, WorklogStrategy worklogStrategy = WorklogStrategy.AutoAdjustRemainingEstimate, string newEstimate = null, CancellationToken token = default)
    {
        await this.InnerIssue.DeleteWorklogAsync(worklog.InnerWorklog, worklogStrategy, newEstimate, token);
    }


    public override string ToString()
    {
        return $"{this.Key}\t{this.Summary}";
    }
}