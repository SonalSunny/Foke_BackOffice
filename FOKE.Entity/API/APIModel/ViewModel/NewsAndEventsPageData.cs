namespace FOKE.Entity.API.APIModel.ViewModel
{
    public class NewsAndEventsPageData
    {
        public List<NewsAndEventsData> NewsOrEventsData { get; set; }
        public bool IsUnread { get; set; }
        public int NextPageNumber { get; set; }
        public int PageCount { get; set; }

    }
}
