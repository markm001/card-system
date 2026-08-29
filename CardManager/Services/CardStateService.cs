using CardManager.Models;
using InventoryManager.Core.Models;
using InventoryManager.Core.Services;
using LevelManager.Core.Models;

namespace CardManager.Services;

public sealed class CardStateService(
    UniqueInventoryService uniqueInventory,
    StateService<CardState> stateService
    ):ICardStateService
{
    public OwnedCard Get(string instanceId)
    {
        UniqueItem item = uniqueInventory.Get(instanceId) 
                          ?? throw new InvalidOperationException($"Unique item '{instanceId}' was not found.");

        CardState cardState = stateService.Get(instanceId) 
                          ?? throw new InvalidOperationException($"Card-State for item '{instanceId}' was not found.");

        LevelProgress progress = new LevelProgress(cardState.ItemState.Level, cardState.ItemState.Experience);
        
        OwnedCard card = new OwnedCard(instanceId, item.Item.Id);
        card.ApplyLevelProgress(progress);

        card.SetRank(cardState.Rank);
        card.SetFavourite(cardState.IsFavourite);
        card.SetNew(cardState.IsNew);

        return card;
    }

    public void Update(OwnedCard card)
    {
        ArgumentNullException.ThrowIfNull(card);

        CardState state = new(
            card.Progress.Level,
            card.Progress.Experience,
            card.Rank,
            card.IsFavourite,
            card.IsNew,
            card.Slots);

        stateService.Update(card.InstanceId, state);
    }

    public void Save(OwnedCard card)
    {
        ArgumentNullException.ThrowIfNull(card);
        
        CardState state = new(
            card.Progress.Level,
            card.Progress.Experience,
            card.Rank,
            card.IsFavourite,
            card.IsNew,
            card.Slots);
        
        stateService.Add(card.InstanceId, state);
    }
}