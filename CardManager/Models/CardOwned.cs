using LevelManager.Core.Models;

namespace CardManager.Models;

public sealed record OwnedCard(string InstanceId, string CardId) : ILevelable
{
    public LevelProgress Progress { get; private set; } = new LevelProgress(1, 0);
    public int Rank { get; private set; }
    public bool IsFavourite { get; private set; }
    public bool IsNew { get; private set; }
    
    public IReadOnlyList<string> Slots { get; private set; } = [];
    
    public void ApplyLevelProgress(LevelProgress progress) => Progress = progress;
    public void SetFavourite(bool isFavourite) => IsFavourite = isFavourite;
    public void SetNew(bool isNew) => IsNew = isNew;
    public void SetRank(int rank) => Rank = rank;
}