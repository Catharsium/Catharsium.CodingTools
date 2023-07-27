using Atlassian.Jira;
using Catharsium.CodingTools.Tools.Jira.Models;

namespace Catharsium.CodingTools.Tools.Jira.Interfaces;

public interface IWorklogMapper
{
    JiraWorklog Map(Worklog worklog, JiraIssue issue);
}