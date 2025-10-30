using FOKE.Entity;
using FOKE.Entity.ProfessionData.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IProfessionRepository
    {
        Task<ResponseEntity<ProfessionViewModel>> SaveProfession(ProfessionViewModel model);
        Task<ResponseEntity<ProfessionViewModel>> UpdateProfession(ProfessionViewModel model);
        ResponseEntity<bool> DeleteProfession(ProfessionViewModel objModel);
        ResponseEntity<ProfessionViewModel> GetProfessionbyId(long profId);
        ResponseEntity<List<ProfessionViewModel>> GetAllProfessions(long? Status, string professionname);
        ResponseEntity<string> ExportProfessionToExcel(long? Status, string search);

    }
}
