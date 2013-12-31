using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.OLE.Interop;

namespace Company.NimrodVS
{
    class NimrodLanguageService : LanguageService
    {
        private LanguagePreferences prefs;

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
            }
            return this.prefs;
        }

        public override IScanner GetScanner(IVsTextLines buffer)
        {
            return new NimrodScanner(buffer);
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
            return new NimrodAuthoringScope();
        }

        public override string GetFormatFilterList()
        {
            return "Nimrod file(*.nim)";
        }
    }
}
