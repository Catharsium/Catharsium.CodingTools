using Catharsium.CodingTools.Tools.Jira.Models;

namespace Catharsium.CodingTools.Tools.Jira.Interfaces;

public interface ITimesheetService
{
    Task<Dictionary<DateTime, List<JiraWorklog>>> GetTimesheet(DateTime startDate, DateTime endDate);
}