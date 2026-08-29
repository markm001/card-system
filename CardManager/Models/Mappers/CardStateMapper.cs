using CardManager.Models.DTO;

namespace CardManager.Models.Mappers;

public static class CardStateMapper
{
    public static Dictionary<string, CardState> ToCardState(IReadOnlyList<CardStateData> cardStateData)
    {
        return cardStateData.ToDictionary(
            i => i.InstanceId,
            i => new CardState(i.Level, i.Experience, i.Rank, i.IsFavourite, i.IsNew ,i.Slots)
        );
    }
    
    public static IReadOnlyList<CardStateData> ToCardStateData(IReadOnlyDictionary<string, CardState> states)
    {
        ArgumentNullException.ThrowIfNull(states);

        return states
            .Select(pair => new CardStateData(pair.Key, 
                pair.Value.ItemState.Level, 
                pair.Value.ItemState.Experience,
                pair.Value.Rank,
                pair.Value.IsFavourite,
                pair.Value.IsNew,
                pair.Value.ItemState.Slots))
            .ToList();
    }
}