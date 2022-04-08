using Catharsium.CodingTools.Tools.Jira._Configuration;
using Catharsium.CodingTools.Tools.Jira.Services;
using Catharsium.Util.IO.Files.Interfaces;
using Catharsium.Util.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.IO;

namespace Catharsium.CodingTools.Tools.Jira.Tests.Services
{
    [TestClass]
    public class CsvFileServiceTests : TestFixture<CsvFileService>
    {
        #region Fixture

        private string CsvText => "My csv text";
        private string FileName => "My file name";

        private JiraCodingToolsSettings Settings { get; set; }
        private IFile File { get; set; }
        private MemoryStream MemoryStream { get; set; }


        [TestInitialize]
        public void Initialize()
        {
            this.Settings = new JiraCodingToolsSettings {
                ReportSettings = new JiraReportSettings {
                    FilesFolder = "My files folder"
                }
            };
            this.SetDependency(this.Settings);

            this.File = Substitute.For<IFile>();
            this.MemoryStream = new MemoryStream();
            this.File.CreateText().Returns(new StreamWriter(this.MemoryStream));

            this.File.CreationTime.Returns(new System.DateTime(2021, 8, 24));
            this.GetDependency<IFileFactory>().CreateFile($"{this.Settings.ReportSettings.FilesFolder}{this.FileName}.csv").Returns(this.File);
        }

        #endregion

        #region WriteToFile

        [TestMethod]
        public void WriteToFile_NewFile_WritesTextToFile()
        {
            this.File.Exists.Returns(false);
            this.Target.WriteToFile(this.CsvText, this.FileName);
            var streamCopy = new MemoryStream(this.MemoryStream.ToArray());
            streamCopy.Seek(0, SeekOrigin.Begin);
            var actualText = new StreamReader(streamCopy).ReadToEnd();
            Assert.AreEqual(this.CsvText, actualText);
        }


        [TestMethod]
        public void WriteToFile_ExistingFile_DeletesFile()
        {
            this.File.Exists.Returns(true);
            this.Target.WriteToFile(this.CsvText, this.FileName);
            this.File.Received().Delete();
        }

        #endregion
    }
}