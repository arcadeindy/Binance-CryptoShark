using Quantum.Framework.GenericProperties.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoShark.Hunting.Data
{
    public class HuntingType
    {
        public virtual string TypeName { get; }
        public virtual string DisplayName { get; }

        public virtual Hunting CreateInstance()
        {
            return null;
        }

        public virtual GenericPropertyCollection GetProperties()
        {
            return new GenericPropertyCollection();
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
