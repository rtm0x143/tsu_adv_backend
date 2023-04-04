using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Common.Infra.Jwt;

public class JwtConfigurationProperties
{
    public required string ApplicationId { get; set; }
    public required string SigningKey { get; set; }
    public required TimeSpan LifeTime { get; set; }
    public required string[] Issuers { get; set; }
    public required string[] Audiences { get; set; }
    public required string SigningAlgorithm { get; set; }
    public required string[] ValidAlgorithms { get; set; }

    /// <summary>
    /// Creates new <see cref="JwtConfigurationProperties"/> from <see cref="IConfiguration"/>
    /// </summary>
    /// <param name="config">
    /// Required props : 
    ///     <list type="bullet">
    ///         <item> 'ApplicationId' </item> 
    ///         <item> 'AUTH_JWT_SIGNING_KEY' or 'Jwt:SigningKey'; </item>
    ///         <item> 'Jwt:LifeTime' - string with d.hh:mm:ss format </item>
    ///     </list>
    /// Other props :
    ///     <list type="bullet">
    ///         <item> 'Jwt:SigningAlgorithm' - default is <see cref="SecurityAlgorithms.HmacSha256Signature"/></item>
    ///         <item> 'Jwt:ValidAlgorithms' - semicolon separated string, default is <see cref="SigningAlgorithm"/> </item>
    ///         <item> 'Jwt:Issuers' - semicolon separated string </item>
    ///         <item> 'Jwt:Audiences' - semicolon separated string </item>
    ///     </list>
    /// </param>
    /// <exception cref="ArgumentException">When <paramref name="config"/> is invalid</exception>
    public void ReadConfiguration(IConfiguration config)
    {
        ApplicationId = config["ApplicationId"]
            ?? throw new ArgumentException("Configuration's prop 'Jwt:ApplicationId' was Null");
        SigningKey = config["AUTH_JWT_SIGNING_KEY"]
            ?? config["Jwt:SigningKey"]
            ?? throw new ArgumentException("Configuration's prop 'Jwt:SigningKey' was Null");
        LifeTime = TimeSpan.TryParse(config["Jwt:LifeTime"], out var lifeTime)
            ? lifeTime
            : throw new ArgumentException("Configuration's prop 'Jwt:LifeTime' was invalid or unspesified");
        SigningAlgorithm = config["Jwt:SigningAlgorithm"] ?? SecurityAlgorithms.HmacSha256Signature;
        Issuers = config["Jwt:Issuers"]?.Split(";") ?? Array.Empty<string>();
        Audiences = config["Jwt:Audiences"]?.Split(";") ?? Array.Empty<string>();
        ValidAlgorithms = config["Jwt:ValidAlgorithms"]?.Split(";") ?? new[] { SigningAlgorithm };
    }
}