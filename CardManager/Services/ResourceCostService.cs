using InventoryManager.Core.Services;
using LevelManager.Core.Models;

namespace CardManager.Services;

public sealed class ResourceCostService(StackableInventoryService inventoryService):IResourceCostService
{
    public bool CanAfford(IReadOnlyList<ResourceCost> costs)
    {
        return costs.All(
            cost => inventoryService.Has(cost.ResourceId, cost.Amount)
        );
    }

    public void Consume(IReadOnlyList<ResourceCost> costs)
    {
        foreach (ResourceCost cost in costs)
        {
            inventoryService.Remove(cost.ResourceId, cost.Amount);
        }
    }
}