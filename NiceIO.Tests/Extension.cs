using System;
using NUnit.Framework;

namespace NiceIO.Tests
{
    [TestFixture]
    public class Extension
    {
        [Test]
        public void SimpleFileNameReturnsExtension()
        {
	        Assert.AreEqual("txt", new NPath("file.txt").Extension);
	        Assert.AreEqual("txt", new NPath("path/to/file.txt").Extension);
        }

        [Test]
        public void FileWithoutExtensionReturnsEmptyString()
        {
	        Assert.AreEqual("", new NPath("myfile").Extension);
	        Assert.AreEqual("", new NPath("path/to/myfile").Extension);
        }

        [Test]
        public void FileWithMultipleDotsReturnsExtension()
        {
	        Assert.AreEqual("exe", new NPath("path/to/myfile.something.something.exe").Extension);
	        Assert.AreEqual("exe", new NPath("myfile.something.something.exe").Extension);
        }

        [Test]
        public void UnixRootThrows()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var result = new NPath("/").Extension;
            });
        }

        [Test]
        public void WindowsRootThrows()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var result = new NPath("C:\\").Extension;
            });
        }

        [Test]
        public void WindowsuNCRootThrows()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var result = new NPath("\\\\MyWindowsPC\\").Extension;
            });
        }
    }
}
