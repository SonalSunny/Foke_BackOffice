using FirebaseAdmin.Messaging;
using FOKE.Entity;
using FOKE.Entity.API.APIModel.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IfireBaseNotificationService
    {
        Task<BatchResponse> SendPushToMultipleAsync(string title, string body, List<string> fcmTokens);
        Task<ResponseEntity<NotificationByMemberDataList>> GetNotificationByMember(string CivilID, long DeviceId);
        Task<ResponseEntity<bool>> UpdateMessageStatus(long MessageId);
        Task<ResponseEntity<bool>> LogOut(long DeviceId);
        Task<ResponseEntity<bool>> SetNotificationContentByMembersAsync();

    }
}
