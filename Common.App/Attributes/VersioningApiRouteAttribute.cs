using Microsoft.AspNetCore.Mvc.Routing;
using System.Diagnostics.CodeAnalysis;

namespace Common.App.Attributes
{
    /// <summary>
    /// Specifies '<see cref="RoutePrefix"/>/[controller][/...]' route
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class VersioningApiRouteAttribute : Attribute, IRouteTemplateProvider
    {
        /// <summary>
        /// Indicates should '[controller]' parameter be omitted
        /// </summary>
        public bool OmitController { get; init; }

        public const string RoutePrefix = "/api/v{version:apiVersion}";
        
        protected readonly string SpecifiedTemplate = String.Empty;

        public string Template
        {
            get
            {
                if (SpecifiedTemplate.StartsWith('/')) return SpecifiedTemplate;
                var template = RoutePrefix;

                if (!OmitController) template += "/[controller]";
                if (template.Length != 0) template += $"/{SpecifiedTemplate}";
                return template;
            }
        }

        /// <inheritdoc cref="Order"/>
        public int Order { get; init; }

        int? IRouteTemplateProvider.Order => Order;

        public string? Name { get; init; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public VersioningApiRouteAttribute()
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="template"> If is not absolute path appends route otherwise replaces it /></param>
        public VersioningApiRouteAttribute([StringSyntax("Route")] string template) => SpecifiedTemplate = template;
    }
}