using NUnit.Framework;
using SOLTEC.Core;
using System.IO;
using System.Text;

namespace SOLTEC.Core.IntegrationTests.NuNit
{
    [TestFixture]
    public class FileManagmentIntegrationTests
    {
        [Test]
        public void CreateAndReadFile_ShouldReturnSameContent()
        {
            var _path = Path.Combine(Path.GetTempPath(), "testfile.txt");
            var _content = "This is a test";
            FileManagment.CreateFile(_path, _content);

            var _read = File.ReadAllText(_path);
            Assert.That(_read, Is.EqualTo(_content));

            FileManagment.DeleteFile(_path);
        }

        [Test]
        public void Base64EncodeDecodeFile_ShouldPreserveBinaryContent()
        {
            var _path = Path.Combine(Path.GetTempPath(), "base64test.txt");
            var _original = "Base64 Test Content";
            FileManagment.CreateFile(_path, _original);
            var _base64 = FileManagment.EncodeFileToBase64(_path);
            var _stream = FileManagment.DecodeBase64ToStream(_base64);

            using var _reader = new StreamReader(_stream, Encoding.UTF8);
            var _decoded = _reader.ReadToEnd();

            Assert.That(_decoded, Is.EqualTo(_original));
            FileManagment.DeleteFile(_path);
        }
    }
}
