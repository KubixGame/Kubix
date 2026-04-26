namespace Kubix.Contracts.Catalog;

public sealed record PublishedGameSummary(
    string GameId,
    string Title,
    string AuthorId,
    string AuthorName,
    string ThumbnailUrl,
    string Genre
);
