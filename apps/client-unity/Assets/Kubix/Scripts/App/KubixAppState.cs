using System;

namespace Kubix.App;

public sealed class KubixAppState
{
    public string? CurrentUserId { get; private set; }
    public string? CurrentUsername { get; private set; }
    public string? CurrentDraftMapId { get; private set; }
    public string? CurrentPublishedGameId { get; private set; }

    public event Action? StateChanged;

    public void SignIn(string userId, string username)
    {
        CurrentUserId = userId;
        CurrentUsername = username;
        NotifyChanged();
    }

    public void SignOut()
    {
        CurrentUserId = null;
        CurrentUsername = null;
        CurrentDraftMapId = null;
        CurrentPublishedGameId = null;
        NotifyChanged();
    }

    public void SelectDraftMap(string mapId)
    {
        CurrentDraftMapId = mapId;
        CurrentPublishedGameId = null;
        NotifyChanged();
    }

    public void SelectPublishedGame(string gameId)
    {
        CurrentPublishedGameId = gameId;
        CurrentDraftMapId = null;
        NotifyChanged();
    }

    private void NotifyChanged()
    {
        StateChanged?.Invoke();
    }
}
