using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using NUnit.Framework;

namespace DocsLinter.Tests
{
    [TestFixture]
    public class SimplifiedMarkdownDocTests
    {
        [Test]
        public void ParseLink_should_return_RemoteLink_when_url_starts_with_http()
        {
            var linkInline = new LinkInline("https://www.google.com", "Title");
            var link = SimplifiedMarkdownDoc.ParseLink(linkInline);

            Assert.IsInstanceOf<RemoteLink>(link);
        }

        [Test]
        public void ParseLink_should_return_RemoteLink_when_url_starts_with_www()
        {
            var linkInline = new LinkInline("www.google.com", "Title");
            var link = SimplifiedMarkdownDoc.ParseLink(linkInline);

            Assert.IsInstanceOf<RemoteLink>(link);
        }

        [Test]
        public void ParseLink_should_return_LocalLink_when_not_a_remote_url()
        {
            var linkInline = new LinkInline("../README.md", "Title");
            var link = SimplifiedMarkdownDoc.ParseLink(linkInline);

            Assert.IsInstanceOf<LocalLink>(link);
        }

        [Test]
        public void RemoteLink_should_assign_the_url_correctly()
        {
            var linkInline = new LinkInline("https://www.google.com", "Title");
            var remoteLink = new RemoteLink(linkInline);

            Assert.AreEqual(linkInline.Url, remoteLink.Url);
        }

        [Test]
        public void LocalLink_should_correctly_parse_in_file_reference()
        {
            var linkInline = new LinkInline("#other-heading-in-doc", "Title");
            var localLink = new LocalLink(linkInline);

            Assert.IsTrue(localLink.Heading.HasValue);
            Assert.IsNull(localLink.FilePath);
        }

        [Test]
        public void LocalLink_should_correctly_parse_other_file_reference()
        {
            var linkInline = new LinkInline("../README.md", "Title");
            var localLink = new LocalLink(linkInline);

            Assert.IsFalse(localLink.Heading.HasValue);
            Assert.IsNotNull(localLink.FilePath);

            Assert.AreEqual(linkInline.Url, localLink.FilePath);
        }

        [Test]
        public void LocalLink_should_correctly_parse_other_markdown_file_reference()
        {
            var linkInline = new LinkInline("../README.md#my-heading", "Title");
            var localLink = new LocalLink(linkInline);

            Assert.IsTrue(localLink.Heading.HasValue);
            Assert.IsNotNull(localLink.FilePath);
        }

        [Test]
        public void Heading_should_construct_from_string_correctly()
        {
            var headingString = "###my-heading";
            var heading = new Heading(headingString);

            Assert.AreEqual("my-heading", heading.Title);
        }

        [Test]
        public void Heading_should_construct_from_HeadingBlock_correctly()
        {
            var headingBlock = new HeadingBlock(new ParagraphBlockParser());
            headingBlock.Inline = new ContainerInline();

            var firstContent = new LiteralInline("myheading");
            headingBlock.Inline.AppendChild(firstContent);

            var firstHeading = new Heading(headingBlock);
            Assert.AreEqual("myheading", firstHeading.Title);

            var secondContent = new LiteralInline("-myotherheading");
            headingBlock.Inline.AppendChild(secondContent);

            var secondHeading = new Heading(headingBlock);
            Assert.AreEqual("myheading-myotherheading", secondHeading.Title);

            var codeContent = new CodeInline();
            codeContent.Content = "awesomeclassname";
            headingBlock.Inline.AppendChild(codeContent);

            var codeHeading = new Heading(headingBlock);
            Assert.AreEqual("myheading-myotherheadingawesomeclassname", codeHeading.Title);
        }

        [Test]
        public void Heading_should_sanitize_the_title()
        {
            var headingString = "###1. mY garbled&9123... heaDiNg";
            var heading = new Heading(headingString);

            Assert.AreEqual("my-garbled-heading", heading.Title);
        }
    }
}
