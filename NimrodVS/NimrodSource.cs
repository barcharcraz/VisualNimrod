using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Text;
namespace Company.NimrodVS
{
    public class NimrodSource : Source
    {
        public NimrodSource(LanguageService service, IVsTextLines buffer, Colorizer color) : base(service, buffer, color) { }
        public override void ReformatSpan(EditArray mgr, Microsoft.VisualStudio.TextManager.Interop.TextSpan span)
        {
            base.ReformatSpan(mgr, span);
            IVsTextLines pBuffer = GetTextLines();
            if (pBuffer != null)
            {
                int startIndex = span.iStartIndex;
                int endIndex = 0;
                string line = "";
                int indentLevel = 0;
                pBuffer.GetLengthOfLine(span.iStartLine, out endIndex);
                pBuffer.GetLineText(span.iStartLine, startIndex, span.iEndLine, endIndex, out line);
                int baseIndent = line.Length - line.TrimStart().Length;
                for (int i = span.iStartLine; i <= span.iEndLine; i++)
                {
                    if (i < span.iEndLine)
                    {
                        pBuffer.GetLengthOfLine(i, out endIndex);
                    }
                    else
                    {
                        endIndex = span.iEndIndex;
                    }
                    pBuffer.GetLineText(i, startIndex, i, endIndex, out line);
                    int numLeading = line.Length - line.TrimStart().Length;
                    line = line.Trim();
                    int spaceCount = baseIndent + 2 * indentLevel;
                    StringBuilder replacementText = new StringBuilder();
                    replacementText.Append(' ', spaceCount);
                    TextSpan editTextSpan = new TextSpan();
                    editTextSpan.iStartLine = i;
                    editTextSpan.iEndLine = i;
                    editTextSpan.iStartIndex = 0;
                    editTextSpan.iEndIndex = numLeading;
                    mgr.Add(new EditSpan(editTextSpan, replacementText.ToString()));
                    if (line.Last() == ':')
                    {
                        indentLevel = indentLevel + 1;
                    }
                    startIndex = 0;
                }
            }
        }
    }
}
