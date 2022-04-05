namespace Catharsium.CodingTools.Tools.Jira._Configuration;

public static class JiraQueries
{
    public static readonly string ActiveSprint =
        "project = IDN AND " +
        "sprint in openSprints() AND " +
        "(issuetype = Story OR issuetype = Bug OR issuetype = Improvement) OR " +
        "issuekey = IDN-234";
    public static readonly string LoggedDuringPeriod =
        "project = IDN AND " +
        "worklogDate >= \"{startDate}\" AND worklogDate <= \"{endDate}\" AND " +
        "worklogAuthor = currentUser()";
}