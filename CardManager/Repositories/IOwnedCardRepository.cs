using CardManager.Models;

namespace CardManager.Repositories;

public interface IOwnedCardRepository
{
    IReadOnlyDictionary<string, OwnedCard> Cards { get; }
    OwnedCard Get(string id);
}