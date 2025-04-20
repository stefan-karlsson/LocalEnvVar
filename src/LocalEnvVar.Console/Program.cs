using LocalEnvVar.Aws.Lambda;
using LocalEnvVar.Core;

await EnvVar.MapToLocal(
   new LambdaEnvProvider("my-fancy-LambdaFunction-TYWXBqyYkkCM")
)