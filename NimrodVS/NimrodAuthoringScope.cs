using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using System.IO;
using NimrodSharp;
using Microsoft.VisualStudio.Project.Automation;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio;
using EnvDTE;
using System.Runtime.InteropServices;
namespace Company.NimrodVS
{
    public class NimrodAuthoringScope : AuthoringScope
    {
        private string m_filename;
        private string m_dirtyname;
        private string m_projectfile;
        private AuthoringSink m_sink;
        public NimrodAuthoringScope(AuthoringSink sink, string filename, string dirtyname, string projectfile) : base()
        {
            m_sink = sink;
            m_filename = filename;
            m_dirtyname = dirtyname;
            m_projectfile = projectfile;
            
        }
        public NimrodAuthoringScope(ParseRequest req, string dirtyFile, string project)
            : base()
        {
            m_sink = req.Sink;
            m_projectfile = project;
            m_dirtyname = dirtyFile;
            m_filename = req.FileName;
        }
        
        public override string GetDataTipText(int line, int col, out TextSpan span)
        {
            span = new TextSpan();
            return null;
        }

        public override Declarations GetDeclarations(IVsTextView view, int line, int col, TokenInfo info, ParseReason reason)
        {
            string text;
            int numLines;
            int lastCol;
            IVsTextLines buf = null;
            var hr = view.GetBuffer(out buf);
            Marshal.ThrowExceptionForHR(hr);
            hr = buf.GetLineCount(out numLines);
            Marshal.ThrowExceptionForHR(hr);
            hr = buf.GetLengthOfLine(numLines - 1, out lastCol);
            Marshal.ThrowExceptionForHR(hr);
            hr = buf.GetLineText(0, 0, numLines - 1, lastCol, out text);
            Marshal.ThrowExceptionForHR(hr);
            File.WriteAllText(m_dirtyname, text, new UTF8Encoding(false));
            switch (reason)
            {
                case ParseReason.CompleteWord:
                case ParseReason.MemberSelect:
                    var reply = idetoolsfuncs.GetDirtySuggestions(m_dirtyname, m_filename, line + 1, col + 1, m_projectfile);
                    return new IntelliSense.NimrodDeclarations(reply);
                default:
                    return null;
            }
        }

        public override Methods GetMethods(int line, int col, string name)
        {
            return null;
        }
        
        public override string Goto(Microsoft.VisualStudio.VSConstants.VSStd97CmdID cmd, IVsTextView textView, int line, int col, out TextSpan span)
        {
            span = new TextSpan();
            switch (cmd)
            {
                case VSConstants.VSStd97CmdID.GotoDefn:
                    var def = idetoolsfuncs.GetDef(m_filename, line, col, m_projectfile);
                    if (def.type == symTypes.none)
                    {
                        break;
                    }
                    else
                    {
                        span = new TextSpan();
                        span.iStartLine = def.line - 1;
                        span.iEndIndex = def.col;
                        span.iStartIndex = 0;
                        span.iEndLine = def.line - 1;
                        return def.filePath;
                    }
                default:
                    break;
            }
            return null;
        }
    }
}
