using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Markdig;

namespace DocsLinter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Contains("--help") || args.Contains("/?") || args.Contains("/help"))
            {
                Console.WriteLine(
                    "A cross-platform helper that lints markdown documents in the current working directory.");
                Console.WriteLine("This linter currently supports checking local links and images.");
                Environment.Exit(0);
            }

            try
            {
                var allFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.*", SearchOption.AllDirectories);
                var markdownFiles = GetMarkdownFiles(allFiles);
                var areLinksValid = true;
                foreach (var markdownFilePair in markdownFiles)
                {
                    areLinksValid &=
                        CheckMarkdownFile(markdownFilePair.Key, markdownFilePair.Value, markdownFiles);
                }

                Environment.Exit(areLinksValid ? 0 : 1);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                if (ex.InnerException != null)
                {
                    Console.Error.WriteLine(ex.InnerException.ToString());
                }

                Environment.Exit(1);
            }
        }

        private static bool CheckMarkdownFile(string markdownFilePath, SimplifiedMarkdownDoc markdownFileContents,
            Dictionary<string, SimplifiedMarkdownDoc> markdownFiles)
        {
            var allLinksValid = true;
            foreach (var link in markdownFileContents.Links)
            {
                if (link.LocalLink.HasValue)
                {
                    var localLink = link.LocalLink.Value;
                    if (localLink.FilePath == null)
                    {
                        // Same file reference. Must have a heading! E.g. - "#documentation"
                        if (!localLink.Heading.HasValue)
                        {
                            LogMissingLink(markdownFilePath, localLink);
                            continue;
                        }

                        if (!markdownFileContents.Headings.Contains(localLink.Heading.Value))
                        {
                            allLinksValid = false;
                            LogMissingLink(markdownFilePath, localLink);
                        }
                    }
                    else
                    {
                        // Other file reference.
                        var fullyQualifiedFilePath =
                            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(markdownFilePath), localLink.FilePath));

                        if (localLink.Heading.HasValue)
                        {
                            // Other markdown file reference. Must have a heading. E.g - "../README.md" or "../README.md#documentation"
                            SimplifiedMarkdownDoc otherMarkdownDoc;
                            if (!markdownFiles.TryGetValue(fullyQualifiedFilePath,
                                out otherMarkdownDoc))
                            {
                                allLinksValid = false;
                                LogMissingLink(markdownFilePath, localLink);
                                continue;
                            }

                            if (!otherMarkdownDoc.Headings.Contains(localLink.Heading.Value))
                            {
                                allLinksValid = false;
                                LogMissingLink(markdownFilePath, localLink);
                            }
                        }
                        else
                        {
                            // Other file reference. May be a markdown file or another file. E.g. - "../README.md" or "../../spatialos.json".
                            if (!markdownFiles.ContainsKey(fullyQualifiedFilePath) &&
                                !File.Exists(fullyQualifiedFilePath))
                            {
                                allLinksValid = false;
                                LogMissingLink(markdownFilePath, localLink);
                            }
                        }
                    }
                }
            }

            return allLinksValid;
        }

        private static void LogMissingLink(string markdownFilePath, Link.LocalReference localLink)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error in {markdownFilePath}. The link: {localLink} is invalid");
            Console.ResetColor();
        }

        private static Dictionary<string, SimplifiedMarkdownDoc> GetMarkdownFiles(string[] allFiles)
        {
            var markdownFiles = new Dictionary<string, SimplifiedMarkdownDoc>();

            foreach (var filePath in allFiles)
            {
                if (Path.GetExtension(filePath) == ".md")
                {
                    var markdownDoc = Markdown.Parse(File.ReadAllText(filePath));
                    markdownFiles.Add(Path.GetFullPath(filePath),
                        new SimplifiedMarkdownDoc(markdownDoc));
                }
            }

            return markdownFiles;
        }
    }
}
