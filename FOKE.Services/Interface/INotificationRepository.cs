using FOKE.Entity;
using FOKE.Entity.Notification.ViewModel;

namespace FOKE.Services.Interface
{
    public interface INotificationRepository
    {
        Task<ResponseEntity<NotificationViewModel>> SaveNotification(NotificationViewModel model);
        ReciepientCountDto AllMemberCount();
        ReciepientCountDto MembersByArea(long? AreaId);
        ReciepientCountDto MembersByZone(long? ZoneId);
        ReciepientCountDto MembersByUnit(long? UnitID);
        ReciepientCountDto AllCommitteeMemberCount();
        ReciepientCountDto AllActiveMemberCount();
        ReciepientCountDto AllInActiveMemberCount();
        ResponseEntity<List<NotificationViewModel>> GetAllNotifications(long? Status);
        ResponseEntity<List<NotificationViewModel>> GetAllNotificationlogs(long? Status);
        ResponseEntity<List<ReciepientData>> GetMemberDetails(string? Keyword, string? SearchText);
    }
}