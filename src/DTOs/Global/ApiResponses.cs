using System.ComponentModel;
using System.Text.Json.Serialization;

namespace reichan_api.src.DTOs.Global
{
    public abstract class ApiResponse<T>
    {
        [Description("URI with an instance reference.")]
        [JsonPropertyName("type")]
        public string Type { get; init; }

        [Description("Response status in accord to RFC 7807.")]
        [JsonPropertyName("status")]
        public int Status { get; init; }

        [Description("Title response.")]
        [JsonPropertyName("title")]
        public string Title { get; init; }

        [Description("Detailed response description.")]
        [JsonPropertyName("detail")]
        public string Detail { get; init; }

        [Description("Response instance ID.")]
        [JsonPropertyName("instance")]
        public string Instance { get; init; }

        [Description("Response Data")]
        [JsonPropertyName("data")]
        public T? Data { get; init; }

        protected ApiResponse(int status, string type, string title, string detail, string instance, T? data)
        {
            Status = status;
            Type = type;
            Title = title;
            Detail = detail;
            Instance = instance;
            Data = data;
        }
    }

    public class OkResponse<T> : ApiResponse<T>
    {
        private static readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();

        public OkResponse(string title, string detail, T? data)
            : base(
                  status: StatusCodes.Status200OK,
                  type: "https://datatracker.ietf.org/doc/html/rfc9110#name-200-ok",
                  title: title ?? "Request Successful",
                  detail: detail ?? "Request fetched successfully.",
                  instance: _httpContextAccessor.HttpContext?.Request.Path ?? "/unknown",
                  data: data)
        {
            // Abstract
        }
    }
    public class NoContentResponse : ApiResponse<object>
    {
        private static readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();

        public NoContentResponse()
            : base(
                  status: StatusCodes.Status204NoContent,
                  type: "https://datatracker.ietf.org/doc/html/rfc9110#name-204-no-content",
                  title: "No Content",
                  detail: "The request was successful, but there is no content to return.",
                  instance: _httpContextAccessor.HttpContext?.Request.Path ?? "/unknown",
                  data: null)
        {
            // Abstract
        }
    }

    public class CreatedResponse : ApiResponse<object>
    {
        private static readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();

        public CreatedResponse()
            : base(
                status: StatusCodes.Status201Created,
                type: "https://datatracker.ietf.org/doc/html/rfc9110#name-201-created",
                title: "Created",
                detail: "The request was succsessful, data created.",
                instance: _httpContextAccessor.HttpContext?.Request.Path ?? "/unkown",
                data: null
            )
        {
            // Abstract
        }
    }
}
