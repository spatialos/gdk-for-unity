using System;
using System.IO;
using System.Linq;
using Improbable.SpatialOS.Deployment.V1Alpha1;
using Improbable.SpatialOS.Platform.Common;

namespace Improbable.Gdk.DeploymentLauncher
{
    public static class ClientFactory
    {
        public static DeploymentServiceClient Create(Options.Global options)
        {
            return string.IsNullOrEmpty(options.Environment)
                ? DeploymentServiceClient.Create()
                : DeploymentServiceClient.Create(GetEndpoint(options.Environment), GetTokenCredential(options.Environment));
        }

        private static PlatformApiEndpoint GetEndpoint(string environment)
        {
            return environment switch
            {
                "cn-production" => new PlatformApiEndpoint("platform.api.spatialoschina.com", 443),
                _ => throw new ArgumentException($"Unknown environment: {environment}", nameof(environment))
            };
        }

        private static PlatformRefreshTokenCredential GetTokenCredential(string environment)
        {
            var refreshToken = GetRefreshToken(environment);

            return environment switch
            {
                "cn-production" => new PlatformRefreshTokenCredential(refreshToken,
                    "https://auth.spatialoschina.com/auth/v1/authcode",
                    "https://auth.spatialoschina.com/auth/v1/token"),
                _ => throw new ArgumentException($"Unknown environment: {environment}", nameof(environment))
            };
        }
        
        private static string GetRefreshToken(string environment)
        {
            var possibleTokenFiles = new[]
            {
                Environment.GetEnvironmentVariable("SPATIALOS_REFRESH_TOKEN_FILE"),
                Path.Combine(Environment.GetEnvironmentVariable("HOME") ?? "", $".improbable/oauth2/oauth2_refresh_token_{environment}"),
                Path.Combine(Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%"), $".improbable/oauth2/oauth2_refresh_token_{environment}")
            };

            var tokenFile = possibleTokenFiles.FirstOrDefault(File.Exists);
            if (!string.IsNullOrEmpty(tokenFile))
            {
                try
                {
                    return File.ReadAllText(tokenFile);
                }
                catch (IOException e)
                {
                    throw new NoRefreshTokenFoundException($"Failed to read refresh token file: {tokenFile}\n{e}");
                }
            }

            // Fail if no form of credentials could be found.
            throw new NoRefreshTokenFoundException("");
        }
    }
}