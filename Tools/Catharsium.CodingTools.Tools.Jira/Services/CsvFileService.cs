using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.CodingTools.Tools.Jira.Interfaces;
using Catharsium.Util.IO.Files.Interfaces;

namespace Catharsium.CodingTools.Tools.Jira.Services;

public class CsvFileService : ICsvFileService
{
    private readonly IFileFactory fileFactory;
    private readonly JiraCodingToolsSettings settings;


    public CsvFileService(IFileFactory fileFactory, JiraCodingToolsSettings settings)
    {
        this.fileFactory = fileFactory;
        this.settings = settings;
    }


    public void WriteToFile(string csvText, string fileName)
    {
        var file = this.fileFactory.CreateFile($"{this.settings.ReportSettings.FilesFolder}{fileName}.csv");
        if (file.Exists) {
            file.Delete();
        }

        using var stream = file.CreateText();
        stream.Write(csvText);
    }
}