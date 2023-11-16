using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PA_Level_Combiner.structs
{
    abstract class Combiner<TLevelData>
    {
        public readonly Version version;

        public abstract TLevelData Combine(List<TLevelData> levels);
    }
}
