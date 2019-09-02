using System;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class PlayerConnectionPortTests
    {
        private const string CorrectTemplate = "PlayerConnection initialized network socket : 0.0.0.0 {0}";

        [Test]
        public void ExtractPlayerConnectionPort_throws_exception_if_no_match()
        {
            var noPortHere = "There isn't a port here.\nYou really shouldn't even bother looking.";
            Assert.Throws<Exception>(() => WorkerConnector.ExtractPlayerConnectionPort(noPortHere));
        }

        [Test]
        public void ExtractPlayerConnectionPort_finds_port_regardless_of_position_in_file()
        {
            const string validPort = "5555";

            // First start with a single line.
            var exampleFile = string.Format(CorrectTemplate, validPort);
            Assert.AreEqual(ushort.Parse(validPort), WorkerConnector.ExtractPlayerConnectionPort(exampleFile));

            // Put something in front.
            exampleFile = $"Something in front!\n{exampleFile}";
            Assert.AreEqual(ushort.Parse(validPort), WorkerConnector.ExtractPlayerConnectionPort(exampleFile));

            // Put something behind.
            exampleFile = $"{exampleFile}\nSomething behind!";
            Assert.AreEqual(ushort.Parse(validPort), WorkerConnector.ExtractPlayerConnectionPort(exampleFile));
        }

        [TestCase("0", typeof(Exception))]
        [TestCase("70000", typeof(OverflowException))]
        public void ExtractPlayerConnectionPort_throws_exception_if_invalid_port(string port, Type exceptionType)
        {
            var file = string.Format(CorrectTemplate, port);
            Assert.Throws(exceptionType, () => WorkerConnector.ExtractPlayerConnectionPort(file));
        }
    }
}
