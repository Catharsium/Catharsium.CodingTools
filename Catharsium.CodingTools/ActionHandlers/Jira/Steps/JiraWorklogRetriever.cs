using Catharsium.CodingTools._Configuration;
using Catharsium.CodingTools.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Models;
namespace Catharsium.CodingTools.ActionHandlers.Jira.Steps;

public class JiraWorklogRetriever : IJiraWorklogRetriever
{
    private readonly Atlassian.Jira.Jira jira;
    private readonly CodingToolsSettings settings;


    public JiraWorklogRetriever(Atlassian.Jira.Jira jira, CodingToolsSettings settings)
    {
        this.jira = jira;
        this.settings = settings;
    }


    public async Task<List<WorklogAdapter>> GetCurrentUserWorklogs(IssueAdapter issue)
    {
        return (await this.jira.Issues.GetWorklogsAsync(issue.Key))
            .Where(l => l.Author == this.settings.Jira.Username)
            .Select(l => new WorklogAdapter(l, issue))
            .ToList();
    }
}