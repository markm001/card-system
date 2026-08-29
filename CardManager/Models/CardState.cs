using InventoryManager.Core.Models;

namespace CardManager.Models;

public sealed record CardState : StateRecord
{
    public CardState(ItemState itemState, int rank, bool isFavourite, bool isNew)
    {
        ItemState = itemState;
        Rank = rank;
        IsFavourite = isFavourite;
        IsNew = isNew;
    }

    public CardState(int level, int experience, int rank, bool isFavourite, bool isNew, IReadOnlyList<string> slots)
    {
        ItemState = new ItemState(level, experience, slots);
        Rank = rank;
        IsFavourite = isFavourite;
        IsNew = isNew;
    }

    public ItemState ItemState { get; }
    public int Rank { get; }
    public bool IsFavourite { get; }
    public bool IsNew { get; }
}

