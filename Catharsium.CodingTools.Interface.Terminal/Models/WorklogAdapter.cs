using Atlassian.Jira;
namespace Catharsium.CodingTools.Interface.Terminal.Models;

public class WorklogAdapter
{
    public Worklog InternalWorklog { get; set; }

    public WorklogAdapter(Worklog worklog)
    {
        this.InternalWorklog = worklog;
    }


    public override string ToString()
    {
        return $"{this.InternalWorklog.StartDate:yyyy-MM-dd}\t{this.ToTime(this.InternalWorklog.TimeSpentInSeconds)}";
    }


    private string ToTime(long timeSpend)
    {
        var hours = timeSpend / 60 / 60;
        var minutes = (timeSpend / 60) % 60;
        return $"{hours} hours, {minutes} minutes";
    }
}