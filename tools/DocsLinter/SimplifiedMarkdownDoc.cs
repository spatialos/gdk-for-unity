using System.Collections.Generic;
using System.Text.RegularExpressions;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace DocsLinter
{
    public class SimplifiedMarkdownDoc
    {
        public List<Link> Links = new List<Link>();
        public List<Heading> Headings = new List<Heading>();

        public SimplifiedMarkdownDoc(MarkdownDocument markdownDoc)
        {
            foreach (var child in markdownDoc.Descendants())
            {
                if (child is LinkInline link)
                {
                    Links.Add(new Link(link));
                }
                else if (child is HeadingBlock heading)
                {
                    Headings.Add(new Heading(heading));
                }
            }
        }
    }

    public class Link
    {
        public RemoteUrl? RemoteLink;
        public LocalReference? LocalLink;

        public Link(LinkInline linkInline)
        {
            if (linkInline.Url.StartsWith("http") || linkInline.Url.StartsWith("www"))
            {
                RemoteLink = new RemoteUrl
                {
                    Url = linkInline.Url,
                };
            }
            else
            {
                LocalLink = new LocalReference(linkInline.Url);
            }
        }

        public override string ToString()
        {
            if (RemoteLink.HasValue)
            {
                return RemoteLink.Value.Url;
            }

            if (LocalLink.HasValue)
            {
                return LocalLink.Value.ToString();
            }

            return "Empty link!";
        }

        public struct RemoteUrl
        {
            public string Url;
        }

        public struct LocalReference
        {
            public string FilePath;
            public Heading? Heading;

            public LocalReference(string path)
            {
                if (!path.Contains("#"))
                {
                    FilePath = path;
                    Heading = null;
                }
                else
                {
                    if (path.StartsWith("#"))
                    {
                        FilePath = null;
                        Heading = new Heading(path);
                    }
                    else
                    {
                        FilePath = path.Split('#')[0];
                        Heading = new Heading(path.Remove(0, FilePath.Length));
                    }
                }
            }

            public override string ToString()
            {
                return $"{FilePath ?? ""}{Heading?.ToString() ?? ""}";
            }
        }
    }

    public struct Heading
    {
        private static Regex sanitizationRegex = new Regex("[^a-zA-Z -]");

        public string Title;

        public Heading(HeadingBlock headingBlock)
        {
            Title = "";
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
            Title = "";
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
            var sanitized = sanitizationRegex.Replace(Title.ToLower(), "");
            Title = sanitized.Trim().Replace(" ", "-");
        }
    }
}
