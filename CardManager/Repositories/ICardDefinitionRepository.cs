using CardManager.Models;

namespace CardManager.Repositories;

public interface ICardDefinitionRepository
{
    IReadOnlyDictionary<string, CardDefinition> Cards { get; }
    CardDefinition Get(string id);
}