using System;

namespace Blog.Web.Models
{
    public class ErrorViewModel
    {

        public string StatusCode { get; set; }
        public string Message { get; set; }
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
