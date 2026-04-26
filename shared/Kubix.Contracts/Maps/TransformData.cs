namespace Kubix.Contracts.Maps;

public sealed record TransformData(
    Vector3Data Position,
    RotationData Rotation,
    Vector3Data Scale
);
