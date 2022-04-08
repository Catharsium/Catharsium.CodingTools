namespace Catharsium.CodingTools.Tools.Jira.Interfaces;

public interface IPeriodSelector
{
    (DateTime startDate, DateTime endDate) SelectWorkWeek();
}