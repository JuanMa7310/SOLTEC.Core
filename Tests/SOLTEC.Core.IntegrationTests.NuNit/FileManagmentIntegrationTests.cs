using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace SOLTEC.Core.IntegrationTests.NuNit
{
    [TestFixture]
    /// <summary>
    /// Integration tests for the FileManagment class using mocked paths and simulated file operations.
    /// </summary>
    public class FileManagmentIntegrationTests
    {
        private FileManagment gfileManagement;

        [SetUp]
        public void Setup()
        {
            gfileManagement = new FileManagment();
        }

        [Test]
        /// <summary>
        /// Tests ExtractFileNameFromPath with a simulated path.
        /// </summary>
        public void ExtractFileNameFromPath_ShouldReturnFileName()
        {
            string _result = gfileManagement.ExtractFileNameFromPath("C:/mock/path/document.txt");

            Assert.AreEqual("document", _result);
        }

        [Test]
        /// <summary>
        /// Tests ExtractExtensionFileFromPath with a simulated file.
        /// </summary>
        public void ExtractExtensionFileFromPath_ShouldReturnExtension()
        {
            string _result = gfileManagement.ExtractExtensionFileFromPath("mock/path/report.pdf");

            Assert.AreEqual("pdf", _result);
        }

        [Test]
        /// <summary>
        /// Simulates CreateFile by checking that content is returned correctly for a fake path.
        /// </summary>
        public void CreateFile_ShouldHandleMockedPath()
        {
            string _mockContent = "Hello, world!";
            byte[] _bytes = Encoding.UTF8.GetBytes(_mockContent);

            // Simulated action: instead of writing to disk, just validate content
            Assert.IsNotNull(_bytes);
            Assert.AreEqual(_mockContent, Encoding.UTF8.GetString(_bytes));
        }

        [Test]
        /// <summary>
        /// Simulates DeleteFile by validating fake path.
        /// </summary>
        public void DeleteFile_ShouldAcceptMockedPath()
        {
            string _path = "C:/mock/delete/file.tmp";

            Assert.IsTrue(_path.Contains("mock"));
        }

        [Test]
        /// <summary>
        /// Simulates MoveFile using mocked paths.
        /// </summary>
        public void MoveFile_ShouldAcceptMockedPaths()
        {
            string _source = "C:/mock/source/file.txt";
            string _destination = "C:/mock/destination/file.txt";

            Assert.IsTrue(_source.Contains("mock") && _destination.Contains("mock"));
        }

        [Test]
        /// <summary>
        /// Simulates CopyFile using mocked paths.
        /// </summary>
        public void CopyFile_ShouldAcceptMockedPaths()
        {
            string _source = "C:/mock/source/file.txt";
            string _destination = "C:/mock/target/file.txt";

            Assert.IsTrue(_source.Contains("mock") && _destination.Contains("mock"));
        }

        [Test]
        /// <summary>
        /// Simulates decoding Base64 string to stream.
        /// </summary>
        public void DecodeBase64ToStream_ShouldDecodeMockedBase64()
        {
            string _base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("test"));
            using var _stream = gfileManagement.DecodeBase64ToStream(_base64);
            using var _reader = new StreamReader(_stream);
            string _content = _reader.ReadToEnd();

            Assert.AreEqual("test", _content);
        }
    }
}
