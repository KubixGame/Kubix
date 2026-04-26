using System.Collections.Generic;

namespace Kubix.Logic;

public sealed class EasyLogicValidationResult
{
    public List<string> Errors { get; } = new();

    public bool IsValid => Errors.Count == 0;

    public void AddError(string error)
    {
        Errors.Add(error);
    }
}
