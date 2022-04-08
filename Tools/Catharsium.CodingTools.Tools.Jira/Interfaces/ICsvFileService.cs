namespace Catharsium.CodingTools.Tools.Jira.Interfaces;

public interface ICsvFileService
{
    void WriteToFile(string csvText, string fileName);
}