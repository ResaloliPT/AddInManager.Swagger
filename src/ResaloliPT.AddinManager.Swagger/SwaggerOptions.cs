namespace ResaloliPT.AddinManager.Swagger
{
    /// <summary>
    /// Configuration Entity.
    /// Already comes with Default values with can be overriden using configuratiion.
    /// </summary>
    /// <example>
    /// <code>
    /// "SwaggerOptions": {
    ///     "ApiName": "My Awsome API",
    ///     "AsSecurity": true
    /// }
    /// </code>
    /// </example>
    public sealed class SwaggerOptions
    {
        /// <summary>
        /// Swagger JSON endpoint.
        /// </summary>
        public string JsonRoute { get; set; } = "swagger/{documentName}/swagger.json";

        /// <summary>
        /// Api Name.
        /// </summary>
        public string ApiName { get; set; } = "Application API";

        /// <summary>
        /// Swagger UI endpoint.
        /// </summary>
        public string UIEndpoint { get; set; } = "v1/swagger.json";

        /// <summary>
        /// Defines if API has Bearer Token Autentication.
        /// </summary>
        public bool AsSecurity { get; set; } = false;
    }
}
