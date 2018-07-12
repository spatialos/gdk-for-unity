using System;
using System.Collections.Generic;
using System.IO;
using Markdig.Syntax.Inlines;
using NUnit.Framework;

namespace DocsLinter.Tests
{
    [TestFixture]
    internal class RemoteLinkCheckingTests
    {
        private static readonly Dictionary<string, string> dotenv = new Dictionary<string, string>
        {
            { Program.GithubUrlEnvKey, "github.com/spatialos/UnityGDK" }
        };

        private static RemoteLink GetUrlForStatusCode(int statusCode)
        {
            var url = $"http://httpstat.us/{statusCode}";
            return new RemoteLink(new LinkInline(url, ""));
        }

        [OneTimeSetUp]
        public void IgnoreSslErrors()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;
        }

        [Test]
        public void CheckRemoteLink_returns_true_for_200_status_code()
        {
            var result = Program.CheckRemoteLink(string.Empty, GetUrlForStatusCode(200), dotenv);
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckRemoteLink_returns_false_for_404_status_code()
        {
            var result = Program.CheckRemoteLink(string.Empty, GetUrlForStatusCode(404), dotenv);
            Assert.IsFalse(result);
        }

        [Test]
        public void CheckRemoteLink_returns_true_for_arbitrary_status_code()
        {
            var result = Program.CheckRemoteLink(string.Empty, GetUrlForStatusCode(301), dotenv); // Permanent redirect
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckRemoteLink_returns_false_for_remote_links_to_repo_files()
        {
            var repoLinkBlob = "https://www.github.com/spatialos/UnityGDK/blob/README.md";
            var resultBlob =
                Program.CheckRemoteLink(string.Empty, new RemoteLink(new LinkInline(repoLinkBlob, "")), dotenv);
            Assert.IsFalse(resultBlob);

            var repoLinkTree = "https://www.github.com/spatialos/UnityGDK/tree/README.md";
            var resultTree =
                Program.CheckRemoteLink(string.Empty, new RemoteLink(new LinkInline(repoLinkTree, "")), dotenv);
            Assert.IsFalse(resultTree);
        }
    }

    [TestFixture]
    internal class LocalLinkCheckingTests
    {
        private string currentDirectory;
        private string tempDirectory;
        private Dictionary<string, SimplifiedMarkdownDoc> corpus = new Dictionary<string, SimplifiedMarkdownDoc>();

        private string markdownDocToTestPath;
        private SimplifiedMarkdownDoc markdownDocToTest;

        private LocalLink sameFileHeadingExistsLink;
        private LocalLink sameFileHeadingDoesNotExistLink;

        private LocalLink otherFileExistsLink;
        private LocalLink otherFileDoesNotExistLink;

        private LocalLink otherDirectoryExistsLink;
        private LocalLink otherDirectoryDoesNotExistLink;

        private LocalLink otherFileHeadingExistsLink;
        private LocalLink otherFileHeadingDoesNotExistLink;

        [OneTimeSetUp]
        public void SetupDocumentCorpus()
        {
            // Move to a temp directory
            tempDirectory = Path.Combine(Path.GetTempPath(), "linter-docs-tests");
            Directory.CreateDirectory(tempDirectory);
            currentDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = tempDirectory;

            // Create a non-markdown file for testing purposes
            Directory.CreateDirectory("test");
            var fs = File.Create("test/image.png");
            fs.Close();

            // Create other markdown file with a heading
            var otherMarkdownDocPath = Path.GetFullPath(Path.Combine(tempDirectory, "test/test.md"));
            var otherMarkdownDoc = new SimplifiedMarkdownDoc();
            otherMarkdownDoc.Headings.Add(new Heading("#test-heading"));
            corpus.Add(otherMarkdownDocPath, otherMarkdownDoc);

            // Create markdown file that would have links to be used in testing.
            markdownDocToTestPath = Path.GetFullPath(Path.Combine(tempDirectory, "links.md"));
            markdownDocToTest = new SimplifiedMarkdownDoc();
            markdownDocToTest.Headings.Add(new Heading("#local-heading"));
            corpus.Add(markdownDocToTestPath, markdownDocToTest);

            // Create same file links
            sameFileHeadingExistsLink = new LocalLink(new LinkInline("#local-heading", string.Empty));
            sameFileHeadingDoesNotExistLink = new LocalLink(new LinkInline("#incorrect-local-heading", string.Empty));

            // Create other file links (no heading)
            otherFileExistsLink = new LocalLink(new LinkInline("test/image.png", string.Empty));
            otherFileDoesNotExistLink = new LocalLink(new LinkInline("test/no-image.png", string.Empty));

            // Create other directory links.
            otherDirectoryExistsLink = new LocalLink(new LinkInline("test/", string.Empty));
            otherDirectoryDoesNotExistLink = new LocalLink(new LinkInline("no-test/", string.Empty));

            // Create other file links with heading
            otherFileHeadingExistsLink = new LocalLink(new LinkInline("test/test.md#test-heading", string.Empty));
            otherFileHeadingDoesNotExistLink = new LocalLink(new LinkInline("test/test.md#no-heading", string.Empty));
        }

        [Test]
        public void CheckLocalLink_should_return_true_if_same_file_heading_exists()
        {
            var result = Program.CheckLocalLink(markdownDocToTestPath, markdownDocToTest, sameFileHeadingExistsLink,
                corpus);
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckLocalLink_should_return_false_if_same_file_heading_does_not_exist()
        {
            var result = Program.CheckLocalLink(markdownDocToTestPath, markdownDocToTest,
                sameFileHeadingDoesNotExistLink,
                corpus);

            Assert.IsFalse(result);
        }

        [Test]
        public void CheckLocalLink_should_return_true_if_other_file_link_exists()
        {
            var result = Program.CheckLocalLink(markdownDocToTestPath, markdownDocToTest, otherFileExistsLink,
                corpus);

            Assert.IsTrue(result);
        }

        [Test]
        public void CheckLocalLink_should_return_false_if_other_file_link_does_not_exist()
        {
            var result = Program.CheckLocalLink(markdownDocToTestPath, markdownDocToTest, otherFileDoesNotExistLink,
                corpus);

            Assert.IsFalse(result);
        }

        [Test]
        public void CheckLocalLink_should_return_true_if_other_directory_link_exists()
        {
            var result = Program.CheckLocalLink(markdownDocToTestPath, markdownDocToTest, otherDirectoryExistsLink,
                corpus);

            Assert.IsTrue(result);
        }

        [Test]
        public void CheckLocalLink_should_return_false_if_other_directory_link_does_not_exist()
        {
            var result = Program.CheckLocalLink(markdownDocToTestPath, markdownDocToTest,
                otherDirectoryDoesNotExistLink,
                corpus);

            Assert.IsFalse(result);
        }

        [Test]
        public void CheckLocalLink_should_return_true_if_other_file_heading_exists()
        {
            var result = Program.CheckLocalLink(markdownDocToTestPath, markdownDocToTest, otherFileHeadingExistsLink,
                corpus);

            Assert.IsTrue(result);
        }

        [Test]
        public void CheckLocalLink_should_return_false_if_other_file_heading_does_not_exist()
        {
            var result = Program.CheckLocalLink(markdownDocToTestPath, markdownDocToTest,
                otherFileHeadingDoesNotExistLink,
                corpus);

            Assert.IsFalse(result);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Environment.CurrentDirectory = currentDirectory;
            Directory.Delete(tempDirectory, true);
        }
    }
}
