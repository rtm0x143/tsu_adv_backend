using System.Net;

namespace AdminPanel.ViewModels
{
    public class ErrorViewModel
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
        public string? Message { get; set; }
        public Uri? UrlReferrer { get; set; }
    }
}