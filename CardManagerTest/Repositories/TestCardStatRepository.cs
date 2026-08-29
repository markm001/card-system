using CardManager.Models;
using CardManager.Repositories;

namespace CardManagerTest.Repositories;

[TestClass]
public class TestCardStatRepository
{
    public TestContext TestContext { get; set; }
    
    [TestMethod]
    public async Task Load_LoadsStatCurvesFromFiles_ReturnsDictionaryStatCurves()
    {
        const string expected = "DEFAULT_SR";
        CardStatRepository repository = await CardStatRepository.CreateAsync(["TestData/DefaultStatCurves.json"]);

        StatBlock actual = repository.GetStats(expected, 4);
        
        Assert.IsNotNull(actual);
        Assert.AreEqual(230, actual.Attack);
        Assert.AreEqual(215, actual.Defense);
        Assert.AreEqual(2300, actual.Health);
        Assert.AreEqual(113, actual.Dexterity);
    }
    
    [TestMethod]
    public async Task Load_DuplicateCurveIds_ThrowsException()
    {
        var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(directory);
        
        const string statCurveOne = "[{\"id\":\"DEFAULT_R\",\"stats\":[{\"level\":1,\"ATK\":100,\"DEF\":100,\"HP\":1000,\"DEX\":100 }]}]";
        const string statCurveTwo = "[{\"id\":\"DEFAULT_R\",\"stats\":[{\"level\":1,\"ATK\":100,\"DEF\":100,\"HP\":1000,\"DEX\":100 }]}]";

        string filepathOne = Path.Combine(directory, "one.json");
        string filepathTwo = Path.Combine(directory, "two.json");
        
        try
        {
            await File.WriteAllTextAsync(filepathOne, statCurveOne, TestContext.CancellationToken);
            await File.WriteAllTextAsync(filepathTwo,statCurveTwo, TestContext.CancellationToken);
            
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                CardStatRepository.CreateAsync([filepathOne, filepathTwo])
            );
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }
    
    [TestMethod]
    public async Task Load_CurveIdEmpty_ThrowsException()
    {
        CardStatRepository repository = await CardStatRepository.CreateAsync(["TestData/DefaultStatCurves.json"]);

        Assert.Throws<ArgumentException>(
            () => repository.GetStats("", 4)
        );
    }
    
    [TestMethod]
    public async Task Load_NonExistingStatId_ThrowsException()
    {
        CardStatRepository repository = await CardStatRepository.CreateAsync(["TestData/DefaultStatCurves.json"]);

        Assert.Throws<KeyNotFoundException>(
            () => repository.GetStats("N/A", 4)
        );
    }
    
    [TestMethod]
    public async Task Load_LevelNegative_ThrowsException()
    {
        CardStatRepository repository = await CardStatRepository.CreateAsync(["TestData/DefaultStatCurves.json"]);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => repository.GetStats("DEFAULT_SSR", -1)
        );
    }
    
    [TestMethod]
    public async Task Load_LevelOutOfBounds_ThrowsException()
    {
        CardStatRepository repository = await CardStatRepository.CreateAsync(["TestData/DefaultStatCurves.json"]);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => repository.GetStats("DEFAULT_R", 99)
        );
    }
}