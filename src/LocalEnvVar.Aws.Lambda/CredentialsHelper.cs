using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using System.Diagnostics;
using System.Reflection;

namespace LocalEnvVar.Aws.Lambda;

internal static class CredentialsHelper
{
    public static AWSCredentials GetCredentials(string profileName = "default")
    {
        var chain = new CredentialProfileStoreChain();
        if (!chain.TryGetAWSCredentials(profileName, out var credentials))
            throw new ProcessAWSCredentialException($"Failed to find the `{profileName}` profile");

        if (credentials is not SSOAWSCredentials ssoCredentials)
            return credentials;

        ssoCredentials.Options.ClientName = Assembly.GetEntryAssembly()?.GetName().Name;

        ssoCredentials.Options.SsoVerificationCallback = args =>
        {
            var psi = new ProcessStartInfo
            {
                FileName = args.VerificationUriComplete,
                UseShellExecute = true
            };

            if (OperatingSystem.IsLinux())
            {
                psi.FileName = "xdg-open";
                psi.Arguments = args.VerificationUriComplete;
            }
            else if (OperatingSystem.IsMacOS())
            {
                psi.FileName = "open";
                psi.Arguments = args.VerificationUriComplete;
            }

            using var process = Process.Start(psi);
        };

        return credentials;
    }
}