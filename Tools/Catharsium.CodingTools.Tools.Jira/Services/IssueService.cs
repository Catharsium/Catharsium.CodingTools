using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;

namespace Catharsium.CodingTools.Tools.Jira.Services;

public class IssueService : IIssueService
{
    private readonly IJiraClient jiraClient;


    public IssueService(IJiraClient jiraClient)
    {
        this.jiraClient = jiraClient;
    }


    public async Task<JiraIssue> GetIssueByKey(string key)
    {
        return await this.jiraClient.GetIssue(key);
    }


    public async Task<JiraIssue> GetEpicForIssue(JiraIssue issue)
    {
        var customFields = issue.CustomFields;
        if (customFields == null || !customFields.ContainsKey(CustomFields.EpicLink) || !customFields[CustomFields.EpicLink].Any()) {
            return null;
        }

        var epicLink = customFields[CustomFields.EpicLink][0];
        if (string.IsNullOrWhiteSpace(epicLink)) {
            return null;
        }

        return await this.GetIssueByKey(epicLink);
    }
}