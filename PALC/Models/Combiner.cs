using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PALC.Models
{
    public abstract class Combiner<TLevelData>
    {
        public abstract Version GetPAVersion();
        public abstract TLevelData Combine(List<TLevelData> levels);
    }
}
