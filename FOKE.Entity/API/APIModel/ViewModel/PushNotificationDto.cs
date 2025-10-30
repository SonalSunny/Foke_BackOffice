namespace FOKE.Entity.API.APIModel.ViewModel
{
    public class PushNotificationDto
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public List<string> FcmToken { get; set; }
    }
}
