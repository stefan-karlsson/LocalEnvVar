namespace LocalEnvVar.Core;

/// <summary>
/// Represents an environment variable with a key and a value.
/// </summary>
public sealed record EnvironmentVariable(string Key, string Value);
