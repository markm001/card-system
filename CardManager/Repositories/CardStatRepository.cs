using CardManager.Models;
using CardManager.Models.DTO;
using CardManager.Models.Mappers;
using Utils;

namespace CardManager.Repositories;

public sealed class CardStatRepository: ICardStatRepository
{
    private readonly Dictionary<string, IReadOnlyList<StatBlock>> _stats;
    public IReadOnlyDictionary<string, IReadOnlyList<StatBlock>> Stats => _stats
        .ToDictionary(k => k.Key, v => v.Value);

    private CardStatRepository(Dictionary<string, IReadOnlyList<StatBlock>> stats)
    {
        _stats = stats.ToDictionary(c => c.Key, c => c.Value);
    }
    
    public static async Task<CardStatRepository> CreateAsync(IEnumerable<string> files)
    {
        Dictionary<string, IReadOnlyList<StatBlock>> stats = [];

        foreach (string file in files)
        {
            JsonRepository<IReadOnlyList<StatCurveData>> repository = 
                new JsonRepository<IReadOnlyList<StatCurveData>>(file, DefaultJsonOptions.Default);

            IReadOnlyList<StatCurveData> statCurveData = await repository.LoadAsync();

            var statCurves = StatCurveMapper.ToCardStats(statCurveData);
            
            foreach (var (curveId, statBlocks) in statCurves)
            {
                if (!stats.TryAdd(curveId, statBlocks))
                    throw new InvalidOperationException($"Duplicate stat curve ID '{curveId}'.");
            }
        }

        return new CardStatRepository(stats);
    }

    public StatBlock GetStats(string curveId, int level)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(curveId);
        
        if (level < 1)
            throw new ArgumentOutOfRangeException(nameof(level), "Level must be at least 1.");

        if (!_stats.TryGetValue(curveId, out var statBlocks))
            throw new KeyNotFoundException($"Stat curve '{curveId}' was not found.");

        if (level > statBlocks.Count)
            throw new ArgumentOutOfRangeException(
                nameof(level),
                level,
                $"Stat curve '{curveId}' only has {statBlocks.Count} levels.");
        
        return statBlocks[level - 1];
    }
}