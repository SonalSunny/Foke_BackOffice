namespace FOKE.Entity.API.APIModel.ViewModel
{
    public class CommitteByGroupDto
    {
        public string GroupName { get; set; }
        public List<CommitteDto> CommitteMembers { get; set; }
    }
}
