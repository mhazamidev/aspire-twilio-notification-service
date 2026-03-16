using TwilioAspireDemo.AppHost.Constants;

namespace TwilioAspireDemo.AppHost.Extensions;

internal static class TwilioExtensions
{
    public static IResourceBuilder<ProjectResource> WithTwilio(
        this IResourceBuilder<ProjectResource> builder,
        IResourceBuilder<ParameterResource> accountSid,
        IResourceBuilder<ParameterResource> authToken,
        IResourceBuilder<ParameterResource> fromNumber,
        IResourceBuilder<ParameterResource> sendGridApiKey,
        IResourceBuilder<ParameterResource> verifyServiceSid)
    {

        builder = builder
         .WithEnvironment(Env.AccountSid, accountSid)
         .WithEnvironment(Env.AuthToken, authToken)
         .WithEnvironment(Env.SendGridApiKey, sendGridApiKey)
         .WithEnvironment(Env.VerifyServiceSid, verifyServiceSid)
         .WithEnvironment(Env.FromNumber, fromNumber);

        return builder;
    }
}
