using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;
using System.Diagnostics.CodeAnalysis;

namespace Catharsium.CodingTools.Tools.Jira.Client;

[ExcludeFromCodeCoverage]
public class JiraClient : IJiraClient
{
    private readonly Atlassian.Jira.Jira jira;
    private readonly IJiraIssueMapper jiraIssueMapper;
    private readonly IWorklogMapper worklogMapper;


    public JiraClient(Atlassian.Jira.Jira jira, IJiraIssueMapper jiraIssueMapper, IWorklogMapper worklogMapper)
    {
        this.jira = jira;
        this.jiraIssueMapper = jiraIssueMapper;
        this.worklogMapper = worklogMapper;
    }


    public async Task<JiraIssue> GetIssue(string key)
    {
        return this.jiraIssueMapper.Map(await this.jira.Issues.GetIssueAsync(key));
    }


    public async Task<IEnumerable<JiraIssue>> GetIssuesByQuery(string query)
    {
        var result = new List<JiraIssue>();
        var startAt = 0;
        var pageSize = 50;

        while (true) {
            var issues = (await this.jira.Issues.GetIssuesFromJqlAsync(query, int.MaxValue, startAt)).Select(i => this.jiraIssueMapper.Map(i));
            result.AddRange(issues);
            if (issues.Count() < pageSize) {
                break;
            }
            startAt += pageSize;
        }

        return result;
    }


    public async Task<IEnumerable<JiraWorklog>> GetWorklogs(JiraIssue issue)
    {
        return (await this.jira.Issues.GetWorklogsAsync(issue.Key)).Select(l => this.worklogMapper.Map(l, issue));
    }


    public async Task DeleteWorklog(string issueKey, string worklogId)
    {
        await this.jira.Issues.DeleteWorklogAsync(issueKey, worklogId);
    }
}