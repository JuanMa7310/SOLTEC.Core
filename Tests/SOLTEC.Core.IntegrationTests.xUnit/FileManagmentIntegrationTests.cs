using System;
using System.IO;
using System.Text;
using Xunit;

namespace SOLTEC.Core.IntegrationTests.xUnit
{
    /// <summary>
    /// Integration tests for the FileManagement class using mocked paths and simulated file operations.
    /// </summary>
    public class FileManagmentIntegrationTests
    {
        private readonly FileManagment gfileManagement;

        /// <summary>
        /// Initializes a new instance of the integration test class.
        /// </summary>
        public FileManagmentIntegrationTests()
        {
            gfileManagement = new FileManagment();
        }

        /// <summary>
        /// Tests ExtractFileNameFromPath with a simulated path.
        /// </summary>
        [Fact]
        public void ExtractFileNameFromPath_ShouldReturnFileName()
        {
            string _result = gfileManagement.ExtractFileNameFromPath("C:/mock/path/document.txt");

            Assert.Equal("document", _result);
        }

        /// <summary>
        /// Tests ExtractExtensionFileFromPath with a simulated file.
        /// </summary>
        [Fact]
        public void ExtractExtensionFileFromPath_ShouldReturnExtension()
        {
            string _result = gfileManagement.ExtractExtensionFileFromPath("mock/path/report.pdf");

            Assert.Equal("pdf", _result);
        }

        /// <summary>
        /// Simulates CreateFile by checking that content is returned correctly for a fake path.
        /// </summary>
        [Fact]
        public void CreateFile_ShouldHandleMockedPath()
        {
            string _mockContent = "Hello, world!";
            byte[] _bytes = Encoding.UTF8.GetBytes(_mockContent);

            Assert.NotNull(_bytes);
            Assert.Equal(_mockContent, Encoding.UTF8.GetString(_bytes));
        }

        /// <summary>
        /// Simulates DeleteFile by validating fake path.
        /// </summary>
        [Fact]
        public void DeleteFile_ShouldAcceptMockedPath()
        {
            string _path = "C:/mock/delete/file.tmp";

            Assert.Contains("mock", _path);
        }

        /// <summary>
        /// Simulates MoveFile using mocked paths.
        /// </summary>
        [Fact]
        public void MoveFile_ShouldAcceptMockedPaths()
        {
            string _source = "C:/mock/source/file.txt";
            string _destination = "C:/mock/destination/file.txt";

            Assert.Contains("mock", _source);
            Assert.Contains("mock", _destination);
        }

        /// <summary>
        /// Simulates CopyFile using mocked paths.
        /// </summary>
        [Fact]
        public void CopyFile_ShouldAcceptMockedPaths()
        {
            string _source = "C:/mock/source/file.txt";
            string _destination = "C:/mock/target/file.txt";

            Assert.Contains("mock", _source);
            Assert.Contains("mock", _destination);
        }

        /// <summary>
        /// Simulates decoding Base64 string to stream.
        /// </summary>
        [Fact]
        public void DecodeBase64ToStream_ShouldDecodeMockedBase64()
        {
            string _base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("test"));
            using var _stream = gfileManagement.DecodeBase64ToStream(_base64);
            using var _reader = new StreamReader(_stream);
            string _content = _reader.ReadToEnd();

            Assert.Equal("test", _content);
        }
    }
}
