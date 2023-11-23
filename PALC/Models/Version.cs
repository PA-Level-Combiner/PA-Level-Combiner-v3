using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PALC.Models
{
    public class Version(string version_str, Branch branch)
    {
        public string version_str = version_str;
        public Branch branch = branch;
    }
}
