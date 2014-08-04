using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NimrodSharp
{
    public enum errTypes
    {
        unknown,
        XDeclaredButNotUsed
    }
    public enum CheckReplyType
    {
        Hint, 
        Warning,
        Error,
        Fatal,
        Unknown
    }
    public class CheckReply
    {
        public CheckReplyType type;
        public errTypes message;
        public string messageString;
        public string filePath;
        public int row;
        public int col;
        public int rowend;
        public int colend;

    }
    public static class checkfuncs
    {
        private static string findIdentifier(string message)
        {
            var firstidx = message.IndexOf('\'');
            var lastidx = message.IndexOf('\'', firstidx + 1);
            var ident = message.Substring(firstidx + 1, lastidx - firstidx - 1);
            var dotidx = ident.IndexOf('.');
            if (dotidx != -1)
            {
                ident = ident.Substring(dotidx);
            }
            return ident;
        }
        private static ProcessStartInfo CreateStartInfo(string filename, string rootDir)
        {
            ProcessStartInfo rv = new ProcessStartInfo("nimrod");
            rv.WorkingDirectory = rootDir;
            rv.Arguments = "--path:" + rootDir + " --verbosity:0 check " + filename;
            rv.WorkingDirectory = Path.GetDirectoryName(filename);
            rv.CreateNoWindow = true;
            rv.UseShellExecute = false;
            rv.RedirectStandardError = true;
            rv.RedirectStandardInput = true;
            rv.RedirectStandardOutput = true;
            return rv;
        }
        private static void addEndInformation(CheckReply reply)
        {
            switch (reply.message)
            {
                case errTypes.unknown:
                    break;
                case errTypes.XDeclaredButNotUsed:
                    var ident = findIdentifier(reply.messageString);
                    reply.colend = reply.colend + ident.Length;
                    break;
                default:
                    break;
            }
        }
        private static CheckReply ParseLine(Match match, string rootDir)
        {
            CheckReply rv = new CheckReply();
            rv.filePath = match.Groups[1].Value;
            if (!Path.IsPathRooted(rv.filePath))
            {
                rv.filePath = Path.Combine(rootDir, rv.filePath);
            }
            rv.row = int.Parse(match.Groups[2].Value) - 1;
            rv.col = int.Parse(match.Groups[3].Value);
            rv.rowend = rv.row;
            rv.colend = rv.col + 1;
            if (match.Groups[4].Value == "Hint")
            {
                rv.type = CheckReplyType.Warning;
            }
            else if (match.Groups[4].Value == "Error")
            {
                rv.type = CheckReplyType.Error;
            }
            else
            {
                rv.type = CheckReplyType.Unknown;
            }
            var result = Enum.TryParse<errTypes>(match.Groups[6].Value, out rv.message);
            if (!result)
            {
                rv.message = errTypes.unknown;
            }
            rv.messageString = match.Groups[5].Value;
            addEndInformation(rv);
            return rv;
        }
        
        private static IEnumerable<CheckReply> ParseReply(string reply, string rootDir)
        {
            var rv = new List<CheckReply>();
            var regex = new Regex(@"(.+\.nim)\((\d+), (\d+)\) (.+?): (.+)\s?(?:\[(.+)\])?");

            var matches = regex.Matches(reply);
            foreach (Match match in matches)
            {
                rv.Add(ParseLine(match, rootDir));
            }
            return rv;
        }
        
        
        public static IEnumerable<CheckReply> CheckFile(string filename, string projectRoot)
        {
            var startInfo = CreateStartInfo(filename, projectRoot);
            startInfo.WorkingDirectory = projectRoot;
            var proc = Process.Start(startInfo);
            string result = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            return ParseReply(result, Path.GetDirectoryName(filename));
        }
    }
}