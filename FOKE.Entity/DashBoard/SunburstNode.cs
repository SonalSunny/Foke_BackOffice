namespace FOKE.Entity.DashBoard
{
    public class SunburstNode
    {
        public string name { get; set; }
        public long? value { get; set; }
        public List<SunburstNode> children { get; set; } = new();
    }
}
