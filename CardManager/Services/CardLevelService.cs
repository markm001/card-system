using CardManager.Models;
using LevelManager.Core.Models;
using LevelManager.Core.Services;

namespace CardManager.Services;

public class CardLevelService(LevelService levelService):ICardLevelService
{
    public void AddExperience(OwnedCard card, LevelCurve levelCurve, int experience)
    {
        ArgumentNullException.ThrowIfNull(card);

        LevelProgress newProgress = levelService.AddExperience(card.Progress, levelCurve, experience);

        card.ApplyLevelProgress(newProgress);
    }
}