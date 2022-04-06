using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;
using System.Diagnostics.CodeAnalysis;
namespace Catharsium.CodingTools.Tools.Jira.Client;

[ExcludeFromCodeCoverage]
public class JiraClient : IJiraClient
{
    private readonly Atlassian.Jira.Jira jira;


    public JiraClient(Atlassian.Jira.Jira jira)
    {
        this.jira = jira;
    }


    public async Task<IEnumerable<IssueAdapter>> GetIssuesByQuery(string query)
    {
        return (await this.jira.Issues.GetIssuesFromJqlAsync(query)).Select(i => new IssueAdapter(i));
    }


    public async Task<IEnumerable<WorklogAdapter>> GetWorklogs(IssueAdapter issue)
    {
        return (await this.jira.Issues.GetWorklogsAsync(issue.Key)).Select(l => new WorklogAdapter(l, issue));
    }


    public async Task DeleteWorklog(string issueKey, string worklogId)
    {
        await this.jira.Issues.DeleteWorklogAsync(issueKey, worklogId);
    }
}