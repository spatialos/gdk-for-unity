using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
                Console.WriteLine("Make sure to run this linter in the root of the project you wish to lint.");
                Console.WriteLine("Example Usage:");
                Console.WriteLine("   DocsLinter.exe");
                Environment.Exit(0);
            }

            try
            {
                var allFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.md", SearchOption.AllDirectories);
                var markdownFiles = GetMarkdownFiles(allFiles);
                var areLinksValid = true;
                foreach (var markdownFilePair in markdownFiles)
                {
                    Console.WriteLine($"Checking links for: {markdownFilePair.Key}");
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
                if (link is LocalLink localLink)
                {
                    allLinksValid &= CheckLocalLink(markdownFilePath, markdownFileContents, localLink, markdownFiles);
                }
                else if (link is RemoteLink remoteLink)
                {
                    allLinksValid &= CheckRemoteLink(markdownFilePath, remoteLink);
                }
            }

            return allLinksValid;
        }

        private static bool CheckLocalLink(string markdownFilePath, SimplifiedMarkdownDoc markdownFileContents,
            LocalLink localLink,
            Dictionary<string, SimplifiedMarkdownDoc> markdownFiles)
        {
            // This link is to something in the same file.
            if (localLink.FilePath == null)
            {
                // Must have a markdown heading! E.g. - "#documentation"
                if (!localLink.Heading.HasValue)
                {
                    LogInvalidLink(markdownFilePath, localLink);
                    return false;
                }

                // Check that the heading is valid.
                if (!markdownFileContents.Headings.Contains(localLink.Heading.Value))
                {
                    LogInvalidLink(markdownFilePath, localLink);
                    return false;
                }
            }
            // This link is to another file.
            else
            {
                var fullyQualifiedFilePath =
                    Path.GetFullPath(Path.Combine(Path.GetDirectoryName(markdownFilePath), localLink.FilePath));

                // The other file is a markdown file and has a heading.
                if (localLink.Heading.HasValue)
                {
                    // Ensure that the other markdown files exists.
                    SimplifiedMarkdownDoc otherMarkdownDoc;
                    if (!markdownFiles.TryGetValue(fullyQualifiedFilePath,
                        out otherMarkdownDoc))
                    {
                        LogInvalidLink(markdownFilePath, localLink);
                        return false;
                    }

                    // Check that the heading is valid.
                    if (!otherMarkdownDoc.Headings.Contains(localLink.Heading.Value))
                    {
                        LogInvalidLink(markdownFilePath, localLink);
                        return false;
                    }
                }
                else
                {
                    // May be a markdown file (without a heading) or non-markdown file. E.g. - "../README.md" or "../../spatialos.json".
                    if (!markdownFiles.ContainsKey(fullyQualifiedFilePath) &&
                        !File.Exists(fullyQualifiedFilePath))
                    {
                        LogInvalidLink(markdownFilePath, localLink);
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool CheckRemoteLink(string markdownFilePath, RemoteLink remoteLink)
        {
            try
            {
                var request = WebRequest.CreateHttp(remoteLink.Url);
                request.Method = "GET";
                var response = request.GetResponse() as HttpWebResponse;
                response.Close();

                // Check for non-200 error codes.
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    LogLinkWarning(markdownFilePath, remoteLink, $"returned a status code of: {(int)response.StatusCode}");
                }

                return true;
            }
            catch (WebException ex)
            {
                // There was an error code. Check if it was a 404.
                // Any other 4xx errors are considered "okay" for now.
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var statusCode = ((HttpWebResponse) ex.Response).StatusCode;
                    if (statusCode == HttpStatusCode.NotFound)
                    {
                        LogInvalidLink(markdownFilePath, remoteLink);
                        return false;
                    }

                    LogLinkWarning(markdownFilePath, remoteLink, $"returned a status code of: {(int)statusCode}");
                    return true;
                }

                Console.Error.WriteLine(ex.ToString());
                if (ex.InnerException != null)
                {
                    Console.Error.WriteLine(ex.InnerException.ToString());
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                if (ex.InnerException != null)
                {
                    Console.Error.WriteLine(ex.InnerException.ToString());
                }

                return false;
            }
        }

        private static void LogInvalidLink(string markdownFilePath, LinkBase link)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error in {markdownFilePath}. The link: {link} is invalid");
            Console.ResetColor();
        }

        private static void LogLinkWarning(string markdownFilePath, LinkBase link, string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Warning in {markdownFilePath}. The link {link} {message}");
            Console.ResetColor();
        }

        private static Dictionary<string, SimplifiedMarkdownDoc> GetMarkdownFiles(string[] allMarkdownFiles)
        {
            var markdownFiles = new Dictionary<string, SimplifiedMarkdownDoc>();

            foreach (var filePath in allMarkdownFiles)
            {
                var markdownDoc = Markdown.Parse(File.ReadAllText(filePath));
                markdownFiles.Add(Path.GetFullPath(filePath),
                    new SimplifiedMarkdownDoc(markdownDoc));
            }

            return markdownFiles;
        }
    }
}
