using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Company.NimrodVS.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal sealed class NimLocDisplayNameAttribute : DisplayNameAttribute
    {
        private string name;
        public NimLocDisplayNameAttribute(string name)
        {
            this.name = name;
        }
        public override string DisplayName
        {
            get
            {
                string result = NimrodResources.GetString(this.name);
                if (result == null)
                {
                    result = this.name;
                }
                return result;
            }
        }
    }
}
