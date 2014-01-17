using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Company.NimrodVS.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    internal sealed class NimCategoryAttribute : CategoryAttribute
    {
        public NimCategoryAttribute(string name)
            : base(name)
        {
        }

        protected override string GetLocalizedString(string value)
        {
            return NimrodResources.GetString(value);
        }
    }
}
