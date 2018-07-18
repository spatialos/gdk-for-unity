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
        public const string GithubUrlEnvKey = "GITHUB_URL";

        private static void Main(string[] args)
        {
            if (args.Contains("--help") || args.Contains("/?") || args.Contains("/help"))
            {
                Console.WriteLine(
                    "A cross-platform helper that lints markdown documents in the current working directory.");
                Console.WriteLine("This linter expects there to be a .env file in the current working directory.");
                Console.WriteLine("This linter currently supports checking local links and images.");
                Console.WriteLine("Make sure to run this linter in the root of the project you wish to lint.");
                Console.WriteLine("Example Usage:");
                Console.WriteLine("   DocsLinter.exe");
                Environment.Exit(0);
            }

            try
            {
                var dotenv = DotenvLoader.LoadDotenvFile();

                if (!dotenv.ContainsKey(GithubUrlEnvKey))
                {
                    Console.WriteLine($"Environment variable {GithubUrlEnvKey} not found in .env file.");
                    Environment.Exit(1);
                }

                var allFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.md", SearchOption.AllDirectories);
                var markdownFiles = GetMarkdownFiles(allFiles);
                var areLinksValid = true;
                foreach (var markdownFilePair in markdownFiles)
                {
                    Console.WriteLine($"Checking links for: {markdownFilePair.Key}");
                    areLinksValid &=
                        CheckMarkdownFile(markdownFilePair.Key, markdownFilePair.Value, markdownFiles, dotenv);
                }

                Environment.Exit(areLinksValid ? 0 : 1);
            }
            catch (Exception ex)
            {
                LogException(ex);
                Environment.Exit(1);
            }
        }

        /// <summary>
        ///     A helper method that checks all the links in a markdown file and returns a success/fail.
        ///     Side effects: Prints to the console.
        /// </summary>
        /// <param name="markdownFilePath">The fully qualified path of the Markdown file to check</param>
        /// <param name="markdownFileContents">An object representing the Markdown file to check.</param>
        /// <param name="markdownFiles">
        ///     The corpus of Markdown files undergoing linting. Maps filename to Markdown file object
        ///     representation.
        /// </param>
        /// <param name="dotenv">A dictionary representing the dotenv contents.</param>
        /// <returns>A bool indicating the success of the check.</returns>
        internal static bool CheckMarkdownFile(string markdownFilePath, SimplifiedMarkdownDoc markdownFileContents,
            Dictionary<string, SimplifiedMarkdownDoc> markdownFiles, Dictionary<string, string> dotenv)
        {
            var allLinksValid = true;

            foreach (var localLink in markdownFileContents.Links.OfType<LocalLink>())
            {
                allLinksValid &= CheckLocalLink(markdownFilePath, markdownFileContents, localLink, markdownFiles);
            }

            foreach (var remoteLink in markdownFileContents.Links.OfType<RemoteLink>())
            {
                allLinksValid &= CheckRemoteLink(markdownFilePath, remoteLink, dotenv);
            }

            return allLinksValid;
        }

        /// <summary>
        ///     A helper function that checks the validity of a single local link.
        ///     Side effects: Prints to the console.
        /// </summary>
        /// <param name="markdownFilePath">The fully qualified path of the Markdown file to check</param>
        /// <param name="markdownFileContents">An object representing the Markdown file to check.</param>
        /// <param name="localLink">The object representing the local link to check.</param>
        /// <param name="markdownFiles">
        ///     The corpus of Markdown files undergoing linting. Maps filename to Markdown file object
        ///     representation.
        /// </param>
        /// <returns>A bool indicating the success of the check.</returns>
        internal static bool CheckLocalLink(string markdownFilePath, SimplifiedMarkdownDoc markdownFileContents,
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
                    // May be a markdown file (without a heading) or non-markdown file or a directory. E.g. - "../README.md" or "../../spatialos.json".
                    if (!markdownFiles.ContainsKey(fullyQualifiedFilePath) &&
                        !File.Exists(fullyQualifiedFilePath) && !Directory.Exists(fullyQualifiedFilePath))
                    {
                        LogInvalidLink(markdownFilePath, localLink);
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///     A helper function that checks the validity of a single remote link.
        ///     Side effects: Prints to the console.
        /// </summary>
        /// <param name="markdownFilePath">The fully qualified path of the Markdown file to check</param>
        /// <param name="remoteLink">The object representing the remote link to check.</param>
        /// <param name="dotenv">A dictionary representing the dotenv contents.</param>
        /// <returns>A bool indicating success/failure</returns>
        internal static bool CheckRemoteLink(string markdownFilePath, RemoteLink remoteLink,
            Dictionary<string, string> dotenv)
        {
            // First check if its linking to something in our repository.
            // Note that github URLs are case-insensitive

            string githubUrl;
            if (!dotenv.TryGetValue("GITHUB_URL", out githubUrl))
            {
                Console.WriteLine("No dotenv file found. Aborting linting.");
                Environment.Exit(1);
            }

            var githubRepoBlobPath = (githubUrl + "/blob/").ToLower();
            var githubRepoTreePath = (githubUrl + "/tree/").ToLower();

            var lowerUrl = remoteLink.Url.ToLower();
            if (lowerUrl.Contains(githubRepoBlobPath) || lowerUrl.Contains(githubRepoTreePath))
            {
                LogInvalidLink(markdownFilePath, remoteLink,
                    "Remote link to repository detected. Use a relative path instead. For example, https://www.github.com/spatialos/UnityGDK/blob/master/README.md#docs referenced from docs/my-docs.md should be ../README.md#docs");
                return false;
            }


            // Necessary to be in scope in finally block.
            HttpWebResponse response = null;

            try
            {
                var strippedUrl = remoteLink.Url;
                // anchors break the link check, need to remove them from the link before creating the web request.
                if (strippedUrl.Contains("#"))
                {
                    strippedUrl = remoteLink.Url.Substring(0, strippedUrl.IndexOf("#", StringComparison.Ordinal));
                }

                var request = WebRequest.CreateHttp(strippedUrl);
                request.Method = WebRequestMethods.Http.Get;
                request.AllowAutoRedirect = true;
                response = request.GetResponse() as HttpWebResponse;

                // Check for non-200 error codes.
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    LogLinkWarning(markdownFilePath, remoteLink,
                        $"returned a status code of: {(int) response.StatusCode}");
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

                    LogLinkWarning(markdownFilePath, remoteLink, $"returned a status code of: {(int) statusCode}");
                    return true;
                }

                LogInvalidLink(markdownFilePath, remoteLink,
                    "An exception occured when trying to access this remote link.");
                LogException(ex);
                return false;
            }
            catch (Exception ex)
            {
                LogInvalidLink(markdownFilePath, remoteLink,
                    "An exception occured when trying to access this remote link.");
                LogException(ex);
                return false;
            }
            finally
            {
                response?.Close();
            }
        }

        /// <summary>
        ///     A helper function to print a error when an invalid link is found.
        /// </summary>
        /// <param name="markdownFilePath">The path of the markdown file the error was found in.</param>
        /// <param name="link">The link that is invalid.</param>
        private static void LogInvalidLink(string markdownFilePath, ILink link, string message = "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error in {markdownFilePath}. The link: {link} is invalid. {message}");
            Console.ResetColor();
        }

        /// <summary>
        ///     A helper function to print a warning about a specific link.
        /// </summary>
        /// <param name="markdownFilePath">The path of the markdown file the error was found in.</param>
        /// <param name="link">The link that the warning is about.</param>
        /// <param name="message">The warning message to print.</param>
        private static void LogLinkWarning(string markdownFilePath, ILink link, string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Warning in {markdownFilePath}. The link {link} {message}");
            Console.ResetColor();
        }

        /// <summary>
        ///     A helper function to log exceptions when they are caught.
        /// </summary>
        /// <param name="ex">The exception that was caught</param>
        private static void LogException(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(ex.ToString());
            if (ex.InnerException != null)
            {
                Console.Error.WriteLine(ex.InnerException.ToString());
            }

            Console.ResetColor();
        }

        /// <summary>
        ///     A helper function the collects all Markdown files from a list of file paths.
        /// </summary>
        /// <param name="allMarkdownFiles">A list of file paths.</param>
        /// <returns>A dictionary mapping a Markdown file path to the object representing that Markdown file.</returns>
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
