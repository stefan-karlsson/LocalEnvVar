using Amazon.Lambda;
using Amazon.Lambda.Model;
using JetBrains.Annotations;
using LocalEnvVar.Core;

namespace LocalEnvVar.Aws.Lambda;

public class LambdaEnvProvider(string functionName) : IEnvVarProvider
{

    [PublicAPI]
    public async Task<IEnumerable<EnvironmentVariable>> GetEnvVars()
    {
        var credentials = CredentialsHelper.GetCredentials();

        using var client = new AmazonLambdaClient(credentials);

        var request = new GetFunctionConfigurationRequest
        {
            FunctionName = functionName
        };

        var response = await client.GetFunctionConfigurationAsync(request).ConfigureAwait(false);

        return response.Environment?.Variables.Select(kv => new EnvironmentVariable(kv.Key, kv.Value)) ?? [];
    }
}
