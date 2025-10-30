using FOKE.Entity;
using FOKE.Entity.WorkPlaceData.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IWorkPlaceRepository
    {
        Task<ResponseEntity<WorkPlaceViewModel>> SaveWorkPlace(WorkPlaceViewModel model);
        Task<ResponseEntity<WorkPlaceViewModel>> UpdateWorkPlace(WorkPlaceViewModel model);
        ResponseEntity<bool> DeleteWorkPlace(WorkPlaceViewModel objModel);
        ResponseEntity<WorkPlaceViewModel> GetWorkPlacebyId(long wpId);
        ResponseEntity<List<WorkPlaceViewModel>> GetAllWorkPlace(long? Status, string workplacename);
        ResponseEntity<string> ExportWorkPlaceToExcel(long? Status, string search);

    }
}
