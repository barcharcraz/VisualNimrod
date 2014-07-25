using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Package;
using System.Text.RegularExpressions;
using NimrodSharp;
namespace Company.NimrodVS.IntelliSense
{
    public class NimrodMethods : Methods
    {
        private List<idetoolsReply> replies;
        private List<List<string>> types;
        private List<string> returnTypes;
        public NimrodMethods(List<idetoolsReply> replies)
        {
            this.replies = replies;
            this.types = new List<List<string>>();
            for (int i = 0; i < replies.Count; i++)
            {
                var sig = replies[i].typeSig;
                var idx1 = sig.IndexOf('(');
                var idx2 = sig.IndexOf(')');
                var idx3 = sig.IndexOf(':');
                if (idx3 == -1)
                {
                    returnTypes.Add(null);
                }
                else
                {
                    returnTypes.Add(sig.Substring(idx3 + 2));
                }
                sig = sig.Substring(idx1 + 1, idx2 - 1 - idx1);
                types.Add(new List<string>(sig.Split(new string[]{", "}, StringSplitOptions.RemoveEmptyEntries)));

            }
        }
        public override int GetCount()
        {
            return replies.Count;
        }

        public override string GetDescription(int index)
        {
            return replies[index].docstring;
        }

        public override string GetName(int index)
        {
            return replies[index].path.Split('.').Last();
        }

        public override int GetParameterCount(int index)
        {
            return types[index].Count;
        }

        public override void GetParameterInfo(int index, int parameter, out string name, out string display, out string description)
        {
            name = "";
            description = "";
            display = ":" + types[index][parameter];
        }

        public override string GetType(int index)
        {
            return returnTypes[index];
        }
    }
}
