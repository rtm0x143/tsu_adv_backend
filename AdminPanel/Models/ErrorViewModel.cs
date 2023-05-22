using System.Net;

namespace AdminPanel.Models
{
    public class ErrorViewModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public Uri? UrlReferrer { get; set; }
    }
}