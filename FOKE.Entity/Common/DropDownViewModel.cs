namespace FOKE.Entity.Common
{
    public class DropDownViewModel
    {
        public long keyID { get; set; }
        public long? keyID1 { get; set; }
        public long? keyID2 { get; set; }
        public long? keyID3 { get; set; }
        public long? keyID4 { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string description1 { get; set; }
        public string description2 { get; set; }
        public string description3 { get; set; }
        public string description4 { get; set; }

        public bool IsSelected { get; set; }
    }
}
