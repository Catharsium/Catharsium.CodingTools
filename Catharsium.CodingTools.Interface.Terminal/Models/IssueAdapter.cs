using Atlassian.Jira;
namespace Catharsium.CodingTools.Interface.Terminal.Models;

public class IssueAdapter
{
    public Issue InternalIssue { get; }


    public IssueAdapter(Issue issue)
    {
        this.InternalIssue = issue;
    }


    public override string ToString()
    {
        return $"{this.InternalIssue.Key}\t{this.InternalIssue.Summary}";
    }
}