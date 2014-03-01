﻿using System;
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
namespace Company.NimrodVS
{
    public class NimrodAuthoringScope : AuthoringScope
    {
        private string m_filename;
        private string m_dirtyname;
        private string m_projectfile;

        public NimrodAuthoringScope(string filename, string dirtyname, string projectfile) : base()
        {
            m_filename = filename;
            m_dirtyname = dirtyname;
            m_projectfile = projectfile;
        }
        public override string GetDataTipText(int line, int col, out TextSpan span)
        {
            span = new TextSpan();
            return null;
        }

        public override Declarations GetDeclarations(IVsTextView view, int line, int col, TokenInfo info, ParseReason reason)
        {
            string text;
            view.GetTextStream(0, 0, line, col, out text);
            File.WriteAllText(m_dirtyname, text, new UTF8Encoding(false));
            switch (reason)
            {
                case ParseReason.CompleteWord:
                case ParseReason.MemberSelect:
                    var reply = idetoolsfuncs.GetDirtySuggestions(m_dirtyname, m_filename, line, col, m_projectfile);
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
            return null;
        }
    }
}
