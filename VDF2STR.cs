// A VERY primitive and single use-case method of converting a Valve Data File type string to JSON type string.
// NOT recommended for use anywhere else. Made painstaingly by kebin#9844.

using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace VDF2STR
{
    public static class VDFToString
    {
        public static string Convert(string dir)
        {
            string[] allLines = File.ReadAllLines(dir);
            allLines = allLines.Skip(1).ToArray();
            string code = "";
            for (int i = 0; i < allLines.Length; i++) code += allLines[i] + "\n";
            while (code.Contains("\"\t\t")) code = code.Replace("\"\t\t", "\": ");
            while (code.Contains("\"\n")) code = code.Replace("\"\n", "\",\n");
            Regex pattern = new Regex("\t\"[\\d]+\",");
            MatchCollection matches = pattern.Matches(code);
            for (int i = 0; i < matches.Count; i++)
            {
                int indexOfMatch = matches.ElementAt(i).Index;
                int lengthOfMatch = matches.ElementAt(i).Length;
                int nextChar = indexOfMatch + lengthOfMatch - 1;
                code = new StringBuilder(code) { [nextChar] = ':' }.ToString();
            }
            return code;
        }
    }
}