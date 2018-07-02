using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace DocsLinter
{
    /// <summary>
    ///     Simple data class that contains information related to a Markdown doc that we require for linting.
    /// </summary>
    public class SimplifiedMarkdownDoc
    {
        public List<ILink> Links = new List<ILink>();
        public List<Heading> Headings = new List<Heading>();

        public SimplifiedMarkdownDoc()
        {
        }

        /// <summary>
        ///     Constructor for automatically parsing a MarkdownDocument object from Markdig.
        /// </summary>
        /// <param name="markdownDoc"></param>
        public SimplifiedMarkdownDoc(MarkdownDocument markdownDoc)
        {
            Links.AddRange(markdownDoc.Descendants().OfType<LinkInline>().Select(ParseLink));
            Headings.AddRange(markdownDoc.Descendants().OfType<HeadingBlock>().Select(heading => new Heading(heading)));
        }

        /// <summary>
        ///     A helper function that parses the Markdig link object and returns the correct type of link - remote or local.
        /// </summary>
        /// <param name="linkInline">The object from Markdig to parse.</param>
        /// <returns></returns>
        public static ILink ParseLink(LinkInline linkInline)
        {
            if (linkInline.Url.StartsWith("http") || linkInline.Url.StartsWith("www"))
            {
                return new RemoteLink(linkInline);
            }

            return new LocalLink(linkInline);
        }
    }

    /// <summary>
    ///     Interface for a link.
    /// </summary>
    public interface ILink
    {
    }

    /// <summary>
    ///     A data class that represents a remote link. I.e. - "https://www.google.com".
    /// </summary>
    public struct RemoteLink : ILink
    {
        public string Url;

        /// <summary>
        ///     Constructor for that parses the Markdig link object.
        /// </summary>
        /// <param name="link">The Markdig link object.</param>
        public RemoteLink(LinkInline link)
        {
            Url = link.Url;
        }

        public override string ToString()
        {
            return Url;
        }
    }

    /// <summary>
    ///     A data class that represents a local link. I.e. - "../README.md#heading"
    /// </summary>
    public struct LocalLink : ILink
    {
        public Heading? Heading;
        public string FilePath;

        /// <summary>
        ///     Constructor for that parses the Markdig link object.
        /// </summary>
        /// <param name="link">The Markdig link object.</param>
        public LocalLink(LinkInline link)
        {
            if (!link.Url.Contains("#"))
            {
                FilePath = link.Url;
                Heading = null;
            }
            else if (link.Url.StartsWith("#"))
            {
                FilePath = null;
                Heading = new Heading(link.Url);
            }
            else
            {
                FilePath = link.Url.Split('#')[0];
                Heading = new Heading(link.Url.Remove(0, FilePath.Length));
            }
        }

        public override string ToString()
        {
            return $"{FilePath ?? string.Empty}{Heading?.ToString() ?? string.Empty}";
        }
    }

    /// <summary>
    ///     A  data struct that represents a Markdown heading.
    /// </summary>
    public struct Heading
    {
        private static readonly Regex sanitizationRegex = new Regex("[^a-zA-Z -]");

        public string Title;

        /// <summary>
        ///     Constructor for parsing the Markdig heading object.
        /// </summary>
        /// <param name="headingBlock">The Markdig heading object. Note that this is an AST object.</param>
        public Heading(HeadingBlock headingBlock)
        {
            Title = string.Empty;
            for (var inline = headingBlock.Inline.FirstChild; inline != null; inline = inline.NextSibling)
            {
                if (inline is CodeInline codeInline)
                {
                    Title += codeInline.Content;
                }
                else
                {
                    Title += inline.ToString();
                }
            }

            SanitizeTitle();
        }

        /// <summary>
        ///     Constructor for parsing a heading from a plain string.
        /// </summary>
        /// <param name="heading">The heading string</param>
        public Heading(string heading)
        {
            Title = string.Empty;
            for (var i = 0; i < heading.Length; i++)
            {
                if (heading[i] == '#')
                {
                    continue;
                }

                Title = heading.Substring(i, heading.Length - i);
                SanitizeTitle();
                break;
            }
        }

        public override string ToString()
        {
            return $"#{Title}";
        }

        /// <summary>
        ///     Helper method to sanitize a Markdown header. Markdown converts the title into a link.
        ///     For example:
        ///     if the Markdown header title is : "### My Awesome Documentation!",
        ///     the corresponding link will be "#my-awesome-documentation"
        /// </summary>
        private void SanitizeTitle()
        {
            var sanitized = sanitizationRegex.Replace(Title.ToLower(), string.Empty);
            Title = sanitized.Trim().Replace(" ", "-");
        }
    }
}
