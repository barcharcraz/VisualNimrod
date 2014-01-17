using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Company.NimrodVS.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    internal sealed class NimDescriptionAttribute : DescriptionAttribute
    {
        private bool replaced;
        public NimDescriptionAttribute(string description)
            : base(description)
        {

        }
        public override string Description
        {
            get
            {
                if (!replaced)
                {
                    replaced = true;
                    DescriptionValue = NimrodResources.GetString(base.Description);
                }
                return base.Description;
            }
        }
    }
}
