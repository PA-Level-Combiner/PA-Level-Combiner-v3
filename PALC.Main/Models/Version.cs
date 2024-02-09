namespace PALC.Main.Models
{
    public class Version(string version_str, Branch branch)
    {
        public string version_str = version_str;
        public Branch branch = branch;
    }
}
