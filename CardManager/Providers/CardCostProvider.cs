using LevelManager.Core.Models;
using LevelManager.Core.Providers;

namespace CardManager.Providers;

public sealed class CardCostProvider(CostCurve curve) : IProgressionCostProvider
{
    public IReadOnlyList<ResourceCost> GetCost(LevelProgress progress)
    {
        return curve.GetCostRequired(progress.Level);
    }
}