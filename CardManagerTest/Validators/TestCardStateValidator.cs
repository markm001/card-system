using CardManager.Models;
using InventoryManager.Core.Models;
using InventoryManager.Core.Validators;

namespace CardManagerTest.Validators;

[TestClass]
public class TestCardStateValidator
{
    [TestMethod]
    public void Validate_ValidUuidForBothItemAndState()
    {
        const string expectedUuid = "UUID";
        
        IReadOnlyDictionary<string, CardState> cardStates = new Dictionary<string, CardState>
        {
            { expectedUuid, new CardState(10, 100, 2, true, true, []) }
        };
        
        var inventory = new Inventory<UniqueItem>([
            new UniqueItem(expectedUuid, new InventoryItem("ABC", false))
        ]);

        var validator = new InventoryStateValidator<CardState>();
        validator.Validate(inventory, cardStates);
    }
}