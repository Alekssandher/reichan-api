using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;


namespace reichan_api.src.DTOs.Global {
    public abstract class ErrorDetails : ProblemDetails
    {

        [Description("Response status in accord to RFC 7807.")]
        public new abstract int Status { get; init; }

        [Description("Title error.")]
        public new abstract string Title { get; init; }

        [Description("Detailed error description.")]
        public new abstract string Detail { get; init; }

        // [Description("URI que referencia mais detalhes sobre o erro.")]
        // public new required string Type { get; set; }

        [Description("Error instance ID.")]
        public new abstract string Instance { get; init; }
    }

    public class InternalError : ErrorDetails {

        private static readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();

        [DefaultValue(500)]
        public override int Status { get; init; }

        [DefaultValue("Internal Error")]
        public override string Title { get; init; }

        [DefaultValue("Something went wrong at our side")]
        public override string Detail { get; init; }

        [DefaultValue("/api/endpointPath/")]
        public override string Instance { get; init; }
        public InternalError()
        {
            Status = StatusCodes.Status500InternalServerError;
            Title = "Internal error";
            Detail = "Something went wrong at our side";
            Instance = _httpContextAccessor.HttpContext?.Request.Path ?? "Unknown";
        }
    }
    public class BadRequest : ErrorDetails {

        private static readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();

        [DefaultValue(400)]
        public override int Status {get; init; }

        [DefaultValue("Bad Request")]
        public override string Title { get; init; }

        [DefaultValue("Request badly formed.")]
        public override string Detail { get; init; }

        [DefaultValue("/api/endpointPath/")]
        public override string Instance { get; init; }

        public BadRequest(string title, string detail)
        {
            Status = StatusCodes.Status400BadRequest;
            Title = title;
            Detail = detail;
            Instance = _httpContextAccessor.HttpContext?.Request.Path ?? "Unknown";
        }

    }

    public class NotFound : ErrorDetails {

        private static readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();

        [DefaultValue(404)]
        public override int Status {get; init; }

        [DefaultValue("Not Found")]
        public override string Title { get; init; }

        [DefaultValue("Couldn't find your requisition.")]
        public override string Detail { get; init; }

        [DefaultValue("/api/endpointPath/")]
        public override string Instance { get; init; }

        public NotFound(string title, string detail)
        {
            Status = StatusCodes.Status404NotFound;
            Title = title;
            Detail = detail;
            Instance = _httpContextAccessor.HttpContext?.Request.Path ?? "Unknown";
        }

    }
}
