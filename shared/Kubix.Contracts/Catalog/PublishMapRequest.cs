using Kubix.Contracts.Maps;

namespace Kubix.Contracts.Catalog;

public sealed record PublishMapRequest(
    string Title,
    string Description,
    string Genre,
    string Visibility,
    string ThumbnailUrl,
    MapDefinition Map
);
