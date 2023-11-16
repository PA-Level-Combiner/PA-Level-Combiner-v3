using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA_Level_Combiner.structs
{
    class Version
    {
        public string version_str;
        public Branch branch;

        public Version(string version_str, Branch branch)
        {
            this.version_str = version_str;
            this.branch = branch;
        }
    }
}
