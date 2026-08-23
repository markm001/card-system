using CardManager.Models;
using LevelManager.Core.Models;

namespace CardManager.Services;

public interface ICardLevelService
{
    void AddExperience(OwnedCard card, LevelCurve levelCurve, int experience);
}