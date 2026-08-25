using CardManager.Models;
using CardManager.Repositories;
using CardManager.Services;
using LevelManager.Core.Models;
using LevelManager.Core.Repositories;
using Moq;

namespace CardManagerTest.Services;

[TestClass]
public class TestCardUpgradeService
{
    private const string InstanceId = "INSTANCE_TEST";
    private const string CardId = "TEST";
    private const string CostCurveId = "DEFAULT_COST";
    private const string LevelCurveId = "DEFAULT_LEVEL";

    private OwnedCard _ownedCard = null!;
    private LevelCurve _levelCurve = null!;

    private Mock<ICardStateService> _mockStateService = null!;
    private Mock<IResourceCostService> _mockResourceCost = null!;
    private Mock<ICardLevelService> _mockLevelService = null!;
    
    private CardUpgradeService _upgradeService = null!;

    [TestInitialize]
    public void Setup()
    {
        _ownedCard = new OwnedCard(InstanceId, CardId);
        _mockStateService = new  Mock<ICardStateService>();
        _mockStateService.Setup(x => x.Get(InstanceId))
            .Returns(_ownedCard);
        
        var cardDefinition = new CardDefinition(CardId, "Test", Rarity.R, Element.Earth, Slot.Lunar, LevelCurveId, CostCurveId);
        var mockDefinition = new Mock<ICardDefinitionRepository>();
        mockDefinition.Setup(x => x.Get(CardId))
            .Returns(cardDefinition);

        var costCurve = new CostCurve(CostCurveId, [
            new CostRequirement(1, [ new ResourceCost("GOLD", 5) ]),
            new CostRequirement(2, [ new ResourceCost("GOLD", 10), new ResourceCost("SHARD", 50) ]),
            new CostRequirement(3, [ new ResourceCost("GOLD", 20) ]),
        ]);
        var mockCostCurve = new Mock<ICostCurveRepository>();
        mockCostCurve.Setup(x => x.Get(CostCurveId))
            .Returns(costCurve);

        _mockResourceCost = new Mock<IResourceCostService>();
        
        _levelCurve = new LevelCurve(LevelCurveId, 4, [
            new LevelRequirement(1, 10),
            new LevelRequirement(2, 20),
            new LevelRequirement(3, 30)
        ]);
        var mockLevelCurve = new Mock<ILevelCurveRepository>();
        mockLevelCurve.Setup(x => x.Get(LevelCurveId))
            .Returns(_levelCurve);
        
        _mockLevelService = new Mock<ICardLevelService>();
        
        _upgradeService = new CardUpgradeService(
            mockDefinition.Object, 
            mockLevelCurve.Object, 
            _mockResourceCost.Object,
            mockCostCurve.Object,
            _mockStateService.Object,
            _mockLevelService.Object
        );

    }
    
    [TestMethod]
    public void LevelUp_CanAfford_ReturnsTrue()
    {
        // Arrange
        const int experience = 65;
        _mockResourceCost.Setup(
                x => x.CanAfford(It.IsAny<IReadOnlyList<ResourceCost>>()))
            .Returns(true);
        
        // Act
        _upgradeService.LevelUp(InstanceId, experience);
        
        // Assert
        _mockResourceCost.Verify(
            x => x.Consume(It.IsAny<IReadOnlyList<ResourceCost>>()), 
            Times.Once
        );
        
        _mockLevelService.Verify(
            x => x.AddExperience(_ownedCard, _levelCurve, experience), 
            Times.Once
        );
        
        _mockStateService.Verify(x => x.Update(_ownedCard), Times.Once);
        
        Assert.IsTrue(_upgradeService.LevelUp(InstanceId, experience));
    }
    
    [TestMethod]
    public void LevelUp_CannotAfford_ReturnsTrue()
    {
        // Arrange
        const int experience = 65;
        _mockResourceCost.Setup(
                x => x.CanAfford(It.IsAny<IReadOnlyList<ResourceCost>>()))
            .Returns(false);
        
        // Act
        _upgradeService.LevelUp(InstanceId, experience);
        
        // Assert
        _mockResourceCost.Verify(
            x => x.Consume(It.IsAny<IReadOnlyList<ResourceCost>>()), 
            Times.Never
        );
        
        _mockLevelService.Verify(
            x => x.AddExperience(_ownedCard, _levelCurve, experience), 
            Times.Never
        );
        
        _mockStateService.Verify(x => x.Update(_ownedCard), Times.Never);
        
        Assert.IsFalse(_upgradeService.LevelUp(InstanceId, experience));
    }

    [TestMethod]
    public void GetCardUpgradeInfo_CardIsMaxLevel_ReturnEmptyResourceCost()
    {
        var cardLevel = 4;
        _ownedCard.ApplyLevelProgress(new LevelProgress(cardLevel, 0));
        
        _mockStateService.Setup(x => x.Get(InstanceId))
            .Returns(_ownedCard);
        
        var actual = _upgradeService.GetCardUpgradeInfo(_ownedCard.InstanceId);
        
        Assert.AreEqual(cardLevel, actual.CurrentLevel);
        Assert.AreEqual(cardLevel, actual.MaxLevel);
        Assert.AreEqual(0, actual.CurrentExperience);
        Assert.AreEqual(0, actual.MaxExperienceForLevel);
        Assert.IsEmpty(actual.MaterialCost);
    }
    
    [TestMethod]
    public void GetCardUpgradeInfo_CardIsNotMaxLevel_ReturnResourceCost()
    {
        const int cardLevel = 2;
        const int expectedExp = 10;
        _ownedCard.ApplyLevelProgress(new LevelProgress(cardLevel, expectedExp));
        
        _mockStateService.Setup(x => x.Get(InstanceId))
            .Returns(_ownedCard);
        
        var actual = _upgradeService.GetCardUpgradeInfo(_ownedCard.InstanceId);
        
        
        Assert.AreEqual(cardLevel, actual.CurrentLevel);
        Assert.AreEqual(_levelCurve.MaxLevel, actual.MaxLevel);
        Assert.AreEqual(expectedExp, actual.CurrentExperience);
        Assert.AreEqual(_levelCurve.Requirements[1].ExperienceRequired, actual.MaxExperienceForLevel);
        
        Assert.HasCount(2, actual.MaterialCost);
        Assert.AreEqual("GOLD", actual.MaterialCost[0].ResourceId);
        Assert.AreEqual(10, actual.MaterialCost[0].Amount);
        Assert.AreEqual("SHARD", actual.MaterialCost[1].ResourceId);
        Assert.AreEqual(50, actual.MaterialCost[1].Amount);
    }
}