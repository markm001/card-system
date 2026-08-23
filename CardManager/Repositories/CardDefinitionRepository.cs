using CardManager.Models;
using Utils;

namespace CardManager.Repositories;

public sealed class CardDefinitionRepository : ICardDefinitionRepository
{
    private readonly Dictionary<string, CardDefinition> _cards;
    public IReadOnlyDictionary<string, CardDefinition> Cards => _cards
        .ToDictionary(k => k.Key, v => v.Value);

    private CardDefinitionRepository(IEnumerable<CardDefinition> cards)
    {
        _cards = cards.ToDictionary(c => c.Id);
    }
    
    public static async Task<CardDefinitionRepository> CreateAsync(IEnumerable<string> files)
    {
        List<CardDefinition> cards = [];

        foreach (string file in files)
        {
            JsonRepository<IReadOnlyList<CardDefinition>> repository = 
                new JsonRepository<IReadOnlyList<CardDefinition>>(file, DefaultJsonOptions.Default);

            IReadOnlyList<CardDefinition> loadedCards = await repository.LoadAsync();

            cards.AddRange(loadedCards);
        }

        return new CardDefinitionRepository(cards);
    }

    public CardDefinition? Get(string cardId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardId);
        
        return _cards.GetValueOrDefault(cardId);
    }
}