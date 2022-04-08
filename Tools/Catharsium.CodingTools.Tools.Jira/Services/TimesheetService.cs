using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.CodingTools.Tools.Jira.Models;

namespace Catharsium.CodingTools.Tools.Jira.Services;

public class TimesheetService : ITimesheetService
{
    private readonly IWorklogService worklogService;


    public TimesheetService(IWorklogService worklogService)
    {
        this.worklogService = worklogService;
    }


    public async Task<Dictionary<DateTime, List<WorklogAdapter>>> GetTimesheet(DateTime startDate, DateTime endDate)
    {
        var worklogs = await this.worklogService.GetWorklogsInPeriodForUser(startDate, endDate);
        var result = new Dictionary<DateTime, List<WorklogAdapter>>();
        foreach (var worklog in worklogs) {
            if (result.ContainsKey(worklog.StartDate.Date)) {
                result[worklog.StartDate.Date].Add(worklog);
            }
            else {
                result.Add(worklog.StartDate.Date, new List<WorklogAdapter> { worklog });
            }
        }

        return result;
    }
}