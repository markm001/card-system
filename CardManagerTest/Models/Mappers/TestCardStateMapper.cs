using CardManager.Models;
using CardManager.Models.DTO;
using CardManager.Models.Mappers;

namespace CardManagerTest.Models.Mappers;

[TestClass]
public class TestCardStateMapper
{
    [TestMethod]
    public void ToCardState_ConvertCardStateData_ReturnsDictionary()
    {
        List<string> expectedSlots = ["SLOT_1","SLOT_2"];
        int expectedLevel = 10;
        int expectedExperience = 100;
        int expectedRank = 3;
        
        CardStateData expected = new CardStateData(
            "UUID", 
            expectedLevel, 
            expectedExperience, 
            expectedRank, 
            true, 
            expectedSlots
        );
    
        Dictionary<string, CardState> states = CardStateMapper.ToCardState([expected]);
        
        Assert.IsNotNull(states["UUID"]);
        Assert.AreEqual(expectedLevel,states["UUID"].ItemState.Level);
        Assert.AreEqual(expectedExperience,states["UUID"].ItemState.Experience);
        Assert.AreEqual(expectedRank,states["UUID"].Rank);
        Assert.IsTrue(states["UUID"].IsFavourite);
        
        CollectionAssert.AreEqual(expectedSlots, states["UUID"].ItemState.Slots.ToList());
    }

    [TestMethod]
    public void ToCardStateData_ConvertStatesDictionary_ReturnsCardStateData()
    {
        string expectedUuid = "UUID";
        int expectedLevel = 10;
        int expectedExperience = 100;
        int expectedRank = 1;
        List<string> expectedSlots = ["SLOT_1","SLOT_2"];

        Dictionary<string, CardState> states =  new Dictionary<string, CardState> {
            { expectedUuid, new CardState(expectedLevel, expectedExperience, expectedRank, false, expectedSlots) }
        };

        IReadOnlyList<CardStateData> actual = CardStateMapper.ToCardStateData(states);
        
        Assert.IsNotNull(actual);
        Assert.AreEqual(expectedUuid,actual[0].InstanceId);
        Assert.AreEqual(expectedLevel,actual[0].Level);
        Assert.AreEqual(expectedExperience,actual[0].Experience);
        Assert.AreEqual(expectedRank,actual[0].Rank);
        Assert.IsFalse(actual[0].IsFavourite);
        CollectionAssert.AreEqual(expectedSlots,actual[0].Slots.ToList());
    }

    [TestMethod]
    public void ToStateData_EmptyDictionary_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => CardStateMapper.ToCardStateData(null)
        );
    }
}