using Catharsium.CodingTools.Interface.Terminal._Configuration;
using Catharsium.CodingTools.Interface.Terminal.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Interface.Terminal.Models;
using Catharsium.Util.IO.Console.Interfaces;
namespace Catharsium.CodingTools.Interface.Terminal.ActionHandlers.Jira.Steps;

public class JiraIssueSelector : IJiraIssueSelector
{
    private readonly Atlassian.Jira.Jira jira;
    private readonly CodingToolsTerminalSettings settings;
    private readonly IConsole console;


    public JiraIssueSelector(Atlassian.Jira.Jira jira, CodingToolsTerminalSettings settings, IConsole console)
    {
        this.jira = jira;
        this.settings = settings;
        this.console = console;
    }


    public async Task<IssueAdapter> SelectIssue()
    {
        var jiraIssues = await this.jira.Issues.GetIssuesFromJqlAsync(JiraQueries.ActiveSprint);
        var issues = jiraIssues.Select(i => new IssueAdapter(i));
        var selectedIssue = this.console.AskForItem(issues, "Select a ticket (or empty to manually enter):");
        if (selectedIssue == null) {
            var selectedIssueKey = this.console.AskForText("Enter the issue key");
            if (!selectedIssueKey.Contains('-')) {
                selectedIssueKey = $"{this.settings.Jira.DefaultProject}-{selectedIssueKey}";
            }

            var manualIssue = await this.jira.Issues.GetIssueAsync(selectedIssueKey);
            if (manualIssue == null) {
                this.console.WriteLine($"Ticket {selectedIssueKey} was not found.");
                return null;
            }

            selectedIssue = new IssueAdapter(manualIssue);
        }

        return selectedIssue;
    }
}