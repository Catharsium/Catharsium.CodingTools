using Catharsium.CodingTools.Interface.Terminal._Configuration;
using Catharsium.CodingTools.Interface.Terminal.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Interface.Terminal.Models;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.Interface.Terminal.ActionHandlers.Jira;

public class JiraSprintOverviewActionHandler : BaseActionHandler, IJiraActionHandler
{
    private readonly Atlassian.Jira.Jira jira;


    public JiraSprintOverviewActionHandler(Atlassian.Jira.Jira jira, IConsole console)
        : base(console, "Sprint overview")
    {
        this.jira = jira;
    }


    public override async Task Run()
    {
        var query = JiraQueries.ActiveSprint;
        var issues = (await this.jira.Issues.GetIssuesFromJqlAsync(query)).Select(i => new IssueAdapter(i));
    }
}