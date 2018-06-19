using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace DocsLinter
{
    public class SimplifiedMarkdownDoc
    {
        public List<LinkBase> Links = new List<LinkBase>();
        public List<Heading> Headings = new List<Heading>();

        public SimplifiedMarkdownDoc()
        {
        }

        public SimplifiedMarkdownDoc(MarkdownDocument markdownDoc)
        {
            Links.AddRange(markdownDoc.Descendants().OfType<LinkInline>().Select(ParseLink));
            Headings.AddRange(markdownDoc.Descendants().OfType<HeadingBlock>().Select(heading => new Heading(heading)));
        }

        public static LinkBase ParseLink(LinkInline linkInline)
        {
            if (linkInline.Url.StartsWith("http") || linkInline.Url.StartsWith("www"))
            {
                return new RemoteLink(linkInline);
            }

            return new LocalLink(linkInline);
        }
    }

    public abstract class LinkBase
    {
    }

    public class RemoteLink : LinkBase
    {
        public string Url;

        public RemoteLink(LinkInline link)
        {
            Url = link.Url;
        }

        public override string ToString()
        {
            return Url;
        }
    }

    public class LocalLink : LinkBase
    {
        public Heading? Heading;
        public string FilePath;

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

    public struct Heading
    {
        private static readonly Regex sanitizationRegex = new Regex("[^a-zA-Z -]");

        public string Title;

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

        private void SanitizeTitle()
        {
            var sanitized = sanitizationRegex.Replace(Title.ToLower(), string.Empty);
            Title = sanitized.Trim().Replace(" ", "-");
        }
    }
}
