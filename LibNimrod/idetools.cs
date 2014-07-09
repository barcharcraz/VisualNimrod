using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
namespace NimrodSharp
{
    public enum symTypes
    {
        skConst,
        skEnumField,
        skForVar,
        skIterator,
        skLabel,
        skLet,
        skMacro,
        skMethod,
        skParam,
        skProc,
        skResult,
        skTemplate,
        skType,
        skVar,
        none
    }
    public enum resultTypes
    {
        def,
        sug,
        unsupported
    }
    public class idetoolsReply
    {
        public resultTypes replyType;
        public string verbatimOutput;
        public symTypes type;
        public string path;
        public string typeSig;
        public string filePath;
        public int line;
        public int col;
        public string docstring;
    }
    public class idetools
    {
        private Process nimrodProc;
        public idetools()
        {
            nimrodProc = Process.Start(idetoolsfuncs.CreateStartInfo());
        }
    }
    public static class idetoolsfuncs
    {
        public static ProcessStartInfo CreateStartInfo()
        {
            ProcessStartInfo rv = new ProcessStartInfo("nimrod");
            rv.Arguments = "serve --server.type:stdin";
            rv.CreateNoWindow = true;
            rv.UseShellExecute = false;
            rv.RedirectStandardError = true;
            rv.RedirectStandardInput = true;
            rv.RedirectStandardOutput = true;
            return rv;
        }
        public static ProcessStartInfo CreateStartInfo(string args)
        {
            var info = CreateStartInfo();
            info.Arguments = args;
            return info;
        }
        public static idetoolsReply ParseReply(string def)
        {
            var rv = new idetoolsReply();
            rv.verbatimOutput = def;
            string[] cols = def.Split(new char[]{'\t'});
            bool result = Enum.TryParse<resultTypes>(cols[0], out rv.replyType);
            if (!result)
            {
                rv.replyType = resultTypes.unsupported;
            }
            result = Enum.TryParse<symTypes>(cols[1], out rv.type);
            if (!result)
            {
                rv.type = symTypes.none;
            }
            rv.path = cols[2];
            rv.typeSig = cols[3];
            rv.filePath = cols[4];
            result = int.TryParse(cols[5], out rv.line);
            if (!result)
            {
                rv.line = -1;
            }
            result = int.TryParse(cols[6], out rv.col);
            if (!result)
            {
                rv.col = -1;
            }
            rv.docstring = cols[7];
            return rv;
        }
        public static List<idetoolsReply> ParseMultipleReply(string reply)
        {
            var rv = new List<idetoolsReply>();
            var lines = reply.Split(new string[]{"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var elm in lines)
            {
                rv.Add(ParseReply(elm));
            }
            return rv;
        }
        public static string GetArgs(string action, string file, int line, int col, string project)
        {
            string fileRelitive = file.Substring(Path.GetDirectoryName(project).Length + 1);
            string rv = "--verbosity:0 idetools --track:" + fileRelitive + "," + (line + 1).ToString() + "," + col.ToString() + " --" + action + " " + Path.GetFileName(project);
            return rv;
        }
        public static string GetDirtyArgs(string action, string dirty_file, string file, int line, int col, string project)
        {
            string fileRelitive = file.Substring(Path.GetDirectoryName(project).Length + 1);
            string rv = " --verbosity:0 idetools --trackDirty:\"" + dirty_file + "," + fileRelitive + "," + line.ToString() + "," + col.ToString() + "\" --" + action + " " + Path.GetFileName(project);
            return rv;
        }
        public static string GetRawResults(string action, string file, int line, int col, string project)
        {
            var startInfo = CreateStartInfo(GetArgs(action, file, line, col, project));
            startInfo.WorkingDirectory = Path.GetDirectoryName(project);
            var proc = Process.Start(startInfo);
            
            string result = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            return result;
        }
        public static string GetRawDirtyResults(string action, string dirty_file, string file, int line, int col, string project)
        {
            var startInfo = CreateStartInfo(GetDirtyArgs(action, dirty_file, file, line, col, project));
            startInfo.WorkingDirectory = Path.GetDirectoryName(project);
            var proc = Process.Start(startInfo);
            
            string result = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            return result;
        }
        public static List<idetoolsReply> GetReply(string action, string file, int line, int col, string project)
        {
            return ParseMultipleReply(GetRawResults(action, file, line, col, project));
        }
        public static List<idetoolsReply> GetDirtyReply(string action, string dirty_file, string file, int line, int col, string project)
        {
            return ParseMultipleReply(GetRawDirtyResults(action, dirty_file, file, line, col, project));
        }
        public static idetoolsReply GetDef(string file, int line, int col, string project)
        {
            return GetReply("def", file, line, col, project).First();
        }
        public static List<idetoolsReply> GetSuggestions(string file, int line, int col, string project)
        {
            return GetReply("suggest", file, line, col, project);
        }
        public static List<idetoolsReply> GetDirtySuggestions(string dirty_file, string file, int line, int col, string project)
        {
            return GetDirtyReply("suggest", dirty_file, file, line, col, project);
        }
    }
}
