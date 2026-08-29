using System.Collections.Immutable;
using System.Collections.ObjectModel;
using CardManager.Models.DTO;

namespace CardManager.Models.Mappers;

public class StatCurveMapper
{
    public static Dictionary<string, ReadOnlyCollection<StatBlock>> ToCardStats(IReadOnlyList<StatCurveData> statCurveData)
    {
        var stats = statCurveData.ToDictionary(
            i => i.Id,
            i => i.Stats.Select(ToStatBlock).ToList().AsReadOnly()
        );

        var s = stats.ContainsKey("DEFAULT_SR");
        return stats;
    }
    
    private static StatBlock ToStatBlock(StatBlockData data) => new StatBlock(
        data.ATK,
        data.DEF,
        data.HP,
        data.DEX
    );
}