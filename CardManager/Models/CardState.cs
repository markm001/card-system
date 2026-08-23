using InventoryManager.Core.Models;

namespace CardManager.Models;

public sealed record CardState : StateRecord
{
    public CardState(ItemState itemState, int rank, bool isFavourite)
    {
        ItemState = itemState;
        Rank = rank;
        IsFavourite = isFavourite;
    }

    public CardState(int level, int experience, int rank, bool isFavourite, IReadOnlyList<string> slots)
    {
        ItemState = new ItemState(level, experience, slots);
        Rank = rank;
        IsFavourite = isFavourite;
    }

    public ItemState ItemState { get; }
    public int Rank { get; }
    public bool IsFavourite { get; }
}

