namespace FOKE.Entity.DashBoard
{
    public class DashBoardData
    {
        public List<SunburstNode> ZoneData { get; set; }
        public List<DashBoardViewModel> AreaData { get; set; }
        public List<SunburstNode> UnitData { get; set; }
        public List<DistrictGenderData> DistrictData { get; set; }
        public List<MemberData> BloodGroupData { get; set; }
        public List<DepartmentGenderData> DepartmentData { get; set; }
        public List<MemberData> GenderData { get; set; }
        public List<MemberData> WorkPlaceData { get; set; }
        public List<MemberData> ProffesionData { get; set; }
        public List<MemberData> OrgMemberData { get; set; }
        public List<SunburstNode> AgewiseCategory { get; set; }
        public List<SunburstNode> ExpwiseCategory { get; set; }

    }
}
