using CardManager.Models;

namespace CardManager.Services;

public interface ICardStateService
{
    OwnedCard Get(string instanceId);
    void Update(OwnedCard card);
}