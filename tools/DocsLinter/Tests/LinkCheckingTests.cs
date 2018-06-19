using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdig.Syntax.Inlines;
using NUnit.Framework;

namespace DocsLinter.Tests 
{
    [TestFixture]
    class LinkCheckingTests 
    {
        private static RemoteLink GetUrlForStatusCode(int statusCode)
        {
            var url = $"https://httpstat.us/{statusCode}";
            return new RemoteLink(new LinkInline(url, ""));
        }

        [Test]
        public void CheckRemoteLink_returns_true_for_200_status_code()
        {
            var result = Program.CheckRemoteLink(string.Empty, GetUrlForStatusCode(200));   
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckRemoteLink_returns_false_for_404_status_code()
        {
            var result = Program.CheckRemoteLink(string.Empty, GetUrlForStatusCode(404));
            Assert.IsFalse(result);
        }

        [Test]
        public void CheckRemoteLink_returns_true_for_arbitrary_status_code()
        {
            var result = Program.CheckRemoteLink(string.Empty, GetUrlForStatusCode(301)); // Permanent redirect
            Assert.IsTrue(result);
        }
    }
}
