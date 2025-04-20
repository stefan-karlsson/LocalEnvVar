namespace LocalEnvVar.Core;

/// <summary>
/// Provides a method for mapping environment variables to the local environment.
/// </summary>
public static class EnvVar
{
    /// <summary>
    /// Maps environment variables provided by the specified providers to the local environment.
    /// </summary>
    /// <param name="providers">An array of providers that supply environment variables to be mapped.</param>
    /// <returns>A task that represents the asynchronous operation of mapping environment variables.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provider array is null.</exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the provider array is empty or contains duplicate entries.
    /// </exception>
    public static async Task MapToLocal(params IEnvVarProvider[] providers)
    {
        ValidateProviders(providers);

        var getEnvVarTasks = providers.Select(p => p.GetEnvVars());
        
        var results = await Task.WhenAll(getEnvVarTasks).ConfigureAwait(false);

        var variables = results.SelectMany(dict => dict);

        foreach (var variable in variables)
        {
            Environment.SetEnvironmentVariable(variable.Key, variable.Value);
        }
    }

    private static void ValidateProviders(IEnvVarProvider[] providers)
    {
        ArgumentNullException.ThrowIfNull(providers);

        if (providers.Length == 0)
            throw new ArgumentException("At least one provider must be specified.", nameof(providers));

        CheckForDuplicateProviders(providers);
    }

    private static void CheckForDuplicateProviders(IEnvVarProvider[] providers)
    {
        var uniqueProviders = new HashSet<IEnvVarProvider>(providers);
        
        if (uniqueProviders.Count != providers.Length)
            throw new ArgumentException("Duplicate providers are not allowed.", nameof(providers));
    }
}
