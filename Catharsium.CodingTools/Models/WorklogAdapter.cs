﻿using Atlassian.Jira;
namespace Catharsium.CodingTools.Models;

public class WorklogAdapter
{
    public Worklog InternalWorklog { get; set; }
    public IssueAdapter Issue { get; set; }


    public WorklogAdapter(Worklog worklog, IssueAdapter issue)
    {
        this.InternalWorklog = worklog;
        this.Issue = issue;
    }


    public override string ToString()
    {
        var timespan = TimeSpan.FromSeconds(this.InternalWorklog.TimeSpentInSeconds);
        return $"{this.InternalWorklog.StartDate:yyyy-MM-dd}\t{timespan.Hours:0}:{timespan.Minutes:00}";
    }


    public string ToReferenceString()
    {
        var timespan = TimeSpan.FromSeconds(this.InternalWorklog.TimeSpentInSeconds);
        var space = this.Issue.Key.Length < 8
            ? "\t"
            : "";
        return $"{this.Issue.Key}\t{space}{timespan.Hours:0}:{timespan.Minutes:00}";
    }
}