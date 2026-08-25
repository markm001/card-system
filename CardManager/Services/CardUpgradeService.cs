using CardManager.Models;
using CardManager.Providers;
using CardManager.Repositories;
using LevelManager.Core.Models;
using LevelManager.Core.Repositories;

namespace CardManager.Services;

public class CardUpgradeService(
    ICardDefinitionRepository cardDefinitionRepository,
    ILevelCurveRepository levelCurveRepository,
    IResourceCostService resourceCostService,
    ICostCurveRepository costCurveRepository,
    ICardStateService cardStateService,
    ICardLevelService cardLevelService
)
{
    public bool LevelUp(string instanceId, int experience)
    {
        OwnedCard card = cardStateService.Get(instanceId);
        
        CardDefinition definition = cardDefinitionRepository.Get(card.CardId) 
                                    ?? throw new InvalidOperationException($"Card Definition for '{card.CardId}' was not found.");
        
        CostCurve costCurve = costCurveRepository.Get(definition.CostCurveId);
        CardCostProvider costProvider = new CardCostProvider(costCurve);
        
        IReadOnlyList<ResourceCost> cost = costProvider.GetCost(card.Progress);

        if (!resourceCostService.CanAfford(cost))
            return false;
            
        resourceCostService.Consume(cost);

        LevelCurve levelCurve = levelCurveRepository.Get(definition.LevelCurveId);

        cardLevelService.AddExperience(card, levelCurve, experience);
    
        cardStateService.Update(card);

        return true;
    }

    public CardUpgradeInfo GetCardUpgradeInfo(string instanceId)
    {
        OwnedCard card = cardStateService.Get(instanceId);


        CardDefinition definition = cardDefinitionRepository.Get(card.CardId) 
                                    ?? throw new InvalidOperationException($"Card Definition for '{card.CardId}' was not found.");
        LevelCurve levelCurve = levelCurveRepository.Get(definition.LevelCurveId);

        if(card.Progress.Level == levelCurve.MaxLevel)
            return new CardUpgradeInfo(
                card.Progress.Level,
                levelCurve.MaxLevel,
                card.Progress.Experience,
                0,
                []
            );

        CostCurve costCurve = costCurveRepository.Get(definition.CostCurveId);
        
        return new CardUpgradeInfo(
            card.Progress.Level,
            levelCurve.MaxLevel,
            card.Progress.Experience,
            levelCurve.GetExperienceRequired(card.Progress.Level),
            costCurve.GetCostRequired(card.Progress.Level)
        );
    }
}