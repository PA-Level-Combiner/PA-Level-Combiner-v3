using System.Collections.Generic;

namespace PALC.Models
{
    public abstract class Combiner<TLevelContainer, TLevelData>
    {
        public abstract Version GetPAVersion();
        public abstract TLevelContainer Combine(List<TLevelData> levels);
    }
}
