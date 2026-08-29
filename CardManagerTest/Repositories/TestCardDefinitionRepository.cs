using CardManager.Models;
using CardManager.Repositories;

namespace CardManagerTest.Repositories;

[TestClass]
public class TestCardDefinitionRepository
{
    [TestMethod]
    public async Task Load_LoadsCardsFromFiles_ReturnsDictionaryOfCards()
    {
        string expected = "DEFAULT_ONE";
        CardDefinitionRepository repository = await CardDefinitionRepository.CreateAsync(["TestData/DefaultCardDefinitions.json"]);

        CardDefinition? actual = repository.Get(expected);
        
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.Id);
        Assert.AreEqual("Default One", actual.Name);
        Assert.AreEqual(Rarity.R, actual.Rarity);
        Assert.AreEqual(Element.Earth, actual.Element);
        Assert.AreEqual(Slot.Lunar, actual.Slot);
        Assert.AreEqual("SPRITE.png", actual.Sprite);
        Assert.AreEqual("DEFAULT_R", actual.StatCurveId);
    }
    
    [TestMethod]
    public async Task Load_EmptyPath_ThrowsException()
    {
        await Assert.ThrowsAsync<ArgumentException>(
            () => CardDefinitionRepository.CreateAsync([""])
        );
    }
    
    [TestMethod]
    public async Task Get_NonExistingCardId_ReturnNull()
    {
        CardDefinitionRepository repository = await CardDefinitionRepository.CreateAsync(["TestData/DefaultCardDefinitions.json"]);

        Assert.IsNull(repository.Get("NON_EXISTING_CARD_ID"));
    }
    
    [TestMethod]
    public async Task Get_EmptyCardId_ThrowsException()
    {
        CardDefinitionRepository repository = await CardDefinitionRepository.CreateAsync(["TestData/DefaultCardDefinitions.json"]);

        Assert.Throws<ArgumentException>(
            () => repository.Get("")
        );
    }
    
    [TestMethod]
    public async Task GetAll_ReturnDictionaryOfAllCards()
    {
        IReadOnlyList<string> expected = ["DEFAULT_ONE", "DEFAULT_TWO"];
        CardDefinitionRepository repository = await CardDefinitionRepository.CreateAsync(["TestData/DefaultCardDefinitions.json"]);

        IReadOnlyDictionary<string, CardDefinition> actual = repository.Cards;

        foreach (var card in expected)
        {
            Assert.IsNotNull(actual.GetValueOrDefault(card));
        }
    }
}