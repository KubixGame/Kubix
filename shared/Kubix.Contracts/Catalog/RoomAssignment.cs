namespace Kubix.Contracts.Catalog;

public sealed record RoomAssignment(
    string RoomId,
    string Host,
    int Port,
    string GameId,
    string VersionId
);
