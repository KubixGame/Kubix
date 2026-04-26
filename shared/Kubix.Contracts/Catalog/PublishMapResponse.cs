namespace Kubix.Contracts.Catalog;

public sealed record PublishMapResponse(
    string GameId,
    string VersionId,
    string Message
);
