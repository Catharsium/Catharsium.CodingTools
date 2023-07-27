using Atlassian.Jira;
using Catharsium.CodingTools.Tools.Jira.Interfaces;

namespace Catharsium.CodingTools.Tools.Jira.Models.Mappers;

public class WorklogMapper : IWorklogMapper
{
    public JiraWorklog Map(Worklog worklog, JiraIssue issue)
    {
        return new JiraWorklog {
            InnerIssue = issue,
            InnerWorklog = worklog,
            Id = worklog.Id,
            Author = worklog.Author,
            StartDate = worklog.StartDate.Value,
            TimeSpentInSeconds = worklog.TimeSpentInSeconds
        };
    }
}