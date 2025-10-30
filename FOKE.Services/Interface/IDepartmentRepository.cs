using FOKE.Entity;
using FOKE.Entity.DepartmentMaster.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IDepartmentRepository
    {
        Task<ResponseEntity<DepartmentViewModel>> AddDepartment(DepartmentViewModel model);
        Task<ResponseEntity<DepartmentViewModel>> UpdateDepartment(DepartmentViewModel model);
        ResponseEntity<DepartmentViewModel> GetDepartmentbyId(long deptId);
        ResponseEntity<List<DepartmentViewModel>> GetAllDepartments(long? Status, string? dept);
        ResponseEntity<bool> DeleteDepartment(DepartmentViewModel objModel);
    }
}
