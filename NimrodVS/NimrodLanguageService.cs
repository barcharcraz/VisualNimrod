using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Project.Automation;
using EnvDTE;

namespace Company.NimrodVS
{
    class NimrodLanguageService : LanguageService
    {
        private LanguagePreferences prefs;
        private NimrodScanner m_scanner;
        private string m_dirtyfile;
        public NimrodLanguageService()
            : base()
        {
            m_dirtyfile = Path.GetTempFileName();
        }
        public override Source CreateSource(IVsTextLines buffer)
        {
            return new NimrodSource(this, buffer, GetColorizer(buffer));
        }
        public override LanguagePreferences GetLanguagePreferences()
        {
            if (prefs == null)
            {
                prefs = new LanguagePreferences(this.Site,
                    typeof(NimrodLanguageService).GUID, this.Name);
                
                //seriously MS...
                if (this.prefs != null)
                {
                    this.prefs.Init();
                }
                this.prefs.ParameterInformation = true;
            }
            return this.prefs;
        }

        public override IScanner GetScanner(IVsTextLines buffer)
        {
            if (m_scanner == null)
            {
                m_scanner = new NimrodScanner(buffer);
            }
            return m_scanner;
        }

        public override string Name
        {
            get { return "Nimrod"; }
        }
        public override Colorizer GetColorizer(IVsTextLines buffer)
        {
            return new Colorizer(this, buffer, GetScanner(buffer));
        }
        
        public override AuthoringScope ParseSource(ParseRequest req)
        {
            
            var dte = (DTE)GetService(typeof(DTE));
            
            var props = dte.Solution.FindProjectItem(req.FileName).ContainingProject.Properties as OAProperties;
            var node = props.Node as ManagedNimrodProject.NimrodProjectNode;
            
            string startupObj = Path.Combine(node.ProjectFolder, node.GetProjectProperty("StartupObject"));
            var rv = new NimrodAuthoringScope(req, m_dirtyfile, startupObj);
            if (req.Reason == ParseReason.Check)
            {
                var errors = NimrodSharp.checkfuncs.CheckFile(req.FileName, node.ProjectFolder);
                foreach (var error in errors)
                {
                    TextSpan ctx;
                    ctx.iStartLine = error.row;
                    ctx.iEndLine = error.rowend;
                    ctx.iStartIndex = error.col;
                    ctx.iEndIndex = error.colend;
                    if (error.filePath == req.FileName)
                    {
                        req.Sink.AddError(error.filePath, error.messageString, ctx, (Severity)error.type);
                    }
                }
            }
            if (req.Reason == ParseReason.MemberSelect || 
                req.Reason == ParseReason.MemberSelectAndHighlightBraces ||
                req.Reason == ParseReason.CompleteWord)
            {
                rv.PopulateDeclerations(req.View, req.Line, req.Col);
            }
            if (req.TokenInfo.Type == TokenType.Identifier)
            {
                TextSpan span;
                span.iStartLine = req.Line;
                span.iEndLine = req.Line;
                span.iStartIndex = req.TokenInfo.StartIndex;
                span.iEndIndex = req.TokenInfo.EndIndex;
                req.Sink.StartName(span, req.Text);
            }
            return rv;
        }

        public override string GetFormatFilterList()
        {
            return "Nimrod file(*.nim)";
        }
    }
}
