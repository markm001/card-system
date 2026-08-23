using CardManager.Models;
using CardManager.Repositories;
using CardManager.Services;
using LevelManager.Core.Models;
using LevelManager.Core.Repositories;
using LevelManager.Core.Services;
using Moq;

namespace CardManagerTest.Services;

[TestClass]
public class TestCardLevelService
{
    [TestMethod]
    public void TestCardLevel()
    {
        // Arrange
        var cardId = "TEST";
        var instanceId = "INSTANCE_TEST";
        var card = new OwnedCard(instanceId, cardId);

        var levelCurveId = "DEFAULT_LEVEL";

        var levelCurve = new LevelCurve(levelCurveId, 5, [
            new LevelRequirement(1, 10),
            new LevelRequirement(2, 20),
            new LevelRequirement(3, 30),
            new LevelRequirement(4, 40)
        ]);
        var curveRepository = new Mock<ILevelCurveRepository>();
        curveRepository.Setup(
                x => x.Get(levelCurveId))
            .Returns(levelCurve);
        
        var levelService = new LevelService();

        // Act
        var cardLevelService = new CardLevelService(levelService);
        
        cardLevelService.AddExperience(card, levelCurve, 65);
        
        // Assert
        Assert.AreEqual(4, card.Progress.Level);
        Assert.AreEqual(5, card.Progress.Experience);
    }
}