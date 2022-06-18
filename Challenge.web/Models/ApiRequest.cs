using static Challenge.web.SD;

namespace Challenge.web.Models
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; }
        public string Url { get; set; }
        public object Data { get;set; }
        public string AccessToken { get; set; }
        public bool Authorize { get; set; } = true;
        public Dictionary<string, string>? Headers { get; set; }
    }
}
