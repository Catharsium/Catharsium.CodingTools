using Catharsium.CodingTools.Interface.Terminal._Configuration;
using Catharsium.CodingTools.Interface.Terminal.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Interface.Terminal.Models;
namespace Catharsium.CodingTools.Interface.Terminal.ActionHandlers.Jira.Steps;

public class JiraWorklogRetriever : IJiraWorklogRetriever
{
    private readonly Atlassian.Jira.Jira jira;
    private readonly CodingToolsTerminalSettings settings;


    public JiraWorklogRetriever(Atlassian.Jira.Jira jira, CodingToolsTerminalSettings settings)
    {
        this.jira = jira;
        this.settings = settings;
    }


    public async Task<List<WorklogAdapter>> GetCurrentUserWorklogs(IssueAdapter issue)
    {
        return (await this.jira.Issues.GetWorklogsAsync(issue.InternalIssue.Key.Value))
            .Where(l => l.Author == this.settings.Jira.Username)
            .Select(l => new WorklogAdapter(l, issue))
            .ToList();
    }
}