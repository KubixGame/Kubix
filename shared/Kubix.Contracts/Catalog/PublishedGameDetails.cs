namespace Kubix.Contracts.Catalog;

public sealed record PublishedGameDetails(
    string GameId,
    string Title,
    string Description,
    string AuthorId,
    string AuthorName,
    string ThumbnailUrl,
    string Genre,
    string CurrentVersionId
);
