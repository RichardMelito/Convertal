using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public interface IBase
    {

    }

    public abstract class BaseObject : Named
    {
        protected BaseObject(string name, string nameSpace)
            : base(name, nameSpace)
        {

        }
    }
}
