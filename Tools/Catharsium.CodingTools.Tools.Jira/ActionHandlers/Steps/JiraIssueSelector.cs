using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.CodingTools.Tools.Jira.ActionHandlers._Interfaces;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;
using Catharsium.Util.IO.Console.Interfaces;

namespace Catharsium.CodingTools.Tools.Jira.ActionHandlers.Steps;

public class JiraIssueSelector : IJiraIssueSelector
{
    private readonly Atlassian.Jira.Jira jira;
    private readonly IJiraIssueMapper jiraIssueMapper;
    private readonly JiraCodingToolsSettings settings;
    private readonly IConsole console;


    public JiraIssueSelector(Atlassian.Jira.Jira jira, IJiraIssueMapper jiraIssueMapper, JiraCodingToolsSettings settings, IConsole console)
    {
        this.jira = jira;
        this.jiraIssueMapper = jiraIssueMapper;
        this.settings = settings;
        this.console = console;
    }


    public async Task<JiraIssue> SelectIssue()
    {
        var jiraIssues = await this.jira.Issues.GetIssuesFromJqlAsync(JiraQueries.ActiveSprint);
        var issues = jiraIssues.Select(i => this.jiraIssueMapper.Map(i));
        var selectedIssue = this.console.AskForItem(issues, "Select a ticket (or empty to manually enter):");
        if (selectedIssue == null) {
            var selectedIssueKey = this.console.AskForText("Enter the issue key");
            if (!selectedIssueKey.Contains('-')) {
                selectedIssueKey = $"{this.settings.DefaultProject}-{selectedIssueKey}";
            }

            var manualIssue = await this.jira.Issues.GetIssueAsync(selectedIssueKey);
            if (manualIssue == null) {
                this.console.WriteLine($"Ticket {selectedIssueKey} was not found.");
                return null;
            }

            selectedIssue = this.jiraIssueMapper.Map(manualIssue);
        }

        return selectedIssue;
    }
}