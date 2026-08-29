using CardManager.Models;
using CardManager.Services;
using InventoryManager.Core.Models;
using InventoryManager.Core.Services;

namespace CardManagerTest.Services;

[TestClass]
public class TestCardStateService
{
    [TestMethod]
    public void TestCardStateService_Operations()
    {
        var itemState = new ItemState(10, 100, []);
        var expectedState = new CardState(itemState, 3, true, true);
        var expectedUuid = "UUID2";

        Dictionary<string, CardState> states = new Dictionary<string, CardState>
        {
            { "UUID1", new CardState(itemState, 1, false, true) }
        };

        var service = new StateService<CardState>(states);

        service.Add(expectedUuid, expectedState);
        Assert.AreEqual(expectedState, service.Get(expectedUuid));
        
        Assert.IsTrue(service.Remove(expectedUuid));
        Assert.IsFalse(service.Contains(expectedUuid));
        
        service.Update("UUID1", expectedState);
        Assert.AreEqual(expectedState, service.Get("UUID1"));
    }
    
    [TestMethod]
    public void Get_ReturnsOwnedCard()
    {
        var instanceId = "TEST_INSTANCE";
        var itemId = "TEST";
        
        var inventoryService = new UniqueInventoryService(
            new Inventory<UniqueItem>([
                new UniqueItem(instanceId, new InventoryItem(itemId, false))
            ])
        );

        var itemState = new ItemState(10, 100, []);
        
        var cardStateService = new StateService<CardState>(new Dictionary<string, CardState>
            {
                { instanceId, new CardState(itemState, 3, true, true)}
            }
        );

        var service = new CardStateService(inventoryService, cardStateService);

        var actual = service.Get(instanceId);
        
        Assert.IsNotNull(actual);
        Assert.AreEqual(instanceId, actual.InstanceId);
        Assert.AreEqual(itemId, actual.CardId);
        Assert.AreEqual(3, actual.Rank);
        Assert.IsTrue(actual.IsFavourite);
        Assert.IsTrue(actual.IsNew);
    } 
}