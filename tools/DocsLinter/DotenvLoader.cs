using System.Collections.Generic;
using System.IO;

namespace DocsLinter
{
  public class DotenvLoader
  {
    public static Dictionary<string, string> LoadDotenvFile()
    {
      var dotenvContents = new Dictionary<string, string>();

      var dotenvFileContents = File.ReadAllLines(".env");

      foreach (var dotenvRow in dotenvFileContents)
      {
        var index = dotenvRow.IndexOf("=");

        if (index < 0)
        {
          continue;
        }

        var key = dotenvRow.Substring(0, index).Trim();
        var value = dotenvRow.Substring(index + 1, dotenvRow.Length - (index + 1)).Trim();

        if (key.Length > 0 && value.Length > 0)
        {
          dotenvContents.Add(key, value);
        }
      }

      return dotenvContents;
    }
  }
}
