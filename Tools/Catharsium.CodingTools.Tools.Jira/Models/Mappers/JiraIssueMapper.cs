using Atlassian.Jira;
using Catharsium.CodingTools.Tools.Jira.Interfaces;

namespace Catharsium.CodingTools.Tools.Jira.Models.Mappers;

public class JiraIssueMapper : IJiraIssueMapper
{
    public JiraIssue Map(Issue issue)
    {
        var result = new JiraIssue {
            InnerIssue = issue,
            Key = issue.Key.Value,
            Project = issue.Project,
            Type = issue.Type.Name,
            Priority = issue.Priority.Name,
            Summary = issue.Summary,
            JiraIdentifier = issue.JiraIdentifier,
        };

        if (issue.Labels != null) {
            result.Labels = issue.Labels.ToArray();
        }

        if (issue.CustomFields != null) {
            result.CustomFields = new(issue.CustomFields.Select(f => new KeyValuePair<string, string[]>(f.Name, f.Values)));
        }

        return result;
    }
}