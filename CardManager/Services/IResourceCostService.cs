using LevelManager.Core.Models;

namespace CardManager.Services;

public interface IResourceCostService
{
    bool CanAfford(IReadOnlyList<ResourceCost> costs);
    void Consume(IReadOnlyList<ResourceCost> costs);
}