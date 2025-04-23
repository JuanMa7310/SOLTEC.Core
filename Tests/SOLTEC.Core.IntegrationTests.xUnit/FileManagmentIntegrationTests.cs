using Xunit;
using SOLTEC.Core;
using System.IO;
using System.Text;

namespace SOLTEC.Core.IntegrationTests.xUnit
{
    public class FileManagmentIntegrationTests
    {
        [Fact]
        public void CreateAndReadFile_ShouldMatchOriginal()
        {
            var _path = Path.Combine(Path.GetTempPath(), "testfile.txt");
            var _content = "Integration file test";
            FileManagment.CreateFile(_path, _content);

            var _read = File.ReadAllText(_path);
            Assert.Equal(_content, _read);

            FileManagment.DeleteFile(_path);
        }

        [Fact]
        public void EncodeDecodeBase64_ShouldPreserveText()
        {
            var _path = Path.Combine(Path.GetTempPath(), "file64.txt");
            var _original = "Some base64 content!";
            FileManagment.CreateFile(_path, _original);
            var _base64 = FileManagment.EncodeFileToBase64(_path);
            var _stream = FileManagment.DecodeBase64ToStream(_base64);

            using var _reader = new StreamReader(_stream, Encoding.UTF8);
            var _decoded = _reader.ReadToEnd();

            Assert.Equal(_original, _decoded);
            FileManagment.DeleteFile(_path);
        }
    }
}
