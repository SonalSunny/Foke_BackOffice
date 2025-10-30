namespace FOKE.Entity.Identity.ViewModel
{
    public class UserLoginLogViewModel
    {

        public long? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public DateTime? LoginDateTime { get; set; }
        public string SessionId { get; set; }
        public DateTime? LogoutDateTime { get; set; }
    }
}
