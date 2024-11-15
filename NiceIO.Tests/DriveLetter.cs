using NUnit.Framework;

namespace NiceIO.Tests
{
    [TestFixture]
    public class DriveLetter
    {
	    [Test]
	    public void DriveAlwaysUpper()
	    {
		    Assert.AreEqual(new NPath("c:/").ToString(), "C:/");
		    Assert.AreEqual(new NPath("C:/").ToString(), "C:/");
	    }

	    [Test]
        public void CRoot()
	    {
		    Assert.AreEqual("C", new NPath("C:/").DriveLetter);
		    Assert.AreEqual("C", new NPath("c:/").DriveLetter);
	    }

        [Test]
        public void DRoot()
        {
	        Assert.AreEqual("D", new NPath("D:/").DriveLetter);
	        Assert.AreEqual("D", new NPath("d:/").DriveLetter);
        }

        [Test]
        public void NormalRoot() => Assert.AreEqual(null, new NPath("/").DriveLetter);

        [Test]
        public void RelativeFile() => Assert.AreEqual(null, new NPath("somefile").DriveLetter);

        [Test]
        public void NormalAbsoluteFile() => Assert.AreEqual(null, new NPath("/somedir/somefile").DriveLetter);

        [Test]
        public void CAbsoluteFile()
        {
	        Assert.AreEqual("C", new NPath("C:/somedir/somefile").DriveLetter);
	        Assert.AreEqual("C", new NPath("c:/somedir/somefile").DriveLetter);
        }

        [Test]
        public void UNCRoot() => Assert.AreEqual(null, new NPath("\\\\MyWindowsPC/").DriveLetter);

        [Test]
        public void UNCPath() => Assert.AreEqual(null, new NPath("\\\\MyWindowsPC/somefile").DriveLetter);
    }
}
