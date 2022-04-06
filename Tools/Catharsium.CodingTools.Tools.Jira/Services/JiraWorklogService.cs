﻿using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;
namespace Catharsium.CodingTools.Tools.Jira.Services;

public class JiraWorklogService : IJiraWorklogService
{
    private readonly Atlassian.Jira.Jira jira;
    private readonly JiraCodingToolsSettings settings;


    public JiraWorklogService(Atlassian.Jira.Jira jira, JiraCodingToolsSettings settings)
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