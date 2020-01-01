using CryptoShark.Hunting.VolumeHunting;
using Quantum.Framework.GenericProperties.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoShark.Hunting.Data
{
    class HuntingTypeManager : Singleton<HuntingTypeManager>
    {
        public List<HuntingType> HuntingTypes { get; set; }

        public HuntingTypeManager()
        {
            HuntingTypes = new List<HuntingType>()
            {
                new VolumeHuntingType()
            };
        }

        public HuntingType GetHuntingType(string typeName)
        {
            foreach (var huntingServiceType in HuntingTypes)
            {
                if (huntingServiceType.TypeName == typeName)
                    return huntingServiceType;
            }

            return null;
        }
    }
}
