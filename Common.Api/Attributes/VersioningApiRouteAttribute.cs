using Microsoft.AspNetCore.Mvc.Routing;
using System.Diagnostics.CodeAnalysis;

namespace Common.Api.Attributes
{
    /// <summary>
    /// Specifies '/api/v{version:apiVersion}/[controller][/...]' route
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class VersioningApiRouteAttribute : Attribute, IRouteTemplateProvider
    {
        /// <summary>
        /// Indicates should '[controller]' parameter be omitted
        /// </summary>
        public bool OmitController { get; init; } = false;

        public string? Template { get; } = "/api/v{version:apiVersion}";

        public int? Order { get; set; }

        public string? Name { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public VersioningApiRouteAttribute()
        {
            if (!OmitController) Template += "/[controller]";
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="template"> If is not absolute path appends route otherwise replaces it /></param>
        public VersioningApiRouteAttribute([StringSyntax("Route")] string template)
        {
            if (template.StartsWith('/'))
            {
                Template = template;
                return;
            }

            if (!OmitController) Template += "/[controller]";
            if (template.Length != 0) Template += $"/{template}";
        }
    }
}