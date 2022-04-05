using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;

namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers.Steps;

public class JiraWorklogRetriever : IJiraWorklogRetriever
{
    private readonly Atlassian.Jira.Jira jira;
    private readonly JiraCodingToolsSettings settings;


    public JiraWorklogRetriever(Atlassian.Jira.Jira jira, JiraCodingToolsSettings settings)
    {
        this.jira = jira;
        this.settings = settings;
    }


    public async Task<List<WorklogAdapter>> GetCurrentUserWorklogs(IssueAdapter issue)
    {
        return (await this.jira.Issues.GetWorklogsAsync(issue.Key))
            .Where(l => l.Author == this.settings.Username)
            .Select(l => new WorklogAdapter(l, issue))
            .ToList();
    }
}