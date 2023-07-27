using Atlassian.Jira;
using Catharsium.CodingTools.Tools.Jira.Models;

namespace Catharsium.CodingTools.Tools.Jira.Interfaces;

public interface IJiraIssueMapper
{
    JiraIssue Map(Issue issue);
}