using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA_Level_Combiner.structs.combiners._20_4_4.exceptions
{
    public class AudioFileNotFoundException(Exception inner) : Exception("The audio file cannot be found or is invalid.", inner)
    {
        public AudioFileNotFoundException() : this(null) { }
    }
}
