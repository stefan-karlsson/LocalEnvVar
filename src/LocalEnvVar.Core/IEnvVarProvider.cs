namespace LocalEnvVar.Core;

/// <summary>
/// Defines a provider for retrieving environment variables.
/// </summary>
public interface IEnvVarProvider
{
    /// <summary>
    /// Retrieves a collection of environment variables asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of environment variables.</returns>
    Task<IEnumerable<EnvironmentVariable>> GetEnvVars();
}
