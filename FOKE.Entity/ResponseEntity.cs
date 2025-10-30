using System.Net;

namespace FOKE.Entity
{
    public class ResponseEntity<T>
    {
        public HttpStatusCode transactionStatus { get; set; }
        public string returnMessage { get; set; }
        public long? TotalCount { get; set; }
        public T returnData { get; set; }
        public string status
        {
            get
            {
                return transactionStatus.ToString();
            }
        }

    }
}
