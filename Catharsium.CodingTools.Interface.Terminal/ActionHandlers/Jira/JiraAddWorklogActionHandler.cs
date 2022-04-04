using Atlassian.Jira;
using Catharsium.CodingTools.Interface.Terminal._Configuration;
using Catharsium.CodingTools.Interface.Terminal.ActionHandlers._Interfaces;
using Catharsium.Util.IO.Console.ActionHandlers.Base;
using Catharsium.Util.IO.Console.Interfaces;

namespace Catharsium.CodingTools.Interface.Terminal.ActionHandlers.Jira
{
    public class JiraAddWorklogActionHandler : BaseActionHandler, IJiraActionHandler
    {
        private readonly CodingToolsTerminalSettings settings;


        public JiraAddWorklogActionHandler(IConsole console, CodingToolsTerminalSettings settings)
            : base(console, "Add worklog")
        {
            this.settings = settings;
        }


        public async Task Run(string issueKey)
        {
            var jira = Atlassian.Jira.Jira.CreateRestClient("https://tickets.unifiedpost.com", this.settings.Username, this.settings.Password);
            var issue = await jira.Issues.GetIssueAsync("IDN-" + issueKey);
            if (issue == null) {
                this.console.WriteLine("Issue was not found");
                return;
            }
            var timespent = this.console.AskForText("Time spent (in Jira notation)");
            var startDate = this.console.AskForDate("Date for the worklog (yyyy-MM-dd)", DateTime.Today);
            await issue.AddWorklogAsync(new Worklog(timespent, startDate));
        }


        public override async Task Run()
        {
            var issueKey = this.console.AskForText("Which issue?");
            await this.Run(issueKey);
        }
    }
}