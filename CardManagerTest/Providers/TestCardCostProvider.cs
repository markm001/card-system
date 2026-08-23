using CardManager.Providers;
using LevelManager.Core.Models;

namespace CardManagerTest.Providers;

[TestClass]
public class TestCardCostProvider
{
    [TestMethod]
    public void GetCost_ForOneLevel_ReturnCostRequired()
    {
        ResourceCost cost = new ResourceCost("GOLD", 100);

        CostRequirement oneRequirement = new CostRequirement(1, [cost]);
        CostRequirement twoRequirement = new CostRequirement(2, [cost]);
        
        CostCurve costCurve = new CostCurve("TEST", [oneRequirement, twoRequirement]);
        CardCostProvider cardCostProvider = new CardCostProvider(costCurve);

        LevelProgress progress = new LevelProgress(1, 10);
        
        IReadOnlyList<ResourceCost> actualCost = cardCostProvider.GetCost(progress);
        
        Assert.HasCount(1, actualCost);
        Assert.AreEqual(cost, actualCost[0]);
    }
    
    [TestMethod]
    public void GetCost_ForTwoLevels_ReturnTwoMaterialsRequired()
    {
        ResourceCost shards = new ResourceCost("SHARDS", 50);
        ResourceCost gold = new ResourceCost("GOLD", 100);

        CostRequirement oneRequirement = new CostRequirement(1, [shards]);
        CostRequirement twoRequirement = new CostRequirement(2, [shards,gold]);
        
        CostCurve costCurve = new CostCurve("TEST", [oneRequirement, twoRequirement]);
        CardCostProvider cardCostProvider = new CardCostProvider(costCurve);

        LevelProgress progress = new LevelProgress(2, 20);
        
        IReadOnlyList<ResourceCost> actualCost = cardCostProvider.GetCost(progress);
        
        Assert.HasCount(2, actualCost);
        Assert.AreEqual(shards, actualCost[0]);
        Assert.AreEqual(gold, actualCost[1]);
    }
}